import React, { useCallback, useEffect, useMemo, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import Notice from "../../../components/Common/CustomNotification";
import SellingPreferenceService from "../../../services/SellingPreferenceService";
import "./BlockedBuyerListPage.scss";

const LIMITS = {
  blocked: 5000,
  exempt: 1000
};

const MODE_LABELS = {
  blocked: "Blocked buyer list",
  exempt: "Exempt buyer list"
};

const BlockedBuyerListPage = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const activeMode = useMemo(() => {
    const mode = searchParams.get("mode");
    return mode === "exempt" ? "exempt" : "blocked";
  }, [searchParams]);

  const [inputValue, setInputValue] = useState("");
  const [lastUpdatedAtUtc, setLastUpdatedAtUtc] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [isSaving, setIsSaving] = useState(false);

  const parseListToTextarea = useCallback((items) => items.join(", \n"), []);

  const parseTextareaToList = useCallback((value) => {
    if (!value) {
      return [];
    }

    return value
      .split(/[\n,]/)
      .map((item) => item.trim())
      .filter((item) => item.length > 0);
  }, []);

  const loadList = useCallback(async () => {
    setIsLoading(true);
    try {
      const serviceCall = activeMode === "exempt"
        ? SellingPreferenceService.getExemptBuyers
        : SellingPreferenceService.getBlockedBuyers;
      const result = await serviceCall();
      setInputValue(parseListToTextarea(result.items));
      setLastUpdatedAtUtc(result.lastUpdatedAtUtc ?? null);
    } catch (error) {
      console.error("Failed to load buyer list", error);
      Notice({ msg: "Unable to load the buyer list. Please try again later.", isSuccess: false });
    } finally {
      setIsLoading(false);
    }
  }, [activeMode, parseListToTextarea]);

  useEffect(() => {
    loadList();
  }, [loadList, activeMode]);

  const handleTabClick = (mode) => {
    setSearchParams((prev) => {
      const next = new URLSearchParams(prev);
      if (mode === "blocked") {
        next.delete("mode");
      } else {
        next.set("mode", mode);
      }
      return next;
    });
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setIsSaving(true);

    const items = parseTextareaToList(inputValue);
    const limit = LIMITS[activeMode];
    if (items.length > limit) {
      Notice({
        msg: `You can only ${activeMode === "blocked" ? "block" : "exempt"} up to ${limit} buyers`,
        isSuccess: false
      });
      setIsSaving(false);
      return;
    }

    try {
      const serviceCall = activeMode === "exempt"
        ? SellingPreferenceService.updateExemptBuyers
        : SellingPreferenceService.updateBlockedBuyers;
      const result = await serviceCall(items);
      setInputValue(parseListToTextarea(result.items));
      setLastUpdatedAtUtc(result.lastUpdatedAtUtc ?? null);
      Notice({ msg: "List updated", isSuccess: true });
    } catch (error) {
      console.error("Failed to update buyer list", error);
      Notice({ msg: "Could not update the list. Please try again.", isSuccess: false });
    } finally {
      setIsSaving(false);
    }
  };

  const helperText = useMemo(() => {
    const verb = activeMode === "blocked" ? "blocked" : "exempt";
    const limit = LIMITS[activeMode];
    return `Enter each buyer's username or email address, separated by commas or new lines. You can have up to ${limit} ${verb} buyers.`;
  }, [activeMode]);

  const formattedUpdatedAt = useMemo(() => {
    if (!lastUpdatedAtUtc) {
      return "Never";
    }

    const parsed = new Date(lastUpdatedAtUtc);
    if (Number.isNaN(parsed.getTime())) {
      return "Unknown";
    }

    return parsed.toLocaleString();
  }, [lastUpdatedAtUtc]);

  return (
    <div className="buyer-list" data-testid="buyer-list-page">
      <header className="buyer-list__header">
        <div>
          <h1>{MODE_LABELS[activeMode]}</h1>
          <p>
            Control who is {activeMode === "blocked" ? "restricted" : "always allowed"} to purchase your listings. Applying
            updates will affect all active and future listings.
          </p>
        </div>
        <Link to=".." relative="path" className="buyer-list__back-link">
          ← Back to selling preferences
        </Link>
      </header>

      <nav className="buyer-list__tabs" aria-label="Buyer list mode">
        <button
          type="button"
          className={`buyer-list__tab${activeMode === "blocked" ? " is-active" : ""}`}
          onClick={() => handleTabClick("blocked")}
          disabled={isSaving}
        >
          Blocked buyers
        </button>
        <button
          type="button"
          className={`buyer-list__tab${activeMode === "exempt" ? " is-active" : ""}`}
          onClick={() => handleTabClick("exempt")}
          disabled={isSaving}
        >
          Exempt buyers
        </button>
      </nav>

      <form className="buyer-list__card" onSubmit={handleSubmit}>
        <label className="buyer-list__label" htmlFor="buyer-list-textarea">
          {MODE_LABELS[activeMode]}
        </label>
        <textarea
          id="buyer-list-textarea"
          value={inputValue}
          onChange={(event) => setInputValue(event.target.value)}
          placeholder="Insert username or email addresses separated by comma."
          rows={12}
          disabled={isLoading || isSaving}
        />
        <p className="buyer-list__helper">{helperText}</p>
        <div className="buyer-list__meta">
          <span>Last updated: {formattedUpdatedAt}</span>
          <span>
            Current entries: {parseTextareaToList(inputValue).length} / {LIMITS[activeMode]}
          </span>
        </div>
        <footer className="buyer-list__actions">
          <button type="submit" disabled={isLoading || isSaving}>
            {isSaving ? "Saving…" : "Save changes"}
          </button>
          <button
            type="button"
            className="buyer-list__secondary"
            onClick={loadList}
            disabled={isLoading || isSaving}
          >
            Reset
          </button>
        </footer>
      </form>
    </div>
  );
};

export default BlockedBuyerListPage;
