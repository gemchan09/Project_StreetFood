🎙️ Ứng dụng Hướng dẫn Du lịch Bằng Âm thanh — Mobile App & CMS
Một hệ thống hướng dẫn du lịch bằng âm thanh thời gian thực hoàn chỉnh được xây dựng bằng .NET MAUI (Android), ASP.NET Core API, và React/Vite CMS. Tự động thuyết minh các điểm tham quan dựa trên vị trí GPS, quét mã QR, hoặc lựa chọn thручo.

✨ Tính năng chính
📍 Theo dõi GPS Thời gian thực
Cập nhật vị trí trực tiếp (dịch vụ nền trước + dịch vụ nền)
Tính toán khoảng cách Haversine để phát hiện POI chính xác
Kích hoạt geofencing — Tự động thuyết minh khi đến gần các điểm tham quan
Chống spam — Ngăn thuyết minh trùng lặp trong khoảng thời gian cooldown
🎤 Thuyết minh tự động
Âm thanh được ghi âm sẵn chuyên nghiệp với giọng nói tự nhiên
Quản lý hàng chờ âm thanh — Xử lý nhiều POI một cách thông minh
Không phát trùng lặp — Tạm dừng tự động khi có thông báo khác
Âm thanh chất lượng cao — Tối ưu hóa độ rõ ràng và tiết kiệm bandwidth
📱 Tích hợp mã QR
Phát tức thì — Quét mã QR để nghe nội dung ngay lập tức (không cần GPS)
Quét máy ảnh hoặc thư viện — Sử dụng máy ảnh thiết bị hoặc chọn ảnh hiện có
Giải mã đa nguồn — Hỗ trợ ZXing và công cụ quét mã vạch gốc
🗺️ Bản đồ tương tác
Trực quan hóa POI — Xem tất cả các điểm tham quan trên bản đồ
Vị trí người dùng thời gian thực — Theo dõi bạn và những điểm tham quan gần đó
Lựa chọn tour — Chọn các tuyến du lịch khác nhau
Hiển thị khoảng cách — Xem bạn cách mỗi POI bao xa
📊 Bảng điều khiển phân tích
Trực quan hóa bản đồ nhiệt — Top 20 vị trí được ghé thăm nhiều nhất
Thống kê sử dụng — POI được nghe nhiều nhất, thời gian nghe trung bình
Theo dõi ẩn danh — Lịch sử di chuyển để phân tích
Dữ liệu thời gian thực — Cập nhật trực tiếp từ tương tác người dùng
🎛️ Hệ thống quản lý nội dung cho quản trị viên
Quản lý POI — Tạo/chỉnh sửa/xóa các điểm tham quan với tìm kiếm & lọc
Quản lý Tour — Tổ chức các POI thành các tour theo chủ đề
Quản lý Âm thanh — Tải lên và gán các bản ghi âm được ghi sẵn
Tạo mã QR — Tạo mã QR liên kết đến nội dung cụ thể
Phân tích người dùng — Xem bản đồ nhiệt và các mẫu sử dụng
🏗️ Kiến trúc
┌──────────────┐     HTTP/REST     ┌──────────────────┐     SQLite
│  MAUI App    │ ───────────────► │  ASP.NET Core API │ ──► touristguide.db
│ (Android)    │  10.0.2.2:5000   │  (.NET 8)         │
│  .NET 9      │                  └──────────────────┘
└──────────────┘                         ▲
                                         │ HTTP
┌──────────────┐                         │
│  CMS Web     │ ────────────────────────┘
│  (React/Vite)│   localhost:5000
│  Port 5173   │
└──────────────┘
