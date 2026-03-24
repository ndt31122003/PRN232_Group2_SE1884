import React, { Suspense, lazy } from "react";
import { Navigate, createBrowserRouter, RouterProvider } from "react-router-dom";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import Register from "../ANONYMOUS/Register/Register";
import ListingForm from "../Listing/Form/ListingForm";
import MainLayout from "../Layout/MainLayout";
import OrdersPage from "../Orders/OrdersLayout";
import VariationForm from "../Listing/Form/VariationForm";
import PrivateRoute from "./PrivateRoute";
import ListingsPage from "../Listing/ListingsLayout";

// Lazy load các trang
const LoginPage = lazy(() => import("../ANONYMOUS/Login/LoginPage"));
const NotFoundPage = lazy(() => import("../NOTFOUND/NotFound"));
const AuthCallbackPage = lazy(() => import("../ANONYMOUS/Login/AuthCallBackPage"));
const AccountSettingsPage = lazy(() => import("../Account/AccountSettings"));
const CreateSellerCouponPage = lazy(() => import("../Coupons/CreateSellerCoupon"));
const CouponsSummaryPage = lazy(() => import("../Coupons/CouponsSummary"));
const SaleEventsSummaryPage = lazy(() => import("../Marketing/SaleEvents/SaleEventsSummary"));
const CreateSaleEventPage = lazy(() => import("../Marketing/SaleEvents/CreateSaleEvent"));
const OverviewPage = lazy(() => import("../Overview/OverviewPage"));
const ResearchLayout = lazy(() => import("../Research/ResearchLayout"));
const ProductResearchPage = lazy(() => import("../Research/ProductResearchPage"));
const SourcingInsightsPage = lazy(() => import("../Research/SourcingInsightsPage"));
const PaymentsLayout = lazy(() => import("../Payments/PaymentsLayout"));
const SummaryPage = lazy(() => import("../Payments/SummaryPage"));
const AllTransactionsPage = lazy(() => import("../Payments/AllTransactionsPage"));
const PayoutsPage = lazy(() => import("../Payments/PayoutsPage"));
const ReportsPage = lazy(() => import("../Payments/ReportsPage"));
const FeedbackPage = lazy(() => import("../Feedback/FeedbackPage"));
const MyDisputes = lazy(() => import("../Disputes/MyDisputes"));
const MySupportTickets = lazy(() => import("../SupportTickets/MySupportTickets"));
const CreateSupportTicket = lazy(() => import("../SupportTickets/CreateSupportTicket"));
const SellerReportsLayout = lazy(() => import("../Reports/ReportLayout"));
const ReportsDownloadsPage = lazy(() => import("../Reports/DownloadsPage"));
const ReportsUploadsPage = lazy(() => import("../Reports/UploadsPage"));
const ReportsSchedulePage = lazy(() => import("../Reports/SchedulePage"));

// Các trang con của Orders
const AllOrdersPage = lazy(() => import("../Orders/AllOrdersPage"));
const ShipOrderPage = lazy(() => import("../Orders/ShipOrderPage"));
const CancelOrderPage = lazy(() => import("../Orders/CancelOrderPage"));
const BulkOrdersShipPage = lazy(() => import("../Orders/BulkShipOrdersPage"));
const BulkOrdersFeedbackPage = lazy(() => import("../Orders/BulkFeedbackPage"));
const OrderDetailPage = lazy(() => import("../Orders/OrderDetailPage"));
// const AwaitingPaymentPage = lazy(() => import("../Orders/AwaitingPaymentPage"));
// const AwaitingShipmentPage = lazy(() => import("../Orders/AwaitingShipmentPage"));
// const PaidAndShippedPage = lazy(() => import("../Orders/PaidAndShippedPage"));
// const ArchivedPage = lazy(() => import("../Orders/ArchivedPage"));
const CancellationsPage = lazy(() => import("../Orders/CancellationsPage"));
const ReturnsPage = lazy(() => import("../Orders/ReturnsPage"));
const CancellationRequestActionPage = lazy(() => import("../Orders/CancellationRequestActionPage"));
const ReturnRequestActionPage = lazy(() => import("../Orders/ReturnRequestActionPage"));
// const RequestsPage = lazy(() => import("../Orders/RequestsPage"));
const ShippingLabelsPage = lazy(() => import("../Orders/ShippingLabelsPage"));
// const ShippingPreferencesPage = lazy(() => import("../Orders/ShippingPreferencesPage"));
// const AutomateFeedbackPage = lazy(() => import("../Orders/AutomateFeedbackPage"));
// const ReturnPreferencesPage = lazy(() => import("../Orders/ReturnPreferencesPage"));

