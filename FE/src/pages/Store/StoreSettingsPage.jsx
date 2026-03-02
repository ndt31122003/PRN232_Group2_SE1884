import React, { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import StoreService from "../../services/StoreService";
import FileService from "../../services/FileService";
import Notice from "../../components/Common/CustomNotification";
import "./StoreSettingsPage.scss";

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
        bannerUrl: ""
    });

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
                const data = response.data;
                setFormData({
                    name: data.Name || data.name || "",
                    description: data.Description || data.description || "",
                    logoUrl: data.LogoUrl || data.logoUrl || "",
                    bannerUrl: data.BannerUrl || data.bannerUrl || ""
                });
            }
        } catch (error) {
            Notice("error", "Failed to load store data");
        } finally {
            setLoadingData(false);
        }
    };

    const handleChange = (field, value) => {
        setFormData(prev => ({ ...prev, [field]: value }));
    };

    const handleFileUpload = async (file, type) => {
        if (type === 'logo') {
            setUploadingLogo(true);
        } else {
            setUploadingBanner(true);
        }

        try {
            const response = await FileService.upload(file);
            const uploadedUrl = response?.data;

            if (!uploadedUrl || typeof uploadedUrl !== "string") {
                throw new Error("Failed to get uploaded file URL");
            }

            setFormData(prev => ({
                ...prev,
                [type === 'logo' ? 'logoUrl' : 'bannerUrl']: uploadedUrl
            }));

            Notice("success", `${type === 'logo' ? 'Logo' : 'Banner'} uploaded successfully!`);
        } catch (error) {
            console.error(`${type} upload failed:`, error);
            Notice("error", `Failed to upload ${type}. Please try again.`);
        } finally {
            if (type === 'logo') {
                setUploadingLogo(false);
            } else {
                setUploadingBanner(false);
            }
        }
    };

    const handleLogoChange = (e) => {
        const file = e.target.files?.[0];
        if (!file) return;

        if (!file.type.startsWith('image/')) {
            Notice("error", "Please select an image file");
            return;
        }

        handleFileUpload(file, 'logo');
    };

    const handleBannerChange = (e) => {
        const file = e.target.files?.[0];
        if (!file) return;

        if (!file.type.startsWith('image/')) {
            Notice("error", "Please select an image file");
            return;
        }

        handleFileUpload(file, 'banner');
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!formData.name.trim()) {
            Notice("error", "Store name is required");
            return;
        }

        setLoading(true);
        try {
            await StoreService.updateStoreProfile(storeId, formData);
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
                                            onClick={() => setFormData(prev => ({ ...prev, logoUrl: "" }))}
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
                                            onClick={() => setFormData(prev => ({ ...prev, bannerUrl: "" }))}
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

