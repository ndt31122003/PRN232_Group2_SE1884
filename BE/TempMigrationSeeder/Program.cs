// using System.Text.Json;
// using Npgsql;

// var projectRoot = ResolveProjectRoot();
// var apiAppSettingsPath = Path.Combine(projectRoot, "PRN232_EbayClone", "PRN232_EbayClone.Api", "appsettings.json");

// if (!File.Exists(apiAppSettingsPath))
// {
// 	Console.Error.WriteLine($"Could not find appsettings.json at {apiAppSettingsPath}");
// 	return;
// }

// var connectionString = ReadConnectionString(apiAppSettingsPath);
// if (string.IsNullOrWhiteSpace(connectionString))
// {
// 	Console.Error.WriteLine("DefaultConnection is missing from appsettings.json.");
// 	return;
// }

// var inventorySeeds = BuildInventorySeeds();
// var emailReadySeeds = inventorySeeds
// 	.Where(seed => seed.EmailNotificationsEnabled && seed.IsLowStock && !seed.LastLowStockNotificationAt.HasValue)
// 	.Take(6)
// 	.ToList();

// await using var connection = new NpgsqlConnection(connectionString);
// await connection.OpenAsync();

// await EnsureLowStockEmailColumnExistsAsync(connection);
// await EnsureAdditionalLowStockEmailsColumnExistsAsync(connection);

// await using var transaction = await connection.BeginTransactionAsync();

// foreach (var inventory in inventorySeeds)
// {
// 	await UpdateListingMetadataAsync(connection, transaction, inventory);
// 	var persistedInventoryId = await UpsertInventoryAsync(connection, transaction, inventory);
// 	await ReplaceReservationsAsync(connection, transaction, persistedInventoryId);

// 	foreach (var reservation in inventory.Reservations)
// 	{
// 		await UpsertReservationAsync(connection, transaction, persistedInventoryId, reservation);
// 	}
// }

// await transaction.CommitAsync();

// Console.WriteLine($"Seeded {inventorySeeds.Count} inventory records for alert testing.");
// Console.WriteLine($"- Ready-to-send email cases: {emailReadySeeds.Count}");
// Console.WriteLine($"- Low-stock inventories: {inventorySeeds.Count(seed => seed.IsLowStock)}");
// Console.WriteLine($"- Inventories with reservations: {inventorySeeds.Count(seed => seed.Reservations.Count > 0)}");
// Console.WriteLine("Demo sellers:");
// Console.WriteLine("- demo.seller1@example.com / 123abc@A");
// Console.WriteLine("- demo.seller2@example.com / 123abc@A");
// Console.WriteLine("- demo.seller3@example.com / 123abc@A");
// Console.WriteLine("Email alert test listings (save the alert settings once to trigger mail immediately):");

// foreach (var inventory in emailReadySeeds)
// {
// 	Console.WriteLine($"- {inventory.SellerEmail} | {inventory.ListingTitle} | SKU {inventory.Sku} | ListingId {inventory.ListingId}");
// }

// static string ResolveProjectRoot()
// {
// 	var current = AppContext.BaseDirectory;

// 	for (var i = 0; i < 6; i++)
// 	{
// 		var relativeSegments = Enumerable.Repeat("..", i).ToArray();
// 		var candidate = relativeSegments.Length == 0
// 			? Path.GetFullPath(current)
// 			: Path.GetFullPath(Path.Combine(new[] { current }.Concat(relativeSegments).ToArray()));

// 		if (File.Exists(Path.Combine(candidate, "docker-compose.yml")) ||
// 			Directory.Exists(Path.Combine(candidate, "PRN232_EbayClone")))
// 		{
// 			return candidate;
// 		}
// 	}

// 	return Directory.GetCurrentDirectory();
// }

// static string? ReadConnectionString(string appSettingsPath)
// {
// 	using var document = JsonDocument.Parse(File.ReadAllText(appSettingsPath));

// 	if (!document.RootElement.TryGetProperty("ConnectionStrings", out var connectionStrings))
// 	{
// 		return null;
// 	}

// 	return connectionStrings.TryGetProperty("DefaultConnection", out var defaultConnection)
// 		? defaultConnection.GetString()
// 		: null;
// }

