# Progress Memory

## Batch A
- Added `IInventoryRepository` abstraction and `InventoryRepository` implementation.
- Added inventory DTO mappings for MediatR responses.
- Added `InitializeInventoryCommand` with validator and handler.
- Added `GetInventoryByListingIdQuery` with validator and handler.
- Added `InventoriesController` endpoints for initialize/get.
- Registered `IInventoryRepository` in infrastructure DI.
- Schema check via Supabase MCP: `inventory`, `inventory_reservation`, `inventory_adjustment` are not present on the target database yet, so local migration exists but is not applied to Supabase.
- `dotnet build PRN232_EbayClone.sln` passed after Batch A.

## Batch B
- Added `ReserveStockCommand` with validator, handler, and response DTO.
- Added `CommitStockCommand` with validator and handler.
- Added `ReleaseStockCommand` with validator and handler.
- Extended `InventoriesController` with `/reserve`, `/commit`, `/release` endpoints.
- `dotnet build PRN232_EbayClone.sln` passed after Batch B.
- `dotnet test PRN232_EbayClone.Tests/PRN232_EbayClone.Tests.csproj --filter "InventoryEntityTests"` passed: 10/10 tests.
- Inventory schema has now been applied to the target database; `inventory`, `inventory_adjustment`, and `inventory_reservation` tables are present.

## Batch C
- `CreateListingCommand` now auto-creates an `Inventory` aggregate when a listing is created, so listing creation and inventory initialization persist together.
- Initial inventory quantity is derived from the created listing aggregate for single-price, multi-variation, and auction listings.

## Batch D
- Added transaction support to `IUnitOfWork` and row-level locking via `FOR UPDATE` for `ReserveStockCommand` to reduce oversell risk under concurrent reservations.
- `ReserveStockCommand` now supports an optional `OrderId` on reservations for forward-compatible checkout and order linkage.
- `CancellationApprovedHandler` now releases exact-match active reservations for the cancelled order when they exist.
- Added `CompleteReturnRefundCommand` and `POST /api/orders/returns/{returnRequestId}/refund` to complete return refunds and restock sold inventory when return items are received back.

## Batch E
- Added `GetPerformanceInventoryDashboardQuery` and `GET /api/performance/inventory-dashboard`.
- Implemented inventory dashboard aggregation in `PerformanceRepository` using Dapper against the `inventory` table and Redis-backed `IDistributedCache` with memory-cache fallback.
- Removed hardcoded seller IDs from `PerformanceController`; performance endpoints now use the authenticated seller context.