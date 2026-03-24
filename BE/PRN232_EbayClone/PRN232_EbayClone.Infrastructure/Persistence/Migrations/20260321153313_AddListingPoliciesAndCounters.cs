using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddListingPoliciesAndCounters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bids_count",
                table: "listing",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "return_policy_id",
                table: "listing",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "shipping_policy_id",
                table: "listing",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "watchers_count",
                table: "listing",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000001"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000002"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000003"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000004"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000005"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000006"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000007"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000008"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000009"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000000f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000010"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000011"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000012"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000013"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000014"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000015"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000016"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000017"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000018"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000019"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000001f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000020"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000021"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000022"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000023"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000024"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000025"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000026"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000027"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000028"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000029"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000002f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000030"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000031"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000032"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000033"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000034"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000035"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000036"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000037"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000038"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000039"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000003f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000040"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000041"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000042"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000043"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000044"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000045"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000046"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000047"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000048"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000049"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000004f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000050"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000051"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000052"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000053"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000054"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000055"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000056"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000057"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000058"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000059"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-00000000005f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000060"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000061"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000062"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000063"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("71000000-0000-0000-0000-000000000064"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000001"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000002"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000003"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000004"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000005"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000006"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000007"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000008"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000009"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000000f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000010"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000011"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000012"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000013"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000014"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000015"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000016"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000017"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000018"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000019"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000001f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000020"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000021"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000022"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000023"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000024"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000025"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000026"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000027"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000028"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000029"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000002f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000030"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000031"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000032"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000033"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000034"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000035"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000036"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000037"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000038"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000039"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000003f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000040"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000041"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000042"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000043"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000044"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000045"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000046"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000047"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000048"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000049"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000004f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000050"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000051"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000052"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000053"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000054"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000055"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000056"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000057"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000058"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000059"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-00000000005f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000060"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000061"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000062"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000063"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("72000000-0000-0000-0000-000000000064"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000001"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000002"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000003"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000004"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000005"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000006"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000007"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000008"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000009"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000000f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000010"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000011"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000012"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000013"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000014"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000015"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000016"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000017"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000018"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000019"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000001f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000020"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000021"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000022"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000023"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000024"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000025"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000026"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000027"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000028"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000029"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000002f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000030"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000031"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000032"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000033"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000034"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000035"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000036"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000037"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000038"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000039"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000003f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000040"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000041"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000042"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000043"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000044"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000045"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000046"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000047"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000048"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000049"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000004f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000050"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000051"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000052"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000053"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000054"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000055"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000056"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000057"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000058"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000059"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005a"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005b"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005c"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005d"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005e"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-00000000005f"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000060"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000061"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000062"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000063"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });

            migrationBuilder.UpdateData(
                table: "listing",
                keyColumn: "id",
                keyValue: new Guid("73000000-0000-0000-0000-000000000064"),
                columns: new[] { "return_policy_id", "shipping_policy_id", "watchers_count" },
                values: new object[] { null, null, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bids_count",
                table: "listing");

            migrationBuilder.DropColumn(
                name: "return_policy_id",
                table: "listing");

            migrationBuilder.DropColumn(
                name: "shipping_policy_id",
                table: "listing");

            migrationBuilder.DropColumn(
                name: "watchers_count",
                table: "listing");
        }
    }
}