// static async Task EnsureLowStockEmailColumnExistsAsync(NpgsqlConnection connection)
// {
// 	const string sql = """
// 		ALTER TABLE inventory
// 		ADD COLUMN IF NOT EXISTS low_stock_email_enabled boolean NOT NULL DEFAULT false;
// 		""";

// 	await using var command = new NpgsqlCommand(sql, connection);
// 	await command.ExecuteNonQueryAsync();
// }

// static async Task EnsureAdditionalLowStockEmailsColumnExistsAsync(NpgsqlConnection connection)
// {
// 	const string sql = """
// 		ALTER TABLE inventory
// 		ADD COLUMN IF NOT EXISTS additional_low_stock_emails character varying(1000) NOT NULL DEFAULT '';
// 		""";

// 	await using var command = new NpgsqlCommand(sql, connection);
// 	await command.ExecuteNonQueryAsync();
// }

// static async Task UpdateListingMetadataAsync(
// 	NpgsqlConnection connection,
// 	NpgsqlTransaction transaction,
// 	InventorySeed inventory)
// {
// 	const string sql = """
// 		UPDATE listing
// 		SET title = @title,
// 			sku = @sku,
// 			listing_description = @listing_description,
// 			updated_at = @updated_at,
// 			updated_by = @updated_by
// 		WHERE id = @id;
// 		""";

// 	await using var command = new NpgsqlCommand(sql, connection, transaction);
// 	command.Parameters.AddWithValue("id", inventory.ListingId);
// 	command.Parameters.AddWithValue("title", inventory.ListingTitle);
// 	command.Parameters.AddWithValue("sku", inventory.Sku);
// 	command.Parameters.AddWithValue("listing_description", inventory.ListingDescription);
// 	command.Parameters.AddWithValue("updated_at", inventory.TimestampUtc);
// 	command.Parameters.AddWithValue("updated_by", "seed");
// 	await command.ExecuteNonQueryAsync();
// }

// static async Task<Guid> UpsertInventoryAsync(
// 	NpgsqlConnection connection,
// 	NpgsqlTransaction transaction,
// 	InventorySeed inventory)
// {
// 	const string sql = """
// 		INSERT INTO inventory (
// 			id,
// 			listing_id,
// 			seller_id,
// 			total_quantity,
// 			available_quantity,
// 			reserved_quantity,
// 			sold_quantity,
// 			threshold_quantity,
// 			is_low_stock,
// 			low_stock_email_enabled,
// 			additional_low_stock_emails,
// 			last_low_stock_notification_at,
// 			last_updated_at,
// 			created_at,
// 			created_by,
// 			updated_at,
// 			updated_by,
// 			is_deleted)
// 		VALUES (
// 			@id,
// 			@listing_id,
// 			@seller_id,
// 			@total_quantity,
// 			@available_quantity,
// 			@reserved_quantity,
// 			@sold_quantity,
// 			@threshold_quantity,
// 			@is_low_stock,
// 			@low_stock_email_enabled,
// 			@additional_low_stock_emails,
// 			@last_low_stock_notification_at,
// 			@last_updated_at,
// 			@created_at,
// 			@created_by,
// 			@updated_at,
// 			@updated_by,
// 			false)
// 		ON CONFLICT (listing_id)
// 		DO UPDATE SET
// 			seller_id = EXCLUDED.seller_id,
// 			total_quantity = EXCLUDED.total_quantity,
// 			available_quantity = EXCLUDED.available_quantity,
// 			reserved_quantity = EXCLUDED.reserved_quantity,
// 			sold_quantity = EXCLUDED.sold_quantity,
// 			threshold_quantity = EXCLUDED.threshold_quantity,
// 			is_low_stock = EXCLUDED.is_low_stock,
// 			low_stock_email_enabled = EXCLUDED.low_stock_email_enabled,
// 			additional_low_stock_emails = EXCLUDED.additional_low_stock_emails,
// 			last_low_stock_notification_at = EXCLUDED.last_low_stock_notification_at,
// 			last_updated_at = EXCLUDED.last_updated_at,
// 			updated_at = EXCLUDED.updated_at,
// 			updated_by = EXCLUDED.updated_by,
// 			is_deleted = false
// 		RETURNING id;
// 		""";

