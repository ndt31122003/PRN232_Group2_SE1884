# eBay Clone Platform - Workspace Instructions

**Project**: PRN232 eBay Clone - Multi-module e-commerce platform  
**Architecture**: Clean Architecture (.NET 8 backend + React frontend)  
**Language**: C# | JavaScript/React  
**Team**: 7 developers (module-based)

---

## ⚡ Quick Start

### Backend Setup
```bash
cd BE
docker-compose up -d
# API: http://localhost:8080
# Swagger: http://localhost:8080/swagger
```

### Frontend Setup
```bash
cd FE
npm install
npm start
```

### Database & Migration
```bash
cd BE/TempMigrationSeeder
dotnet run
```

---

## 📋 Project Structure

### Backend (Clean Architecture)
```
BE/PRN232_EbayClone/
├── PRN232_EbayClone.Api/          # Controllers, HTTP endpoints
├── PRN232_EbayClone.Application/   # Commands, Queries, Business Logic (MediatR)
├── PRN232_EbayClone.Domain/        # Entities, Value Objects, Domain Logic
├── PRN232_EbayClone.Infrastructure/# Persistence, Repositories, External Services
└── PRN232_EbayClone.Tests/         # Unit & Integration Tests
```

### Frontend (React)
```
FE/src/
├── pages/                  # By role: ANONYMOUS, SELLER, BUYER, ADMIN
├── components/             # Reusable UI components
├── services/               # API service clients
├── Context/                # Global state (Auth, Store, etc.)
├── utils/                  # Helpers (auth, axios, storage)
└── constants/              # App-wide constants & configs
```

---

## 🏗️ Architecture Principles

### Backend
- **Pattern**: CQRS (MediatR) + Domain-Driven Design
- **Commands**: Write operations (CreateStore, RegisterUser, etc.)
- **Queries**: Read operations (GetStoreById, GetRegistrationStatus, etc.)
- **Validation**: FluentValidation on every Command/Query
- **Error Handling**: Result<T> pattern for explicit error handling
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management (SaveChangesAsync)

### Frontend
- **Framework**: React 18+
- **State Management**: Context API + localStorage for auth persistence
- **HTTP Client**: Axios with custom retry/interceptor logic
- **Routing**: React Router v6
- **Styling**: Tailwind CSS + SCSS modules
- **Forms**: Controlled components with useReducer or useState

---

## 👥 Team Responsibilities

### 🧑‍💼 Giang - Identity & Store Management ✅
**Status**: Core features implemented

#### Identity (Authentication & Verification)
- **Files**: `Application/Identity/*`, `Api/Controllers/IdentityController`
- **Endpoints**:
  - POST `/api/identity/register` - Register with email
  - POST `/api/identity/login` - Login with credentials
  - POST `/api/identity/verify-email` - Verify email OTP
  - POST `/api/identity/set-phone` - Set phone number
  - POST `/api/identity/verify-phone` - Verify phone OTP
  - POST `/api/identity/submit-business` - Submit business verification
  - GET `/api/identity/registration-status` - Check verification progress
  - GET `/api/identity/google/login` - Google OAuth2 login

- **Key Commands/Queries**:
  - `RegisterUserCommand` - Create user account
  - `LoginCommand` - Authenticate user
  - `VerifyEmailCommand`, `VerifyPhoneCommand`, `SubmitBusinessVerificationCommand`
  - `GetRegistrationStatusQuery` - Seller verification status
  - User entity has `IsEmailVerified`, `IsPhoneVerified`, `IsBusinessVerified` flags
  - **Seller Verification Rule**: `IsSellerVerified = IsEmailVerified && IsPhoneVerified && IsBusinessVerified`

#### Store Management
- **Files**: `Application/Stores/*`, `Api/Controllers/StoresController`
- **Endpoints**:
  - POST `/api/stores` - Create store
  - GET `/api/stores/{storeId}` - Get store details
  - GET `/api/stores/my-stores` - List user's stores
  - PUT `/api/stores/{storeId}` - Update store profile

- **Key Commands/Queries**:
  - `CreateStoreCommand(name, description, storeType)` - Create store with subscription
  - `UpdateStoreProfileCommand` - Update: name, description, logo, banner, themeColor, contactEmail, contactPhone, socialLinks
  - `GetStoreByIdQuery`, `GetUserStoresQuery`
  - Store entity supports: Name, Slug, Description, LogoUrl, BannerUrl, ThemeColor, ContactEmail, ContactPhone, SocialLinks
  - **Store Types**: Basic, Premium, Anchor (with tiered subscription fees)
  - **Expansion Ready**: LayoutConfig field for future theme/layout customization

