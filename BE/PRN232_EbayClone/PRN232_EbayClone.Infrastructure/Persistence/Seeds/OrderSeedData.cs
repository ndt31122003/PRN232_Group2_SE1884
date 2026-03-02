using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Seeds;

internal static class OrderSeedData
{
    internal static readonly Guid SellerAliceId = Guid.Parse("70000000-0000-0000-0000-000000000001");
    internal static readonly Guid SellerBrianId = Guid.Parse("70000000-0000-0000-0000-000000000002");
    internal static readonly Guid SellerCeciliaId = Guid.Parse("70000000-0000-0000-0000-000000000003");

    internal static readonly UserId BuyerAliceId = new UserId(SellerAliceId);
    internal static readonly UserId BuyerBrianId = new UserId(SellerBrianId);
    internal static readonly UserId BuyerCeciliaId = new UserId(SellerCeciliaId);

    internal static readonly Guid Order1Id = Guid.Parse("f6de3ce0-2d3d-4709-923d-cbb61f956947");
    internal static readonly Guid Order2Id = Guid.Parse("c721f605-43cb-4b1b-8f0c-b1c5833420a9");
    internal static readonly Guid Order3Id = Guid.Parse("7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a");
    internal static readonly Guid Order4Id = Guid.Parse("1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4");
    internal static readonly Guid Order5Id = Guid.Parse("d2ee4d4a-5be0-4d76-bce6-0b8578c87407");
    internal static readonly Guid Order6Id = Guid.Parse("973cac8a-9be0-44a0-90b7-fd8263f8e78a");
    internal static readonly Guid Order7Id = Guid.Parse("bd34cf77-4551-4194-ad16-d20c94b58289");
    internal static readonly Guid Order8Id = Guid.Parse("a4206ad5-6a35-43bb-8a8c-8c7b244594ac");
    internal static readonly Guid Order9Id = Guid.Parse("1e86f219-1dd0-4cac-a545-cb98e65ce429");
    internal static readonly Guid Order10Id = Guid.Parse("fa236302-3864-4e54-9e40-3ebdb4749734");
    internal static readonly Guid Order11Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000011");
    internal static readonly Guid Order12Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000012");
    internal static readonly Guid Order13Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000013");
    internal static readonly Guid Order14Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000014");
    internal static readonly Guid Order15Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000015");
    internal static readonly Guid Order16Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000016");
    internal static readonly Guid Order17Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000017");
    internal static readonly Guid Order18Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000018");
    internal static readonly Guid Order19Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-000000000019");
    internal static readonly Guid Order20Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-00000000001a");
    internal static readonly Guid Order21Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-00000000001b");
    internal static readonly Guid Order22Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-00000000001c");
    internal static readonly Guid Order23Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-00000000001d");
    internal static readonly Guid Order24Id = Guid.Parse("0f0c1a22-11aa-4c6d-8f10-00000000001e");

    internal static readonly Guid OrderItem1Id = Guid.Parse("6cbb0f3e-9fd9-4c83-b181-74d3432fb953");
    internal static readonly Guid OrderItem2Id = Guid.Parse("1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d");
    internal static readonly Guid OrderItem3Id = Guid.Parse("3e54a8a8-3b35-4bdf-9d09-75042c7f7d4f");
    internal static readonly Guid OrderItem4Id = Guid.Parse("a9d23977-7d99-4d44-bb79-4cff5ec2f56f");
    internal static readonly Guid OrderItem5Id = Guid.Parse("c5b7436e-0ae9-4265-9f2b-7a1fd7e7d78f");
    internal static readonly Guid OrderItem6Id = Guid.Parse("f2a8249e-2643-49b5-bd73-0cac89fb4fc5");
    internal static readonly Guid OrderItem7Id = Guid.Parse("7fdde15f-acca-41c7-97a3-e1df2c6a4b8d");
    internal static readonly Guid OrderItem8Id = Guid.Parse("0a3e9070-0a5e-4114-8634-8e9353a5369e");
    internal static readonly Guid OrderItem9Id = Guid.Parse("e1d40241-43f4-4d93-b9ed-4ac8c9e52088");
    internal static readonly Guid OrderItem10Id = Guid.Parse("4a1ab1de-4a10-4326-a0be-5d3ab27c9df7");
    internal static readonly Guid OrderItem11Id = Guid.Parse("5f2f8987-3b95-4b9f-8cc0-0f7c4b8d3b92");
    internal static readonly Guid OrderItem12Id = Guid.Parse("b7fe44b8-3d3a-49f0-91c5-8ed5cb0c824a");
    internal static readonly Guid OrderItem13Id = Guid.Parse("a3d8f848-7cf3-4058-9f09-3a78d4d64a5d");
    internal static readonly Guid OrderItem14Id = Guid.Parse("8fb2678e-8b5d-4d1e-b079-0fb2aa3a055c");
    internal static readonly Guid OrderItem15Id = Guid.Parse("9be4d720-31f2-4456-94d7-2bf0c76fa0ec");
    internal static readonly Guid OrderItem16Id = Guid.Parse("30f2c0f3-09bb-4f52-93a9-6e98b0171c3f");
    internal static readonly Guid OrderItem17Id = Guid.Parse("6bd3f47d-4f1e-467f-8797-3b2a151dd09f");
    internal static readonly Guid OrderItem18Id = Guid.Parse("e9ad9da9-07b8-42ae-9ce2-764f76d4b657");
    internal static readonly Guid OrderItem19Id = Guid.Parse("6ccf331f-2863-411a-8f9e-1a28857e2a31");
    internal static readonly Guid OrderItem20Id = Guid.Parse("55c9f2a2-dba1-4c66-9b83-a8b4c9e7a0d4");
    internal static readonly Guid OrderItem21Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000021");
    internal static readonly Guid OrderItem22Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000022");
    internal static readonly Guid OrderItem23Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000023");
    internal static readonly Guid OrderItem24Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000024");
    internal static readonly Guid OrderItem25Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000025");
    internal static readonly Guid OrderItem26Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000026");
    internal static readonly Guid OrderItem27Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000027");
    internal static readonly Guid OrderItem28Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000028");
    internal static readonly Guid OrderItem29Id = Guid.Parse("c579fb6b-b172-4e17-b610-000000000029");
    internal static readonly Guid OrderItem30Id = Guid.Parse("c579fb6b-b172-4e17-b610-00000000002a");
    internal static readonly Guid OrderItem31Id = Guid.Parse("c579fb6b-b172-4e17-b610-00000000002b");
    internal static readonly Guid OrderItem32Id = Guid.Parse("c579fb6b-b172-4e17-b610-00000000002c");
    internal static readonly Guid OrderItem33Id = Guid.Parse("c579fb6b-b172-4e17-b610-00000000002d");
    internal static readonly Guid OrderItem34Id = Guid.Parse("c579fb6b-b172-4e17-b610-00000000002e");