// 	await using var command = new NpgsqlCommand(sql, connection, transaction);
// 	command.Parameters.AddWithValue("id", inventory.InventoryId);
// 	command.Parameters.AddWithValue("listing_id", inventory.ListingId);
// 	command.Parameters.AddWithValue("seller_id", inventory.SellerId);
// 	command.Parameters.AddWithValue("total_quantity", inventory.TotalQuantity);
// 	command.Parameters.AddWithValue("available_quantity", inventory.AvailableQuantity);
// 	command.Parameters.AddWithValue("reserved_quantity", inventory.ReservedQuantity);
// 	command.Parameters.AddWithValue("sold_quantity", inventory.SoldQuantity);
// 	command.Parameters.AddWithValue("threshold_quantity", (object?)inventory.ThresholdQuantity ?? DBNull.Value);
// 	command.Parameters.AddWithValue("is_low_stock", inventory.IsLowStock);
// 	command.Parameters.AddWithValue("low_stock_email_enabled", inventory.EmailNotificationsEnabled);
// 	command.Parameters.AddWithValue("additional_low_stock_emails", string.Empty);
// 	command.Parameters.AddWithValue("last_low_stock_notification_at", (object?)inventory.LastLowStockNotificationAt ?? DBNull.Value);
// 	command.Parameters.AddWithValue("last_updated_at", inventory.TimestampUtc);
// 	command.Parameters.AddWithValue("created_at", inventory.TimestampUtc);
// 	command.Parameters.AddWithValue("created_by", "seed");
// 	command.Parameters.AddWithValue("updated_at", inventory.TimestampUtc);
// 	command.Parameters.AddWithValue("updated_by", "seed");
// 	return (Guid)(await command.ExecuteScalarAsync() ?? throw new InvalidOperationException("Inventory upsert did not return an id."));
// }

// static async Task ReplaceReservationsAsync(
// 	NpgsqlConnection connection,
// 	NpgsqlTransaction transaction,
// 	Guid inventoryId)
// {
// 	const string sql = "DELETE FROM inventory_reservation WHERE inventory_id = @inventory_id;";

// 	await using var command = new NpgsqlCommand(sql, connection, transaction);
// 	command.Parameters.AddWithValue("inventory_id", inventoryId);
// 	await command.ExecuteNonQueryAsync();
// }

// static async Task UpsertReservationAsync(
// 	NpgsqlConnection connection,
// 	NpgsqlTransaction transaction,
// 	Guid inventoryId,
// 	ReservationSeed reservation)
// {
// 	const string sql = """
// 		INSERT INTO inventory_reservation (
// 			id,
// 			inventory_id,
// 			order_id,
// 			buyer_id,
// 			reservation_type,
// 			quantity,
// 			reserved_at,
// 			expires_at,
// 			released_at,
// 			committed_at)
// 		VALUES (
// 			@id,
// 			@inventory_id,
// 			@order_id,
// 			@buyer_id,
// 			@reservation_type,
// 			@quantity,
// 			@reserved_at,
// 			@expires_at,
// 			@released_at,
// 			@committed_at)
// 		ON CONFLICT (id)
// 		DO UPDATE SET
// 			inventory_id = EXCLUDED.inventory_id,
// 			order_id = EXCLUDED.order_id,
// 			buyer_id = EXCLUDED.buyer_id,
// 			reservation_type = EXCLUDED.reservation_type,
// 			quantity = EXCLUDED.quantity,
// 			reserved_at = EXCLUDED.reserved_at,
// 			expires_at = EXCLUDED.expires_at,
// 			released_at = EXCLUDED.released_at,
// 			committed_at = EXCLUDED.committed_at;
// 		""";

// 	await using var command = new NpgsqlCommand(sql, connection, transaction);
// 	command.Parameters.AddWithValue("id", reservation.ReservationId);
// 	command.Parameters.AddWithValue("inventory_id", inventoryId);
// 	command.Parameters.AddWithValue("order_id", (object?)reservation.OrderId ?? DBNull.Value);
// 	command.Parameters.AddWithValue("buyer_id", reservation.BuyerId);
// 	command.Parameters.AddWithValue("reservation_type", reservation.ReservationType);
// 	command.Parameters.AddWithValue("quantity", reservation.Quantity);
// 	command.Parameters.AddWithValue("reserved_at", reservation.ReservedAtUtc);
// 	command.Parameters.AddWithValue("expires_at", reservation.ExpiresAtUtc);
// 	command.Parameters.AddWithValue("released_at", (object?)reservation.ReleasedAtUtc ?? DBNull.Value);
// 	command.Parameters.AddWithValue("committed_at", (object?)reservation.CommittedAtUtc ?? DBNull.Value);
// 	await command.ExecuteNonQueryAsync();
// }