//Listings pages
const AllListingsPage = lazy(() => import("../Listing/AllListingsPage"));
const ListingTemplatesPage = lazy(() => import("../Listing/ListingTemplates/ListingTemplatesPage"));
const SellingPreferencesPage = lazy(() => import("../Listing/SellingPreferences/SellingPreferencesPage"));
const BuyerManagementPage = lazy(() => import("../Listing/SellingPreferences/BuyerManagementPage"));
const BlockedBuyerListPage = lazy(() => import("../Listing/SellingPreferences/BlockedBuyerListPage"));

// Performance pages
const PerformanceLayout = lazy(() => import("../Performance/PerformanceLayout"));
const PerformanceSummaryPage = lazy(() => import("../Performance/PerformanceSummaryPage"));
const PerformanceSalesPage = lazy(() => import("../Performance/PerformanceSalesPage"));
const PerformanceTrafficPage = lazy(() => import("../Performance/PerformanceTrafficPage"));
const PerformanceSellerLevelPage = lazy(() => import("../Performance/PerformanceSellerLevelPage"));
const PerformanceServiceMetricsPage = lazy(() => import("../Performance/PerformanceServiceMetricsPage"));

// Store pages
const MyStoresPage = lazy(() => import("../Store/MyStoresPage"));
const StoreLayout = lazy(() => import("../Store/StoreLayout"));
const CreateStorePage = lazy(() => import("../Store/CreateStorePage"));
const StoreSettingsPage = lazy(() => import("../Store/StoreSettingsPage"));
const PoliciesPage = lazy(() => import("../Store/PoliciesPage"));
const SubscriptionPage = lazy(() => import("../Store/SubscriptionPage"));

// thêm lazy import cho các trang dispute
const CreateDisputePage = lazy(() => import("../Disputes/CreateDispute"));
const DisputeDetailPage = lazy(() => import("../Disputes/DisputeDetail"));
const DisputeActionPage = lazy(() => import("../Disputes/DisputeAction"));

