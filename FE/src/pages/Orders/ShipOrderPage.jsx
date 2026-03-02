import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import dayjs from "dayjs";
import OrderService from "../../services/OrderService";
import ShippingServiceApi from "../../services/ShippingService";
import axiosInstance from "../../utils/axiosCustomize";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import "./ShipOrderPage.scss";

const DEFAULT_CARRIER_TAB = "eBay";

const LABEL_DELIVERY_FORMATS = [
    {
        value: "pdf",
        label: "Download a PDF file",
        description: "Save and print from your device.",
    },
    {
        value: "qr",
        label: "Get a QR code",
        description: "Show the code at the carrier to print in-store.",
    },
];

const LABEL_PAPER_SIZES = [
    { value: "4x6", label: "4\" x 6\" label" },
    { value: "4x6.75", label: "4\" x 6.75\" label" },
    { value: "8x11", label: "8.5\" x 11\" letter" },
];

const PAYMENT_METHODS = [
    { value: "ebay-balance", label: "eBay balance" },
    { value: "paypal", label: "PayPal" },
    { value: "card", label: "Credit or debit card" },
];

const PRICE_FORMATTER = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
});

const DEFAULT_PAGE_SIZE = { widthIn: 4, heightIn: 6 };

const buildDefaultShippingForm = () => ({
    packaging: "custom",
    weightLb: "1",
    weightOz: "0",
    lengthIn: "12",
    widthIn: "10",
    heightIn: "4",
    shipOn: dayjs().format("YYYY-MM-DD"),
    includeHazmat: false,
    serviceId: "",
    paymentMethod: PAYMENT_METHODS[0]?.value ?? "ebay-balance",
});

const buildDefaultLabelPreferences = () => ({
    deliveryFormat: LABEL_DELIVERY_FORMATS[0]?.value ?? "pdf",
    paperSize: LABEL_PAPER_SIZES[0]?.value ?? "4x6",
});

const normalizeOrder = (order) => ({
    id: order?.id ?? order?.Id ?? "",
    orderNumber: order?.orderNumber ?? order?.OrderNumber ?? "",
    buyerFullName: order?.buyerFullName ?? order?.BuyerFullName ?? "",
    buyerUsername: order?.buyerUsername ?? order?.BuyerUsername ?? "",
    shipToName: order?.shipToName ?? order?.ShipToName ?? null,
    shipToAddress: order?.shipToAddress ?? order?.ShipToAddress ?? null,
    items: order?.items ?? order?.Items ?? [],
    subTotal: order?.subTotal ?? order?.SubTotal ?? null,
    total: order?.total ?? order?.Total ?? null,
    shippingCost: order?.shippingCost ?? order?.ShippingCost ?? null,
    taxAmount: order?.taxAmount ?? order?.TaxAmount ?? null,
    orderedAt: order?.orderedAt ?? order?.OrderedAt ?? null,
    paidAt: order?.paidAt ?? order?.PaidAt ?? null,
});

const formatCurrency = (value) => {
    if (value === null || value === undefined) {
        return "-";
    }
    return PRICE_FORMATTER.format(value);
};

const formatDate = (value) => {
    if (!value) {
        return "-";
    }
    const parsed = dayjs(value);
    return parsed.isValid() ? parsed.format("MMM D, YYYY") : "-";
};

const parseDecimal = (value, fallback = 0) => {
    const numeric = Number(value);
    return Number.isFinite(numeric) ? numeric : fallback;
};

const convertWeightToOz = (pounds, ounces) => {
    const lb = Math.max(0, parseDecimal(pounds));
    const oz = Math.max(0, parseDecimal(ounces));
    return lb * 16 + oz;
};