#### Frontend Pages (Giang)
- `FE/src/pages/ANONYMOUS/Register/` - Multi-step registration (email → phone → business)
- `FE/src/pages/Store/CreateStorePage.jsx` - Store creation with subscription selection
- `FE/src/pages/Store/StoreSettingsPage.jsx` - Store profile update
- `FE/src/pages/Store/MyStoresPage.jsx` - List user's stores
- `FE/src/components/Common/SellerVerificationBanner.jsx` - Seller verification status banner

#### Key Integration Points
- **Auth Context**: `FE/src/Context/AuthContext.jsx` - Manages login state & seller role switching
- **Token Storage**: JWT tokens + user info stored in localStorage
- **API Service**: `FE/src/services/IdentityService/`, `FE/src/services/StoreService/`
- **Rate Limiting**: Implemented for auth/OTP endpoints (see Infrastructure/RateLimitConfig)
- **Serilog Integration**: Error logging for debugging (configured in Program.cs)

---

### 🏪 Thịnh - Product Catalog
**Files**: `Application/Listings/*`, `FE/src/pages/SELLER/CreateListing/`
- Create/Edit/Delete product listings
- Upload images (Cloudinary integration)
- Listing templates & categories
- **Coordination**: Apply rate limiting from Hiếu's security layer

### 📦 Thành - Inventory & Performance
**Files**: `Application/Listings/Inventory/`, `Domain/Listings/Inventory/`
- Inventory management (stock tracking)
- Dashboard optimization (Redis caching for heavy queries)
- Use Dapper for raw SQL on performance-critical queries
- **Coordination**: Monitor dashboard API response times

### 📮 Hòa - Order Execution  
**Files**: `Application/Orders/*`, `Application/Checkout/*`
- Order confirmation workflow
- Shipping label generation (PuppeteerSharp for PDF)
- Order status state machine
- **Coordination**: Use Thành's inventory to update stock

### 💰 Hải - Marketing & Promotions
**Files**: `Application/Coupons/`, `Application/SaleEvents/`
- Voucher/Discount management
- Promotion rules (fixed amount / percentage)
- Assign discounts to products/stores
- **Coordination**: Backend validation in checkout (Hòa)

### 💬 Tùng - Customer Relations
**Files**: `Application/Reviews/`, `Application/Disputes/`
- Review/Rating management
- Dispute handling
- Customer feedback collection
- **Coordination**: Track disputes tied to orders (Hòa)

### 🔒 Hiếu - Analytics, Security & Logging
**Files**: `Infrastructure/RateLimitConfig/`, `Infrastructure/Realtime/`, `Application/Reports/`
- Rate limiting (IP + User-based via Redis)
- reCAPTCHA integration
- Analytics reporting (sales, order trends)
- Detailed error logging (Serilog)
- **Coordination**: Provide rate limiting for Thịnh's listing creation

---

## 🔐 Security & Rate Limiting

### Implemented
- **JWT Authentication**: Token-based auth with claims (email_verified, phone_verified, business_verified, seller_verified)
- **Rate Limiting**: Redis-based rate limiting for API endpoints
- **CORS**: Origin validation for frontend
- **Password Hashing**: BCrypt-based hashing
- **Input Validation**: FluentValidation on all commands

### Todo (Hiếu's scope)
- reCAPTCHA integration for forms
- IP-based rate limiting for spam prevention

---

## 📊 Key Patterns & Conventions

### Command/Query Pattern (CQRS)
```csharp
// Example: Create Store
public sealed record CreateStoreCommand(string Name, string? Description, StoreType StoreType) : ICommand<CreateStoreCommandResult>;

public sealed class CreateStoreCommandHandler : ICommandHandler<CreateStoreCommand, CreateStoreCommandResult>
{
    public async Task<Result<CreateStoreCommandResult>> Handle(CreateStoreCommand request, CancellationToken cancellationToken) { ... }
}

// Usage in Controller
[HttpPost]
public Task<IActionResult> CreateStore([FromBody] CreateStoreCommand command, CancellationToken cancellationToken)
    => SendAsync(command, cancellationToken);
```

### Result<T> Pattern (Error Handling)
```csharp
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public Error? Error { get; }
}

// Usage
var result = await _handler.Handle(command, cancellationToken);
if (result.IsFailure) return result.Error; // Explicit error
return result.Value; // Success
```

