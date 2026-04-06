# 🎙️ Ứng dụng Hướng dẫn Du lịch Bằng Âm thanh — Mobile App & CMS

Một hệ thống **hướng dẫn du lịch bằng âm thanh thời gian thực** hoàn chỉnh được xây dựng bằng **.NET MAUI** (Android), **ASP.NET Core API**, và **React/Vite CMS**. Tự động thuyết minh các điểm tham quan dựa trên vị trí GPS, quét mã QR, hoặc lựa chọn thручo.

## ✨ Tính năng chính

### 📍 Theo dõi GPS Thời gian thực
- **Cập nhật vị trí trực tiếp** (dịch vụ nền trước + dịch vụ nền)
- **Tính toán khoảng cách Haversine** để phát hiện POI chính xác
- **Kích hoạt geofencing** — Tự động thuyết minh khi đến gần các điểm tham quan
- **Chống spam** — Ngăn thuyết minh trùng lặp trong khoảng thời gian cooldown

### 🎤 Thuyết minh tự động
- **Âm thanh được ghi âm sẵn chuyên nghiệp** với giọng nói tự nhiên
- **Quản lý hàng chờ âm thanh** — Xử lý nhiều POI một cách thông minh
- **Không phát trùng lặp** — Tạm dừng tự động khi có thông báo khác
- **Âm thanh chất lượng cao** — Tối ưu hóa độ rõ ràng và tiết kiệm bandwidth

### 📱 Tích hợp mã QR
- **Phát tức thì** — Quét mã QR để nghe nội dung ngay lập tức (không cần GPS)
- **Quét máy ảnh hoặc thư viện** — Sử dụng máy ảnh thiết bị hoặc chọn ảnh hiện có
- **Giải mã đa nguồn** — Hỗ trợ ZXing và công cụ quét mã vạch gốc

### 🗺️ Bản đồ tương tác
- **Trực quan hóa POI** — Xem tất cả các điểm tham quan trên bản đồ
- **Vị trí người dùng thời gian thực** — Theo dõi bạn và những điểm tham quan gần đó
- **Lựa chọn tour** — Chọn các tuyến du lịch khác nhau
- **Hiển thị khoảng cách** — Xem bạn cách mỗi POI bao xa

### 📊 Bảng điều khiển phân tích
- **Trực quan hóa bản đồ nhiệt** — Top 20 vị trí được ghé thăm nhiều nhất
- **Thống kê sử dụng** — POI được nghe nhiều nhất, thời gian nghe trung bình
- **Theo dõi ẩn danh** — Lịch sử di chuyển để phân tích
- **Dữ liệu thời gian thực** — Cập nhật trực tiếp từ tương tác người dùng

### 🎛️ Hệ thống quản lý nội dung cho quản trị viên
- **Quản lý POI** — Tạo/chỉnh sửa/xóa các điểm tham quan với tìm kiếm & lọc
- **Quản lý Tour** — Tổ chức các POI thành các tour theo chủ đề
- **Quản lý Âm thanh** — Tải lên và gán các bản ghi âm được ghi sẵn
- **Tạo mã QR** — Tạo mã QR liên kết đến nội dung cụ thể
- **Phân tích người dùng** — Xem bản đồ nhiệt và các mẫu sử dụng

## 🏗️ Kiến trúc

```
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
```

## 🛠️ Công nghệ sử dụng

### Ứng dụng Mobile (Android)
- **.NET MAUI 9.0** — Framework mobile đa nền tảng
- **Microsoft.Maui.Controls.Maps** — Trực quan hóa bản đồ
- **ZXing.Net.Bindings.SkiaSharp** — Giải mã mã QR
- **BarcodeScanning.Native.Maui** — Quét mã QR dựa trên máy ảnh
- **SQLite-net** — Cơ sở dữ liệu cục bộ để hỗ trợ ngoại tuyến
- **CommunityToolkit.MVVM** — Triển khai mẫu MVVM

