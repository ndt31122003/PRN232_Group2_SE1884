import dayjs from "dayjs";

const PRICE_FORMATTER = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
});

const normalizeOrderItem = (item) => {
    if (!item) {
        return {
            id: "",
            title: "Untitled item",
            quantity: 0,
            imageUrl: "",
        };
    }

    const resolvedQuantity = Number(item?.quantity ?? item?.Quantity ?? item?.qty ?? 0);

    return {
        id: item?.id ?? item?.Id ?? item?.orderItemId ?? item?.OrderItemId ?? "",
        title: item?.title ?? item?.Title ?? "Untitled item",
        quantity: Number.isFinite(resolvedQuantity) ? resolvedQuantity : 0,
        imageUrl: item?.imageUrl ?? item?.ImageUrl ?? "",
        sku: item?.sku ?? item?.Sku ?? "",
    };
};

export const normalizeOrderForBulk = (order) => {
    if (!order) {
        return {
            id: "",
            orderNumber: "",
            buyerFullName: "",
            buyerUsername: "",
            shipToName: null,
            shipToAddress: null,
            items: [],
            subTotal: null,
            total: null,
            shippingCost: null,
            taxAmount: null,
            orderedAt: null,
            paidAt: null,
            statusName: "",
            statusCode: "",
            sellerFeedback: null,
        };
    }

    const rawItems = order?.items ?? order?.Items ?? [];
    const normalizedItems = Array.isArray(rawItems) ? rawItems.map(normalizeOrderItem) : [];

    const feedback = order?.sellerFeedback ?? order?.SellerFeedback ?? null;

    return {
        id: order?.id ?? order?.Id ?? "",
        orderNumber: order?.orderNumber ?? order?.OrderNumber ?? "",
        buyerFullName: order?.buyerFullName ?? order?.BuyerFullName ?? "",
        buyerUsername: order?.buyerUsername ?? order?.BuyerUsername ?? "",
        shipToName: order?.shipToName ?? order?.ShipToName ?? null,
        shipToAddress: order?.shipToAddress ?? order?.ShipToAddress ?? null,
        items: normalizedItems,
        subTotal: order?.subTotal ?? order?.SubTotal ?? null,
        total: order?.total ?? order?.Total ?? null,
        shippingCost: order?.shippingCost ?? order?.ShippingCost ?? null,
        taxAmount: order?.taxAmount ?? order?.TaxAmount ?? null,
        orderedAt: order?.orderedAt ?? order?.OrderedAt ?? null,
        paidAt: order?.paidAt ?? order?.PaidAt ?? null,
        statusName: order?.statusName ?? order?.StatusName ?? "",
        statusCode: order?.statusCode ?? order?.StatusCode ?? "",
        sellerFeedback: feedback,
    };
};

export const collectBulkOrderContext = (location) => {
    const fromPath = location.state?.from ?? "/order/all?status=all";
    const stateOrders = Array.isArray(location.state?.orders) ? location.state.orders : [];
    const stateOrderIds = Array.isArray(location.state?.orderIds) ? location.state.orderIds : [];

    const searchParams = new URLSearchParams(location.search ?? "");
    const idsParam = searchParams.get("ids");
    const queryIds = idsParam
        ? idsParam
            .split(",")
            .map((value) => value.trim())
            .filter((value) => value.length > 0)
        : [];

    const normalizedSnapshots = stateOrders.map(normalizeOrderForBulk).filter((order) => order.id);
    const snapshotMap = normalizedSnapshots.reduce((accumulator, order) => {
        accumulator[order.id] = order;
        return accumulator;
    }, {});

    const derivedIds = normalizedSnapshots.map((order) => order.id);
    const orderIds = Array.from(new Set([...stateOrderIds, ...queryIds, ...derivedIds]));

    return {
        fromPath,
        orderIds,
        snapshotMap,
    };
};

const flattenAddressObject = (addressObject) => {
    if (!addressObject || typeof addressObject !== "object") {
        return [];
    }

    const streetParts = [
        addressObject.addressLine1 ?? addressObject.line1 ?? addressObject.street1 ?? null,
        addressObject.addressLine2 ?? addressObject.line2 ?? addressObject.street2 ?? null,
        addressObject.addressLine3 ?? addressObject.line3 ?? addressObject.street3 ?? null,
    ].filter(Boolean);

    const localityParts = [
        addressObject.city ?? addressObject.cityName ?? addressObject.town ?? null,
        addressObject.state ?? addressObject.stateOrProvince ?? addressObject.province ?? addressObject.region ?? null,
        addressObject.postalCode ?? addressObject.zip ?? addressObject.postCode ?? null,
    ].filter(Boolean);

    const country = addressObject.country ?? addressObject.countryName ?? addressObject.countryCode ?? null;

    const lines = [];
    if (streetParts.length > 0) {
        lines.push(streetParts.join(" ").trim());
    }
    if (localityParts.length > 0) {
        lines.push(localityParts.join(", ").trim());
    }
    if (country) {
        lines.push(String(country).trim());
    }

    return lines.filter((line) => line.length > 0);
};

export const formatAddressLines = (address) => {
    if (!address) {
        return [];
    }

    if (Array.isArray(address)) {
        return address
            .map((line) => String(line ?? "").trim())
            .filter((line) => line.length > 0);
    }

    if (typeof address === "object") {
        return flattenAddressObject(address);
    }

    return String(address)
        .split(/\r?\n/)
        .map((line) => line.trim())
        .filter((line) => line.length > 0);
};

export const formatCurrency = (value) => {
    if (value === null || value === undefined || Number.isNaN(value)) {
        return "-";
    }
    return PRICE_FORMATTER.format(value);
};

export const formatDate = (value) => {
    if (!value) {
        return "-";
    }
    const parsed = dayjs(value);
    return parsed.isValid() ? parsed.format("MMM D, YYYY") : "-";
};

export const buyerDisplayName = (order) => {
    if (!order) {
        return "Unknown buyer";
    }
    const fullName = (order.buyerFullName ?? "").trim();
    if (fullName.length > 0) {
        return fullName;
    }
    const username = (order.buyerUsername ?? "").trim();
    if (username.length > 0) {
        return username;
    }
    return "Unknown buyer";
};
