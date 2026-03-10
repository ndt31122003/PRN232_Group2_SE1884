using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_trgm", ",,");

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_category_ParentId",
                        column: x => x.ParentId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "condition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "coupon_type",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupon_type", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "file_metadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkedEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_metadata", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "listing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Format = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Sku = table.Column<string>(type: "text", nullable: false),
                    ListingDescription = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConditionId = table.Column<Guid>(type: "uuid", nullable: true),
                    ConditionDescription = table.Column<string>(type: "text", nullable: false),
                    ScheduledStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DraftExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    ListingFormat = table.Column<int>(type: "integer", nullable: false),
                    Pricing_StartPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Pricing_ReservePrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Pricing_BuyItNowPrice = table.Column<decimal>(type: "numeric", nullable: true),
                    Pricing_Quantity = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Pricing_Price = table.Column<decimal>(type: "numeric", nullable: true),
                    FixedPricePricing_Pricing_Quantity = table.Column<int>(type: "integer", nullable: true),
                    OfferSettings_AllowOffers = table.Column<bool>(type: "boolean", nullable: true),
                    OfferSettings_MinimumOffer = table.Column<decimal>(type: "numeric", nullable: true),
                    OfferSettings_AutoAcceptOffer = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "listing_template",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    PayloadJson = table.Column<string>(type: "jsonb", nullable: false),
                    FormatLabel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_template", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "order_statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "otp",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_otp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_outbox_message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "qrtz_blob_triggers",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    TriggerName = table.Column<string>(type: "text", nullable: false),
                    TriggerGroup = table.Column<string>(type: "text", nullable: false),
                    BlobData = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_blob_triggers", x => new { x.SchedName, x.TriggerName, x.TriggerGroup });
                });

            migrationBuilder.CreateTable(
                name: "qrtz_calendars",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    CalendarName = table.Column<string>(type: "text", nullable: false),
                    Calendar = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_calendars", x => new { x.SchedName, x.CalendarName });
                });

            migrationBuilder.CreateTable(
                name: "qrtz_cron_triggers",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    TriggerName = table.Column<string>(type: "text", nullable: false),
                    TriggerGroup = table.Column<string>(type: "text", nullable: false),
                    CronExpression = table.Column<string>(type: "text", nullable: false),
                    TimeZoneId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_cron_triggers", x => new { x.SchedName, x.TriggerName, x.TriggerGroup });
                });

            migrationBuilder.CreateTable(
                name: "qrtz_fired_triggers",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    EntryId = table.Column<string>(type: "text", nullable: false),
                    TriggerName = table.Column<string>(type: "text", nullable: false),
                    TriggerGroup = table.Column<string>(type: "text", nullable: false),
                    InstanceName = table.Column<string>(type: "text", nullable: false),
                    FiredTime = table.Column<long>(type: "bigint", nullable: false),
                    SchedTime = table.Column<long>(type: "bigint", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    JobName = table.Column<string>(type: "text", nullable: true),
                    JobGroup = table.Column<string>(type: "text", nullable: true),
                    IsNonconcurrent = table.Column<string>(type: "text", nullable: true),
                    RequestsRecovery = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_fired_triggers", x => new { x.SchedName, x.EntryId });
                });

            migrationBuilder.CreateTable(
                name: "qrtz_job_details",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    JobName = table.Column<string>(type: "text", nullable: false),
                    JobGroup = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    JobClassName = table.Column<string>(type: "text", nullable: false),
                    IsDurable = table.Column<string>(type: "text", nullable: false),
                    IsNonconcurrent = table.Column<string>(type: "text", nullable: false),
                    IsUpdateData = table.Column<string>(type: "text", nullable: false),
                    RequestsRecovery = table.Column<string>(type: "text", nullable: true),
                    JobData = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_job_details", x => new { x.SchedName, x.JobName, x.JobGroup });
                });

            migrationBuilder.CreateTable(
                name: "qrtz_locks",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    LockName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_locks", x => new { x.SchedName, x.LockName });
                });

            migrationBuilder.CreateTable(
                name: "qrtz_paused_trigger_grps",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    TriggerGroup = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_paused_trigger_grps", x => new { x.SchedName, x.TriggerGroup });
                });

            migrationBuilder.CreateTable(
                name: "qrtz_scheduler_state",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    InstanceName = table.Column<string>(type: "text", nullable: false),
                    LastCheckinTime = table.Column<long>(type: "bigint", nullable: false),
                    CheckinInterval = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_scheduler_state", x => new { x.SchedName, x.InstanceName });
                });

            migrationBuilder.CreateTable(
                name: "qrtz_simple_triggers",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    TriggerName = table.Column<string>(type: "text", nullable: false),
                    TriggerGroup = table.Column<string>(type: "text", nullable: false),
                    RepeatCount = table.Column<long>(type: "bigint", nullable: false),
                    RepeatInterval = table.Column<long>(type: "bigint", nullable: false),
                    TimesTriggered = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_simple_triggers", x => new { x.SchedName, x.TriggerName, x.TriggerGroup });
                });

            migrationBuilder.CreateTable(
                name: "report_downloads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReferenceCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Source = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    RequestedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    FileUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    DateRange_StartUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateRange_EndUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DateRange_TimeZone = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_downloads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "report_schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Type = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Frequency = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastRunAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NextRunAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDateUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    DeliveryEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_report_schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "return_policy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    AcceptReturns = table.Column<bool>(type: "boolean", nullable: false),
                    ReturnPeriodDays = table.Column<int>(type: "integer", nullable: true),
                    RefundMethod = table.Column<int>(type: "integer", nullable: true),
                    ReturnShippingPaidBy = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_return_policy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "sale_event",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(90)", maxLength: 90, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Mode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    OfferFreeShipping = table.Column<bool>(type: "boolean", nullable: false),
                    IncludeSkippedItems = table.Column<bool>(type: "boolean", nullable: false),
                    BlockPriceIncreaseRevisions = table.Column<bool>(type: "boolean", nullable: false),
                    HighlightPercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_event", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "seller_preference",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingsStayActiveWhenOutOfStock = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ShowExactQuantityAvailable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    BuyersCanSeeVatNumber = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    VatNumber = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    BuyerManagement_BlockUnpaidItemStrikes = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    BuyerManagement_UnpaidItemStrikesCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    BuyerManagement_UnpaidItemStrikesPeriodInMonths = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    BuyerManagement_BlockPrimaryAddressOutsideShippingLocation = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    BuyerManagement_BlockMaxItemsInLastTenDays = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    BuyerManagement_MaxItemsInLastTenDays = table.Column<int>(type: "integer", nullable: true),
                    BuyerManagement_ApplyFeedbackScoreThreshold = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    BuyerManagement_FeedbackScoreThreshold = table.Column<int>(type: "integer", nullable: true),
                    BuyerManagement_UpdateBlockSettingsForActiveListings = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    BuyerManagement_RequirePaymentMethodBeforeBid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    BuyerManagement_RequirePaymentMethodBeforeOffer = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    BuyerManagement_PreventBlockedBuyersFromContacting = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    InvoicePreference_Format = table.Column<int>(type: "integer", nullable: false),
                    InvoicePreference_SendEmailCopy = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    InvoicePreference_ApplyCreditsAutomatically = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seller_preference", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shipping_policy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    Carrier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Cost_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Cost_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    HandlingTimeDays = table.Column<int>(type: "integer", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipping_policy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "shipping_services",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Carrier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    ServiceCode = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SavingsDescription = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CoverageDescription = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Notes = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DeliveryWindowLabel = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    PrinterRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    SupportsQrCode = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MinEstimatedDeliveryDays = table.Column<int>(type: "integer", nullable: false),
                    MaxEstimatedDeliveryDays = table.Column<int>(type: "integer", nullable: false),
                    BaseCost_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    BaseCost_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipping_services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "store",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Slug = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LogoUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BannerUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StoreType = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    IsEmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IsPaymentVerified = table.Column<bool>(type: "boolean", nullable: false),
                    PerformanceLevel = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    _activeTotalValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: false),
                    InitialValue = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignedUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsTransferable = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "category_specific",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    AllowMultiple = table.Column<bool>(type: "boolean", nullable: false),
                    _values = table.Column<string>(type: "jsonb", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_specific", x => x.Id);
                    table.ForeignKey(
                        name: "FK_category_specific_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "category_condition",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConditionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_condition", x => new { x.CategoryId, x.ConditionId });
                    table.ForeignKey(
                        name: "FK_category_condition_category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_category_condition_condition_ConditionId",
                        column: x => x.ConditionId,
                        principalTable: "condition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coupon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    DiscountUnit = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    MaxDiscount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsageLimit = table.Column<int>(type: "integer", nullable: true),
                    UsagePerUser = table.Column<int>(type: "integer", nullable: true),
                    MinimumOrderValue = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    ApplicablePriceMin = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    ApplicablePriceMax = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coupon_coupon_type_CouponTypeId",
                        column: x => x.CouponTypeId,
                        principalTable: "coupon_type",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dispute",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingId = table.Column<Guid>(type: "uuid", nullable: false),
                    RaisedById = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Reason = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dispute", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dispute_listing_ListingId",
                        column: x => x.ListingId,
                        principalTable: "listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "listing_image",
                columns: table => new
                {
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: false),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_image", x => new { x.listing_id, x.Id });
                    table.ForeignKey(
                        name: "FK_listing_image_listing_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_item_specific",
                columns: table => new
                {
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Values = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_listing_item_specific", x => new { x.listing_id, x.Id });
                    table.ForeignKey(
                        name: "FK_listing_item_specific_listing_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "review",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RecipientId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ReviewerRole = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Buyer"),
                    RecipientRole = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "Seller"),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    Reply = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    RepliedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RevisionStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "None"),
                    RevisionRequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_review_listing_ListingId",
                        column: x => x.ListingId,
                        principalTable: "listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "variation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sku = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    VariationSpecifics = table.Column<string>(type: "jsonb", nullable: false),
                    Images = table.Column<string>(type: "jsonb", nullable: false),
                    ListingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_variation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_variation_listing_ListingId",
                        column: x => x.ListingId,
                        principalTable: "listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_status_transitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    AllowedRoles = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_status_transitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_status_transitions_order_statuses_FromStatusId",
                        column: x => x.FromStatusId,
                        principalTable: "order_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_status_transitions_order_statuses_ToStatusId",
                        column: x => x.ToStatusId,
                        principalTable: "order_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qrtz_triggers",
                columns: table => new
                {
                    SchedName = table.Column<string>(type: "text", nullable: false),
                    TriggerName = table.Column<string>(type: "text", nullable: false),
                    TriggerGroup = table.Column<string>(type: "text", nullable: false),
                    JobName = table.Column<string>(type: "text", nullable: false),
                    JobGroup = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    NextFireTime = table.Column<long>(type: "bigint", nullable: true),
                    PrevFireTime = table.Column<long>(type: "bigint", nullable: true),
                    Priority = table.Column<int>(type: "integer", nullable: true),
                    TriggerState = table.Column<string>(type: "text", nullable: false),
                    TriggerType = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<long>(type: "bigint", nullable: false),
                    EndTime = table.Column<long>(type: "bigint", nullable: true),
                    CalendarName = table.Column<string>(type: "text", nullable: true),
                    MisfireInstr = table.Column<short>(type: "smallint", nullable: true),
                    JobData = table.Column<byte[]>(type: "bytea", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrtz_triggers", x => new { x.SchedName, x.TriggerName, x.TriggerGroup });
                    table.ForeignKey(
                        name: "FK_QRTZ_TRIGGERS_JOB_DETAILS",
                        columns: x => new { x.SchedName, x.JobName, x.JobGroup },
                        principalTable: "qrtz_job_details",
                        principalColumns: new[] { "SchedName", "JobName", "JobGroup" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    Permission = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => new { x.RoleId, x.Permission });
                    table.ForeignKey(
                        name: "FK_role_permissions_role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sale_event_discount_tier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DiscountValue = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    Label = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_event_discount_tier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sale_event_discount_tier_sale_event_SaleEventId",
                        column: x => x.SaleEventId,
                        principalTable: "sale_event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seller_blocked_buyer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NormalizedIdentifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    seller_preference_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seller_blocked_buyer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seller_blocked_buyer_seller_preference_seller_preference_id",
                        column: x => x.seller_preference_id,
                        principalTable: "seller_preference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seller_exempt_buyer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NormalizedIdentifier = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now() at time zone 'utc'"),
                    seller_preference_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seller_exempt_buyer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seller_exempt_buyer_seller_preference_seller_preference_id",
                        column: x => x.seller_preference_id,
                        principalTable: "seller_preference",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "store_subscription",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionType = table.Column<int>(type: "integer", nullable: false),
                    MonthlyFee_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MonthlyFee_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    FinalValueFeePercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    ListingLimit = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_store_subscription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_store_subscription_store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListingId",
                columns: table => new
                {
                    Value = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingId", x => x.Value);
                    table.ForeignKey(
                        name: "FK_ListingId_user_seller_id",
                        column: x => x.seller_id,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    sub_total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    sub_total_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    shipping_cost_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    shipping_cost_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    platform_fee_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    platform_fee_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    tax_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    tax_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    discount_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    total_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShippingStatus = table.Column<int>(type: "integer", nullable: false),
                    FulfillmentType = table.Column<int>(type: "integer", nullable: false),
                    OrderedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CouponCode = table.Column<string>(type: "text", nullable: true),
                    PromotionId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_orders_order_statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "order_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_user_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "user",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refresh_token", x => x.Id);
                    table.ForeignKey(
                        name: "FK_refresh_token_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_role_RolesId",
                        column: x => x.RolesId,
                        principalTable: "role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoucherTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VoucherId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    AmountUsed = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoucherTransactions_Vouchers_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Vouchers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coupon_condition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyQuantity = table.Column<int>(type: "integer", nullable: true),
                    GetQuantity = table.Column<int>(type: "integer", nullable: true),
                    GetDiscountPercent = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    SaveEveryAmount = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    SaveEveryItems = table.Column<int>(type: "integer", nullable: true),
                    ConditionDescription = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coupon_condition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coupon_condition_coupon_CouponId",
                        column: x => x.CouponId,
                        principalTable: "coupon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouponExcludedCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponExcludedCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponExcludedCategories_coupon_CouponId",
                        column: x => x.CouponId,
                        principalTable: "coupon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouponExcludedItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponId = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponExcludedItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponExcludedItems_coupon_CouponId",
                        column: x => x.CouponId,
                        principalTable: "coupon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouponTargetAudiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CouponId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserType = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    MinAccountAgeDays = table.Column<int>(type: "integer", nullable: true),
                    MinTotalSpent = table.Column<decimal>(type: "numeric", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponTargetAudiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponTargetAudiences_coupon_CouponId",
                        column: x => x.CouponId,
                        principalTable: "coupon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sale_event_listing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SaleEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountTierId = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale_event_listing", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sale_event_listing_sale_event_SaleEventId",
                        column: x => x.SaleEventId,
                        principalTable: "sale_event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_sale_event_listing_sale_event_discount_tier_DiscountTierId",
                        column: x => x.DiscountTierId,
                        principalTable: "sale_event_discount_tier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_buyer_feedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Comment = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    UsesStoredComment = table.Column<bool>(type: "boolean", nullable: false),
                    StoredCommentKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    FollowUpComment = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    FollowUpCommentedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_buyer_feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_buyer_feedback_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_buyer_feedback_user_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "user",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "order_cancellation_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    InitiatedBy = table.Column<int>(type: "integer", nullable: false),
                    Reason = table.Column<int>(type: "integer", nullable: false),
                    BuyerNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SellerNote = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SellerResponseDeadlineUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SellerRespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AutoClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RefundAmount_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    RefundAmount_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    OrderTotalSnapshot_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OrderTotalSnapshot_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_cancellation_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_cancellation_requests_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ListingId = table.Column<Guid>(type: "uuid", nullable: false),
                    VariationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    UnitPrice_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    TotalPrice_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_items_listing_ListingId",
                        column: x => x.ListingId,
                        principalTable: "listing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_order_items_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_return_requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Reason = table.Column<int>(type: "integer", nullable: false),
                    PreferredResolution = table.Column<int>(type: "integer", nullable: false),
                    BuyerNote = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    SellerNote = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SellerRespondedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BuyerReturnDueAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BuyerShippedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefundIssuedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ReturnCarrier = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    TrackingNumber = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RefundAmount_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    RefundAmount_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    RestockingFee_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    RestockingFee_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    OrderTotalSnapshot_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    OrderTotalSnapshot_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_return_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_return_requests_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_shipping_labels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShippingServiceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Carrier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ServiceCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ServiceName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    TrackingNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LabelUrl = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    LabelFileName = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    Cost_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Cost_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Insurance_Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Insurance_Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    PackageType = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    WeightOz = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    LengthIn = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    WidthIn = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    HeightIn = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    PurchasedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EstimatedDelivery = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LabelDocumentId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsVoided = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    VoidedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    VoidReason = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_shipping_labels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_shipping_labels_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_status_histories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToStatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_status_histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_status_histories_order_statuses_FromStatusId",
                        column: x => x.FromStatusId,
                        principalTable: "order_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_status_histories_order_statuses_ToStatusId",
                        column: x => x.ToStatusId,
                        principalTable: "order_statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_status_histories_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_item_shipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    ShippingLabelId = table.Column<Guid>(type: "uuid", nullable: true),
                    TrackingNumber = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Carrier = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    ShippedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_item_shipments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_order_item_shipments_order_shipping_labels_ShippingLabelId",
                        column: x => x.ShippingLabelId,
                        principalTable: "order_shipping_labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_order_item_shipments_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsDeleted", "Name", "ParentId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Consumer electronics, components, and accessories.", false, "Electronics", null, null, null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Apparel, shoes, and accessories.", false, "Fashion", null, null, null },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Home improvement, décor, and outdoor living.", false, "Home & Garden", null, null, null },
                    { new Guid("50000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Complete automotive marketplace for vehicles and parts.", false, "eBay Motors", null, null, null },
                    { new Guid("60000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Treasures from pop culture, history, and fine art.", false, "Collectibles & Art", null, null, null },
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Playsets, model kits, and collector favorites.", false, "Toys & Hobbies", null, null, null },
                    { new Guid("80000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Gear for every sport, indoors and out.", false, "Sporting Goods", null, null, null },
                    { new Guid("90000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Wellness essentials and personal care favorites.", false, "Health & Beauty", null, null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Equipment, supplies, and services for every trade.", false, "Business & Industrial", null, null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Instruments, pro audio, and stage equipment.", false, "Musical Instruments & Gear", null, null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Care essentials for pets large and small.", false, "Pet Supplies", null, null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nursery gear, travel systems, and feeding must-haves.", false, "Baby Essentials", null, null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "DIY staples spanning every creative discipline.", false, "Crafts", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "condition",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsDeleted", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Factory sealed, unused item in original packaging.", false, "New", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Professionally restored to working order by the manufacturer or certified provider.", false, "Manufacturer Refurbished", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Restored to working order by a third-party seller.", false, "Seller Refurbished", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Previously owned item that shows signs of use.", false, "Used", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Item does not function as intended and is being sold for parts or repair.", false, "For parts or not working", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Item is unused but the original packaging has been opened.", false, "Open box", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Unused apparel item with original tags attached.", false, "New with tags", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Unused apparel item missing the retail tags.", false, "New without tags", null, null },
                    { new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Previously worn item that remains in good condition.", false, "Pre-owned", null, null }
                });

            migrationBuilder.InsertData(
                table: "coupon_type",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("0d0c32fe-349c-4857-b20a-2d3f8db91ed4"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Percentage discount when buying Y or more items", true, "Extra % off Y or more items", null, null },
                    { new Guid("2c5a6a6a-fe7e-4813-a134-70572b5ab90a"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Percentage discount when total order value reaches a minimum value", true, "Extra % off $ or more", null, null },
                    { new Guid("3b980145-62b6-4ae6-9cf8-7838bc7b84e0"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Save a fixed amount for every X items purchased", true, "Save $ for every X items", null, null },
                    { new Guid("51f2ed38-06bb-496e-b5cb-7aa3057c21b7"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Save a fixed amount for every dollar spent", true, "Save $ for every $ spent", null, null },
                    { new Guid("773f8d9b-eb8e-4ff4-a21e-4bb2fa5407f4"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fixed amount discount when buying X or more items", true, "Extra $ off X or more items", null, null },
                    { new Guid("7a5a0b7a-ed8f-4b91-a7c3-59e5363b76f3"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fixed amount discount for each item purchased", true, "Extra $ off each item", null, null },
                    { new Guid("7eaa19cf-6b36-4a1c-b7b5-a9abcb7eeff2"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Buy X items and get Y items at a percentage discount", true, "Buy X get Y at % off", null, null },
                    { new Guid("990c28b3-753e-41b1-a798-965cf46b7dcd"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fixed amount discount on all eligible items", true, "Extra $ off", null, null },
                    { new Guid("9e1d4ea5-5b09-48be-be90-e2790f6ba537"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Percentage discount on all eligible items", true, "Extra % off", null, null },
                    { new Guid("cfa2e0f1-b720-4590-a7d4-4ce0844f9671"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fixed amount discount when order value reaches a minimum threshold", true, "Extra $ off $ or more", null, null },
                    { new Guid("ed9d5151-6f8c-4628-a5a9-4c24867e5673"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Buy X items and get Y items for free", true, "Buy X get Y free", null, null }
                });

            migrationBuilder.InsertData(
                table: "listing",
                columns: new[] { "Id", "CategoryId", "ConditionDescription", "ConditionId", "CreatedAt", "CreatedBy", "DraftExpiredAt", "Duration", "EndDate", "Format", "IsDeleted", "ListingDescription", "ListingFormat", "ScheduledStartTime", "Sku", "StartDate", "Status", "Title", "Type", "UpdatedAt", "UpdatedBy", "Pricing_Price", "FixedPricePricing_Pricing_Quantity", "OfferSettings_AllowOffers", "OfferSettings_AutoAcceptOffer", "OfferSettings_MinimumOffer" },
                values: new object[,]
                {
                    { new Guid("71000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #1 for Alice Johnson.", 2, null, "DEMO-1-0001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #1", 1, null, null, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-000000000002"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #2 for Alice Johnson.", 2, null, "DEMO-1-0002", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #2", 1, null, null, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #3 for Alice Johnson.", 2, null, "DEMO-1-0003", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #3", 1, null, null, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #4 for Alice Johnson.", 2, null, "DEMO-1-0004", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #4", 1, null, null, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000005"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #5 for Alice Johnson.", 2, null, "DEMO-1-0005", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #5", 1, null, null, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000006"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #6 for Alice Johnson.", 2, null, "DEMO-1-0006", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #6", 1, null, null, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000007"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #7 for Alice Johnson.", 2, null, "DEMO-1-0007", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #7", 1, null, null, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000008"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #8 for Alice Johnson.", 2, null, "DEMO-1-0008", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #8", 1, null, null, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000009"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #9 for Alice Johnson.", 2, null, "DEMO-1-0009", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #9", 1, null, null, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000000a"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #10 for Alice Johnson.", 2, null, "DEMO-1-0010", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #10", 1, null, null, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-00000000000b"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #11 for Alice Johnson.", 2, null, "DEMO-1-0011", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #11", 1, null, null, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000000c"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #12 for Alice Johnson.", 2, null, "DEMO-1-0012", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #12", 1, null, null, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000000d"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #13 for Alice Johnson.", 2, null, "DEMO-1-0013", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #13", 1, null, null, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000000e"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #14 for Alice Johnson.", 2, null, "DEMO-1-0014", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #14", 1, null, null, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000000f"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #15 for Alice Johnson.", 2, null, "DEMO-1-0015", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #15", 1, null, null, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000010"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #16 for Alice Johnson.", 2, null, "DEMO-1-0016", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #16", 1, null, null, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-000000000011"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #17 for Alice Johnson.", 2, null, "DEMO-1-0017", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #17", 1, null, null, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000012"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #18 for Alice Johnson.", 2, null, "DEMO-1-0018", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #18", 1, null, null, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000013"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #19 for Alice Johnson.", 2, null, "DEMO-1-0019", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #19", 1, null, null, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000014"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #20 for Alice Johnson.", 2, null, "DEMO-1-0020", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #20", 1, null, null, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000015"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #21 for Alice Johnson.", 2, null, "DEMO-1-0021", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #21", 1, null, null, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000016"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #22 for Alice Johnson.", 2, null, "DEMO-1-0022", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #22", 1, null, null, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000017"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #23 for Alice Johnson.", 2, null, "DEMO-1-0023", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #23", 1, null, null, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000018"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #24 for Alice Johnson.", 2, null, "DEMO-1-0024", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #24", 1, null, null, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000019"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #25 for Alice Johnson.", 2, null, "DEMO-1-0025", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #25", 1, null, null, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-00000000001a"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #26 for Alice Johnson.", 2, null, "DEMO-1-0026", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #26", 1, null, null, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000001b"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #27 for Alice Johnson.", 2, null, "DEMO-1-0027", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #27", 1, null, null, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000001c"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #28 for Alice Johnson.", 2, null, "DEMO-1-0028", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #28", 1, null, null, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000001d"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #29 for Alice Johnson.", 2, null, "DEMO-1-0029", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #29", 1, null, null, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000001e"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #30 for Alice Johnson.", 2, null, "DEMO-1-0030", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #30", 1, null, null, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000001f"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #31 for Alice Johnson.", 2, null, "DEMO-1-0031", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #31", 1, null, null, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-000000000020"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #32 for Alice Johnson.", 2, null, "DEMO-1-0032", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #32", 1, null, null, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000021"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #33 for Alice Johnson.", 2, null, "DEMO-1-0033", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #33", 1, null, null, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000022"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #34 for Alice Johnson.", 2, null, "DEMO-1-0034", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #34", 1, null, null, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000023"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #35 for Alice Johnson.", 2, null, "DEMO-1-0035", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #35", 1, null, null, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000024"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #36 for Alice Johnson.", 2, null, "DEMO-1-0036", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #36", 1, null, null, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000025"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #37 for Alice Johnson.", 2, null, "DEMO-1-0037", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #37", 1, null, null, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000026"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #38 for Alice Johnson.", 2, null, "DEMO-1-0038", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #38", 1, null, null, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000027"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #39 for Alice Johnson.", 2, null, "DEMO-1-0039", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #39", 1, null, null, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000028"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #40 for Alice Johnson.", 2, null, "DEMO-1-0040", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #40", 1, null, null, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-000000000029"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #41 for Alice Johnson.", 2, null, "DEMO-1-0041", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #41", 1, null, null, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000002a"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #42 for Alice Johnson.", 2, null, "DEMO-1-0042", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #42", 1, null, null, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000002b"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #43 for Alice Johnson.", 2, null, "DEMO-1-0043", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #43", 1, null, null, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000002c"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #44 for Alice Johnson.", 2, null, "DEMO-1-0044", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #44", 1, null, null, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000002d"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #45 for Alice Johnson.", 2, null, "DEMO-1-0045", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #45", 1, null, null, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000002e"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #46 for Alice Johnson.", 2, null, "DEMO-1-0046", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #46", 1, null, null, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-00000000002f"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #47 for Alice Johnson.", 2, null, "DEMO-1-0047", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #47", 1, null, null, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000030"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #48 for Alice Johnson.", 2, null, "DEMO-1-0048", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #48", 1, null, null, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000031"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #49 for Alice Johnson.", 2, null, "DEMO-1-0049", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #49", 1, null, null, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000032"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #50 for Alice Johnson.", 2, null, "DEMO-1-0050", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #50", 1, null, null, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000033"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #51 for Alice Johnson.", 2, null, "DEMO-1-0051", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #51", 1, null, null, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000034"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #52 for Alice Johnson.", 2, null, "DEMO-1-0052", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #52", 1, null, null, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000035"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #53 for Alice Johnson.", 2, null, "DEMO-1-0053", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #53", 1, null, null, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000036"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #54 for Alice Johnson.", 2, null, "DEMO-1-0054", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #54", 1, null, null, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000037"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #55 for Alice Johnson.", 2, null, "DEMO-1-0055", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #55", 1, null, null, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-000000000038"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #56 for Alice Johnson.", 2, null, "DEMO-1-0056", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #56", 1, null, null, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000039"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #57 for Alice Johnson.", 2, null, "DEMO-1-0057", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #57", 1, null, null, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000003a"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #58 for Alice Johnson.", 2, null, "DEMO-1-0058", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #58", 1, null, null, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000003b"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #59 for Alice Johnson.", 2, null, "DEMO-1-0059", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #59", 1, null, null, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000003c"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #60 for Alice Johnson.", 2, null, "DEMO-1-0060", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #60", 1, null, null, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000003d"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #61 for Alice Johnson.", 2, null, "DEMO-1-0061", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #61", 1, null, null, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-00000000003e"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #62 for Alice Johnson.", 2, null, "DEMO-1-0062", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #62", 1, null, null, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000003f"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #63 for Alice Johnson.", 2, null, "DEMO-1-0063", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #63", 1, null, null, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000040"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #64 for Alice Johnson.", 2, null, "DEMO-1-0064", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #64", 1, null, null, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000041"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #65 for Alice Johnson.", 2, null, "DEMO-1-0065", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #65", 1, null, null, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000042"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #66 for Alice Johnson.", 2, null, "DEMO-1-0066", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #66", 1, null, null, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000043"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #67 for Alice Johnson.", 2, null, "DEMO-1-0067", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #67", 1, null, null, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000044"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #68 for Alice Johnson.", 2, null, "DEMO-1-0068", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #68", 1, null, null, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000045"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #69 for Alice Johnson.", 2, null, "DEMO-1-0069", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #69", 1, null, null, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000046"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #70 for Alice Johnson.", 2, null, "DEMO-1-0070", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #70", 1, null, null, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-000000000047"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #71 for Alice Johnson.", 2, null, "DEMO-1-0071", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #71", 1, null, null, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000048"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #72 for Alice Johnson.", 2, null, "DEMO-1-0072", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #72", 1, null, null, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000049"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #73 for Alice Johnson.", 2, null, "DEMO-1-0073", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #73", 1, null, null, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000004a"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #74 for Alice Johnson.", 2, null, "DEMO-1-0074", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #74", 1, null, null, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000004b"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #75 for Alice Johnson.", 2, null, "DEMO-1-0075", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #75", 1, null, null, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000004c"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #76 for Alice Johnson.", 2, null, "DEMO-1-0076", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #76", 1, null, null, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-00000000004d"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #77 for Alice Johnson.", 2, null, "DEMO-1-0077", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #77", 1, null, null, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000004e"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #78 for Alice Johnson.", 2, null, "DEMO-1-0078", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #78", 1, null, null, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000004f"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #79 for Alice Johnson.", 2, null, "DEMO-1-0079", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #79", 1, null, null, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000050"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #80 for Alice Johnson.", 2, null, "DEMO-1-0080", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #80", 1, null, null, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000051"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #81 for Alice Johnson.", 2, null, "DEMO-1-0081", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #81", 1, null, null, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000052"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #82 for Alice Johnson.", 2, null, "DEMO-1-0082", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #82", 1, null, null, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000053"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #83 for Alice Johnson.", 2, null, "DEMO-1-0083", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #83", 1, null, null, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000054"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #84 for Alice Johnson.", 2, null, "DEMO-1-0084", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #84", 1, null, null, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000055"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #85 for Alice Johnson.", 2, null, "DEMO-1-0085", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #85", 1, null, null, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-000000000056"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #86 for Alice Johnson.", 2, null, "DEMO-1-0086", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #86", 1, null, null, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000057"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #87 for Alice Johnson.", 2, null, "DEMO-1-0087", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #87", 1, null, null, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000058"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #88 for Alice Johnson.", 2, null, "DEMO-1-0088", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #88", 1, null, null, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-000000000059"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #89 for Alice Johnson.", 2, null, "DEMO-1-0089", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #89", 1, null, null, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000005a"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #90 for Alice Johnson.", 2, null, "DEMO-1-0090", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #90", 1, null, null, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000005b"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #91 for Alice Johnson.", 2, null, "DEMO-1-0091", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #91", 1, null, null, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-00000000005c"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #92 for Alice Johnson.", 2, null, "DEMO-1-0092", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #92", 1, null, null, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000005d"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #93 for Alice Johnson.", 2, null, "DEMO-1-0093", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #93", 1, null, null, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000005e"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #94 for Alice Johnson.", 2, null, "DEMO-1-0094", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #94", 1, null, null, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-00000000005f"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #95 for Alice Johnson.", 2, null, "DEMO-1-0095", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #95", 1, null, null, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000060"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #96 for Alice Johnson.", 2, null, "DEMO-1-0096", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #96", 1, null, null, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000061"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #97 for Alice Johnson.", 2, null, "DEMO-1-0097", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #97", 1, null, null, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000062"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #98 for Alice Johnson.", 2, null, "DEMO-1-0098", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #98", 1, null, null, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000063"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #99 for Alice Johnson.", 2, null, "DEMO-1-0099", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #99", 1, null, null, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000064"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #100 for Alice Johnson.", 2, null, "DEMO-1-0100", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #100", 1, null, null, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("72000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #1 for Brian Carter.", 2, null, "DEMO-2-0001", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #1", 1, null, null, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-000000000002"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #2 for Brian Carter.", 2, null, "DEMO-2-0002", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #2", 1, null, null, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #3 for Brian Carter.", 2, null, "DEMO-2-0003", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #3", 1, null, null, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000004"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #4 for Brian Carter.", 2, null, "DEMO-2-0004", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #4", 1, null, null, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000005"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #5 for Brian Carter.", 2, null, "DEMO-2-0005", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #5", 1, null, null, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000006"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #6 for Brian Carter.", 2, null, "DEMO-2-0006", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #6", 1, null, null, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000007"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #7 for Brian Carter.", 2, null, "DEMO-2-0007", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #7", 1, null, null, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000008"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #8 for Brian Carter.", 2, null, "DEMO-2-0008", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #8", 1, null, null, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000009"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #9 for Brian Carter.", 2, null, "DEMO-2-0009", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #9", 1, null, null, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000000a"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #10 for Brian Carter.", 2, null, "DEMO-2-0010", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #10", 1, null, null, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-00000000000b"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #11 for Brian Carter.", 2, null, "DEMO-2-0011", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #11", 1, null, null, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000000c"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #12 for Brian Carter.", 2, null, "DEMO-2-0012", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #12", 1, null, null, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000000d"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #13 for Brian Carter.", 2, null, "DEMO-2-0013", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #13", 1, null, null, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000000e"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #14 for Brian Carter.", 2, null, "DEMO-2-0014", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #14", 1, null, null, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000000f"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #15 for Brian Carter.", 2, null, "DEMO-2-0015", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #15", 1, null, null, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000010"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #16 for Brian Carter.", 2, null, "DEMO-2-0016", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #16", 1, null, null, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-000000000011"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #17 for Brian Carter.", 2, null, "DEMO-2-0017", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #17", 1, null, null, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000012"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #18 for Brian Carter.", 2, null, "DEMO-2-0018", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #18", 1, null, null, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000013"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #19 for Brian Carter.", 2, null, "DEMO-2-0019", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #19", 1, null, null, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000014"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #20 for Brian Carter.", 2, null, "DEMO-2-0020", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #20", 1, null, null, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000015"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #21 for Brian Carter.", 2, null, "DEMO-2-0021", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #21", 1, null, null, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000016"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #22 for Brian Carter.", 2, null, "DEMO-2-0022", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #22", 1, null, null, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000017"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #23 for Brian Carter.", 2, null, "DEMO-2-0023", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #23", 1, null, null, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000018"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #24 for Brian Carter.", 2, null, "DEMO-2-0024", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #24", 1, null, null, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000019"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #25 for Brian Carter.", 2, null, "DEMO-2-0025", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #25", 1, null, null, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-00000000001a"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #26 for Brian Carter.", 2, null, "DEMO-2-0026", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #26", 1, null, null, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000001b"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #27 for Brian Carter.", 2, null, "DEMO-2-0027", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #27", 1, null, null, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000001c"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #28 for Brian Carter.", 2, null, "DEMO-2-0028", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #28", 1, null, null, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000001d"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #29 for Brian Carter.", 2, null, "DEMO-2-0029", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #29", 1, null, null, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000001e"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #30 for Brian Carter.", 2, null, "DEMO-2-0030", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #30", 1, null, null, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000001f"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #31 for Brian Carter.", 2, null, "DEMO-2-0031", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #31", 1, null, null, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-000000000020"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #32 for Brian Carter.", 2, null, "DEMO-2-0032", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #32", 1, null, null, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000021"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #33 for Brian Carter.", 2, null, "DEMO-2-0033", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #33", 1, null, null, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000022"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #34 for Brian Carter.", 2, null, "DEMO-2-0034", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #34", 1, null, null, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000023"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #35 for Brian Carter.", 2, null, "DEMO-2-0035", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #35", 1, null, null, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000024"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #36 for Brian Carter.", 2, null, "DEMO-2-0036", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #36", 1, null, null, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000025"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #37 for Brian Carter.", 2, null, "DEMO-2-0037", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #37", 1, null, null, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000026"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #38 for Brian Carter.", 2, null, "DEMO-2-0038", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #38", 1, null, null, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000027"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #39 for Brian Carter.", 2, null, "DEMO-2-0039", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #39", 1, null, null, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000028"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #40 for Brian Carter.", 2, null, "DEMO-2-0040", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #40", 1, null, null, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-000000000029"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #41 for Brian Carter.", 2, null, "DEMO-2-0041", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #41", 1, null, null, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000002a"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #42 for Brian Carter.", 2, null, "DEMO-2-0042", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #42", 1, null, null, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000002b"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #43 for Brian Carter.", 2, null, "DEMO-2-0043", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #43", 1, null, null, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000002c"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #44 for Brian Carter.", 2, null, "DEMO-2-0044", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #44", 1, null, null, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000002d"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #45 for Brian Carter.", 2, null, "DEMO-2-0045", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #45", 1, null, null, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000002e"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #46 for Brian Carter.", 2, null, "DEMO-2-0046", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #46", 1, null, null, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-00000000002f"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #47 for Brian Carter.", 2, null, "DEMO-2-0047", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #47", 1, null, null, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000030"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #48 for Brian Carter.", 2, null, "DEMO-2-0048", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #48", 1, null, null, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000031"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #49 for Brian Carter.", 2, null, "DEMO-2-0049", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #49", 1, null, null, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000032"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #50 for Brian Carter.", 2, null, "DEMO-2-0050", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #50", 1, null, null, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000033"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #51 for Brian Carter.", 2, null, "DEMO-2-0051", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #51", 1, null, null, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000034"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #52 for Brian Carter.", 2, null, "DEMO-2-0052", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #52", 1, null, null, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000035"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #53 for Brian Carter.", 2, null, "DEMO-2-0053", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #53", 1, null, null, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000036"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #54 for Brian Carter.", 2, null, "DEMO-2-0054", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #54", 1, null, null, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000037"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #55 for Brian Carter.", 2, null, "DEMO-2-0055", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #55", 1, null, null, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-000000000038"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #56 for Brian Carter.", 2, null, "DEMO-2-0056", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #56", 1, null, null, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000039"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #57 for Brian Carter.", 2, null, "DEMO-2-0057", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #57", 1, null, null, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000003a"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #58 for Brian Carter.", 2, null, "DEMO-2-0058", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #58", 1, null, null, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000003b"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #59 for Brian Carter.", 2, null, "DEMO-2-0059", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #59", 1, null, null, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000003c"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #60 for Brian Carter.", 2, null, "DEMO-2-0060", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #60", 1, null, null, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000003d"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #61 for Brian Carter.", 2, null, "DEMO-2-0061", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #61", 1, null, null, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-00000000003e"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #62 for Brian Carter.", 2, null, "DEMO-2-0062", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #62", 1, null, null, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000003f"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #63 for Brian Carter.", 2, null, "DEMO-2-0063", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #63", 1, null, null, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000040"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #64 for Brian Carter.", 2, null, "DEMO-2-0064", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #64", 1, null, null, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000041"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #65 for Brian Carter.", 2, null, "DEMO-2-0065", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #65", 1, null, null, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000042"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #66 for Brian Carter.", 2, null, "DEMO-2-0066", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #66", 1, null, null, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000043"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #67 for Brian Carter.", 2, null, "DEMO-2-0067", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #67", 1, null, null, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000044"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #68 for Brian Carter.", 2, null, "DEMO-2-0068", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #68", 1, null, null, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000045"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #69 for Brian Carter.", 2, null, "DEMO-2-0069", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #69", 1, null, null, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000046"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #70 for Brian Carter.", 2, null, "DEMO-2-0070", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #70", 1, null, null, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-000000000047"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #71 for Brian Carter.", 2, null, "DEMO-2-0071", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #71", 1, null, null, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000048"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #72 for Brian Carter.", 2, null, "DEMO-2-0072", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #72", 1, null, null, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000049"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #73 for Brian Carter.", 2, null, "DEMO-2-0073", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #73", 1, null, null, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000004a"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #74 for Brian Carter.", 2, null, "DEMO-2-0074", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #74", 1, null, null, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000004b"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #75 for Brian Carter.", 2, null, "DEMO-2-0075", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #75", 1, null, null, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000004c"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #76 for Brian Carter.", 2, null, "DEMO-2-0076", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #76", 1, null, null, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-00000000004d"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #77 for Brian Carter.", 2, null, "DEMO-2-0077", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #77", 1, null, null, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000004e"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #78 for Brian Carter.", 2, null, "DEMO-2-0078", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #78", 1, null, null, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000004f"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #79 for Brian Carter.", 2, null, "DEMO-2-0079", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #79", 1, null, null, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000050"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #80 for Brian Carter.", 2, null, "DEMO-2-0080", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #80", 1, null, null, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000051"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #81 for Brian Carter.", 2, null, "DEMO-2-0081", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #81", 1, null, null, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000052"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #82 for Brian Carter.", 2, null, "DEMO-2-0082", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #82", 1, null, null, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000053"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #83 for Brian Carter.", 2, null, "DEMO-2-0083", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #83", 1, null, null, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000054"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #84 for Brian Carter.", 2, null, "DEMO-2-0084", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #84", 1, null, null, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000055"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #85 for Brian Carter.", 2, null, "DEMO-2-0085", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #85", 1, null, null, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-000000000056"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #86 for Brian Carter.", 2, null, "DEMO-2-0086", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #86", 1, null, null, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000057"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #87 for Brian Carter.", 2, null, "DEMO-2-0087", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #87", 1, null, null, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000058"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #88 for Brian Carter.", 2, null, "DEMO-2-0088", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #88", 1, null, null, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-000000000059"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #89 for Brian Carter.", 2, null, "DEMO-2-0089", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #89", 1, null, null, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000005a"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #90 for Brian Carter.", 2, null, "DEMO-2-0090", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #90", 1, null, null, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000005b"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #91 for Brian Carter.", 2, null, "DEMO-2-0091", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #91", 1, null, null, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-00000000005c"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #92 for Brian Carter.", 2, null, "DEMO-2-0092", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #92", 1, null, null, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000005d"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #93 for Brian Carter.", 2, null, "DEMO-2-0093", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #93", 1, null, null, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000005e"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #94 for Brian Carter.", 2, null, "DEMO-2-0094", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #94", 1, null, null, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-00000000005f"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #95 for Brian Carter.", 2, null, "DEMO-2-0095", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #95", 1, null, null, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000060"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #96 for Brian Carter.", 2, null, "DEMO-2-0096", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #96", 1, null, null, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000061"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #97 for Brian Carter.", 2, null, "DEMO-2-0097", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #97", 1, null, null, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000062"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #98 for Brian Carter.", 2, null, "DEMO-2-0098", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #98", 1, null, null, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000063"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #99 for Brian Carter.", 2, null, "DEMO-2-0099", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #99", 1, null, null, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000064"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #100 for Brian Carter.", 2, null, "DEMO-2-0100", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #100", 1, null, null, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("73000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #1 for Cecilia Gomez.", 2, null, "DEMO-3-0001", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #1", 1, null, null, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #2 for Cecilia Gomez.", 2, null, "DEMO-3-0002", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #2", 1, null, null, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #3 for Cecilia Gomez.", 2, null, "DEMO-3-0003", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #3", 1, null, null, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000004"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #4 for Cecilia Gomez.", 2, null, "DEMO-3-0004", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #4", 1, null, null, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000005"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #5 for Cecilia Gomez.", 2, null, "DEMO-3-0005", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #5", 1, null, null, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000006"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #6 for Cecilia Gomez.", 2, null, "DEMO-3-0006", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #6", 1, null, null, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000007"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #7 for Cecilia Gomez.", 2, null, "DEMO-3-0007", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #7", 1, null, null, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000008"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #8 for Cecilia Gomez.", 2, null, "DEMO-3-0008", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #8", 1, null, null, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000009"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #9 for Cecilia Gomez.", 2, null, "DEMO-3-0009", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #9", 1, null, null, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000000a"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #10 for Cecilia Gomez.", 2, null, "DEMO-3-0010", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #10", 1, null, null, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-00000000000b"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #11 for Cecilia Gomez.", 2, null, "DEMO-3-0011", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #11", 1, null, null, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000000c"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #12 for Cecilia Gomez.", 2, null, "DEMO-3-0012", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #12", 1, null, null, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000000d"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #13 for Cecilia Gomez.", 2, null, "DEMO-3-0013", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #13", 1, null, null, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000000e"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #14 for Cecilia Gomez.", 2, null, "DEMO-3-0014", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #14", 1, null, null, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000000f"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #15 for Cecilia Gomez.", 2, null, "DEMO-3-0015", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #15", 1, null, null, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000010"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #16 for Cecilia Gomez.", 2, null, "DEMO-3-0016", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #16", 1, null, null, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-000000000011"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #17 for Cecilia Gomez.", 2, null, "DEMO-3-0017", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #17", 1, null, null, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000012"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #18 for Cecilia Gomez.", 2, null, "DEMO-3-0018", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #18", 1, null, null, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000013"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #19 for Cecilia Gomez.", 2, null, "DEMO-3-0019", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #19", 1, null, null, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000014"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #20 for Cecilia Gomez.", 2, null, "DEMO-3-0020", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #20", 1, null, null, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000015"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #21 for Cecilia Gomez.", 2, null, "DEMO-3-0021", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #21", 1, null, null, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000016"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #22 for Cecilia Gomez.", 2, null, "DEMO-3-0022", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #22", 1, null, null, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000017"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #23 for Cecilia Gomez.", 2, null, "DEMO-3-0023", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #23", 1, null, null, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000018"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #24 for Cecilia Gomez.", 2, null, "DEMO-3-0024", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #24", 1, null, null, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000019"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #25 for Cecilia Gomez.", 2, null, "DEMO-3-0025", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #25", 1, null, null, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-00000000001a"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #26 for Cecilia Gomez.", 2, null, "DEMO-3-0026", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #26", 1, null, null, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000001b"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #27 for Cecilia Gomez.", 2, null, "DEMO-3-0027", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #27", 1, null, null, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000001c"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #28 for Cecilia Gomez.", 2, null, "DEMO-3-0028", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #28", 1, null, null, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000001d"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #29 for Cecilia Gomez.", 2, null, "DEMO-3-0029", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #29", 1, null, null, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000001e"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #30 for Cecilia Gomez.", 2, null, "DEMO-3-0030", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #30", 1, null, null, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000001f"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #31 for Cecilia Gomez.", 2, null, "DEMO-3-0031", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #31", 1, null, null, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-000000000020"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #32 for Cecilia Gomez.", 2, null, "DEMO-3-0032", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #32", 1, null, null, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000021"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #33 for Cecilia Gomez.", 2, null, "DEMO-3-0033", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #33", 1, null, null, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000022"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #34 for Cecilia Gomez.", 2, null, "DEMO-3-0034", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #34", 1, null, null, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000023"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #35 for Cecilia Gomez.", 2, null, "DEMO-3-0035", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #35", 1, null, null, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000024"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #36 for Cecilia Gomez.", 2, null, "DEMO-3-0036", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #36", 1, null, null, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000025"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #37 for Cecilia Gomez.", 2, null, "DEMO-3-0037", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #37", 1, null, null, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000026"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #38 for Cecilia Gomez.", 2, null, "DEMO-3-0038", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #38", 1, null, null, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000027"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #39 for Cecilia Gomez.", 2, null, "DEMO-3-0039", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #39", 1, null, null, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000028"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #40 for Cecilia Gomez.", 2, null, "DEMO-3-0040", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #40", 1, null, null, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-000000000029"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #41 for Cecilia Gomez.", 2, null, "DEMO-3-0041", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #41", 1, null, null, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000002a"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #42 for Cecilia Gomez.", 2, null, "DEMO-3-0042", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #42", 1, null, null, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000002b"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #43 for Cecilia Gomez.", 2, null, "DEMO-3-0043", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #43", 1, null, null, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000002c"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #44 for Cecilia Gomez.", 2, null, "DEMO-3-0044", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #44", 1, null, null, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000002d"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #45 for Cecilia Gomez.", 2, null, "DEMO-3-0045", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #45", 1, null, null, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000002e"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #46 for Cecilia Gomez.", 2, null, "DEMO-3-0046", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #46", 1, null, null, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-00000000002f"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #47 for Cecilia Gomez.", 2, null, "DEMO-3-0047", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #47", 1, null, null, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000030"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #48 for Cecilia Gomez.", 2, null, "DEMO-3-0048", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #48", 1, null, null, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000031"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #49 for Cecilia Gomez.", 2, null, "DEMO-3-0049", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #49", 1, null, null, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000032"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #50 for Cecilia Gomez.", 2, null, "DEMO-3-0050", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #50", 1, null, null, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000033"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #51 for Cecilia Gomez.", 2, null, "DEMO-3-0051", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #51", 1, null, null, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000034"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #52 for Cecilia Gomez.", 2, null, "DEMO-3-0052", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #52", 1, null, null, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000035"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #53 for Cecilia Gomez.", 2, null, "DEMO-3-0053", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #53", 1, null, null, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000036"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #54 for Cecilia Gomez.", 2, null, "DEMO-3-0054", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #54", 1, null, null, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000037"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #55 for Cecilia Gomez.", 2, null, "DEMO-3-0055", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #55", 1, null, null, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-000000000038"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #56 for Cecilia Gomez.", 2, null, "DEMO-3-0056", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #56", 1, null, null, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000039"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #57 for Cecilia Gomez.", 2, null, "DEMO-3-0057", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #57", 1, null, null, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000003a"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #58 for Cecilia Gomez.", 2, null, "DEMO-3-0058", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #58", 1, null, null, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000003b"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #59 for Cecilia Gomez.", 2, null, "DEMO-3-0059", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #59", 1, null, null, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000003c"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #60 for Cecilia Gomez.", 2, null, "DEMO-3-0060", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #60", 1, null, null, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000003d"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #61 for Cecilia Gomez.", 2, null, "DEMO-3-0061", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #61", 1, null, null, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-00000000003e"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #62 for Cecilia Gomez.", 2, null, "DEMO-3-0062", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #62", 1, null, null, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000003f"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #63 for Cecilia Gomez.", 2, null, "DEMO-3-0063", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #63", 1, null, null, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000040"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #64 for Cecilia Gomez.", 2, null, "DEMO-3-0064", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #64", 1, null, null, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000041"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #65 for Cecilia Gomez.", 2, null, "DEMO-3-0065", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #65", 1, null, null, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000042"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #66 for Cecilia Gomez.", 2, null, "DEMO-3-0066", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #66", 1, null, null, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000043"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #67 for Cecilia Gomez.", 2, null, "DEMO-3-0067", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #67", 1, null, null, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000044"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #68 for Cecilia Gomez.", 2, null, "DEMO-3-0068", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #68", 1, null, null, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000045"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #69 for Cecilia Gomez.", 2, null, "DEMO-3-0069", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #69", 1, null, null, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000046"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #70 for Cecilia Gomez.", 2, null, "DEMO-3-0070", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #70", 1, null, null, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-000000000047"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #71 for Cecilia Gomez.", 2, null, "DEMO-3-0071", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #71", 1, null, null, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000048"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #72 for Cecilia Gomez.", 2, null, "DEMO-3-0072", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #72", 1, null, null, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000049"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #73 for Cecilia Gomez.", 2, null, "DEMO-3-0073", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #73", 1, null, null, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000004a"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #74 for Cecilia Gomez.", 2, null, "DEMO-3-0074", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #74", 1, null, null, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000004b"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #75 for Cecilia Gomez.", 2, null, "DEMO-3-0075", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #75", 1, null, null, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000004c"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #76 for Cecilia Gomez.", 2, null, "DEMO-3-0076", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #76", 1, null, null, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-00000000004d"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #77 for Cecilia Gomez.", 2, null, "DEMO-3-0077", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #77", 1, null, null, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000004e"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #78 for Cecilia Gomez.", 2, null, "DEMO-3-0078", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #78", 1, null, null, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000004f"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #79 for Cecilia Gomez.", 2, null, "DEMO-3-0079", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #79", 1, null, null, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000050"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #80 for Cecilia Gomez.", 2, null, "DEMO-3-0080", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #80", 1, null, null, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000051"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #81 for Cecilia Gomez.", 2, null, "DEMO-3-0081", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #81", 1, null, null, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000052"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #82 for Cecilia Gomez.", 2, null, "DEMO-3-0082", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #82", 1, null, null, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000053"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #83 for Cecilia Gomez.", 2, null, "DEMO-3-0083", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #83", 1, null, null, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000054"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #84 for Cecilia Gomez.", 2, null, "DEMO-3-0084", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #84", 1, null, null, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000055"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #85 for Cecilia Gomez.", 2, null, "DEMO-3-0085", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #85", 1, null, null, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-000000000056"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #86 for Cecilia Gomez.", 2, null, "DEMO-3-0086", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #86", 1, null, null, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000057"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #87 for Cecilia Gomez.", 2, null, "DEMO-3-0087", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #87", 1, null, null, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000058"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #88 for Cecilia Gomez.", 2, null, "DEMO-3-0088", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #88", 1, null, null, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-000000000059"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #89 for Cecilia Gomez.", 2, null, "DEMO-3-0089", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #89", 1, null, null, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000005a"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #90 for Cecilia Gomez.", 2, null, "DEMO-3-0090", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #90", 1, null, null, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000005b"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #91 for Cecilia Gomez.", 2, null, "DEMO-3-0091", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #91", 1, null, null, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-00000000005c"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #92 for Cecilia Gomez.", 2, null, "DEMO-3-0092", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #92", 1, null, null, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000005d"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #93 for Cecilia Gomez.", 2, null, "DEMO-3-0093", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #93", 1, null, null, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000005e"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #94 for Cecilia Gomez.", 2, null, "DEMO-3-0094", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #94", 1, null, null, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-00000000005f"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #95 for Cecilia Gomez.", 2, null, "DEMO-3-0095", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #95", 1, null, null, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000060"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #96 for Cecilia Gomez.", 2, null, "DEMO-3-0096", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #96", 1, null, null, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000061"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #97 for Cecilia Gomez.", 2, null, "DEMO-3-0097", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #97", 1, null, null, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000062"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #98 for Cecilia Gomez.", 2, null, "DEMO-3-0098", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #98", 1, null, null, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000063"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #99 for Cecilia Gomez.", 2, null, "DEMO-3-0099", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #99", 1, null, null, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000064"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #100 for Cecilia Gomez.", 2, null, "DEMO-3-0100", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #100", 1, null, null, 54.99m, 5, true, 54.99m, 49.49m }
                });

            migrationBuilder.InsertData(
                table: "listing_template",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "FormatLabel", "IsDeleted", "Name", "PayloadJson", "ThumbnailUrl", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("81000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", "Reusable template seeded for demo purposes.", "Fixed Price", false, "Alice's Starter Template", "{\"title\":\"Sample Listing Template\",\"price\":49.99,\"quantity\":5,\"categoryId\":\"10000000-0000-0000-0000-000000000002\",\"conditionId\":\"40000000-0000-0000-0000-000000000001\"}", "https://picsum.photos/seed/template-1/320/180", null, null },
                    { new Guid("82000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", "Reusable template seeded for demo purposes.", "Fixed Price", false, "Brian's Starter Template", "{\"title\":\"Sample Listing Template\",\"price\":49.99,\"quantity\":5,\"categoryId\":\"10000000-0000-0000-0000-000000000003\",\"conditionId\":\"40000000-0000-0000-0000-000000000001\"}", "https://picsum.photos/seed/template-2/320/180", null, null },
                    { new Guid("83000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", "Reusable template seeded for demo purposes.", "Fixed Price", false, "Cecilia's Starter Template", "{\"title\":\"Sample Listing Template\",\"price\":49.99,\"quantity\":5,\"categoryId\":\"10000000-0000-0000-0000-000000000004\",\"conditionId\":\"40000000-0000-0000-0000-000000000001\"}", "https://picsum.photos/seed/template-3/320/180", null, null }
                });

            migrationBuilder.InsertData(
                table: "order_statuses",
                columns: new[] { "Id", "Code", "Color", "Description", "Name", "SortOrder" },
                values: new object[,]
                {
                    { new Guid("0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e"), "Archived", "#64748b", "Order archived", "Archived", 11 },
                    { new Guid("2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1"), "Draft", "#94a3b8", "Order created but not submitted", "Draft", 0 },
                    { new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), "AwaitingShipmentShipWithin24h", "#fbbf24", "Must ship within 24 hours", "Ship within 24h", 4 },
                    { new Guid("4d128ab1-64a7-4c65-b8f5-434a258f0c52"), "AwaitingPayment", "#fb923c", "Order awaits buyer payment", "Awaiting payment", 1 },
                    { new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), "PaidAndShipped", "#10b981", "Order shipped to buyer", "Paid & shipped", 6 },
                    { new Guid("5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9"), "PaidAwaitingFeedback", "#a855f7", "Waiting for buyer feedback", "Awaiting feedback", 7 },
                    { new Guid("859b47f4-0d05-4f43-8ff5-57acb8d5da1d"), "AwaitingExpeditedShipment", "#22c55e", "Expedited shipping requested", "Expedited shipment", 5 },
                    { new Guid("970c8d97-6081-43db-9083-8f3c026ded84"), "DeliveryFailed", "#f97316", "Delivery attempt unsuccessful", "Delivery failed", 10 },
                    { new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), "AwaitingShipment", "#3b82f6", "Payment received; waiting to ship", "Awaiting shipment", 2 },
                    { new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd"), "Cancelled", "#ef4444", "Order cancelled", "Cancelled", 12 },
                    { new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930"), "ShippedAwaitingFeedback", "#38bdf8", "Shipped and awaiting buyer confirmation", "Shipped - awaiting feedback", 8 },
                    { new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), "AwaitingShipmentOverdue", "#ef4444", "Shipment overdue based on handling time", "Shipment overdue", 3 }
                });

            migrationBuilder.InsertData(
                table: "shipping_services",
                columns: new[] { "Id", "Carrier", "CoverageDescription", "CreatedAt", "CreatedBy", "DeliveryWindowLabel", "IsDeleted", "MaxEstimatedDeliveryDays", "MinEstimatedDeliveryDays", "Notes", "PrinterRequired", "SavingsDescription", "ServiceCode", "ServiceName", "Slug", "UpdatedAt", "UpdatedBy", "BaseCost_Amount", "BaseCost_Currency" },
                values: new object[] { new Guid("5a4af094-9a6b-4d6f-9a19-9b5360f0a6ec"), "UPS", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 28 - Apr 2", false, 6, 3, "Reliable ground service - Includes tracking", true, "On eBay you save 21%", "UPS_GROUND", "UPS Ground", "ups-ground", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 15.62m, "USD" });

            migrationBuilder.InsertData(
                table: "shipping_services",
                columns: new[] { "Id", "Carrier", "CoverageDescription", "CreatedAt", "CreatedBy", "DeliveryWindowLabel", "IsDeleted", "MaxEstimatedDeliveryDays", "MinEstimatedDeliveryDays", "Notes", "SavingsDescription", "ServiceCode", "ServiceName", "Slug", "SupportsQrCode", "UpdatedAt", "UpdatedBy", "BaseCost_Amount", "BaseCost_Currency" },
                values: new object[] { new Guid("6f7e3c0f-2bc6-4f1b-aa0b-4c1a9f76f950"), "USPS", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 28 - Apr 1", false, 5, 3, "Max weight 70 lb - Max dimensions 130\" (length + girth)", "On eBay you save 28%", "USPS_GROUND_ADVANTAGE", "USPS Ground Advantage", "usps-ground", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 11.45m, "USD" });

            migrationBuilder.InsertData(
                table: "shipping_services",
                columns: new[] { "Id", "Carrier", "CoverageDescription", "CreatedAt", "CreatedBy", "DeliveryWindowLabel", "IsDeleted", "MaxEstimatedDeliveryDays", "MinEstimatedDeliveryDays", "Notes", "PrinterRequired", "SavingsDescription", "ServiceCode", "ServiceName", "Slug", "UpdatedAt", "UpdatedBy", "BaseCost_Amount", "BaseCost_Currency" },
                values: new object[,]
                {
                    { new Guid("9e1f84fd-8c9c-459d-b2c5-bf6e47668f5d"), "FedEx", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 29 - Apr 3", false, 7, 4, "2-5 business days - Ideal for small parcels", true, "On eBay you save 18%", "FEDEX_GROUND_ECONOMY", "FedEx Ground Economy", "fedex-ground", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 14.10m, "USD" },
                    { new Guid("a1d9551e-5c5c-4ca6-9a0e-1aa855b77af7"), "USPS", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 27 - 31", false, 4, 2, "Legal-size documents - Insured up to $100", true, "On eBay you save 12%", "USPS_PRIORITY_MAIL_FLAT_RATE_LEGAL_ENVELOPE", "USPS Priority Mail Flat Rate Legal Envelope", "usps-priority-legal", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 9.05m, "USD" },
                    { new Guid("c1d3c7f4-6ac1-4a7f-8a29-6dbaf9ecbb51"), "USPS", "Up to $100.00", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", "Mar 27 - 31", false, 4, 2, "Best for documents - Includes tracking", true, "On eBay you save 13%", "USPS_PRIORITY_MAIL_FLAT_RATE_ENVELOPE", "USPS Priority Mail Flat Rate Envelope", "usps-priority-flat", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "seed", 8.75m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Email", "FullName", "IsDeleted", "IsEmailVerified", "IsPaymentVerified", "PasswordHash", "PerformanceLevel", "UpdatedAt", "UpdatedBy", "Username", "_activeTotalValue" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "demo.seller1@example.com", "Alice Johnson", false, true, true, "$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq", "TopRated", null, null, "demo.seller1@example.com", 11222.00m },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "demo.seller2@example.com", "Brian Carter", false, true, true, "$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq", "TopRated", null, null, "demo.seller2@example.com", 13622.00m },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "demo.seller3@example.com", "Cecilia Gomez", false, true, true, "$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq", "TopRated", null, null, "demo.seller3@example.com", 16022.00m }
                });

            migrationBuilder.InsertData(
                table: "ListingId",
                columns: new[] { "Value", "seller_id" },
                values: new object[,]
                {
                    { new Guid("71000000-0000-0000-0000-000000000001"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000002"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000003"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000004"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000005"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000006"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000007"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000008"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000009"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000000a"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000000b"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000000c"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000000d"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000000e"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000000f"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000010"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000011"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000012"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000013"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000014"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000015"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000016"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000017"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000018"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000019"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000001a"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000001b"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000001c"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000001d"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000001e"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000001f"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000020"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000021"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000022"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000023"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000024"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000025"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000026"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000027"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000028"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000029"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000002a"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000002b"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000002c"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000002d"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000002e"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000002f"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000030"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000031"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000032"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000033"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000034"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000035"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000036"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000037"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000038"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000039"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000003a"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000003b"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000003c"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000003d"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000003e"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000003f"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000040"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000041"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000042"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000043"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000044"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000045"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000046"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000047"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000048"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000049"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000004a"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000004b"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000004c"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000004d"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000004e"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000004f"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000050"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000051"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000052"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000053"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000054"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000055"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000056"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000057"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000058"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000059"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000005a"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000005b"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000005c"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000005d"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000005e"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-00000000005f"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000060"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000061"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000062"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000063"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("71000000-0000-0000-0000-000000000064"), new Guid("70000000-0000-0000-0000-000000000001") },
                    { new Guid("72000000-0000-0000-0000-000000000001"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000002"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000003"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000004"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000005"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000006"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000007"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000008"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000009"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000000a"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000000b"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000000c"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000000d"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000000e"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000000f"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000010"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000011"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000012"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000013"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000014"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000015"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000016"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000017"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000018"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000019"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000001a"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000001b"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000001c"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000001d"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000001e"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000001f"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000020"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000021"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000022"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000023"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000024"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000025"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000026"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000027"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000028"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000029"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000002a"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000002b"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000002c"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000002d"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000002e"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000002f"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000030"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000031"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000032"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000033"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000034"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000035"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000036"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000037"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000038"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000039"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000003a"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000003b"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000003c"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000003d"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000003e"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000003f"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000040"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000041"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000042"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000043"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000044"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000045"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000046"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000047"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000048"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000049"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000004a"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000004b"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000004c"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000004d"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000004e"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000004f"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000050"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000051"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000052"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000053"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000054"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000055"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000056"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000057"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000058"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000059"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000005a"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000005b"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000005c"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000005d"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000005e"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-00000000005f"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000060"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000061"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000062"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000063"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("72000000-0000-0000-0000-000000000064"), new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("73000000-0000-0000-0000-000000000001"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000002"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000003"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000004"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000005"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000006"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000007"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000008"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000009"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000000a"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000000b"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000000c"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000000d"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000000e"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000000f"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000010"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000011"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000012"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000013"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000014"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000015"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000016"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000017"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000018"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000019"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000001a"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000001b"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000001c"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000001d"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000001e"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000001f"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000020"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000021"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000022"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000023"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000024"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000025"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000026"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000027"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000028"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000029"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000002a"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000002b"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000002c"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000002d"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000002e"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000002f"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000030"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000031"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000032"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000033"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000034"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000035"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000036"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000037"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000038"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000039"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000003a"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000003b"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000003c"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000003d"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000003e"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000003f"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000040"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000041"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000042"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000043"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000044"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000045"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000046"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000047"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000048"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000049"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000004a"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000004b"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000004c"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000004d"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000004e"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000004f"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000050"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000051"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000052"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000053"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000054"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000055"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000056"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000057"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000058"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000059"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000005a"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000005b"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000005c"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000005d"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000005e"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-00000000005f"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000060"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000061"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000062"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000063"), new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("73000000-0000-0000-0000-000000000064"), new Guid("70000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsDeleted", "Name", "ParentId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Smartphones and cell phone devices.", false, "Cell Phones & Smartphones", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Portable computers and accessories.", false, "Laptops & Netbooks", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Digital cameras and photography equipment.", false, "Cameras & Photo", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Televisions, speakers, and streaming devices.", false, "TV, Video & Home Audio", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Home and handheld gaming systems.", false, "Video Game Consoles", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Smartwatches, fitness trackers, and smart eyewear.", false, "Wearable Technology", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Connected home devices and automation hubs.", false, "Smart Home", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Navigation, dash cams, and in-car entertainment.", false, "Vehicle Electronics & GPS", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Performance and casual athletic footwear for men.", false, "Men's Athletic Shoes", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dresses for every style and occasion.", false, "Women's Dresses", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Casual, business, and formal apparel for men.", false, "Men's Clothing", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Designer totes, crossbody bags, and backpacks.", false, "Women's Handbags & Bags", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Heels, flats, and casual footwear.", false, "Women's Shoes", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Timepieces ranging from vintage to luxury.", false, "Watches", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Rings, necklaces, and bracelets crafted in precious metals.", false, "Fine Jewelry", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Countertop appliances and kitchen helpers.", false, "Small Kitchen Appliances", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Indoor and outdoor furniture collections.", false, "Furniture", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Interior accents, wall art, and lighting.", false, "Home Décor", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Power tools and shop essentials.", false, "Tools & Workshop Equipment", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Outdoor décor, landscaping, and patio gear.", false, "Yard, Garden & Outdoor Living", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Building supplies, fixtures, and hardware.", false, "Home Improvement", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "OEM and aftermarket components for every ride.", false, "Car Parts & Accessories", new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Upgrades and replacement parts for bikes.", false, "Motorcycle Parts", new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Garage lifts, diagnostics, and specialty tools.", false, "Automotive Tools & Supplies", new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Rims, tire sets, TPMS sensors, and more.", false, "Wheels, Tires & Parts", new Guid("50000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "TCG singles, sealed product, and memorabilia.", false, "Collectible Card Games", new Guid("60000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Golden Age through modern runs and collectibles.", false, "Comics & Graphic Novels", new Guid("60000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Limited editions, lithographs, and posters.", false, "Art Prints", new Guid("60000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Graded coins, bullion, and currency.", false, "Coins & Paper Money", new Guid("60000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Superheroes, anime, and pop-culture icons.", false, "Action Figures", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Locomotives, rolling stock, and scenery kits.", false, "Model Railroads & Trains", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Radio-controlled cars, drones, and planes.", false, "RC Model Vehicles & Kits", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Barbie, Blythe, Build-A-Bear, and more.", false, "Dolls & Bears", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Modular builds and sealed collectible sets.", false, "LEGO & Building Toys", new Guid("70000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Camping, hiking, hunting, and fishing gear.", false, "Outdoor Sports", new Guid("80000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Exercise machines, apparel, and accessories.", false, "Fitness, Running & Yoga", new Guid("80000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bikes, parts, helmets, and apparel.", false, "Cycling", new Guid("80000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Clubs, balls, carts, and training aids.", false, "Golf", new Guid("80000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cosmetics, palettes, and tools.", false, "Makeup", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Serums, moisturizers, and devices.", false, "Skin Care", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Wellness, immunity, and performance blends.", false, "Vitamins & Dietary Supplements", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Styling tools, treatments, and color.", false, "Hair Care", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Perfumes, colognes, and body mists.", false, "Fragrances", new Guid("90000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Excavators, loaders, and industrial vehicles.", false, "Heavy Equipment", new Guid("a0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Maintenance, repair, and operations essentials.", false, "MRO & Industrial Supplies", new Guid("a0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Point-of-sale, signage, and consulting packages.", false, "Retail & Services", new Guid("a0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Printers, copiers, and office machines.", false, "Office Equipment", new Guid("a0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Electric, acoustic, and bass guitars.", false, "Guitars & Basses", new Guid("b0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Mixers, microphones, and studio gear.", false, "Pro Audio Equipment", new Guid("b0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Controllers, turntables, and lighting.", false, "DJ Equipment", new Guid("b0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Saxes, trumpets, clarinets, and accessories.", false, "Brass & Woodwind", new Guid("b0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Beds, crates, and training essentials.", false, "Dog Supplies", new Guid("c0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Litter, scratchers, and cat furniture.", false, "Cat Supplies", new Guid("c0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Aquariums, filtration, and décor.", false, "Fish & Aquarium", new Guid("c0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Habitat accessories for hamsters, rabbits, and more.", false, "Small Animal Supplies", new Guid("c0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lightweight, jogging, and convertible options.", false, "Strollers & Travel Systems", new Guid("d0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cribs, dressers, and gliders.", false, "Nursery Furniture", new Guid("d0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Monitors, gates, and proofing essentials.", false, "Baby Safety", new Guid("d0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("d0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bottles, warmers, and nursing support.", false, "Baby Feeding", new Guid("d0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Stamps, dies, and embellishments.", false, "Scrapbooking & Paper Crafting", new Guid("e0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Paints, canvases, and studio tools.", false, "Art Supplies", new Guid("e0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Yardage, quilting, and upholstery textiles.", false, "Fabric", new Guid("e0000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("e0000000-0000-0000-0000-000000000005"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Findings, gemstones, and tools.", false, "Beads & Jewelry Making", new Guid("e0000000-0000-0000-0000-000000000001"), null, null }
                });

            migrationBuilder.InsertData(
                table: "listing_image",
                columns: new[] { "Id", "listing_id", "IsPrimary", "Url" },
                values: new object[,]
                {
                    { 1, new Guid("71000000-0000-0000-0000-000000000001"), true, "https://picsum.photos/seed/1-1/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000002"), true, "https://picsum.photos/seed/1-2/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000003"), true, "https://picsum.photos/seed/1-3/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000004"), true, "https://picsum.photos/seed/1-4/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000005"), true, "https://picsum.photos/seed/1-5/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000006"), true, "https://picsum.photos/seed/1-6/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000007"), true, "https://picsum.photos/seed/1-7/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000008"), true, "https://picsum.photos/seed/1-8/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000009"), true, "https://picsum.photos/seed/1-9/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000000a"), true, "https://picsum.photos/seed/1-10/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000000b"), true, "https://picsum.photos/seed/1-11/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000000c"), true, "https://picsum.photos/seed/1-12/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000000d"), true, "https://picsum.photos/seed/1-13/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000000e"), true, "https://picsum.photos/seed/1-14/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000000f"), true, "https://picsum.photos/seed/1-15/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000010"), true, "https://picsum.photos/seed/1-16/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000011"), true, "https://picsum.photos/seed/1-17/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000012"), true, "https://picsum.photos/seed/1-18/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000013"), true, "https://picsum.photos/seed/1-19/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000014"), true, "https://picsum.photos/seed/1-20/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000015"), true, "https://picsum.photos/seed/1-21/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000016"), true, "https://picsum.photos/seed/1-22/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000017"), true, "https://picsum.photos/seed/1-23/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000018"), true, "https://picsum.photos/seed/1-24/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000019"), true, "https://picsum.photos/seed/1-25/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000001a"), true, "https://picsum.photos/seed/1-26/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000001b"), true, "https://picsum.photos/seed/1-27/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000001c"), true, "https://picsum.photos/seed/1-28/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000001d"), true, "https://picsum.photos/seed/1-29/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000001e"), true, "https://picsum.photos/seed/1-30/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000001f"), true, "https://picsum.photos/seed/1-31/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000020"), true, "https://picsum.photos/seed/1-32/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000021"), true, "https://picsum.photos/seed/1-33/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000022"), true, "https://picsum.photos/seed/1-34/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000023"), true, "https://picsum.photos/seed/1-35/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000024"), true, "https://picsum.photos/seed/1-36/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000025"), true, "https://picsum.photos/seed/1-37/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000026"), true, "https://picsum.photos/seed/1-38/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000027"), true, "https://picsum.photos/seed/1-39/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000028"), true, "https://picsum.photos/seed/1-40/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000029"), true, "https://picsum.photos/seed/1-41/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000002a"), true, "https://picsum.photos/seed/1-42/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000002b"), true, "https://picsum.photos/seed/1-43/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000002c"), true, "https://picsum.photos/seed/1-44/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000002d"), true, "https://picsum.photos/seed/1-45/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000002e"), true, "https://picsum.photos/seed/1-46/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000002f"), true, "https://picsum.photos/seed/1-47/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000030"), true, "https://picsum.photos/seed/1-48/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000031"), true, "https://picsum.photos/seed/1-49/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000032"), true, "https://picsum.photos/seed/1-50/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000033"), true, "https://picsum.photos/seed/1-51/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000034"), true, "https://picsum.photos/seed/1-52/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000035"), true, "https://picsum.photos/seed/1-53/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000036"), true, "https://picsum.photos/seed/1-54/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000037"), true, "https://picsum.photos/seed/1-55/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000038"), true, "https://picsum.photos/seed/1-56/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000039"), true, "https://picsum.photos/seed/1-57/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000003a"), true, "https://picsum.photos/seed/1-58/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000003b"), true, "https://picsum.photos/seed/1-59/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000003c"), true, "https://picsum.photos/seed/1-60/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000003d"), true, "https://picsum.photos/seed/1-61/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000003e"), true, "https://picsum.photos/seed/1-62/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000003f"), true, "https://picsum.photos/seed/1-63/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000040"), true, "https://picsum.photos/seed/1-64/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000041"), true, "https://picsum.photos/seed/1-65/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000042"), true, "https://picsum.photos/seed/1-66/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000043"), true, "https://picsum.photos/seed/1-67/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000044"), true, "https://picsum.photos/seed/1-68/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000045"), true, "https://picsum.photos/seed/1-69/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000046"), true, "https://picsum.photos/seed/1-70/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000047"), true, "https://picsum.photos/seed/1-71/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000048"), true, "https://picsum.photos/seed/1-72/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000049"), true, "https://picsum.photos/seed/1-73/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000004a"), true, "https://picsum.photos/seed/1-74/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000004b"), true, "https://picsum.photos/seed/1-75/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000004c"), true, "https://picsum.photos/seed/1-76/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000004d"), true, "https://picsum.photos/seed/1-77/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000004e"), true, "https://picsum.photos/seed/1-78/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000004f"), true, "https://picsum.photos/seed/1-79/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000050"), true, "https://picsum.photos/seed/1-80/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000051"), true, "https://picsum.photos/seed/1-81/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000052"), true, "https://picsum.photos/seed/1-82/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000053"), true, "https://picsum.photos/seed/1-83/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000054"), true, "https://picsum.photos/seed/1-84/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000055"), true, "https://picsum.photos/seed/1-85/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000056"), true, "https://picsum.photos/seed/1-86/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000057"), true, "https://picsum.photos/seed/1-87/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000058"), true, "https://picsum.photos/seed/1-88/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000059"), true, "https://picsum.photos/seed/1-89/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000005a"), true, "https://picsum.photos/seed/1-90/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000005b"), true, "https://picsum.photos/seed/1-91/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000005c"), true, "https://picsum.photos/seed/1-92/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000005d"), true, "https://picsum.photos/seed/1-93/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000005e"), true, "https://picsum.photos/seed/1-94/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-00000000005f"), true, "https://picsum.photos/seed/1-95/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000060"), true, "https://picsum.photos/seed/1-96/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000061"), true, "https://picsum.photos/seed/1-97/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000062"), true, "https://picsum.photos/seed/1-98/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000063"), true, "https://picsum.photos/seed/1-99/640/640" },
                    { 1, new Guid("71000000-0000-0000-0000-000000000064"), true, "https://picsum.photos/seed/1-100/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000001"), true, "https://picsum.photos/seed/2-1/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000002"), true, "https://picsum.photos/seed/2-2/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000003"), true, "https://picsum.photos/seed/2-3/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000004"), true, "https://picsum.photos/seed/2-4/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000005"), true, "https://picsum.photos/seed/2-5/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000006"), true, "https://picsum.photos/seed/2-6/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000007"), true, "https://picsum.photos/seed/2-7/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000008"), true, "https://picsum.photos/seed/2-8/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000009"), true, "https://picsum.photos/seed/2-9/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000000a"), true, "https://picsum.photos/seed/2-10/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000000b"), true, "https://picsum.photos/seed/2-11/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000000c"), true, "https://picsum.photos/seed/2-12/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000000d"), true, "https://picsum.photos/seed/2-13/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000000e"), true, "https://picsum.photos/seed/2-14/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000000f"), true, "https://picsum.photos/seed/2-15/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000010"), true, "https://picsum.photos/seed/2-16/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000011"), true, "https://picsum.photos/seed/2-17/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000012"), true, "https://picsum.photos/seed/2-18/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000013"), true, "https://picsum.photos/seed/2-19/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000014"), true, "https://picsum.photos/seed/2-20/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000015"), true, "https://picsum.photos/seed/2-21/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000016"), true, "https://picsum.photos/seed/2-22/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000017"), true, "https://picsum.photos/seed/2-23/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000018"), true, "https://picsum.photos/seed/2-24/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000019"), true, "https://picsum.photos/seed/2-25/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000001a"), true, "https://picsum.photos/seed/2-26/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000001b"), true, "https://picsum.photos/seed/2-27/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000001c"), true, "https://picsum.photos/seed/2-28/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000001d"), true, "https://picsum.photos/seed/2-29/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000001e"), true, "https://picsum.photos/seed/2-30/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000001f"), true, "https://picsum.photos/seed/2-31/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000020"), true, "https://picsum.photos/seed/2-32/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000021"), true, "https://picsum.photos/seed/2-33/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000022"), true, "https://picsum.photos/seed/2-34/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000023"), true, "https://picsum.photos/seed/2-35/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000024"), true, "https://picsum.photos/seed/2-36/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000025"), true, "https://picsum.photos/seed/2-37/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000026"), true, "https://picsum.photos/seed/2-38/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000027"), true, "https://picsum.photos/seed/2-39/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000028"), true, "https://picsum.photos/seed/2-40/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000029"), true, "https://picsum.photos/seed/2-41/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000002a"), true, "https://picsum.photos/seed/2-42/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000002b"), true, "https://picsum.photos/seed/2-43/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000002c"), true, "https://picsum.photos/seed/2-44/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000002d"), true, "https://picsum.photos/seed/2-45/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000002e"), true, "https://picsum.photos/seed/2-46/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000002f"), true, "https://picsum.photos/seed/2-47/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000030"), true, "https://picsum.photos/seed/2-48/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000031"), true, "https://picsum.photos/seed/2-49/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000032"), true, "https://picsum.photos/seed/2-50/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000033"), true, "https://picsum.photos/seed/2-51/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000034"), true, "https://picsum.photos/seed/2-52/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000035"), true, "https://picsum.photos/seed/2-53/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000036"), true, "https://picsum.photos/seed/2-54/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000037"), true, "https://picsum.photos/seed/2-55/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000038"), true, "https://picsum.photos/seed/2-56/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000039"), true, "https://picsum.photos/seed/2-57/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000003a"), true, "https://picsum.photos/seed/2-58/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000003b"), true, "https://picsum.photos/seed/2-59/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000003c"), true, "https://picsum.photos/seed/2-60/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000003d"), true, "https://picsum.photos/seed/2-61/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000003e"), true, "https://picsum.photos/seed/2-62/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000003f"), true, "https://picsum.photos/seed/2-63/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000040"), true, "https://picsum.photos/seed/2-64/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000041"), true, "https://picsum.photos/seed/2-65/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000042"), true, "https://picsum.photos/seed/2-66/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000043"), true, "https://picsum.photos/seed/2-67/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000044"), true, "https://picsum.photos/seed/2-68/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000045"), true, "https://picsum.photos/seed/2-69/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000046"), true, "https://picsum.photos/seed/2-70/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000047"), true, "https://picsum.photos/seed/2-71/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000048"), true, "https://picsum.photos/seed/2-72/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000049"), true, "https://picsum.photos/seed/2-73/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000004a"), true, "https://picsum.photos/seed/2-74/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000004b"), true, "https://picsum.photos/seed/2-75/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000004c"), true, "https://picsum.photos/seed/2-76/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000004d"), true, "https://picsum.photos/seed/2-77/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000004e"), true, "https://picsum.photos/seed/2-78/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000004f"), true, "https://picsum.photos/seed/2-79/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000050"), true, "https://picsum.photos/seed/2-80/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000051"), true, "https://picsum.photos/seed/2-81/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000052"), true, "https://picsum.photos/seed/2-82/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000053"), true, "https://picsum.photos/seed/2-83/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000054"), true, "https://picsum.photos/seed/2-84/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000055"), true, "https://picsum.photos/seed/2-85/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000056"), true, "https://picsum.photos/seed/2-86/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000057"), true, "https://picsum.photos/seed/2-87/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000058"), true, "https://picsum.photos/seed/2-88/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000059"), true, "https://picsum.photos/seed/2-89/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000005a"), true, "https://picsum.photos/seed/2-90/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000005b"), true, "https://picsum.photos/seed/2-91/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000005c"), true, "https://picsum.photos/seed/2-92/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000005d"), true, "https://picsum.photos/seed/2-93/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000005e"), true, "https://picsum.photos/seed/2-94/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-00000000005f"), true, "https://picsum.photos/seed/2-95/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000060"), true, "https://picsum.photos/seed/2-96/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000061"), true, "https://picsum.photos/seed/2-97/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000062"), true, "https://picsum.photos/seed/2-98/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000063"), true, "https://picsum.photos/seed/2-99/640/640" },
                    { 1, new Guid("72000000-0000-0000-0000-000000000064"), true, "https://picsum.photos/seed/2-100/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000001"), true, "https://picsum.photos/seed/3-1/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000002"), true, "https://picsum.photos/seed/3-2/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000003"), true, "https://picsum.photos/seed/3-3/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000004"), true, "https://picsum.photos/seed/3-4/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000005"), true, "https://picsum.photos/seed/3-5/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000006"), true, "https://picsum.photos/seed/3-6/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000007"), true, "https://picsum.photos/seed/3-7/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000008"), true, "https://picsum.photos/seed/3-8/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000009"), true, "https://picsum.photos/seed/3-9/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000000a"), true, "https://picsum.photos/seed/3-10/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000000b"), true, "https://picsum.photos/seed/3-11/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000000c"), true, "https://picsum.photos/seed/3-12/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000000d"), true, "https://picsum.photos/seed/3-13/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000000e"), true, "https://picsum.photos/seed/3-14/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000000f"), true, "https://picsum.photos/seed/3-15/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000010"), true, "https://picsum.photos/seed/3-16/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000011"), true, "https://picsum.photos/seed/3-17/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000012"), true, "https://picsum.photos/seed/3-18/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000013"), true, "https://picsum.photos/seed/3-19/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000014"), true, "https://picsum.photos/seed/3-20/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000015"), true, "https://picsum.photos/seed/3-21/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000016"), true, "https://picsum.photos/seed/3-22/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000017"), true, "https://picsum.photos/seed/3-23/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000018"), true, "https://picsum.photos/seed/3-24/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000019"), true, "https://picsum.photos/seed/3-25/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000001a"), true, "https://picsum.photos/seed/3-26/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000001b"), true, "https://picsum.photos/seed/3-27/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000001c"), true, "https://picsum.photos/seed/3-28/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000001d"), true, "https://picsum.photos/seed/3-29/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000001e"), true, "https://picsum.photos/seed/3-30/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000001f"), true, "https://picsum.photos/seed/3-31/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000020"), true, "https://picsum.photos/seed/3-32/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000021"), true, "https://picsum.photos/seed/3-33/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000022"), true, "https://picsum.photos/seed/3-34/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000023"), true, "https://picsum.photos/seed/3-35/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000024"), true, "https://picsum.photos/seed/3-36/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000025"), true, "https://picsum.photos/seed/3-37/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000026"), true, "https://picsum.photos/seed/3-38/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000027"), true, "https://picsum.photos/seed/3-39/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000028"), true, "https://picsum.photos/seed/3-40/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000029"), true, "https://picsum.photos/seed/3-41/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000002a"), true, "https://picsum.photos/seed/3-42/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000002b"), true, "https://picsum.photos/seed/3-43/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000002c"), true, "https://picsum.photos/seed/3-44/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000002d"), true, "https://picsum.photos/seed/3-45/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000002e"), true, "https://picsum.photos/seed/3-46/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000002f"), true, "https://picsum.photos/seed/3-47/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000030"), true, "https://picsum.photos/seed/3-48/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000031"), true, "https://picsum.photos/seed/3-49/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000032"), true, "https://picsum.photos/seed/3-50/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000033"), true, "https://picsum.photos/seed/3-51/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000034"), true, "https://picsum.photos/seed/3-52/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000035"), true, "https://picsum.photos/seed/3-53/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000036"), true, "https://picsum.photos/seed/3-54/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000037"), true, "https://picsum.photos/seed/3-55/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000038"), true, "https://picsum.photos/seed/3-56/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000039"), true, "https://picsum.photos/seed/3-57/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000003a"), true, "https://picsum.photos/seed/3-58/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000003b"), true, "https://picsum.photos/seed/3-59/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000003c"), true, "https://picsum.photos/seed/3-60/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000003d"), true, "https://picsum.photos/seed/3-61/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000003e"), true, "https://picsum.photos/seed/3-62/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000003f"), true, "https://picsum.photos/seed/3-63/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000040"), true, "https://picsum.photos/seed/3-64/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000041"), true, "https://picsum.photos/seed/3-65/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000042"), true, "https://picsum.photos/seed/3-66/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000043"), true, "https://picsum.photos/seed/3-67/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000044"), true, "https://picsum.photos/seed/3-68/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000045"), true, "https://picsum.photos/seed/3-69/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000046"), true, "https://picsum.photos/seed/3-70/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000047"), true, "https://picsum.photos/seed/3-71/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000048"), true, "https://picsum.photos/seed/3-72/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000049"), true, "https://picsum.photos/seed/3-73/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000004a"), true, "https://picsum.photos/seed/3-74/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000004b"), true, "https://picsum.photos/seed/3-75/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000004c"), true, "https://picsum.photos/seed/3-76/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000004d"), true, "https://picsum.photos/seed/3-77/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000004e"), true, "https://picsum.photos/seed/3-78/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000004f"), true, "https://picsum.photos/seed/3-79/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000050"), true, "https://picsum.photos/seed/3-80/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000051"), true, "https://picsum.photos/seed/3-81/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000052"), true, "https://picsum.photos/seed/3-82/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000053"), true, "https://picsum.photos/seed/3-83/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000054"), true, "https://picsum.photos/seed/3-84/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000055"), true, "https://picsum.photos/seed/3-85/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000056"), true, "https://picsum.photos/seed/3-86/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000057"), true, "https://picsum.photos/seed/3-87/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000058"), true, "https://picsum.photos/seed/3-88/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000059"), true, "https://picsum.photos/seed/3-89/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000005a"), true, "https://picsum.photos/seed/3-90/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000005b"), true, "https://picsum.photos/seed/3-91/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000005c"), true, "https://picsum.photos/seed/3-92/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000005d"), true, "https://picsum.photos/seed/3-93/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000005e"), true, "https://picsum.photos/seed/3-94/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-00000000005f"), true, "https://picsum.photos/seed/3-95/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000060"), true, "https://picsum.photos/seed/3-96/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000061"), true, "https://picsum.photos/seed/3-97/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000062"), true, "https://picsum.photos/seed/3-98/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000063"), true, "https://picsum.photos/seed/3-99/640/640" },
                    { 1, new Guid("73000000-0000-0000-0000-000000000064"), true, "https://picsum.photos/seed/3-100/640/640" }
                });

            migrationBuilder.InsertData(
                table: "order_status_transitions",
                columns: new[] { "Id", "AllowedRoles", "FromStatusId", "ToStatusId" },
                values: new object[,]
                {
                    { new Guid("1bd31fd1-5a79-4a8e-9035-7cbc71dbb8b9"), "SYSTEM", new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91") },
                    { new Guid("2abdffad-037d-48a0-8c3d-a8dd0f00c5ba"), "SELLER,SYSTEM", new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8") },
                    { new Guid("3334f1c8-0fb7-4b17-974a-16f4f492ade4"), "SYSTEM", new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad") },
                    { new Guid("3cf5a7f5-8f3f-4dcb-907e-e4d27744ef98"), "SELLER,SYSTEM", new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd") },
                    { new Guid("42059f6f-8e43-4b6a-9b59-cf9670091b8f"), "SYSTEM", new Guid("4d128ab1-64a7-4c65-b8f5-434a258f0c52"), new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91") },
                    { new Guid("55b5fadc-7f2f-4f43-ac4c-c6eb6f633d58"), "SELLER,SYSTEM", new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8") },
                    { new Guid("5a3f5769-6c6d-4b89-9347-118bd3fba3d6"), "SYSTEM", new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad") },
                    { new Guid("5c76de08-97eb-43d5-9c01-8c7c6262ec66"), "SELLER,BUYER,SYSTEM", new Guid("4d128ab1-64a7-4c65-b8f5-434a258f0c52"), new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd") },
                    { new Guid("64648c83-2c87-47b8-8c2a-32e96c369f41"), "SELLER,SYSTEM", new Guid("859b47f4-0d05-4f43-8ff5-57acb8d5da1d"), new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8") },
                    { new Guid("6cb6fa65-3d6c-45f0-9f27-cf5d292743ff"), "SELLER,SUPPORT,SYSTEM", new Guid("970c8d97-6081-43db-9083-8f3c026ded84"), new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91") },
                    { new Guid("6fbe36c4-98e4-4d1d-8c3c-1ea29fe8d08c"), "SYSTEM", new Guid("5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9"), new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930") },
                    { new Guid("7cf6e659-8025-49e8-94d5-3a4dd3b5a793"), "SYSTEM", new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060") },
                    { new Guid("8ac18f4b-ea8d-4b72-b6cf-01c3d233cbea"), "SELLER,SYSTEM", new Guid("2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1"), new Guid("4d128ab1-64a7-4c65-b8f5-434a258f0c52") },
                    { new Guid("8c6f6f3e-18c6-4aa5-ba61-033fa3c0bb0e"), "SYSTEM", new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91") },
                    { new Guid("94b12ce3-6d7c-4ea1-86f9-72f65e75d8de"), "SYSTEM", new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new Guid("859b47f4-0d05-4f43-8ff5-57acb8d5da1d") },
                    { new Guid("a4c5df71-b5bb-4f13-9659-a5047cf4f087"), "SELLER,SUPPORT,SYSTEM", new Guid("970c8d97-6081-43db-9083-8f3c026ded84"), new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd") },
                    { new Guid("b62c4a77-6a54-47d9-8d09-af22bd0caf23"), "SELLER,SYSTEM", new Guid("2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1"), new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd") },
                    { new Guid("b8fa2c60-13ad-4e83-9516-8f406bcf8414"), "SELLER,SUPPORT,SYSTEM", new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new Guid("970c8d97-6081-43db-9083-8f3c026ded84") },
                    { new Guid("c6a927ee-4fb6-48cc-bbf0-d2624de3458f"), "SYSTEM", new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new Guid("5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9") },
                    { new Guid("ce68729c-6df0-466b-ae26-737d1b10dd93"), "SYSTEM", new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930"), new Guid("0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e") },
                    { new Guid("d0cb2575-023a-45dc-840a-8e09b2f4c4c8"), "SYSTEM", new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930") },
                    { new Guid("d10a4517-efbb-4b8d-af6f-baf2b022a850"), "SYSTEM", new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060") },
                    { new Guid("ee0a6840-bf0b-46f3-9c41-96b5a91a02ab"), "SELLER,SYSTEM", new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8") },
                    { new Guid("f5ceb762-3d65-4f6d-b052-053c55c1a08d"), "SYSTEM", new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd"), new Guid("0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e") }
                });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "Id", "ArchivedAt", "BuyerId", "CancelledAt", "CouponCode", "CreatedAt", "CreatedBy", "DeliveredAt", "FulfillmentType", "IsDeleted", "OrderNumber", "OrderedAt", "PaidAt", "PromotionId", "SellerId", "ShippedAt", "ShippingStatus", "StatusId", "UpdatedAt", "UpdatedBy", "discount_amount", "discount_currency", "platform_fee_amount", "platform_fee_currency", "shipping_cost_amount", "shipping_cost_currency", "sub_total_amount", "sub_total_currency", "tax_amount", "tax_currency", "total_amount", "total_currency" },
                values: new object[,]
                {
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000011"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 1, 9, 15, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1011", new DateTime(2025, 11, 1, 9, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 1, 9, 50, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 1, 9, 50, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.10m, "USD", 9.95m, "USD", 58.99m, "USD", 5.30m, "USD", 77.34m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000012"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 2, 13, 30, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1012", new DateTime(2025, 11, 2, 13, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 2, 14, 10, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 2, 14, 10, 0, 0, DateTimeKind.Utc), "seed", 2.50m, "USD", 3.45m, "USD", 8.25m, "USD", 64.50m, "USD", 4.86m, "USD", 78.56m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000013"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 3, 17, 5, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1013", new DateTime(2025, 11, 3, 17, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 3, 17, 55, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 3, 17, 55, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.90m, "USD", 10.00m, "USD", 72.00m, "USD", 6.12m, "USD", 92.02m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000014"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 11, 4, 8, 45, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1014", new DateTime(2025, 11, 4, 8, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 4, 9, 13, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 4, 9, 13, 0, 0, DateTimeKind.Utc), "seed", 1.20m, "USD", 2.95m, "USD", 7.80m, "USD", 55.75m, "USD", 4.46m, "USD", 69.76m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000015"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 5, 10, 0, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1015", new DateTime(2025, 11, 5, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 5, 10, 30, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 5, 10, 30, 0, 0, DateTimeKind.Utc), "seed", 2.50m, "USD", 3.15m, "USD", 8.25m, "USD", 62.75m, "USD", 5.02m, "USD", 76.67m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000016"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 5, 11, 30, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1016", new DateTime(2025, 11, 5, 11, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 5, 11, 55, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 5, 11, 55, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 2.45m, "USD", 7.50m, "USD", 48.40m, "USD", 3.87m, "USD", 62.22m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000017"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 6, 9, 20, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1017", new DateTime(2025, 11, 6, 9, 20, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 6, 9, 40, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 6, 9, 40, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.95m, "USD", 9.95m, "USD", 79.90m, "USD", 6.39m, "USD", 100.19m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000018"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 11, 6, 13, 45, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1018", new DateTime(2025, 11, 6, 13, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 6, 14, 20, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 6, 14, 20, 0, 0, DateTimeKind.Utc), "seed", 3.00m, "USD", 4.30m, "USD", 10.25m, "USD", 88.60m, "USD", 7.08m, "USD", 107.23m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-000000000019"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 11, 7, 8, 10, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1019", new DateTime(2025, 11, 7, 8, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 7, 8, 28, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 7, 8, 28, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.55m, "USD", 8.75m, "USD", 71.25m, "USD", 5.70m, "USD", 89.25m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001a"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 7, 15, 25, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1020", new DateTime(2025, 11, 7, 15, 25, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 7, 15, 47, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 7, 15, 47, 0, 0, DateTimeKind.Utc), "seed", 1.80m, "USD", 3.25m, "USD", 8.40m, "USD", 65.80m, "USD", 4.94m, "USD", 80.59m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001b"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 8, 10, 40, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1021", new DateTime(2025, 11, 8, 10, 40, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 8, 11, 7, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 8, 11, 7, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 2.95m, "USD", 7.95m, "USD", 59.10m, "USD", 4.43m, "USD", 74.43m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001c"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 8, 14, 5, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1022", new DateTime(2025, 11, 8, 14, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 8, 14, 37, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 8, 14, 37, 0, 0, DateTimeKind.Utc), "seed", 2.20m, "USD", 3.80m, "USD", 9.10m, "USD", 83.45m, "USD", 6.68m, "USD", 100.83m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001d"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 11, 9, 11, 15, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1023", new DateTime(2025, 11, 9, 11, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 11, 39, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 9, 11, 39, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.45m, "USD", 10.60m, "USD", 90.30m, "USD", 7.22m, "USD", 112.57m, "USD" },
                    { new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001e"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 11, 9, 17, 50, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1024", new DateTime(2025, 11, 9, 17, 50, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 18, 19, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 11, 9, 18, 19, 0, 0, DateTimeKind.Utc), "seed", 1.50m, "USD", 3.60m, "USD", 8.90m, "USD", 74.95m, "USD", 5.83m, "USD", 91.78m, "USD" },
                    { new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, "HOLIDAY10", new DateTime(2025, 10, 28, 12, 10, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 10, 31, 12, 10, 0, 0, DateTimeKind.Utc), 0, false, "ORD-SEED-1009", new DateTime(2025, 10, 28, 12, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 28, 13, 10, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 28, 22, 10, 0, 0, DateTimeKind.Utc), 2, new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930"), new DateTime(2025, 10, 31, 12, 10, 0, 0, DateTimeKind.Utc), "seed", 7.50m, "USD", 6.20m, "USD", 14.00m, "USD", 145.97m, "USD", 10.60m, "USD", 169.27m, "USD" },
                    { new Guid("1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4"), null, new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 21, 16, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 10, 20, 16, 0, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1004", new DateTime(2025, 10, 20, 16, 0, 0, 0, DateTimeKind.Utc), null, null, new Guid("70000000-0000-0000-0000-000000000001"), null, 6, new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd"), new DateTime(2025, 10, 21, 16, 0, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 3.25m, "USD", 0.00m, "USD", 74.98m, "USD", 5.50m, "USD", 83.73m, "USD" },
                    { new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1003", new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 18, 10, 45, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), new DateTime(2025, 10, 18, 10, 45, 0, 0, DateTimeKind.Utc), "seed", 2.00m, "USD", 3.40m, "USD", 9.25m, "USD", 70.98m, "USD", 6.20m, "USD", 87.83m, "USD" },
                    { new Guid("973cac8a-9be0-44a0-90b7-fd8263f8e78a"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, null, new DateTime(2025, 10, 24, 11, 5, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1006", new DateTime(2025, 10, 24, 11, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 24, 11, 50, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, 0, new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), new DateTime(2025, 10, 24, 11, 50, 0, 0, DateTimeKind.Utc), "seed", 4.00m, "USD", 3.99m, "USD", 11.00m, "USD", 79.98m, "USD", 6.40m, "USD", 97.37m, "USD" },
                    { new Guid("a4206ad5-6a35-43bb-8a8c-8c7b244594ac"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1008", new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 26, 19, 40, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), null, 0, new Guid("859b47f4-0d05-4f43-8ff5-57acb8d5da1d"), new DateTime(2025, 10, 26, 19, 40, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.60m, "USD", 13.00m, "USD", 92.98m, "USD", 8.60m, "USD", 119.18m, "USD" },
                    { new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 25, 15, 30, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 10, 28, 15, 30, 0, 0, DateTimeKind.Utc), 0, false, "ORD-SEED-1007", new DateTime(2025, 10, 25, 15, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 25, 17, 30, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 26, 15, 30, 0, 0, DateTimeKind.Utc), 5, new Guid("5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9"), new DateTime(2025, 10, 28, 15, 30, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.20m, "USD", 12.50m, "USD", 84.98m, "USD", 7.20m, "USD", 108.88m, "USD" },
                    { new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), null, new Guid("70000000-0000-0000-0000-000000000003"), null, "OCTDEAL", new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1002", new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 15, 15, 15, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 16, 0, 15, 0, 0, DateTimeKind.Utc), 2, new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new DateTime(2025, 10, 16, 0, 15, 0, 0, DateTimeKind.Utc), "seed", 5.00m, "USD", 3.35m, "USD", 12.00m, "USD", 66.98m, "USD", 5.36m, "USD", 82.69m, "USD" },
                    { new Guid("d2ee4d4a-5be0-4d76-bce6-0b8578c87407"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 22, 8, 20, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1005", new DateTime(2025, 10, 22, 8, 20, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 22, 9, 20, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2025, 10, 22, 17, 20, 0, 0, DateTimeKind.Utc), 2, new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new DateTime(2025, 10, 22, 17, 20, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.10m, "USD", 10.00m, "USD", 76.98m, "USD", 6.16m, "USD", 97.24m, "USD" },
                    { new Guid("f6de3ce0-2d3d-4709-923d-cbb61f956947"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1001", new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 12, 12, 30, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 10, 12, 12, 30, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 4.70m, "USD", 8.50m, "USD", 93.97m, "USD", 7.52m, "USD", 114.69m, "USD" },
                    { new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), null, new Guid("70000000-0000-0000-0000-000000000002"), null, "BULKBUY", new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), 0, false, "ORD-SEED-1010", new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 30, 10, 5, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2025, 10, 30, 22, 5, 0, 0, DateTimeKind.Utc), 2, new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), "seed", 10.00m, "USD", 7.20m, "USD", 15.00m, "USD", 152.97m, "USD", 12.20m, "USD", 177.37m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "category_condition",
                columns: new[] { "CategoryId", "ConditionId" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000002") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("10000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("10000000-0000-0000-0000-000000000009"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("20000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000008"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000007"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000002") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000003") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("50000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("60000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000005") },
                    { new Guid("70000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("70000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("70000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("80000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("90000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("90000000-0000-0000-0000-000000000006"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("a0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("b0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("b0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("c0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("d0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("d0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("d0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("d0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("d0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("e0000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("e0000000-0000-0000-0000-000000000004"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("e0000000-0000-0000-0000-000000000005"), new Guid("40000000-0000-0000-0000-000000000006") }
                });

            migrationBuilder.InsertData(
                table: "category_specific",
                columns: new[] { "Id", "AllowMultiple", "IsRequired", "Name", "_values", "category_id" },
                values: new object[,]
                {
                    { new Guid("10100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Apple\",\"Samsung\",\"Google\",\"OnePlus\",\"Motorola\"]", new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("10100000-0000-0000-0000-000000000002"), false, false, "Model", "[\"iPhone 15\",\"Galaxy S24\",\"Pixel 8\",\"OnePlus 12\",\"Moto G Power\"]", new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("10100000-0000-0000-0000-000000000003"), false, true, "Storage Capacity", "[\"64 GB\",\"128 GB\",\"256 GB\",\"512 GB\",\"1 TB\"]", new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("10100000-0000-0000-0000-000000000004"), false, false, "Color", "[\"Black\",\"White\",\"Blue\",\"Red\",\"Purple\"]", new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("10100000-0000-0000-0000-000000000005"), false, false, "Network", "[\"Unlocked\",\"AT\\u0026T\",\"Verizon\",\"T-Mobile\",\"US Cellular\"]", new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("10200000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Apple\",\"Dell\",\"HP\",\"Lenovo\",\"ASUS\"]", new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10200000-0000-0000-0000-000000000002"), false, true, "Processor", "[\"Intel Core i5\",\"Intel Core i7\",\"AMD Ryzen 5\",\"AMD Ryzen 7\",\"Apple M2\"]", new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10200000-0000-0000-0000-000000000003"), false, true, "RAM Size", "[\"8 GB\",\"16 GB\",\"32 GB\",\"64 GB\"]", new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10200000-0000-0000-0000-000000000004"), false, false, "Storage Type", "[\"SSD\",\"HDD\",\"Hybrid\"]", new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10200000-0000-0000-0000-000000000005"), false, false, "Screen Size", "[\"13 in\",\"14 in\",\"15.6 in\",\"17 in\"]", new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("10300000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Canon\",\"Nikon\",\"Sony\",\"Fujifilm\",\"Panasonic\"]", new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("10300000-0000-0000-0000-000000000002"), false, true, "Camera Type", "[\"DSLR\",\"Mirrorless\",\"Point \\u0026 Shoot\",\"Action\"]", new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("10300000-0000-0000-0000-000000000003"), false, false, "Maximum Resolution", "[\"12 MP\",\"16 MP\",\"24 MP\",\"32 MP\"]", new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("10300000-0000-0000-0000-000000000004"), false, false, "Optical Zoom", "[\"None\",\"5x\",\"10x\",\"20x\"]", new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("10300000-0000-0000-0000-000000000005"), false, false, "Series", "[\"EOS\",\"Alpha\",\"X-T\",\"Lumix\"]", new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("10400000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"LG\",\"Samsung\",\"Sony\",\"TCL\",\"Vizio\"]", new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("10400000-0000-0000-0000-000000000002"), false, true, "Type", "[\"4K UHD TV\",\"Soundbar\",\"AV Receiver\",\"Streaming Device\"]", new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("10400000-0000-0000-0000-000000000003"), false, false, "Smart Platform", "[\"Google TV\",\"Roku\",\"Fire TV\",\"webOS\",\"Tizen\"]", new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("10500000-0000-0000-0000-000000000001"), false, true, "Platform", "[\"Nintendo\",\"PlayStation\",\"Xbox\",\"Steam Deck\"]", new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("10500000-0000-0000-0000-000000000002"), false, true, "Model", "[\"PlayStation 5\",\"Xbox Series X\",\"Nintendo Switch OLED\",\"Steam Deck OLED\"]", new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("10500000-0000-0000-0000-000000000003"), false, false, "Storage Capacity", "[\"64 GB\",\"512 GB\",\"1 TB\",\"2 TB\"]", new Guid("10000000-0000-0000-0000-000000000006") },
                    { new Guid("10600000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Apple\",\"Fitbit\",\"Garmin\",\"Samsung\",\"Withings\"]", new Guid("10000000-0000-0000-0000-000000000007") },
                    { new Guid("10600000-0000-0000-0000-000000000002"), true, false, "Features", "[\"GPS\",\"Heart Rate Monitor\",\"NFC\",\"SpO2\",\"Sleep Tracking\"]", new Guid("10000000-0000-0000-0000-000000000007") },
                    { new Guid("10700000-0000-0000-0000-000000000001"), false, true, "Device Type", "[\"Smart Speaker\",\"Smart Display\",\"Smart Lighting\",\"Smart Thermostat\",\"Security Camera\"]", new Guid("10000000-0000-0000-0000-000000000008") },
                    { new Guid("10700000-0000-0000-0000-000000000002"), false, false, "Ecosystem", "[\"Alexa\",\"Apple Home\",\"Google Home\",\"Matter\",\"SmartThings\"]", new Guid("10000000-0000-0000-0000-000000000008") },
                    { new Guid("10800000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Dash Cam\",\"GPS\",\"Car Stereo\",\"Backup Camera\",\"Radar Detector\"]", new Guid("10000000-0000-0000-0000-000000000009") },
                    { new Guid("10800000-0000-0000-0000-000000000002"), true, false, "Compatible Vehicle", "[\"Universal\",\"Ford\",\"GM\",\"Toyota\",\"Volkswagen\"]", new Guid("10000000-0000-0000-0000-000000000009") },
                    { new Guid("20100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Nike\",\"Adidas\",\"New Balance\",\"Puma\",\"Under Armour\"]", new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("20100000-0000-0000-0000-000000000002"), false, true, "US Shoe Size", "[\"7\",\"8\",\"9\",\"10\",\"11\",\"12\",\"13\"]", new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("20100000-0000-0000-0000-000000000003"), false, false, "Color", "[\"Black\",\"White\",\"Red\",\"Blue\",\"Gray\"]", new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("20100000-0000-0000-0000-000000000004"), false, false, "Style", "[\"Low Top\",\"Mid Top\",\"High Top\"]", new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("20100000-0000-0000-0000-000000000005"), false, false, "Width", "[\"Standard\",\"Wide\",\"Extra Wide\"]", new Guid("20000000-0000-0000-0000-000000000002") },
                    { new Guid("20200000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"ASOS\",\"Free People\",\"H\\u0026M\",\"Reformation\",\"Zara\"]", new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("20200000-0000-0000-0000-000000000002"), false, true, "Size Type", "[\"Regular\",\"Petite\",\"Plus\",\"Tall\"]", new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("20200000-0000-0000-0000-000000000003"), false, false, "Dress Length", "[\"Mini\",\"Knee Length\",\"Midi\",\"Maxi\"]", new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("20200000-0000-0000-0000-000000000004"), false, false, "Material", "[\"Cotton\",\"Linen\",\"Polyester\",\"Silk\"]", new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("20200000-0000-0000-0000-000000000005"), false, false, "Pattern", "[\"Solid\",\"Floral\",\"Striped\",\"Polka Dot\",\"Animal Print\"]", new Guid("20000000-0000-0000-0000-000000000003") },
                    { new Guid("20300000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Banana Republic\",\"Hugo Boss\",\"Levi\\u0027s\",\"Nike\",\"Ralph Lauren\"]", new Guid("20000000-0000-0000-0000-000000000004") },
                    { new Guid("20300000-0000-0000-0000-000000000002"), false, true, "Size", "[\"S\",\"M\",\"L\",\"XL\",\"XXL\"]", new Guid("20000000-0000-0000-0000-000000000004") },
                    { new Guid("20400000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Coach\",\"Gucci\",\"Kate Spade\",\"Louis Vuitton\",\"Tory Burch\"]", new Guid("20000000-0000-0000-0000-000000000005") },
                    { new Guid("20400000-0000-0000-0000-000000000002"), true, false, "Materials", "[\"Canvas\",\"Leather\",\"Nylon\",\"Patent Leather\",\"Vegan Leather\"]", new Guid("20000000-0000-0000-0000-000000000005") },
                    { new Guid("20400000-0000-0000-0000-000000000003"), false, false, "Style", "[\"Backpack\",\"Crossbody\",\"Satchel\",\"Shoulder Bag\",\"Tote\"]", new Guid("20000000-0000-0000-0000-000000000005") },
                    { new Guid("20500000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Birkenstock\",\"Clarks\",\"Dr. Martens\",\"Sam Edelman\",\"Steve Madden\"]", new Guid("20000000-0000-0000-0000-000000000006") },
                    { new Guid("20500000-0000-0000-0000-000000000002"), false, true, "US Shoe Size", "[\"5\",\"6\",\"7\",\"8\",\"9\",\"10\"]", new Guid("20000000-0000-0000-0000-000000000006") },
                    { new Guid("20500000-0000-0000-0000-000000000003"), false, false, "Style", "[\"Boots\",\"Flats\",\"Heels\",\"Sandals\",\"Sneakers\"]", new Guid("20000000-0000-0000-0000-000000000006") },
                    { new Guid("20600000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Casio\",\"Citizen\",\"Omega\",\"Rolex\",\"Seiko\"]", new Guid("20000000-0000-0000-0000-000000000007") },
                    { new Guid("20600000-0000-0000-0000-000000000002"), false, false, "Movement", "[\"Automatic\",\"Quartz\",\"Mechanical\",\"Solar\"]", new Guid("20000000-0000-0000-0000-000000000007") },
                    { new Guid("20700000-0000-0000-0000-000000000001"), false, true, "Metal", "[\"Gold\",\"Platinum\",\"Rose Gold\",\"Sterling Silver\",\"White Gold\"]", new Guid("20000000-0000-0000-0000-000000000008") },
                    { new Guid("20700000-0000-0000-0000-000000000002"), false, true, "Jewelry Type", "[\"Bracelet\",\"Earrings\",\"Necklace\",\"Ring\"]", new Guid("20000000-0000-0000-0000-000000000008") },
                    { new Guid("30100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Breville\",\"Cuisinart\",\"Instant Pot\",\"KitchenAid\",\"Ninja\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000002"), false, true, "Appliance Type", "[\"Air Fryer\",\"Blender\",\"Coffee Maker\",\"Mixer\",\"Pressure Cooker\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000003"), false, false, "Color", "[\"Black\",\"Red\",\"Silver\",\"Stainless Steel\",\"White\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000004"), false, false, "Power Source", "[\"Electric\",\"Battery\",\"Manual\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000005"), false, false, "Capacity", "[\"2 qt\",\"4 qt\",\"6 qt\",\"8 qt\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000006"), false, false, "Style", "[\"Bohemian\",\"Farmhouse\",\"Mid-Century\",\"Modern\",\"Traditional\"]", new Guid("30000000-0000-0000-0000-000000000004") },
                    { new Guid("30100000-0000-0000-0000-000000000007"), false, true, "Room", "[\"Bedroom\",\"Dining Room\",\"Kitchen\",\"Living Room\",\"Office\"]", new Guid("30000000-0000-0000-0000-000000000004") },
                    { new Guid("30200000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Ashley\",\"Crate \\u0026 Barrel\",\"IKEA\",\"West Elm\",\"Wayfair\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30200000-0000-0000-0000-000000000002"), false, true, "Room", "[\"Bedroom\",\"Dining Room\",\"Home Office\",\"Living Room\",\"Patio\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30200000-0000-0000-0000-000000000003"), false, false, "Material", "[\"Fabric\",\"Glass\",\"Leather\",\"Metal\",\"Wood\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30200000-0000-0000-0000-000000000004"), false, false, "Color", "[\"Black\",\"Gray\",\"Natural\",\"White\",\"Walnut\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30200000-0000-0000-0000-000000000005"), false, false, "Assembly Required", "[\"No\",\"Yes\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30300000-0000-0000-0000-000000000001"), false, false, "Brand", "[\"Bosch\",\"DeWalt\",\"Hilti\",\"Makita\",\"Milwaukee\"]", new Guid("30000000-0000-0000-0000-000000000005") },
                    { new Guid("30300000-0000-0000-0000-000000000002"), false, true, "Power Source", "[\"Battery\",\"Corded Electric\",\"Compressed Air\",\"Manual\"]", new Guid("30000000-0000-0000-0000-000000000005") },
                    { new Guid("30400000-0000-0000-0000-000000000001"), false, true, "Category", "[\"Flooring\",\"Hardware\",\"Lighting\",\"Plumbing\",\"Storage\"]", new Guid("30000000-0000-0000-0000-000000000007") },
                    { new Guid("30400000-0000-0000-0000-000000000002"), false, false, "Finish", "[\"Brushed Nickel\",\"Chrome\",\"Matte Black\",\"Oil-Rubbed Bronze\"]", new Guid("30000000-0000-0000-0000-000000000007") },
                    { new Guid("40100000-0000-0000-0000-000000000001"), false, true, "Part Type", "[\"Brakes\",\"Engine\",\"Exterior\",\"Interior\",\"Suspension\"]", new Guid("50000000-0000-0000-0000-000000000002") },
                    { new Guid("40100000-0000-0000-0000-000000000002"), false, false, "Brand", "[\"ACDelco\",\"Bosch\",\"Denso\",\"Mopar\",\"Motorcraft\"]", new Guid("50000000-0000-0000-0000-000000000002") },
                    { new Guid("40100000-0000-0000-0000-000000000003"), true, false, "Compatible Make", "[\"Chevrolet\",\"Ford\",\"Honda\",\"Toyota\",\"Universal\"]", new Guid("50000000-0000-0000-0000-000000000002") },
                    { new Guid("40200000-0000-0000-0000-000000000001"), false, true, "Part Type", "[\"Body \\u0026 Frame\",\"Drivetrain\",\"Electrical\",\"Engine\",\"Suspension\"]", new Guid("50000000-0000-0000-0000-000000000003") },
                    { new Guid("40300000-0000-0000-0000-000000000001"), false, true, "Tool Type", "[\"Diagnostic\",\"Hand Tool\",\"Lifts \\u0026 Jacks\",\"Power Tool\",\"Specialty\"]", new Guid("50000000-0000-0000-0000-000000000004") },
                    { new Guid("40400000-0000-0000-0000-000000000001"), false, true, "Tire Type", "[\"All-Season\",\"Performance\",\"Snow/Winter\",\"Off-Road\"]", new Guid("50000000-0000-0000-0000-000000000005") },
                    { new Guid("40400000-0000-0000-0000-000000000002"), false, false, "Rim Diameter", "[\"16 in\",\"17 in\",\"18 in\",\"19 in\",\"20 in\"]", new Guid("50000000-0000-0000-0000-000000000005") },
                    { new Guid("60100000-0000-0000-0000-000000000001"), false, true, "Franchise", "[\"Magic: The Gathering\",\"Pok\\u00E9mon\",\"Yu-Gi-Oh!\",\"Marvel Snap\",\"Disney Lorcana\"]", new Guid("60000000-0000-0000-0000-000000000002") },
                    { new Guid("60100000-0000-0000-0000-000000000002"), false, true, "Card Condition", "[\"Gem Mint\",\"Near Mint\",\"Lightly Played\",\"Moderately Played\",\"Heavily Played\"]", new Guid("60000000-0000-0000-0000-000000000002") },
                    { new Guid("60100000-0000-0000-0000-000000000003"), false, false, "Graded", "[\"BGS\",\"CGC\",\"PSA\",\"SGC\",\"Ungraded\"]", new Guid("60000000-0000-0000-0000-000000000002") },
                    { new Guid("60200000-0000-0000-0000-000000000001"), false, true, "Publisher", "[\"DC\",\"Dark Horse\",\"IDW\",\"Image\",\"Marvel\"]", new Guid("60000000-0000-0000-0000-000000000003") },
                    { new Guid("60200000-0000-0000-0000-000000000002"), false, false, "Era", "[\"Golden Age\",\"Silver Age\",\"Bronze Age\",\"Modern\"]", new Guid("60000000-0000-0000-0000-000000000003") },
                    { new Guid("60300000-0000-0000-0000-000000000001"), false, true, "Artist", "[\"Andy Warhol\",\"Banksy\",\"Jean-Michel Basquiat\",\"Salvador Dal\\u00ED\",\"Yoshitomo Nara\"]", new Guid("60000000-0000-0000-0000-000000000004") },
                    { new Guid("60300000-0000-0000-0000-000000000002"), false, false, "Medium", "[\"Gicl\\u00E9e\",\"Lithograph\",\"Screenprint\",\"Serigraph\"]", new Guid("60000000-0000-0000-0000-000000000004") },
                    { new Guid("60400000-0000-0000-0000-000000000001"), false, false, "Certification", "[\"ANACS\",\"NGC\",\"PCGS\",\"PMG\",\"Uncertified\"]", new Guid("60000000-0000-0000-0000-000000000005") },
                    { new Guid("60400000-0000-0000-0000-000000000002"), false, false, "Grade", "[\"MS 70\",\"MS 69\",\"MS 65\",\"AU 55\",\"XF 45\"]", new Guid("60000000-0000-0000-0000-000000000005") },
                    { new Guid("70100000-0000-0000-0000-000000000001"), false, true, "Franchise", "[\"DC\",\"Dragon Ball\",\"Marvel\",\"Star Wars\",\"Transformers\"]", new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("70100000-0000-0000-0000-000000000002"), false, false, "Scale", "[\"1:6\",\"1:12\",\"1:18\",\"6 in\",\"12 in\"]", new Guid("70000000-0000-0000-0000-000000000002") },
                    { new Guid("70200000-0000-0000-0000-000000000001"), false, true, "Scale", "[\"HO\",\"N\",\"O\",\"G\",\"Z\"]", new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("70200000-0000-0000-0000-000000000002"), false, false, "Power Type", "[\"DC\",\"DCC\",\"Battery\"]", new Guid("70000000-0000-0000-0000-000000000003") },
                    { new Guid("70300000-0000-0000-0000-000000000001"), false, true, "Vehicle Type", "[\"Car\",\"Truck\",\"Boat\",\"Plane\",\"Drone\"]", new Guid("70000000-0000-0000-0000-000000000004") },
                    { new Guid("70300000-0000-0000-0000-000000000002"), false, false, "Power", "[\"Electric\",\"Gas\",\"Nitro\"]", new Guid("70000000-0000-0000-0000-000000000004") },
                    { new Guid("70400000-0000-0000-0000-000000000001"), false, true, "Type", "[\"Barbie\",\"Fashion Doll\",\"Teddy Bear\",\"Vintage Doll\"]", new Guid("70000000-0000-0000-0000-000000000005") },
                    { new Guid("70500000-0000-0000-0000-000000000001"), false, false, "Theme", "[\"Architecture\",\"City\",\"Ideas\",\"Star Wars\",\"Technic\"]", new Guid("70000000-0000-0000-0000-000000000006") },
                    { new Guid("80100000-0000-0000-0000-000000000001"), false, true, "Sport", "[\"Camping\",\"Climbing\",\"Fishing\",\"Hunting\",\"Water Sports\"]", new Guid("80000000-0000-0000-0000-000000000002") },
                    { new Guid("80200000-0000-0000-0000-000000000001"), false, true, "Equipment Type", "[\"Cardio Machine\",\"Free Weights\",\"Resistance Bands\",\"Yoga Mat\"]", new Guid("80000000-0000-0000-0000-000000000003") },
                    { new Guid("80300000-0000-0000-0000-000000000001"), false, true, "Bicycle Type", "[\"Mountain\",\"Road\",\"Hybrid\",\"Gravel\",\"Electric\"]", new Guid("80000000-0000-0000-0000-000000000004") },
                    { new Guid("80300000-0000-0000-0000-000000000002"), false, false, "Frame Size", "[\"Small\",\"Medium\",\"Large\",\"X-Large\"]", new Guid("80000000-0000-0000-0000-000000000004") },
                    { new Guid("80400000-0000-0000-0000-000000000001"), false, true, "Club Type", "[\"Driver\",\"Fairway Wood\",\"Hybrid\",\"Iron Set\",\"Putter\"]", new Guid("80000000-0000-0000-0000-000000000005") },
                    { new Guid("80400000-0000-0000-0000-000000000002"), false, false, "Flex", "[\"Ladies\",\"Regular\",\"Stiff\",\"Extra Stiff\"]", new Guid("80000000-0000-0000-0000-000000000005") },
                    { new Guid("90100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Charlotte Tilbury\",\"Dior\",\"Fenty Beauty\",\"MAC\",\"Rare Beauty\"]", new Guid("90000000-0000-0000-0000-000000000002") },
                    { new Guid("90100000-0000-0000-0000-000000000002"), false, true, "Product Type", "[\"Foundation\",\"Eyeshadow\",\"Lipstick\",\"Mascara\",\"Primer\"]", new Guid("90000000-0000-0000-0000-000000000002") },
                    { new Guid("90100000-0000-0000-0000-000000000003"), false, false, "Shade", "[\"Fair\",\"Light\",\"Medium\",\"Tan\",\"Deep\"]", new Guid("90000000-0000-0000-0000-000000000002") },
                    { new Guid("90200000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"CeraVe\",\"Dermalogica\",\"Drunk Elephant\",\"La Roche-Posay\",\"Tatcha\"]", new Guid("90000000-0000-0000-0000-000000000003") },
                    { new Guid("90200000-0000-0000-0000-000000000002"), false, true, "Skin Type", "[\"Dry\",\"Normal\",\"Oily\",\"Combination\",\"Sensitive\"]", new Guid("90000000-0000-0000-0000-000000000003") },
                    { new Guid("90200000-0000-0000-0000-000000000003"), true, false, "Concern", "[\"Acne\",\"Anti-Aging\",\"Brightening\",\"Hydration\",\"Sun Protection\"]", new Guid("90000000-0000-0000-0000-000000000003") },
                    { new Guid("90300000-0000-0000-0000-000000000001"), false, true, "Formulation", "[\"Capsule\",\"Gummy\",\"Powder\",\"Softgel\",\"Tablet\"]", new Guid("90000000-0000-0000-0000-000000000004") },
                    { new Guid("90300000-0000-0000-0000-000000000002"), true, false, "Main Purpose", "[\"Energy\",\"General Wellness\",\"Immune Support\",\"Joint Health\",\"Sleep\"]", new Guid("90000000-0000-0000-0000-000000000004") },
                    { new Guid("90400000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Amika\",\"Dyson\",\"GHD\",\"Olaplex\",\"Redken\"]", new Guid("90000000-0000-0000-0000-000000000005") },
                    { new Guid("90400000-0000-0000-0000-000000000002"), true, false, "Hair Type", "[\"Coily\",\"Curly\",\"Straight\",\"Wavy\",\"Fine\"]", new Guid("90000000-0000-0000-0000-000000000005") },
                    { new Guid("90500000-0000-0000-0000-000000000001"), false, true, "Fragrance Type", "[\"Eau de Parfum\",\"Eau de Toilette\",\"Eau de Cologne\",\"Perfume Oil\"]", new Guid("90000000-0000-0000-0000-000000000006") },
                    { new Guid("90500000-0000-0000-0000-000000000002"), false, false, "Volume", "[\"30 ml\",\"50 ml\",\"75 ml\",\"100 ml\"]", new Guid("90000000-0000-0000-0000-000000000006") },
                    { new Guid("a0100000-0000-0000-0000-000000000001"), false, true, "Equipment Type", "[\"Backhoe\",\"Bulldozer\",\"Excavator\",\"Forklift\",\"Skid Steer\"]", new Guid("a0000000-0000-0000-0000-000000000002") },
                    { new Guid("a0100000-0000-0000-0000-000000000002"), false, false, "Hours", "[\"0-500\",\"501-1500\",\"1501-3000\",\"3001\\u002B\"]", new Guid("a0000000-0000-0000-0000-000000000002") },
                    { new Guid("a0200000-0000-0000-0000-000000000001"), false, true, "Supply Type", "[\"Fasteners\",\"Hydraulics\",\"HVAC\",\"Pneumatics\",\"Safety\"]", new Guid("a0000000-0000-0000-0000-000000000003") },
                    { new Guid("a0300000-0000-0000-0000-000000000001"), false, true, "Service Type", "[\"Consulting\",\"Installation\",\"Maintenance\",\"Marketing\",\"Training\"]", new Guid("a0000000-0000-0000-0000-000000000004") },
                    { new Guid("a0400000-0000-0000-0000-000000000001"), false, true, "Equipment Type", "[\"3D Printer\",\"Laser Printer\",\"Multifunction\",\"Scanner\",\"Shredder\"]", new Guid("a0000000-0000-0000-0000-000000000005") },
                    { new Guid("b0100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Fender\",\"Gibson\",\"Ibanez\",\"PRS\",\"Taylor\"]", new Guid("b0000000-0000-0000-0000-000000000002") },
                    { new Guid("b0100000-0000-0000-0000-000000000002"), false, false, "Body Type", "[\"Solid\",\"Semi-Hollow\",\"Hollow\"]", new Guid("b0000000-0000-0000-0000-000000000002") },
                    { new Guid("b0200000-0000-0000-0000-000000000001"), false, true, "Type", "[\"Mixer\",\"Microphone\",\"Monitor\",\"Interface\",\"Outboard Gear\"]", new Guid("b0000000-0000-0000-0000-000000000003") },
                    { new Guid("b0300000-0000-0000-0000-000000000001"), false, true, "Type", "[\"Controller\",\"Mixer\",\"Turntable\",\"Lighting\"]", new Guid("b0000000-0000-0000-0000-000000000004") },
                    { new Guid("b0400000-0000-0000-0000-000000000001"), false, true, "Instrument", "[\"Clarinet\",\"Flute\",\"Saxophone\",\"Trombone\",\"Trumpet\"]", new Guid("b0000000-0000-0000-0000-000000000005") },
                    { new Guid("c0100000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Apparel\",\"Crates\",\"Food\",\"Grooming\",\"Toys\"]", new Guid("c0000000-0000-0000-0000-000000000002") },
                    { new Guid("c0100000-0000-0000-0000-000000000002"), false, false, "Size", "[\"XS\",\"S\",\"M\",\"L\",\"XL\"]", new Guid("c0000000-0000-0000-0000-000000000002") },
                    { new Guid("c0200000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Food\",\"Litter\",\"Scratchers\",\"Toys\"]", new Guid("c0000000-0000-0000-0000-000000000003") },
                    { new Guid("c0300000-0000-0000-0000-000000000001"), false, true, "Aquarium Type", "[\"Freshwater\",\"Marine\",\"Reef\",\"Brackish\"]", new Guid("c0000000-0000-0000-0000-000000000004") },
                    { new Guid("c0300000-0000-0000-0000-000000000002"), false, false, "Tank Capacity", "[\"1-10 gal\",\"11-30 gal\",\"31-55 gal\",\"56\\u002B gal\"]", new Guid("c0000000-0000-0000-0000-000000000004") },
                    { new Guid("c0400000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Bedding\",\"Cages\",\"Food\",\"Toys\"]", new Guid("c0000000-0000-0000-0000-000000000005") },
                    { new Guid("d0100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Baby Jogger\",\"Bugaboo\",\"Chicco\",\"Graco\",\"UPPAbaby\"]", new Guid("d0000000-0000-0000-0000-000000000002") },
                    { new Guid("d0100000-0000-0000-0000-000000000002"), false, true, "Stroller Type", "[\"Full-Size\",\"Jogging\",\"Travel System\",\"Umbrella\"]", new Guid("d0000000-0000-0000-0000-000000000002") },
                    { new Guid("d0200000-0000-0000-0000-000000000001"), false, true, "Furniture Piece", "[\"Crib\",\"Changing Table\",\"Glider\",\"Dresser\"]", new Guid("d0000000-0000-0000-0000-000000000003") },
                    { new Guid("d0300000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Baby Monitor\",\"Safety Gate\",\"Outlet Cover\",\"Cabinet Lock\"]", new Guid("d0000000-0000-0000-0000-000000000004") },
                    { new Guid("d0400000-0000-0000-0000-000000000001"), false, true, "Feeding Type", "[\"Bottle\",\"Breastfeeding\",\"Solid Food\",\"Toddler\"]", new Guid("d0000000-0000-0000-0000-000000000005") },
                    { new Guid("e0100000-0000-0000-0000-000000000001"), false, true, "Product Type", "[\"Adhesives\",\"Dies\",\"Paper\",\"Stamps\"]", new Guid("e0000000-0000-0000-0000-000000000002") },
                    { new Guid("e0200000-0000-0000-0000-000000000001"), false, true, "Medium", "[\"Acrylic\",\"Oil\",\"Watercolor\",\"Pastel\"]", new Guid("e0000000-0000-0000-0000-000000000003") },
                    { new Guid("e0300000-0000-0000-0000-000000000001"), false, true, "Fabric Type", "[\"Cotton\",\"Denim\",\"Fleece\",\"Linen\",\"Silk\"]", new Guid("e0000000-0000-0000-0000-000000000004") },
                    { new Guid("e0400000-0000-0000-0000-000000000001"), false, true, "Material", "[\"Glass\",\"Gemstone\",\"Metal\",\"Resin\",\"Wood\"]", new Guid("e0000000-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "Id", "AutoClosedAt", "BuyerId", "BuyerNote", "CompletedAt", "CreatedAt", "CreatedBy", "InitiatedBy", "IsDeleted", "OrderId", "Reason", "RequestedAt", "SellerId", "SellerNote", "SellerRespondedAt", "SellerResponseDeadlineUtc", "Status", "UpdatedAt", "UpdatedBy", "OrderTotalSnapshot_Amount", "OrderTotalSnapshot_Currency", "RefundAmount_Amount", "RefundAmount_Currency" },
                values: new object[] { new Guid("5d4e7a11-0c4e-4a6f-9f2f-000000000004"), null, new Guid("70000000-0000-0000-0000-000000000003"), "Item still not handed to carrier, requesting cancellation.", null, new DateTime(2025, 11, 6, 18, 0, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("0f0c1a22-11aa-4c6d-8f10-000000000018"), 4, new DateTime(2025, 11, 6, 18, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), "Approved – refund issued to buyer's original payment method.", new DateTime(2025, 11, 7, 9, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 8, 18, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 11, 7, 9, 30, 0, 0, DateTimeKind.Utc), "seed", 107.23m, "USD", 107.23m, "USD" });

            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "Id", "AutoClosedAt", "BuyerId", "BuyerNote", "CompletedAt", "CreatedAt", "CreatedBy", "InitiatedBy", "IsDeleted", "OrderId", "Reason", "RequestedAt", "SellerId", "SellerNote", "SellerRespondedAt", "SellerResponseDeadlineUtc", "Status", "UpdatedAt", "UpdatedBy", "OrderTotalSnapshot_Amount", "OrderTotalSnapshot_Currency" },
                values: new object[,]
                {
                    { new Guid("5d4e7a11-0c4e-4a6f-9f2f-000000000005"), null, new Guid("70000000-0000-0000-0000-000000000002"), "Accidentally placed duplicate order.", null, new DateTime(2025, 11, 8, 11, 0, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001b"), 1, new DateTime(2025, 11, 8, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 11, 10, 11, 0, 0, 0, DateTimeKind.Utc), 0, new DateTime(2025, 11, 8, 11, 0, 0, 0, DateTimeKind.Utc), "seed", 74.43m, "USD" },
                    { new Guid("6f1f9f0c-898f-4c7b-bb38-1b689e9f7331"), null, new Guid("70000000-0000-0000-0000-000000000002"), "Realized I ordered the wrong variation, please cancel.", null, new DateTime(2025, 10, 13, 14, 15, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, new DateTime(2025, 10, 13, 14, 15, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), 0, new DateTime(2025, 10, 13, 14, 15, 0, 0, DateTimeKind.Utc), "seed", 114.69m, "USD" },
                    { new Guid("c3c25c5b-f1a3-4e5f-9ccd-da6a46b91753"), new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000003"), null, new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), "seed", 2, false, new Guid("973cac8a-9be0-44a0-90b7-fd8263f8e78a"), 99, new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000002"), "Order auto-cancelled after missing shipping deadline.", null, null, 5, new DateTime(2025, 10, 30, 9, 0, 0, 0, DateTimeKind.Utc), "seed", 97.37m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "order_cancellation_requests",
                columns: new[] { "Id", "AutoClosedAt", "BuyerId", "BuyerNote", "CompletedAt", "CreatedAt", "CreatedBy", "InitiatedBy", "IsDeleted", "OrderId", "Reason", "RequestedAt", "SellerId", "SellerNote", "SellerRespondedAt", "SellerResponseDeadlineUtc", "Status", "UpdatedAt", "UpdatedBy", "OrderTotalSnapshot_Amount", "OrderTotalSnapshot_Currency", "RefundAmount_Amount", "RefundAmount_Currency" },
                values: new object[] { new Guid("d3f7d907-6b71-47d8-8651-922629540277"), null, new Guid("70000000-0000-0000-0000-000000000002"), "Need to update the delivery address; requesting cancellation.", null, new DateTime(2025, 10, 19, 12, 0, 0, 0, DateTimeKind.Utc), "seed", 0, false, new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"), 3, new DateTime(2025, 10, 19, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("70000000-0000-0000-0000-000000000001"), "Approved – refund processing with payment provider.", new DateTime(2025, 10, 19, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 21, 12, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2025, 10, 19, 18, 0, 0, 0, DateTimeKind.Utc), "seed", 87.83m, "USD", 87.83m, "USD" });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "CreatedBy", "ImageUrl", "IsDeleted", "ListingId", "OrderId", "Quantity", "Sku", "Title", "UpdatedAt", "UpdatedBy", "VariationId", "TotalPrice_Amount", "TotalPrice_Currency", "UnitPrice_Amount", "UnitPrice_Currency" },
                values: new object[,]
                {
                    { new Guid("0a3e9070-0a5e-4114-8634-8e9353a5369e"), null, new DateTime(2025, 10, 20, 16, 0, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-9/640/640", false, new Guid("71000000-0000-0000-0000-000000000009"), new Guid("1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4"), 1, "DEMO-1-0009", "Alice's Item #9", new DateTime(2025, 10, 21, 16, 0, 0, 0, DateTimeKind.Utc), "seed", null, 37.99m, "USD", 37.99m, "USD" },
                    { new Guid("1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d"), null, new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-3/640/640", false, new Guid("71000000-0000-0000-0000-000000000003"), new Guid("f6de3ce0-2d3d-4709-923d-cbb61f956947"), 2, "DEMO-1-0003", "Alice's Item #3", new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", null, 63.98m, "USD", 31.99m, "USD" },
                    { new Guid("30f2c0f3-09bb-4f52-93a9-6e98b0171c3f"), null, new DateTime(2025, 10, 28, 12, 10, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-3/640/640", false, new Guid("73000000-0000-0000-0000-000000000003"), new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"), 1, "DEMO-3-0003", "Cecilia's Item #3", new DateTime(2025, 10, 31, 12, 10, 0, 0, DateTimeKind.Utc), "seed", null, 47.99m, "USD", 47.99m, "USD" },
                    { new Guid("3e54a8a8-3b35-4bdf-9d09-75042c7f7d4f"), null, new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-4/640/640", false, new Guid("71000000-0000-0000-0000-000000000004"), new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, "DEMO-1-0004", "Alice's Item #4", new DateTime(2025, 10, 16, 0, 15, 0, 0, DateTimeKind.Utc), "seed", null, 32.99m, "USD", 32.99m, "USD" },
                    { new Guid("4a1ab1de-4a10-4326-a0be-5d3ab27c9df7"), null, new DateTime(2025, 10, 22, 8, 20, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-2/640/640", false, new Guid("72000000-0000-0000-0000-000000000002"), new Guid("d2ee4d4a-5be0-4d76-bce6-0b8578c87407"), 1, "DEMO-2-0002", "Brian's Item #2", new DateTime(2025, 10, 22, 17, 20, 0, 0, DateTimeKind.Utc), "seed", null, 38.99m, "USD", 38.99m, "USD" },
                    { new Guid("55c9f2a2-dba1-4c66-9b83-a8b4c9e7a0d4"), null, new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-7/640/640", false, new Guid("73000000-0000-0000-0000-000000000007"), new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), 1, "DEMO-3-0007", "Cecilia's Item #7", new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), "seed", null, 51.99m, "USD", 51.99m, "USD" },
                    { new Guid("5f2f8987-3b95-4b9f-8cc0-0f7c4b8d3b92"), null, new DateTime(2025, 10, 24, 11, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-3/640/640", false, new Guid("72000000-0000-0000-0000-000000000003"), new Guid("973cac8a-9be0-44a0-90b7-fd8263f8e78a"), 2, "DEMO-2-0003", "Brian's Item #3", new DateTime(2025, 10, 24, 11, 5, 0, 0, DateTimeKind.Utc), "seed", null, 79.98m, "USD", 39.99m, "USD" },
                    { new Guid("6bd3f47d-4f1e-467f-8797-3b2a151dd09f"), null, new DateTime(2025, 10, 28, 12, 10, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-4/640/640", false, new Guid("73000000-0000-0000-0000-000000000004"), new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"), 2, "DEMO-3-0004", "Cecilia's Item #4", new DateTime(2025, 10, 31, 12, 10, 0, 0, DateTimeKind.Utc), "seed", null, 97.98m, "USD", 48.99m, "USD" },
                    { new Guid("6cbb0f3e-9fd9-4c83-b181-74d3432fb953"), null, new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-1/640/640", false, new Guid("71000000-0000-0000-0000-000000000001"), new Guid("f6de3ce0-2d3d-4709-923d-cbb61f956947"), 1, "DEMO-1-0001", "Alice's Item #1", new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", null, 29.99m, "USD", 29.99m, "USD" },
                    { new Guid("6ccf331f-2863-411a-8f9e-1a28857e2a31"), null, new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-6/640/640", false, new Guid("73000000-0000-0000-0000-000000000006"), new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), 1, "DEMO-3-0006", "Cecilia's Item #6", new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), "seed", null, 50.99m, "USD", 50.99m, "USD" },
                    { new Guid("7fdde15f-acca-41c7-97a3-e1df2c6a4b8d"), null, new DateTime(2025, 10, 20, 16, 0, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-8/640/640", false, new Guid("71000000-0000-0000-0000-000000000008"), new Guid("1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4"), 1, "DEMO-1-0008", "Alice's Item #8", new DateTime(2025, 10, 21, 16, 0, 0, 0, DateTimeKind.Utc), "seed", null, 36.99m, "USD", 36.99m, "USD" },
                    { new Guid("8fb2678e-8b5d-4d1e-b079-0fb2aa3a055c"), null, new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-1/640/640", false, new Guid("73000000-0000-0000-0000-000000000001"), new Guid("a4206ad5-6a35-43bb-8a8c-8c7b244594ac"), 1, "DEMO-3-0001", "Cecilia's Item #1", new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", null, 45.99m, "USD", 45.99m, "USD" },
                    { new Guid("9be4d720-31f2-4456-94d7-2bf0c76fa0ec"), null, new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-2/640/640", false, new Guid("73000000-0000-0000-0000-000000000002"), new Guid("a4206ad5-6a35-43bb-8a8c-8c7b244594ac"), 1, "DEMO-3-0002", "Cecilia's Item #2", new DateTime(2025, 10, 26, 18, 40, 0, 0, DateTimeKind.Utc), "seed", null, 46.99m, "USD", 46.99m, "USD" },
                    { new Guid("a3d8f848-7cf3-4058-9f09-3a78d4d64a5d"), null, new DateTime(2025, 10, 25, 15, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-5/640/640", false, new Guid("72000000-0000-0000-0000-000000000005"), new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"), 1, "DEMO-2-0005", "Brian's Item #5", new DateTime(2025, 10, 28, 15, 30, 0, 0, DateTimeKind.Utc), "seed", null, 41.99m, "USD", 41.99m, "USD" },
                    { new Guid("a9d23977-7d99-4d44-bb79-4cff5ec2f56f"), null, new DateTime(2025, 10, 15, 14, 15, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-5/640/640", false, new Guid("71000000-0000-0000-0000-000000000005"), new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, "DEMO-1-0005", "Alice's Item #5", new DateTime(2025, 10, 16, 0, 15, 0, 0, DateTimeKind.Utc), "seed", null, 33.99m, "USD", 33.99m, "USD" },
                    { new Guid("b7fe44b8-3d3a-49f0-91c5-8ed5cb0c824a"), null, new DateTime(2025, 10, 25, 15, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-4/640/640", false, new Guid("72000000-0000-0000-0000-000000000004"), new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"), 1, "DEMO-2-0004", "Brian's Item #4", new DateTime(2025, 10, 28, 15, 30, 0, 0, DateTimeKind.Utc), "seed", null, 42.99m, "USD", 42.99m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000021"), null, new DateTime(2025, 11, 1, 9, 15, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-10/640/640", false, new Guid("71000000-0000-0000-0000-00000000000a"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000011"), 1, "DEMO-1-0010", "Alice's Item #10", new DateTime(2025, 11, 1, 9, 50, 0, 0, DateTimeKind.Utc), "seed", null, 58.99m, "USD", 58.99m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000022"), null, new DateTime(2025, 11, 2, 13, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-6/640/640", false, new Guid("72000000-0000-0000-0000-000000000006"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000012"), 1, "DEMO-2-0006", "Brian's Item #6", new DateTime(2025, 11, 2, 14, 10, 0, 0, DateTimeKind.Utc), "seed", null, 64.50m, "USD", 64.50m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000023"), null, new DateTime(2025, 11, 3, 17, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-8/640/640", false, new Guid("73000000-0000-0000-0000-000000000008"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000013"), 1, "DEMO-3-0008", "Cecilia's Item #8", new DateTime(2025, 11, 3, 17, 55, 0, 0, DateTimeKind.Utc), "seed", null, 72.00m, "USD", 72.00m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000024"), null, new DateTime(2025, 11, 4, 8, 45, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-11/640/640", false, new Guid("71000000-0000-0000-0000-00000000000b"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000014"), 1, "DEMO-1-0011", "Alice's Item #11", new DateTime(2025, 11, 4, 9, 13, 0, 0, DateTimeKind.Utc), "seed", null, 55.75m, "USD", 55.75m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000025"), null, new DateTime(2025, 11, 5, 10, 0, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-12/640/640", false, new Guid("71000000-0000-0000-0000-00000000000c"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000015"), 1, "DEMO-1-0012", "Alice's Item #12", new DateTime(2025, 11, 5, 10, 30, 0, 0, DateTimeKind.Utc), "seed", null, 62.75m, "USD", 62.75m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000026"), null, new DateTime(2025, 11, 5, 11, 30, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-7/640/640", false, new Guid("72000000-0000-0000-0000-000000000007"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000016"), 1, "DEMO-2-0007", "Brian's Item #7", new DateTime(2025, 11, 5, 11, 55, 0, 0, DateTimeKind.Utc), "seed", null, 48.40m, "USD", 48.40m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000027"), null, new DateTime(2025, 11, 6, 9, 20, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-9/640/640", false, new Guid("73000000-0000-0000-0000-000000000009"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000017"), 1, "DEMO-3-0009", "Cecilia's Item #9", new DateTime(2025, 11, 6, 9, 40, 0, 0, DateTimeKind.Utc), "seed", null, 79.90m, "USD", 79.90m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000028"), null, new DateTime(2025, 11, 6, 13, 45, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-13/640/640", false, new Guid("71000000-0000-0000-0000-00000000000d"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000018"), 1, "DEMO-1-0013", "Alice's Item #13", new DateTime(2025, 11, 6, 14, 20, 0, 0, DateTimeKind.Utc), "seed", null, 88.60m, "USD", 88.60m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-000000000029"), null, new DateTime(2025, 11, 7, 8, 10, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-8/640/640", false, new Guid("72000000-0000-0000-0000-000000000008"), new Guid("0f0c1a22-11aa-4c6d-8f10-000000000019"), 1, "DEMO-2-0008", "Brian's Item #8", new DateTime(2025, 11, 7, 8, 28, 0, 0, DateTimeKind.Utc), "seed", null, 71.25m, "USD", 71.25m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002a"), null, new DateTime(2025, 11, 7, 15, 25, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-10/640/640", false, new Guid("73000000-0000-0000-0000-00000000000a"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001a"), 1, "DEMO-3-0010", "Cecilia's Item #10", new DateTime(2025, 11, 7, 15, 47, 0, 0, DateTimeKind.Utc), "seed", null, 65.80m, "USD", 65.80m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002b"), null, new DateTime(2025, 11, 8, 10, 40, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-14/640/640", false, new Guid("71000000-0000-0000-0000-00000000000e"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001b"), 1, "DEMO-1-0014", "Alice's Item #14", new DateTime(2025, 11, 8, 11, 7, 0, 0, DateTimeKind.Utc), "seed", null, 59.10m, "USD", 59.10m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002c"), null, new DateTime(2025, 11, 8, 14, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-9/640/640", false, new Guid("72000000-0000-0000-0000-000000000009"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001c"), 1, "DEMO-2-0009", "Brian's Item #9", new DateTime(2025, 11, 8, 14, 37, 0, 0, DateTimeKind.Utc), "seed", null, 83.45m, "USD", 83.45m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002d"), null, new DateTime(2025, 11, 9, 11, 15, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-11/640/640", false, new Guid("73000000-0000-0000-0000-00000000000b"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001d"), 1, "DEMO-3-0011", "Cecilia's Item #11", new DateTime(2025, 11, 9, 11, 39, 0, 0, DateTimeKind.Utc), "seed", null, 90.30m, "USD", 90.30m, "USD" },
                    { new Guid("c579fb6b-b172-4e17-b610-00000000002e"), null, new DateTime(2025, 11, 9, 17, 50, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-15/640/640", false, new Guid("71000000-0000-0000-0000-00000000000f"), new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001e"), 1, "DEMO-1-0015", "Alice's Item #15", new DateTime(2025, 11, 9, 18, 19, 0, 0, DateTimeKind.Utc), "seed", null, 74.95m, "USD", 74.95m, "USD" },
                    { new Guid("c5b7436e-0ae9-4265-9f2b-7a1fd7e7d78f"), null, new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-6/640/640", false, new Guid("71000000-0000-0000-0000-000000000006"), new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"), 1, "DEMO-1-0006", "Alice's Item #6", new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", null, 34.99m, "USD", 34.99m, "USD" },
                    { new Guid("e1d40241-43f4-4d93-b9ed-4ac8c9e52088"), null, new DateTime(2025, 10, 22, 8, 20, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/2-1/640/640", false, new Guid("72000000-0000-0000-0000-000000000001"), new Guid("d2ee4d4a-5be0-4d76-bce6-0b8578c87407"), 1, "DEMO-2-0001", "Brian's Item #1", new DateTime(2025, 10, 22, 17, 20, 0, 0, DateTimeKind.Utc), "seed", null, 37.99m, "USD", 37.99m, "USD" },
                    { new Guid("e9ad9da9-07b8-42ae-9ce2-764f76d4b657"), null, new DateTime(2025, 10, 30, 9, 5, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/3-5/640/640", false, new Guid("73000000-0000-0000-0000-000000000005"), new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), 1, "DEMO-3-0005", "Cecilia's Item #5", new DateTime(2025, 11, 3, 9, 5, 0, 0, DateTimeKind.Utc), "seed", null, 49.99m, "USD", 49.99m, "USD" },
                    { new Guid("f2a8249e-2643-49b5-bd73-0cac89fb4fc5"), null, new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", "https://picsum.photos/seed/1-7/640/640", false, new Guid("71000000-0000-0000-0000-000000000007"), new Guid("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a"), 1, "DEMO-1-0007", "Alice's Item #7", new DateTime(2025, 10, 18, 9, 45, 0, 0, DateTimeKind.Utc), "seed", null, 35.99m, "USD", 35.99m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "Id", "BuyerId", "BuyerNote", "BuyerReturnDueAt", "BuyerShippedAt", "ClosedAt", "CreatedAt", "CreatedBy", "DeliveredAt", "IsDeleted", "OrderId", "PreferredResolution", "Reason", "RefundIssuedAt", "RequestedAt", "ReturnCarrier", "SellerId", "SellerNote", "SellerRespondedAt", "Status", "TrackingNumber", "UpdatedAt", "UpdatedBy", "OrderTotalSnapshot_Amount", "OrderTotalSnapshot_Currency" },
                values: new object[,]
                {
                    { new Guid("8cb7ab44-0d7d-4d7d-9b24-1cc54d4da7bf"), new Guid("70000000-0000-0000-0000-000000000001"), "Item color differs from the listing photos.", null, null, null, new DateTime(2025, 10, 29, 10, 0, 0, 0, DateTimeKind.Utc), "seed", null, false, new Guid("bd34cf77-4551-4194-ad16-d20c94b58289"), 0, 0, null, new DateTime(2025, 10, 29, 10, 0, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000002"), null, null, 0, null, new DateTime(2025, 10, 29, 10, 0, 0, 0, DateTimeKind.Utc), "seed", 108.88m, "USD" },
                    { new Guid("9a7f6b12-5e2d-4d91-8c22-000000000004"), new Guid("70000000-0000-0000-0000-000000000001"), "Received the 64GB variant instead of 128GB.", new DateTime(2025, 11, 13, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 10, 10, 20, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 11, 8, 9, 30, 0, 0, DateTimeKind.Utc), "seed", null, false, new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001a"), 2, 2, null, new DateTime(2025, 11, 8, 9, 30, 0, 0, DateTimeKind.Utc), "FedEx", new Guid("70000000-0000-0000-0000-000000000003"), "Exchange approved once return is in transit.", new DateTime(2025, 11, 8, 12, 45, 0, 0, DateTimeKind.Utc), 2, "612999AA10NEWRT4", new DateTime(2025, 11, 10, 10, 20, 0, 0, DateTimeKind.Utc), "seed", 80.59m, "USD" },
                    { new Guid("9a7f6b12-5e2d-4d91-8c22-000000000005"), new Guid("70000000-0000-0000-0000-000000000002"), "Decided to keep a different model instead.", new DateTime(2025, 11, 14, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 11, 8, 40, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 11, 9, 15, 0, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 11, 13, 16, 5, 0, 0, DateTimeKind.Utc), false, new Guid("0f0c1a22-11aa-4c6d-8f10-00000000001d"), 0, 6, null, new DateTime(2025, 11, 9, 15, 0, 0, 0, DateTimeKind.Utc), "USPS", new Guid("70000000-0000-0000-0000-000000000003"), "Refund pending inspection of returned item.", new DateTime(2025, 11, 9, 17, 15, 0, 0, DateTimeKind.Utc), 4, "9405511899223857264999", new DateTime(2025, 11, 13, 16, 5, 0, 0, DateTimeKind.Utc), "seed", 112.57m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "Id", "BuyerId", "BuyerNote", "BuyerReturnDueAt", "BuyerShippedAt", "ClosedAt", "CreatedAt", "CreatedBy", "DeliveredAt", "IsDeleted", "OrderId", "PreferredResolution", "Reason", "RefundIssuedAt", "RequestedAt", "ReturnCarrier", "SellerId", "SellerNote", "SellerRespondedAt", "Status", "TrackingNumber", "UpdatedAt", "UpdatedBy", "OrderTotalSnapshot_Amount", "OrderTotalSnapshot_Currency", "RefundAmount_Amount", "RefundAmount_Currency", "RestockingFee_Amount", "RestockingFee_Currency" },
                values: new object[] { new Guid("dc3329e1-14fb-4d00-a395-e76e25a6822b"), new Guid("70000000-0000-0000-0000-000000000002"), "Shoes run smaller than expected.", new DateTime(2025, 11, 9, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 6, 9, 10, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 9, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 4, 17, 45, 0, 0, DateTimeKind.Utc), "seed", new DateTime(2025, 11, 8, 16, 20, 0, 0, DateTimeKind.Utc), false, new Guid("fa236302-3864-4e54-9e40-3ebdb4749734"), 3, 4, new DateTime(2025, 11, 9, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 4, 17, 45, 0, 0, DateTimeKind.Utc), "USPS", new Guid("70000000-0000-0000-0000-000000000003"), "Refunded minus restocking fee.", new DateTime(2025, 11, 4, 20, 30, 0, 0, DateTimeKind.Utc), 5, "9405511899223857264837", new DateTime(2025, 11, 9, 14, 0, 0, 0, DateTimeKind.Utc), "seed", 177.37m, "USD", 150.00m, "USD", 5.00m, "USD" });

            migrationBuilder.InsertData(
                table: "order_return_requests",
                columns: new[] { "Id", "BuyerId", "BuyerNote", "BuyerReturnDueAt", "BuyerShippedAt", "ClosedAt", "CreatedAt", "CreatedBy", "DeliveredAt", "IsDeleted", "OrderId", "PreferredResolution", "Reason", "RefundIssuedAt", "RequestedAt", "ReturnCarrier", "SellerId", "SellerNote", "SellerRespondedAt", "Status", "TrackingNumber", "UpdatedAt", "UpdatedBy", "OrderTotalSnapshot_Amount", "OrderTotalSnapshot_Currency" },
                values: new object[] { new Guid("fd21bed5-6c0c-4bcf-b099-31c8b0d08f27"), new Guid("70000000-0000-0000-0000-000000000001"), "Screen arrived cracked; requesting replacement.", new DateTime(2025, 11, 5, 23, 59, 0, 0, DateTimeKind.Utc), new DateTime(2025, 11, 3, 10, 15, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 11, 1, 9, 0, 0, 0, DateTimeKind.Utc), "seed", null, false, new Guid("1e86f219-1dd0-4cac-a545-cb98e65ce429"), 1, 1, null, new DateTime(2025, 11, 1, 9, 0, 0, 0, DateTimeKind.Utc), "UPS", new Guid("70000000-0000-0000-0000-000000000003"), "Please return using the provided UPS label.", new DateTime(2025, 11, 1, 12, 0, 0, 0, DateTimeKind.Utc), 2, "1Z999AA10123456784", new DateTime(2025, 11, 3, 10, 15, 0, 0, DateTimeKind.Utc), "seed", 169.27m, "USD" });

            migrationBuilder.CreateIndex(
                name: "IX_category_ParentId",
                table: "category",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_category_condition_ConditionId",
                table: "category_condition",
                column: "ConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_category_specific_category_id",
                table: "category_specific",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_coupon_CouponTypeId",
                table: "coupon",
                column: "CouponTypeId");

            migrationBuilder.CreateIndex(
                name: "ux_coupon_code",
                table: "coupon",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coupon_condition_CouponId",
                table: "coupon_condition",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponExcludedCategories_CouponId",
                table: "CouponExcludedCategories",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponExcludedItems_CouponId",
                table: "CouponExcludedItems",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponTargetAudiences_CouponId",
                table: "CouponTargetAudiences",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_dispute_ListingId",
                table: "dispute",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_dispute_RaisedById",
                table: "dispute",
                column: "RaisedById");

            migrationBuilder.CreateIndex(
                name: "IX_dispute_Status",
                table: "dispute",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "idx_listing_active_owner_sort",
                table: "listing",
                columns: new[] { "CreatedBy", "StartDate", "CreatedAt", "Id", "CategoryId", "Format" },
                descending: new[] { false, true, true, false, false, false },
                filter: "\"Status\" = 3");

            migrationBuilder.CreateIndex(
                name: "idx_listing_owner_status",
                table: "listing",
                columns: new[] { "CreatedBy", "Status" });

            migrationBuilder.CreateIndex(
                name: "idx_listing_sku_trgm",
                table: "listing",
                column: "Sku")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "idx_listing_title_trgm",
                table: "listing",
                column: "Title")
                .Annotation("Npgsql:IndexMethod", "gin")
                .Annotation("Npgsql:IndexOperators", new[] { "gin_trgm_ops" });

            migrationBuilder.CreateIndex(
                name: "ix_listing_template_name",
                table: "listing_template",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ListingId_seller_id",
                table: "ListingId",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_buyer_feedback_BuyerId",
                table: "order_buyer_feedback",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "ux_buyer_feedback_order",
                table: "order_buyer_feedback",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_cancellation_requests_order",
                table: "order_cancellation_requests",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "idx_cancellation_requests_status",
                table: "order_cancellation_requests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "ix_order_item_shipments_order_id",
                table: "order_item_shipments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "ix_order_item_shipments_order_item_id",
                table: "order_item_shipments",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "ix_order_item_shipments_shipping_label_id",
                table: "order_item_shipments",
                column: "ShippingLabelId");

            migrationBuilder.CreateIndex(
                name: "idx_order_items_listing_id",
                table: "order_items",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_order_items_OrderId",
                table: "order_items",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "idx_return_requests_order",
                table: "order_return_requests",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "idx_return_requests_status",
                table: "order_return_requests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "ix_order_shipping_labels_order_id",
                table: "order_shipping_labels",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_histories_FromStatusId",
                table: "order_status_histories",
                column: "FromStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_histories_OrderId",
                table: "order_status_histories",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_histories_ToStatusId",
                table: "order_status_histories",
                column: "ToStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_transitions_FromStatusId",
                table: "order_status_transitions",
                column: "FromStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_order_status_transitions_ToStatusId",
                table: "order_status_transitions",
                column: "ToStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_order_statuses_Code",
                table: "order_statuses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_BuyerId",
                table: "orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_OrderNumber",
                table: "orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_StatusId",
                table: "orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_otp_Email_Code",
                table: "otp",
                columns: new[] { "Email", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_qrtz_triggers_SchedName_JobName_JobGroup",
                table: "qrtz_triggers",
                columns: new[] { "SchedName", "JobName", "JobGroup" });

            migrationBuilder.CreateIndex(
                name: "IX_refresh_token_UserId",
                table: "refresh_token",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_report_downloads_UserId_ReferenceCode",
                table: "report_downloads",
                columns: new[] { "UserId", "ReferenceCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_report_schedules_UserId_Source_Type_IsActive",
                table: "report_schedules",
                columns: new[] { "UserId", "Source", "Type", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "idx_return_policy_store_id",
                table: "return_policy",
                column: "StoreId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_review_ListingId",
                table: "review",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_review_RecipientId",
                table: "review",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_review_RecipientRole",
                table: "review",
                column: "RecipientRole");

            migrationBuilder.CreateIndex(
                name: "IX_review_ReviewerId",
                table: "review",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserId",
                table: "RoleUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_sale_event_discount_tier_SaleEventId",
                table: "sale_event_discount_tier",
                column: "SaleEventId");

            migrationBuilder.CreateIndex(
                name: "IX_sale_event_listing_DiscountTierId",
                table: "sale_event_listing",
                column: "DiscountTierId");

            migrationBuilder.CreateIndex(
                name: "IX_sale_event_listing_SaleEventId_ListingId",
                table: "sale_event_listing",
                columns: new[] { "SaleEventId", "ListingId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seller_blocked_buyer_seller_preference_id",
                table: "seller_blocked_buyer",
                column: "seller_preference_id");

            migrationBuilder.CreateIndex(
                name: "ux_seller_blocked_buyer_identifier",
                table: "seller_blocked_buyer",
                columns: new[] { "NormalizedIdentifier", "seller_preference_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seller_exempt_buyer_seller_preference_id",
                table: "seller_exempt_buyer",
                column: "seller_preference_id");

            migrationBuilder.CreateIndex(
                name: "ux_seller_exempt_buyer_identifier",
                table: "seller_exempt_buyer",
                columns: new[] { "NormalizedIdentifier", "seller_preference_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ux_seller_preference_seller",
                table: "seller_preference",
                column: "SellerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_shipping_policy_store_id",
                table: "shipping_policy",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_shipping_services_Slug",
                table: "shipping_services",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_store_slug",
                table: "store",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_store_user_id",
                table: "store",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_store_subscription_store_id",
                table: "store_subscription",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "idx_store_subscription_store_status",
                table: "store_subscription",
                columns: new[] { "StoreId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_user_Email",
                table: "user",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_variation_listing_id",
                table: "variation",
                column: "ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherTransactions_VoucherId",
                table: "VoucherTransactions",
                column: "VoucherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_condition");

            migrationBuilder.DropTable(
                name: "category_specific");

            migrationBuilder.DropTable(
                name: "coupon_condition");

            migrationBuilder.DropTable(
                name: "CouponExcludedCategories");

            migrationBuilder.DropTable(
                name: "CouponExcludedItems");

            migrationBuilder.DropTable(
                name: "CouponTargetAudiences");

            migrationBuilder.DropTable(
                name: "dispute");

            migrationBuilder.DropTable(
                name: "file_metadata");

            migrationBuilder.DropTable(
                name: "listing_image");

            migrationBuilder.DropTable(
                name: "listing_item_specific");

            migrationBuilder.DropTable(
                name: "listing_template");

            migrationBuilder.DropTable(
                name: "ListingId");

            migrationBuilder.DropTable(
                name: "order_buyer_feedback");

            migrationBuilder.DropTable(
                name: "order_cancellation_requests");

            migrationBuilder.DropTable(
                name: "order_item_shipments");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "order_return_requests");

            migrationBuilder.DropTable(
                name: "order_status_histories");

            migrationBuilder.DropTable(
                name: "order_status_transitions");

            migrationBuilder.DropTable(
                name: "otp");

            migrationBuilder.DropTable(
                name: "outbox_message");

            migrationBuilder.DropTable(
                name: "qrtz_blob_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_calendars");

            migrationBuilder.DropTable(
                name: "qrtz_cron_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_fired_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_locks");

            migrationBuilder.DropTable(
                name: "qrtz_paused_trigger_grps");

            migrationBuilder.DropTable(
                name: "qrtz_scheduler_state");

            migrationBuilder.DropTable(
                name: "qrtz_simple_triggers");

            migrationBuilder.DropTable(
                name: "qrtz_triggers");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "report_downloads");

            migrationBuilder.DropTable(
                name: "report_schedules");

            migrationBuilder.DropTable(
                name: "return_policy");

            migrationBuilder.DropTable(
                name: "review");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.DropTable(
                name: "sale_event_listing");

            migrationBuilder.DropTable(
                name: "seller_blocked_buyer");

            migrationBuilder.DropTable(
                name: "seller_exempt_buyer");

            migrationBuilder.DropTable(
                name: "shipping_policy");

            migrationBuilder.DropTable(
                name: "shipping_services");

            migrationBuilder.DropTable(
                name: "store_subscription");

            migrationBuilder.DropTable(
                name: "variation");

            migrationBuilder.DropTable(
                name: "VoucherTransactions");

            migrationBuilder.DropTable(
                name: "condition");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "coupon");

            migrationBuilder.DropTable(
                name: "order_shipping_labels");

            migrationBuilder.DropTable(
                name: "qrtz_job_details");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "sale_event_discount_tier");

            migrationBuilder.DropTable(
                name: "seller_preference");

            migrationBuilder.DropTable(
                name: "store");

            migrationBuilder.DropTable(
                name: "listing");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "coupon_type");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "sale_event");

            migrationBuilder.DropTable(
                name: "order_statuses");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
