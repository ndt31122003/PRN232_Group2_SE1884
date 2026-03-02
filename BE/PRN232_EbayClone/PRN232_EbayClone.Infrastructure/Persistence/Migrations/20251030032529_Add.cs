using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    parent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_category_category_parent_id",
                        column: x => x.parent_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "condition",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_condition", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "file_metadata",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    linked_entity_id = table.Column<Guid>(type: "uuid", nullable: true),
                    url = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    size = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_file_metadata", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "listing",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    format = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    sku = table.Column<string>(type: "text", nullable: false),
                    listing_description = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    condition_id = table.Column<Guid>(type: "uuid", nullable: true),
                    condition_description = table.Column<string>(type: "text", nullable: false),
                    scheduled_start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    draft_expired_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: false),
                    listing_format = table.Column<int>(type: "integer", nullable: false),
                    pricing_start_price = table.Column<decimal>(type: "numeric", nullable: true),
                    pricing_reserve_price = table.Column<decimal>(type: "numeric", nullable: true),
                    pricing_buy_it_now_price = table.Column<decimal>(type: "numeric", nullable: true),
                    pricing_quantity = table.Column<int>(type: "integer", nullable: true),
                    type = table.Column<int>(type: "integer", nullable: true),
                    pricing_price = table.Column<decimal>(type: "numeric", nullable: true),
                    offer_settings_allow_offers = table.Column<bool>(type: "boolean", nullable: true),
                    offer_settings_minimum_offer = table.Column<decimal>(type: "numeric", nullable: true),
                    offer_settings_auto_accept_offer = table.Column<decimal>(type: "numeric", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_listing", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "listing_template",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    payload_json = table.Column<string>(type: "jsonb", nullable: false),
                    format_label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    thumbnail_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_listing_template", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_statuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    color = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    sort_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "otp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    expires_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_used = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_otp", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    occurred_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    processed_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    retry_count = table.Column<int>(type: "integer", nullable: false),
                    error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_message", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "shipping_services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    carrier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    base_cost_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    base_cost_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    estimated_delivery_time = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shipping_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    is_email_verified = table.Column<bool>(type: "boolean", nullable: false),
                    is_payment_verified = table.Column<bool>(type: "boolean", nullable: false),
                    performance_level = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    active_total_value = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category_specific",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    is_required = table.Column<bool>(type: "boolean", nullable: false),
                    allow_multiple = table.Column<bool>(type: "boolean", nullable: false),
                    values = table.Column<string>(type: "jsonb", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category_specific", x => x.id);
                    table.ForeignKey(
                        name: "fk_category_specific_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "category_condition",
                columns: table => new
                {
                    category_id = table.Column<Guid>(type: "uuid", nullable: false),
                    condition_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category_condition", x => new { x.category_id, x.condition_id });
                    table.ForeignKey(
                        name: "fk_category_condition_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_category_condition_condition_condition_id",
                        column: x => x.condition_id,
                        principalTable: "condition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_image",
                columns: table => new
                {
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    url = table.Column<string>(type: "text", nullable: false),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_listing_image", x => new { x.listing_id, x.id });
                    table.ForeignKey(
                        name: "fk_listing_image_listing_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_item_specific",
                columns: table => new
                {
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    val = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_listing_item_specific", x => new { x.listing_id, x.id });
                    table.ForeignKey(
                        name: "fk_listing_item_specific_listing_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "variation",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sku = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    specifics = table.Column<string>(type: "jsonb", nullable: false),
                    images = table.Column<string>(type: "jsonb", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_variation", x => x.id);
                    table.ForeignKey(
                        name: "fk_variation_fixed_price_listings_listing_id",
                        column: x => x.listing_id,
                        principalTable: "listing",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_status_transitions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    to_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    AllowedRoles = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_status_transitions", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_status_transitions_order_statuses_from_status_id",
                        column: x => x.from_status_id,
                        principalTable: "order_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_status_transitions_order_statuses_to_status_id",
                        column: x => x.to_status_id,
                        principalTable: "order_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    Permission = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_permissions", x => new { x.role_id, x.Permission });
                    table.ForeignKey(
                        name: "fk_role_permissions_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "listing_id",
                columns: table => new
                {
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_listing_id", x => x.listing_id);
                    table.ForeignKey(
                        name: "fk_listing_id_user_seller_id",
                        column: x => x.seller_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    buyer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    seller_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                    status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    shipping_status = table.Column<int>(type: "integer", nullable: false),
                    fulfillment_type = table.Column<int>(type: "integer", nullable: false),
                    ordered_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    paid_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    shipped_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    delivered_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    archived_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    coupon_code = table.Column<string>(type: "text", nullable: true),
                    promotion_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_order_statuses_status_id",
                        column: x => x.status_id,
                        principalTable: "order_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_orders_user_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refresh_token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    expires_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_user",
                columns: table => new
                {
                    roles_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role_user", x => new { x.roles_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_role_user_role_roles_id",
                        column: x => x.roles_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_role_user_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    image_url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    sku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    unit_price_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    total_price_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    total_price_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_shipping_labels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    shipping_service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    carrier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    service_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    tracking_number = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    label_url = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    label_file_name = table.Column<string>(type: "character varying(260)", maxLength: 260, nullable: false),
                    cost_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    cost_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    insurance_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    insurance_currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    package_type = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    weight_oz = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    length_in = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    width_in = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    height_in = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    purchased_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    estimated_delivery = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    label_document_id = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_shipping_labels", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_shipping_labels_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_status_histories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    from_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    to_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    changed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_status_histories", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_status_histories_order_statuses_from_status_id",
                        column: x => x.from_status_id,
                        principalTable: "order_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_status_histories_order_statuses_to_status_id",
                        column: x => x.to_status_id,
                        principalTable: "order_statuses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_status_histories_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "id", "created_at", "created_by", "description", "is_deleted", "name", "parent_id", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Consumer electronics, components, and accessories.", false, "Electronics", null, null, null },
                    { new Guid("20000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Apparel, shoes, and accessories.", false, "Fashion", null, null, null },
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Home improvement, décor, and outdoor living.", false, "Home & Garden", null, null, null }
                });

            migrationBuilder.InsertData(
                table: "condition",
                columns: new[] { "id", "created_at", "created_by", "description", "is_deleted", "name", "updated_at", "updated_by" },
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
                table: "listing",
                columns: new[] { "id", "category_id", "condition_description", "condition_id", "created_at", "created_by", "draft_expired_at", "duration", "end_date", "format", "is_deleted", "listing_description", "scheduled_start_time", "sku", "start_date", "status", "title", "type", "updated_at", "updated_by", "listing_format", "pricing_price", "pricing_quantity", "offer_settings_allow_offers", "offer_settings_auto_accept_offer", "offer_settings_minimum_offer" },
                values: new object[,]
                {
                    { new Guid("71000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #1 for Alice Johnson.", null, "DEMO-1-0001", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #1", 1, null, null, 2, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-000000000002"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #2 for Alice Johnson.", null, "DEMO-1-0002", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #2", 1, null, null, 2, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000003"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #3 for Alice Johnson.", null, "DEMO-1-0003", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #3", 1, null, null, 2, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000004"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #4 for Alice Johnson.", null, "DEMO-1-0004", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #4", 1, null, null, 2, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000005"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #5 for Alice Johnson.", null, "DEMO-1-0005", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #5", 1, null, null, 2, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000006"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #6 for Alice Johnson.", null, "DEMO-1-0006", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #6", 1, null, null, 2, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000007"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #7 for Alice Johnson.", null, "DEMO-1-0007", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #7", 1, null, null, 2, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000008"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #8 for Alice Johnson.", null, "DEMO-1-0008", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #8", 1, null, null, 2, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000009"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #9 for Alice Johnson.", null, "DEMO-1-0009", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #9", 1, null, null, 2, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000000a"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #10 for Alice Johnson.", null, "DEMO-1-0010", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #10", 1, null, null, 2, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-00000000000b"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #11 for Alice Johnson.", null, "DEMO-1-0011", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #11", 1, null, null, 2, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000000c"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #12 for Alice Johnson.", null, "DEMO-1-0012", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #12", 1, null, null, 2, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000000d"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #13 for Alice Johnson.", null, "DEMO-1-0013", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #13", 1, null, null, 2, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000000e"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #14 for Alice Johnson.", null, "DEMO-1-0014", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #14", 1, null, null, 2, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000000f"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #15 for Alice Johnson.", null, "DEMO-1-0015", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #15", 1, null, null, 2, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000010"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #16 for Alice Johnson.", null, "DEMO-1-0016", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #16", 1, null, null, 2, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-000000000011"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #17 for Alice Johnson.", null, "DEMO-1-0017", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #17", 1, null, null, 2, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000012"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #18 for Alice Johnson.", null, "DEMO-1-0018", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #18", 1, null, null, 2, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000013"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #19 for Alice Johnson.", null, "DEMO-1-0019", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #19", 1, null, null, 2, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000014"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #20 for Alice Johnson.", null, "DEMO-1-0020", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #20", 1, null, null, 2, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000015"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #21 for Alice Johnson.", null, "DEMO-1-0021", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #21", 1, null, null, 2, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000016"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #22 for Alice Johnson.", null, "DEMO-1-0022", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #22", 1, null, null, 2, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000017"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #23 for Alice Johnson.", null, "DEMO-1-0023", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #23", 1, null, null, 2, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000018"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #24 for Alice Johnson.", null, "DEMO-1-0024", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #24", 1, null, null, 2, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000019"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #25 for Alice Johnson.", null, "DEMO-1-0025", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #25", 1, null, null, 2, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-00000000001a"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #26 for Alice Johnson.", null, "DEMO-1-0026", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #26", 1, null, null, 2, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000001b"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #27 for Alice Johnson.", null, "DEMO-1-0027", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #27", 1, null, null, 2, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000001c"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #28 for Alice Johnson.", null, "DEMO-1-0028", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #28", 1, null, null, 2, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000001d"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #29 for Alice Johnson.", null, "DEMO-1-0029", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #29", 1, null, null, 2, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000001e"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #30 for Alice Johnson.", null, "DEMO-1-0030", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #30", 1, null, null, 2, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000001f"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #31 for Alice Johnson.", null, "DEMO-1-0031", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #31", 1, null, null, 2, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-000000000020"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #32 for Alice Johnson.", null, "DEMO-1-0032", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #32", 1, null, null, 2, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000021"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #33 for Alice Johnson.", null, "DEMO-1-0033", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #33", 1, null, null, 2, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000022"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #34 for Alice Johnson.", null, "DEMO-1-0034", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #34", 1, null, null, 2, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000023"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #35 for Alice Johnson.", null, "DEMO-1-0035", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #35", 1, null, null, 2, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000024"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #36 for Alice Johnson.", null, "DEMO-1-0036", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #36", 1, null, null, 2, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000025"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #37 for Alice Johnson.", null, "DEMO-1-0037", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #37", 1, null, null, 2, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000026"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #38 for Alice Johnson.", null, "DEMO-1-0038", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #38", 1, null, null, 2, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000027"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #39 for Alice Johnson.", null, "DEMO-1-0039", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #39", 1, null, null, 2, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000028"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #40 for Alice Johnson.", null, "DEMO-1-0040", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #40", 1, null, null, 2, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-000000000029"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #41 for Alice Johnson.", null, "DEMO-1-0041", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #41", 1, null, null, 2, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000002a"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #42 for Alice Johnson.", null, "DEMO-1-0042", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #42", 1, null, null, 2, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000002b"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #43 for Alice Johnson.", null, "DEMO-1-0043", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #43", 1, null, null, 2, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000002c"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #44 for Alice Johnson.", null, "DEMO-1-0044", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #44", 1, null, null, 2, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000002d"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #45 for Alice Johnson.", null, "DEMO-1-0045", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #45", 1, null, null, 2, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000002e"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #46 for Alice Johnson.", null, "DEMO-1-0046", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #46", 1, null, null, 2, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-00000000002f"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #47 for Alice Johnson.", null, "DEMO-1-0047", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #47", 1, null, null, 2, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000030"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #48 for Alice Johnson.", null, "DEMO-1-0048", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #48", 1, null, null, 2, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000031"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #49 for Alice Johnson.", null, "DEMO-1-0049", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #49", 1, null, null, 2, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000032"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #50 for Alice Johnson.", null, "DEMO-1-0050", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #50", 1, null, null, 2, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000033"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #51 for Alice Johnson.", null, "DEMO-1-0051", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #51", 1, null, null, 2, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000034"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #52 for Alice Johnson.", null, "DEMO-1-0052", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #52", 1, null, null, 2, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000035"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #53 for Alice Johnson.", null, "DEMO-1-0053", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #53", 1, null, null, 2, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000036"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #54 for Alice Johnson.", null, "DEMO-1-0054", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #54", 1, null, null, 2, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000037"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #55 for Alice Johnson.", null, "DEMO-1-0055", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #55", 1, null, null, 2, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-000000000038"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #56 for Alice Johnson.", null, "DEMO-1-0056", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #56", 1, null, null, 2, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000039"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #57 for Alice Johnson.", null, "DEMO-1-0057", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #57", 1, null, null, 2, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000003a"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #58 for Alice Johnson.", null, "DEMO-1-0058", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #58", 1, null, null, 2, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000003b"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #59 for Alice Johnson.", null, "DEMO-1-0059", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #59", 1, null, null, 2, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000003c"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #60 for Alice Johnson.", null, "DEMO-1-0060", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #60", 1, null, null, 2, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000003d"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #61 for Alice Johnson.", null, "DEMO-1-0061", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #61", 1, null, null, 2, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-00000000003e"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #62 for Alice Johnson.", null, "DEMO-1-0062", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #62", 1, null, null, 2, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000003f"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #63 for Alice Johnson.", null, "DEMO-1-0063", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #63", 1, null, null, 2, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000040"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #64 for Alice Johnson.", null, "DEMO-1-0064", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #64", 1, null, null, 2, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000041"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #65 for Alice Johnson.", null, "DEMO-1-0065", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #65", 1, null, null, 2, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000042"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #66 for Alice Johnson.", null, "DEMO-1-0066", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #66", 1, null, null, 2, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000043"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #67 for Alice Johnson.", null, "DEMO-1-0067", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #67", 1, null, null, 2, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000044"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #68 for Alice Johnson.", null, "DEMO-1-0068", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #68", 1, null, null, 2, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000045"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #69 for Alice Johnson.", null, "DEMO-1-0069", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #69", 1, null, null, 2, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000046"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #70 for Alice Johnson.", null, "DEMO-1-0070", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #70", 1, null, null, 2, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-000000000047"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #71 for Alice Johnson.", null, "DEMO-1-0071", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #71", 1, null, null, 2, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000048"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #72 for Alice Johnson.", null, "DEMO-1-0072", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #72", 1, null, null, 2, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000049"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #73 for Alice Johnson.", null, "DEMO-1-0073", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #73", 1, null, null, 2, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-00000000004a"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #74 for Alice Johnson.", null, "DEMO-1-0074", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #74", 1, null, null, 2, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000004b"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #75 for Alice Johnson.", null, "DEMO-1-0075", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #75", 1, null, null, 2, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000004c"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #76 for Alice Johnson.", null, "DEMO-1-0076", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #76", 1, null, null, 2, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-00000000004d"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #77 for Alice Johnson.", null, "DEMO-1-0077", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #77", 1, null, null, 2, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000004e"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #78 for Alice Johnson.", null, "DEMO-1-0078", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #78", 1, null, null, 2, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000004f"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #79 for Alice Johnson.", null, "DEMO-1-0079", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #79", 1, null, null, 2, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-000000000050"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #80 for Alice Johnson.", null, "DEMO-1-0080", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #80", 1, null, null, 2, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000051"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #81 for Alice Johnson.", null, "DEMO-1-0081", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #81", 1, null, null, 2, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000052"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #82 for Alice Johnson.", null, "DEMO-1-0082", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #82", 1, null, null, 2, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000053"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #83 for Alice Johnson.", null, "DEMO-1-0083", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #83", 1, null, null, 2, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000054"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #84 for Alice Johnson.", null, "DEMO-1-0084", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #84", 1, null, null, 2, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000055"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #85 for Alice Johnson.", null, "DEMO-1-0085", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #85", 1, null, null, 2, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("71000000-0000-0000-0000-000000000056"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #86 for Alice Johnson.", null, "DEMO-1-0086", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #86", 1, null, null, 2, 39.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000057"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #87 for Alice Johnson.", null, "DEMO-1-0087", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #87", 1, null, null, 2, 40.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000058"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #88 for Alice Johnson.", null, "DEMO-1-0088", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #88", 1, null, null, 2, 41.99m, 3, true, 41.99m, 37.79m },
                    { new Guid("71000000-0000-0000-0000-000000000059"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #89 for Alice Johnson.", null, "DEMO-1-0089", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #89", 1, null, null, 2, 42.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000005a"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #90 for Alice Johnson.", null, "DEMO-1-0090", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #90", 1, null, null, 2, 43.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000005b"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #91 for Alice Johnson.", null, "DEMO-1-0091", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #91", 1, null, null, 2, 29.99m, 1, true, 29.99m, 26.99m },
                    { new Guid("71000000-0000-0000-0000-00000000005c"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #92 for Alice Johnson.", null, "DEMO-1-0092", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #92", 1, null, null, 2, 30.99m, 2, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000005d"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #93 for Alice Johnson.", null, "DEMO-1-0093", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #93", 1, null, null, 2, 31.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-00000000005e"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #94 for Alice Johnson.", null, "DEMO-1-0094", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #94", 1, null, null, 2, 32.99m, 4, true, 32.99m, 29.69m },
                    { new Guid("71000000-0000-0000-0000-00000000005f"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #95 for Alice Johnson.", null, "DEMO-1-0095", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #95", 1, null, null, 2, 33.99m, 5, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000060"), new Guid("10000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #96 for Alice Johnson.", null, "DEMO-1-0096", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #96", 1, null, null, 2, 34.99m, 1, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000061"), new Guid("10000000-0000-0000-0000-000000000003"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #97 for Alice Johnson.", null, "DEMO-1-0097", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #97", 1, null, null, 2, 35.99m, 2, true, 35.99m, 32.39m },
                    { new Guid("71000000-0000-0000-0000-000000000062"), new Guid("10000000-0000-0000-0000-000000000004"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #98 for Alice Johnson.", null, "DEMO-1-0098", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #98", 1, null, null, 2, 36.99m, 3, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000063"), new Guid("20000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #99 for Alice Johnson.", null, "DEMO-1-0099", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #99", 1, null, null, 2, 37.99m, 4, false, null, null },
                    { new Guid("71000000-0000-0000-0000-000000000064"), new Guid("30000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", null, 0, null, 2, false, "Curated demo listing #100 for Alice Johnson.", null, "DEMO-1-0100", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Alice's Item #100", 1, null, null, 2, 38.99m, 5, true, 38.99m, 35.09m },
                    { new Guid("72000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #1 for Brian Carter.", null, "DEMO-2-0001", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #1", 1, null, null, 2, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-000000000002"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #2 for Brian Carter.", null, "DEMO-2-0002", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #2", 1, null, null, 2, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000003"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #3 for Brian Carter.", null, "DEMO-2-0003", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #3", 1, null, null, 2, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000004"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #4 for Brian Carter.", null, "DEMO-2-0004", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #4", 1, null, null, 2, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000005"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #5 for Brian Carter.", null, "DEMO-2-0005", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #5", 1, null, null, 2, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000006"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #6 for Brian Carter.", null, "DEMO-2-0006", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #6", 1, null, null, 2, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000007"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #7 for Brian Carter.", null, "DEMO-2-0007", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #7", 1, null, null, 2, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000008"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #8 for Brian Carter.", null, "DEMO-2-0008", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #8", 1, null, null, 2, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000009"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #9 for Brian Carter.", null, "DEMO-2-0009", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #9", 1, null, null, 2, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000000a"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #10 for Brian Carter.", null, "DEMO-2-0010", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #10", 1, null, null, 2, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-00000000000b"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #11 for Brian Carter.", null, "DEMO-2-0011", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #11", 1, null, null, 2, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000000c"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #12 for Brian Carter.", null, "DEMO-2-0012", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #12", 1, null, null, 2, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000000d"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #13 for Brian Carter.", null, "DEMO-2-0013", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #13", 1, null, null, 2, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000000e"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #14 for Brian Carter.", null, "DEMO-2-0014", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #14", 1, null, null, 2, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000000f"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #15 for Brian Carter.", null, "DEMO-2-0015", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #15", 1, null, null, 2, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000010"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #16 for Brian Carter.", null, "DEMO-2-0016", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #16", 1, null, null, 2, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-000000000011"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #17 for Brian Carter.", null, "DEMO-2-0017", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #17", 1, null, null, 2, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000012"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #18 for Brian Carter.", null, "DEMO-2-0018", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #18", 1, null, null, 2, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000013"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #19 for Brian Carter.", null, "DEMO-2-0019", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #19", 1, null, null, 2, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000014"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #20 for Brian Carter.", null, "DEMO-2-0020", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #20", 1, null, null, 2, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000015"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #21 for Brian Carter.", null, "DEMO-2-0021", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #21", 1, null, null, 2, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000016"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #22 for Brian Carter.", null, "DEMO-2-0022", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #22", 1, null, null, 2, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000017"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #23 for Brian Carter.", null, "DEMO-2-0023", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #23", 1, null, null, 2, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000018"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #24 for Brian Carter.", null, "DEMO-2-0024", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #24", 1, null, null, 2, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000019"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #25 for Brian Carter.", null, "DEMO-2-0025", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #25", 1, null, null, 2, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-00000000001a"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #26 for Brian Carter.", null, "DEMO-2-0026", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #26", 1, null, null, 2, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000001b"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #27 for Brian Carter.", null, "DEMO-2-0027", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #27", 1, null, null, 2, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000001c"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #28 for Brian Carter.", null, "DEMO-2-0028", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #28", 1, null, null, 2, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000001d"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #29 for Brian Carter.", null, "DEMO-2-0029", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #29", 1, null, null, 2, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000001e"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #30 for Brian Carter.", null, "DEMO-2-0030", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #30", 1, null, null, 2, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000001f"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #31 for Brian Carter.", null, "DEMO-2-0031", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #31", 1, null, null, 2, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-000000000020"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #32 for Brian Carter.", null, "DEMO-2-0032", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #32", 1, null, null, 2, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000021"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #33 for Brian Carter.", null, "DEMO-2-0033", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #33", 1, null, null, 2, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000022"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #34 for Brian Carter.", null, "DEMO-2-0034", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #34", 1, null, null, 2, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000023"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #35 for Brian Carter.", null, "DEMO-2-0035", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #35", 1, null, null, 2, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000024"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #36 for Brian Carter.", null, "DEMO-2-0036", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #36", 1, null, null, 2, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000025"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #37 for Brian Carter.", null, "DEMO-2-0037", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #37", 1, null, null, 2, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000026"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #38 for Brian Carter.", null, "DEMO-2-0038", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #38", 1, null, null, 2, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000027"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #39 for Brian Carter.", null, "DEMO-2-0039", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #39", 1, null, null, 2, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000028"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #40 for Brian Carter.", null, "DEMO-2-0040", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #40", 1, null, null, 2, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-000000000029"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #41 for Brian Carter.", null, "DEMO-2-0041", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #41", 1, null, null, 2, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000002a"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #42 for Brian Carter.", null, "DEMO-2-0042", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #42", 1, null, null, 2, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000002b"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #43 for Brian Carter.", null, "DEMO-2-0043", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #43", 1, null, null, 2, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000002c"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #44 for Brian Carter.", null, "DEMO-2-0044", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #44", 1, null, null, 2, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000002d"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #45 for Brian Carter.", null, "DEMO-2-0045", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #45", 1, null, null, 2, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000002e"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #46 for Brian Carter.", null, "DEMO-2-0046", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #46", 1, null, null, 2, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-00000000002f"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #47 for Brian Carter.", null, "DEMO-2-0047", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #47", 1, null, null, 2, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000030"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #48 for Brian Carter.", null, "DEMO-2-0048", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #48", 1, null, null, 2, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000031"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #49 for Brian Carter.", null, "DEMO-2-0049", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #49", 1, null, null, 2, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000032"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #50 for Brian Carter.", null, "DEMO-2-0050", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #50", 1, null, null, 2, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000033"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #51 for Brian Carter.", null, "DEMO-2-0051", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #51", 1, null, null, 2, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000034"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #52 for Brian Carter.", null, "DEMO-2-0052", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #52", 1, null, null, 2, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000035"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #53 for Brian Carter.", null, "DEMO-2-0053", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #53", 1, null, null, 2, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000036"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #54 for Brian Carter.", null, "DEMO-2-0054", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #54", 1, null, null, 2, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000037"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #55 for Brian Carter.", null, "DEMO-2-0055", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #55", 1, null, null, 2, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-000000000038"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #56 for Brian Carter.", null, "DEMO-2-0056", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #56", 1, null, null, 2, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000039"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #57 for Brian Carter.", null, "DEMO-2-0057", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #57", 1, null, null, 2, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000003a"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #58 for Brian Carter.", null, "DEMO-2-0058", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #58", 1, null, null, 2, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000003b"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #59 for Brian Carter.", null, "DEMO-2-0059", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #59", 1, null, null, 2, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000003c"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #60 for Brian Carter.", null, "DEMO-2-0060", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #60", 1, null, null, 2, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000003d"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #61 for Brian Carter.", null, "DEMO-2-0061", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #61", 1, null, null, 2, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-00000000003e"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #62 for Brian Carter.", null, "DEMO-2-0062", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #62", 1, null, null, 2, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000003f"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #63 for Brian Carter.", null, "DEMO-2-0063", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #63", 1, null, null, 2, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000040"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #64 for Brian Carter.", null, "DEMO-2-0064", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #64", 1, null, null, 2, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000041"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #65 for Brian Carter.", null, "DEMO-2-0065", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #65", 1, null, null, 2, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000042"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #66 for Brian Carter.", null, "DEMO-2-0066", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #66", 1, null, null, 2, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000043"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #67 for Brian Carter.", null, "DEMO-2-0067", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #67", 1, null, null, 2, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000044"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #68 for Brian Carter.", null, "DEMO-2-0068", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #68", 1, null, null, 2, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000045"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #69 for Brian Carter.", null, "DEMO-2-0069", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #69", 1, null, null, 2, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000046"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #70 for Brian Carter.", null, "DEMO-2-0070", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #70", 1, null, null, 2, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-000000000047"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #71 for Brian Carter.", null, "DEMO-2-0071", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #71", 1, null, null, 2, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000048"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #72 for Brian Carter.", null, "DEMO-2-0072", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #72", 1, null, null, 2, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000049"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #73 for Brian Carter.", null, "DEMO-2-0073", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #73", 1, null, null, 2, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-00000000004a"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #74 for Brian Carter.", null, "DEMO-2-0074", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #74", 1, null, null, 2, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000004b"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #75 for Brian Carter.", null, "DEMO-2-0075", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #75", 1, null, null, 2, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000004c"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #76 for Brian Carter.", null, "DEMO-2-0076", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #76", 1, null, null, 2, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-00000000004d"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #77 for Brian Carter.", null, "DEMO-2-0077", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #77", 1, null, null, 2, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000004e"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #78 for Brian Carter.", null, "DEMO-2-0078", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #78", 1, null, null, 2, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000004f"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #79 for Brian Carter.", null, "DEMO-2-0079", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #79", 1, null, null, 2, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-000000000050"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #80 for Brian Carter.", null, "DEMO-2-0080", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #80", 1, null, null, 2, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000051"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #81 for Brian Carter.", null, "DEMO-2-0081", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #81", 1, null, null, 2, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000052"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #82 for Brian Carter.", null, "DEMO-2-0082", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #82", 1, null, null, 2, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000053"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #83 for Brian Carter.", null, "DEMO-2-0083", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #83", 1, null, null, 2, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000054"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #84 for Brian Carter.", null, "DEMO-2-0084", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #84", 1, null, null, 2, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000055"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #85 for Brian Carter.", null, "DEMO-2-0085", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #85", 1, null, null, 2, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("72000000-0000-0000-0000-000000000056"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #86 for Brian Carter.", null, "DEMO-2-0086", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #86", 1, null, null, 2, 47.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000057"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #87 for Brian Carter.", null, "DEMO-2-0087", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #87", 1, null, null, 2, 48.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000058"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #88 for Brian Carter.", null, "DEMO-2-0088", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #88", 1, null, null, 2, 49.99m, 3, true, 49.99m, 44.99m },
                    { new Guid("72000000-0000-0000-0000-000000000059"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #89 for Brian Carter.", null, "DEMO-2-0089", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #89", 1, null, null, 2, 50.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000005a"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #90 for Brian Carter.", null, "DEMO-2-0090", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #90", 1, null, null, 2, 51.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000005b"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #91 for Brian Carter.", null, "DEMO-2-0091", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #91", 1, null, null, 2, 37.99m, 1, true, 37.99m, 34.19m },
                    { new Guid("72000000-0000-0000-0000-00000000005c"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #92 for Brian Carter.", null, "DEMO-2-0092", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #92", 1, null, null, 2, 38.99m, 2, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000005d"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #93 for Brian Carter.", null, "DEMO-2-0093", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #93", 1, null, null, 2, 39.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-00000000005e"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #94 for Brian Carter.", null, "DEMO-2-0094", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #94", 1, null, null, 2, 40.99m, 4, true, 40.99m, 36.89m },
                    { new Guid("72000000-0000-0000-0000-00000000005f"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #95 for Brian Carter.", null, "DEMO-2-0095", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #95", 1, null, null, 2, 41.99m, 5, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000060"), new Guid("10000000-0000-0000-0000-000000000003"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #96 for Brian Carter.", null, "DEMO-2-0096", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #96", 1, null, null, 2, 42.99m, 1, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000061"), new Guid("10000000-0000-0000-0000-000000000004"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #97 for Brian Carter.", null, "DEMO-2-0097", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #97", 1, null, null, 2, 43.99m, 2, true, 43.99m, 39.59m },
                    { new Guid("72000000-0000-0000-0000-000000000062"), new Guid("20000000-0000-0000-0000-000000000002"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #98 for Brian Carter.", null, "DEMO-2-0098", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #98", 1, null, null, 2, 44.99m, 3, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000063"), new Guid("30000000-0000-0000-0000-000000000002"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #99 for Brian Carter.", null, "DEMO-2-0099", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #99", 1, null, null, 2, 45.99m, 4, false, null, null },
                    { new Guid("72000000-0000-0000-0000-000000000064"), new Guid("10000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", null, 0, null, 2, false, "Curated demo listing #100 for Brian Carter.", null, "DEMO-2-0100", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Brian's Item #100", 1, null, null, 2, 46.99m, 5, true, 46.99m, 42.29m },
                    { new Guid("73000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #1 for Cecilia Gomez.", null, "DEMO-3-0001", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #1", 1, null, null, 2, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-000000000002"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #2 for Cecilia Gomez.", null, "DEMO-3-0002", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #2", 1, null, null, 2, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000003"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #3 for Cecilia Gomez.", null, "DEMO-3-0003", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #3", 1, null, null, 2, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000004"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #4 for Cecilia Gomez.", null, "DEMO-3-0004", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #4", 1, null, null, 2, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000005"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #5 for Cecilia Gomez.", null, "DEMO-3-0005", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #5", 1, null, null, 2, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000006"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #6 for Cecilia Gomez.", null, "DEMO-3-0006", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #6", 1, null, null, 2, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000007"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #7 for Cecilia Gomez.", null, "DEMO-3-0007", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #7", 1, null, null, 2, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000008"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #8 for Cecilia Gomez.", null, "DEMO-3-0008", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #8", 1, null, null, 2, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000009"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #9 for Cecilia Gomez.", null, "DEMO-3-0009", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #9", 1, null, null, 2, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000000a"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #10 for Cecilia Gomez.", null, "DEMO-3-0010", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #10", 1, null, null, 2, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-00000000000b"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #11 for Cecilia Gomez.", null, "DEMO-3-0011", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #11", 1, null, null, 2, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000000c"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #12 for Cecilia Gomez.", null, "DEMO-3-0012", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #12", 1, null, null, 2, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000000d"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #13 for Cecilia Gomez.", null, "DEMO-3-0013", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #13", 1, null, null, 2, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000000e"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #14 for Cecilia Gomez.", null, "DEMO-3-0014", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #14", 1, null, null, 2, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000000f"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #15 for Cecilia Gomez.", null, "DEMO-3-0015", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #15", 1, null, null, 2, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000010"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #16 for Cecilia Gomez.", null, "DEMO-3-0016", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #16", 1, null, null, 2, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-000000000011"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #17 for Cecilia Gomez.", null, "DEMO-3-0017", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #17", 1, null, null, 2, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000012"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #18 for Cecilia Gomez.", null, "DEMO-3-0018", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #18", 1, null, null, 2, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000013"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #19 for Cecilia Gomez.", null, "DEMO-3-0019", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #19", 1, null, null, 2, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000014"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #20 for Cecilia Gomez.", null, "DEMO-3-0020", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #20", 1, null, null, 2, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000015"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #21 for Cecilia Gomez.", null, "DEMO-3-0021", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #21", 1, null, null, 2, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000016"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #22 for Cecilia Gomez.", null, "DEMO-3-0022", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #22", 1, null, null, 2, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000017"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #23 for Cecilia Gomez.", null, "DEMO-3-0023", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #23", 1, null, null, 2, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000018"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #24 for Cecilia Gomez.", null, "DEMO-3-0024", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #24", 1, null, null, 2, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000019"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #25 for Cecilia Gomez.", null, "DEMO-3-0025", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #25", 1, null, null, 2, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-00000000001a"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #26 for Cecilia Gomez.", null, "DEMO-3-0026", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #26", 1, null, null, 2, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000001b"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #27 for Cecilia Gomez.", null, "DEMO-3-0027", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #27", 1, null, null, 2, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000001c"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #28 for Cecilia Gomez.", null, "DEMO-3-0028", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #28", 1, null, null, 2, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000001d"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #29 for Cecilia Gomez.", null, "DEMO-3-0029", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #29", 1, null, null, 2, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000001e"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #30 for Cecilia Gomez.", null, "DEMO-3-0030", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #30", 1, null, null, 2, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000001f"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #31 for Cecilia Gomez.", null, "DEMO-3-0031", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #31", 1, null, null, 2, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-000000000020"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #32 for Cecilia Gomez.", null, "DEMO-3-0032", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #32", 1, null, null, 2, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000021"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #33 for Cecilia Gomez.", null, "DEMO-3-0033", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #33", 1, null, null, 2, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000022"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #34 for Cecilia Gomez.", null, "DEMO-3-0034", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #34", 1, null, null, 2, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000023"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #35 for Cecilia Gomez.", null, "DEMO-3-0035", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #35", 1, null, null, 2, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000024"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #36 for Cecilia Gomez.", null, "DEMO-3-0036", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #36", 1, null, null, 2, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000025"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #37 for Cecilia Gomez.", null, "DEMO-3-0037", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #37", 1, null, null, 2, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000026"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #38 for Cecilia Gomez.", null, "DEMO-3-0038", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #38", 1, null, null, 2, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000027"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #39 for Cecilia Gomez.", null, "DEMO-3-0039", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #39", 1, null, null, 2, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000028"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #40 for Cecilia Gomez.", null, "DEMO-3-0040", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #40", 1, null, null, 2, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-000000000029"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #41 for Cecilia Gomez.", null, "DEMO-3-0041", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #41", 1, null, null, 2, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000002a"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #42 for Cecilia Gomez.", null, "DEMO-3-0042", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #42", 1, null, null, 2, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000002b"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #43 for Cecilia Gomez.", null, "DEMO-3-0043", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #43", 1, null, null, 2, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000002c"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #44 for Cecilia Gomez.", null, "DEMO-3-0044", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #44", 1, null, null, 2, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000002d"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #45 for Cecilia Gomez.", null, "DEMO-3-0045", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #45", 1, null, null, 2, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000002e"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #46 for Cecilia Gomez.", null, "DEMO-3-0046", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #46", 1, null, null, 2, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-00000000002f"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #47 for Cecilia Gomez.", null, "DEMO-3-0047", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #47", 1, null, null, 2, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000030"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #48 for Cecilia Gomez.", null, "DEMO-3-0048", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #48", 1, null, null, 2, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000031"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #49 for Cecilia Gomez.", null, "DEMO-3-0049", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #49", 1, null, null, 2, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000032"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #50 for Cecilia Gomez.", null, "DEMO-3-0050", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #50", 1, null, null, 2, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000033"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #51 for Cecilia Gomez.", null, "DEMO-3-0051", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #51", 1, null, null, 2, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000034"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #52 for Cecilia Gomez.", null, "DEMO-3-0052", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #52", 1, null, null, 2, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000035"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #53 for Cecilia Gomez.", null, "DEMO-3-0053", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #53", 1, null, null, 2, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000036"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #54 for Cecilia Gomez.", null, "DEMO-3-0054", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #54", 1, null, null, 2, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000037"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #55 for Cecilia Gomez.", null, "DEMO-3-0055", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #55", 1, null, null, 2, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-000000000038"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #56 for Cecilia Gomez.", null, "DEMO-3-0056", new DateTime(2023, 12, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #56", 1, null, null, 2, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000039"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #57 for Cecilia Gomez.", null, "DEMO-3-0057", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #57", 1, null, null, 2, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000003a"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #58 for Cecilia Gomez.", null, "DEMO-3-0058", new DateTime(2023, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #58", 1, null, null, 2, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000003b"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #59 for Cecilia Gomez.", null, "DEMO-3-0059", new DateTime(2023, 11, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #59", 1, null, null, 2, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000003c"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #60 for Cecilia Gomez.", null, "DEMO-3-0060", new DateTime(2023, 11, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #60", 1, null, null, 2, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000003d"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #61 for Cecilia Gomez.", null, "DEMO-3-0061", new DateTime(2023, 11, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #61", 1, null, null, 2, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-00000000003e"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #62 for Cecilia Gomez.", null, "DEMO-3-0062", new DateTime(2023, 11, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #62", 1, null, null, 2, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000003f"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #63 for Cecilia Gomez.", null, "DEMO-3-0063", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #63", 1, null, null, 2, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000040"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #64 for Cecilia Gomez.", null, "DEMO-3-0064", new DateTime(2023, 11, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #64", 1, null, null, 2, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000041"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #65 for Cecilia Gomez.", null, "DEMO-3-0065", new DateTime(2023, 11, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #65", 1, null, null, 2, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000042"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #66 for Cecilia Gomez.", null, "DEMO-3-0066", new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #66", 1, null, null, 2, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000043"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #67 for Cecilia Gomez.", null, "DEMO-3-0067", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #67", 1, null, null, 2, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000044"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #68 for Cecilia Gomez.", null, "DEMO-3-0068", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #68", 1, null, null, 2, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000045"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #69 for Cecilia Gomez.", null, "DEMO-3-0069", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #69", 1, null, null, 2, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000046"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #70 for Cecilia Gomez.", null, "DEMO-3-0070", new DateTime(2023, 11, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #70", 1, null, null, 2, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-000000000047"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #71 for Cecilia Gomez.", null, "DEMO-3-0071", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #71", 1, null, null, 2, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000048"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #72 for Cecilia Gomez.", null, "DEMO-3-0072", new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #72", 1, null, null, 2, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000049"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #73 for Cecilia Gomez.", null, "DEMO-3-0073", new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #73", 1, null, null, 2, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-00000000004a"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #74 for Cecilia Gomez.", null, "DEMO-3-0074", new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #74", 1, null, null, 2, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000004b"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #75 for Cecilia Gomez.", null, "DEMO-3-0075", new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #75", 1, null, null, 2, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000004c"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #76 for Cecilia Gomez.", null, "DEMO-3-0076", new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #76", 1, null, null, 2, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-00000000004d"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #77 for Cecilia Gomez.", null, "DEMO-3-0077", new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #77", 1, null, null, 2, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000004e"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #78 for Cecilia Gomez.", null, "DEMO-3-0078", new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #78", 1, null, null, 2, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000004f"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #79 for Cecilia Gomez.", null, "DEMO-3-0079", new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #79", 1, null, null, 2, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-000000000050"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #80 for Cecilia Gomez.", null, "DEMO-3-0080", new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #80", 1, null, null, 2, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000051"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #81 for Cecilia Gomez.", null, "DEMO-3-0081", new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #81", 1, null, null, 2, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000052"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #82 for Cecilia Gomez.", null, "DEMO-3-0082", new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #82", 1, null, null, 2, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000053"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #83 for Cecilia Gomez.", null, "DEMO-3-0083", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #83", 1, null, null, 2, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000054"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #84 for Cecilia Gomez.", null, "DEMO-3-0084", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #84", 1, null, null, 2, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000055"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #85 for Cecilia Gomez.", null, "DEMO-3-0085", new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #85", 1, null, null, 2, 54.99m, 5, true, 54.99m, 49.49m },
                    { new Guid("73000000-0000-0000-0000-000000000056"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #86 for Cecilia Gomez.", null, "DEMO-3-0086", new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #86", 1, null, null, 2, 55.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000057"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #87 for Cecilia Gomez.", null, "DEMO-3-0087", new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #87", 1, null, null, 2, 56.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000058"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #88 for Cecilia Gomez.", null, "DEMO-3-0088", new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #88", 1, null, null, 2, 57.99m, 3, true, 57.99m, 52.19m },
                    { new Guid("73000000-0000-0000-0000-000000000059"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #89 for Cecilia Gomez.", null, "DEMO-3-0089", new DateTime(2023, 12, 14, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #89", 1, null, null, 2, 58.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000005a"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #90 for Cecilia Gomez.", null, "DEMO-3-0090", new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #90", 1, null, null, 2, 59.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000005b"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #91 for Cecilia Gomez.", null, "DEMO-3-0091", new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #91", 1, null, null, 2, 45.99m, 1, true, 45.99m, 41.39m },
                    { new Guid("73000000-0000-0000-0000-00000000005c"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #92 for Cecilia Gomez.", null, "DEMO-3-0092", new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #92", 1, null, null, 2, 46.99m, 2, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000005d"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #93 for Cecilia Gomez.", null, "DEMO-3-0093", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #93", 1, null, null, 2, 47.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-00000000005e"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #94 for Cecilia Gomez.", null, "DEMO-3-0094", new DateTime(2023, 12, 9, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #94", 1, null, null, 2, 48.99m, 4, true, 48.99m, 44.09m },
                    { new Guid("73000000-0000-0000-0000-00000000005f"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #95 for Cecilia Gomez.", null, "DEMO-3-0095", new DateTime(2023, 12, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #95", 1, null, null, 2, 49.99m, 5, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000060"), new Guid("10000000-0000-0000-0000-000000000004"), "New with retail tags attached.", new Guid("40000000-0000-0000-0000-000000000007"), new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #96 for Cecilia Gomez.", null, "DEMO-3-0096", new DateTime(2023, 12, 7, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #96", 1, null, null, 2, 50.99m, 1, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000061"), new Guid("20000000-0000-0000-0000-000000000002"), "Open box item inspected for quality.", new Guid("40000000-0000-0000-0000-000000000006"), new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #97 for Cecilia Gomez.", null, "DEMO-3-0097", new DateTime(2023, 12, 6, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #97", 1, null, null, 2, 51.99m, 2, true, 51.99m, 46.79m },
                    { new Guid("73000000-0000-0000-0000-000000000062"), new Guid("30000000-0000-0000-0000-000000000002"), "Gently used and fully functional.", new Guid("40000000-0000-0000-0000-000000000009"), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #98 for Cecilia Gomez.", null, "DEMO-3-0098", new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #98", 1, null, null, 2, 52.99m, 3, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000063"), new Guid("10000000-0000-0000-0000-000000000002"), "Used item in working condition.", new Guid("40000000-0000-0000-0000-000000000004"), new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #99 for Cecilia Gomez.", null, "DEMO-3-0099", new DateTime(2023, 12, 4, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #99", 1, null, null, 2, 53.99m, 4, false, null, null },
                    { new Guid("73000000-0000-0000-0000-000000000064"), new Guid("10000000-0000-0000-0000-000000000003"), "Brand new condition with original packaging.", new Guid("40000000-0000-0000-0000-000000000001"), new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", null, 0, null, 2, false, "Curated demo listing #100 for Cecilia Gomez.", null, "DEMO-3-0100", new DateTime(2023, 12, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Cecilia's Item #100", 1, null, null, 2, 54.99m, 5, true, 54.99m, 49.49m }
                });

            migrationBuilder.InsertData(
                table: "listing_template",
                columns: new[] { "id", "created_at", "created_by", "description", "format_label", "is_deleted", "name", "payload_json", "thumbnail_url", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { new Guid("81000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000001", "Reusable template seeded for demo purposes.", "Fixed Price", false, "Alice's Starter Template", "{\"title\":\"Sample Listing Template\",\"price\":49.99,\"quantity\":5,\"categoryId\":\"10000000-0000-0000-0000-000000000002\",\"conditionId\":\"40000000-0000-0000-0000-000000000001\"}", "https://picsum.photos/seed/template-1/320/180", null, null },
                    { new Guid("82000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000002", "Reusable template seeded for demo purposes.", "Fixed Price", false, "Brian's Starter Template", "{\"title\":\"Sample Listing Template\",\"price\":49.99,\"quantity\":5,\"categoryId\":\"10000000-0000-0000-0000-000000000003\",\"conditionId\":\"40000000-0000-0000-0000-000000000001\"}", "https://picsum.photos/seed/template-2/320/180", null, null },
                    { new Guid("83000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "70000000-0000-0000-0000-000000000003", "Reusable template seeded for demo purposes.", "Fixed Price", false, "Cecilia's Starter Template", "{\"title\":\"Sample Listing Template\",\"price\":49.99,\"quantity\":5,\"categoryId\":\"10000000-0000-0000-0000-000000000004\",\"conditionId\":\"40000000-0000-0000-0000-000000000001\"}", "https://picsum.photos/seed/template-3/320/180", null, null }
                });

            migrationBuilder.InsertData(
                table: "order_statuses",
                columns: new[] { "id", "code", "color", "description", "name", "sort_order" },
                values: new object[,]
                {
                    { new Guid("0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e"), "Archived", "#64748b", "Order archived", "Archived", 11 },
                    { new Guid("2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1"), "Draft", "#94a3b8", "Order created but not submitted", "Draft", 0 },
                    { new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), "AwaitingShipmentShipWithin24h", "#fbbf24", "Must ship within 24 hours", "Ship within 24h", 4 },
                    { new Guid("4d128ab1-64a7-4c65-b8f5-434a258f0c52"), "AwaitingPayment", "#fb923c", "Order awaits buyer payment", "Awaiting payment", 1 },
                    { new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), "PaidAndShipped", "#10b981", "Order shipped to buyer", "Paid & shipped", 6 },
                    { new Guid("5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9"), "PaidAwaitingFeedback", "#a855f7", "Waiting for buyer feedback", "Awaiting feedback", 7 },
                    { new Guid("859b47f4-0d05-4f43-8ff5-57acb8d5da1d"), "AwaitingExpeditedShipment", "#22c55e", "Expedited shipping requested", "Expedited shipment", 5 },
                    { new Guid("949ce7f8-6d6b-4d65-9032-b9f51c4508eb"), "Delivered", "#0ea5e9", "Package delivered to buyer", "Delivered", 9 },
                    { new Guid("970c8d97-6081-43db-9083-8f3c026ded84"), "DeliveryFailed", "#f97316", "Delivery attempt unsuccessful", "Delivery failed", 10 },
                    { new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), "AwaitingShipment", "#3b82f6", "Payment received; waiting to ship", "Awaiting shipment", 2 },
                    { new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd"), "Cancelled", "#ef4444", "Order cancelled", "Cancelled", 12 },
                    { new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930"), "ShippedAwaitingFeedback", "#38bdf8", "Shipped and awaiting buyer confirmation", "Shipped - awaiting feedback", 8 },
                    { new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), "AwaitingShipmentOverdue", "#ef4444", "Shipment overdue based on handling time", "Shipment overdue", 3 }
                });

            migrationBuilder.InsertData(
                table: "user",
                columns: new[] { "id", "created_at", "created_by", "email", "full_name", "is_deleted", "is_email_verified", "is_payment_verified", "password_hash", "performance_level", "updated_at", "updated_by", "username", "active_total_value" },
                values: new object[,]
                {
                    { new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "demo.seller1@example.com", "Alice Johnson", false, true, true, "$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq", "TopRated", null, null, "demo.seller1@example.com", 11222.00m },
                    { new Guid("70000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "demo.seller2@example.com", "Brian Carter", false, true, true, "$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq", "TopRated", null, null, "demo.seller2@example.com", 13622.00m },
                    { new Guid("70000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "demo.seller3@example.com", "Cecilia Gomez", false, true, true, "$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq", "TopRated", null, null, "demo.seller3@example.com", 16022.00m }
                });

            migrationBuilder.InsertData(
                table: "category",
                columns: new[] { "id", "created_at", "created_by", "description", "is_deleted", "name", "parent_id", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Smartphones and cell phone devices.", false, "Cell Phones & Smartphones", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Portable computers and accessories.", false, "Laptops & Netbooks", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("10000000-0000-0000-0000-000000000004"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Digital cameras and photography equipment.", false, "Cameras & Photo", new Guid("10000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Performance and casual athletic footwear for men.", false, "Men's Athletic Shoes", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dresses for every style and occasion.", false, "Women's Dresses", new Guid("20000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Countertop appliances and kitchen helpers.", false, "Small Kitchen Appliances", new Guid("30000000-0000-0000-0000-000000000001"), null, null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Indoor and outdoor furniture collections.", false, "Furniture", new Guid("30000000-0000-0000-0000-000000000001"), null, null }
                });

            migrationBuilder.InsertData(
                table: "listing_id",
                columns: new[] { "listing_id", "seller_id" },
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
                table: "listing_image",
                columns: new[] { "id", "listing_id", "is_primary", "url" },
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
                columns: new[] { "id", "AllowedRoles", "from_status_id", "to_status_id" },
                values: new object[,]
                {
                    { new Guid("1b9c7e7f-9d15-41b0-9417-51c5723a7792"), "SELLER,SYSTEM", new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new Guid("949ce7f8-6d6b-4d65-9032-b9f51c4508eb") },
                    { new Guid("2abdffad-037d-48a0-8c3d-a8dd0f00c5ba"), "SELLER,SYSTEM", new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad"), new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8") },
                    { new Guid("3334f1c8-0fb7-4b17-974a-16f4f492ade4"), "SYSTEM", new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new Guid("dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad") },
                    { new Guid("3cf5a7f5-8f3f-4dcb-907e-e4d27744ef98"), "SELLER,SYSTEM", new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd") },
                    { new Guid("42059f6f-8e43-4b6a-9b59-cf9670091b8f"), "SYSTEM", new Guid("4d128ab1-64a7-4c65-b8f5-434a258f0c52"), new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91") },
                    { new Guid("55b5fadc-7f2f-4f43-ac4c-c6eb6f633d58"), "SELLER,SYSTEM", new Guid("3c8a4f5d-1b89-4a5e-bc53-2612b72d3060"), new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8") },
                    { new Guid("5c76de08-97eb-43d5-9c01-8c7c6262ec66"), "SELLER,BUYER,SYSTEM", new Guid("4d128ab1-64a7-4c65-b8f5-434a258f0c52"), new Guid("ab0ecf06-0e67-4a5d-9820-3a276f59a4fd") },
                    { new Guid("64648c83-2c87-47b8-8c2a-32e96c369f41"), "SELLER,SYSTEM", new Guid("859b47f4-0d05-4f43-8ff5-57acb8d5da1d"), new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8") },
                    { new Guid("6cb6fa65-3d6c-45f0-9f27-cf5d292743ff"), "SELLER,SUPPORT,SYSTEM", new Guid("970c8d97-6081-43db-9083-8f3c026ded84"), new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91") },
                    { new Guid("6fbe36c4-98e4-4d1d-8c3c-1ea29fe8d08c"), "SYSTEM", new Guid("5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9"), new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930") },
                    { new Guid("70e8c0f9-6fa8-4ff6-bb3f-5b53b22e2afd"), "SYSTEM", new Guid("949ce7f8-6d6b-4d65-9032-b9f51c4508eb"), new Guid("c21a6b64-f0e9-4947-8b1b-38ef45aa4930") },
                    { new Guid("8ac18f4b-ea8d-4b72-b6cf-01c3d233cbea"), "SELLER,SYSTEM", new Guid("2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1"), new Guid("4d128ab1-64a7-4c65-b8f5-434a258f0c52") },
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
                columns: new[] { "id", "archived_at", "buyer_id", "cancelled_at", "coupon_code", "created_at", "created_by", "delivered_at", "fulfillment_type", "is_deleted", "order_number", "ordered_at", "paid_at", "promotion_id", "seller_id", "shipped_at", "shipping_status", "status_id", "updated_at", "updated_by", "discount_amount", "discount_currency", "platform_fee_amount", "platform_fee_currency", "shipping_cost_amount", "shipping_cost_currency", "sub_total_amount", "sub_total_currency", "tax_amount", "tax_currency", "total_amount", "total_currency" },
                values: new object[,]
                {
                    { new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, "OCTDEAL", new DateTime(2025, 10, 18, 14, 15, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1002", new DateTime(2025, 10, 18, 14, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 18, 15, 15, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), new DateTime(2025, 10, 19, 2, 15, 0, 0, DateTimeKind.Utc), 2, new Guid("5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8"), new DateTime(2025, 10, 19, 2, 15, 0, 0, DateTimeKind.Utc), "seed", 5.00m, "USD", 4.00m, "USD", 12.00m, "USD", 89.95m, "USD", 6.30m, "USD", 107.25m, "USD" },
                    { new Guid("f6de3ce0-2d3d-4709-923d-cbb61f956947"), null, new Guid("70000000-0000-0000-0000-000000000001"), null, null, new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", null, 0, false, "ORD-SEED-1001", new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 12, 12, 30, 0, 0, DateTimeKind.Utc), null, new Guid("70000000-0000-0000-0000-000000000001"), null, 0, new Guid("9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91"), new DateTime(2025, 10, 12, 12, 30, 0, 0, DateTimeKind.Utc), "seed", 0.00m, "USD", 5.25m, "USD", 8.50m, "USD", 104.99m, "USD", 7.10m, "USD", 125.84m, "USD" }
                });

            migrationBuilder.InsertData(
                table: "category_condition",
                columns: new[] { "category_id", "condition_id" },
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
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000007") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000008") },
                    { new Guid("20000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000009") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new Guid("40000000-0000-0000-0000-000000000006") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000001") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000004") },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new Guid("40000000-0000-0000-0000-000000000006") }
                });

            migrationBuilder.InsertData(
                table: "category_specific",
                columns: new[] { "id", "allow_multiple", "is_required", "name", "values", "category_id" },
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
                    { new Guid("30100000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Breville\",\"Cuisinart\",\"Instant Pot\",\"KitchenAid\",\"Ninja\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000002"), false, true, "Appliance Type", "[\"Air Fryer\",\"Blender\",\"Coffee Maker\",\"Mixer\",\"Pressure Cooker\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000003"), false, false, "Color", "[\"Black\",\"Red\",\"Silver\",\"Stainless Steel\",\"White\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000004"), false, false, "Power Source", "[\"Electric\",\"Battery\",\"Manual\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30100000-0000-0000-0000-000000000005"), false, false, "Capacity", "[\"2 qt\",\"4 qt\",\"6 qt\",\"8 qt\"]", new Guid("30000000-0000-0000-0000-000000000002") },
                    { new Guid("30200000-0000-0000-0000-000000000001"), false, true, "Brand", "[\"Ashley\",\"Crate \\u0026 Barrel\",\"IKEA\",\"West Elm\",\"Wayfair\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30200000-0000-0000-0000-000000000002"), false, true, "Room", "[\"Bedroom\",\"Dining Room\",\"Home Office\",\"Living Room\",\"Patio\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30200000-0000-0000-0000-000000000003"), false, false, "Material", "[\"Fabric\",\"Glass\",\"Leather\",\"Metal\",\"Wood\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30200000-0000-0000-0000-000000000004"), false, false, "Color", "[\"Black\",\"Gray\",\"Natural\",\"White\",\"Walnut\"]", new Guid("30000000-0000-0000-0000-000000000003") },
                    { new Guid("30200000-0000-0000-0000-000000000005"), false, false, "Assembly Required", "[\"No\",\"Yes\"]", new Guid("30000000-0000-0000-0000-000000000003") }
                });

            migrationBuilder.InsertData(
                table: "order_items",
                columns: new[] { "id", "created_at", "created_by", "image_url", "is_deleted", "listing_id", "order_id", "quantity", "sku", "title", "updated_at", "updated_by", "variation_id", "total_price_amount", "total_price_currency", "unit_price_amount", "unit_price_currency" },
                values: new object[,]
                {
                    { new Guid("1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d"), new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", "https://example.com/images/strap.jpg", false, new Guid("c1dbcf74-221e-4e10-9cd6-c4a4060b1baa"), new Guid("f6de3ce0-2d3d-4709-923d-cbb61f956947"), 2, "SKU-ACC-004", "Camera strap pack", new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", null, 45.00m, "USD", 22.50m, "USD" },
                    { new Guid("6cbb0f3e-9fd9-4c83-b181-74d3432fb953"), new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", "https://example.com/images/lens.jpg", false, new Guid("cbebba4e-72dc-4d5d-83b7-2fdd7ecb79d9"), new Guid("f6de3ce0-2d3d-4709-923d-cbb61f956947"), 1, "SKU-VCAM-001", "Vintage camera lens", new DateTime(2025, 10, 12, 10, 30, 0, 0, DateTimeKind.Utc), "seed", null, 59.99m, "USD", 59.99m, "USD" },
                    { new Guid("964d1131-9f9c-4db8-8e6c-86fdb46f1520"), new DateTime(2025, 10, 18, 14, 15, 0, 0, DateTimeKind.Utc), "seed", "https://example.com/images/tripod.jpg", false, new Guid("fbe6aa87-7114-4184-a4f5-89b2b36c27e3"), new Guid("c721f605-43cb-4b1b-8f0c-b1c5833420a9"), 1, "SKU-TRI-010", "Travel tripod kit", new DateTime(2025, 10, 18, 14, 15, 0, 0, DateTimeKind.Utc), "seed", null, 89.95m, "USD", 89.95m, "USD" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_category_parent_id",
                table: "category",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_category_condition_condition_id",
                table: "category_condition",
                column: "condition_id");

            migrationBuilder.CreateIndex(
                name: "ix_category_specific_category_id",
                table: "category_specific",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_listing_id_seller_id",
                table: "listing_id",
                column: "seller_id");

            migrationBuilder.CreateIndex(
                name: "ix_listing_template_name",
                table: "listing_template",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_order_items_order_id",
                table: "order_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_shipping_labels_order_id",
                table: "order_shipping_labels",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_status_histories_from_status_id",
                table: "order_status_histories",
                column: "from_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_status_histories_order_id",
                table: "order_status_histories",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_status_histories_to_status_id",
                table: "order_status_histories",
                column: "to_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_status_transitions_from_status_id",
                table: "order_status_transitions",
                column: "from_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_status_transitions_to_status_id",
                table: "order_status_transitions",
                column: "to_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_statuses_code",
                table: "order_statuses",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_orders_buyer_id",
                table: "orders",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_order_number",
                table: "orders",
                column: "order_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_orders_status_id",
                table: "orders",
                column: "status_id");

            migrationBuilder.CreateIndex(
                name: "ix_otp_email_code",
                table: "otp",
                columns: new[] { "email", "code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_token_user_id",
                table: "refresh_token",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_role_user_user_id",
                table: "role_user",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_email",
                table: "user",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_variation_listing_id",
                table: "variation",
                column: "listing_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_condition");

            migrationBuilder.DropTable(
                name: "category_specific");

            migrationBuilder.DropTable(
                name: "file_metadata");

            migrationBuilder.DropTable(
                name: "listing_id");

            migrationBuilder.DropTable(
                name: "listing_image");

            migrationBuilder.DropTable(
                name: "listing_item_specific");

            migrationBuilder.DropTable(
                name: "listing_template");

            migrationBuilder.DropTable(
                name: "order_items");

            migrationBuilder.DropTable(
                name: "order_shipping_labels");

            migrationBuilder.DropTable(
                name: "order_status_histories");

            migrationBuilder.DropTable(
                name: "order_status_transitions");

            migrationBuilder.DropTable(
                name: "otp");

            migrationBuilder.DropTable(
                name: "outbox_message");

            migrationBuilder.DropTable(
                name: "refresh_token");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "role_user");

            migrationBuilder.DropTable(
                name: "shipping_services");

            migrationBuilder.DropTable(
                name: "variation");

            migrationBuilder.DropTable(
                name: "condition");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "listing");

            migrationBuilder.DropTable(
                name: "order_statuses");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