// static List<InventorySeed> BuildInventorySeeds()
// {
// 	var now = DateTime.UtcNow;
// 	var seeds = new List<InventorySeed>();

// 	for (var sellerIndex = 0; sellerIndex < 3; sellerIndex++)
// 	{
// 		for (var listingIndex = 0; listingIndex < 24; listingIndex++)
// 		{
// 			seeds.Add(CreateInventoryScenario(sellerIndex, listingIndex, now));
// 		}
// 	}

// 	return seeds;
// }

// static InventorySeed CreateInventoryScenario(int sellerIndex, int listingIndex, DateTime now)
// {
// 	return listingIndex switch
// 	{
// 		0 => CreateInventorySeed(sellerIndex, listingIndex, now, 24, 18, 4, 6, true),
// 		1 => CreateInventorySeed(sellerIndex, listingIndex, now, 10, 3, 2, 4, true),
// 		2 => CreateInventorySeed(sellerIndex, listingIndex, now, 20, 0, 0, 2, true, now.AddHours(-12)),
// 		3 => CreateInventorySeed(sellerIndex, listingIndex, now, 16, 4, 5, 6, false),
// 		4 => CreateInventorySeed(sellerIndex, listingIndex, now, 14, 9, 0, null, false),
// 		5 => CreateInventorySeed(sellerIndex, listingIndex, now, 30, 10, 10, 12, true),
// 		_ => CreateGeneratedInventorySeed(sellerIndex, listingIndex, now)
// 	};
// }

// static InventorySeed CreateGeneratedInventorySeed(int sellerIndex, int listingIndex, DateTime now)
// {
// 	var totalQuantity = 18 + (sellerIndex * 4) + (listingIndex % 9);
// 	var reservedQuantity = listingIndex % 4 == 0 ? 0 : (listingIndex % 3) + sellerIndex;
// 	var soldQuantity = (listingIndex % 6) + sellerIndex;
// 	var availableQuantity = Math.Max(1, totalQuantity - reservedQuantity - soldQuantity);
// 	var thresholdQuantity = listingIndex % 5 == 0 ? 5 + sellerIndex : listingIndex % 2 == 0 ? 8 + sellerIndex : (int?)null;
// 	var emailNotificationsEnabled = thresholdQuantity.HasValue && listingIndex % 3 != 0;
// 	DateTime? lastNotificationAt = null;

// 	if (thresholdQuantity.HasValue && availableQuantity <= thresholdQuantity.Value)
// 	{
// 		if (listingIndex % 4 == 0)
// 		{
// 			lastNotificationAt = now.AddHours(-(listingIndex % 7 + 1));
// 		}
// 		else if (listingIndex % 6 == 0)
// 		{
// 			emailNotificationsEnabled = false;
// 		}
// 	}

// 	return CreateInventorySeed(
// 		sellerIndex,
// 		listingIndex,
// 		now,
// 		totalQuantity,
// 		availableQuantity,
// 		reservedQuantity,
// 		thresholdQuantity,
// 		emailNotificationsEnabled,
// 		lastNotificationAt);
// }

// static InventorySeed CreateInventorySeed(
// 	int sellerIndex,
// 	int listingIndex,
// 	DateTime now,
// 	int totalQuantity,
// 	int availableQuantity,
// 	int reservedQuantity,
// 	int? thresholdQuantity,
// 	bool emailNotificationsEnabled,
// 	DateTime? lastLowStockNotificationAt = null)
// {
// 	var sellerId = CreateSellerId(sellerIndex);
// 	var listingId = CreateListingId(sellerIndex, listingIndex);
// 	var soldQuantity = totalQuantity - availableQuantity - reservedQuantity;

// 	if (soldQuantity < 0)
// 	{
// 		throw new InvalidOperationException($"Invalid inventory seed for seller {sellerIndex}, listing {listingIndex}.");
// 	}

