import React, { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import CouponService from "../../services/CouponService";
import CategoryService from "../../services/CategoryService";
import Notice from "../../components/Common/CustomNotification";
import "./CreateSellerCoupon.scss";

dayjs.extend(utc);

const CODE_REGEX = /^[A-Z0-9-]+$/;
const DATETIME_FORMAT = "YYYY-MM-DDTHH:mm";

const createBlankCondition = () => ({
  buyQuantity: "",
  getQuantity: "",
  getDiscountPercent: "",
  saveEveryAmount: "",
  saveEveryItems: "",
  conditionDescription: ""
});

const createInitialForm = (type) => {
  const startingType = type ?? CouponService.listTypes()[0] ?? null;
  const start = dayjs().add(10, "minute");
  const end = start.add(14, "day");

  return {
    couponTypeId: startingType?.id ?? "",
    categoryId: "",
    name: "",
    code: "",
    discountValue: "",
    discountUnit: startingType?.defaultUnit ?? "Percent",
    maxDiscount: "",
    startDate: start.format(DATETIME_FORMAT),
    endDate: end.format(DATETIME_FORMAT),
    usageLimit: "",
    usagePerUser: "",
    minimumOrderValue: "",
    applicablePriceMin: "",
    applicablePriceMax: "",
    isActive: true,
    conditions:
      startingType?.conditionMode === "single"
        ? [createBlankCondition()]
        : []
  };
};

const resolveDiscountUnitLabel = (form, type) => {
  if (type?.unitLocked) {
    return type?.defaultUnit ?? "Percent";
  }

  if (typeof form.discountUnit === "string" && form.discountUnit.trim()) {
    return form.discountUnit;
  }

  if (typeof form.discountUnit === "number") {
    return form.discountUnit === 2 ? "Amount" : "Percent";
  }

  return "Percent";
};

const mapDiscountUnitToEnum = (unit) => {
  if (typeof unit === "number") {
    return unit === 2 ? 2 : 1;
  }

  if (typeof unit === "string") {
    const normalized = unit.trim().toLowerCase();
    return normalized === "amount" ? 2 : 1;
  }

  return 1;
};

const parseNullableNumber = (value) => {
  if (value === null || value === undefined) return null;
  if (value === "") return null;
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : null;
};

const parseNullableInt = (value) => {
  if (value === null || value === undefined) return null;
  if (value === "") return null;
  const parsed = Number.parseInt(value, 10);
  return Number.isNaN(parsed) ? null : parsed;
};

const normalizeConditions = (form, type) => {
  if (type?.conditionMode !== "single") {
    return [];
  }

  const source = form.conditions?.[0] ?? createBlankCondition();
  return [
    {
      buyQuantity: parseNullableInt(source.buyQuantity),
      getQuantity: parseNullableInt(source.getQuantity),
      getDiscountPercent: parseNullableNumber(source.getDiscountPercent),
      saveEveryAmount: parseNullableNumber(source.saveEveryAmount),
      saveEveryItems: parseNullableInt(source.saveEveryItems),
      conditionDescription: source.conditionDescription?.trim()
        ? source.conditionDescription.trim()
        : null
    }
  ];
};

const buildPayload = (form, type) => ({
  couponTypeId: form.couponTypeId,
  categoryId: form.categoryId?.trim() ? form.categoryId.trim() : null,
  name: form.name.trim(),
  code: form.code.trim().toUpperCase(),
  discountValue: Number(form.discountValue),
  discountUnit: mapDiscountUnitToEnum(resolveDiscountUnitLabel(form, type)),
  maxDiscount: parseNullableNumber(form.maxDiscount),
  startDate: dayjs(form.startDate).utc().toISOString(),
  endDate: dayjs(form.endDate).utc().toISOString(),
  usageLimit: parseNullableInt(form.usageLimit),
  usagePerUser: parseNullableInt(form.usagePerUser),
  minimumOrderValue: parseNullableNumber(form.minimumOrderValue),
  applicablePriceMin: parseNullableNumber(form.applicablePriceMin),
  applicablePriceMax: parseNullableNumber(form.applicablePriceMax),
  isActive: Boolean(form.isActive),
  conditions: normalizeConditions(form, type)
});

const validateForm = (form, type) => {
  const errors = {};
  const activeDiscountUnit = resolveDiscountUnitLabel(form, type);
  if (!form.name.trim()) {
    errors.name = "Enter a promotion name.";
  }

  if (!form.code.trim()) {
    errors.code = "Enter a public coupon code.";
  } else if (!CODE_REGEX.test(form.code.trim())) {
    errors.code = "Use letters, numbers, or dashes only.";
  }

  if (!form.discountValue) {
    errors.discountValue = "Enter the discount value.";
  } else if (!(Number(form.discountValue) > 0)) {
    errors.discountValue = "Discount must be greater than zero.";
  }

  if (activeDiscountUnit === "Percent" && Number(form.discountValue) > 95) {
    errors.discountValue = "Percent discount should be 95 or lower.";
  }

  if (form.maxDiscount && !(Number(form.maxDiscount) > 0)) {
    errors.maxDiscount = "Max discount must be greater than zero.";
  }

  if (!form.startDate) {
    errors.startDate = "Select a start date.";
  }
  if (!form.endDate) {
    errors.endDate = "Select an end date.";
  }
  if (form.startDate && form.endDate) {
    const start = dayjs(form.startDate);
    const end = dayjs(form.endDate);
    if (!start.isValid() || !end.isValid()) {
      errors.endDate = "Enter a valid date range.";
    } else if (!end.isAfter(start)) {
      errors.endDate = "End date must be after start date.";
    }
  }

  if (form.usageLimit && !(Number(form.usageLimit) > 0)) {
    errors.usageLimit = "Usage limit must be positive.";
  }
  if (form.usagePerUser && !(Number(form.usagePerUser) > 0)) {
    errors.usagePerUser = "Per-buyer limit must be positive.";
  }
  if (
    form.usageLimit &&
    form.usagePerUser &&
    Number(form.usagePerUser) > Number(form.usageLimit)
  ) {
    errors.usagePerUser = "Per-buyer limit cannot exceed total limit.";
  }

  if (type?.requiresMinOrderValue) {
    if (!form.minimumOrderValue) {
      errors.minimumOrderValue = "Enter the minimum order total.";
    } else if (!(Number(form.minimumOrderValue) > 0)) {
      errors.minimumOrderValue = "Minimum order must be positive.";
    }
  } else if (form.minimumOrderValue && !(Number(form.minimumOrderValue) > 0)) {
    errors.minimumOrderValue = "Minimum order must be positive.";
  }

  if (form.applicablePriceMin && !(Number(form.applicablePriceMin) >= 0)) {
    errors.applicablePriceMin = "Enter a valid price.";
  }
  if (form.applicablePriceMax && !(Number(form.applicablePriceMax) >= 0)) {
    errors.applicablePriceMax = "Enter a valid price.";
  }
  if (
    form.applicablePriceMin &&
    form.applicablePriceMax &&
    Number(form.applicablePriceMin) > Number(form.applicablePriceMax)
  ) {
    errors.applicablePriceMax = "Max price must be higher than min price.";
  }

  if (type?.conditionMode === "single") {
    const condition = form.conditions?.[0] ?? createBlankCondition();
    type.conditionFields?.forEach((field) => {
      const rawValue = condition[field.key];
      const errorKey = `conditions.${field.key}`;
      if (field.type === "number") {
        if (rawValue === "" || rawValue === null || rawValue === undefined) {
          errors[errorKey] = "Required.";
          return;
        }
        const numeric = Number(rawValue);
        if (!Number.isFinite(numeric)) {
          errors[errorKey] = "Enter a number.";
          return;
        }
        if (field.min !== undefined && numeric < field.min) {
          errors[errorKey] = `Must be at least ${field.min}.`;
          return;
        }
        if (field.max !== undefined && numeric > field.max) {
          errors[errorKey] = `Must be ${field.max} or less.`;
        }
      }
    });
  }

  return errors;
};

const buildSummaryLine = (form, type) => {
  if (!type) {
    return "Select a coupon type to preview the buyer message.";
  }

  const discountUnitLabel = resolveDiscountUnitLabel(form, type);
  const discountLabel =
    discountUnitLabel === "Percent"
      ? `${form.discountValue || ".."}% off`
      : `$${form.discountValue || ".."} off`;

  const base = `${discountLabel} on eligible items`;

  if (type.conditionMode === "single") {
    const condition = form.conditions?.[0] ?? createBlankCondition();
    if (type.id === "0D0C32FE-349C-4857-B20A-2D3F8DB91ED4" && condition.saveEveryItems) {
      return `${discountLabel} when buyers purchase ${condition.saveEveryItems}+ items`;
    }
    if (type.id === "7EAA19CF-6B36-4A1C-B7B5-A9ABCB7EEFF2" && condition.buyQuantity && condition.getQuantity) {
      return `Buy ${condition.buyQuantity}, get ${condition.getQuantity} at ${form.discountValue || ".."}% off`;
    }
    if (type.id === "ED9D5151-6F8C-4628-A5A9-4C24867E5673" && condition.buyQuantity && condition.getQuantity) {
      return `Buy ${condition.buyQuantity}, get ${condition.getQuantity} free`;
    }
    if (type.id === "773F8D9B-EB8E-4FF4-A21E-4BB2FA5407F4" && condition.saveEveryItems) {
      return `$${form.discountValue || ".."} off when buyers add ${condition.saveEveryItems}+ items`;
    }
    if (type.id === "3B980145-62B6-4AE6-9CF8-7838BC7B84E0" && condition.saveEveryItems) {
      return `Save $${condition.saveEveryAmount || ".."} every ${condition.saveEveryItems} items`;
    }
    if (type.id === "51F2ED38-06BB-496E-B5CB-7AA3057C21B7" && condition.saveEveryAmount) {
      return `Save $${form.discountValue || ".."} every $${condition.saveEveryAmount}`;
    }
  }

  if (type.requiresMinOrderValue && form.minimumOrderValue) {
    return `${discountLabel} when buyers spend $${form.minimumOrderValue}`;
  }

  return base;
};

const CreateSellerCouponPage = () => {
  const types = useMemo(() => CouponService.listTypes(), []);
  const initialType = types[0] ?? null;
  const [selectedTypeId, setSelectedTypeId] = useState(initialType?.id ?? "");
  const [form, setForm] = useState(() => createInitialForm(initialType));
  const [errors, setErrors] = useState({});
  const [submitting, setSubmitting] = useState(false);
  const [categories, setCategories] = useState([]);
  const [loadingCategories, setLoadingCategories] = useState(false);
  const navigate = useNavigate();

  const activeType = useMemo(
    () => types.find((t) => t.id === selectedTypeId) ?? null,
    [types, selectedTypeId]
  );

  const summaryLine = useMemo(
    () => buildSummaryLine(form, activeType),
    [form, activeType]
  );

  useEffect(() => {
    let isMounted = true;

    const loadCategories = async () => {
      setLoadingCategories(true);
      try {
        const data = await CategoryService.getCategories();
        if (!isMounted) {
          return;
        }
        setCategories(Array.isArray(data) ? data : []);
      } catch {
        if (isMounted) {
          setCategories([]);
          Notice({
            msg: "Could not load categories.",
            desc: "Try refreshing the page or check your connection.",
            isSuccess: false
          });
        }
      } finally {
        if (isMounted) {
          setLoadingCategories(false);
        }
      }
    };

    loadCategories();

    return () => {
      isMounted = false;
    };
  }, []);

  const clearError = (key) => {
    setErrors((prev) => {
      if (!prev[key]) return prev;
      const next = { ...prev };
      delete next[key];
      return next;
    });
  };

  const updateField = (key, value) => {
    setForm((prev) => ({
      ...prev,
      [key]: value
    }));
    clearError(key);
  };

  const updateConditionField = (index, key, value) => {
    setForm((prev) => {
      const draft = prev.conditions?.slice() ?? [];
      const target = draft[index] ? { ...draft[index] } : createBlankCondition();
      target[key] = value;
      draft[index] = target;
      return { ...prev, conditions: draft };
    });
    clearError(`conditions.${key}`);
  };

  const handleTypeSelect = (typeId) => {
    const nextType = types.find((item) => item.id === typeId) ?? null;
    setSelectedTypeId(typeId);
    setForm((prev) => {
      const template = createInitialForm(nextType);
      return {
        ...template,
        name: prev.name,
        code: prev.code,
        categoryId: prev.categoryId,
        startDate: prev.startDate,
        endDate: prev.endDate,
        usageLimit: prev.usageLimit,
        usagePerUser: prev.usagePerUser,
        applicablePriceMin: prev.applicablePriceMin,
        applicablePriceMax: prev.applicablePriceMax,
        isActive: prev.isActive,
        minimumOrderValue: nextType?.requiresMinOrderValue
          ? prev.minimumOrderValue
          : ""
      };
    });
    setErrors({});
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (!activeType) {
      Notice({ msg: "Select a coupon type.", isSuccess: false });
      return;
    }

    const validation = validateForm(form, activeType);
    if (Object.keys(validation).length > 0) {
      setErrors(validation);
      Notice({ msg: "Review the highlighted fields.", isSuccess: false });
      return;
    }

    try {
      setSubmitting(true);
      const payload = buildPayload(form, activeType);
      const couponId = await CouponService.createSellerCoupon(payload);
      Notice({
        msg: "Coupon published",
        desc: `Coupon ${payload.code} is ready for buyers.`,
        isSuccess: true
      });
      setForm(createInitialForm(activeType));
      if (couponId) {
        navigate(`/marketing/coupons/create?created=${couponId}`, { replace: true });
      }
    } catch {
      Notice({ msg: "Could not create coupon.", isSuccess: false });
    } finally {
      setSubmitting(false);
    }
  };

  const renderError = (key) =>
    errors[key] ? <span className="coupon-creator__error">{errors[key]}</span> : null;

  return (
    <div className="coupon-creator">
      <header className="coupon-creator__header">
        <div>
          <h1 className="coupon-creator__title">Create a coupon</h1>
          <p className="coupon-creator__subtitle">
            Launch eBay-style promotions that feel familiar to your buyers.
          </p>
        </div>
        <span className="coupon-creator__badge">Seller tools</span>
      </header>

      <section className="coupon-creator__type-picker">
        <h2 className="coupon-creator__section-title">Choose your promotion style</h2>
        <div className="coupon-creator__types-grid">
          {types.map((type) => (
            <button
              type="button"
              key={type.id}
              className={`coupon-creator__type-card ${
                type.id === selectedTypeId ? "coupon-creator__type-card--active" : ""
              }`}
              style={{ borderColor: type.colorToken }}
              onClick={() => handleTypeSelect(type.id)}
            >
              <div className="coupon-creator__type-card-header">
                <span className="coupon-creator__type-name">{type.label}</span>
                <span className="coupon-creator__type-pill" style={{ backgroundColor: type.colorToken }}>
                  {type.defaultUnit}
                </span>
              </div>
              <p className="coupon-creator__type-summary">{type.summary}</p>
              <p className="coupon-creator__type-tagline">{type.heroTagline}</p>
            </button>
          ))}
        </div>
      </section>

      <div className="coupon-creator__layout">
        <form className="coupon-creator__form" onSubmit={handleSubmit}>
          <section className="coupon-creator__panel">
            <h3>Basics</h3>
            <div className="coupon-creator__fields">
              <label className="coupon-creator__field">
                <span>Internal name</span>
                <input
                  type="text"
                  value={form.name}
                  onChange={(event) => updateField("name", event.target.value)}
                  placeholder="Holiday spotlight"
                />
                {renderError("name")}
              </label>
              <label className="coupon-creator__field">
                <span>Public coupon code</span>
                <input
                  type="text"
                  value={form.code}
                  onChange={(event) =>
                    updateField(
                      "code",
                      event.target.value.toUpperCase().replace(/[^A-Z0-9-]/g, "")
                    )
                  }
                  placeholder="FLASH25"
                />
                {renderError("code")}
              </label>
            </div>
            <label className="coupon-creator__field">
              <span>Limit to category (optional)</span>
              <select
                value={form.categoryId}
                onChange={(event) => updateField("categoryId", event.target.value)}
                disabled={loadingCategories}
              >
                <option value="">All eligible categories</option>
                {loadingCategories && (
                  <option value="" disabled>
                    Loading categories...
                  </option>
                )}
                {!loadingCategories &&
                  categories.map((category) => (
                    <option key={category.id} value={category.id}>
                      {category.name}
                    </option>
                  ))}
              </select>
            </label>
          </section>

          <section className="coupon-creator__panel">
            <h3>Discount details</h3>
            <div className="coupon-creator__fields">
              <label className="coupon-creator__field">
                <span>Discount value</span>
                <input
                  type="number"
                  min="0"
                  step={activeType?.defaultUnit === "Percent" ? "1" : "0.5"}
                  value={form.discountValue}
                  onChange={(event) => updateField("discountValue", event.target.value)}
                  placeholder={activeType?.defaultUnit === "Percent" ? "10" : "5"}
                />
                {renderError("discountValue")}
              </label>
              <label className="coupon-creator__field">
                <span>Discount unit</span>
                <select
                  value={form.discountUnit}
                  onChange={(event) => updateField("discountUnit", event.target.value)}
                  disabled={activeType?.unitLocked}
                >
                  <option value="Percent">Percent</option>
                  <option value="Amount">Amount</option>
                </select>
              </label>
            </div>
            {form.discountUnit === "Percent" && (
              <label className="coupon-creator__field">
                <span>Maximum discount (optional)</span>
                <input
                  type="number"
                  min="0"
                  step="0.5"
                  value={form.maxDiscount}
                  onChange={(event) => updateField("maxDiscount", event.target.value)}
                  placeholder="50"
                />
                {renderError("maxDiscount")}
              </label>
            )}
            {activeType?.requiresMinOrderValue && (
              <label className="coupon-creator__field">
                <span>Minimum order total</span>
                <input
                  type="number"
                  min="0"
                  step="1"
                  value={form.minimumOrderValue}
                  onChange={(event) => updateField("minimumOrderValue", event.target.value)}
                  placeholder="100"
                />
                {renderError("minimumOrderValue")}
              </label>
            )}
          </section>

          {activeType?.conditionMode === "single" && (
            <section className="coupon-creator__panel">
              <h3>Buyer conditions</h3>
              <p className="coupon-creator__helper">
                Match eBay's conditional fields so shoppers instantly understand the offer.
              </p>
              <div className="coupon-creator__fields coupon-creator__fields--grid">
                {activeType.conditionFields?.map((field) => (
                  <label className="coupon-creator__field" key={field.key}>
                    <span>{field.label}</span>
                    <input
                      type={field.type === "number" ? "number" : "text"}
                      min={field.min ?? undefined}
                      max={field.max ?? undefined}
                      step={field.step ?? (field.type === "number" ? "1" : undefined)}
                      value={form.conditions?.[0]?.[field.key] ?? ""}
                      onChange={(event) =>
                        updateConditionField(0, field.key, event.target.value)
                      }
                      placeholder={field.placeholder ?? ""}
                    />
                    {renderError(`conditions.${field.key}`)}
                    {field.helper && (
                      <span className="coupon-creator__helper-text">{field.helper}</span>
                    )}
                  </label>
                ))}
              </div>
            </section>
          )}

          <section className="coupon-creator__panel">
            <h3>Availability</h3>
            <div className="coupon-creator__fields">
              <label className="coupon-creator__field">
                <span>Start date</span>
                <input
                  type="datetime-local"
                  value={form.startDate}
                  onChange={(event) => updateField("startDate", event.target.value)}
                />
                {renderError("startDate")}
              </label>
              <label className="coupon-creator__field">
                <span>End date</span>
                <input
                  type="datetime-local"
                  value={form.endDate}
                  onChange={(event) => updateField("endDate", event.target.value)}
                />
                {renderError("endDate")}
              </label>
            </div>
            <label className="coupon-creator__toggle">
              <input
                type="checkbox"
                checked={form.isActive}
                onChange={(event) => updateField("isActive", event.target.checked)}
              />
              <span>Activate immediately after saving</span>
            </label>
          </section>

          <section className="coupon-creator__panel">
            <h3>Advanced targeting</h3>
            <div className="coupon-creator__fields">
              <label className="coupon-creator__field">
                <span>Total redemption limit (optional)</span>
                <input
                  type="number"
                  min="0"
                  step="1"
                  value={form.usageLimit}
                  onChange={(event) => updateField("usageLimit", event.target.value)}
                  placeholder="500"
                />
                {renderError("usageLimit")}
              </label>
              <label className="coupon-creator__field">
                <span>Per-buyer limit (optional)</span>
                <input
                  type="number"
                  min="0"
                  step="1"
                  value={form.usagePerUser}
                  onChange={(event) => updateField("usagePerUser", event.target.value)}
                  placeholder="1"
                />
                {renderError("usagePerUser")}
              </label>
            </div>
            <div className="coupon-creator__fields">
              <label className="coupon-creator__field">
                <span>Item price minimum (optional)</span>
                <input
                  type="number"
                  min="0"
                  step="1"
                  value={form.applicablePriceMin}
                  onChange={(event) => updateField("applicablePriceMin", event.target.value)}
                  placeholder="10"
                />
                {renderError("applicablePriceMin")}
              </label>
              <label className="coupon-creator__field">
                <span>Item price maximum (optional)</span>
                <input
                  type="number"
                  min="0"
                  step="1"
                  value={form.applicablePriceMax}
                  onChange={(event) => updateField("applicablePriceMax", event.target.value)}
                  placeholder="250"
                />
                {renderError("applicablePriceMax")}
              </label>
            </div>
          </section>

          <div className="coupon-creator__actions">
            <button type="submit" className="coupon-creator__primary" disabled={submitting}>
              {submitting ? "Publishing..." : "Publish coupon"}
            </button>
            <button
              type="button"
              className="coupon-creator__secondary"
              onClick={() => setForm(createInitialForm(activeType))}
              disabled={submitting}
            >
              Reset form
            </button>
          </div>
        </form>

        <aside className="coupon-creator__summary">
          <div className="coupon-creator__summary-card">
            <span className="coupon-creator__summary-badge">
              {activeType?.label ?? "Choose a type"}
            </span>
            <h3 className="coupon-creator__summary-headline">{summaryLine}</h3>
            <p className="coupon-creator__summary-desc">
              Live from
              {" "}
              {form.startDate
                ? dayjs(form.startDate).format("MMM D, YYYY h:mm A")
                : "-"}
              {" "}
              to
              {" "}
              {form.endDate ? dayjs(form.endDate).format("MMM D, YYYY h:mm A") : "-"}.
            </p>
            {form.minimumOrderValue && (
              <p className="coupon-creator__summary-note">
                Buyers must spend at least ${form.minimumOrderValue}.
              </p>
            )}
            {form.conditions?.[0]?.conditionDescription && (
              <p className="coupon-creator__summary-note">
                {form.conditions[0].conditionDescription}
              </p>
            )}
            <ul className="coupon-creator__summary-list">
              <li>Code: {form.code || "-"}</li>
              <li>Status: {form.isActive ? "Active" : "Draft"}</li>
              <li>
                Redemption limits:
                {" "}
                {form.usageLimit ? `${form.usageLimit} total` : "Unlimited"}
                {form.usagePerUser ? ` • ${form.usagePerUser} per buyer` : ""}
              </li>
            </ul>
          </div>
        </aside>
      </div>
    </div>
  );
};

export default CreateSellerCouponPage;
