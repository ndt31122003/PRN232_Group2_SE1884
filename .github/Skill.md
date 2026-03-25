# .NET Backend Expert Profile
## Tech Stack Context
- **Framework**: .NET 8/9 ASP.NET Core Web API.
- **ORM**: Entity Framework Core (Code First).
- **Architecture**: Clean Architecture (Domain, Application, Infrastructure, WebAPI).
- **Patterns**: CQRS (with MediatR), Repository, Unit of Work.

## Coding Standards (Skills)
1. **Async/Await**: Luôn sử dụng lập trình bất đồng bộ.
2. **Result Pattern**: Không dùng Exception để điều hướng logic, hãy dùng `Result<T>` object.
3. **Validation**: Sử dụng FluentValidation cho các Command/Query.
4. **Mapping**: Sử dụng AutoMapper hoặc Mapster để chuyển đổi DTOs.
5. **Security**: Luôn check JWT Claims và phân quyền ở mức Policy-based.

## Guidelines for Agent
- Khi tạo Migration mới, luôn kiểm tra phương thức `Down()` để đảm bảo tính rollback.
- Luôn viết Unit Test bằng xUnit và Moq cho tầng Application.