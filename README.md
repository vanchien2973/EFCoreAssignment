# EFCoreAssignment

Dự án Web API sử dụng Entity Framework Core với SQL Server

## Cấu trúc dự án

Dự án được tổ chức theo kiến trúc Clean Architecture với các layer sau:

- **EFCoreAssignment.API**: Layer chứa các controllers và cấu hình API
- **EFCoreAssignment.Application**: Layer chứa business logic và các services
- **EFCoreAssignment.Domain**: Layer chứa các entities và interfaces
- **EFCoreAssignment.Persistence**: Layer chứa các repositories và cấu hình database

## Công nghệ sử dụng

- .NET 9.0
- Entity Framework Core 9.0.4
- SQL Server
- ASP.NET Core Web API
- Swagger/OpenAPI 8.1.0

## Yêu cầu hệ thống

- .NET 9.0 SDK
- SQL Server (local hoặc remote)
- Visual Studio 2022 hoặc JetBrains Rider

## Cài đặt và chạy dự án

1. Clone repository từ GitHub:
   ```bash
   git clone https://github.com/vanchien2973/EFCoreAssignment.git
   cd EFCoreAssignment
   ```

2. Cài đặt các dependencies:
   ```bash
   dotnet restore
   ```

3. Cấu hình database:
   - Mở file `EFCoreAssignment.API/appsettings.json`
   - Cập nhật connection string trong phần `ConnectionStrings`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=EFCoreAssignment;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

4. Tạo và cập nhật database:
   ```bash
   cd EFCoreAssignment.API
   dotnet ef database update --startup-project ../EFCoreAssignment.API
   ```

5. Chạy dự án:
   ```bash
   dotnet run
   ```

6. Truy cập Swagger UI:
   - Mở trình duyệt và truy cập: `http://localhost:5292/swagger` hoặc `http://localhost:7097/swagger`

## Ghi chú

- Đảm bảo SQL Server đã được cài đặt và chạy trước khi thực hiện migrations
- Nếu gặp lỗi SSL certificate, có thể thêm `TrustServerCertificate=True` vào connection string
<<<<<<< HEAD
- Để tạo migration mới, sử dụng lệnh: `dotnet ef migrations add [MigrationName]` 
=======
- Để tạo migration mới, sử dụng lệnh: `dotnet ef migrations add [MigrationName]` 
>>>>>>> origin/master