const parsePaperSize = (value) => {
    if (!value) {
        return DEFAULT_PAGE_SIZE;
    }

    const normalized = String(value).trim().toLowerCase();
    const knownSizes = {
        "4x6": { widthIn: 4, heightIn: 6 },
        "4x6.75": { widthIn: 4, heightIn: 6.75 },
        "8x11": { widthIn: 8.5, heightIn: 11 },
        "8.5x11": { widthIn: 8.5, heightIn: 11 },
        letter: { widthIn: 8.5, heightIn: 11 },
    };

    if (knownSizes[normalized]) {
        return knownSizes[normalized];
    }

    const match = normalized.match(/^(\d+(?:\.\d+)?)\s*x\s*(\d+(?:\.\d+)?)$/);
    if (match) {
        return {
            widthIn: parseDecimal(match[1], DEFAULT_PAGE_SIZE.widthIn),
            heightIn: parseDecimal(match[2], DEFAULT_PAGE_SIZE.heightIn),
        };
    }

    return DEFAULT_PAGE_SIZE;
};

const buildAbsoluteUrl = (path) => {
    if (!path) {
        return "";
    }
    if (/^https?:/i.test(path)) {
        return path;
    }

    const base = axiosInstance.defaults?.baseURL ?? window.location.origin;
    try {
        const normalizedBase = base.replace(/\/$/, "");
        const apiTrimmed = normalizedBase.endsWith("/api") ? normalizedBase.slice(0, -4) : normalizedBase;
        return new URL(path, `${apiTrimmed}/`).toString();
    } catch (error) {
        console.warn("Unable to build label URL", error);
        return path;
    }
};

const fallbackDeliveryWindowLabel = (label, minDays, maxDays) => {
    if (label && label.trim().length > 0) {
        return label;
    }

    const min = Number(minDays);
    const max = Number(maxDays);

    if (Number.isFinite(min) && Number.isFinite(max)) {
        if (min === max) {
            const dayLabel = min === 1 ? "day" : "days";
            return `In ${min} business ${dayLabel}`;
        }
        return `${min}-${max} business days`;
    }

    if (Number.isFinite(min)) {
        return `In about ${min} business days`;
    }

    return "";
};

