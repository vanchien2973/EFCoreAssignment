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
- Entity Framework Core
- SQL Server
- ASP.NET Core Web API

## Cài đặt và chạy dự án

1. Clone repository from github: 
2. Cài đặt các dependencies:
   ```bash
   dotnet restore
   ```
3. Cập nhật connection string trong file `appsettings.json`
4. Chạy migrations:
   ```bash
   dotnet ef database update
   ```
5. Chạy dự án:
   ```bash
   dotnet run
   ```

## Cấu trúc database

### Departments
- Id (int, Primary Key)
- Name (nvarchar)

### Projects
- Id (int, Primary Key)
- Name (nvarchar)

### Employees
- Id (int, Primary Key)
- Name (nvarchar)
- DepartmentId (int, Foreign Key)
- JoinedDate (datetime)

## API Endpoints

(Đang cập nhật...) 