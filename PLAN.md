# 📋 Tourist Audio Guide - Kế hoạch Implement

## Trạng thái hiện tại ✅
- API backend (ASP.NET Core + SQLite) đang chạy, có seed 6 POI Quận 1
- MAUI app Android chạy được: map OpenStreetMap, GPS tracking, TTS thuyết minh, QR demo (prompt)
- Database: SQLite (touristguide.db) - **dữ liệu được lưu trên file, tồn tại khi restart**

## Database choice: **SQLite** 
- API side: EF Core + SQLite → file `touristguide.db` giữ lại data khi restart
- MAUI side: sqlite-net-pcl → cache POI offline trên điện thoại  
- Phù hợp dự án sinh viên, không cần cài server DB riêng

---

## Phase 1: QR Code Scanner (Chức năng CHÍNH) 🔴
**Mục tiêu**: Quét QR thật bằng camera → phát thuyết minh ngay

### Tasks:
1. Thêm package `ZXing.Net.Maui.Controls` hoặc dùng camera intent
2. Tạo trang QR scan thật (thay thế prompt demo hiện tại)
3. QR format: `poi:{ID}` → tìm POI → phát TTS ngay
4. Tạo API endpoint generate QR image cho mỗi POI  
5. Hiển thị QR trên CMS để in ra dán tại địa điểm

**Estimate**: 2-3 giờ

---

## Phase 2: GPS + TTS Narration (Chức năng phụ) 🟡
**Mục tiêu**: Hoàn thiện auto narration khi đi ngang POI

### Tasks:
1. Audio queue - chống phát trùng, cooldown 5 phút (đã có)
2. Cải thiện TTS: thêm nút pause/resume
3. Hiển thị khoảng cách đến POI gần nhất trên map
4. Lưu narration log vào API (đã có)

**Estimate**: 1-2 giờ

---

## Phase 3: CMS Web Admin (React) 🟡
**Mục tiêu**: Trang web quản lý POI, Tour, xem analytics

### Tasks:
1. Setup React project (Vite + React)
2. Trang quản lý POI: CRUD + chọn vị trí trên map (Leaflet)
3. Trang quản lý Tour: gắn POI vào tour
4. Trang QR Generator: tạo & in QR code cho mỗi POI
5. Trang Analytics: top POI, heatmap, lịch sử

**Estimate**: 3-4 giờ

---

## Phase 4: Analytics 🟢
**Mục tiêu**: Thống kê sử dụng

### Tasks:
1. API endpoint đã có (dashboard, heatmap)
2. Hiển thị trên CMS: biểu đồ top POI, heatmap bản đồ
3. Lịch sử sử dụng theo session

**Estimate**: 1-2 giờ

---

## Thứ tự implement:
```
Phase 1 (QR) → Phase 2 (GPS+TTS) → Phase 3 (CMS) → Phase 4 (Analytics)
```

## Stack tổng:
| Component | Tech |
|-----------|------|
| Mobile App | .NET MAUI (Android) |
| Backend API | ASP.NET Core 8 |
| Database | SQLite (file-based, persist data) |
| Map | OpenStreetMap + Leaflet.js |
| CMS Web | React + Vite |
| TTS | MAUI Essentials TextToSpeech |
| QR Scan | ZXing hoặc Camera Intent |