    private static readonly DateTime Order1OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 12, 10, 30, 0), DateTimeKind.Utc);
    private static readonly DateTime Order2OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 15, 14, 15, 0), DateTimeKind.Utc);
    private static readonly DateTime Order3OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 18, 9, 45, 0), DateTimeKind.Utc);
    private static readonly DateTime Order4OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 20, 16, 0, 0), DateTimeKind.Utc);
    private static readonly DateTime Order5OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 22, 8, 20, 0), DateTimeKind.Utc);
    private static readonly DateTime Order6OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 24, 11, 5, 0), DateTimeKind.Utc);
    private static readonly DateTime Order7OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 25, 15, 30, 0), DateTimeKind.Utc);
    private static readonly DateTime Order8OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 26, 18, 40, 0), DateTimeKind.Utc);
    private static readonly DateTime Order9OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 28, 12, 10, 0), DateTimeKind.Utc);
    private static readonly DateTime Order10OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 10, 30, 9, 5, 0), DateTimeKind.Utc);
    private static readonly DateTime Order11OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 1, 9, 15, 0), DateTimeKind.Utc);
    private static readonly DateTime Order12OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 2, 13, 30, 0), DateTimeKind.Utc);
    private static readonly DateTime Order13OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 3, 17, 5, 0), DateTimeKind.Utc);
    private static readonly DateTime Order14OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 4, 8, 45, 0), DateTimeKind.Utc);
    private static readonly DateTime Order15OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 5, 10, 0, 0), DateTimeKind.Utc);
    private static readonly DateTime Order16OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 5, 11, 30, 0), DateTimeKind.Utc);
    private static readonly DateTime Order17OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 6, 9, 20, 0), DateTimeKind.Utc);
    private static readonly DateTime Order18OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 6, 13, 45, 0), DateTimeKind.Utc);
    private static readonly DateTime Order19OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 7, 8, 10, 0), DateTimeKind.Utc);
    private static readonly DateTime Order20OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 7, 15, 25, 0), DateTimeKind.Utc);
    private static readonly DateTime Order21OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 8, 10, 40, 0), DateTimeKind.Utc);
    private static readonly DateTime Order22OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 8, 14, 5, 0), DateTimeKind.Utc);
    private static readonly DateTime Order23OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 9, 11, 15, 0), DateTimeKind.Utc);
    private static readonly DateTime Order24OrderedAt = DateTime.SpecifyKind(new DateTime(2025, 11, 9, 17, 50, 0), DateTimeKind.Utc);

    internal static IEnumerable<object> Orders => new object[]
    {
        new
        {
            Id = Order1Id,
            OrderNumber = "ORD-SEED-1001",
            BuyerId = BuyerBrianId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order1OrderedAt,
            PaidAt = Order1OrderedAt.AddHours(2),
            ShippedAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order1OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order1OrderedAt.AddHours(2),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order2Id,
            OrderNumber = "ORD-SEED-1002",
            BuyerId = BuyerCeciliaId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.PaidAndShippedId,
            ShippingStatus = ShippingStatus.Shipped,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order2OrderedAt,
            PaidAt = Order2OrderedAt.AddHours(1),
            ShippedAt = Order2OrderedAt.AddHours(10),
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = "OCTDEAL",
            PromotionId = (Guid?)null,
            CreatedAt = Order2OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order2OrderedAt.AddHours(10),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order3Id,
            OrderNumber = "ORD-SEED-1003",
            BuyerId = BuyerBrianId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.AwaitingShipmentShipWithin24hId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order3OrderedAt,
            PaidAt = Order3OrderedAt.AddHours(1),
            ShippedAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order3OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order3OrderedAt.AddHours(1),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order4Id,
            OrderNumber = "ORD-SEED-1004",
            BuyerId = BuyerCeciliaId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.CancelledId,
            ShippingStatus = ShippingStatus.Returned,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order4OrderedAt,
            PaidAt = (DateTime?)null,
            ShippedAt = (DateTime?)null,
            CancelledAt = Order4OrderedAt.AddDays(1),
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order4OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order4OrderedAt.AddDays(1),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order5Id,
            OrderNumber = "ORD-SEED-1005",
            BuyerId = BuyerAliceId,
            SellerId = SellerBrianId,
            StatusId = OrderStatusSeed.PaidAndShippedId,
            ShippingStatus = ShippingStatus.Shipped,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order5OrderedAt,
            PaidAt = Order5OrderedAt.AddHours(1),
            ShippedAt = Order5OrderedAt.AddHours(9),
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order5OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order5OrderedAt.AddHours(9),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order6Id,
            OrderNumber = "ORD-SEED-1006",
            BuyerId = BuyerCeciliaId,
            SellerId = SellerBrianId,
            StatusId = OrderStatusSeed.AwaitingShipmentOverdueId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order6OrderedAt,
            PaidAt = Order6OrderedAt.AddMinutes(45),
            ShippedAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order6OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order6OrderedAt.AddMinutes(45),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order7Id,
            OrderNumber = "ORD-SEED-1007",
            BuyerId = BuyerAliceId,
            SellerId = SellerBrianId,
            StatusId = OrderStatusSeed.PaidAwaitingFeedbackId,
            ShippingStatus = ShippingStatus.Delivered,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order7OrderedAt,
            PaidAt = Order7OrderedAt.AddHours(2),
            ShippedAt = Order7OrderedAt.AddDays(1),
            DeliveredAt = Order7OrderedAt.AddDays(3),
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order7OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order7OrderedAt.AddDays(3),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order8Id,
            OrderNumber = "ORD-SEED-1008",
            BuyerId = BuyerBrianId,
            SellerId = SellerCeciliaId,
            StatusId = OrderStatusSeed.AwaitingExpeditedShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order8OrderedAt,
            PaidAt = Order8OrderedAt.AddHours(1),
            ShippedAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order8OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order8OrderedAt.AddHours(1),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order9Id,
            OrderNumber = "ORD-SEED-1009",
            BuyerId = BuyerAliceId,
            SellerId = SellerCeciliaId,
            StatusId = OrderStatusSeed.ShippedAwaitingFeedbackId,
            ShippingStatus = ShippingStatus.Shipped,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order9OrderedAt,
            PaidAt = Order9OrderedAt.AddHours(1),
            ShippedAt = Order9OrderedAt.AddHours(10),
            DeliveredAt = Order9OrderedAt.AddDays(3),
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = "HOLIDAY10",
            PromotionId = (Guid?)null,
            CreatedAt = Order9OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order9OrderedAt.AddDays(3),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order10Id,
            OrderNumber = "ORD-SEED-1010",
            BuyerId = BuyerBrianId,
            SellerId = SellerCeciliaId,
            StatusId = OrderStatusSeed.PaidAndShippedId,
            ShippingStatus = ShippingStatus.Shipped,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order10OrderedAt,
            PaidAt = Order10OrderedAt.AddHours(1),
            ShippedAt = Order10OrderedAt.AddHours(13),
            DeliveredAt = Order10OrderedAt.AddDays(4),
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = "BULKBUY",
            PromotionId = (Guid?)null,
            CreatedAt = Order10OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order10OrderedAt.AddDays(4),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order11Id,
            OrderNumber = "ORD-SEED-1011",
            BuyerId = BuyerBrianId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order11OrderedAt,
            PaidAt = Order11OrderedAt.AddMinutes(35),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order11OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order11OrderedAt.AddMinutes(35),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order12Id,
            OrderNumber = "ORD-SEED-1012",
            BuyerId = BuyerAliceId,
            SellerId = SellerBrianId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order12OrderedAt,
            PaidAt = Order12OrderedAt.AddMinutes(40),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order12OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order12OrderedAt.AddMinutes(40),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order13Id,
            OrderNumber = "ORD-SEED-1013",
            BuyerId = BuyerBrianId,
            SellerId = SellerCeciliaId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order13OrderedAt,
            PaidAt = Order13OrderedAt.AddMinutes(50),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order13OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order13OrderedAt.AddMinutes(50),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order14Id,
            OrderNumber = "ORD-SEED-1014",
            BuyerId = BuyerCeciliaId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order14OrderedAt,
            PaidAt = Order14OrderedAt.AddMinutes(28),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order14OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order14OrderedAt.AddMinutes(28),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order15Id,
            OrderNumber = "ORD-SEED-1015",
            BuyerId = BuyerBrianId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order15OrderedAt,
            PaidAt = Order15OrderedAt.AddMinutes(30),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order15OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order15OrderedAt.AddMinutes(30),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order16Id,
            OrderNumber = "ORD-SEED-1016",
            BuyerId = BuyerAliceId,
            SellerId = SellerBrianId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order16OrderedAt,
            PaidAt = Order16OrderedAt.AddMinutes(25),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order16OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order16OrderedAt.AddMinutes(25),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order17Id,
            OrderNumber = "ORD-SEED-1017",
            BuyerId = BuyerBrianId,
            SellerId = SellerCeciliaId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order17OrderedAt,
            PaidAt = Order17OrderedAt.AddMinutes(20),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order17OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order17OrderedAt.AddMinutes(20),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order18Id,
            OrderNumber = "ORD-SEED-1018",
            BuyerId = BuyerCeciliaId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order18OrderedAt,
            PaidAt = Order18OrderedAt.AddMinutes(35),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order18OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order18OrderedAt.AddMinutes(35),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order19Id,
            OrderNumber = "ORD-SEED-1019",
            BuyerId = BuyerCeciliaId,
            SellerId = SellerBrianId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order19OrderedAt,
            PaidAt = Order19OrderedAt.AddMinutes(18),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order19OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order19OrderedAt.AddMinutes(18),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order20Id,
            OrderNumber = "ORD-SEED-1020",
            BuyerId = BuyerAliceId,
            SellerId = SellerCeciliaId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order20OrderedAt,
            PaidAt = Order20OrderedAt.AddMinutes(22),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order20OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order20OrderedAt.AddMinutes(22),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order21Id,
            OrderNumber = "ORD-SEED-1021",
            BuyerId = BuyerBrianId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order21OrderedAt,
            PaidAt = Order21OrderedAt.AddMinutes(27),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order21OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order21OrderedAt.AddMinutes(27),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order22Id,
            OrderNumber = "ORD-SEED-1022",
            BuyerId = BuyerAliceId,
            SellerId = SellerBrianId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order22OrderedAt,
            PaidAt = Order22OrderedAt.AddMinutes(32),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order22OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order22OrderedAt.AddMinutes(32),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order23Id,
            OrderNumber = "ORD-SEED-1023",
            BuyerId = BuyerBrianId,
            SellerId = SellerCeciliaId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order23OrderedAt,
            PaidAt = Order23OrderedAt.AddMinutes(24),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order23OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order23OrderedAt.AddMinutes(24),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = Order24Id,
            OrderNumber = "ORD-SEED-1024",
            BuyerId = BuyerCeciliaId,
            SellerId = SellerAliceId,
            StatusId = OrderStatusSeed.AwaitingShipmentId,
            ShippingStatus = ShippingStatus.Pending,
            FulfillmentType = FulfillmentType.SellerShips,
            OrderedAt = Order24OrderedAt,
            PaidAt = Order24OrderedAt.AddMinutes(29),
            ShippedAt = (DateTime?)null,
            DeliveredAt = (DateTime?)null,
            CancelledAt = (DateTime?)null,
            ArchivedAt = (DateTime?)null,
            CouponCode = (string?)null,
            PromotionId = (Guid?)null,
            CreatedAt = Order24OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order24OrderedAt.AddMinutes(29),
            UpdatedBy = "seed",
            IsDeleted = false
        }
    };

    internal static IEnumerable<object> SubTotals => new object[]
    {
        new { OrderId = Order1Id, Amount = 93.97m, Currency = "USD" },
        new { OrderId = Order2Id, Amount = 66.98m, Currency = "USD" },
        new { OrderId = Order3Id, Amount = 70.98m, Currency = "USD" },
        new { OrderId = Order4Id, Amount = 74.98m, Currency = "USD" },
        new { OrderId = Order5Id, Amount = 76.98m, Currency = "USD" },
        new { OrderId = Order6Id, Amount = 79.98m, Currency = "USD" },
        new { OrderId = Order7Id, Amount = 84.98m, Currency = "USD" },
        new { OrderId = Order8Id, Amount = 92.98m, Currency = "USD" },
        new { OrderId = Order9Id, Amount = 145.97m, Currency = "USD" },
        new { OrderId = Order10Id, Amount = 152.97m, Currency = "USD" },
        new { OrderId = Order11Id, Amount = 58.99m, Currency = "USD" },
        new { OrderId = Order12Id, Amount = 64.50m, Currency = "USD" },
        new { OrderId = Order13Id, Amount = 72.00m, Currency = "USD" },
        new { OrderId = Order14Id, Amount = 55.75m, Currency = "USD" },
        new { OrderId = Order15Id, Amount = 62.75m, Currency = "USD" },
        new { OrderId = Order16Id, Amount = 48.40m, Currency = "USD" },
        new { OrderId = Order17Id, Amount = 79.90m, Currency = "USD" },
        new { OrderId = Order18Id, Amount = 88.60m, Currency = "USD" },
        new { OrderId = Order19Id, Amount = 71.25m, Currency = "USD" },
        new { OrderId = Order20Id, Amount = 65.80m, Currency = "USD" },
        new { OrderId = Order21Id, Amount = 59.10m, Currency = "USD" },
        new { OrderId = Order22Id, Amount = 83.45m, Currency = "USD" },
        new { OrderId = Order23Id, Amount = 90.30m, Currency = "USD" },
        new { OrderId = Order24Id, Amount = 74.95m, Currency = "USD" }
    };

    internal static IEnumerable<object> ShippingCosts => new object[]
    {
        new { OrderId = Order1Id, Amount = 8.50m, Currency = "USD" },
        new { OrderId = Order2Id, Amount = 12.00m, Currency = "USD" },
        new { OrderId = Order3Id, Amount = 9.25m, Currency = "USD" },
        new { OrderId = Order4Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order5Id, Amount = 10.00m, Currency = "USD" },
        new { OrderId = Order6Id, Amount = 11.00m, Currency = "USD" },
        new { OrderId = Order7Id, Amount = 12.50m, Currency = "USD" },
        new { OrderId = Order8Id, Amount = 13.00m, Currency = "USD" },
        new { OrderId = Order9Id, Amount = 14.00m, Currency = "USD" },
        new { OrderId = Order10Id, Amount = 15.00m, Currency = "USD" },
        new { OrderId = Order11Id, Amount = 9.95m, Currency = "USD" },
        new { OrderId = Order12Id, Amount = 8.25m, Currency = "USD" },
        new { OrderId = Order13Id, Amount = 10.00m, Currency = "USD" },
        new { OrderId = Order14Id, Amount = 7.80m, Currency = "USD" },
        new { OrderId = Order15Id, Amount = 8.25m, Currency = "USD" },
        new { OrderId = Order16Id, Amount = 7.50m, Currency = "USD" },
        new { OrderId = Order17Id, Amount = 9.95m, Currency = "USD" },
        new { OrderId = Order18Id, Amount = 10.25m, Currency = "USD" },
        new { OrderId = Order19Id, Amount = 8.75m, Currency = "USD" },
        new { OrderId = Order20Id, Amount = 8.40m, Currency = "USD" },
        new { OrderId = Order21Id, Amount = 7.95m, Currency = "USD" },
        new { OrderId = Order22Id, Amount = 9.10m, Currency = "USD" },
        new { OrderId = Order23Id, Amount = 10.60m, Currency = "USD" },
        new { OrderId = Order24Id, Amount = 8.90m, Currency = "USD" }
    };

    internal static IEnumerable<object> PlatformFees => new object[]
    {
        new { OrderId = Order1Id, Amount = 4.70m, Currency = "USD" },
        new { OrderId = Order2Id, Amount = 3.35m, Currency = "USD" },
        new { OrderId = Order3Id, Amount = 3.40m, Currency = "USD" },
        new { OrderId = Order4Id, Amount = 3.25m, Currency = "USD" },
        new { OrderId = Order5Id, Amount = 4.10m, Currency = "USD" },
        new { OrderId = Order6Id, Amount = 3.99m, Currency = "USD" },
        new { OrderId = Order7Id, Amount = 4.20m, Currency = "USD" },
        new { OrderId = Order8Id, Amount = 4.60m, Currency = "USD" },
        new { OrderId = Order9Id, Amount = 6.20m, Currency = "USD" },
        new { OrderId = Order10Id, Amount = 7.20m, Currency = "USD" },
        new { OrderId = Order11Id, Amount = 3.10m, Currency = "USD" },
        new { OrderId = Order12Id, Amount = 3.45m, Currency = "USD" },
        new { OrderId = Order13Id, Amount = 3.90m, Currency = "USD" },
        new { OrderId = Order14Id, Amount = 2.95m, Currency = "USD" },
        new { OrderId = Order15Id, Amount = 3.15m, Currency = "USD" },
        new { OrderId = Order16Id, Amount = 2.45m, Currency = "USD" },
        new { OrderId = Order17Id, Amount = 3.95m, Currency = "USD" },
        new { OrderId = Order18Id, Amount = 4.30m, Currency = "USD" },
        new { OrderId = Order19Id, Amount = 3.55m, Currency = "USD" },
        new { OrderId = Order20Id, Amount = 3.25m, Currency = "USD" },
        new { OrderId = Order21Id, Amount = 2.95m, Currency = "USD" },
        new { OrderId = Order22Id, Amount = 3.80m, Currency = "USD" },
        new { OrderId = Order23Id, Amount = 4.45m, Currency = "USD" },
        new { OrderId = Order24Id, Amount = 3.60m, Currency = "USD" }
    };

    internal static IEnumerable<object> TaxAmounts => new object[]
    {
        new { OrderId = Order1Id, Amount = 7.52m, Currency = "USD" },
        new { OrderId = Order2Id, Amount = 5.36m, Currency = "USD" },
        new { OrderId = Order3Id, Amount = 6.20m, Currency = "USD" },
        new { OrderId = Order4Id, Amount = 5.50m, Currency = "USD" },
        new { OrderId = Order5Id, Amount = 6.16m, Currency = "USD" },
        new { OrderId = Order6Id, Amount = 6.40m, Currency = "USD" },
        new { OrderId = Order7Id, Amount = 7.20m, Currency = "USD" },
        new { OrderId = Order8Id, Amount = 8.60m, Currency = "USD" },
        new { OrderId = Order9Id, Amount = 10.60m, Currency = "USD" },
        new { OrderId = Order10Id, Amount = 12.20m, Currency = "USD" },
        new { OrderId = Order11Id, Amount = 5.30m, Currency = "USD" },
        new { OrderId = Order12Id, Amount = 4.86m, Currency = "USD" },
        new { OrderId = Order13Id, Amount = 6.12m, Currency = "USD" },
        new { OrderId = Order14Id, Amount = 4.46m, Currency = "USD" },
        new { OrderId = Order15Id, Amount = 5.02m, Currency = "USD" },
        new { OrderId = Order16Id, Amount = 3.87m, Currency = "USD" },
        new { OrderId = Order17Id, Amount = 6.39m, Currency = "USD" },
        new { OrderId = Order18Id, Amount = 7.08m, Currency = "USD" },
        new { OrderId = Order19Id, Amount = 5.70m, Currency = "USD" },
        new { OrderId = Order20Id, Amount = 4.94m, Currency = "USD" },
        new { OrderId = Order21Id, Amount = 4.43m, Currency = "USD" },
        new { OrderId = Order22Id, Amount = 6.68m, Currency = "USD" },
        new { OrderId = Order23Id, Amount = 7.22m, Currency = "USD" },
        new { OrderId = Order24Id, Amount = 5.83m, Currency = "USD" }
    };

    internal static IEnumerable<object> DiscountAmounts => new object[]
    {
        new { OrderId = Order1Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order2Id, Amount = 5.00m, Currency = "USD" },
        new { OrderId = Order3Id, Amount = 2.00m, Currency = "USD" },
        new { OrderId = Order4Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order5Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order6Id, Amount = 4.00m, Currency = "USD" },
        new { OrderId = Order7Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order8Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order9Id, Amount = 7.50m, Currency = "USD" },
        new { OrderId = Order10Id, Amount = 10.00m, Currency = "USD" },
        new { OrderId = Order11Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order12Id, Amount = 2.50m, Currency = "USD" },
        new { OrderId = Order13Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order14Id, Amount = 1.20m, Currency = "USD" },
        new { OrderId = Order15Id, Amount = 2.50m, Currency = "USD" },
        new { OrderId = Order16Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order17Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order18Id, Amount = 3.00m, Currency = "USD" },
        new { OrderId = Order19Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order20Id, Amount = 1.80m, Currency = "USD" },
        new { OrderId = Order21Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order22Id, Amount = 2.20m, Currency = "USD" },
        new { OrderId = Order23Id, Amount = 0.00m, Currency = "USD" },
        new { OrderId = Order24Id, Amount = 1.50m, Currency = "USD" }
    };

    internal static IEnumerable<object> Totals => new object[]
    {
        new { OrderId = Order1Id, Amount = 114.69m, Currency = "USD" },
        new { OrderId = Order2Id, Amount = 82.69m, Currency = "USD" },
        new { OrderId = Order3Id, Amount = 87.83m, Currency = "USD" },
        new { OrderId = Order4Id, Amount = 83.73m, Currency = "USD" },
        new { OrderId = Order5Id, Amount = 97.24m, Currency = "USD" },
        new { OrderId = Order6Id, Amount = 97.37m, Currency = "USD" },
        new { OrderId = Order7Id, Amount = 108.88m, Currency = "USD" },
        new { OrderId = Order8Id, Amount = 119.18m, Currency = "USD" },
        new { OrderId = Order9Id, Amount = 169.27m, Currency = "USD" },
        new { OrderId = Order10Id, Amount = 177.37m, Currency = "USD" },
        new { OrderId = Order11Id, Amount = 77.34m, Currency = "USD" },
        new { OrderId = Order12Id, Amount = 78.56m, Currency = "USD" },
        new { OrderId = Order13Id, Amount = 92.02m, Currency = "USD" },
        new { OrderId = Order14Id, Amount = 69.76m, Currency = "USD" },
        new { OrderId = Order15Id, Amount = 76.67m, Currency = "USD" },
        new { OrderId = Order16Id, Amount = 62.22m, Currency = "USD" },
        new { OrderId = Order17Id, Amount = 100.19m, Currency = "USD" },
        new { OrderId = Order18Id, Amount = 107.23m, Currency = "USD" },
        new { OrderId = Order19Id, Amount = 89.25m, Currency = "USD" },
        new { OrderId = Order20Id, Amount = 80.59m, Currency = "USD" },
        new { OrderId = Order21Id, Amount = 74.43m, Currency = "USD" },
        new { OrderId = Order22Id, Amount = 100.83m, Currency = "USD" },
        new { OrderId = Order23Id, Amount = 112.57m, Currency = "USD" },
        new { OrderId = Order24Id, Amount = 91.78m, Currency = "USD" }
    };

    internal static IEnumerable<object> OrderItems => new object[]
    {
        new
        {
            Id = OrderItem1Id,
            OrderId = Order1Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-000000000001"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #1",
            ImageUrl = "https://picsum.photos/seed/1-1/640/640",
            Sku = "DEMO-1-0001",
            Quantity = 1,
            CreatedAt = Order1OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order1OrderedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem2Id,
            OrderId = Order1Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-000000000003"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #3",
            ImageUrl = "https://picsum.photos/seed/1-3/640/640",
            Sku = "DEMO-1-0003",
            Quantity = 2,
            CreatedAt = Order1OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order1OrderedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem3Id,
            OrderId = Order2Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-000000000004"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #4",
            ImageUrl = "https://picsum.photos/seed/1-4/640/640",
            Sku = "DEMO-1-0004",
            Quantity = 1,
            CreatedAt = Order2OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order2OrderedAt.AddHours(10),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem4Id,
            OrderId = Order2Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-000000000005"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #5",
            ImageUrl = "https://picsum.photos/seed/1-5/640/640",
            Sku = "DEMO-1-0005",
            Quantity = 1,
            CreatedAt = Order2OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order2OrderedAt.AddHours(10),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem5Id,
            OrderId = Order3Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-000000000006"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #6",
            ImageUrl = "https://picsum.photos/seed/1-6/640/640",
            Sku = "DEMO-1-0006",
            Quantity = 1,
            CreatedAt = Order3OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order3OrderedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem6Id,
            OrderId = Order3Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-000000000007"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #7",
            ImageUrl = "https://picsum.photos/seed/1-7/640/640",
            Sku = "DEMO-1-0007",
            Quantity = 1,
            CreatedAt = Order3OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order3OrderedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem7Id,
            OrderId = Order4Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-000000000008"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #8",
            ImageUrl = "https://picsum.photos/seed/1-8/640/640",
            Sku = "DEMO-1-0008",
            Quantity = 1,
            CreatedAt = Order4OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order4OrderedAt.AddDays(1),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem8Id,
            OrderId = Order4Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-000000000009"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #9",
            ImageUrl = "https://picsum.photos/seed/1-9/640/640",
            Sku = "DEMO-1-0009",
            Quantity = 1,
            CreatedAt = Order4OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order4OrderedAt.AddDays(1),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem9Id,
            OrderId = Order5Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000001"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #1",
            ImageUrl = "https://picsum.photos/seed/2-1/640/640",
            Sku = "DEMO-2-0001",
            Quantity = 1,
            CreatedAt = Order5OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order5OrderedAt.AddHours(9),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem10Id,
            OrderId = Order5Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000002"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #2",
            ImageUrl = "https://picsum.photos/seed/2-2/640/640",
            Sku = "DEMO-2-0002",
            Quantity = 1,
            CreatedAt = Order5OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order5OrderedAt.AddHours(9),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem11Id,
            OrderId = Order6Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000003"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #3",
            ImageUrl = "https://picsum.photos/seed/2-3/640/640",
            Sku = "DEMO-2-0003",
            Quantity = 2,
            CreatedAt = Order6OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order6OrderedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem12Id,
            OrderId = Order7Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000004"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #4",
            ImageUrl = "https://picsum.photos/seed/2-4/640/640",
            Sku = "DEMO-2-0004",
            Quantity = 1,
            CreatedAt = Order7OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order7OrderedAt.AddDays(3),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem13Id,
            OrderId = Order7Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000005"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #5",
            ImageUrl = "https://picsum.photos/seed/2-5/640/640",
            Sku = "DEMO-2-0005",
            Quantity = 1,
            CreatedAt = Order7OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order7OrderedAt.AddDays(3),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem14Id,
            OrderId = Order8Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000001"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #1",
            ImageUrl = "https://picsum.photos/seed/3-1/640/640",
            Sku = "DEMO-3-0001",
            Quantity = 1,
            CreatedAt = Order8OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order8OrderedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem15Id,
            OrderId = Order8Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000002"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #2",
            ImageUrl = "https://picsum.photos/seed/3-2/640/640",
            Sku = "DEMO-3-0002",
            Quantity = 1,
            CreatedAt = Order8OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order8OrderedAt,
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem16Id,
            OrderId = Order9Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000003"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #3",
            ImageUrl = "https://picsum.photos/seed/3-3/640/640",
            Sku = "DEMO-3-0003",
            Quantity = 1,
            CreatedAt = Order9OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order9OrderedAt.AddDays(3),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem17Id,
            OrderId = Order9Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000004"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #4",
            ImageUrl = "https://picsum.photos/seed/3-4/640/640",
            Sku = "DEMO-3-0004",
            Quantity = 2,
            CreatedAt = Order9OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order9OrderedAt.AddDays(3),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem18Id,
            OrderId = Order10Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000005"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #5",
            ImageUrl = "https://picsum.photos/seed/3-5/640/640",
            Sku = "DEMO-3-0005",
            Quantity = 1,
            CreatedAt = Order10OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order10OrderedAt.AddDays(4),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem19Id,
            OrderId = Order10Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000006"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #6",
            ImageUrl = "https://picsum.photos/seed/3-6/640/640",
            Sku = "DEMO-3-0006",
            Quantity = 1,
            CreatedAt = Order10OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order10OrderedAt.AddDays(4),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem20Id,
            OrderId = Order10Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000007"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #7",
            ImageUrl = "https://picsum.photos/seed/3-7/640/640",
            Sku = "DEMO-3-0007",
            Quantity = 1,
            CreatedAt = Order10OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order10OrderedAt.AddDays(4),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem21Id,
            OrderId = Order11Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-00000000000a"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #10",
            ImageUrl = "https://picsum.photos/seed/1-10/640/640",
            Sku = "DEMO-1-0010",
            Quantity = 1,
            CreatedAt = Order11OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order11OrderedAt.AddMinutes(35),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem22Id,
            OrderId = Order12Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000006"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #6",
            ImageUrl = "https://picsum.photos/seed/2-6/640/640",
            Sku = "DEMO-2-0006",
            Quantity = 1,
            CreatedAt = Order12OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order12OrderedAt.AddMinutes(40),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem23Id,
            OrderId = Order13Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000008"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #8",
            ImageUrl = "https://picsum.photos/seed/3-8/640/640",
            Sku = "DEMO-3-0008",
            Quantity = 1,
            CreatedAt = Order13OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order13OrderedAt.AddMinutes(50),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem24Id,
            OrderId = Order14Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-00000000000b"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #11",
            ImageUrl = "https://picsum.photos/seed/1-11/640/640",
            Sku = "DEMO-1-0011",
            Quantity = 1,
            CreatedAt = Order14OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order14OrderedAt.AddMinutes(28),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem25Id,
            OrderId = Order15Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-00000000000c"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #12",
            ImageUrl = "https://picsum.photos/seed/1-12/640/640",
            Sku = "DEMO-1-0012",
            Quantity = 1,
            CreatedAt = Order15OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order15OrderedAt.AddMinutes(30),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem26Id,
            OrderId = Order16Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000007"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #7",
            ImageUrl = "https://picsum.photos/seed/2-7/640/640",
            Sku = "DEMO-2-0007",
            Quantity = 1,
            CreatedAt = Order16OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order16OrderedAt.AddMinutes(25),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem27Id,
            OrderId = Order17Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-000000000009"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #9",
            ImageUrl = "https://picsum.photos/seed/3-9/640/640",
            Sku = "DEMO-3-0009",
            Quantity = 1,
            CreatedAt = Order17OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order17OrderedAt.AddMinutes(20),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem28Id,
            OrderId = Order18Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-00000000000d"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #13",
            ImageUrl = "https://picsum.photos/seed/1-13/640/640",
            Sku = "DEMO-1-0013",
            Quantity = 1,
            CreatedAt = Order18OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order18OrderedAt.AddMinutes(35),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem29Id,
            OrderId = Order19Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000008"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #8",
            ImageUrl = "https://picsum.photos/seed/2-8/640/640",
            Sku = "DEMO-2-0008",
            Quantity = 1,
            CreatedAt = Order19OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order19OrderedAt.AddMinutes(18),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem30Id,
            OrderId = Order20Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-00000000000a"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #10",
            ImageUrl = "https://picsum.photos/seed/3-10/640/640",
            Sku = "DEMO-3-0010",
            Quantity = 1,
            CreatedAt = Order20OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order20OrderedAt.AddMinutes(22),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem31Id,
            OrderId = Order21Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-00000000000e"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #14",
            ImageUrl = "https://picsum.photos/seed/1-14/640/640",
            Sku = "DEMO-1-0014",
            Quantity = 1,
            CreatedAt = Order21OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order21OrderedAt.AddMinutes(27),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem32Id,
            OrderId = Order22Id,
            ListingId = Guid.Parse("72000000-0000-0000-0000-000000000009"),
            VariationId = (Guid?)null,
            Title = "Brian's Item #9",
            ImageUrl = "https://picsum.photos/seed/2-9/640/640",
            Sku = "DEMO-2-0009",
            Quantity = 1,
            CreatedAt = Order22OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order22OrderedAt.AddMinutes(32),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem33Id,
            OrderId = Order23Id,
            ListingId = Guid.Parse("73000000-0000-0000-0000-00000000000b"),
            VariationId = (Guid?)null,
            Title = "Cecilia's Item #11",
            ImageUrl = "https://picsum.photos/seed/3-11/640/640",
            Sku = "DEMO-3-0011",
            Quantity = 1,
            CreatedAt = Order23OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order23OrderedAt.AddMinutes(24),
            UpdatedBy = "seed",
            IsDeleted = false
        },
        new
        {
            Id = OrderItem34Id,
            OrderId = Order24Id,
            ListingId = Guid.Parse("71000000-0000-0000-0000-00000000000f"),
            VariationId = (Guid?)null,
            Title = "Alice's Item #15",
            ImageUrl = "https://picsum.photos/seed/1-15/640/640",
            Sku = "DEMO-1-0015",
            Quantity = 1,
            CreatedAt = Order24OrderedAt,
            CreatedBy = "seed",
            UpdatedAt = Order24OrderedAt.AddMinutes(29),
            UpdatedBy = "seed",
            IsDeleted = false
        }
    };

    internal static IEnumerable<object> OrderItemUnitPrices => new object[]
    {
        new { OrderItemId = OrderItem1Id, Amount = 29.99m, Currency = "USD" },
        new { OrderItemId = OrderItem2Id, Amount = 31.99m, Currency = "USD" },
        new { OrderItemId = OrderItem3Id, Amount = 32.99m, Currency = "USD" },
        new { OrderItemId = OrderItem4Id, Amount = 33.99m, Currency = "USD" },
        new { OrderItemId = OrderItem5Id, Amount = 34.99m, Currency = "USD" },
        new { OrderItemId = OrderItem6Id, Amount = 35.99m, Currency = "USD" },
        new { OrderItemId = OrderItem7Id, Amount = 36.99m, Currency = "USD" },
        new { OrderItemId = OrderItem8Id, Amount = 37.99m, Currency = "USD" },
        new { OrderItemId = OrderItem9Id, Amount = 37.99m, Currency = "USD" },
        new { OrderItemId = OrderItem10Id, Amount = 38.99m, Currency = "USD" },
        new { OrderItemId = OrderItem11Id, Amount = 39.99m, Currency = "USD" },
    new { OrderItemId = OrderItem12Id, Amount = 42.99m, Currency = "USD" },
        new { OrderItemId = OrderItem13Id, Amount = 41.99m, Currency = "USD" },
        new { OrderItemId = OrderItem14Id, Amount = 45.99m, Currency = "USD" },
        new { OrderItemId = OrderItem15Id, Amount = 46.99m, Currency = "USD" },
        new { OrderItemId = OrderItem16Id, Amount = 47.99m, Currency = "USD" },
        new { OrderItemId = OrderItem17Id, Amount = 48.99m, Currency = "USD" },
        new { OrderItemId = OrderItem18Id, Amount = 49.99m, Currency = "USD" },
        new { OrderItemId = OrderItem19Id, Amount = 50.99m, Currency = "USD" },
        new { OrderItemId = OrderItem20Id, Amount = 51.99m, Currency = "USD" },
        new { OrderItemId = OrderItem21Id, Amount = 58.99m, Currency = "USD" },
        new { OrderItemId = OrderItem22Id, Amount = 64.50m, Currency = "USD" },
        new { OrderItemId = OrderItem23Id, Amount = 72.00m, Currency = "USD" },
        new { OrderItemId = OrderItem24Id, Amount = 55.75m, Currency = "USD" },
        new { OrderItemId = OrderItem25Id, Amount = 62.75m, Currency = "USD" },
        new { OrderItemId = OrderItem26Id, Amount = 48.40m, Currency = "USD" },
        new { OrderItemId = OrderItem27Id, Amount = 79.90m, Currency = "USD" },
        new { OrderItemId = OrderItem28Id, Amount = 88.60m, Currency = "USD" },
        new { OrderItemId = OrderItem29Id, Amount = 71.25m, Currency = "USD" },
        new { OrderItemId = OrderItem30Id, Amount = 65.80m, Currency = "USD" },
        new { OrderItemId = OrderItem31Id, Amount = 59.10m, Currency = "USD" },
        new { OrderItemId = OrderItem32Id, Amount = 83.45m, Currency = "USD" },
        new { OrderItemId = OrderItem33Id, Amount = 90.30m, Currency = "USD" },
        new { OrderItemId = OrderItem34Id, Amount = 74.95m, Currency = "USD" }
    };

    internal static IEnumerable<object> OrderItemTotalPrices => new object[]
    {
        new { OrderItemId = OrderItem1Id, Amount = 29.99m, Currency = "USD" },
        new { OrderItemId = OrderItem2Id, Amount = 63.98m, Currency = "USD" },
        new { OrderItemId = OrderItem3Id, Amount = 32.99m, Currency = "USD" },
        new { OrderItemId = OrderItem4Id, Amount = 33.99m, Currency = "USD" },
        new { OrderItemId = OrderItem5Id, Amount = 34.99m, Currency = "USD" },
        new { OrderItemId = OrderItem6Id, Amount = 35.99m, Currency = "USD" },
        new { OrderItemId = OrderItem7Id, Amount = 36.99m, Currency = "USD" },
        new { OrderItemId = OrderItem8Id, Amount = 37.99m, Currency = "USD" },
        new { OrderItemId = OrderItem9Id, Amount = 37.99m, Currency = "USD" },
        new { OrderItemId = OrderItem10Id, Amount = 38.99m, Currency = "USD" },
        new { OrderItemId = OrderItem11Id, Amount = 79.98m, Currency = "USD" },
    new { OrderItemId = OrderItem12Id, Amount = 42.99m, Currency = "USD" },
        new { OrderItemId = OrderItem13Id, Amount = 41.99m, Currency = "USD" },
        new { OrderItemId = OrderItem14Id, Amount = 45.99m, Currency = "USD" },
        new { OrderItemId = OrderItem15Id, Amount = 46.99m, Currency = "USD" },
        new { OrderItemId = OrderItem16Id, Amount = 47.99m, Currency = "USD" },
        new { OrderItemId = OrderItem17Id, Amount = 97.98m, Currency = "USD" },
        new { OrderItemId = OrderItem18Id, Amount = 49.99m, Currency = "USD" },
        new { OrderItemId = OrderItem19Id, Amount = 50.99m, Currency = "USD" },
        new { OrderItemId = OrderItem20Id, Amount = 51.99m, Currency = "USD" },
        new { OrderItemId = OrderItem21Id, Amount = 58.99m, Currency = "USD" },
        new { OrderItemId = OrderItem22Id, Amount = 64.50m, Currency = "USD" },
        new { OrderItemId = OrderItem23Id, Amount = 72.00m, Currency = "USD" },
        new { OrderItemId = OrderItem24Id, Amount = 55.75m, Currency = "USD" },
        new { OrderItemId = OrderItem25Id, Amount = 62.75m, Currency = "USD" },
        new { OrderItemId = OrderItem26Id, Amount = 48.40m, Currency = "USD" },
        new { OrderItemId = OrderItem27Id, Amount = 79.90m, Currency = "USD" },
        new { OrderItemId = OrderItem28Id, Amount = 88.60m, Currency = "USD" },
        new { OrderItemId = OrderItem29Id, Amount = 71.25m, Currency = "USD" },
        new { OrderItemId = OrderItem30Id, Amount = 65.80m, Currency = "USD" },
        new { OrderItemId = OrderItem31Id, Amount = 59.10m, Currency = "USD" },
        new { OrderItemId = OrderItem32Id, Amount = 83.45m, Currency = "USD" },
        new { OrderItemId = OrderItem33Id, Amount = 90.30m, Currency = "USD" },
        new { OrderItemId = OrderItem34Id, Amount = 74.95m, Currency = "USD" }
    };
}