const ShipOrderPage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const { orderId } = useParams();

    const orderFromState = location.state?.order ?? null;
    const fromPath = location.state?.from ?? "/order/all?status=all";

    const [order, setOrder] = useState(() => (orderFromState ? normalizeOrder(orderFromState) : null));
    const [loading, setLoading] = useState(!orderFromState);
    const [error, setError] = useState("");
    const [shippingForm, setShippingForm] = useState(() => buildDefaultShippingForm());
    const [shippingServices, setShippingServices] = useState([]);
    const [shippingServicesLoading, setShippingServicesLoading] = useState(true);
    const [shippingServicesError, setShippingServicesError] = useState("");
    const [carrierTab, setCarrierTab] = useState(DEFAULT_CARRIER_TAB);
    const [labelPreferences, setLabelPreferences] = useState(() => buildDefaultLabelPreferences());
    const [preferencesOpen, setPreferencesOpen] = useState(false);
    const [submissionState, setSubmissionState] = useState({ isSubmitting: false, error: "", result: null });

    useEffect(() => {
        if (order || !orderId) {
            return;
        }

        let ignore = false;
        const controller = new AbortController();

        const fetchOrder = async () => {
            try {
                setLoading(true);
                setError("");
                const response = await OrderService.getOrderById(orderId, controller.signal);
                if (ignore) {
                    return;
                }
                const data = response?.data ?? response;
                setOrder(normalizeOrder(data));
            } catch (err) {
                if (controller.signal.aborted) {
                    return;
                }
                console.error("Failed to load order detail", err);
                setError("Unable to load order details. Return to orders and try again.");
            } finally {
                if (!ignore) {
                    setLoading(false);
                }
            }
        };

        fetchOrder();

        return () => {
            ignore = true;
            controller.abort();
        };
    }, [order, orderId]);

    useEffect(() => {
        const controller = new AbortController();

        const fetchShippingServices = async () => {
            try {
                setShippingServicesLoading(true);
                setShippingServicesError("");

                const response = await ShippingServiceApi.list(controller.signal);
                const data = response?.data ?? response;

                if (!Array.isArray(data)) {
                    setShippingServices([]);
                    return;
                }

                const normalized = data
                    .filter((service) => service)
                    .map((service) => {
                        const price = Number(service?.baseCost?.amount ?? 0);
                        return {
                            id: service?.slug ?? service?.id ?? "",
                            shippingServiceId: service?.id ?? "",
                            carrier: service?.carrier ?? "",
                            serviceCode: service?.serviceCode ?? service?.slug ?? "",
                            name: service?.serviceName ?? "",
                            savings: service?.savingsDescription ?? "",
                            printerRequired: Boolean(service?.printerRequired),
                            coverage: service?.coverageDescription ?? "",
                            window: fallbackDeliveryWindowLabel(
                                service?.deliveryWindowLabel,
                                service?.minEstimatedDeliveryDays,
                                service?.maxEstimatedDeliveryDays
                            ),
                            price,
                            currency: service?.baseCost?.currency ?? "USD",
                            notes: service?.notes ?? "",
                            qrCode: Boolean(service?.supportsQrCode),
                            minDeliveryDays: service?.minEstimatedDeliveryDays ?? null,
                            maxDeliveryDays: service?.maxEstimatedDeliveryDays ?? null,
                        };
                    })
                    .filter((service) => service.id);

                setShippingServices(normalized);
            } catch (err) {
                if (!controller.signal.aborted) {
                    console.error("Failed to load shipping services", err);
                    setShippingServicesError("Unable to load shipping services. Refresh and try again.");
                    setShippingServices([]);
                }
            } finally {
                if (!controller.signal.aborted) {
                    setShippingServicesLoading(false);
                }
            }
        };

        fetchShippingServices();

        return () => {
            controller.abort();
        };
    }, []);

    useEffect(() => {
        if (shippingServices.length === 0) {
            return;
        }

        setShippingForm((prev) => {
            if (!prev) {
                return prev;
            }

            if (prev.serviceId) {
                const exists = shippingServices.some((service) => service.id === prev.serviceId);
                if (exists) {
                    return prev;
                }
            }

            return { ...prev, serviceId: shippingServices[0].id };
        });
    }, [shippingServices]);

    const carrierTabs = useMemo(() => {
        if (shippingServices.length === 0) {
            return [DEFAULT_CARRIER_TAB];
        }

        const carrierSet = new Set(
            shippingServices
                .map((service) => service.carrier)
                .filter((carrier) => typeof carrier === "string" && carrier.trim().length > 0)
        );

        const carrierList = Array.from(carrierSet).sort((a, b) => a.localeCompare(b));

        return [DEFAULT_CARRIER_TAB, ...carrierList];
    }, [shippingServices]);

    useEffect(() => {
        if (!carrierTabs.includes(carrierTab)) {
            setCarrierTab(carrierTabs[0] ?? DEFAULT_CARRIER_TAB);
        }
    }, [carrierTabs, carrierTab]);

    const servicesForCarrier = useMemo(() => {
        if (shippingServices.length === 0) {
            return [];
        }
        if (carrierTab === DEFAULT_CARRIER_TAB) {
            return shippingServices;
        }
        return shippingServices.filter((service) => service.carrier === carrierTab);
    }, [carrierTab, shippingServices]);

    useEffect(() => {
        setShippingForm((prev) => {
            if (!prev) {
                return prev;
            }
            if (servicesForCarrier.length === 0) {
                return prev;
            }
            const hasSelectedInCarrier = servicesForCarrier.some((service) => service.id === prev.serviceId);
            if (hasSelectedInCarrier) {
                return prev;
            }
            return { ...prev, serviceId: servicesForCarrier[0].id };
        });
    }, [servicesForCarrier]);

    const selectedService = useMemo(() => {
        if (!shippingForm?.serviceId) {
            return null;
        }
        return shippingServices.find((service) => service.id === shippingForm.serviceId) ?? null;
    }, [shippingForm?.serviceId, shippingServices]);

    const orderItems = useMemo(() => (Array.isArray(order?.items) ? order.items : []), [order]);
    const primaryItem = orderItems[0] ?? null;
    const remainingItems = Math.max(0, orderItems.length - 1);

    const handleBack = () => {
        navigate(fromPath, { replace: true });
    };

    const handlePackagingChange = (event) => {
        const nextValue = event.target.value;
        setShippingForm((prev) => ({ ...prev, packaging: nextValue }));
    };

    const handleFieldChange = (field) => (event) => {
        const value = event?.target?.value ?? "";
        setShippingForm((prev) => ({ ...prev, [field]: value }));
    };

    const toggleHazmat = () => {
        setShippingForm((prev) => ({ ...prev, includeHazmat: !prev.includeHazmat }));
    };

    const handleSelectService = (serviceId) => {
        setShippingForm((prev) => ({ ...prev, serviceId }));
    };

    const buildShippingPayload = useCallback(() => {
        if (!order) {
            return { error: "Order information is unavailable." };
        }

        if (!selectedService) {
            return { error: "Select a service to continue." };
        }

        const shipDate = dayjs(shippingForm.shipOn);
        if (!shipDate.isValid()) {
            return { error: "Choose a valid ship date." };
        }

        const weightOz = convertWeightToOz(shippingForm.weightLb, shippingForm.weightOz);
        if (weightOz <= 0) {
            return { error: "Package weight must be greater than zero." };
        }

        const { widthIn: pageWidthIn, heightIn: pageHeightIn } = parsePaperSize(labelPreferences.paperSize);
        const labelFormat = (labelPreferences.deliveryFormat || "pdf").toLowerCase();
        const labelPaperSize = labelPreferences.paperSize || "4x6";

        const payload = {
            carrier: selectedService.carrier,
            serviceCode: selectedService.serviceCode ?? selectedService.id,
            serviceName: selectedService.name,
            packageType: shippingForm.packaging,
            weightOz,
            lengthIn: parseDecimal(shippingForm.lengthIn),
            widthIn: parseDecimal(shippingForm.widthIn),
            heightIn: parseDecimal(shippingForm.heightIn),
            shipDate: shipDate.startOf("day").toISOString(),
            paymentMethod: shippingForm.paymentMethod,
            labelFormat,
            labelPaperSize,
            pageWidthIn,
            pageHeightIn,
        };

        return { payload };
    }, [order, selectedService, shippingForm, labelPreferences]);

    const handleSubmit = useCallback(
        async (event) => {
            event.preventDefault();
            if (!order) {
                return;
            }

            const { payload, error } = buildShippingPayload();
            if (error) {
                setSubmissionState({ isSubmitting: false, error, result: null });
                return;
            }

            if (!payload) {
                setSubmissionState({ isSubmitting: false, error: "We could not prepare the shipping details.", result: null });
                return;
            }

            setSubmissionState({ isSubmitting: true, error: "", result: null });

            try {
                const response = await OrderService.printShippingLabel(order.id, payload);
                const result = response?.data ?? response;
                setSubmissionState({ isSubmitting: false, error: "", result });
            } catch (err) {
                console.error("Failed to print shipping label", err);
                const detailMessage =
                    err?.response?.data?.detail ??
                    err?.response?.data?.message ??
                    err?.message ??
                    "We could not complete the purchase. Check your details and try again.";
                setSubmissionState({
                    isSubmitting: false,
                    error: detailMessage,
                    result: null,
                });
            }
        },
        [order, buildShippingPayload]
    );

    const handleViewLabel = useCallback(() => {
        const labelUrl = submissionState.result?.labelUrl;
        if (!labelUrl) {
            return;
        }
        const absoluteUrl = buildAbsoluteUrl(labelUrl);
        window.open(absoluteUrl, "_blank", "noopener,noreferrer");
    }, [submissionState.result]);

    const handlePreferencesSave = useCallback((nextPreferences) => {
        setLabelPreferences(nextPreferences);
        setPreferencesOpen(false);
    }, []);

    const resetSubmissionError = () => {
        setSubmissionState((prev) => ({ ...prev, error: "" }));
    };

    const hasSuccessfulPurchase = Boolean(submissionState.result);

    const buyerDisplay = order?.buyerFullName || order?.buyerUsername || "Unknown buyer";

    if (loading) {
        return (
            <div className="ship-order__loading">
                <LoadingScreen isOverlay={true} />
            </div>
        );
    }

    if (error) {
        return (
            <div className="ship-order ship-order--error">
                <header className="ship-order__header">
                    <button type="button" className="ship-order__back" onClick={handleBack}>
                        ← Back to orders
                    </button>
                </header>
                <div className="ship-order__error">{error}</div>
            </div>
        );
    }

    if (!order) {
        return (
            <div className="ship-order ship-order--empty">
                <header className="ship-order__header">
                    <button type="button" className="ship-order__back" onClick={handleBack}>
                        ← Back to orders
                    </button>
                </header>
                <div className="ship-order__error">We could not find that order.</div>
            </div>
        );
    }

    return (
        <div className="ship-order">
            <header className="ship-order__header">
                <div className="ship-order__branding">
                    <span className="ship-order__logo">e</span>
                    <span className="ship-order__logo ship-order__logo--green">b</span>
                    <span className="ship-order__logo ship-order__logo--yellow">a</span>
                    <span className="ship-order__logo ship-order__logo--blue">y</span>
                </div>
                <div className="ship-order__header-actions">
                    <button type="button" className="ship-order__back" onClick={handleBack}>
                        ← Back to orders
                    </button>
                </div>
            </header>

            <main className="ship-order__main">
                <section className="ship-order__intro">
                    <h1 className="ship-order__title">Ship your order</h1>
                    <p className="ship-order__subtitle">
                        Confirm the package details below to get the best rate from your selected carrier.
                    </p>
                    <div className="ship-order__intro-actions">
                        <button
                            type="button"
                            className="ship-order__link"
                            onClick={() => window.open("https://www.ebay.com/sh/ord/?stage=bulk-shipping", "_blank", "noopener")}
                        >
                            Try bulk shipping
                        </button>
                        <span className="ship-order__intro-separator" aria-hidden="true">
                            &middot;
                        </span>
                        <button type="button" className="ship-order__link" onClick={() => setPreferencesOpen(true)}>
                            Label printing preferences
                        </button>
                    </div>
                </section>

                <div className="ship-order__content">
                    <form className="ship-order__form" onSubmit={handleSubmit}>
                        {hasSuccessfulPurchase && (
                            <div className="ship-order__callout ship-order__callout--success">
                                <div>
                                    <h3>Your label is ready</h3>
                                    <p>Download or print the label and attach it securely before handing it to the carrier.</p>
                                </div>
                                <div className="ship-order__callout-actions">
                                    <button
                                        type="button"
                                        className="ship-order__primary ship-order__primary--compact"
                                        onClick={handleViewLabel}
                                    >
                                        View label
                                    </button>
                                    <button
                                        type="button"
                                        className="ship-order__secondary ship-order__secondary--compact"
                                        onClick={handleBack}
                                    >
                                        Done
                                    </button>
                                </div>
                            </div>
                        )}

                        {submissionState.error && (
                            <div className="ship-order__callout ship-order__callout--error" role="alert">
                                <div>
                                    <h3>Something went wrong</h3>
                                    <p>{submissionState.error}</p>
                                </div>
                                <button type="button" className="ship-order__link" onClick={resetSubmissionError}>
                                    Dismiss
                                </button>
                            </div>
                        )}

                        <section className="ship-order__section">
                            <h2 className="ship-order__section-title">Package</h2>
                            <div className="ship-order__radio-group" role="radiogroup" aria-label="Package type">
                                <label className="ship-order__radio-option">
                                    <input
                                        type="radio"
                                        name="packaging"
                                        value="custom"
                                        checked={shippingForm.packaging === "custom"}
                                        onChange={handlePackagingChange}
                                    />
                                    <span>Custom size</span>
                                </label>
                                <label className="ship-order__radio-option">
                                    <input
                                        type="radio"
                                        name="packaging"
                                        value="carrier"
                                        checked={shippingForm.packaging === "carrier"}
                                        onChange={handlePackagingChange}
                                    />
                                    <span>Carrier packaging</span>
                                </label>
                            </div>

                            <div className="ship-order__field-group">
                                <span className="ship-order__field-label">Weight</span>
                                <div className="ship-order__weight">
                                    <label className="ship-order__weight-input">
                                        <input
                                            type="number"
                                            min="0"
                                            step="1"
                                            value={shippingForm.weightLb}
                                            onChange={handleFieldChange("weightLb")}
                                            aria-label="Weight pounds"
                                        />
                                        <span>lb</span>
                                    </label>
                                    <label className="ship-order__weight-input">
                                        <input
                                            type="number"
                                            min="0"
                                            max="15"
                                            step="1"
                                            value={shippingForm.weightOz}
                                            onChange={handleFieldChange("weightOz")}
                                            aria-label="Weight ounces"
                                        />
                                        <span>oz</span>
                                    </label>
                                </div>
                            </div>

                            <div className="ship-order__field-group">
                                <span className="ship-order__field-label">Dimensions</span>
                                <div className="ship-order__dimensions">
                                    <label className="ship-order__dimension-input">
                                        <input
                                            type="number"
                                            min="0"
                                            step="0.5"
                                            value={shippingForm.lengthIn}
                                            onChange={handleFieldChange("lengthIn")}
                                            aria-label="Length"
                                        />
                                        <span>in</span>
                                    </label>
                                    <span className="ship-order__dimension-separator">x</span>
                                    <label className="ship-order__dimension-input">
                                        <input
                                            type="number"
                                            min="0"
                                            step="0.5"
                                            value={shippingForm.widthIn}
                                            onChange={handleFieldChange("widthIn")}
                                            aria-label="Width"
                                        />
                                        <span>in</span>
                                    </label>
                                    <span className="ship-order__dimension-separator">x</span>
                                    <label className="ship-order__dimension-input">
                                        <input
                                            type="number"
                                            min="0"
                                            step="0.5"
                                            value={shippingForm.heightIn}
                                            onChange={handleFieldChange("heightIn")}
                                            aria-label="Height"
                                        />
                                        <span>in</span>
                                    </label>
                                </div>
                            </div>

                            <div className="ship-order__callout ship-order__callout--info">
                                <h3>Is your package size correct?</h3>
                                <p>Double-check weight and dimensions to avoid adjustment charges from your carrier.</p>
                            </div>

                            <div className="ship-order__callout">
                                <h3>This package does not contain hazardous materials</h3>
                                <p>
                                    Unsure what is considered hazardous?
                                    <button type="button" className="ship-order__link ship-order__link--inline">
                                        View list
                                    </button>
                                </p>
                                <label className="ship-order__checkbox">
                                    <input type="checkbox" checked={shippingForm.includeHazmat} onChange={toggleHazmat} />
                                    <span>My package contains hazardous materials</span>
                                </label>
                            </div>
                        </section>

                        <section className="ship-order__section">
                            <div className="ship-order__section-heading">
                                <h2 className="ship-order__section-title">Service</h2>
                                <label className="ship-order__field-inline" htmlFor="ship-order__ship-on">
                                    Ship on
                                    <input
                                        id="ship-order__ship-on"
                                        type="date"
                                        value={shippingForm.shipOn}
                                        onChange={handleFieldChange("shipOn")}
                                    />
                                </label>
                            </div>

                            <div className="ship-order__carrier-tabs">
                                {carrierTabs.map((tab) => {
                                    const isActive = carrierTab === tab;
                                    return (
                                        <button
                                            key={tab}
                                            type="button"
                                            className={`ship-order__carrier-tab${isActive ? " ship-order__carrier-tab--active" : ""}`}
                                            onClick={() => setCarrierTab(tab)}
                                        >
                                            {tab}
                                        </button>
                                    );
                                })}
                            </div>

                            <div className="ship-order__services">
                                {shippingServicesLoading ? (
                                    <div className="ship-order__services-loading">Loading shipping services...</div>
                                ) : shippingServicesError ? (
                                    <div className="ship-order__services-error" role="alert">
                                        {shippingServicesError}
                                    </div>
                                ) : servicesForCarrier.length === 0 ? (
                                    <div className="ship-order__no-services">
                                        No services are available for {carrierTab} right now.
                                    </div>
                                ) : (
                                    servicesForCarrier.map((service) => {
                                        const isSelected = shippingForm.serviceId === service.id;
                                        return (
                                            <button
                                                key={service.id}
                                                type="button"
                                                className={`ship-order__service${isSelected ? " ship-order__service--selected" : ""}`}
                                                onClick={() => handleSelectService(service.id)}
                                            >
                                                <div className="ship-order__service-header">
                                                    <div>
                                                        <h3>{service.name}</h3>
                                                        <p>{service.savings}</p>
                                                    </div>
                                                    <span className="ship-order__service-price">{formatCurrency(service.price)}</span>
                                                </div>
                                                <div className="ship-order__service-body">
                                                    <div className="ship-order__service-grid">
                                                        <span className="ship-order__service-grid-item">
                                                            <strong>QR code</strong>
                                                            <span>{service.qrCode ? "Available" : "Not available"}</span>
                                                        </span>
                                                        <span className="ship-order__service-grid-item">
                                                            <strong>Included coverage</strong>
                                                            <span>{service.coverage}</span>
                                                        </span>
                                                        <span className="ship-order__service-grid-item">
                                                            <strong>Estimated delivery</strong>
                                                            <span>{service.window}</span>
                                                        </span>
                                                        <span className="ship-order__service-grid-item">
                                                            <strong>Printer</strong>
                                                            <span>{service.printerRequired ? "Required" : "Not required"}</span>
                                                        </span>
                                                    </div>
                                                    <p className="ship-order__service-footnote">{service.notes}</p>
                                                </div>
                                            </button>
                                        );
                                    })
                                )}
                            </div>

                            <button type="button" className="ship-order__link ship-order__link--inline">
                                Compare all services
                            </button>
                        </section>

                        <footer className="ship-order__footer">
                            <button type="button" className="ship-order__secondary" onClick={handleBack}>
                                Cancel
                            </button>
                            <button
                                type="submit"
                                className="ship-order__primary"
                                disabled={
                                    submissionState.isSubmitting ||
                                    shippingServicesLoading ||
                                    Boolean(shippingServicesError)
                                }
                            >
                                {submissionState.isSubmitting ? "Processing..." : "Confirm and pay"}
                                {selectedService && !submissionState.isSubmitting
                                    ? ` ${formatCurrency(selectedService.price)}`
                                    : ""}
                            </button>
                        </footer>
                    </form>

                    <aside className="ship-order__sidebar">
                        <div className="ship-order__summary-card">
                            <h2>Order details</h2>
                            <dl>
                                <div className="ship-order__summary-row">
                                    <dt>Order number</dt>
                                    <dd>{order.orderNumber || "-"}</dd>
                                </div>
                                <div className="ship-order__summary-row">
                                    <dt>Buyer</dt>
                                    <dd>{buyerDisplay}</dd>
                                </div>
                                <div className="ship-order__summary-row">
                                    <dt>Total</dt>
                                    <dd>{formatCurrency(order.total)}</dd>
                                </div>
                                <div className="ship-order__summary-row">
                                    <dt>Items</dt>
                                    <dd>{Array.isArray(order.items) ? order.items.length : 0}</dd>
                                </div>
                                <div className="ship-order__summary-row">
                                    <dt>Sold on</dt>
                                    <dd>{formatDate(order.orderedAt)}</dd>
                                </div>
                                <div className="ship-order__summary-row">
                                    <dt>Paid on</dt>
                                    <dd>{formatDate(order.paidAt)}</dd>
                                </div>
                            </dl>
                        </div>

                        <div className="ship-order__summary-card">
                            <h2>Ship to</h2>
                            <p className="ship-order__summary-text">
                                {order.shipToName || buyerDisplay}
                                <br />
                                {order.shipToAddress || "Shipping address available after confirmation."}
                            </p>
                        </div>

                        {primaryItem && (
                            <div className="ship-order__summary-card">
                                <h2>Items in this order</h2>
                                <p className="ship-order__summary-text">{primaryItem.title ?? primaryItem.Title ?? "Untitled item"}</p>
                                {remainingItems > 0 && (
                                    <p className="ship-order__summary-meta">+{remainingItems} more item{remainingItems > 1 ? "s" : ""}</p>
                                )}
                            </div>
                        )}

                        {selectedService && (
                            <div className="ship-order__summary-card">
                                <h2>Selected service</h2>
                                <p className="ship-order__summary-text">{selectedService.name}</p>
                                <p className="ship-order__summary-meta">{selectedService.savings}</p>
                                <p className="ship-order__summary-price">{formatCurrency(selectedService.price)}</p>
                                <button
                                    type="button"
                                    className="ship-order__link ship-order__link--block"
                                    onClick={() => setCarrierTab(selectedService.carrier)}
                                >
                                    View carrier details
                                </button>
                            </div>
                        )}

                        <div className="ship-order__summary-card">
                            <h2>Label preferences</h2>
                            <p className="ship-order__summary-text">
                                Delivery: {LABEL_DELIVERY_FORMATS.find((option) => option.value === labelPreferences.deliveryFormat)?.label ?? ""}
                                <br />
                                Paper size: {LABEL_PAPER_SIZES.find((option) => option.value === labelPreferences.paperSize)?.label ?? ""}
                            </p>
                            <button
                                type="button"
                                className="ship-order__link ship-order__link--block"
                                onClick={() => setPreferencesOpen(true)}
                            >
                                Edit preferences
                            </button>
                        </div>

                        <div className="ship-order__summary-card">
                            <h2>Payment method</h2>
                            <div className="ship-order__radio-stack">
                                {PAYMENT_METHODS.map((method) => (
                                    <label key={method.value} className="ship-order__radio-payment">
                                        <input
                                            type="radio"
                                            name="payment-method"
                                            value={method.value}
                                            checked={shippingForm.paymentMethod === method.value}
                                            onChange={handleFieldChange("paymentMethod")}
                                        />
                                        <span>{method.label}</span>
                                    </label>
                                ))}
                            </div>
                        </div>
                    </aside>
                </div>
            </main>

            <LabelPreferencesDialog
                isOpen={preferencesOpen}
                preferences={labelPreferences}
                onClose={() => setPreferencesOpen(false)}
                onSave={handlePreferencesSave}
            />

            {submissionState.isSubmitting && (
                <div className="ship-order__overlay" aria-hidden="true">
                    <LoadingScreen isOverlay={true} />
                </div>
            )}
        </div>
    );
};

