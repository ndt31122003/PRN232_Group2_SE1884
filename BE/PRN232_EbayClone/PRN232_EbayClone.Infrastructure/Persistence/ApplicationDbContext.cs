using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.FileMetadata.Entities;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.ListingTemplates.Entities;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Reports.Entities;
using PRN232_EbayClone.Domain.Policies.Entities;
using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.SellingPreferences.Entities;
using PRN232_EbayClone.Domain.Stores.Entities;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Coupons.Entities;
using PRN232_EbayClone.Domain.Vouchers.Entities;
using PRN232_EbayClone.Domain.Reviews.Entities;
using PRN232_EbayClone.Domain.Disputes.Entities;
using PRN232_EbayClone.Domain.BuyerFeedback.Entities;
using PRN232_EbayClone.Infrastructure.Outbox;
using System.Reflection;

namespace PRN232_EbayClone.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
    DbContext(options),
    IUnitOfWork
{
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Otp> Otps => Set<Otp>();
    public DbSet<FileMetadata> FileMetadatas => Set<FileMetadata>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<FixedPriceListing> FixedPriceListings => Set<FixedPriceListing>();
    public DbSet<AuctionListing> AuctionListings => Set<AuctionListing>();
    public DbSet<ListingTemplate> ListingTemplates => Set<ListingTemplate>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    public DbSet<OrderStatusTransition> OrderStatusTransitions => Set<OrderStatusTransition>();
public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<CouponType> CouponTypes => Set<CouponType>();
    public DbSet<CouponCondition> CouponConditions => Set<CouponCondition>();
    public DbSet<CouponExcludedCategory> CouponExcludedCategories => Set<CouponExcludedCategory>();
    public DbSet<CouponExcludedItem> CouponExcludedItems => Set<CouponExcludedItem>();
    public DbSet<CouponTargetAudience> CouponTargetAudiences => Set<CouponTargetAudience>();
    public DbSet<Voucher> Vouchers => Set<Voucher>();
    public DbSet<VoucherTransaction> VoucherTransactions => Set<VoucherTransaction>();
    public DbSet<PRN232_EbayClone.Domain.SaleEvents.Entities.SaleEvent> SaleEvents => Set<PRN232_EbayClone.Domain.SaleEvents.Entities.SaleEvent>();
    public DbSet<PRN232_EbayClone.Domain.SaleEvents.Entities.SaleEventDiscountTier> SaleEventDiscountTiers => Set<PRN232_EbayClone.Domain.SaleEvents.Entities.SaleEventDiscountTier>();
    public DbSet<PRN232_EbayClone.Domain.SaleEvents.Entities.SaleEventListing> SaleEventListings => Set<PRN232_EbayClone.Domain.SaleEvents.Entities.SaleEventListing>();
    public DbSet<ShippingLabel> ShippingLabels => Set<ShippingLabel>();
    public DbSet<OrderItemShipment> OrderItemShipments => Set<OrderItemShipment>();
    public DbSet<Domain.BuyerFeedback.Entities.BuyerFeedbackEntity> BuyerFeedbacks => Set<Domain.BuyerFeedback.Entities.BuyerFeedbackEntity>();
    public DbSet<SellerBlacklist> SellerBlacklists => Set<SellerBlacklist>();
    public DbSet<ShippingService> ShippingServices => Set<ShippingService>();
    public DbSet<CancellationRequest> CancellationRequests => Set<CancellationRequest>();
    public DbSet<ReturnRequest> ReturnRequests => Set<ReturnRequest>();
    public DbSet<ReportDownload> ReportDownloads => Set<ReportDownload>();
    public DbSet<ReportSchedule> ReportSchedules => Set<ReportSchedule>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<StoreSubscription> StoreSubscriptions => Set<StoreSubscription>();
    public DbSet<ShippingPolicy> ShippingPolicies => Set<ShippingPolicy>();
    public DbSet<ReturnPolicy> ReturnPolicies => Set<ReturnPolicy>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Dispute> Disputes => Set<Dispute>();
    public DbSet<DisputeResponse> DisputeResponses => Set<DisputeResponse>();
    public DbSet<SellerPreference> SellerPreferences => Set<SellerPreference>();
    public DbSet<ListingIdMapping> ListingIdMappings => Set<ListingIdMapping>();



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("pg_trgm");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