### Validator Pattern
```csharp
public sealed class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
{
    public CreateStoreCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.StoreType).IsInEnum();
    }
}
```

### Frontend State Management
```javascript
// Auth Context - Manages login state
const { user, isAuthenticated, loginUser, logoutUser } = useContext(AuthContext);

// Local Storage Keys
STORAGE.TOKEN              // JWT access token
STORAGE.REFRESH_TOKEN      // Refresh token
STORAGE.USER_INFO          // User object with verification flags
```

---

## 🧪 Testing & Build

### Build Backend
```bash
cd BE
dotnet build PRN232_EbayClone.sln
```

### Run Tests
```bash
cd BE
dotnet test PRN232_EbayClone.Tests/PRN232_EbayClone.Tests.csproj
```

### Run API (Docker)
```bash
docker-compose up -d
```

### Performance Testing
```bash
# JMeter test files available
BE/performance/listings_performance.jmx
```

---

## 🔗 API Response Conventions

### Success Response
```json
{
  "statusCode": 200,
  "data": { ... },
  "message": "Operation successful"
}
```

### Error Response
```json
{
  "statusCode": 400,
  "detail": "Validation error message",
  "errors": { "fieldName": ["error1", "error2"] }
}
```

---

## 📝 Git & Collaboration

### Branch Strategy
- `main` - Production ready
- `develop` - Integration branch
- `feature/{module}-{description}` - Feature branches (e.g., `feature/giang-store-settings`)

### Commit Format
- Prefix: `[Module] Description`
- Example: `[Giang] Add store profile update functionality`

### Code Review Checklist
- ✅ New Commands/Queries have validators
- ✅ Error handling uses Result<T> pattern
- ✅ All public methods are documented
- ✅ Frontend: UserContext check for auth endpoints
- ✅ Tests added for business logic

---

## 🎯 Common Development Tasks

### Adding a New Endpoint
1. Create Command/Query in `Application/{Module}/{Commands|Queries}/`
2. Create Handler implementing `ICommandHandler<>` or `IQueryHandler<>`
3. Add FluentValidator
4. Register in MediatR (auto-registered from assembly)
5. Create Controller endpoint in `Api/Controllers/{Module}Controller.cs`
6. Add corresponding frontend service in `FE/src/services/`

### Adding a New Domain Entity
1. Create entity in `Domain/{Module}/Entities/`
2. Create ID value object in `Domain/{Module}/ValueObjects/`
3. Create EF Core configuration in `Infrastructure/Persistence/Configurations/`
4. Create repository interface & implementation
5. Add migration: `dotnet ef migrations add AddNewEntity`

### Frontend Page Template
1. Create page component in `FE/src/pages/{Role}/{FeatureName}/`
2. Add service in `FE/src/services/{Module}Service/`
3. Use AuthContext for auth checks
4. Handle loading/error states with Notice notifications
5. Add route to main router

---

## 🚀 Deployment

### Production Build
```bash
# Backend Docker
docker build -t ebayclone-api .
docker run -p 8080:8080 ebayclone-api

# Frontend
npm run build
```

### Environment Configuration
- API endpoints: `appsettings.json` / `.env.production`
- Redis connection: Docker Compose or Azure Redis
- Database: SQL Server (via EF Core)
- Logging: Serilog (configured in Program.cs)

---

## ❓ FAQ & Troubleshooting

### Login Issues
- Check JWT token expiration in `Infrastructure/Identity/JwtTokenProvider.cs`
- Verify verification flags: `IsEmailVerified && IsPhoneVerified && IsBusinessVerified`
- Clear localStorage and retry

### Store Creation Fails
- Ensure user is seller verified (see SellerVerificationBanner)
- Check subscription fees in `CreateStoreCommandHandler`
- Verify StoreType enum value (0=Basic, 1=Premium, 2=Anchor)

### Docker Issues
- Ensure Redis is running: `docker ps | grep redis`
- Check connection string: `ConnectionStrings__Redis=redis:6379`
- Rebuild: `docker-compose down && docker-compose up --build`

---

## 📚 References

- **Clean Architecture**: Martin, Robert C. - *Clean Code*
- **Domain-Driven Design**: Evans, Eric - *Domain-Driven Design*
- **CQRS Pattern**: MediatR documentation
- **JWT Security**: RFC 7519
- **Fluent Validation**: GitHub FluentValidation/FluentValidation

---

**Last Updated**: March 2026  
**Maintained by**: Giang (Identity & Store), Team Lead  
**Contact**: Team repository wiki for module-specific docs
