using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryIdToOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "category_id",
                table: "order_items",
                type: "uuid",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("0a3e9070-0a5e-4114-8634-8e9353a5369e"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("30f2c0f3-09bb-4f52-93a9-6e98b0171c3f"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("3e54a8a8-3b35-4bdf-9d09-75042c7f7d4f"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("4a1ab1de-4a10-4326-a0be-5d3ab27c9df7"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("55c9f2a2-dba1-4c66-9b83-a8b4c9e7a0d4"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("5f2f8987-3b95-4b9f-8cc0-0f7c4b8d3b92"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("6bd3f47d-4f1e-467f-8797-3b2a151dd09f"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("6cbb0f3e-9fd9-4c83-b181-74d3432fb953"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("6ccf331f-2863-411a-8f9e-1a28857e2a31"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("7fdde15f-acca-41c7-97a3-e1df2c6a4b8d"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("8fb2678e-8b5d-4d1e-b079-0fb2aa3a055c"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("9be4d720-31f2-4456-94d7-2bf0c76fa0ec"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("a3d8f848-7cf3-4058-9f09-3a78d4d64a5d"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("a9d23977-7d99-4d44-bb79-4cff5ec2f56f"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("b7fe44b8-3d3a-49f0-91c5-8ed5cb0c824a"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000021"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000022"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000023"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000024"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000025"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000026"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000027"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000028"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-000000000029"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002a"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002b"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002c"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002d"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c579fb6b-b172-4e17-b610-00000000002e"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("c5b7436e-0ae9-4265-9f2b-7a1fd7e7d78f"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("e1d40241-43f4-4d93-b9ed-4ac8c9e52088"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("e9ad9da9-07b8-42ae-9ce2-764f76d4b657"),
                column: "category_id",
                value: null);

            migrationBuilder.UpdateData(
                table: "order_items",
                keyColumn: "id",
                keyValue: new Guid("f2a8249e-2643-49b5-bd73-0cac89fb4fc5"),
                column: "category_id",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category_id",
                table: "order_items");
        }
    }
}
