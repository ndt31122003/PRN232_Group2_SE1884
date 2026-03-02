import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import StoreService from "../../services/StoreService";
import Notice from "../../components/Common/CustomNotification";
import "./MyStoresPage.scss";

const MyStoresPage = () => {
    const navigate = useNavigate();
    const [stores, setStores] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchStores();
    }, []);

    const fetchStores = async () => {
        try {
            const response = await StoreService.getMyStores();
            console.log("API Response:", response?.data);
            if (response?.data) {
                setStores(response.data);
            }
        } catch (error) {
            console.error("Failed to load stores:", error);
        } finally {
            setLoading(false);
        }
    };

    const handleCreateStore = () => {
        navigate("/store/create");
    };

    const handleSelectStore = (storeId) => {
        navigate(`/store/settings?storeId=${storeId}`);
    };

    if (loading) {
        return (
            <div className="my-stores-page">
                <div className="container">
                    <div className="loading">Loading...</div>
                </div>
            </div>
        );
    }

    return (
        <div className="my-stores-page">
            <div className="container">
                <div className="page-header">
                    <h1>My Stores</h1>
                    <p>Manage your eBay stores</p>
                </div>

                {stores.length === 0 ? (
                    <div className="empty-state">
                        <div className="empty-state__icon">
                            <svg width="64" height="64" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
                                <path d="M3 21l9-9 3 3 9-9M3 21h18M3 21v-4.586l9-9m0 0l9 9" />
                            </svg>
                        </div>
                        <h2>No stores yet</h2>
                        <p>Create your first store to start selling on eBay Clone</p>
                        <button className="btn-primary" onClick={handleCreateStore}>
                            Create Store
                        </button>
                    </div>
                ) : (
                    <div>
                        <div className="actions-bar">
                            <button className="btn-primary" onClick={handleCreateStore}>
                                + Create New Store
                            </button>
                        </div>

                        <div className="stores-grid">
                            {stores.map((store, index) => {
                                // Support both PascalCase and camelCase
                                const storeId = store.StoreId || store.id || store.storeId;
                                const name = store.Name || store.name;
                                const logoUrl = store.LogoUrl || store.logoUrl;
                                const bannerUrl = store.BannerUrl || store.bannerUrl;
                                const storeType = store.StoreType ?? store.storeType ?? 0;
                                const isActive = store.IsActive ?? store.isActive ?? false;
                                
                                console.log(`Store ${index}:`, { storeId, name, logoUrl, bannerUrl, storeType, isActive });
                                
                                return (
                                <div key={storeId} className="store-card" onClick={() => handleSelectStore(storeId)}>
                                    {bannerUrl ? (
                                        <img src={bannerUrl} alt={`${name} banner`} className="store-card__banner" />
                                    ) : (
                                        <div className="store-card__banner store-card__banner--placeholder"></div>
                                    )}
                                    <div className="store-card__content">
                                        {logoUrl && (
                                            <img src={logoUrl} alt={name} className="store-card__logo" />
                                        )}
                                        
                                        <h3 className="store-card__name">{name || "Unnamed Store"}</h3>

                                        <div className="store-card__badge">
                                            {storeType === 0 && "Basic"}
                                            {storeType === 1 && "Premium"}
                                            {storeType === 2 && "Anchor"}
                                        </div>

                                        <div className="store-card__status">
                                            <span className={`status-indicator ${isActive ? 'status-indicator--active' : 'status-indicator--inactive'}`}>
                                                {isActive ? 'Active' : 'Inactive'}
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            );
                            })}
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default MyStoresPage;