// 	return new InventorySeed(
// 		InventoryId: CreateInventoryId(sellerIndex, listingIndex),
// 		ListingId: listingId,
// 		SellerId: sellerId,
// 		SellerEmail: $"demo.seller{sellerIndex + 1}@example.com",
// 		ListingTitle: BuildListingTitle(sellerIndex, listingIndex),
// 		Sku: BuildSku(sellerIndex, listingIndex),
// 		ListingDescription: BuildListingDescription(sellerIndex, listingIndex),
// 		TotalQuantity: totalQuantity,
// 		AvailableQuantity: availableQuantity,
// 		ReservedQuantity: reservedQuantity,
// 		SoldQuantity: soldQuantity,
// 		ThresholdQuantity: thresholdQuantity,
// 		EmailNotificationsEnabled: emailNotificationsEnabled,
// 		LastLowStockNotificationAt: lastLowStockNotificationAt,
// 		TimestampUtc: now,
// 		Reservations: BuildReservations(sellerIndex, listingIndex, reservedQuantity, now));
// }

// static IReadOnlyList<ReservationSeed> BuildReservations(int sellerIndex, int listingIndex, int reservedQuantity, DateTime now)
// {
// 	if (reservedQuantity <= 0)
// 	{
// 		return [];
// 	}

// 	var reservations = new List<ReservationSeed>();
// 	var buyerIds = new[]
// 	{
// 		Guid.Parse("70000000-0000-0000-0000-000000000001"),
// 		Guid.Parse("70000000-0000-0000-0000-000000000002"),
// 		Guid.Parse("70000000-0000-0000-0000-000000000003")
// 	};
// 	var splitCount = reservedQuantity >= 4 ? 2 : 1;
// 	var remaining = reservedQuantity;

// 	for (var reservationIndex = 0; reservationIndex < splitCount; reservationIndex++)
// 	{
// 		var quantity = reservationIndex == splitCount - 1
// 			? remaining
// 			: Math.Max(1, reservedQuantity / 2);

// 		remaining -= quantity;

// 		reservations.Add(new ReservationSeed(
// 			ReservationId: CreateReservationId(sellerIndex, listingIndex, reservationIndex),
// 			BuyerId: buyerIds[(sellerIndex + listingIndex + reservationIndex + 1) % buyerIds.Length],
// 			ReservationType: 0,
// 			Quantity: quantity,
// 			ReservedAtUtc: now.AddMinutes(-(15 + listingIndex + reservationIndex * 7)),
// 			ExpiresAtUtc: now.AddMinutes(20 + listingIndex + reservationIndex * 10),
// 			ReleasedAtUtc: null,
// 			CommittedAtUtc: null,
// 			OrderId: null));
// 	}

// 	return reservations;
// }

// static Guid CreateSellerId(int sellerIndex)
// {
// 	return Guid.Parse($"70000000-0000-0000-0000-{(sellerIndex + 1):x12}");
// }

// static Guid CreateListingId(int sellerIndex, int listingIndex)
// {
// 	var prefix = sellerIndex switch
// 	{
// 		0 => "71000000",
// 		1 => "72000000",
// 		2 => "73000000",
// 		_ => throw new ArgumentOutOfRangeException(nameof(sellerIndex), sellerIndex, "Unsupported seller index")
// 	};

// 	return Guid.Parse($"{prefix}-0000-0000-0000-{(listingIndex + 1):x12}");
// }

// static Guid CreateInventoryId(int sellerIndex, int listingIndex)
// {
// 	var sequence = sellerIndex * 100 + listingIndex + 1;
// 	return Guid.Parse($"91500000-0000-0000-0000-{sequence:x12}");
// }

// static Guid CreateReservationId(int sellerIndex, int listingIndex, int reservationIndex)
// {
// 	var sequence = sellerIndex * 1000 + listingIndex * 10 + reservationIndex + 1;
// 	return Guid.Parse($"94000000-0000-0000-0000-{sequence:x12}");
// }

// static string BuildListingTitle(int sellerIndex, int listingIndex)
// {
// 	var profile = BuildProductProfile(sellerIndex, listingIndex);
// 	return $"{profile.Brand} {profile.Name} {profile.Variant}";
// }

// static string BuildSku(int sellerIndex, int listingIndex)
// {
// 	var profile = BuildProductProfile(sellerIndex, listingIndex);
// 	return $"{profile.CategoryCode}-{sellerIndex + 1:D1}-{listingIndex + 1:D4}";
// }

