# Developer Guide - eBay Clone Platform

Quick reference for daily development workflow.

---

## 🚀 Local Development Setup

### Frontend Quick Start
```bash
cd FE
npm install
npm start
# Runs on http://localhost:3000
```

### Backend Quick Start
```bash
cd BE
docker-compose up -d
# API on http://localhost:8080
# Swagger on http://localhost:8080/swagger
# Redis on localhost:6379
```

### Database Setup
```bash
cd BE/TempMigrationSeeder
dotnet run
# Seeds initial data including categories, conditions, policies, etc.
```

---

## 📝 Common Commands

### Frontend Development
```bash
# Install dependencies
npm install

# Start dev server (with hot reload)
npm start

# Build for production
npm run build

# Run tests
npm test

# Lint & format check
npm run lint

# Docker build for frontend
docker build -t ebayclone-ui -f nginx/Dockerfile .
```

### Backend Development
```bash
# Build solution
cd BE
dotnet build PRN232_EbayClone.sln

# Run tests
dotnet test PRN232_EbayClone.Tests/PRN232_EbayClone.Tests.csproj

# View available migrations
dotnet ef migrations list

# Create new migration
dotnet ef migrations add MigrationName \
  -p PRN232_EbayClone.Infrastructure \
  -s PRN232_EbayClone.Api

# View Swagger docs
GET http://localhost:8080/swagger

# Check API health
GET http://localhost:8080/
```

### Docker Commands
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop all services
docker-compose down

# Rebuild images
docker-compose up --build

# Remove volumes (clean database)
docker-compose down -v
```

---

## 🔧 Development Workflow by Role

### 👤 Giang - Identity & Store Management

#### Adding a new verification step
1. Create Command in `Application/Identity/Commands/NewVerificationCommand.cs`
2. Add validator: `NewVerificationCommandValidator`
3. Create handler: `NewVerificationCommandHandler`
4. Add endpoint to `Api/Controllers/IdentityController.cs`
5. Create frontend service method
6. Add React component in `pages/ANONYMOUS/Register/`

#### Updating Store Profile
1. Modify `Domain/Stores/Entities/Store.cs` UpdateProfile method if adding fields
2. Update `Application/Stores/Commands/UpdateStoreProfileCommand.cs`
3. Update validator & handler
4. Add schema to `Api/Controllers/StoresController.cs` UpdateStoreProfileRequest
5. Update `FE/src/pages/Store/StoreSettingsPage.jsx` form

---

### 🏪 Thịnh - Product Catalog

#### Creating a new listing
1. Create `CreateListingCommand` in `Application/Listings/Commands/`
2. Verify seller is verified (check within handler)
3. Create command handler with Cloudinary upload
4. Add controller endpoint to `Api/Controllers/ListingsController.cs`
5. Frontend: Use `components/Listing/CreateListingForm.jsx`

**Important**: 
- Check Rate Limiting from Hiếu before listing creation
- Use inventory reservation API to check stock
- Upload images to Cloudinary (API key in appsettings)

---

### 📦 Thành - Inventory & Performance

#### Dashboard Query Optimization
1. Use Dapper for complex queries (see `Infrastructure/Persistence/Dapper/`)
2. Cache with Redis via `Infrastructure/Caching/`
3. Create `GetSellerDashboardQuery` returning cached results
4. Invalidate cache on inventory changes (Order creation/cancellation)

**Performance Guidelines**:
- EF Core for single aggregates
- Dapper for reporting/dashboard queries
- Redis for frequently accessed data
- Monitor query execution time in Serilog

---

### 📮 Hòa - Order Execution

#### Order Creation Workflow
1. Create `CreateOrderCommand` with order items
2. Validate seller eligibility & inventory
3. Generate tracking number & shipping info
4. Create `GenerateShippingLabelCommand` using PuppeteerSharp
5. Emit order created event
6. Reserve inventory (coordinate with Thành)

**Integration Points**:
- Use `Hải.GetApplicableCouponsQuery` to calculate discount
- Update `Thành.InventoryService.ReserveStock()`
- Notify `Tùng` for dispute tracking

---

### 💰 Hải - Marketing & Promotions

#### Creating Promotions
1. Create `CreateCouponCommand` with:
   - Discount type (Fixed / Percentage)
   - Min purchase / max discount
   - Date range
   - Applicable stores/categories
2. Create `ValidateCouponQuery` for checkout validation
3. Add coupon application logic to `Hòa.CheckoutService`

**Integration Points**:
- Used by `Hòa` during order creation
- Tracked by `Tùng` for dispute resolution
- Reported by `Hiếu` in analytics

---

### 💬 Tùng - Customer Relations

#### Review Management
1. Create `CreateReviewCommand` after order delivery
2. Seller can respond via `RespondToReviewCommand`
3. Query: `GetProductReviewsQuery` with pagination
4. Report: `GetReviewsAnalyticsQuery`

#### Dispute Management
1. `CreateDisputeCommand` with reason & evidence
2. Track dispute resolution via state machine
3. Query: `GetSellerDisputesQuery`
4. Coordinate with `Hòa` for order context

---

### 🔒 Hiếu - Analytics, Security & Logging

#### Rate Limiting Setup
```csharp
// In Program.cs
services.AddRateLimiting(builder =>
{
    builder.AddPolicy("register", policy =>
        policy.RateLimitByIp(10, TimeSpan.FromMinutes(15)));
});