// 🔹 Khởi tạo router
const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <PrivateRoute>
        <Suspense fallback={<LoadingScreen />}>
          <MainLayout />
        </Suspense>
      </PrivateRoute>
    ),
    children: [
      {
        index: true,
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <OverviewPage />
          </Suspense>
        )
      },
      {
        path: "overview",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <OverviewPage />
          </Suspense>
        )
      },
      // ================================
      // Orders Module
      // ================================
      {
        path: "order",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <OrdersPage />
          </Suspense>
        ),
        children: [
          { index: true, element: <Navigate to="all?status=all" replace /> },
          // order history: chuyển tới danh sách đơn hàng với status=history
          { path: "history", element: <Navigate to="all?status=history" replace /> },
          { path: "all", element: <AllOrdersPage /> },
          { path: "detail/:orderId", element: <OrderDetailPage /> },
          // { path: "awaiting-payment", element: <AwaitingPaymentPage /> },
          // { path: "awaiting-shipment", element: <AwaitingShipmentPage /> },
          // { path: "paid-shipped", element: <PaidAndShippedPage /> },
          // { path: "archived", element: <ArchivedPage /> },
          { path: "cancellations", element: <CancellationsPage /> },
          { path: "cancellations/:requestId/:mode", element: <CancellationRequestActionPage /> },
          { path: "returns", element: <ReturnsPage /> },
          { path: "returns/:requestId/:mode", element: <ReturnRequestActionPage /> },
          // { path: "requests", element: <RequestsPage /> },
          { path: "shipping-labels", element: <ShippingLabelsPage /> },
          // { path: "shipping-preferences", element: <ShippingPreferencesPage /> },
          // { path: "automate-feedback", element: <AutomateFeedbackPage /> },
          // { path: "return-preferences", element: <ReturnPreferencesPage /> },
        ],
      },
      {
        path: "/order/ship/:orderId",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <ShipOrderPage />
          </Suspense>
        ),
      },
      {
        path: "/order/bulk-ship",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <BulkOrdersShipPage />
          </Suspense>
        ),
      },
      {
        path: "/order/bulk-feedback",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <BulkOrdersFeedbackPage />
          </Suspense>
        ),
      },
      {
        path: "/order/cancel/:orderId",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <CancelOrderPage />
          </Suspense>
        ),
      },
      {
        path: "listings",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <ListingsPage />
          </Suspense>
        ),
        children: [
          { index: true, element: <Navigate to="active" replace /> },
          {
            path: "selling-preferences",
            element: (
              <Suspense fallback={<LoadingScreen isOverlay={true} />}>
                <SellingPreferencesPage />
              </Suspense>
            ),
          },
          {
            path: "selling-preferences/buyer-management",
            element: (
              <Suspense fallback={<LoadingScreen isOverlay={true} />}>
                <BuyerManagementPage />
              </Suspense>
            ),
          },
          {
            path: "selling-preferences/blocked-buyers",
            element: (
              <Suspense fallback={<LoadingScreen isOverlay={true} />}>
                <BlockedBuyerListPage />
              </Suspense>
            ),
          },
          { path: "listing-templates", element: <ListingTemplatesPage /> },
          { path: ":statusSlug", element: <AllListingsPage /> },
        ],
      },
      {
        path: "feedback",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <FeedbackPage />
          </Suspense>
        ),
      },
      {
        path: "disputes",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <MyDisputes />
          </Suspense>
        ),
        children: [
          // index: list
          { index: true, element: <MyDisputes /> },
          // create new dispute
          {
            path: "create",
            element: (
              <Suspense fallback={<LoadingScreen isOverlay={true} />}>
                <CreateDisputePage />
              </Suspense>
            ),
          },
          // dispute detail
          {
            path: "detail/:disputeId",
            element: (
              <Suspense fallback={<LoadingScreen isOverlay={true} />}>
                <DisputeDetailPage />
              </Suspense>
            ),
          },
          // action on dispute (respond/escalate ...)
          {
            path: "action/:disputeId/:mode",
            element: (
              <Suspense fallback={<LoadingScreen isOverlay={true} />}>
                <DisputeActionPage />
              </Suspense>
            ),
          },
        ],
      },
      {
        path: "support-tickets",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <MySupportTickets />
          </Suspense>
        ),
      },
      {
        path: "support-tickets/create",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <CreateSupportTicket />
          </Suspense>
        ),
      },
      {
        path: "research",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <ResearchLayout />
          </Suspense>
        ),
        children: [
          { index: true, element: <Navigate to="product" replace /> },
          { path: "product", element: <ProductResearchPage /> },
          { path: "sourcing", element: <SourcingInsightsPage /> },
        ],
      },
      {
        path: "performance",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <PerformanceLayout />
          </Suspense>
        ),
        children: [
          { index: true, element: <Navigate to="summary" replace /> },
          { path: "summary", element: <PerformanceSummaryPage /> },
          { path: "seller-level", element: <PerformanceSellerLevelPage /> },
          { path: "sales", element: <PerformanceSalesPage /> },
          { path: "traffic", element: <PerformanceTrafficPage /> },
          { path: "service-metrics", element: <PerformanceServiceMetricsPage /> },
        ],
      },
      {
        path: "payments",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <PaymentsLayout />
          </Suspense>
        ),
        children: [
          { index: true, element: <Navigate to="summary" replace /> },
          { path: "summary", element: <SummaryPage /> },
          { path: "transactions", element: <AllTransactionsPage /> },
          { path: "payouts", element: <PayoutsPage /> },
          { path: "reports", element: <ReportsPage /> },
        ],
      },
      {
        path: "reports",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <SellerReportsLayout />
          </Suspense>
        ),
        children: [
          { index: true, element: <Navigate to="downloads" replace /> },
          { path: "uploads", element: <ReportsUploadsPage /> },
          { path: "downloads", element: <ReportsDownloadsPage /> },
          { path: "schedule", element: <ReportsSchedulePage /> },
        ],
      },
      {
        path: "account/settings",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <AccountSettingsPage />
          </Suspense>
        ),
      },
      {
        path: "marketing/coupons/create",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <CreateSellerCouponPage />
          </Suspense>
        ),
      },
      {
        path: "marketing/coupons",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <CouponsSummaryPage />
          </Suspense>
        ),
      },
      {
        path: "marketing/sale-events",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <SaleEventsSummaryPage />
          </Suspense>
        ),
      },
      {
        path: "marketing/sale-events/create",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <CreateSaleEventPage />
          </Suspense>
        ),
      },
      {
        path: "marketing",
        element: <Navigate to="/marketing/sale-events" replace />
      },
      
      // ================================
      // Store Module
      // ================================
      {
        path: "stores",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <MyStoresPage />
          </Suspense>
        ),
      },
      {
        path: "store",
        element: (
          <Suspense fallback={<LoadingScreen isOverlay={true} />}>
            <StoreLayout />
          </Suspense>
        ),
        children: [
          { index: true, element: <Navigate to="settings" replace /> },
          { path: "settings", element: <StoreSettingsPage /> },
          { path: "policies", element: <PoliciesPage /> },
          { path: "subscription", element: <SubscriptionPage /> },
        ],
      },
    ],
  },

  {

    path: "store/create",
    element: (
      <PrivateRoute>
        <Suspense fallback={<LoadingScreen />}>
          <CreateStorePage />
        </Suspense>
      </PrivateRoute>
    ),
  },
  {
    path: "listing-form",
    element: (
      <PrivateRoute>
        <Suspense fallback={<LoadingScreen />}>
          <ListingForm />
        </Suspense>
      </PrivateRoute>
    ),
  },
  {
    path: "listing-form/:listingId?",
    element: (
      <PrivateRoute>
        <Suspense fallback={<LoadingScreen />}>
          <ListingForm />
        </Suspense>
      </PrivateRoute>
    ),
  },
  // Authentication routes
  {
    path: "login",
    element: (
      <Suspense fallback={<LoadingScreen />}>
        <LoginPage />
      </Suspense>
    ),
  },
  {
    path: "auth/callback",
    element: (
      <Suspense fallback={<LoadingScreen />}>
        <AuthCallbackPage />
      </Suspense>
    ),
  },
  {
    path: "register",
    element: (
      <Suspense fallback={<LoadingScreen />}>
        <Register />
      </Suspense>
    ),
  },

  {
    path: "orders",
    element: <Navigate to="/order/all?status=all" replace />,
  },
  // alias cho order history (nếu UI bật link tới /orders/history)
  {
    path: "orders/history",
    element: <Navigate to="/order/all?status=history" replace />,
  },
  // thêm alias ngắn gọn nếu có link /order-history
  {
    path: "order-history",
    element: <Navigate to="/order/all?status=history" replace />,
  },
  {
    path: "orders/:rest/*",
    element: <Navigate to="/order/all?status=all" replace />,
  },

  {
    path: "*",
    element: (
      <Suspense fallback={<LoadingScreen />}>
        <NotFoundPage />
      </Suspense>
    ),
  },
  {
    path: "variation-form",
    element: (
      <PrivateRoute>
        <Suspense fallback={<LoadingScreen />}>
          <VariationForm />
        </Suspense>
      </PrivateRoute>
    ),
  }
]);

const AppRouter = () => {
  return (
    <Suspense fallback={<LoadingScreen />}>
      <RouterProvider router={router} />
    </Suspense>
  );
};

export default AppRouter;
