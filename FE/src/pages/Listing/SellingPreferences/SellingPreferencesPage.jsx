import React, { useCallback, useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import Notice from "../../../components/Common/CustomNotification";
import SellingPreferenceService from "../../../services/SellingPreferenceService";
import "./SellingPreferencesPage.scss";

const INVOICE_FORMAT_OPTIONS = [
  { value: 1, label: "Detailed (downloadable csv file)" },
  { value: 0, label: "Summary" }
];

const FALLBACK_PREFERENCES = SellingPreferenceService.normalizeSellerPreference({});

const SellingPreferencesPage = () => {
  const [preferences, setPreferences] = useState(FALLBACK_PREFERENCES);
  const [isLoading, setIsLoading] = useState(true);
  const [pending, setPending] = useState({ multiQuantity: false, invoice: false });

  const fetchPreferences = useCallback(async () => {
    setIsLoading(true);
    try {
      const result = await SellingPreferenceService.getOverview();
      setPreferences(result);
    } catch (error) {
      console.error("Failed to load selling preferences", error);
      Notice({ msg: "Unable to load selling preferences. Please try again later.", isSuccess: false });
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchPreferences();
  }, [fetchPreferences]);

  const updatePreferences = useCallback(async (payload, scope) => {
    setPending((prev) => ({ ...prev, [scope]: true }));
    try {
      const updated = await SellingPreferenceService.updateOverview(payload);
      setPreferences(updated);
      Notice({ msg: "Preferences updated", isSuccess: true });
    } catch (error) {
      console.error("Failed to update preferences", error);
      Notice({ msg: "Update failed. Please try again.", isSuccess: false });
      await fetchPreferences();
    } finally {
      setPending((prev) => ({ ...prev, [scope]: false }));
    }
  }, [fetchPreferences]);

  const handleToggleListingsStayActive = () => {
    const nextValue = !preferences.multiQuantity.listingsStayActiveWhenOutOfStock;
    updatePreferences({ listingsStayActiveWhenOutOfStock: nextValue }, "multiQuantity");
  };

  const handleToggleShowQuantity = () => {
    const nextValue = !preferences.multiQuantity.showExactQuantityAvailable;
    updatePreferences({ showExactQuantityAvailable: nextValue }, "multiQuantity");
  };

  const handleInvoiceFormatChange = async (event) => {
    const selectedValue = Number.parseInt(event.target.value, 10);
    if (Number.isNaN(selectedValue)) {
      return;
    }

    await updatePreferences({ invoice: { format: selectedValue } }, "invoice");
  };

  const handleToggleInvoiceEmail = () => {
    const nextValue = !preferences.invoice.sendEmailCopy;
    updatePreferences({ invoice: { sendEmailCopy: nextValue } }, "invoice");
  };

  const handleToggleApplyCredits = () => {
    const nextValue = !preferences.invoice.applyCreditsAutomatically;
    updatePreferences({ invoice: { applyCreditsAutomatically: nextValue } }, "invoice");
  };

  const buyerManagementSummary = useMemo(() => {
    const { blockSettings, rules } = preferences.buyerManagement;
    const summaryParts = [];

    if (blockSettings.blockUnpaidItemStrikes) {
      summaryParts.push(
        `Blocking buyers with ${blockSettings.unpaidItemStrikesCount} unpaid order cancellations in ${blockSettings.unpaidItemStrikesPeriodInMonths} month(s)`
      );
    }

    if (blockSettings.blockPrimaryAddressOutsideShippingLocation) {
      summaryParts.push("Blocking buyers outside your shipping locations");
    }

    if (blockSettings.blockMaxItemsInLastTenDays && blockSettings.maxItemsInLastTenDays) {
      summaryParts.push(`Limiting buyers to ${blockSettings.maxItemsInLastTenDays} item(s) in 10 days`);
    }

    if (blockSettings.applyFeedbackScoreThreshold && typeof blockSettings.feedbackScoreThreshold === "number") {
      summaryParts.push(`Restricting buyers with feedback score above ${blockSettings.feedbackScoreThreshold}`);
    }

    if (rules.preventBlockedBuyersFromContacting) {
      summaryParts.push("Blocked buyers cannot contact you");
    }

    if (summaryParts.length === 0) {
      return "Using default buyer management rules.";
    }

    return summaryParts.join(" • ");
  }, [preferences.buyerManagement]);

  const multiQuantitySection = (
    <section className="selling-preferences__section">
      <header className="selling-preferences__section-header">
        <h2>Multi-quantity listings</h2>
        <p>Manage how multi-quantity listings behave when inventory runs low.</p>
      </header>
      <ul className="selling-preferences__list">
        <li className="selling-preferences__list-item">
          <div>
            <h3>Listings stay active when you&apos;re out of stock</h3>
            <p>Keep listings active when inventory is temporarily zero so watchers and SEO signals remain.</p>
          </div>
          <label className="selling-preferences__toggle">
            <input
              type="checkbox"
              checked={preferences.multiQuantity.listingsStayActiveWhenOutOfStock}
              onChange={handleToggleListingsStayActive}
              disabled={pending.multiQuantity || isLoading}
            />
            <span />
          </label>
        </li>
        <li className="selling-preferences__list-item">
          <div>
            <h3>Buyers can see exactly how many items are left</h3>
            <p>Show remaining inventory so buyers have urgency to purchase.</p>
          </div>
          <label className="selling-preferences__toggle">
            <input
              type="checkbox"
              checked={preferences.multiQuantity.showExactQuantityAvailable}
              onChange={handleToggleShowQuantity}
              disabled={pending.multiQuantity || isLoading}
            />
            <span />
          </label>
        </li>
      </ul>
    </section>
  );

  const buyerSection = (
    <section className="selling-preferences__section">
      <header className="selling-preferences__section-header">
        <h2>Your buyers</h2>
        <p>Control who can buy from you and how blocked lists are applied.</p>
      </header>
      <ul className="selling-preferences__list">
        <li className="selling-preferences__list-item">
          <div>
            <h3>Managing who can buy from you</h3>
            <p>{buyerManagementSummary}</p>
          </div>
          <Link to="buyer-management" className="selling-preferences__action-link">
            Edit
          </Link>
        </li>
        <li className="selling-preferences__list-item">
          <div>
            <h3>Blocked buyer list</h3>
            <p>
              {preferences.blockedBuyerCount > 0
                ? `${preferences.blockedBuyerCount} buyer(s) currently blocked.`
                : "No blocked buyers yet."}
            </p>
          </div>
          <Link to="blocked-buyers" className="selling-preferences__action-link">
            Edit
          </Link>
        </li>
        <li className="selling-preferences__list-item">
          <div>
            <h3>Buyers can see your VAT number</h3>
            <p>
              {preferences.buyersCanSeeVatNumber && preferences.vatNumber
                ? `Showing VAT: ${preferences.vatNumber}`
                : "Hidden from buyers."}
            </p>
          </div>
          <button
            type="button"
            className="selling-preferences__action-link selling-preferences__action-link--disabled"
            onClick={() => Notice({ msg: "VAT preference management coming soon.", isSuccess: false })}
          >
            Edit
          </button>
        </li>
      </ul>
    </section>
  );

  const invoiceSection = (
    <section className="selling-preferences__section">
      <header className="selling-preferences__section-header">
        <h2>Your monthly invoice</h2>
        <p>Decide how you receive and pay your monthly eBay invoice.</p>
      </header>
      <ul className="selling-preferences__list">
        <li className="selling-preferences__list-item">
          <div>
            <h3>Format</h3>
            <p>Select the invoice detail level you prefer.</p>
          </div>
          <select
            value={preferences.invoice.format}
            onChange={handleInvoiceFormatChange}
            disabled={pending.invoice || isLoading}
          >
            {INVOICE_FORMAT_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </li>
        <li className="selling-preferences__list-item">
          <div>
            <h3>Receive email with detailed invoice</h3>
            <p>Get a copy of the detailed invoice sent to your primary email each month.</p>
          </div>
          <label className="selling-preferences__toggle">
            <input
              type="checkbox"
              checked={preferences.invoice.sendEmailCopy}
              onChange={handleToggleInvoiceEmail}
              disabled={pending.invoice || isLoading}
            />
            <span />
          </label>
        </li>
        <li className="selling-preferences__list-item">
          <div>
            <h3>Apply account credits to amount due</h3>
            <p>Automatically use any available credits toward your invoice payment.</p>
          </div>
          <label className="selling-preferences__toggle">
            <input
              type="checkbox"
              checked={preferences.invoice.applyCreditsAutomatically}
              onChange={handleToggleApplyCredits}
              disabled={pending.invoice || isLoading}
            />
            <span />
          </label>
        </li>
      </ul>
    </section>
  );

  if (isLoading) {
    return (
      <div className="selling-preferences selling-preferences--loading">
        <div className="selling-preferences__loading-state">Loading selling preferences…</div>
      </div>
    );
  }

  return (
    <div className="selling-preferences" data-testid="selling-preferences-page">
      <header className="selling-preferences__hero">
        <div>
          <h1>Selling preferences</h1>
          <p>Review automation and buyer management rules that apply to all of your listings.</p>
        </div>
        <button
          type="button"
          className="selling-preferences__hero-refresh"
          onClick={fetchPreferences}
          disabled={isLoading || pending.multiQuantity || pending.invoice}
        >
          Refresh
        </button>
      </header>

      <section className="selling-preferences__section">
        <header className="selling-preferences__section-header">
          <h2>All listings and orders</h2>
          <p>Quick shortcuts to manage automation for daily operations.</p>
        </header>
        <ul className="selling-preferences__list">
          {["Automate feedback", "Return preferences", "Sales tax table", "Preferences for items awaiting payment"].map((item) => (
            <li key={item} className="selling-preferences__list-item">
              <div>
                <h3>{item}</h3>
                <p>Managed from the legacy settings experience.</p>
              </div>
              <button
                type="button"
                className="selling-preferences__action-link selling-preferences__action-link--disabled"
                onClick={() => Notice({ msg: `${item} configuration will be available soon.`, isSuccess: false })}
              >
                Edit
              </button>
            </li>
          ))}
        </ul>
      </section>

      {multiQuantitySection}
      {buyerSection}
      {invoiceSection}
    </div>
  );
};

export default SellingPreferencesPage;
