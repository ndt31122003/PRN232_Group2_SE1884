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
using PRN232_EbayClone.Domain.Quartz;
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
    public DbSet<BuyerFeedback> BuyerFeedbacks => Set<BuyerFeedback>();
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
    public DbSet<SellerPreference> SellerPreferences => Set<SellerPreference>();

    public DbSet<QrtzJobDetail> QrtzJobDetails => Set<QrtzJobDetail>();
    public DbSet<QrtzTrigger> QrtzTriggers => Set<QrtzTrigger>();
    public DbSet<QrtzSimpleTrigger> QrtzSimpleTriggers => Set<QrtzSimpleTrigger>();
    public DbSet<QrtzCronTrigger> QrtzCronTriggers => Set<QrtzCronTrigger>();
    public DbSet<QrtzBlobTrigger> QrtzBlobTriggers => Set<QrtzBlobTrigger>();
    public DbSet<QrtzCalendar> QrtzCalendars => Set<QrtzCalendar>();
    public DbSet<QrtzPausedTriggerGrp> QrtzPausedTriggerGrps => Set<QrtzPausedTriggerGrp>();
    public DbSet<QrtzFiredTrigger> QrtzFiredTriggers => Set<QrtzFiredTrigger>();
    public DbSet<QrtzSchedulerState> QrtzSchedulerStates => Set<QrtzSchedulerState>();
    public DbSet<QrtzLock> QrtzLocks => Set<QrtzLock>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("pg_trgm");
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        builder.Entity<QrtzJobDetail>(e =>
        {
            e.ToTable("qrtz_job_details");
            e.HasKey(x => new { x.SchedName, x.JobName, x.JobGroup });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.JobName).HasColumnName("job_name");
            e.Property(x => x.JobGroup).HasColumnName("job_group");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.JobClassName).HasColumnName("job_class_name");
            e.Property(x => x.IsDurable).HasColumnName("is_durable");
            e.Property(x => x.IsNonconcurrent).HasColumnName("is_nonconcurrent");
            e.Property(x => x.IsUpdateData).HasColumnName("is_update_data");
            e.Property(x => x.RequestsRecovery).HasColumnName("requests_recovery");
            e.Property(x => x.JobData).HasColumnName("job_data");
        });

        builder.Entity<QrtzTrigger>(e =>
        {
            e.ToTable("qrtz_triggers");
            e.HasKey(x => new { x.SchedName, x.TriggerName, x.TriggerGroup });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.TriggerName).HasColumnName("trigger_name");
            e.Property(x => x.TriggerGroup).HasColumnName("trigger_group");
            e.Property(x => x.JobName).HasColumnName("job_name");
            e.Property(x => x.JobGroup).HasColumnName("job_group");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.NextFireTime).HasColumnName("next_fire_time");
            e.Property(x => x.PrevFireTime).HasColumnName("prev_fire_time");
            e.Property(x => x.Priority).HasColumnName("priority");
            e.Property(x => x.TriggerState).HasColumnName("trigger_state");
            e.Property(x => x.TriggerType).HasColumnName("trigger_type");
            e.Property(x => x.StartTime).HasColumnName("start_time");
            e.Property(x => x.EndTime).HasColumnName("end_time");
            e.Property(x => x.CalendarName).HasColumnName("calendar_name");
            e.Property(x => x.MisfireInstr).HasColumnName("misfire_instr");
            e.Property(x => x.JobData).HasColumnName("job_data");
            e.HasOne<QrtzJobDetail>()
                .WithMany()
                .HasForeignKey(x => new { x.SchedName, x.JobName, x.JobGroup })
                .HasConstraintName("FK_QRTZ_TRIGGERS_JOB_DETAILS");
        });

        builder.Entity<QrtzSimpleTrigger>(e =>
        {
            e.ToTable("qrtz_simple_triggers");
            e.HasKey(x => new { x.SchedName, x.TriggerName, x.TriggerGroup });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.TriggerName).HasColumnName("trigger_name");
            e.Property(x => x.TriggerGroup).HasColumnName("trigger_group");
            e.Property(x => x.RepeatCount).HasColumnName("repeat_count");
            e.Property(x => x.RepeatInterval).HasColumnName("repeat_interval");
            e.Property(x => x.TimesTriggered).HasColumnName("times_triggered");
        });

        builder.Entity<QrtzCronTrigger>(e =>
        {
            e.ToTable("qrtz_cron_triggers");
            e.HasKey(x => new { x.SchedName, x.TriggerName, x.TriggerGroup });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.TriggerName).HasColumnName("trigger_name");
            e.Property(x => x.TriggerGroup).HasColumnName("trigger_group");
            e.Property(x => x.CronExpression).HasColumnName("cron_expression");
            e.Property(x => x.TimeZoneId).HasColumnName("time_zone_id");
        });

        builder.Entity<QrtzBlobTrigger>(e =>
        {
            e.ToTable("qrtz_blob_triggers");
            e.HasKey(x => new { x.SchedName, x.TriggerName, x.TriggerGroup });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.TriggerName).HasColumnName("trigger_name");
            e.Property(x => x.TriggerGroup).HasColumnName("trigger_group");
            e.Property(x => x.BlobData).HasColumnName("blob_data");
        });

        builder.Entity<QrtzCalendar>(e =>
        {
            e.ToTable("qrtz_calendars");
            e.HasKey(x => new { x.SchedName, x.CalendarName });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.CalendarName).HasColumnName("calendar_name");
            e.Property(x => x.Calendar).HasColumnName("calendar");
        });

        builder.Entity<QrtzPausedTriggerGrp>(e =>
        {
            e.ToTable("qrtz_paused_trigger_grps");
            e.HasKey(x => new { x.SchedName, x.TriggerGroup });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.TriggerGroup).HasColumnName("trigger_group");
        });

        builder.Entity<QrtzFiredTrigger>(e =>
        {
            e.ToTable("qrtz_fired_triggers");
            e.HasKey(x => new { x.SchedName, x.EntryId });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.EntryId).HasColumnName("entry_id");
            e.Property(x => x.TriggerName).HasColumnName("trigger_name");
            e.Property(x => x.TriggerGroup).HasColumnName("trigger_group");
            e.Property(x => x.InstanceName).HasColumnName("instance_name");
            e.Property(x => x.FiredTime).HasColumnName("fired_time");
            e.Property(x => x.SchedTime).HasColumnName("sched_time");
            e.Property(x => x.Priority).HasColumnName("priority");
            e.Property(x => x.State).HasColumnName("state");
            e.Property(x => x.JobName).HasColumnName("job_name");
            e.Property(x => x.JobGroup).HasColumnName("job_group");
            e.Property(x => x.IsNonconcurrent).HasColumnName("is_nonconcurrent");
            e.Property(x => x.RequestsRecovery).HasColumnName("requests_recovery");
        });

        builder.Entity<QrtzSchedulerState>(e =>
        {
            e.ToTable("qrtz_scheduler_state");
            e.HasKey(x => new { x.SchedName, x.InstanceName });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.InstanceName).HasColumnName("instance_name");
            e.Property(x => x.LastCheckinTime).HasColumnName("last_checkin_time");
            e.Property(x => x.CheckinInterval).HasColumnName("checkin_interval");
        });

        builder.Entity<QrtzLock>(e =>
        {
            e.ToTable("qrtz_locks");
            e.HasKey(x => new { x.SchedName, x.LockName });
            e.Property(x => x.SchedName).HasColumnName("sched_name");
            e.Property(x => x.LockName).HasColumnName("lock_name");
        });
    }
}