// static string BuildListingDescription(int sellerIndex, int listingIndex)
// {
// 	var sellerShortName = sellerIndex switch
// 	{
// 		0 => "Alice",
// 		1 => "Brian",
// 		2 => "Cecilia",
// 		_ => throw new ArgumentOutOfRangeException(nameof(sellerIndex), sellerIndex, "Unsupported seller index")
// 	};

// 	var profile = BuildProductProfile(sellerIndex, listingIndex);
// 	return $"{profile.Brand} {profile.Name} {profile.Variant} curated by {sellerShortName} for the demo storefront. Includes seeded stock and alert scenarios for testing.";
// }

// static ProductProfile BuildProductProfile(int sellerIndex, int listingIndex)
// {
// 	var profiles = new[]
// 	{
// 		new ProductProfile("MBL", new[] { "Astra", "Nova", "Pulse", "Orbit", "Zenith" }, new[] { "Smartphone", "5G Phone", "Pocket Camera Phone" }, new[] { "128GB", "256GB", "Pro Edition", "Travel Bundle" }),
// 		new ProductProfile("LTP", new[] { "Northpeak", "Vertex", "Skyline", "Atlas", "Summit" }, new[] { "Laptop", "Ultrabook", "Creator Laptop" }, new[] { "13-inch", "15-inch", "RTX Ready", "Workstation" }),
// 		new ProductProfile("CAM", new[] { "Lumina", "ShutterLab", "FramePro", "Optix", "Aerial" }, new[] { "Mirrorless Camera", "Action Camera", "Vlog Camera" }, new[] { "Starter Kit", "Dual Lens", "4K Edition", "Travel Pack" }),
// 		new ProductProfile("SHO", new[] { "TrailCore", "Aerofit", "PulseRun", "StreetFlex", "MotionLab" }, new[] { "Running Shoes", "Training Shoes", "Athletic Sneakers" }, new[] { "Men's", "Women's", "Lite", "All-Terrain" }),
// 		new ProductProfile("HOM", new[] { "KitchenForge", "HomeNest", "DailyCraft", "Warmtable", "SteamWorks" }, new[] { "Air Fryer", "Espresso Machine", "Blender", "Rice Cooker" }, new[] { "Compact", "Family Size", "Premium", "Stainless" }),
// 	};

// 	var category = profiles[(sellerIndex + listingIndex) % profiles.Length];
// 	var brand = category.Brands[(sellerIndex + listingIndex) % category.Brands.Length];
// 	var name = category.Names[(listingIndex + sellerIndex * 2) % category.Names.Length];
// 	var variant = category.Variants[(listingIndex * 2 + sellerIndex) % category.Variants.Length];

// 	return category with { Brand = brand, Name = name, Variant = variant };
// }

// sealed record InventorySeed(
// 	Guid InventoryId,
// 	Guid ListingId,
// 	Guid SellerId,
// 	string SellerEmail,
// 	string ListingTitle,
// 	string Sku,
// 	string ListingDescription,
// 	int TotalQuantity,
// 	int AvailableQuantity,
// 	int ReservedQuantity,
// 	int SoldQuantity,
// 	int? ThresholdQuantity,
// 	bool EmailNotificationsEnabled,
// 	DateTime? LastLowStockNotificationAt,
// 	DateTime TimestampUtc,
// 	IReadOnlyList<ReservationSeed> Reservations)
// {
// 	public bool IsLowStock => ThresholdQuantity.HasValue && AvailableQuantity <= ThresholdQuantity.Value;
// }

// sealed record ReservationSeed(
// 	Guid ReservationId,
// 	Guid BuyerId,
// 	byte ReservationType,
// 	int Quantity,
// 	DateTime ReservedAtUtc,
// 	DateTime ExpiresAtUtc,
// 	DateTime? ReleasedAtUtc,
// 	DateTime? CommittedAtUtc,
// 	Guid? OrderId);

// sealed record ProductProfile(
// 	string CategoryCode,
// 	string[] Brands,
// 	string[] Names,
// 	string[] Variants)
// {
// 	public string Brand { get; init; } = string.Empty;
// 	public string Name { get; init; } = string.Empty;
// 	public string Variant { get; init; } = string.Empty;
// }
