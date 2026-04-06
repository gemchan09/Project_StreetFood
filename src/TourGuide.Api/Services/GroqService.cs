using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TouristGuide.Api.Data;
using TouristGuide.Shared.DTOs;
using TouristGuide.Shared.Models;

namespace TouristGuide.Api.Services;

public class GroqService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly AppDbContext _db;
    private readonly ILogger<GroqService> _logger;

    private const string GroqEndpoint = "https://api.groq.com/openai/v1/chat/completions";

    public GroqService(IConfiguration config, AppDbContext db, ILogger<GroqService> logger)
    {
        _config = config;
        _db = db;
        _logger = logger;
        _http = new HttpClient();
        _http.Timeout = TimeSpan.FromSeconds(30);
    }

    public async Task<AiRecommendResponse> GetRecommendationAsync(AiRecommendRequest request)
    {
        try
        {
            // 1. Get all tours with POIs
            var tours = await _db.Tours
                .Where(t => t.IsActive)
                .Include(t => t.Pois.Where(p => p.IsActive))
                .ToListAsync();

            if (tours.Count == 0)
            {
                return new AiRecommendResponse
                {
                    Success = false,
                    Error = "Không có tour nào trong hệ thống"
                };
            }

            // 2. Get visit history if SessionId provided
            var visitedPois = new List<string>();
            if (!string.IsNullOrEmpty(request.SessionId))
            {
                var visitedPoiIds = await _db.NarrationLogs
                    .Where(n => n.SessionId == request.SessionId)
                    .Select(n => n.PoiId)
                    .Distinct()
                    .ToListAsync();

                visitedPois = await _db.PointsOfInterest
                    .Where(p => visitedPoiIds.Contains(p.Id))
                    .Select(p => p.Name)
                    .ToListAsync();
            }

            // 3. Build prompt
            var prompt = BuildPrompt(tours, request, visitedPois);

            // 4. Call Groq API
            var apiKey = _config["Groq:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return new AiRecommendResponse
                {
                    Success = false,
                    Error = "Groq API key chưa được cấu hình"
                };
            }

            var groqRequest = new
            {
                model = _config["Groq:Model"] ?? "llama-3.1-70b-versatile",
                messages = new[]
                {
                    new { role = "system", content = GetSystemPrompt() },
                    new { role = "user", content = prompt }
                },
                temperature = 0.7,
                max_tokens = 1000
            };

            _http.DefaultRequestHeaders.Clear();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var response = await _http.PostAsJsonAsync(GroqEndpoint, groqRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Groq API error: {Status} - {Content}", response.StatusCode, errorContent);
                return new AiRecommendResponse
                {
                    Success = false,
                    Error = $"Lỗi khi gọi AI: {response.StatusCode}"
                };
            }

            var groqResponse = await response.Content.ReadFromJsonAsync<GroqChatResponse>();
            var content = groqResponse?.Choices?.FirstOrDefault()?.Message?.Content;

            if (string.IsNullOrEmpty(content))
            {
                return new AiRecommendResponse
                {
                    Success = false,
                    Error = "AI không trả về kết quả"
                };
            }

            // 5. Parse AI response
            var parsed = ParseAiResponse(content);

            // 6. Build final response
            var recommendedTours = tours
                .Where(t => parsed.RecommendedTourIds.Contains(t.Id))
                .Select(t => new RecommendedTourInfo
                {
                    Id = t.Id,
                    Name = t.Name,
                    PoiCount = t.Pois.Count
                })
                .ToList();

            return new AiRecommendResponse
            {
                Success = true,
                RecommendedTourIds = parsed.RecommendedTourIds,
                RecommendedTours = recommendedTours,
                Explanation = parsed.Explanation,
                VisitedPois = visitedPois
            };
        }
        catch (TaskCanceledException)
        {
            return new AiRecommendResponse
            {
                Success = false,
                Error = "Timeout khi gọi AI (quá 30 giây)"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetRecommendationAsync");
            return new AiRecommendResponse
            {
                Success = false,
                Error = $"Lỗi hệ thống: {ex.Message}"
            };
        }
    }

    private string GetSystemPrompt()
    {
        return @"Bạn là chuyên gia tư vấn tour du lịch TP.HCM. 
Nhiệm vụ: Phân tích thông tin du khách và gợi ý tour phù hợp nhất.
Luôn trả lời bằng tiếng Việt.
Trả lời PHẢI theo đúng JSON format được yêu cầu.";
    }

    private string BuildPrompt(List<Tour> tours, AiRecommendRequest request, List<string> visitedPois)
    {
        var tourInfo = string.Join("\n", tours.Select(t =>
        {
            var poiNames = string.Join(", ", t.Pois.Select(p => p.Name));
            var categories = string.Join(", ", t.Pois.Select(p => p.Category).Distinct());
            return $@"- Tour #{t.Id}: {t.Name}
  Mô tả: {t.Description}
  Các điểm ({t.Pois.Count}): {poiNames}
  Thể loại: {categories}";
        }));

        var historySection = visitedPois.Count > 0
            ? $@"
**LỊCH SỬ THAM QUAN:**
- Đã nghe thuyết minh tại: {string.Join(", ", visitedPois)}
- Lưu ý: Có thể gợi ý tour khác hoặc nhắc về những điểm chưa đi trong tour đã tham quan"
            : "\n**LỊCH SỬ:** Chưa có lịch sử tham quan";

        var healthNote = request.HealthCondition switch
        {
            "limited-walking" => "⚠️ Hạn chế đi bộ nhiều - ưu tiên tour ít điểm, các điểm gần nhau",
            "wheelchair" => "⚠️ Cần xe lăn - chỉ gợi ý tour có điểm tiếp cận được",
            _ => ""
        };

        return $@"**DANH SÁCH TOUR CÓ SẴN:**
{tourInfo}

**THÔNG TIN DU KHÁCH:**
- Sở thích: {request.Interests ?? "Chưa nói rõ"}
- Ngân sách: {FormatBudget(request.Budget)}
- Tuổi: {(request.Age.HasValue ? request.Age.ToString() : "Không rõ")}
- Sức khỏe: {FormatHealth(request.HealthCondition)}
{healthNote}
{historySection}

**YÊU CẦU:**
1. Gợi ý 1-2 tour phù hợp nhất với du khách
2. Giải thích ngắn gọn (2-3 câu) tại sao tour đó phù hợp
3. Nếu có lịch sử, cân nhắc không trùng lặp

**BẮT BUỘC trả lời theo JSON format sau (không thêm text khác):**
{{
  ""recommendedTourIds"": [số ID tour],
  ""explanation"": ""Giải thích tiếng Việt tại sao gợi ý tour này""
}}";
    }

    private string FormatBudget(string? budget) => budget switch
    {
        "low" => "Thấp (tiết kiệm)",
        "medium" => "Trung bình",
        "high" => "Cao (thoải mái)",
        _ => "Không giới hạn"
    };

    private string FormatHealth(string? health) => health switch
    {
        "good" => "Tốt - có thể đi bộ nhiều",
        "normal" => "Bình thường",
        "limited-walking" => "Hạn chế đi bộ",
        "wheelchair" => "Cần hỗ trợ xe lăn",
        _ => "Bình thường"
    };

    private AiParsedRecommendation ParseAiResponse(string content)
    {
        try
        {
            // Try to extract JSON from response
            var jsonStart = content.IndexOf('{');
            var jsonEnd = content.LastIndexOf('}');
            
            if (jsonStart >= 0 && jsonEnd > jsonStart)
            {
                var json = content.Substring(jsonStart, jsonEnd - jsonStart + 1);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var parsed = JsonSerializer.Deserialize<AiParsedRecommendation>(json, options);
                
                if (parsed != null)
                    return parsed;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to parse AI response as JSON: {Content}", content);
        }

        // Fallback: return explanation as-is
        return new AiParsedRecommendation
        {
            RecommendedTourIds = new List<int> { 1 }, // Default to first tour
            Explanation = content.Length > 500 ? content[..500] : content
        };
    }
}
