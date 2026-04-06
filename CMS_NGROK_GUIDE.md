
# 🌐 Hướng Dẫn Sử Dụng CMS Qua Ngrok

## Vấn Đề
Khi truy cập CMS qua ngrok (VD: https://abc.ngrok-free.app), browser chặn request đến `localhost:5000` (API) do CORS policy: "loopback address space".

## Giải Pháp: Expose CẢ CMS VÀ API Qua Ngrok

### Bước 1: Chạy API (Terminal 1)
```powershell
cd d:\An\tourist\src\TouristGuide.Api
dotnet run
```
API chạy trên http://localhost:5000

### Bước 2: Expose API Qua Ngrok (Terminal 2)
```powershell
ngrok http 5000
```
**Output:**
```
Forwarding    https://1234abcd.ngrok-free.app -> http://localhost:5000
```
➡️ **Copy URL này:** `https://1234abcd.ngrok-free.app` (không có dấu `/` cuối)

### Bước 3: Cấu Hình CMS Để Dùng API Ngrok
**File:** `d:\An\tourist\src\cms\.env`

```env
VITE_API_URL=https://1234abcd.ngrok-free.app
```
(Thay `1234abcd` bằng ngrok URL thực tế của bạn)

### Bước 4: Khởi Động CMS (Terminal 3)
```powershell
cd d:\An\tourist\src\cms
npm run dev
```
CMS chạy trên http://localhost:5173

### Bước 5: Expose CMS Qua Ngrok (Terminal 4)
```powershell
ngrok http 5173
```
**Output:**
```
Forwarding    https://xyz789.ngrok-free.app -> http://localhost:5173
```
➡️ **Mở URL này trên browser:** `https://xyz789.ngrok-free.app`

### Bước 6: Bypass Ngrok Warning Page
1. Truy cập URL CMS ngrok (VD: https://xyz789.ngrok-free.app)
2. Click button **"Visit Site"** 
3. CMS sẽ tải và gọi API qua ngrok URL (https://1234abcd.ngrok-free.app)

---

## Tóm Tắt 4 Terminals

| Terminal | Lệnh | Mô Tả |
|----------|------|-------|
| 1 | `cd d:\An\tourist\src\TouristGuide.Api && dotnet run` | API :5000 |
| 2 | `ngrok http 5000` | API ngrok |
| 3 | `cd d:\An\tourist\src\cms && npm run dev` | CMS :5173 |
| 4 | `ngrok http 5173` | CMS ngrok |

---

## Các Tùy Chọn Khác

### Option A: Dùng IP LAN (Không Cần Ngrok API)
**Ưu điểm:** Chỉ cần 1 ngrok tunnel (cho CMS)

1. Kiểm tra IP máy chủ:
```powershell
ipconfig
# Tìm IPv4 Address (VD: 192.168.2.152)
```

2. Cập nhật `.env`:
```env
VITE_API_URL=http://192.168.2.152:5000
```

3. Người dùng cùng mạng LAN có thể truy cập:
   - CMS: https://xyz789.ngrok-free.app (qua ngrok)
   - API: http://192.168.2.152:5000 (qua LAN)

**Hạn chế:** Chỉ hoạt động khi browser truy cập CMS từ cùng mạng LAN.

### Option B: Dev Local (Không Ngrok)
**File `.env`:**
```env
VITE_API_URL=http://localhost:5000
```

Truy cập: http://localhost:5173 (không qua ngrok)

---

## Troubleshooting

### ❌ Lỗi: `ERR_FAILED` hoặc `net::ERR_BLOCKED_BY_RESPONSE`
**Nguyên nhân:** Chưa bypass ngrok warning page  
**Giải pháp:** Click "Visit Site" trên warning page

### ❌ Lỗi: `CORS policy: loopback address`
**Nguyên nhân:** CMS qua ngrok đang gọi localhost API  
**Giải pháp:** Expose API qua ngrok và update `.env`

### ❌ Lỗi: `404 Not Found` khi gọi API
**Nguyên nhân:** Sai VITE_API_URL trong .env  
**Check:**
```powershell
# Mở browser, truy cập:
https://YOUR-API-NGROK-URL/api/pois
# Phải thấy JSON data
```

### ❌ Ngrok URL thay đổi mỗi lần khởi động
**Nguyên nhân:** Ngrok free tier random URL  
**Giải pháp:** 
1. Upgrade ngrok (paid) để có static domain
2. Hoặc update `.env` mỗi lần restart ngrok

---

## Debug Tips

### Kiểm Tra API Ngrok
```powershell
# Mở browser → Developer Tools → Console
fetch('https://YOUR-API-NGROK-URL/api/pois')
  .then(r => r.json())
  .then(console.log)
```

### Xem Ngrok Dashboard
Mở: http://localhost:4040 để xem:
- Requests history
- Request/response headers
- Errors

### Kiểm Tra Env Variable
Mở CMS trong browser → Console:
```javascript
console.log(import.meta.env.VITE_API_URL)
// Phải in ra ngrok URL, KHÔNG phải localhost:5000
```

---

## Lưu Ý Quan Trọng

1. **Restart CMS sau khi sửa `.env`:**
   ```powershell
   # Ctrl+C để dừng npm run dev
   npm run dev
   ```

2. **Mỗi lần restart ngrok API, cần update `.env` với URL mới**

3. **Ngrok free tier có limit:**
   - 1 ngrok process = 1 tunnel
   - Cần 2 terminals cho 2 tunnels (API + CMS)

4. **Security:** Ngrok URL công khai, ai có link đều truy cập được. Tắt ngrok khi không dùng.