export default ShipOrderPage;

const LabelPreferencesDialog = ({ isOpen, preferences, onClose, onSave }) => {
    const [localPrefs, setLocalPrefs] = useState(preferences);

    useEffect(() => {
        setLocalPrefs(preferences);
    }, [preferences]);

    if (!isOpen) {
        return null;
    }

    const handleChange = (field) => (event) => {
        const value = event.target.value;
        setLocalPrefs((prev) => ({ ...prev, [field]: value }));
    };

    const handleSave = () => {
        onSave(localPrefs);
    };

    return (
        <div className="ship-order__modal-backdrop">
            <div className="ship-order__modal" role="dialog" aria-modal="true" aria-labelledby="label-preferences-heading">
                <header className="ship-order__modal-header">
                    <h2 id="label-preferences-heading">Label printing preferences</h2>
                    <button type="button" className="ship-order__modal-close" onClick={onClose} aria-label="Close preferences">
                        &times;
                    </button>
                </header>

                <div className="ship-order__modal-section">
                    <h3>Label delivery</h3>
                    <div className="ship-order__radio-stack">
                        {LABEL_DELIVERY_FORMATS.map((option) => (
                            <label key={option.value} className="ship-order__radio-payment">
                                <input
                                    type="radio"
                                    name="label-delivery"
                                    value={option.value}
                                    checked={localPrefs.deliveryFormat === option.value}
                                    onChange={handleChange("deliveryFormat")}
                                />
                                <div>
                                    <span className="ship-order__radio-title">{option.label}</span>
                                    <p className="ship-order__radio-description">{option.description}</p>
                                </div>
                            </label>
                        ))}
                    </div>
                </div>

                <div className="ship-order__modal-section">
                    <h3>Paper size</h3>
                    <select value={localPrefs.paperSize} onChange={handleChange("paperSize")}>
                        {LABEL_PAPER_SIZES.map((option) => (
                            <option key={option.value} value={option.value}>
                                {option.label}
                            </option>
                        ))}
                    </select>
                </div>

                <footer className="ship-order__modal-footer">
                    <button type="button" className="ship-order__secondary" onClick={onClose}>
                        Cancel
                    </button>
                    <button type="button" className="ship-order__primary" onClick={handleSave}>
                        Save preferences
                    </button>
                </footer>
            </div>
        </div>
    );
};