// On endpoint
[RateLimitPolicy("register")]
public async Task<IActionResult> Register(RegisterCommand cmd)
```

#### Analytics Query
1. Create `GetSalesMetricsQuery` (daily/weekly/monthly)
2. Cache with Redis for performance
3. Use Dapper for aggregation queries
4. Return `SalesMetricsDto` with trends

#### Logging Best Practices
```csharp
// Serilog is configured in Program.cs
_logger.LogInformation("Store {StoreId} created by {UserId}", storeId, userId);
_logger.LogError("Login failed for {Email}", email);

// Access logs via Docker
docker-compose logs -f api | grep "ERROR"
```

---

## 🐛 Debugging Tips

### Frontend Issues
```javascript
// Check localStorage
localStorage.getItem('token')
localStorage.getItem('user_info')

// AuthContext state
console.log(useContext(AuthContext))

// Axios interceptors
// Check FE/src/axios/axiosCustomize.js
```

### Backend Issues
```bash
# View Serilog output
docker-compose logs -f api

# Check JWT claims
# Decode token at jwt.io and verify claims

# Verify database state
# Connect with SQL Server Management Studio
# Connection: server=localhost
```

### Docker Issues
```bash
# Check if Redis is running
docker ps | grep redis

# Check Redis connection
docker exec redis redis-cli ping

# View API startup logs
docker-compose logs api | head -50

# Rebuild without cache
docker-compose down -v && docker-compose up --build
```

---

## 📊 Testing Workflow

### Unit Tests
```bash
cd BE
# Test specific project
dotnet test \
  PRN232_EbayClone.Tests/PRN232_EbayClone.Tests.csproj \
  --verbosity detailed

# Run specific test
dotnet test \
  --filter FullyQualifiedName~CreateStoreCommandHandlerTests

# Code coverage
dotnet test /p:CollectCoverage=true
```

### Frontend Tests
```bash
cd FE

# Run all tests
npm test

# Run with coverage
npm test -- --coverage

# Run specific test file
npm test -- Register.test.js

# Watch mode
npm test -- --watch
```

### Manual Testing Checklist

#### Registration Flow
- [ ] Register with valid email
- [ ] Verify email with OTP
- [ ] Set phone number
- [ ] Verify phone with OTP
- [ ] Submit business info
- [ ] Check registration-status endpoint
- [ ] Create store after verification
- [ ] Update store profile

#### Store Management
- [ ] Create store with Basic plan
- [ ] Create store with Premium plan
- [ ] Update store name
- [ ] Upload logo
- [ ] Upload banner
- [ ] Add theme color
- [ ] Add contact info
- [ ] Add social links
- [ ] View my stores list

---

## 🔗 Useful Links

### Documentation
- Swagger UI: http://localhost:8080/swagger
- MediatR Docs: https://github.com/jbogard/MediatR
- FluentValidation: https://docs.fluentvalidation.net/
- Entity Framework Core: https://learn.microsoft.com/en-us/ef/core/

### Team Resources
- Project Wiki: Check team repository
- Design Files: Ask Giang for Figma links
- Database Schema: See Infrastructure/Persistence/Configurations/

### External Services
- Cloudinary API: Check appsettings.json for AppUrl
- Email Service: Setup in Infrastructure/Email/
- SMS Service: Ready for Twilio integration

---

## ⚠️ Important Notes

### Code Style
- Use PascalCase for public properties
- Use camelCase for private fields
- Use meaningful names (avoid `data`, `result`, `temp`)
- Max 100 lines per method
- XML comments on public APIs

### Performance
- Index frequently queried columns (UserId, StoreId, etc.)
- Use async/await throughout
- Cache expensive queries in Redis
- Monitor N+1 query problems with Entity Framework profiler

### Security
- Never log passwords or sensitive data
- Validate all input on backend
- Use parameterized queries (EF Core default)
- Check user authorization in handlers
- Verify sellers for sensitive operations

### Git Workflow
```bash
# Create feature branch
git checkout -b feature/giang-new-feature

# Work on feature
git add .
git commit -m "[Giang] Add new feature"

# Push and create PR
git push origin feature/giang-new-feature

# After merge, pull develop
git checkout develop
git pull origin develop
```

---

## 📞 Quick Troubleshooting

| Issue | Solution |
|-------|----------|
| Port 8080 already in use | `lsof -i :8080` and kill process, or `docker-compose down` |
| Redis connection refused | `docker ps \| grep redis` to verify, restart with `docker-compose restart redis` |
| Migration failed | Drop database and rerun seeder, or use `dotnet ef database drop` |
| CORS errors in frontend | Check `appsettings.json` Cors:AllowedOrigins |
| Token expired constantly | Check JWT expiration in `JwtTokenProvider.cs` |
| Seller verification not working | Verify all three flags (email, phone, business) in user entity |

---

## 🎓 Getting Help

1. Check this guide first
2. Review existing code in same module
3. Ask module lead (e.g., Giang for Identity/Store)
4. Check Serilog logs for backend errors
5. Check browser DevTools for frontend errors
6. Post in team chat with error details

---

**Last Updated**: March 2026  
**Maintained by**: Team Leads  
**Report Issues**: Team repository issues tab