### Máy chủ API
- **ASP.NET Core 8.0** — Back-end RESTful (tránh lỗi CLR .NET 9 trên một số CPU Intel cũ)
- **Entity Framework Core** — ORM cho hoạt động cơ sở dữ liệu
- **SQLite** — Lưu trữ bền vững nhẹ
- **Swagger/OpenAPI** — Tài liệu API tự động

### CMS Web
- **React 18** — Framework giao diện người dùng
- **Vite** — Công cụ xây dựng nhanh và máy chủ phát triển
- **Axios** — HTTP Client cho các cuộc gọi API
- **Recharts** — Trực quan hóa dữ liệu & bản đồ nhiệt
- **Tailwind CSS** — Tạo kiểu dáng

### Triển khai
- **Ngrok** — Đường hầm an toàn để truy cập bên ngoài (tùy chọn)


## 📡 Các điểm cuối API

| Phương thức | Điểm cuối | Mục đích |
|--------|----------|---------|
| GET | `/api/pois` | Lấy tất cả điểm tham quan |
| GET | `/api/pois/{id}` | Lấy POI cụ thể |
| GET | `/api/tours` | Lấy tất cả tour |
| GET | `/api/tours/{id}` | Lấy tour cụ thể với POI |
| POST | `/api/narration-logs` | Ghi nhật ký phát thuyết minh |
| POST | `/api/location-logs` | Ghi nhật ký vị trí người dùng |
| GET | `/api/analytics/dashboard` | Lấy tóm tắt phân tích |
| GET | `/api/analytics/heatmapData` | Lấy tọa độ bản đồ nhiệt |

## 🎯 Các trang & thành phần chính

### Ứng dụng Mobile (MAUI)
- **TourSelectionPage** — Chọn tour hoặc xem tất cả POI
- **MainPage (Bản đồ)** — Bản đồ tương tác với theo dõi GPS
- **QrScanPage** — Quét mã QR dựa trên máy ảnh
- **ImageViewerPage** — Xem ảnh POI toàn màn hình
- **SearchPage** — Tìm POI với các bộ lọc

### CMS (React)
- **Bảng điều khiển** — Tổng quan phân tích với bản đồ nhiệt
- **Quản lý POI** — Hoạt động CRUD cho các điểm tham quan
- **Quản lý Tour** — Tạo & tổ chức các tour
- **Tạo mã QR** — Tạo mã QR cho POI

## 🔧 Cấu hình

### Cài đặt ứng dụng (API)
- Kết nối cơ sở dữ liệu: `appsettings.json`
- Cổng mặc định: `5000`
- Dữ liệu seed tạo 6 POI ở Quận 1, TP.HCM

### Cấu hình Emulator
Để hỗ trợ âm thanh, đảm bảo `~/.android/avd/Pixel_5.avd/config.ini` chứa:
```ini
hw.audioInput=yes
hw.audioOutput=yes
```

### Môi trường CMS (`.env`)
```
VITE_API_URL=http://localhost:5000
```

Để truy cập Ngrok, cập nhật URL đường hầm Ngrok.

## 🌐 Truy cập từ xa (Ngrok)

Chia sẻ ứng dụng & CMS qua internet mà không cần chuyển tiếp cổng:

```bash
# Terminal 1: CMS trên cổng 5173
ngrok http 5173
# → Nhận URL công khai như https://abc123.ngrok.io

# Terminal 2: API trên cổng 5000
ngrok http 5000
# → Nhận URL công khai như https://xyz789.ngrok.io
```

Cập nhật CMS `.env` với URL API Ngrok. Xem [CMS_NGROK_GUIDE.md](CMS_NGROK_GUIDE.md) để biết chi tiết.

## 📊 Lược đồ cơ sở dữ liệu

**PointsOfInterest** (Các điểm tham quan)
- `id`, `name`, `latitude`, `longitude`, `radius`, `description`, `narrative`, `imageUrl`, `tourId`

**Tours** (Các tour)
- `id`, `name`, `description`, `isActive`

**NarrationLogs** (Nhật ký thuyết minh)
- `id`, `poiId`, `sessionId`, `timestamp`, `duration`

**LocationLogs** (Nhật ký vị trí)
- `id`, `latitude`, `longitude`, `sessionId`, `timestamp` (theo dõi ẩn danh)

