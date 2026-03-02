import React, { useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import Notice from "../../../components/Common/CustomNotification";
import SellingPreferenceService from "../../../services/SellingPreferenceService";
import "./BuyerManagementPage.scss";

const DEFAULT_FORM = {
  blockUnpaidItemStrikes: false,
  unpaidItemStrikesCount: 2,
  unpaidItemStrikesPeriodInMonths: 1,
  blockPrimaryAddressOutsideShippingLocation: true,
  blockMaxItemsInLastTenDays: false,
  maxItemsInLastTenDays: 1,
  applyFeedbackScoreThreshold: false,
  feedbackScoreThreshold: 5,
  updateBlockSettingsForActiveListings: false,
  requirePaymentMethodBeforeBid: true,
  requirePaymentMethodBeforeOffer: true,
  preventBlockedBuyersFromContacting: true
};

const BuyerManagementPage = () => {
  const [form, setForm] = useState(DEFAULT_FORM);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);

  const loadBuyerManagement = useCallback(async () => {
    setIsLoading(true);
    try {
      const data = await SellingPreferenceService.getBuyerManagement();
      const { blockSettings, rules } = data;
      setForm({
        blockUnpaidItemStrikes: blockSettings.blockUnpaidItemStrikes,
        unpaidItemStrikesCount: blockSettings.unpaidItemStrikesCount,
        unpaidItemStrikesPeriodInMonths: blockSettings.unpaidItemStrikesPeriodInMonths,
        blockPrimaryAddressOutsideShippingLocation: blockSettings.blockPrimaryAddressOutsideShippingLocation,
        blockMaxItemsInLastTenDays: blockSettings.blockMaxItemsInLastTenDays,
        maxItemsInLastTenDays: blockSettings.maxItemsInLastTenDays ?? DEFAULT_FORM.maxItemsInLastTenDays,
        applyFeedbackScoreThreshold: blockSettings.applyFeedbackScoreThreshold,
        feedbackScoreThreshold: blockSettings.feedbackScoreThreshold ?? DEFAULT_FORM.feedbackScoreThreshold,
        updateBlockSettingsForActiveListings: blockSettings.updateBlockSettingsForActiveListings,
        requirePaymentMethodBeforeBid: rules.requirePaymentMethodBeforeBid,
        requirePaymentMethodBeforeOffer: rules.requirePaymentMethodBeforeOffer,
        preventBlockedBuyersFromContacting: rules.preventBlockedBuyersFromContacting
      });
    } catch (error) {
      console.error("Failed to load buyer management settings", error);
      Notice({ msg: "Unable to load buyer management. Please try again later.", isSuccess: false });
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    loadBuyerManagement();
  }, [loadBuyerManagement]);

  const handleCheckboxChange = (event) => {
    const { name, checked } = event.target;
    setForm((prev) => ({ ...prev, [name]: checked }));
  };

  const handleNumberChange = (event) => {
    const { name, value } = event.target;
    const parsed = Number.parseInt(value, 10);
    setForm((prev) => ({ ...prev, [name]: Number.isNaN(parsed) ? "" : parsed }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setIsSaving(true);

    const payload = {
      ...form,
      maxItemsInLastTenDays: form.blockMaxItemsInLastTenDays ? form.maxItemsInLastTenDays : undefined,
      feedbackScoreThreshold: form.applyFeedbackScoreThreshold ? form.feedbackScoreThreshold : undefined
    };

    try {
      await SellingPreferenceService.updateBuyerManagement(payload);
      Notice({ msg: "Buyer management settings updated", isSuccess: true });
      await loadBuyerManagement();
    } catch (error) {
      console.error("Failed to update buyer management", error);
      Notice({ msg: "Could not save buyer management settings.", isSuccess: false });
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="buyer-management" data-testid="buyer-management-page">
      <header className="buyer-management__header">
        <div>
          <h1>Buyer management</h1>
          <p>Manage who can bid on or purchase your listings by defining block rules and buyer requirements.</p>
        </div>
        <Link to=".." relative="path" className="buyer-management__back-link">
          ← Back to selling preferences
        </Link>
      </header>

      <form className="buyer-management__form" onSubmit={handleSubmit}>
        <fieldset className="buyer-management__section" disabled={isLoading || isSaving}>
          <legend>Block settings</legend>
          <div className="buyer-management__row">
            <label className="buyer-management__checkbox">
              <input
                type="checkbox"
                name="blockUnpaidItemStrikes"
                checked={form.blockUnpaidItemStrikes}
                onChange={handleCheckboxChange}
              />
              <span>
                Block buyers who caused
                <input
                  type="number"
                  min={1}
                  max={4}
                  name="unpaidItemStrikesCount"
                  value={form.unpaidItemStrikesCount}
                  onChange={handleNumberChange}
                  disabled={!form.blockUnpaidItemStrikes}
                />
                cancellations of unpaid order within
                <input
                  type="number"
                  min={1}
                  max={12}
                  name="unpaidItemStrikesPeriodInMonths"
                  value={form.unpaidItemStrikesPeriodInMonths}
                  onChange={handleNumberChange}
                  disabled={!form.blockUnpaidItemStrikes}
                />
                month(s)
              </span>
            </label>
          </div>

          <div className="buyer-management__row">
            <label className="buyer-management__checkbox">
              <input
                type="checkbox"
                name="blockPrimaryAddressOutsideShippingLocation"
                checked={form.blockPrimaryAddressOutsideShippingLocation}
                onChange={handleCheckboxChange}
              />
              <span>Block buyers whose primary shipping address is in a location to which I don&apos;t ship</span>
            </label>
          </div>

          <div className="buyer-management__row">
            <label className="buyer-management__checkbox">
              <input
                type="checkbox"
                name="blockMaxItemsInLastTenDays"
                checked={form.blockMaxItemsInLastTenDays}
                onChange={handleCheckboxChange}
              />
              <span>
                Block buyers who are currently winning or have bought
                <input
                  type="number"
                  min={1}
                  max={25}
                  name="maxItemsInLastTenDays"
                  value={form.maxItemsInLastTenDays}
                  onChange={handleNumberChange}
                  disabled={!form.blockMaxItemsInLastTenDays}
                />
                items in the last 10 days
              </span>
            </label>
          </div>

          <div className="buyer-management__row buyer-management__row--nested">
            <label className="buyer-management__checkbox">
              <input
                type="checkbox"
                name="applyFeedbackScoreThreshold"
                checked={form.applyFeedbackScoreThreshold}
                onChange={handleCheckboxChange}
              />
              <span>
                Only apply this filter to buyers with a feedback score up to
                <input
                  type="number"
                  min={0}
                  max={1000}
                  name="feedbackScoreThreshold"
                  value={form.feedbackScoreThreshold}
                  onChange={handleNumberChange}
                  disabled={!form.applyFeedbackScoreThreshold}
                />
              </span>
            </label>
          </div>

          <div className="buyer-management__row">
            <label className="buyer-management__checkbox buyer-management__checkbox--inline">
              <input
                type="checkbox"
                name="updateBlockSettingsForActiveListings"
                checked={form.updateBlockSettingsForActiveListings}
                onChange={handleCheckboxChange}
              />
              <span>Update block settings for active listings</span>
            </label>
          </div>
        </fieldset>

        <fieldset className="buyer-management__section" disabled={isLoading || isSaving}>
          <legend>Buyer rules</legend>
          <label className="buyer-management__checkbox">
            <input
              type="checkbox"
              name="requirePaymentMethodBeforeBid"
              checked={form.requirePaymentMethodBeforeBid}
              onChange={handleCheckboxChange}
            />
            <span>Require buyers to provide a payment method before they place a bid</span>
          </label>

          <label className="buyer-management__checkbox">
            <input
              type="checkbox"
              name="requirePaymentMethodBeforeOffer"
              checked={form.requirePaymentMethodBeforeOffer}
              onChange={handleCheckboxChange}
            />
            <span>Require all buyers to provide a payment method before they make an offer</span>
          </label>

          <label className="buyer-management__checkbox">
            <input
              type="checkbox"
              name="preventBlockedBuyersFromContacting"
              checked={form.preventBlockedBuyersFromContacting}
              onChange={handleCheckboxChange}
            />
            <span>Don&apos;t allow blocked buyers to contact me</span>
          </label>
        </fieldset>

        <footer className="buyer-management__footer">
          <button type="submit" disabled={isLoading || isSaving}>
            {isSaving ? "Saving…" : "Save changes"}
          </button>
          <Link to="../blocked-buyers" relative="path" className="buyer-management__secondary">
            Manage blocked buyer list
          </Link>
          <Link
            to={{ pathname: "../blocked-buyers", search: "?mode=exempt" }}
            relative="path"
            className="buyer-management__secondary"
          >
            Manage exempt buyer list
          </Link>
        </footer>
      </form>
    </div>
  );
};

export default BuyerManagementPage;
