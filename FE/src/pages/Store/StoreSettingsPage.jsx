import React, { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import StoreService from "../../services/StoreService";
import FileService from "../../services/FileService";
import Notice from "../../components/Common/CustomNotification";
import { HiGlobeAlt } from "react-icons/hi2";
import "./StoreSettingsPage.scss";

const DEFAULT_SOCIAL = { facebook: "", instagram: "", twitter: "" };

const parseSocialLinks = (raw) => {
    if (!raw) return { ...DEFAULT_SOCIAL };
    try {
        const parsed = typeof raw === "string" ? JSON.parse(raw) : raw;
        return { ...DEFAULT_SOCIAL, ...parsed };
    } catch {
        return { ...DEFAULT_SOCIAL };
    }
};

const StoreSettingsPage = () => {
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    const storeId = searchParams.get("storeId");

    const [loading, setLoading] = useState(false);
    const [loadingData, setLoadingData] = useState(true);
    const [uploadingLogo, setUploadingLogo] = useState(false);
    const [uploadingBanner, setUploadingBanner] = useState(false);
    const [formData, setFormData] = useState({
        name: "",
        description: "",
        logoUrl: "",
        bannerUrl: "",
        themeColor: "#3b82f6",
        contactEmail: "",
        contactPhone: "",
    });
    const [socialLinks, setSocialLinks] = useState({ ...DEFAULT_SOCIAL });

    useEffect(() => {
        if (storeId) {
            fetchStoreData();
        } else {
            setLoadingData(false);
            Notice("error", "No store selected. Please select a store first.");
            navigate("/stores");
        }
    }, [storeId, navigate]);

    const fetchStoreData = async () => {
        try {
            const response = await StoreService.getStoreById(storeId);
            if (response?.data) {
                const d = response.data;
                setFormData({
                    name: d.Name || d.name || "",
                    description: d.Description || d.description || "",
                    logoUrl: d.LogoUrl || d.logoUrl || "",
                    bannerUrl: d.BannerUrl || d.bannerUrl || "",
                    themeColor: d.ThemeColor || d.themeColor || "#3b82f6",
                    contactEmail: d.ContactEmail || d.contactEmail || "",
                    contactPhone: d.ContactPhone || d.contactPhone || "",
                });
                setSocialLinks(parseSocialLinks(d.SocialLinks || d.socialLinks));
            }
        } catch (error) {
            Notice("error", "Failed to load store data");
        } finally {
            setLoadingData(false);
        }
    };

    const handleChange = (field, value) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSocialChange = (platform, value) => {
        setSocialLinks((prev) => ({ ...prev, [platform]: value }));
    };

    const handleFileUpload = async (file, type) => {
        if (type === "logo") setUploadingLogo(true);
        else setUploadingBanner(true);

        try {
            const response = await FileService.upload(file);
            const uploadedUrl = response?.data;
            if (!uploadedUrl || typeof uploadedUrl !== "string") {
                throw new Error("Failed to get uploaded file URL");
            }
            setFormData((prev) => ({
                ...prev,
                [type === "logo" ? "logoUrl" : "bannerUrl"]: uploadedUrl,
            }));
            Notice("success", `${type === "logo" ? "Logo" : "Banner"} uploaded successfully!`);
        } catch (error) {
            console.error(`${type} upload failed:`, error);
            Notice("error", `Failed to upload ${type}. Please try again.`);
        } finally {
            if (type === "logo") setUploadingLogo(false);
            else setUploadingBanner(false);
        }
    };

    const handleLogoChange = (e) => {
        const file = e.target.files?.[0];
        if (!file) return;
        if (!file.type.startsWith("image/")) {
            Notice("error", "Please select an image file");
            return;
        }
        handleFileUpload(file, "logo");
    };

    const handleBannerChange = (e) => {
        const file = e.target.files?.[0];
        if (!file) return;
        if (!file.type.startsWith("image/")) {
            Notice("error", "Please select an image file");
            return;
        }
        handleFileUpload(file, "banner");
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!formData.name.trim()) {
            Notice("error", "Store name is required");
            return;
        }

        setLoading(true);
        try {
            const hasSocial = Object.values(socialLinks).some((v) => v.trim());
            const payload = {
                ...formData,
                socialLinks: hasSocial ? JSON.stringify(socialLinks) : null,
            };
            await StoreService.updateStoreProfile(storeId, payload);
            Notice("success", "Store profile updated successfully!");
        } catch (error) {
            Notice("error", error?.response?.data?.detail || "Failed to update store");
        } finally {
            setLoading(false);
        }
    };

    if (loadingData) {
        return <div className="loading">Loading...</div>;
    }

    return (
        <div className="store-settings-page">
            <div className="container">
                <div className="page-header">
                    <h1>Store Settings</h1>
                    <p>Update your store profile information</p>
                </div>

                <form onSubmit={handleSubmit} className="settings-form">
                    {/* Basic Information */}
                    <div className="form-section">
                        <h2>Basic Information</h2>

                        <div className="form-group">
                            <label htmlFor="store-name">Store Name *</label>
                            <input
                                type="text"
                                id="store-name"
                                required
                                maxLength={255}
                                value={formData.name}
                                onChange={(e) => handleChange("name", e.target.value)}
                                placeholder="Enter your store name"
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="store-description">Description</label>
                            <textarea
                                id="store-description"
                                rows="5"
                                maxLength={2000}
                                value={formData.description}
                                onChange={(e) => handleChange("description", e.target.value)}
                                placeholder="Describe your store..."
                            />
                        </div>
                    </div>

                    {/* Store Images */}
                    <div className="form-section">
                        <h2>Store Images</h2>

                        <div className="form-group">
                            <label htmlFor="logo-upload">Store Logo</label>
                            <div className="file-upload-group">
                                <input
                                    type="file"
                                    id="logo-upload"
                                    accept="image/*"
                                    onChange={handleLogoChange}
                                    style={{ display: "none" }}
                                    disabled={uploadingLogo}
                                />
                                <label htmlFor="logo-upload" className="file-upload-button">
                                    {uploadingLogo ? "Uploading..." : "Choose Logo"}
                                </label>
                                {formData.logoUrl && (
                                    <div className="image-preview">
                                        <img src={formData.logoUrl} alt="Logo preview" />
                                        <button
                                            type="button"
                                            className="remove-image"
                                            onClick={() => setFormData((prev) => ({ ...prev, logoUrl: "" }))}
                                            disabled={uploadingLogo}
                                        >
                                            ×
                                        </button>
                                    </div>
                                )}
                            </div>
                            <small>Square image, recommended: 500x500px</small>
                        </div>

                        <div className="form-group">
                            <label htmlFor="banner-upload">Store Banner</label>
                            <div className="file-upload-group">
                                <input
                                    type="file"
                                    id="banner-upload"
                                    accept="image/*"
                                    onChange={handleBannerChange}
                                    style={{ display: "none" }}
                                    disabled={uploadingBanner}
                                />
                                <label htmlFor="banner-upload" className="file-upload-button">
                                    {uploadingBanner ? "Uploading..." : "Choose Banner"}
                                </label>
                                {formData.bannerUrl && (
                                    <div className="image-preview">
                                        <img src={formData.bannerUrl} alt="Banner preview" />
                                        <button
                                            type="button"
                                            className="remove-image"
                                            onClick={() => setFormData((prev) => ({ ...prev, bannerUrl: "" }))}
                                            disabled={uploadingBanner}
                                        >
                                            ×
                                        </button>
                                    </div>
                                )}
                            </div>
                            <small>Wide image, recommended: 1200x300px</small>
                        </div>
                    </div>

                    {/* Theme */}
                    <div className="form-section">
                        <h2>Theme</h2>

                        <div className="form-group">
                            <label htmlFor="theme-color">Brand Color</label>
                            <div style={{ display: "flex", alignItems: "center", gap: "12px" }}>
                                <input
                                    type="color"
                                    id="theme-color"
                                    value={formData.themeColor}
                                    onChange={(e) => handleChange("themeColor", e.target.value)}
                                    style={{ width: 48, height: 48, padding: 0, border: "1px solid #d1d5db", borderRadius: 8, cursor: "pointer" }}
                                />
                                <input
                                    type="text"
                                    value={formData.themeColor}
                                    onChange={(e) => handleChange("themeColor", e.target.value)}
                                    maxLength={50}
                                    placeholder="#3b82f6"
                                    style={{ width: 120 }}
                                />
                                <div
                                    style={{
                                        width: 80,
                                        height: 32,
                                        borderRadius: 6,
                                        backgroundColor: formData.themeColor,
                                        border: "1px solid #e5e7eb",
                                    }}
                                />
                            </div>
                            <small>This color will be used for your store's branding accents</small>
                        </div>
                    </div>

                    {/* Contact Information */}
                    <div className="form-section">
                        <h2>Contact Information</h2>

                        <div className="form-group">
                            <label htmlFor="contact-email">Contact Email</label>
                            <input
                                type="email"
                                id="contact-email"
                                maxLength={255}
                                value={formData.contactEmail}
                                onChange={(e) => handleChange("contactEmail", e.target.value)}
                                placeholder="support@yourstore.com"
                            />
                            <small>Public email customers can reach you at</small>
                        </div>

                        <div className="form-group">
                            <label htmlFor="contact-phone">Contact Phone</label>
                            <input
                                type="tel"
                                id="contact-phone"
                                maxLength={50}
                                value={formData.contactPhone}
                                onChange={(e) => handleChange("contactPhone", e.target.value)}
                                placeholder="+84 xxx xxx xxxx"
                            />
                            <small>Public phone number for customer inquiries</small>
                        </div>
                    </div>

                    {/* Social Links */}
                    <div className="form-section">
                        <h2>
                            <span style={{ display: "flex", alignItems: "center", gap: 8 }}>
                                <HiGlobeAlt /> Social Links
                            </span>
                        </h2>

                        <div className="form-group">
                            <label htmlFor="social-facebook">Facebook</label>
                            <input
                                type="url"
                                id="social-facebook"
                                value={socialLinks.facebook}
                                onChange={(e) => handleSocialChange("facebook", e.target.value)}
                                placeholder="https://facebook.com/yourstore"
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="social-instagram">Instagram</label>
                            <input
                                type="url"
                                id="social-instagram"
                                value={socialLinks.instagram}
                                onChange={(e) => handleSocialChange("instagram", e.target.value)}
                                placeholder="https://instagram.com/yourstore"
                            />
                        </div>

                        <div className="form-group">
                            <label htmlFor="social-twitter">Twitter / X</label>
                            <input
                                type="url"
                                id="social-twitter"
                                value={socialLinks.twitter}
                                onChange={(e) => handleSocialChange("twitter", e.target.value)}
                                placeholder="https://x.com/yourstore"
                            />
                        </div>
                    </div>

                    {/* Actions */}
                    <div className="form-actions">
                        <button type="button" onClick={() => navigate(-1)} className="btn-secondary">
                            Cancel
                        </button>
                        <button type="submit" className="btn-primary" disabled={loading}>
                            {loading ? "Saving..." : "Save Changes"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default StoreSettingsPage;
