# Project Architecture: eBay Clone

## System Flow (MediatR)
1. **API Layer**: Controllers receive requests -> Send Command/Query to MediatR.
2. **Application Layer**: Handlers process logic -> Use Domain Entities -> Call Repositories.
3. **Infrastructure Layer**: EF Core implementation -> Supabase (Postgres).
4. **Domain Layer**: Pure logic, Entities, and Value Objects.

## Directory Map
- `BE/PRN232_EbayClone.Api/`: Endpoints & Controllers.
- `BE/PRN232_EbayClone.Application/`: MediatR Commands, Queries, Handlers.
- `BE/PRN232_EbayClone.Domain/`: Core Entities (User, Store, Product).
- `BE/PRN232_EbayClone.Infrastructure/`: DbContext, Migrations, External Services.
- `FE/src/pages/`: Divided by Role (ADMIN, SELLER, BUYER).

## Mermaid Diagram for Logic Flow
```mermaid
graph TD
    Client[React Frontend] -->|HTTP| API[ASP.NET Core API]
    API -->|Send| MediatR{MediatR}
    MediatR -->|Command/Query| Handler[Application Handler]
    Handler -->|Validate| Fluent[FluentValidation]
    Handler -->|Domain Logic| Entity[Domain Entity]
    Handler -->|Save/Read| EF[EF Core / Supabase]
    EF -->|Result| Handler
    Handler -->|Result-T| API
    API -->|JSON| Client