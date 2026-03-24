import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import instance from "../../utils/axiosCustomize";
import { getCurrentUserId } from "../../utils/jwtUtils";
import "./PromotionsOverview.scss";

const PromotionsOverview = () => {
  const navigate = useNavigate();
  const [promotions, setPromotions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [activeDropdown, setActiveDropdown] = useState(null);

  useEffect(() => {
    fetchPromotions();
  }, []);

  useEffect(() => {
    const handleClickOutside = (event) => {
      if (activeDropdown && !event.target.closest('.action-dropdown')) {
        setActiveDropdown(null);
      }
    };

    document.addEventListener('click', handleClickOutside);
    return () => document.removeEventListener('click', handleClickOutside);
  }, [activeDropdown]);

  const fetchPromotions = async () => {
    try {
      setLoading(true);
      
      // Fetch all promotion types sequentially to prevent exhausting Supabase connection pool limits
      const sellerId = getCurrentUserId() ?? '00000000-0000-0000-0000-000000000000';
      
      const couponsRes = await instance.get('/coupons').catch(() => ({ data: [] }));
      const orderDiscountsRes = await instance.get(`/order-discounts/seller/${sellerId}`).catch(() => ({ data: [] }));
      const shippingRes = await instance.get(`/shipping-discounts/seller/${sellerId}`).catch(() => ({ data: [] }));
      const volumeRes = await instance.get(`/volume-pricings/seller/${sellerId}`).catch(() => ({ data: [] }));
      
      // Combine and format promotions
      const allPromotions = [
        ...(couponsRes.data || []).map(c => ({
          ...c,
          promotionType: 'Coupon',
          discountDisplay: c.discountType === 1 ? `${c.discountValue}%` : `$${c.discountValue}`,
          typeDescription: `Coupon code: ${c.code}`
        })),
        ...(orderDiscountsRes.data || []).map(d => ({
          ...d,
          promotionType: 'Order discount',
          discountDisplay: d.discountUnit === 1 ? `${d.discountValue}%` : `$${d.discountValue}`,
          typeDescription: d.thresholdType === 1 
            ? `Spend $${d.thresholdAmount}+` 
            : `Buy ${d.thresholdQuantity}+ items`
        })),
        ...(shippingRes.data || []).map(s => ({
          ...s,
          promotionType: 'Shipping discount',
          discountDisplay: s.isFreeShipping ? 'Free Shipping' : (s.discountUnit === "percent" || s.discountUnit === 1 ? `${s.discountValue}%` : `$${s.discountValue}`),
          typeDescription: `Shipping offer`
        })),
        ...(volumeRes.data || []).map(v => ({
          ...v,
          promotionType: 'Volume pricing',
          discountDisplay: v.tiers?.length ? `Up to ${Math.max(...v.tiers.map(t => t.discountValue))}${v.tiers[0].discountUnit === 1 ? '%' : '$'} off` : 'Volume discount',
          typeDescription: `Multi-buy tiers`
        }))
      ];
      
      setPromotions(allPromotions);
    } catch (error) {
      console.error("Error fetching promotions:", error);
      setPromotions([]);
    } finally {
      setLoading(false);
    }
  };

  const filteredPromotions = promotions.filter(promo =>
    promo.name.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  const getDiscountDisplay = (promo) => {
    return promo.discountDisplay || 'N/A';
  };

  const getDiscountType = (promo) => {
    return `${promo.promotionType} - ${promo.typeDescription}`;
  };

  const toggleDropdown = (promoId) => {
    setActiveDropdown(activeDropdown === promoId ? null : promoId);
  };

  const handleActivateDeactivate = async (promo) => {
    try {
      const newStatus = !promo.isActive;
      
      if (promo.promotionType === 'Coupon') {
        await instance.patch(`/coupons/${promo.id}/status`, { isActive: newStatus });
      } else if (promo.promotionType === 'Order discount') {
        const endpoint = newStatus ? 'activate' : 'deactivate';
        await instance.post(`/order-discounts/${promo.id}/${endpoint}`);
      } else if (promo.promotionType === 'Shipping discount') {
        const endpoint = newStatus ? 'activate' : 'deactivate';
        await instance.post(`/shipping-discounts/${promo.id}/${endpoint}`);
      } else if (promo.promotionType === 'Volume pricing') {
        const endpoint = newStatus ? 'activate' : 'deactivate';
        await instance.post(`/volume-pricings/${promo.id}/${endpoint}`);
      }
      
      // Refresh promotions
      await fetchPromotions();
      setActiveDropdown(null);
    } catch (error) {
      console.error("Error updating status:", error);
      alert("Failed to update promotion status");
    }
  };

  const handleDelete = async (promo) => {
    if (!window.confirm(`Are you sure you want to delete "${promo.name}"?`)) {
      return;
    }

    try {
      if (promo.promotionType === 'Coupon') {
        await instance.delete(`/coupons/${promo.id}`);
      } else if (promo.promotionType === 'Order discount') {
        await instance.delete(`/order-discounts/${promo.id}`);
      } else if (promo.promotionType === 'Shipping discount') {
        await instance.delete(`/shipping-discounts/${promo.id}`);
      } else if (promo.promotionType === 'Volume pricing') {
        await instance.delete(`/volume-pricings/${promo.id}`);
      }
      
      // Refresh promotions
      await fetchPromotions();
      setActiveDropdown(null);
    } catch (error) {
      console.error("Error deleting promotion:", error);
      alert("Failed to delete promotion");
    }
  };

  const handleEdit = (promo) => {
    // TODO: Navigate to edit page
    alert(`Edit functionality for ${promo.promotionType} coming soon!`);
    setActiveDropdown(null);
  };

  return (
    <div className="promotions-overview">
      <div className="promotions-overview__main-header">
        <h1 className="promotions-overview__main-title">Manage your discounts</h1>
      </div>

      <section className="promotions-overview__section">
        <div className="promotions-overview__section-header">
          <div className="promotions-overview__section-title">
            <h2>Performance</h2>
            <p>Check how your discounts are impacting orders and sales.</p>
          </div>
          <div className="promotions-overview__section-filters">
            <select className="promotions-overview__select">
              <option>All promotions</option>
            </select>
            <select className="promotions-overview__select">
              <option>Past 90 days</option>
            </select>
          </div>
        </div>

        <div className="promotions-overview__performance-chart">
          <div className="chart-header">Sales with all discounts</div>
          <div className="chart-body">
            <div className="chart-y-axis">
              <span>$100.00</span>
              <span>$80.00</span>
              <span>$60.00</span>
              <span>$40.00</span>
              <span>$20.00</span>
              <span>$0.00</span>
            </div>
            <div className="chart-grid">
              <div className="chart-grid-line"></div>
              <div className="chart-grid-line"></div>
              <div className="chart-grid-line"></div>
              <div className="chart-grid-line"></div>
              <div className="chart-grid-line"></div>
              <div className="chart-grid-line"></div>

              <div className="chart-empty-message">
                <strong>No results for the selected filters</strong>
                <p>Try adjusting your time period and discount type.</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      <section className="promotions-overview__section">
        <div className="promotions-overview__section-header">
          <div className="promotions-overview__section-title">
            <h2>Your discounts</h2>
            <p>Review and manage your discounts in one place.</p>
          </div>
        </div>

        <div className="promotions-overview__table-toolbar">
          <div className="search-bar">
            <svg
              className="search-icon"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
            >
              <path
                d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              />
            </svg>
            <input 
              type="text" 
              placeholder="Search by discount name"
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </div>
          <div className="table-filters">
            <select className="promotions-overview__select">
              <option>All promotions</option>
            </select>
            <select className="promotions-overview__select">
              <option>All status</option>
            </select>
            <select className="promotions-overview__select">
              <option>Past 90 days</option>
            </select>
          </div>
        </div>

        <div className="promotions-overview__table-container">
          <table className="promotions-table">
            <thead>
              <tr>
                <th className="th-checkbox">
                  <input type="checkbox" />
                </th>
                <th>Actions</th>
                <th>Status</th>
                <th>
                  Name <span className="sort-icon">↓↑</span>
                </th>
                <th>Discount type</th>
                <th>Discount value</th>
                <th>
                  Start date <span className="sort-icon">↓↑</span>
                </th>
                <th>
                  End date <span className="sort-icon">↓↑</span>
                </th>
              </tr>
            </thead>
            <tbody>
              {loading ? (
                <tr>
                  <td colSpan="8" className="promotions-table__loading">
                    Loading promotions...
                  </td>
                </tr>
              ) : filteredPromotions.length === 0 ? (
                <tr>
                  <td colSpan="8" className="promotions-table__empty">
                    <div className="empty-state">
                      <div className="empty-state__icon">
                        <svg width="80" height="80" viewBox="0 0 100 100" fill="none" xmlns="http://www.w3.org/2000/svg">
                          <rect x="20" y="25" width="50" height="60" rx="4" fill="#FDEEBD" stroke="#E2B17F" strokeWidth="2"/>
                          <line x1="25" y1="20" x2="25" y2="28" stroke="#555" strokeWidth="2" strokeLinecap="round"/>
                          <line x1="35" y1="20" x2="35" y2="28" stroke="#555" strokeWidth="2" strokeLinecap="round"/>
                          <line x1="45" y1="20" x2="45" y2="28" stroke="#555" strokeWidth="2" strokeLinecap="round"/>
                          <line x1="55" y1="20" x2="55" y2="28" stroke="#555" strokeWidth="2" strokeLinecap="round"/>
                          <line x1="65" y1="20" x2="65" y2="28" stroke="#555" strokeWidth="2" strokeLinecap="round"/>
                          <rect x="30" y="40" width="15" height="10" fill="#E2B17F" rx="1"/>
                          <rect x="30" y="55" width="20" height="10" fill="#E2B17F" rx="1"/>
                          <path d="M75 25 L85 15 L88 18 L78 28 Z" fill="#F05A50"/>
                          <path d="M75 25 L73 28 L78 28 Z" fill="#FFCDB2"/>
                        </svg>
                      </div>
                      <strong>No discounts found</strong>
                      <p>Adjust your filter selection to view your discounts</p>
                    </div>
                  </td>
                </tr>
              ) : (
                filteredPromotions.map((promo) => (
                  <tr key={promo.id}>
                    <td className="th-checkbox">
                      <input type="checkbox" />
                    </td>
                    <td>
                      <div className="action-dropdown">
                        <button 
                          className="action-btn"
                          onClick={() => toggleDropdown(promo.id)}
                        >
                          •••
                        </button>
                        {activeDropdown === promo.id && (
                          <div className="dropdown-menu">
                            <button onClick={() => handleEdit(promo)}>
                              Edit
                            </button>
                            <button onClick={() => handleActivateDeactivate(promo)}>
                              {promo.isActive ? 'Deactivate' : 'Activate'}
                            </button>
                            <button 
                              onClick={() => handleDelete(promo)}
                              className="delete-btn"
                            >
                              Delete
                            </button>
                          </div>
                        )}
                      </div>
                    </td>
                    <td>
                      <span className={`status-badge ${promo.isActive ? 'active' : 'inactive'}`}>
                        {promo.isActive ? 'Active' : 'Inactive'}
                      </span>
                    </td>
                    <td className="name-cell">{promo.name}</td>
                    <td>{getDiscountType(promo)}</td>
                    <td>{getDiscountDisplay(promo)}</td>
                    <td>{formatDate(promo.startDate)}</td>
                    <td>{formatDate(promo.endDate)}</td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </section>
    </div>
  );
};

export default PromotionsOverview;
