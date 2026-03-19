--
-- PostgreSQL database dump
--

\restrict JfJKwA3P3WTO8dpbe4neCGaPS4FQGFqf8Q09xkAEhFGa4KcLXMzAS8vJtcLc8xC

-- Dumped from database version 17.6
-- Dumped by pg_dump version 18.1

-- Started on 2026-03-18 15:31:31

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 70 (class 2615 OID 2200)
-- Name: public; Type: SCHEMA; Schema: -; Owner: pg_database_owner
--

CREATE SCHEMA public;


ALTER SCHEMA public OWNER TO pg_database_owner;

--
-- TOC entry 4363 (class 0 OID 0)
-- Dependencies: 70
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 365 (class 1259 OID 32407)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 366 (class 1259 OID 32975)
-- Name: category; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.category (
    id uuid NOT NULL,
    name text NOT NULL,
    description text NOT NULL,
    parent_id uuid,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.category OWNER TO postgres;

--
-- TOC entry 378 (class 1259 OID 33069)
-- Name: category_condition; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.category_condition (
    category_id uuid NOT NULL,
    condition_id uuid NOT NULL
);


ALTER TABLE public.category_condition OWNER TO postgres;

--
-- TOC entry 377 (class 1259 OID 33057)
-- Name: category_specific; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.category_specific (
    id uuid NOT NULL,
    name text NOT NULL,
    is_required boolean NOT NULL,
    allow_multiple boolean NOT NULL,
    "values" jsonb NOT NULL,
    category_id uuid
);


ALTER TABLE public.category_specific OWNER TO postgres;

--
-- TOC entry 367 (class 1259 OID 32987)
-- Name: condition; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.condition (
    id uuid NOT NULL,
    name text NOT NULL,
    description text NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.condition OWNER TO postgres;

--
-- TOC entry 404 (class 1259 OID 33409)
-- Name: coupon; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coupon (
    id uuid NOT NULL,
    coupon_type_id uuid NOT NULL,
    category_id uuid,
    seller_id uuid,
    name character varying(100) NOT NULL,
    code character varying(50) NOT NULL,
    discount_value numeric(10,2) NOT NULL,
    discount_unit character varying(10) NOT NULL,
    max_discount numeric(10,2),
    start_date timestamp with time zone NOT NULL,
    end_date timestamp with time zone NOT NULL,
    usage_limit integer,
    usage_per_user integer,
    minimum_order_value numeric(10,2),
    applicable_price_min numeric(10,2),
    applicable_price_max numeric(10,2),
    is_active boolean NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean DEFAULT false NOT NULL
);


ALTER TABLE public.coupon OWNER TO postgres;

--
-- TOC entry 405 (class 1259 OID 33422)
-- Name: coupon_condition; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coupon_condition (
    id uuid NOT NULL,
    coupon_id uuid NOT NULL,
    buy_quantity integer,
    get_quantity integer,
    get_discount_percent numeric(5,2),
    save_every_amount numeric(10,2),
    save_every_items integer,
    condition_description character varying(255)
);


ALTER TABLE public.coupon_condition OWNER TO postgres;

--
-- TOC entry 415 (class 1259 OID 33572)
-- Name: coupon_excluded_categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coupon_excluded_categories (
    id uuid NOT NULL,
    coupon_id uuid NOT NULL,
    category_id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.coupon_excluded_categories OWNER TO postgres;

--
-- TOC entry 416 (class 1259 OID 33584)
-- Name: coupon_excluded_items; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coupon_excluded_items (
    id uuid NOT NULL,
    coupon_id uuid NOT NULL,
    item_id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.coupon_excluded_items OWNER TO postgres;

--
-- TOC entry 417 (class 1259 OID 33596)
-- Name: coupon_target_audiences; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coupon_target_audiences (
    id uuid NOT NULL,
    coupon_id uuid NOT NULL,
    user_type integer NOT NULL,
    location_id uuid,
    min_account_age_days integer,
    min_total_spent numeric,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.coupon_target_audiences OWNER TO postgres;

--
-- TOC entry 403 (class 1259 OID 33401)
-- Name: coupon_type; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.coupon_type (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    description character varying(255),
    is_active boolean NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean DEFAULT false NOT NULL
);


ALTER TABLE public.coupon_type OWNER TO postgres;

--
-- TOC entry 413 (class 1259 OID 33536)
-- Name: dispute; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dispute (
    id uuid NOT NULL,
    listing_id uuid NOT NULL,
    raised_by_id character varying(100) NOT NULL,
    reason character varying(2000) NOT NULL,
    status character varying(50) NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.dispute OWNER TO postgres;

--
-- TOC entry 368 (class 1259 OID 32994)
-- Name: file_metadata; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.file_metadata (
    id uuid NOT NULL,
    linked_entity_id uuid,
    url text NOT NULL,
    file_name text NOT NULL,
    content_type text NOT NULL,
    size bigint NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.file_metadata OWNER TO postgres;

--
-- TOC entry 369 (class 1259 OID 33001)
-- Name: listing; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.listing (
    id uuid NOT NULL,
    format integer NOT NULL,
    status integer NOT NULL,
    title text NOT NULL,
    sku text NOT NULL,
    listing_description text NOT NULL,
    category_id uuid NOT NULL,
    condition_id uuid,
    condition_description text NOT NULL,
    scheduled_start_time timestamp with time zone,
    draft_expired_at timestamp with time zone,
    start_date timestamp with time zone,
    end_date timestamp with time zone,
    duration integer NOT NULL,
    listing_format integer NOT NULL,
    pricing_start_price numeric,
    pricing_reserve_price numeric,
    pricing_buy_it_now_price numeric,
    pricing_quantity integer,
    type integer,
    pricing_price numeric,
    offer_settings_allow_offers boolean,
    offer_settings_minimum_offer numeric,
    offer_settings_auto_accept_offer numeric,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.listing OWNER TO postgres;

--
-- TOC entry 387 (class 1259 OID 33152)
-- Name: listing_id; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.listing_id (
    listing_id uuid NOT NULL,
    seller_id uuid NOT NULL
);


ALTER TABLE public.listing_id OWNER TO postgres;

--
-- TOC entry 380 (class 1259 OID 33085)
-- Name: listing_image; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.listing_image (
    listing_id uuid NOT NULL,
    id integer NOT NULL,
    url text NOT NULL,
    is_primary boolean NOT NULL
);


ALTER TABLE public.listing_image OWNER TO postgres;

--
-- TOC entry 379 (class 1259 OID 33084)
-- Name: listing_image_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.listing_image ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.listing_image_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 382 (class 1259 OID 33098)
-- Name: listing_item_specific; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.listing_item_specific (
    listing_id uuid NOT NULL,
    id integer NOT NULL,
    name character varying(100) NOT NULL,
    val text NOT NULL
);


ALTER TABLE public.listing_item_specific OWNER TO postgres;

--
-- TOC entry 381 (class 1259 OID 33097)
-- Name: listing_item_specific_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.listing_item_specific ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.listing_item_specific_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 370 (class 1259 OID 33008)
-- Name: listing_template; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.listing_template (
    id uuid NOT NULL,
    name character varying(150) NOT NULL,
    description character varying(500),
    payload_json jsonb NOT NULL,
    format_label character varying(50),
    thumbnail_url character varying(500),
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.listing_template OWNER TO postgres;

--
-- TOC entry 406 (class 1259 OID 33435)
-- Name: order_buyer_feedback; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_buyer_feedback (
    id uuid NOT NULL,
    order_id uuid NOT NULL,
    seller_id uuid NOT NULL,
    buyer_id uuid NOT NULL,
    comment character varying(80) NOT NULL,
    uses_stored_comment boolean NOT NULL,
    stored_comment_key character varying(100),
    created_at timestamp with time zone NOT NULL,
    follow_up_comment character varying(80),
    follow_up_commented_at timestamp with time zone
);


ALTER TABLE public.order_buyer_feedback OWNER TO postgres;

--
-- TOC entry 398 (class 1259 OID 33331)
-- Name: order_cancellation_requests; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_cancellation_requests (
    id uuid NOT NULL,
    order_id uuid NOT NULL,
    buyer_id uuid NOT NULL,
    seller_id uuid NOT NULL,
    initiated_by integer NOT NULL,
    reason integer NOT NULL,
    buyer_note character varying(1000),
    seller_note character varying(1000),
    requested_at timestamp with time zone NOT NULL,
    seller_response_deadline_utc timestamp with time zone,
    seller_responded_at timestamp with time zone,
    auto_closed_at timestamp with time zone,
    completed_at timestamp with time zone,
    status integer NOT NULL,
    refund_amount numeric(18,2),
    refund_currency character varying(3),
    order_total_amount numeric(18,2) NOT NULL,
    order_total_currency character varying(3) NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.order_cancellation_requests OWNER TO postgres;

--
-- TOC entry 402 (class 1259 OID 33382)
-- Name: order_item_shipments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_item_shipments (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    order_id uuid NOT NULL,
    order_item_id uuid NOT NULL,
    shipping_label_id uuid,
    tracking_number character varying(120) NOT NULL,
    carrier character varying(120) NOT NULL,
    shipped_at timestamp with time zone NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.order_item_shipments OWNER TO postgres;

--
-- TOC entry 391 (class 1259 OID 33206)
-- Name: order_items; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_items (
    id uuid NOT NULL,
    listing_id uuid NOT NULL,
    variation_id uuid,
    title character varying(255) NOT NULL,
    image_url character varying(500) NOT NULL,
    sku character varying(100) NOT NULL,
    quantity integer NOT NULL,
    unit_price_amount numeric NOT NULL,
    unit_price_currency character varying(3) NOT NULL,
    total_price_amount numeric NOT NULL,
    total_price_currency character varying(3) NOT NULL,
    order_id uuid NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL,
    category_id uuid
);


ALTER TABLE public.order_items OWNER TO postgres;

--
-- TOC entry 399 (class 1259 OID 33343)
-- Name: order_return_requests; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_return_requests (
    id uuid NOT NULL,
    order_id uuid NOT NULL,
    buyer_id uuid NOT NULL,
    seller_id uuid NOT NULL,
    reason integer NOT NULL,
    preferred_resolution integer NOT NULL,
    buyer_note character varying(1500),
    seller_note character varying(1500),
    requested_at timestamp with time zone NOT NULL,
    seller_responded_at timestamp with time zone,
    buyer_return_due_at timestamp with time zone,
    buyer_shipped_at timestamp with time zone,
    delivered_at timestamp with time zone,
    refund_issued_at timestamp with time zone,
    closed_at timestamp with time zone,
    return_carrier character varying(120),
    tracking_number character varying(120),
    status integer NOT NULL,
    refund_amount numeric(18,2),
    refund_currency character varying(3),
    restocking_fee_amount numeric(18,2),
    restocking_fee_currency character varying(3),
    order_total_amount numeric(18,2) NOT NULL,
    order_total_currency character varying(3) NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.order_return_requests OWNER TO postgres;

--
-- TOC entry 392 (class 1259 OID 33218)
-- Name: order_shipping_labels; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_shipping_labels (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    order_id uuid NOT NULL,
    shipping_service_id uuid NOT NULL,
    carrier character varying(100) NOT NULL,
    service_code character varying(100) NOT NULL,
    service_name character varying(150) NOT NULL,
    tracking_number character varying(100) NOT NULL,
    label_url character varying(1024) NOT NULL,
    label_file_name character varying(260) NOT NULL,
    cost_amount numeric(18,2) NOT NULL,
    cost_currency character varying(3) NOT NULL,
    insurance_amount numeric(18,2) NOT NULL,
    insurance_currency character varying(3) NOT NULL,
    package_type character varying(80) NOT NULL,
    weight_oz numeric(18,2) NOT NULL,
    length_in numeric(18,2) NOT NULL,
    width_in numeric(18,2) NOT NULL,
    height_in numeric(18,2) NOT NULL,
    purchased_at timestamp with time zone NOT NULL,
    estimated_delivery timestamp with time zone,
    label_document_id character varying(100) NOT NULL,
    is_voided boolean DEFAULT false NOT NULL,
    void_reason character varying(250),
    voided_at timestamp with time zone
);


ALTER TABLE public.order_shipping_labels OWNER TO postgres;

--
-- TOC entry 393 (class 1259 OID 33230)
-- Name: order_status_histories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_status_histories (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    order_id uuid NOT NULL,
    from_status_id uuid NOT NULL,
    to_status_id uuid NOT NULL,
    changed_at timestamp with time zone NOT NULL
);


ALTER TABLE public.order_status_histories OWNER TO postgres;

--
-- TOC entry 385 (class 1259 OID 33123)
-- Name: order_status_transitions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_status_transitions (
    id uuid NOT NULL,
    from_status_id uuid NOT NULL,
    to_status_id uuid NOT NULL,
    allowed_roles text NOT NULL
);


ALTER TABLE public.order_status_transitions OWNER TO postgres;

--
-- TOC entry 371 (class 1259 OID 33015)
-- Name: order_statuses; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_statuses (
    id uuid NOT NULL,
    code character varying(50) NOT NULL,
    name character varying(100) NOT NULL,
    description character varying(500) NOT NULL,
    color character varying(20) NOT NULL,
    sort_order integer NOT NULL
);


ALTER TABLE public.order_statuses OWNER TO postgres;

--
-- TOC entry 388 (class 1259 OID 33162)
-- Name: orders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orders (
    id uuid NOT NULL,
    order_number character varying(50) NOT NULL,
    buyer_id uuid NOT NULL,
    seller_id uuid NOT NULL,
    sub_total_amount numeric NOT NULL,
    sub_total_currency character varying(3) NOT NULL,
    shipping_cost_amount numeric NOT NULL,
    shipping_cost_currency character varying(3) NOT NULL,
    platform_fee_amount numeric NOT NULL,
    platform_fee_currency character varying(3) NOT NULL,
    tax_amount numeric NOT NULL,
    tax_currency character varying(3) NOT NULL,
    discount_amount numeric NOT NULL,
    discount_currency character varying(3) NOT NULL,
    total_amount numeric NOT NULL,
    total_currency character varying(3) NOT NULL,
    status_id uuid NOT NULL,
    shipping_status integer NOT NULL,
    fulfillment_type integer NOT NULL,
    ordered_at timestamp with time zone NOT NULL,
    paid_at timestamp with time zone,
    shipped_at timestamp with time zone,
    delivered_at timestamp with time zone,
    archived_at timestamp with time zone,
    cancelled_at timestamp with time zone,
    coupon_code text,
    promotion_id uuid,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.orders OWNER TO postgres;

--
-- TOC entry 372 (class 1259 OID 33022)
-- Name: otp; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.otp (
    id uuid NOT NULL,
    email text NOT NULL,
    code character varying(10) NOT NULL,
    expires_on_utc timestamp with time zone NOT NULL,
    is_used boolean NOT NULL,
    type integer NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.otp OWNER TO postgres;

--
-- TOC entry 373 (class 1259 OID 33029)
-- Name: outbox_message; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.outbox_message (
    id uuid NOT NULL,
    type text NOT NULL,
    content text NOT NULL,
    occurred_on timestamp with time zone NOT NULL,
    processed_on timestamp with time zone,
    retry_count integer NOT NULL,
    error text
);


ALTER TABLE public.outbox_message OWNER TO postgres;

--
-- TOC entry 389 (class 1259 OID 33179)
-- Name: refresh_token; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.refresh_token (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    token character varying(200) NOT NULL,
    expires_on_utc timestamp with time zone NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.refresh_token OWNER TO postgres;

--
-- TOC entry 400 (class 1259 OID 33359)
-- Name: report_downloads; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.report_downloads (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    reference_code character varying(32) NOT NULL,
    source character varying(64) NOT NULL,
    type character varying(128) NOT NULL,
    requested_at_utc timestamp with time zone NOT NULL,
    completed_at_utc timestamp with time zone,
    status character varying(32) NOT NULL,
    file_url character varying(512),
    range_start_utc timestamp with time zone,
    range_end_utc timestamp with time zone,
    range_timezone character varying(64),
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.report_downloads OWNER TO postgres;

--
-- TOC entry 401 (class 1259 OID 33366)
-- Name: report_schedules; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.report_schedules (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    source character varying(64) NOT NULL,
    type character varying(128) NOT NULL,
    frequency character varying(32) NOT NULL,
    created_at_utc timestamp with time zone NOT NULL,
    last_run_at_utc timestamp with time zone,
    next_run_at_utc timestamp with time zone,
    end_date_utc timestamp with time zone,
    is_active boolean DEFAULT true NOT NULL,
    delivery_email character varying(256),
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.report_schedules OWNER TO postgres;

--
-- TOC entry 420 (class 1259 OID 34865)
-- Name: research_saved_category; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.research_saved_category (
    user_id uuid NOT NULL,
    category_id text NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.research_saved_category OWNER TO postgres;

--
-- TOC entry 394 (class 1259 OID 33291)
-- Name: return_policy; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.return_policy (
    id uuid NOT NULL,
    store_id uuid NOT NULL,
    accept_returns boolean NOT NULL,
    return_period_days integer,
    refund_method integer,
    return_shipping_paid_by integer,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.return_policy OWNER TO postgres;

--
-- TOC entry 414 (class 1259 OID 33548)
-- Name: review; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.review (
    id uuid NOT NULL,
    listing_id uuid NOT NULL,
    reviewer_id character varying(100) NOT NULL,
    reviewer_role character varying(20) DEFAULT 'Buyer'::character varying NOT NULL,
    recipient_id character varying(100) NOT NULL,
    recipient_role character varying(20) DEFAULT 'Seller'::character varying NOT NULL,
    rating integer NOT NULL,
    comment character varying(2000),
    reply character varying(2000),
    replied_at timestamp with time zone,
    revision_status character varying(50) DEFAULT 'None'::character varying NOT NULL,
    revision_requested_at timestamp with time zone,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.review OWNER TO postgres;

--
-- TOC entry 374 (class 1259 OID 33036)
-- Name: role; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.role (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    description text NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.role OWNER TO postgres;

--
-- TOC entry 386 (class 1259 OID 33140)
-- Name: role_permissions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.role_permissions (
    "Permission" text NOT NULL,
    role_id uuid NOT NULL
);


ALTER TABLE public.role_permissions OWNER TO postgres;

--
-- TOC entry 390 (class 1259 OID 33191)
-- Name: role_user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.role_user (
    roles_id uuid NOT NULL,
    user_id uuid NOT NULL
);


ALTER TABLE public.role_user OWNER TO postgres;

--
-- TOC entry 407 (class 1259 OID 33452)
-- Name: sale_event; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sale_event (
    id uuid NOT NULL,
    seller_id uuid NOT NULL,
    name character varying(90) NOT NULL,
    description character varying(255),
    mode character varying(50) NOT NULL,
    status character varying(50) NOT NULL,
    start_date timestamp with time zone NOT NULL,
    end_date timestamp with time zone NOT NULL,
    offer_free_shipping boolean NOT NULL,
    include_skipped_items boolean NOT NULL,
    block_price_increase_revisions boolean NOT NULL,
    highlight_percentage numeric(5,2),
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.sale_event OWNER TO postgres;

--
-- TOC entry 408 (class 1259 OID 33459)
-- Name: sale_event_discount_tier; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sale_event_discount_tier (
    id uuid NOT NULL,
    sale_event_id uuid NOT NULL,
    discount_type character varying(20) NOT NULL,
    discount_value numeric(10,2) NOT NULL,
    priority integer NOT NULL,
    label character varying(100)
);


ALTER TABLE public.sale_event_discount_tier OWNER TO postgres;

--
-- TOC entry 409 (class 1259 OID 33469)
-- Name: sale_event_listing; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sale_event_listing (
    id uuid NOT NULL,
    sale_event_id uuid NOT NULL,
    discount_tier_id uuid NOT NULL,
    listing_id uuid NOT NULL
);


ALTER TABLE public.sale_event_listing OWNER TO postgres;

--
-- TOC entry 411 (class 1259 OID 33509)
-- Name: seller_blocked_buyer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.seller_blocked_buyer (
    id uuid NOT NULL,
    identifier character varying(200) NOT NULL,
    normalized_identifier character varying(200) NOT NULL,
    created_at_utc timestamp with time zone DEFAULT (now() AT TIME ZONE 'utc'::text) NOT NULL,
    seller_preference_id uuid NOT NULL
);


ALTER TABLE public.seller_blocked_buyer OWNER TO postgres;

--
-- TOC entry 412 (class 1259 OID 33520)
-- Name: seller_exempt_buyer; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.seller_exempt_buyer (
    id uuid NOT NULL,
    identifier character varying(200) NOT NULL,
    normalized_identifier character varying(200) NOT NULL,
    created_at_utc timestamp with time zone DEFAULT (now() AT TIME ZONE 'utc'::text) NOT NULL,
    seller_preference_id uuid NOT NULL
);


ALTER TABLE public.seller_exempt_buyer OWNER TO postgres;

--
-- TOC entry 410 (class 1259 OID 33487)
-- Name: seller_preference; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.seller_preference (
    id uuid NOT NULL,
    seller_id uuid NOT NULL,
    listings_stay_active_when_out_of_stock boolean DEFAULT false NOT NULL,
    show_exact_quantity_available boolean DEFAULT true NOT NULL,
    buyers_can_see_vat_number boolean DEFAULT false NOT NULL,
    vat_number character varying(200),
    block_unpaid_item_strikes boolean DEFAULT false NOT NULL,
    unpaid_item_strikes_count integer DEFAULT 0 NOT NULL,
    unpaid_item_strikes_period_months integer DEFAULT 0 NOT NULL,
    block_primary_address_outside_shipping_location boolean DEFAULT true NOT NULL,
    block_max_items_last_ten_days boolean DEFAULT false NOT NULL,
    max_items_last_ten_days integer,
    apply_feedback_score_threshold boolean DEFAULT false NOT NULL,
    feedback_score_threshold integer,
    update_block_settings_active_listings boolean DEFAULT false NOT NULL,
    require_payment_method_before_bid boolean DEFAULT true NOT NULL,
    require_payment_method_before_offer boolean DEFAULT true NOT NULL,
    prevent_blocked_buyers_contacting boolean DEFAULT true NOT NULL,
    invoice_format integer NOT NULL,
    invoice_send_email_copy boolean DEFAULT true NOT NULL,
    invoice_apply_credits_automatically boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.seller_preference OWNER TO postgres;

--
-- TOC entry 395 (class 1259 OID 33298)
-- Name: shipping_policy; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.shipping_policy (
    id uuid NOT NULL,
    store_id uuid NOT NULL,
    cost_amount numeric(18,2) NOT NULL,
    cost_currency character varying(3) NOT NULL,
    handling_time_days integer NOT NULL,
    is_default boolean NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL,
    carrier character varying(100) DEFAULT ''::character varying NOT NULL,
    service_name character varying(100) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE public.shipping_policy OWNER TO postgres;

--
-- TOC entry 375 (class 1259 OID 33043)
-- Name: shipping_services; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.shipping_services (
    id uuid NOT NULL,
    carrier character varying(100) NOT NULL,
    service_name character varying(100) NOT NULL,
    base_cost_amount numeric NOT NULL,
    base_cost_currency character varying(3) NOT NULL,
    coverage_description character varying(150) DEFAULT ''::character varying NOT NULL,
    created_at timestamp with time zone DEFAULT '-infinity'::timestamp with time zone NOT NULL,
    created_by text,
    delivery_window_label character varying(80) DEFAULT ''::character varying NOT NULL,
    is_deleted boolean DEFAULT false NOT NULL,
    max_estimated_delivery_days integer DEFAULT 0 NOT NULL,
    min_estimated_delivery_days integer DEFAULT 0 NOT NULL,
    notes character varying(256) DEFAULT ''::character varying NOT NULL,
    printer_required boolean DEFAULT false NOT NULL,
    savings_description character varying(150) DEFAULT ''::character varying NOT NULL,
    service_code character varying(120) DEFAULT ''::character varying NOT NULL,
    slug character varying(60) DEFAULT ''::character varying NOT NULL,
    supports_qr_code boolean DEFAULT false NOT NULL,
    updated_at timestamp with time zone,
    updated_by text
);


ALTER TABLE public.shipping_services OWNER TO postgres;

--
-- TOC entry 396 (class 1259 OID 33305)
-- Name: store; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.store (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    name character varying(255) NOT NULL,
    slug character varying(255) NOT NULL,
    description text,
    logo_url character varying(500),
    banner_url character varying(500),
    store_type integer NOT NULL,
    is_active boolean NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL,
    contact_email character varying(255),
    contact_phone character varying(50),
    layout_config text,
    social_links text,
    theme_color character varying(50)
);


ALTER TABLE public.store OWNER TO postgres;

--
-- TOC entry 397 (class 1259 OID 33312)
-- Name: store_subscription; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.store_subscription (
    id uuid NOT NULL,
    store_id uuid NOT NULL,
    subscription_type integer NOT NULL,
    monthly_fee numeric(18,2) NOT NULL,
    monthly_fee_currency character varying(3) NOT NULL,
    final_value_fee_percentage numeric(5,2) NOT NULL,
    listing_limit integer NOT NULL,
    start_date timestamp with time zone NOT NULL,
    end_date timestamp with time zone,
    status integer NOT NULL
);


ALTER TABLE public.store_subscription OWNER TO postgres;

--
-- TOC entry 376 (class 1259 OID 33050)
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    id uuid NOT NULL,
    username text NOT NULL,
    full_name text NOT NULL,
    email text NOT NULL,
    password_hash text NOT NULL,
    is_email_verified boolean NOT NULL,
    is_payment_verified boolean NOT NULL,
    performance_level character varying(32) NOT NULL,
    active_total_value numeric(18,2) NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL,
    business_city character varying(100),
    business_country character varying(100),
    business_name character varying(200),
    business_state character varying(100),
    business_street character varying(300),
    business_zip_code character varying(20),
    is_business_verified boolean DEFAULT false NOT NULL,
    is_phone_verified boolean DEFAULT false NOT NULL,
    phone_number character varying(20)
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- TOC entry 384 (class 1259 OID 33111)
-- Name: variation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.variation (
    id integer NOT NULL,
    sku text NOT NULL,
    price numeric NOT NULL,
    quantity integer NOT NULL,
    specifics jsonb NOT NULL,
    images jsonb NOT NULL,
    listing_id uuid NOT NULL
);


ALTER TABLE public.variation OWNER TO postgres;

--
-- TOC entry 383 (class 1259 OID 33110)
-- Name: variation_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public.variation ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.variation_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 419 (class 1259 OID 33615)
-- Name: voucher_transactions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.voucher_transactions (
    id uuid NOT NULL,
    voucher_id uuid NOT NULL,
    order_id uuid NOT NULL,
    amount_used numeric NOT NULL,
    transaction_type integer NOT NULL,
    transaction_date timestamp with time zone NOT NULL,
    notes text,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.voucher_transactions OWNER TO postgres;

--
-- TOC entry 418 (class 1259 OID 33608)
-- Name: vouchers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.vouchers (
    id uuid NOT NULL,
    seller_id uuid,
    code text NOT NULL,
    initial_value numeric NOT NULL,
    current_balance numeric NOT NULL,
    currency text NOT NULL,
    issue_date timestamp with time zone NOT NULL,
    expiry_date timestamp with time zone,
    assigned_user_id uuid,
    is_transferable boolean NOT NULL,
    is_active boolean NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.vouchers OWNER TO postgres;

--
-- TOC entry 4302 (class 0 OID 32407)
-- Dependencies: 365
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" VALUES ('20251030032529_Add', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251030173809_changeColNameOfAllowedRoles', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251031130833_AddResearchIndexes', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251031173245_updateIdStatusHistory', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251031181927_addShippingServiceSeed', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251101073256_AddStoreManagement', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251101075315_UpdateShippingPolicyStructure', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251101173452_addCancellationsAndReturnRequest', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251101175728_addCancellationsAndReturnRequest3', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251101192218_AddReportsModule', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251102111951_UpdateOrderSeeds', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251105122228_AddOrderItemShipments', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251105173454_AddSellerCoupons', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251107182651_AddBuyerFeedback', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251111172411_AddSaleEventsModule', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251112135441_SellerPreferenceIndexFix', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251112135954_SellerPreferenceIndexFix1', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20251113202832_AddOrderSeed2s', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260306132214_AddSellerVerificationFields', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260307061213_AddVoucherAndCouponEnhancement', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260307062047_AddCategoryIdToOrderItem', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260307062735_AddVoucherTables', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260307105552_AddStoreExtensibility', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260308190334_AddStoreExtensibility2', '9.0.8');


--
-- TOC entry 4303 (class 0 OID 32975)
-- Dependencies: 366
-- Data for Name: category; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000001', 'Electronics', 'Consumer electronics, components, and accessories.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('20000000-0000-0000-0000-000000000001', 'Fashion', 'Apparel, shoes, and accessories.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('30000000-0000-0000-0000-000000000001', 'Home & Garden', 'Home improvement, décor, and outdoor living.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000002', 'Cell Phones & Smartphones', 'Smartphones and cell phone devices.', '10000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000003', 'Laptops & Netbooks', 'Portable computers and accessories.', '10000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000004', 'Cameras & Photo', 'Digital cameras and photography equipment.', '10000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('20000000-0000-0000-0000-000000000002', 'Men''s Athletic Shoes', 'Performance and casual athletic footwear for men.', '20000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('20000000-0000-0000-0000-000000000003', 'Women''s Dresses', 'Dresses for every style and occasion.', '20000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('30000000-0000-0000-0000-000000000002', 'Small Kitchen Appliances', 'Countertop appliances and kitchen helpers.', '30000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('30000000-0000-0000-0000-000000000003', 'Furniture', 'Indoor and outdoor furniture collections.', '30000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000005', 'TV, Video & Home Audio', 'Televisions, speakers, and streaming devices.', '10000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000006', 'Video Game Consoles', 'Home and handheld gaming systems.', '10000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000007', 'Wearable Technology', 'Smartwatches, fitness trackers, and smart eyewear.', '10000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000008', 'Smart Home', 'Connected home devices and automation hubs.', '10000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('10000000-0000-0000-0000-000000000009', 'Vehicle Electronics & GPS', 'Navigation, dash cams, and in-car entertainment.', '10000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('20000000-0000-0000-0000-000000000004', 'Men''s Clothing', 'Casual, business, and formal apparel for men.', '20000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('20000000-0000-0000-0000-000000000005', 'Women''s Handbags & Bags', 'Designer totes, crossbody bags, and backpacks.', '20000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('20000000-0000-0000-0000-000000000006', 'Women''s Shoes', 'Heels, flats, and casual footwear.', '20000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('20000000-0000-0000-0000-000000000007', 'Watches', 'Timepieces ranging from vintage to luxury.', '20000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('20000000-0000-0000-0000-000000000008', 'Fine Jewelry', 'Rings, necklaces, and bracelets crafted in precious metals.', '20000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('30000000-0000-0000-0000-000000000004', 'Home Décor', 'Interior accents, wall art, and lighting.', '30000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('30000000-0000-0000-0000-000000000005', 'Tools & Workshop Equipment', 'Power tools and shop essentials.', '30000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('30000000-0000-0000-0000-000000000006', 'Yard, Garden & Outdoor Living', 'Outdoor décor, landscaping, and patio gear.', '30000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('30000000-0000-0000-0000-000000000007', 'Home Improvement', 'Building supplies, fixtures, and hardware.', '30000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('50000000-0000-0000-0000-000000000001', 'eBay Motors', 'Complete automotive marketplace for vehicles and parts.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('60000000-0000-0000-0000-000000000001', 'Collectibles & Art', 'Treasures from pop culture, history, and fine art.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('70000000-0000-0000-0000-000000000001', 'Toys & Hobbies', 'Playsets, model kits, and collector favorites.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('80000000-0000-0000-0000-000000000001', 'Sporting Goods', 'Gear for every sport, indoors and out.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('90000000-0000-0000-0000-000000000001', 'Health & Beauty', 'Wellness essentials and personal care favorites.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('a0000000-0000-0000-0000-000000000001', 'Business & Industrial', 'Equipment, supplies, and services for every trade.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('b0000000-0000-0000-0000-000000000001', 'Musical Instruments & Gear', 'Instruments, pro audio, and stage equipment.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('c0000000-0000-0000-0000-000000000001', 'Pet Supplies', 'Care essentials for pets large and small.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('d0000000-0000-0000-0000-000000000001', 'Baby Essentials', 'Nursery gear, travel systems, and feeding must-haves.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('e0000000-0000-0000-0000-000000000001', 'Crafts', 'DIY staples spanning every creative discipline.', NULL, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('50000000-0000-0000-0000-000000000002', 'Car Parts & Accessories', 'OEM and aftermarket components for every ride.', '50000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('50000000-0000-0000-0000-000000000003', 'Motorcycle Parts', 'Upgrades and replacement parts for bikes.', '50000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('50000000-0000-0000-0000-000000000004', 'Automotive Tools & Supplies', 'Garage lifts, diagnostics, and specialty tools.', '50000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('50000000-0000-0000-0000-000000000005', 'Wheels, Tires & Parts', 'Rims, tire sets, TPMS sensors, and more.', '50000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('60000000-0000-0000-0000-000000000002', 'Collectible Card Games', 'TCG singles, sealed product, and memorabilia.', '60000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('60000000-0000-0000-0000-000000000003', 'Comics & Graphic Novels', 'Golden Age through modern runs and collectibles.', '60000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('60000000-0000-0000-0000-000000000004', 'Art Prints', 'Limited editions, lithographs, and posters.', '60000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('60000000-0000-0000-0000-000000000005', 'Coins & Paper Money', 'Graded coins, bullion, and currency.', '60000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('70000000-0000-0000-0000-000000000002', 'Action Figures', 'Superheroes, anime, and pop-culture icons.', '70000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('70000000-0000-0000-0000-000000000003', 'Model Railroads & Trains', 'Locomotives, rolling stock, and scenery kits.', '70000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('70000000-0000-0000-0000-000000000004', 'RC Model Vehicles & Kits', 'Radio-controlled cars, drones, and planes.', '70000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('70000000-0000-0000-0000-000000000005', 'Dolls & Bears', 'Barbie, Blythe, Build-A-Bear, and more.', '70000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('70000000-0000-0000-0000-000000000006', 'LEGO & Building Toys', 'Modular builds and sealed collectible sets.', '70000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('80000000-0000-0000-0000-000000000002', 'Outdoor Sports', 'Camping, hiking, hunting, and fishing gear.', '80000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('80000000-0000-0000-0000-000000000003', 'Fitness, Running & Yoga', 'Exercise machines, apparel, and accessories.', '80000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('80000000-0000-0000-0000-000000000004', 'Cycling', 'Bikes, parts, helmets, and apparel.', '80000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('80000000-0000-0000-0000-000000000005', 'Golf', 'Clubs, balls, carts, and training aids.', '80000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('90000000-0000-0000-0000-000000000002', 'Makeup', 'Cosmetics, palettes, and tools.', '90000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('90000000-0000-0000-0000-000000000003', 'Skin Care', 'Serums, moisturizers, and devices.', '90000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('90000000-0000-0000-0000-000000000004', 'Vitamins & Dietary Supplements', 'Wellness, immunity, and performance blends.', '90000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('90000000-0000-0000-0000-000000000005', 'Hair Care', 'Styling tools, treatments, and color.', '90000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('90000000-0000-0000-0000-000000000006', 'Fragrances', 'Perfumes, colognes, and body mists.', '90000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('a0000000-0000-0000-0000-000000000002', 'Heavy Equipment', 'Excavators, loaders, and industrial vehicles.', 'a0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('a0000000-0000-0000-0000-000000000003', 'MRO & Industrial Supplies', 'Maintenance, repair, and operations essentials.', 'a0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('a0000000-0000-0000-0000-000000000004', 'Retail & Services', 'Point-of-sale, signage, and consulting packages.', 'a0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('a0000000-0000-0000-0000-000000000005', 'Office Equipment', 'Printers, copiers, and office machines.', 'a0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('b0000000-0000-0000-0000-000000000002', 'Guitars & Basses', 'Electric, acoustic, and bass guitars.', 'b0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('b0000000-0000-0000-0000-000000000003', 'Pro Audio Equipment', 'Mixers, microphones, and studio gear.', 'b0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('b0000000-0000-0000-0000-000000000004', 'DJ Equipment', 'Controllers, turntables, and lighting.', 'b0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('b0000000-0000-0000-0000-000000000005', 'Brass & Woodwind', 'Saxes, trumpets, clarinets, and accessories.', 'b0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('c0000000-0000-0000-0000-000000000002', 'Dog Supplies', 'Beds, crates, and training essentials.', 'c0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('c0000000-0000-0000-0000-000000000003', 'Cat Supplies', 'Litter, scratchers, and cat furniture.', 'c0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('c0000000-0000-0000-0000-000000000004', 'Fish & Aquarium', 'Aquariums, filtration, and décor.', 'c0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('c0000000-0000-0000-0000-000000000005', 'Small Animal Supplies', 'Habitat accessories for hamsters, rabbits, and more.', 'c0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('d0000000-0000-0000-0000-000000000002', 'Strollers & Travel Systems', 'Lightweight, jogging, and convertible options.', 'd0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('d0000000-0000-0000-0000-000000000003', 'Nursery Furniture', 'Cribs, dressers, and gliders.', 'd0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('d0000000-0000-0000-0000-000000000004', 'Baby Safety', 'Monitors, gates, and proofing essentials.', 'd0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('d0000000-0000-0000-0000-000000000005', 'Baby Feeding', 'Bottles, warmers, and nursing support.', 'd0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('e0000000-0000-0000-0000-000000000002', 'Scrapbooking & Paper Crafting', 'Stamps, dies, and embellishments.', 'e0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('e0000000-0000-0000-0000-000000000003', 'Art Supplies', 'Paints, canvases, and studio tools.', 'e0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('e0000000-0000-0000-0000-000000000004', 'Fabric', 'Yardage, quilting, and upholstery textiles.', 'e0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.category VALUES ('e0000000-0000-0000-0000-000000000005', 'Beads & Jewelry Making', 'Findings, gemstones, and tools.', 'e0000000-0000-0000-0000-000000000001', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);


--
-- TOC entry 4315 (class 0 OID 33069)
-- Dependencies: 378
-- Data for Name: category_condition; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000002');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000003');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000005');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000002');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000003');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000005');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000005');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000008');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000008');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000005');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000009', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000009', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('10000000-0000-0000-0000-000000000009', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000008');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000007');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000008');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000007');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000008');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000007');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000008');
INSERT INTO public.category_condition VALUES ('20000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('30000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000002');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000003');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000005');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000005');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000005');
INSERT INTO public.category_condition VALUES ('50000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('60000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000005');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000009');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('70000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('80000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('90000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('90000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('90000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('90000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('90000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('90000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('90000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('90000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('a0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('b0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('c0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('d0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('e0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('e0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('e0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('e0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
INSERT INTO public.category_condition VALUES ('e0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
INSERT INTO public.category_condition VALUES ('e0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('e0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
INSERT INTO public.category_condition VALUES ('e0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');


--
-- TOC entry 4314 (class 0 OID 33057)
-- Dependencies: 377
-- Data for Name: category_specific; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.category_specific VALUES ('10100000-0000-0000-0000-000000000001', 'Brand', true, false, '["Apple", "Samsung", "Google", "OnePlus", "Motorola"]', '10000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('10100000-0000-0000-0000-000000000002', 'Model', false, false, '["iPhone 15", "Galaxy S24", "Pixel 8", "OnePlus 12", "Moto G Power"]', '10000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('10100000-0000-0000-0000-000000000003', 'Storage Capacity', true, false, '["64 GB", "128 GB", "256 GB", "512 GB", "1 TB"]', '10000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('10100000-0000-0000-0000-000000000004', 'Color', false, false, '["Black", "White", "Blue", "Red", "Purple"]', '10000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('10100000-0000-0000-0000-000000000005', 'Network', false, false, '["Unlocked", "AT&T", "Verizon", "T-Mobile", "US Cellular"]', '10000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('10200000-0000-0000-0000-000000000001', 'Brand', true, false, '["Apple", "Dell", "HP", "Lenovo", "ASUS"]', '10000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('10200000-0000-0000-0000-000000000002', 'Processor', true, false, '["Intel Core i5", "Intel Core i7", "AMD Ryzen 5", "AMD Ryzen 7", "Apple M2"]', '10000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('10200000-0000-0000-0000-000000000003', 'RAM Size', true, false, '["8 GB", "16 GB", "32 GB", "64 GB"]', '10000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('10200000-0000-0000-0000-000000000004', 'Storage Type', false, false, '["SSD", "HDD", "Hybrid"]', '10000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('10200000-0000-0000-0000-000000000005', 'Screen Size', false, false, '["13 in", "14 in", "15.6 in", "17 in"]', '10000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('10300000-0000-0000-0000-000000000001', 'Brand', true, false, '["Canon", "Nikon", "Sony", "Fujifilm", "Panasonic"]', '10000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('10300000-0000-0000-0000-000000000002', 'Camera Type', true, false, '["DSLR", "Mirrorless", "Point & Shoot", "Action"]', '10000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('10300000-0000-0000-0000-000000000003', 'Maximum Resolution', false, false, '["12 MP", "16 MP", "24 MP", "32 MP"]', '10000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('10300000-0000-0000-0000-000000000004', 'Optical Zoom', false, false, '["None", "5x", "10x", "20x"]', '10000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('10300000-0000-0000-0000-000000000005', 'Series', false, false, '["EOS", "Alpha", "X-T", "Lumix"]', '10000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('20100000-0000-0000-0000-000000000001', 'Brand', true, false, '["Nike", "Adidas", "New Balance", "Puma", "Under Armour"]', '20000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('20100000-0000-0000-0000-000000000002', 'US Shoe Size', true, false, '["7", "8", "9", "10", "11", "12", "13"]', '20000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('20100000-0000-0000-0000-000000000003', 'Color', false, false, '["Black", "White", "Red", "Blue", "Gray"]', '20000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('20100000-0000-0000-0000-000000000004', 'Style', false, false, '["Low Top", "Mid Top", "High Top"]', '20000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('20100000-0000-0000-0000-000000000005', 'Width', false, false, '["Standard", "Wide", "Extra Wide"]', '20000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('20200000-0000-0000-0000-000000000001', 'Brand', true, false, '["ASOS", "Free People", "H&M", "Reformation", "Zara"]', '20000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('20200000-0000-0000-0000-000000000002', 'Size Type', true, false, '["Regular", "Petite", "Plus", "Tall"]', '20000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('20200000-0000-0000-0000-000000000003', 'Dress Length', false, false, '["Mini", "Knee Length", "Midi", "Maxi"]', '20000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('20200000-0000-0000-0000-000000000004', 'Material', false, false, '["Cotton", "Linen", "Polyester", "Silk"]', '20000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('20200000-0000-0000-0000-000000000005', 'Pattern', false, false, '["Solid", "Floral", "Striped", "Polka Dot", "Animal Print"]', '20000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('30100000-0000-0000-0000-000000000001', 'Brand', true, false, '["Breville", "Cuisinart", "Instant Pot", "KitchenAid", "Ninja"]', '30000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('30100000-0000-0000-0000-000000000002', 'Appliance Type', true, false, '["Air Fryer", "Blender", "Coffee Maker", "Mixer", "Pressure Cooker"]', '30000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('30100000-0000-0000-0000-000000000003', 'Color', false, false, '["Black", "Red", "Silver", "Stainless Steel", "White"]', '30000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('30100000-0000-0000-0000-000000000004', 'Power Source', false, false, '["Electric", "Battery", "Manual"]', '30000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('30100000-0000-0000-0000-000000000005', 'Capacity', false, false, '["2 qt", "4 qt", "6 qt", "8 qt"]', '30000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('30200000-0000-0000-0000-000000000001', 'Brand', true, false, '["Ashley", "Crate & Barrel", "IKEA", "West Elm", "Wayfair"]', '30000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('30200000-0000-0000-0000-000000000002', 'Room', true, false, '["Bedroom", "Dining Room", "Home Office", "Living Room", "Patio"]', '30000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('30200000-0000-0000-0000-000000000003', 'Material', false, false, '["Fabric", "Glass", "Leather", "Metal", "Wood"]', '30000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('30200000-0000-0000-0000-000000000004', 'Color', false, false, '["Black", "Gray", "Natural", "White", "Walnut"]', '30000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('30200000-0000-0000-0000-000000000005', 'Assembly Required', false, false, '["No", "Yes"]', '30000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('10400000-0000-0000-0000-000000000001', 'Brand', true, false, '["LG", "Samsung", "Sony", "TCL", "Vizio"]', '10000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('10400000-0000-0000-0000-000000000002', 'Type', true, false, '["4K UHD TV", "Soundbar", "AV Receiver", "Streaming Device"]', '10000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('10400000-0000-0000-0000-000000000003', 'Smart Platform', false, false, '["Google TV", "Roku", "Fire TV", "webOS", "Tizen"]', '10000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('10500000-0000-0000-0000-000000000001', 'Platform', true, false, '["Nintendo", "PlayStation", "Xbox", "Steam Deck"]', '10000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('10500000-0000-0000-0000-000000000002', 'Model', true, false, '["PlayStation 5", "Xbox Series X", "Nintendo Switch OLED", "Steam Deck OLED"]', '10000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('10500000-0000-0000-0000-000000000003', 'Storage Capacity', false, false, '["64 GB", "512 GB", "1 TB", "2 TB"]', '10000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('10600000-0000-0000-0000-000000000001', 'Brand', true, false, '["Apple", "Fitbit", "Garmin", "Samsung", "Withings"]', '10000000-0000-0000-0000-000000000007');
INSERT INTO public.category_specific VALUES ('10600000-0000-0000-0000-000000000002', 'Features', false, true, '["GPS", "Heart Rate Monitor", "NFC", "SpO2", "Sleep Tracking"]', '10000000-0000-0000-0000-000000000007');
INSERT INTO public.category_specific VALUES ('10700000-0000-0000-0000-000000000001', 'Device Type', true, false, '["Smart Speaker", "Smart Display", "Smart Lighting", "Smart Thermostat", "Security Camera"]', '10000000-0000-0000-0000-000000000008');
INSERT INTO public.category_specific VALUES ('10700000-0000-0000-0000-000000000002', 'Ecosystem', false, false, '["Alexa", "Apple Home", "Google Home", "Matter", "SmartThings"]', '10000000-0000-0000-0000-000000000008');
INSERT INTO public.category_specific VALUES ('10800000-0000-0000-0000-000000000001', 'Product Type', true, false, '["Dash Cam", "GPS", "Car Stereo", "Backup Camera", "Radar Detector"]', '10000000-0000-0000-0000-000000000009');
INSERT INTO public.category_specific VALUES ('10800000-0000-0000-0000-000000000002', 'Compatible Vehicle', false, true, '["Universal", "Ford", "GM", "Toyota", "Volkswagen"]', '10000000-0000-0000-0000-000000000009');
INSERT INTO public.category_specific VALUES ('20300000-0000-0000-0000-000000000001', 'Brand', true, false, '["Banana Republic", "Hugo Boss", "Levi''s", "Nike", "Ralph Lauren"]', '20000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('20300000-0000-0000-0000-000000000002', 'Size', true, false, '["S", "M", "L", "XL", "XXL"]', '20000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('20400000-0000-0000-0000-000000000001', 'Brand', true, false, '["Coach", "Gucci", "Kate Spade", "Louis Vuitton", "Tory Burch"]', '20000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('20400000-0000-0000-0000-000000000002', 'Materials', false, true, '["Canvas", "Leather", "Nylon", "Patent Leather", "Vegan Leather"]', '20000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('20400000-0000-0000-0000-000000000003', 'Style', false, false, '["Backpack", "Crossbody", "Satchel", "Shoulder Bag", "Tote"]', '20000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('20500000-0000-0000-0000-000000000001', 'Brand', true, false, '["Birkenstock", "Clarks", "Dr. Martens", "Sam Edelman", "Steve Madden"]', '20000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('20500000-0000-0000-0000-000000000002', 'US Shoe Size', true, false, '["5", "6", "7", "8", "9", "10"]', '20000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('20500000-0000-0000-0000-000000000003', 'Style', false, false, '["Boots", "Flats", "Heels", "Sandals", "Sneakers"]', '20000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('20600000-0000-0000-0000-000000000001', 'Brand', true, false, '["Casio", "Citizen", "Omega", "Rolex", "Seiko"]', '20000000-0000-0000-0000-000000000007');
INSERT INTO public.category_specific VALUES ('20600000-0000-0000-0000-000000000002', 'Movement', false, false, '["Automatic", "Quartz", "Mechanical", "Solar"]', '20000000-0000-0000-0000-000000000007');
INSERT INTO public.category_specific VALUES ('20700000-0000-0000-0000-000000000001', 'Metal', true, false, '["Gold", "Platinum", "Rose Gold", "Sterling Silver", "White Gold"]', '20000000-0000-0000-0000-000000000008');
INSERT INTO public.category_specific VALUES ('20700000-0000-0000-0000-000000000002', 'Jewelry Type', true, false, '["Bracelet", "Earrings", "Necklace", "Ring"]', '20000000-0000-0000-0000-000000000008');
INSERT INTO public.category_specific VALUES ('30100000-0000-0000-0000-000000000006', 'Style', false, false, '["Bohemian", "Farmhouse", "Mid-Century", "Modern", "Traditional"]', '30000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('30100000-0000-0000-0000-000000000007', 'Room', true, false, '["Bedroom", "Dining Room", "Kitchen", "Living Room", "Office"]', '30000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('30300000-0000-0000-0000-000000000001', 'Brand', false, false, '["Bosch", "DeWalt", "Hilti", "Makita", "Milwaukee"]', '30000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('30300000-0000-0000-0000-000000000002', 'Power Source', true, false, '["Battery", "Corded Electric", "Compressed Air", "Manual"]', '30000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('30400000-0000-0000-0000-000000000001', 'Category', true, false, '["Flooring", "Hardware", "Lighting", "Plumbing", "Storage"]', '30000000-0000-0000-0000-000000000007');
INSERT INTO public.category_specific VALUES ('30400000-0000-0000-0000-000000000002', 'Finish', false, false, '["Brushed Nickel", "Chrome", "Matte Black", "Oil-Rubbed Bronze"]', '30000000-0000-0000-0000-000000000007');
INSERT INTO public.category_specific VALUES ('40100000-0000-0000-0000-000000000001', 'Part Type', true, false, '["Brakes", "Engine", "Exterior", "Interior", "Suspension"]', '50000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('40100000-0000-0000-0000-000000000002', 'Brand', false, false, '["ACDelco", "Bosch", "Denso", "Mopar", "Motorcraft"]', '50000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('40100000-0000-0000-0000-000000000003', 'Compatible Make', false, true, '["Chevrolet", "Ford", "Honda", "Toyota", "Universal"]', '50000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('40200000-0000-0000-0000-000000000001', 'Part Type', true, false, '["Body & Frame", "Drivetrain", "Electrical", "Engine", "Suspension"]', '50000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('40300000-0000-0000-0000-000000000001', 'Tool Type', true, false, '["Diagnostic", "Hand Tool", "Lifts & Jacks", "Power Tool", "Specialty"]', '50000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('40400000-0000-0000-0000-000000000001', 'Tire Type', true, false, '["All-Season", "Performance", "Snow/Winter", "Off-Road"]', '50000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('40400000-0000-0000-0000-000000000002', 'Rim Diameter', false, false, '["16 in", "17 in", "18 in", "19 in", "20 in"]', '50000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('60100000-0000-0000-0000-000000000001', 'Franchise', true, false, '["Magic: The Gathering", "Pokémon", "Yu-Gi-Oh!", "Marvel Snap", "Disney Lorcana"]', '60000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('60100000-0000-0000-0000-000000000002', 'Card Condition', true, false, '["Gem Mint", "Near Mint", "Lightly Played", "Moderately Played", "Heavily Played"]', '60000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('60100000-0000-0000-0000-000000000003', 'Graded', false, false, '["BGS", "CGC", "PSA", "SGC", "Ungraded"]', '60000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('60200000-0000-0000-0000-000000000001', 'Publisher', true, false, '["DC", "Dark Horse", "IDW", "Image", "Marvel"]', '60000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('60200000-0000-0000-0000-000000000002', 'Era', false, false, '["Golden Age", "Silver Age", "Bronze Age", "Modern"]', '60000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('60300000-0000-0000-0000-000000000001', 'Artist', true, false, '["Andy Warhol", "Banksy", "Jean-Michel Basquiat", "Salvador Dalí", "Yoshitomo Nara"]', '60000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('60300000-0000-0000-0000-000000000002', 'Medium', false, false, '["Giclée", "Lithograph", "Screenprint", "Serigraph"]', '60000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('60400000-0000-0000-0000-000000000001', 'Certification', false, false, '["ANACS", "NGC", "PCGS", "PMG", "Uncertified"]', '60000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('60400000-0000-0000-0000-000000000002', 'Grade', false, false, '["MS 70", "MS 69", "MS 65", "AU 55", "XF 45"]', '60000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('70100000-0000-0000-0000-000000000001', 'Franchise', true, false, '["DC", "Dragon Ball", "Marvel", "Star Wars", "Transformers"]', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('70100000-0000-0000-0000-000000000002', 'Scale', false, false, '["1:6", "1:12", "1:18", "6 in", "12 in"]', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('70200000-0000-0000-0000-000000000001', 'Scale', true, false, '["HO", "N", "O", "G", "Z"]', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('70200000-0000-0000-0000-000000000002', 'Power Type', false, false, '["DC", "DCC", "Battery"]', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('70300000-0000-0000-0000-000000000001', 'Vehicle Type', true, false, '["Car", "Truck", "Boat", "Plane", "Drone"]', '70000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('70300000-0000-0000-0000-000000000002', 'Power', false, false, '["Electric", "Gas", "Nitro"]', '70000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('70400000-0000-0000-0000-000000000001', 'Type', true, false, '["Barbie", "Fashion Doll", "Teddy Bear", "Vintage Doll"]', '70000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('70500000-0000-0000-0000-000000000001', 'Theme', false, false, '["Architecture", "City", "Ideas", "Star Wars", "Technic"]', '70000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('80100000-0000-0000-0000-000000000001', 'Sport', true, false, '["Camping", "Climbing", "Fishing", "Hunting", "Water Sports"]', '80000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('80200000-0000-0000-0000-000000000001', 'Equipment Type', true, false, '["Cardio Machine", "Free Weights", "Resistance Bands", "Yoga Mat"]', '80000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('80300000-0000-0000-0000-000000000001', 'Bicycle Type', true, false, '["Mountain", "Road", "Hybrid", "Gravel", "Electric"]', '80000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('80300000-0000-0000-0000-000000000002', 'Frame Size', false, false, '["Small", "Medium", "Large", "X-Large"]', '80000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('80400000-0000-0000-0000-000000000001', 'Club Type', true, false, '["Driver", "Fairway Wood", "Hybrid", "Iron Set", "Putter"]', '80000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('80400000-0000-0000-0000-000000000002', 'Flex', false, false, '["Ladies", "Regular", "Stiff", "Extra Stiff"]', '80000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('90100000-0000-0000-0000-000000000001', 'Brand', true, false, '["Charlotte Tilbury", "Dior", "Fenty Beauty", "MAC", "Rare Beauty"]', '90000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('90100000-0000-0000-0000-000000000002', 'Product Type', true, false, '["Foundation", "Eyeshadow", "Lipstick", "Mascara", "Primer"]', '90000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('90100000-0000-0000-0000-000000000003', 'Shade', false, false, '["Fair", "Light", "Medium", "Tan", "Deep"]', '90000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('90200000-0000-0000-0000-000000000001', 'Brand', true, false, '["CeraVe", "Dermalogica", "Drunk Elephant", "La Roche-Posay", "Tatcha"]', '90000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('90200000-0000-0000-0000-000000000002', 'Skin Type', true, false, '["Dry", "Normal", "Oily", "Combination", "Sensitive"]', '90000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('90200000-0000-0000-0000-000000000003', 'Concern', false, true, '["Acne", "Anti-Aging", "Brightening", "Hydration", "Sun Protection"]', '90000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('90300000-0000-0000-0000-000000000001', 'Formulation', true, false, '["Capsule", "Gummy", "Powder", "Softgel", "Tablet"]', '90000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('90300000-0000-0000-0000-000000000002', 'Main Purpose', false, true, '["Energy", "General Wellness", "Immune Support", "Joint Health", "Sleep"]', '90000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('90400000-0000-0000-0000-000000000001', 'Brand', true, false, '["Amika", "Dyson", "GHD", "Olaplex", "Redken"]', '90000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('90400000-0000-0000-0000-000000000002', 'Hair Type', false, true, '["Coily", "Curly", "Straight", "Wavy", "Fine"]', '90000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('90500000-0000-0000-0000-000000000001', 'Fragrance Type', true, false, '["Eau de Parfum", "Eau de Toilette", "Eau de Cologne", "Perfume Oil"]', '90000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('90500000-0000-0000-0000-000000000002', 'Volume', false, false, '["30 ml", "50 ml", "75 ml", "100 ml"]', '90000000-0000-0000-0000-000000000006');
INSERT INTO public.category_specific VALUES ('a0100000-0000-0000-0000-000000000001', 'Equipment Type', true, false, '["Backhoe", "Bulldozer", "Excavator", "Forklift", "Skid Steer"]', 'a0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('a0100000-0000-0000-0000-000000000002', 'Hours', false, false, '["0-500", "501-1500", "1501-3000", "3001+"]', 'a0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('a0200000-0000-0000-0000-000000000001', 'Supply Type', true, false, '["Fasteners", "Hydraulics", "HVAC", "Pneumatics", "Safety"]', 'a0000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('a0300000-0000-0000-0000-000000000001', 'Service Type', true, false, '["Consulting", "Installation", "Maintenance", "Marketing", "Training"]', 'a0000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('a0400000-0000-0000-0000-000000000001', 'Equipment Type', true, false, '["3D Printer", "Laser Printer", "Multifunction", "Scanner", "Shredder"]', 'a0000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('b0100000-0000-0000-0000-000000000001', 'Brand', true, false, '["Fender", "Gibson", "Ibanez", "PRS", "Taylor"]', 'b0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('b0100000-0000-0000-0000-000000000002', 'Body Type', false, false, '["Solid", "Semi-Hollow", "Hollow"]', 'b0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('b0200000-0000-0000-0000-000000000001', 'Type', true, false, '["Mixer", "Microphone", "Monitor", "Interface", "Outboard Gear"]', 'b0000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('b0300000-0000-0000-0000-000000000001', 'Type', true, false, '["Controller", "Mixer", "Turntable", "Lighting"]', 'b0000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('b0400000-0000-0000-0000-000000000001', 'Instrument', true, false, '["Clarinet", "Flute", "Saxophone", "Trombone", "Trumpet"]', 'b0000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('c0100000-0000-0000-0000-000000000001', 'Product Type', true, false, '["Apparel", "Crates", "Food", "Grooming", "Toys"]', 'c0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('c0100000-0000-0000-0000-000000000002', 'Size', false, false, '["XS", "S", "M", "L", "XL"]', 'c0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('c0200000-0000-0000-0000-000000000001', 'Product Type', true, false, '["Food", "Litter", "Scratchers", "Toys"]', 'c0000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('c0300000-0000-0000-0000-000000000001', 'Aquarium Type', true, false, '["Freshwater", "Marine", "Reef", "Brackish"]', 'c0000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('c0300000-0000-0000-0000-000000000002', 'Tank Capacity', false, false, '["1-10 gal", "11-30 gal", "31-55 gal", "56+ gal"]', 'c0000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('c0400000-0000-0000-0000-000000000001', 'Product Type', true, false, '["Bedding", "Cages", "Food", "Toys"]', 'c0000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('d0100000-0000-0000-0000-000000000001', 'Brand', true, false, '["Baby Jogger", "Bugaboo", "Chicco", "Graco", "UPPAbaby"]', 'd0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('d0100000-0000-0000-0000-000000000002', 'Stroller Type', true, false, '["Full-Size", "Jogging", "Travel System", "Umbrella"]', 'd0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('d0200000-0000-0000-0000-000000000001', 'Furniture Piece', true, false, '["Crib", "Changing Table", "Glider", "Dresser"]', 'd0000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('d0300000-0000-0000-0000-000000000001', 'Product Type', true, false, '["Baby Monitor", "Safety Gate", "Outlet Cover", "Cabinet Lock"]', 'd0000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('d0400000-0000-0000-0000-000000000001', 'Feeding Type', true, false, '["Bottle", "Breastfeeding", "Solid Food", "Toddler"]', 'd0000000-0000-0000-0000-000000000005');
INSERT INTO public.category_specific VALUES ('e0100000-0000-0000-0000-000000000001', 'Product Type', true, false, '["Adhesives", "Dies", "Paper", "Stamps"]', 'e0000000-0000-0000-0000-000000000002');
INSERT INTO public.category_specific VALUES ('e0200000-0000-0000-0000-000000000001', 'Medium', true, false, '["Acrylic", "Oil", "Watercolor", "Pastel"]', 'e0000000-0000-0000-0000-000000000003');
INSERT INTO public.category_specific VALUES ('e0300000-0000-0000-0000-000000000001', 'Fabric Type', true, false, '["Cotton", "Denim", "Fleece", "Linen", "Silk"]', 'e0000000-0000-0000-0000-000000000004');
INSERT INTO public.category_specific VALUES ('e0400000-0000-0000-0000-000000000001', 'Material', true, false, '["Glass", "Gemstone", "Metal", "Resin", "Wood"]', 'e0000000-0000-0000-0000-000000000005');


--
-- TOC entry 4304 (class 0 OID 32987)
-- Dependencies: 367
-- Data for Name: condition; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000001', 'New', 'Factory sealed, unused item in original packaging.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000002', 'Manufacturer Refurbished', 'Professionally restored to working order by the manufacturer or certified provider.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000003', 'Seller Refurbished', 'Restored to working order by a third-party seller.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000004', 'Used', 'Previously owned item that shows signs of use.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000005', 'For parts or not working', 'Item does not function as intended and is being sold for parts or repair.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000006', 'Open box', 'Item is unused but the original packaging has been opened.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000007', 'New with tags', 'Unused apparel item with original tags attached.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000008', 'New without tags', 'Unused apparel item missing the retail tags.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.condition VALUES ('40000000-0000-0000-0000-000000000009', 'Pre-owned', 'Previously worn item that remains in good condition.', '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);


--
-- TOC entry 4341 (class 0 OID 33409)
-- Dependencies: 404
-- Data for Name: coupon; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4342 (class 0 OID 33422)
-- Dependencies: 405
-- Data for Name: coupon_condition; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4352 (class 0 OID 33572)
-- Dependencies: 415
-- Data for Name: coupon_excluded_categories; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4353 (class 0 OID 33584)
-- Dependencies: 416
-- Data for Name: coupon_excluded_items; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4354 (class 0 OID 33596)
-- Dependencies: 417
-- Data for Name: coupon_target_audiences; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4340 (class 0 OID 33401)
-- Dependencies: 403
-- Data for Name: coupon_type; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.coupon_type VALUES ('0d0c32fe-349c-4857-b20a-2d3f8db91ed4', 'Extra % off Y or more items', 'Percentage discount when buying Y or more items', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('2c5a6a6a-fe7e-4813-a134-70572b5ab90a', 'Extra % off $ or more', 'Percentage discount when total order value reaches a minimum value', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('3b980145-62b6-4ae6-9cf8-7838bc7b84e0', 'Save $ for every X items', 'Save a fixed amount for every X items purchased', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('51f2ed38-06bb-496e-b5cb-7aa3057c21b7', 'Save $ for every $ spent', 'Save a fixed amount for every dollar spent', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('773f8d9b-eb8e-4ff4-a21e-4bb2fa5407f4', 'Extra $ off X or more items', 'Fixed amount discount when buying X or more items', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('7a5a0b7a-ed8f-4b91-a7c3-59e5363b76f3', 'Extra $ off each item', 'Fixed amount discount for each item purchased', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('7eaa19cf-6b36-4a1c-b7b5-a9abcb7eeff2', 'Buy X get Y at % off', 'Buy X items and get Y items at a percentage discount', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('990c28b3-753e-41b1-a798-965cf46b7dcd', 'Extra $ off', 'Fixed amount discount on all eligible items', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('9e1d4ea5-5b09-48be-be90-e2790f6ba537', 'Extra % off', 'Percentage discount on all eligible items', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('cfa2e0f1-b720-4590-a7d4-4ce0844f9671', 'Extra $ off $ or more', 'Fixed amount discount when order value reaches a minimum threshold', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);
INSERT INTO public.coupon_type VALUES ('ed9d5151-6f8c-4628-a5a9-4c24867e5673', 'Buy X get Y free', 'Buy X items and get Y items for free', true, '2024-01-01 00:00:00+00', NULL, NULL, NULL, false);


--
-- TOC entry 4350 (class 0 OID 33536)
-- Dependencies: 413
-- Data for Name: dispute; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4305 (class 0 OID 32994)
-- Dependencies: 368
-- Data for Name: file_metadata; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4306 (class 0 OID 33001)
-- Dependencies: 369
-- Data for Name: listing; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000001', 2, 3, 'Alice''s Item #1', 'DEMO-1-0001', 'Curated demo listing #1 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000002', 2, 3, 'Alice''s Item #2', 'DEMO-1-0002', 'Curated demo listing #2 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000003', 2, 3, 'Alice''s Item #3', 'DEMO-1-0003', 'Curated demo listing #3 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000004', 2, 3, 'Alice''s Item #4', 'DEMO-1-0004', 'Curated demo listing #4 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000005', 2, 3, 'Alice''s Item #5', 'DEMO-1-0005', 'Curated demo listing #5 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000006', 2, 3, 'Alice''s Item #6', 'DEMO-1-0006', 'Curated demo listing #6 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000007', 2, 3, 'Alice''s Item #7', 'DEMO-1-0007', 'Curated demo listing #7 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000008', 2, 3, 'Alice''s Item #8', 'DEMO-1-0008', 'Curated demo listing #8 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000009', 2, 3, 'Alice''s Item #9', 'DEMO-1-0009', 'Curated demo listing #9 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000a', 2, 3, 'Alice''s Item #10', 'DEMO-1-0010', 'Curated demo listing #10 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000b', 2, 3, 'Alice''s Item #11', 'DEMO-1-0011', 'Curated demo listing #11 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000c', 2, 3, 'Alice''s Item #12', 'DEMO-1-0012', 'Curated demo listing #12 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000d', 2, 3, 'Alice''s Item #13', 'DEMO-1-0013', 'Curated demo listing #13 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000e', 2, 3, 'Alice''s Item #14', 'DEMO-1-0014', 'Curated demo listing #14 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000f', 2, 3, 'Alice''s Item #15', 'DEMO-1-0015', 'Curated demo listing #15 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000010', 2, 3, 'Alice''s Item #16', 'DEMO-1-0016', 'Curated demo listing #16 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000011', 2, 3, 'Alice''s Item #17', 'DEMO-1-0017', 'Curated demo listing #17 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000012', 2, 3, 'Alice''s Item #18', 'DEMO-1-0018', 'Curated demo listing #18 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000013', 2, 3, 'Alice''s Item #19', 'DEMO-1-0019', 'Curated demo listing #19 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000014', 2, 3, 'Alice''s Item #20', 'DEMO-1-0020', 'Curated demo listing #20 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000015', 2, 3, 'Alice''s Item #21', 'DEMO-1-0021', 'Curated demo listing #21 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000016', 2, 3, 'Alice''s Item #22', 'DEMO-1-0022', 'Curated demo listing #22 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000017', 2, 3, 'Alice''s Item #23', 'DEMO-1-0023', 'Curated demo listing #23 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000018', 2, 3, 'Alice''s Item #24', 'DEMO-1-0024', 'Curated demo listing #24 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000019', 2, 3, 'Alice''s Item #25', 'DEMO-1-0025', 'Curated demo listing #25 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001a', 2, 3, 'Alice''s Item #26', 'DEMO-1-0026', 'Curated demo listing #26 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001b', 2, 3, 'Alice''s Item #27', 'DEMO-1-0027', 'Curated demo listing #27 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001c', 2, 3, 'Alice''s Item #28', 'DEMO-1-0028', 'Curated demo listing #28 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001d', 2, 3, 'Alice''s Item #29', 'DEMO-1-0029', 'Curated demo listing #29 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001e', 2, 3, 'Alice''s Item #30', 'DEMO-1-0030', 'Curated demo listing #30 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001f', 2, 3, 'Alice''s Item #31', 'DEMO-1-0031', 'Curated demo listing #31 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000020', 2, 3, 'Alice''s Item #32', 'DEMO-1-0032', 'Curated demo listing #32 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000021', 2, 3, 'Alice''s Item #33', 'DEMO-1-0033', 'Curated demo listing #33 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000022', 2, 3, 'Alice''s Item #34', 'DEMO-1-0034', 'Curated demo listing #34 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000023', 2, 3, 'Alice''s Item #35', 'DEMO-1-0035', 'Curated demo listing #35 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000024', 2, 3, 'Alice''s Item #36', 'DEMO-1-0036', 'Curated demo listing #36 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000025', 2, 3, 'Alice''s Item #37', 'DEMO-1-0037', 'Curated demo listing #37 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000026', 2, 3, 'Alice''s Item #38', 'DEMO-1-0038', 'Curated demo listing #38 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000027', 2, 3, 'Alice''s Item #39', 'DEMO-1-0039', 'Curated demo listing #39 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000028', 2, 3, 'Alice''s Item #40', 'DEMO-1-0040', 'Curated demo listing #40 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000029', 2, 3, 'Alice''s Item #41', 'DEMO-1-0041', 'Curated demo listing #41 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002a', 2, 3, 'Alice''s Item #42', 'DEMO-1-0042', 'Curated demo listing #42 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002b', 2, 3, 'Alice''s Item #43', 'DEMO-1-0043', 'Curated demo listing #43 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002c', 2, 3, 'Alice''s Item #44', 'DEMO-1-0044', 'Curated demo listing #44 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002d', 2, 3, 'Alice''s Item #45', 'DEMO-1-0045', 'Curated demo listing #45 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002e', 2, 3, 'Alice''s Item #46', 'DEMO-1-0046', 'Curated demo listing #46 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002f', 2, 3, 'Alice''s Item #47', 'DEMO-1-0047', 'Curated demo listing #47 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000030', 2, 3, 'Alice''s Item #48', 'DEMO-1-0048', 'Curated demo listing #48 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000031', 2, 3, 'Alice''s Item #49', 'DEMO-1-0049', 'Curated demo listing #49 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000032', 2, 3, 'Alice''s Item #50', 'DEMO-1-0050', 'Curated demo listing #50 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000033', 2, 3, 'Alice''s Item #51', 'DEMO-1-0051', 'Curated demo listing #51 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000034', 2, 3, 'Alice''s Item #52', 'DEMO-1-0052', 'Curated demo listing #52 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000035', 2, 3, 'Alice''s Item #53', 'DEMO-1-0053', 'Curated demo listing #53 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000036', 2, 3, 'Alice''s Item #54', 'DEMO-1-0054', 'Curated demo listing #54 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000037', 2, 3, 'Alice''s Item #55', 'DEMO-1-0055', 'Curated demo listing #55 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000038', 2, 3, 'Alice''s Item #56', 'DEMO-1-0056', 'Curated demo listing #56 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000039', 2, 3, 'Alice''s Item #57', 'DEMO-1-0057', 'Curated demo listing #57 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003a', 2, 3, 'Alice''s Item #58', 'DEMO-1-0058', 'Curated demo listing #58 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003b', 2, 3, 'Alice''s Item #59', 'DEMO-1-0059', 'Curated demo listing #59 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003c', 2, 3, 'Alice''s Item #60', 'DEMO-1-0060', 'Curated demo listing #60 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003d', 2, 3, 'Alice''s Item #61', 'DEMO-1-0061', 'Curated demo listing #61 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003e', 2, 3, 'Alice''s Item #62', 'DEMO-1-0062', 'Curated demo listing #62 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003f', 2, 3, 'Alice''s Item #63', 'DEMO-1-0063', 'Curated demo listing #63 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000040', 2, 3, 'Alice''s Item #64', 'DEMO-1-0064', 'Curated demo listing #64 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000041', 2, 3, 'Alice''s Item #65', 'DEMO-1-0065', 'Curated demo listing #65 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000042', 2, 3, 'Alice''s Item #66', 'DEMO-1-0066', 'Curated demo listing #66 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000043', 2, 3, 'Alice''s Item #67', 'DEMO-1-0067', 'Curated demo listing #67 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000044', 2, 3, 'Alice''s Item #68', 'DEMO-1-0068', 'Curated demo listing #68 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000045', 2, 3, 'Alice''s Item #69', 'DEMO-1-0069', 'Curated demo listing #69 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000046', 2, 3, 'Alice''s Item #70', 'DEMO-1-0070', 'Curated demo listing #70 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000047', 2, 3, 'Alice''s Item #71', 'DEMO-1-0071', 'Curated demo listing #71 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000048', 2, 3, 'Alice''s Item #72', 'DEMO-1-0072', 'Curated demo listing #72 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000049', 2, 3, 'Alice''s Item #73', 'DEMO-1-0073', 'Curated demo listing #73 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004a', 2, 3, 'Alice''s Item #74', 'DEMO-1-0074', 'Curated demo listing #74 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004b', 2, 3, 'Alice''s Item #75', 'DEMO-1-0075', 'Curated demo listing #75 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004c', 2, 3, 'Alice''s Item #76', 'DEMO-1-0076', 'Curated demo listing #76 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004d', 2, 3, 'Alice''s Item #77', 'DEMO-1-0077', 'Curated demo listing #77 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004e', 2, 3, 'Alice''s Item #78', 'DEMO-1-0078', 'Curated demo listing #78 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004f', 2, 3, 'Alice''s Item #79', 'DEMO-1-0079', 'Curated demo listing #79 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000050', 2, 3, 'Alice''s Item #80', 'DEMO-1-0080', 'Curated demo listing #80 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000051', 2, 3, 'Alice''s Item #81', 'DEMO-1-0081', 'Curated demo listing #81 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000052', 2, 3, 'Alice''s Item #82', 'DEMO-1-0082', 'Curated demo listing #82 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000053', 2, 3, 'Alice''s Item #83', 'DEMO-1-0083', 'Curated demo listing #83 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000054', 2, 3, 'Alice''s Item #84', 'DEMO-1-0084', 'Curated demo listing #84 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000055', 2, 3, 'Alice''s Item #85', 'DEMO-1-0085', 'Curated demo listing #85 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000056', 2, 3, 'Alice''s Item #86', 'DEMO-1-0086', 'Curated demo listing #86 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000057', 2, 3, 'Alice''s Item #87', 'DEMO-1-0087', 'Curated demo listing #87 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000058', 2, 3, 'Alice''s Item #88', 'DEMO-1-0088', 'Curated demo listing #88 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000059', 2, 3, 'Alice''s Item #89', 'DEMO-1-0089', 'Curated demo listing #89 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005a', 2, 3, 'Alice''s Item #90', 'DEMO-1-0090', 'Curated demo listing #90 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005b', 2, 3, 'Alice''s Item #91', 'DEMO-1-0091', 'Curated demo listing #91 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005c', 2, 3, 'Alice''s Item #92', 'DEMO-1-0092', 'Curated demo listing #92 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005d', 2, 3, 'Alice''s Item #93', 'DEMO-1-0093', 'Curated demo listing #93 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005e', 2, 3, 'Alice''s Item #94', 'DEMO-1-0094', 'Curated demo listing #94 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005f', 2, 3, 'Alice''s Item #95', 'DEMO-1-0095', 'Curated demo listing #95 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000060', 2, 3, 'Alice''s Item #96', 'DEMO-1-0096', 'Curated demo listing #96 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000061', 2, 3, 'Alice''s Item #97', 'DEMO-1-0097', 'Curated demo listing #97 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000062', 2, 3, 'Alice''s Item #98', 'DEMO-1-0098', 'Curated demo listing #98 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000063', 2, 3, 'Alice''s Item #99', 'DEMO-1-0099', 'Curated demo listing #99 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000064', 2, 3, 'Alice''s Item #100', 'DEMO-1-0100', 'Curated demo listing #100 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000001', 2, 3, 'Brian''s Item #1', 'DEMO-2-0001', 'Curated demo listing #1 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000002', 2, 3, 'Brian''s Item #2', 'DEMO-2-0002', 'Curated demo listing #2 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000003', 2, 3, 'Brian''s Item #3', 'DEMO-2-0003', 'Curated demo listing #3 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000004', 2, 3, 'Brian''s Item #4', 'DEMO-2-0004', 'Curated demo listing #4 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000005', 2, 3, 'Brian''s Item #5', 'DEMO-2-0005', 'Curated demo listing #5 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000006', 2, 3, 'Brian''s Item #6', 'DEMO-2-0006', 'Curated demo listing #6 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000007', 2, 3, 'Brian''s Item #7', 'DEMO-2-0007', 'Curated demo listing #7 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000008', 2, 3, 'Brian''s Item #8', 'DEMO-2-0008', 'Curated demo listing #8 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000009', 2, 3, 'Brian''s Item #9', 'DEMO-2-0009', 'Curated demo listing #9 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000a', 2, 3, 'Brian''s Item #10', 'DEMO-2-0010', 'Curated demo listing #10 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000b', 2, 3, 'Brian''s Item #11', 'DEMO-2-0011', 'Curated demo listing #11 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000c', 2, 3, 'Brian''s Item #12', 'DEMO-2-0012', 'Curated demo listing #12 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000d', 2, 3, 'Brian''s Item #13', 'DEMO-2-0013', 'Curated demo listing #13 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000e', 2, 3, 'Brian''s Item #14', 'DEMO-2-0014', 'Curated demo listing #14 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000f', 2, 3, 'Brian''s Item #15', 'DEMO-2-0015', 'Curated demo listing #15 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000010', 2, 3, 'Brian''s Item #16', 'DEMO-2-0016', 'Curated demo listing #16 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000011', 2, 3, 'Brian''s Item #17', 'DEMO-2-0017', 'Curated demo listing #17 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000012', 2, 3, 'Brian''s Item #18', 'DEMO-2-0018', 'Curated demo listing #18 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000013', 2, 3, 'Brian''s Item #19', 'DEMO-2-0019', 'Curated demo listing #19 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000014', 2, 3, 'Brian''s Item #20', 'DEMO-2-0020', 'Curated demo listing #20 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000015', 2, 3, 'Brian''s Item #21', 'DEMO-2-0021', 'Curated demo listing #21 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000016', 2, 3, 'Brian''s Item #22', 'DEMO-2-0022', 'Curated demo listing #22 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000017', 2, 3, 'Brian''s Item #23', 'DEMO-2-0023', 'Curated demo listing #23 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000018', 2, 3, 'Brian''s Item #24', 'DEMO-2-0024', 'Curated demo listing #24 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000019', 2, 3, 'Brian''s Item #25', 'DEMO-2-0025', 'Curated demo listing #25 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001a', 2, 3, 'Brian''s Item #26', 'DEMO-2-0026', 'Curated demo listing #26 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001b', 2, 3, 'Brian''s Item #27', 'DEMO-2-0027', 'Curated demo listing #27 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001c', 2, 3, 'Brian''s Item #28', 'DEMO-2-0028', 'Curated demo listing #28 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001d', 2, 3, 'Brian''s Item #29', 'DEMO-2-0029', 'Curated demo listing #29 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001e', 2, 3, 'Brian''s Item #30', 'DEMO-2-0030', 'Curated demo listing #30 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001f', 2, 3, 'Brian''s Item #31', 'DEMO-2-0031', 'Curated demo listing #31 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000020', 2, 3, 'Brian''s Item #32', 'DEMO-2-0032', 'Curated demo listing #32 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000021', 2, 3, 'Brian''s Item #33', 'DEMO-2-0033', 'Curated demo listing #33 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000022', 2, 3, 'Brian''s Item #34', 'DEMO-2-0034', 'Curated demo listing #34 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000023', 2, 3, 'Brian''s Item #35', 'DEMO-2-0035', 'Curated demo listing #35 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000024', 2, 3, 'Brian''s Item #36', 'DEMO-2-0036', 'Curated demo listing #36 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000025', 2, 3, 'Brian''s Item #37', 'DEMO-2-0037', 'Curated demo listing #37 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000026', 2, 3, 'Brian''s Item #38', 'DEMO-2-0038', 'Curated demo listing #38 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000027', 2, 3, 'Brian''s Item #39', 'DEMO-2-0039', 'Curated demo listing #39 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000028', 2, 3, 'Brian''s Item #40', 'DEMO-2-0040', 'Curated demo listing #40 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000029', 2, 3, 'Brian''s Item #41', 'DEMO-2-0041', 'Curated demo listing #41 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002a', 2, 3, 'Brian''s Item #42', 'DEMO-2-0042', 'Curated demo listing #42 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002b', 2, 3, 'Brian''s Item #43', 'DEMO-2-0043', 'Curated demo listing #43 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002c', 2, 3, 'Brian''s Item #44', 'DEMO-2-0044', 'Curated demo listing #44 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002d', 2, 3, 'Brian''s Item #45', 'DEMO-2-0045', 'Curated demo listing #45 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002e', 2, 3, 'Brian''s Item #46', 'DEMO-2-0046', 'Curated demo listing #46 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002f', 2, 3, 'Brian''s Item #47', 'DEMO-2-0047', 'Curated demo listing #47 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000030', 2, 3, 'Brian''s Item #48', 'DEMO-2-0048', 'Curated demo listing #48 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000031', 2, 3, 'Brian''s Item #49', 'DEMO-2-0049', 'Curated demo listing #49 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000032', 2, 3, 'Brian''s Item #50', 'DEMO-2-0050', 'Curated demo listing #50 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000033', 2, 3, 'Brian''s Item #51', 'DEMO-2-0051', 'Curated demo listing #51 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000034', 2, 3, 'Brian''s Item #52', 'DEMO-2-0052', 'Curated demo listing #52 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000035', 2, 3, 'Brian''s Item #53', 'DEMO-2-0053', 'Curated demo listing #53 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000036', 2, 3, 'Brian''s Item #54', 'DEMO-2-0054', 'Curated demo listing #54 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000037', 2, 3, 'Brian''s Item #55', 'DEMO-2-0055', 'Curated demo listing #55 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000038', 2, 3, 'Brian''s Item #56', 'DEMO-2-0056', 'Curated demo listing #56 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000039', 2, 3, 'Brian''s Item #57', 'DEMO-2-0057', 'Curated demo listing #57 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003a', 2, 3, 'Brian''s Item #58', 'DEMO-2-0058', 'Curated demo listing #58 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003b', 2, 3, 'Brian''s Item #59', 'DEMO-2-0059', 'Curated demo listing #59 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003c', 2, 3, 'Brian''s Item #60', 'DEMO-2-0060', 'Curated demo listing #60 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003d', 2, 3, 'Brian''s Item #61', 'DEMO-2-0061', 'Curated demo listing #61 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003e', 2, 3, 'Brian''s Item #62', 'DEMO-2-0062', 'Curated demo listing #62 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003f', 2, 3, 'Brian''s Item #63', 'DEMO-2-0063', 'Curated demo listing #63 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000040', 2, 3, 'Brian''s Item #64', 'DEMO-2-0064', 'Curated demo listing #64 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000041', 2, 3, 'Brian''s Item #65', 'DEMO-2-0065', 'Curated demo listing #65 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000042', 2, 3, 'Brian''s Item #66', 'DEMO-2-0066', 'Curated demo listing #66 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000043', 2, 3, 'Brian''s Item #67', 'DEMO-2-0067', 'Curated demo listing #67 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000044', 2, 3, 'Brian''s Item #68', 'DEMO-2-0068', 'Curated demo listing #68 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000045', 2, 3, 'Brian''s Item #69', 'DEMO-2-0069', 'Curated demo listing #69 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000046', 2, 3, 'Brian''s Item #70', 'DEMO-2-0070', 'Curated demo listing #70 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000047', 2, 3, 'Brian''s Item #71', 'DEMO-2-0071', 'Curated demo listing #71 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000048', 2, 3, 'Brian''s Item #72', 'DEMO-2-0072', 'Curated demo listing #72 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000049', 2, 3, 'Brian''s Item #73', 'DEMO-2-0073', 'Curated demo listing #73 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004a', 2, 3, 'Brian''s Item #74', 'DEMO-2-0074', 'Curated demo listing #74 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004b', 2, 3, 'Brian''s Item #75', 'DEMO-2-0075', 'Curated demo listing #75 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004c', 2, 3, 'Brian''s Item #76', 'DEMO-2-0076', 'Curated demo listing #76 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004d', 2, 3, 'Brian''s Item #77', 'DEMO-2-0077', 'Curated demo listing #77 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004e', 2, 3, 'Brian''s Item #78', 'DEMO-2-0078', 'Curated demo listing #78 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004f', 2, 3, 'Brian''s Item #79', 'DEMO-2-0079', 'Curated demo listing #79 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000050', 2, 3, 'Brian''s Item #80', 'DEMO-2-0080', 'Curated demo listing #80 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000051', 2, 3, 'Brian''s Item #81', 'DEMO-2-0081', 'Curated demo listing #81 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000052', 2, 3, 'Brian''s Item #82', 'DEMO-2-0082', 'Curated demo listing #82 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000053', 2, 3, 'Brian''s Item #83', 'DEMO-2-0083', 'Curated demo listing #83 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000054', 2, 3, 'Brian''s Item #84', 'DEMO-2-0084', 'Curated demo listing #84 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000055', 2, 3, 'Brian''s Item #85', 'DEMO-2-0085', 'Curated demo listing #85 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000056', 2, 3, 'Brian''s Item #86', 'DEMO-2-0086', 'Curated demo listing #86 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000057', 2, 3, 'Brian''s Item #87', 'DEMO-2-0087', 'Curated demo listing #87 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000058', 2, 3, 'Brian''s Item #88', 'DEMO-2-0088', 'Curated demo listing #88 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000059', 2, 3, 'Brian''s Item #89', 'DEMO-2-0089', 'Curated demo listing #89 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005a', 2, 3, 'Brian''s Item #90', 'DEMO-2-0090', 'Curated demo listing #90 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005b', 2, 3, 'Brian''s Item #91', 'DEMO-2-0091', 'Curated demo listing #91 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005c', 2, 3, 'Brian''s Item #92', 'DEMO-2-0092', 'Curated demo listing #92 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005d', 2, 3, 'Brian''s Item #93', 'DEMO-2-0093', 'Curated demo listing #93 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005e', 2, 3, 'Brian''s Item #94', 'DEMO-2-0094', 'Curated demo listing #94 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005f', 2, 3, 'Brian''s Item #95', 'DEMO-2-0095', 'Curated demo listing #95 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000060', 2, 3, 'Brian''s Item #96', 'DEMO-2-0096', 'Curated demo listing #96 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000061', 2, 3, 'Brian''s Item #97', 'DEMO-2-0097', 'Curated demo listing #97 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000062', 2, 3, 'Brian''s Item #98', 'DEMO-2-0098', 'Curated demo listing #98 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000063', 2, 3, 'Brian''s Item #99', 'DEMO-2-0099', 'Curated demo listing #99 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000064', 2, 3, 'Brian''s Item #100', 'DEMO-2-0100', 'Curated demo listing #100 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000001', 2, 3, 'Cecilia''s Item #1', 'DEMO-3-0001', 'Curated demo listing #1 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000002', 2, 3, 'Cecilia''s Item #2', 'DEMO-3-0002', 'Curated demo listing #2 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000003', 2, 3, 'Cecilia''s Item #3', 'DEMO-3-0003', 'Curated demo listing #3 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000004', 2, 3, 'Cecilia''s Item #4', 'DEMO-3-0004', 'Curated demo listing #4 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000005', 2, 3, 'Cecilia''s Item #5', 'DEMO-3-0005', 'Curated demo listing #5 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000006', 2, 3, 'Cecilia''s Item #6', 'DEMO-3-0006', 'Curated demo listing #6 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000007', 2, 3, 'Cecilia''s Item #7', 'DEMO-3-0007', 'Curated demo listing #7 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000008', 2, 3, 'Cecilia''s Item #8', 'DEMO-3-0008', 'Curated demo listing #8 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000009', 2, 3, 'Cecilia''s Item #9', 'DEMO-3-0009', 'Curated demo listing #9 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000a', 2, 3, 'Cecilia''s Item #10', 'DEMO-3-0010', 'Curated demo listing #10 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000b', 2, 3, 'Cecilia''s Item #11', 'DEMO-3-0011', 'Curated demo listing #11 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000c', 2, 3, 'Cecilia''s Item #12', 'DEMO-3-0012', 'Curated demo listing #12 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000d', 2, 3, 'Cecilia''s Item #13', 'DEMO-3-0013', 'Curated demo listing #13 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000e', 2, 3, 'Cecilia''s Item #14', 'DEMO-3-0014', 'Curated demo listing #14 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000f', 2, 3, 'Cecilia''s Item #15', 'DEMO-3-0015', 'Curated demo listing #15 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000010', 2, 3, 'Cecilia''s Item #16', 'DEMO-3-0016', 'Curated demo listing #16 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000011', 2, 3, 'Cecilia''s Item #17', 'DEMO-3-0017', 'Curated demo listing #17 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000012', 2, 3, 'Cecilia''s Item #18', 'DEMO-3-0018', 'Curated demo listing #18 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000013', 2, 3, 'Cecilia''s Item #19', 'DEMO-3-0019', 'Curated demo listing #19 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000014', 2, 3, 'Cecilia''s Item #20', 'DEMO-3-0020', 'Curated demo listing #20 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000015', 2, 3, 'Cecilia''s Item #21', 'DEMO-3-0021', 'Curated demo listing #21 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000016', 2, 3, 'Cecilia''s Item #22', 'DEMO-3-0022', 'Curated demo listing #22 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000017', 2, 3, 'Cecilia''s Item #23', 'DEMO-3-0023', 'Curated demo listing #23 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000018', 2, 3, 'Cecilia''s Item #24', 'DEMO-3-0024', 'Curated demo listing #24 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000019', 2, 3, 'Cecilia''s Item #25', 'DEMO-3-0025', 'Curated demo listing #25 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001a', 2, 3, 'Cecilia''s Item #26', 'DEMO-3-0026', 'Curated demo listing #26 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001b', 2, 3, 'Cecilia''s Item #27', 'DEMO-3-0027', 'Curated demo listing #27 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001c', 2, 3, 'Cecilia''s Item #28', 'DEMO-3-0028', 'Curated demo listing #28 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001d', 2, 3, 'Cecilia''s Item #29', 'DEMO-3-0029', 'Curated demo listing #29 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001e', 2, 3, 'Cecilia''s Item #30', 'DEMO-3-0030', 'Curated demo listing #30 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001f', 2, 3, 'Cecilia''s Item #31', 'DEMO-3-0031', 'Curated demo listing #31 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000020', 2, 3, 'Cecilia''s Item #32', 'DEMO-3-0032', 'Curated demo listing #32 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000021', 2, 3, 'Cecilia''s Item #33', 'DEMO-3-0033', 'Curated demo listing #33 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000022', 2, 3, 'Cecilia''s Item #34', 'DEMO-3-0034', 'Curated demo listing #34 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000023', 2, 3, 'Cecilia''s Item #35', 'DEMO-3-0035', 'Curated demo listing #35 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000024', 2, 3, 'Cecilia''s Item #36', 'DEMO-3-0036', 'Curated demo listing #36 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000025', 2, 3, 'Cecilia''s Item #37', 'DEMO-3-0037', 'Curated demo listing #37 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000026', 2, 3, 'Cecilia''s Item #38', 'DEMO-3-0038', 'Curated demo listing #38 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000027', 2, 3, 'Cecilia''s Item #39', 'DEMO-3-0039', 'Curated demo listing #39 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000028', 2, 3, 'Cecilia''s Item #40', 'DEMO-3-0040', 'Curated demo listing #40 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000029', 2, 3, 'Cecilia''s Item #41', 'DEMO-3-0041', 'Curated demo listing #41 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002a', 2, 3, 'Cecilia''s Item #42', 'DEMO-3-0042', 'Curated demo listing #42 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002b', 2, 3, 'Cecilia''s Item #43', 'DEMO-3-0043', 'Curated demo listing #43 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002c', 2, 3, 'Cecilia''s Item #44', 'DEMO-3-0044', 'Curated demo listing #44 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002d', 2, 3, 'Cecilia''s Item #45', 'DEMO-3-0045', 'Curated demo listing #45 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002e', 2, 3, 'Cecilia''s Item #46', 'DEMO-3-0046', 'Curated demo listing #46 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002f', 2, 3, 'Cecilia''s Item #47', 'DEMO-3-0047', 'Curated demo listing #47 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000030', 2, 3, 'Cecilia''s Item #48', 'DEMO-3-0048', 'Curated demo listing #48 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000031', 2, 3, 'Cecilia''s Item #49', 'DEMO-3-0049', 'Curated demo listing #49 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000032', 2, 3, 'Cecilia''s Item #50', 'DEMO-3-0050', 'Curated demo listing #50 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000033', 2, 3, 'Cecilia''s Item #51', 'DEMO-3-0051', 'Curated demo listing #51 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000034', 2, 3, 'Cecilia''s Item #52', 'DEMO-3-0052', 'Curated demo listing #52 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000035', 2, 3, 'Cecilia''s Item #53', 'DEMO-3-0053', 'Curated demo listing #53 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000036', 2, 3, 'Cecilia''s Item #54', 'DEMO-3-0054', 'Curated demo listing #54 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000037', 2, 3, 'Cecilia''s Item #55', 'DEMO-3-0055', 'Curated demo listing #55 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000038', 2, 3, 'Cecilia''s Item #56', 'DEMO-3-0056', 'Curated demo listing #56 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000039', 2, 3, 'Cecilia''s Item #57', 'DEMO-3-0057', 'Curated demo listing #57 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003a', 2, 3, 'Cecilia''s Item #58', 'DEMO-3-0058', 'Curated demo listing #58 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003b', 2, 3, 'Cecilia''s Item #59', 'DEMO-3-0059', 'Curated demo listing #59 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003c', 2, 3, 'Cecilia''s Item #60', 'DEMO-3-0060', 'Curated demo listing #60 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003d', 2, 3, 'Cecilia''s Item #61', 'DEMO-3-0061', 'Curated demo listing #61 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003e', 2, 3, 'Cecilia''s Item #62', 'DEMO-3-0062', 'Curated demo listing #62 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003f', 2, 3, 'Cecilia''s Item #63', 'DEMO-3-0063', 'Curated demo listing #63 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000040', 2, 3, 'Cecilia''s Item #64', 'DEMO-3-0064', 'Curated demo listing #64 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000041', 2, 3, 'Cecilia''s Item #65', 'DEMO-3-0065', 'Curated demo listing #65 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000042', 2, 3, 'Cecilia''s Item #66', 'DEMO-3-0066', 'Curated demo listing #66 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000043', 2, 3, 'Cecilia''s Item #67', 'DEMO-3-0067', 'Curated demo listing #67 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000044', 2, 3, 'Cecilia''s Item #68', 'DEMO-3-0068', 'Curated demo listing #68 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000045', 2, 3, 'Cecilia''s Item #69', 'DEMO-3-0069', 'Curated demo listing #69 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000046', 2, 3, 'Cecilia''s Item #70', 'DEMO-3-0070', 'Curated demo listing #70 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000047', 2, 3, 'Cecilia''s Item #71', 'DEMO-3-0071', 'Curated demo listing #71 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000048', 2, 3, 'Cecilia''s Item #72', 'DEMO-3-0072', 'Curated demo listing #72 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000049', 2, 3, 'Cecilia''s Item #73', 'DEMO-3-0073', 'Curated demo listing #73 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004a', 2, 3, 'Cecilia''s Item #74', 'DEMO-3-0074', 'Curated demo listing #74 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004b', 2, 3, 'Cecilia''s Item #75', 'DEMO-3-0075', 'Curated demo listing #75 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004c', 2, 3, 'Cecilia''s Item #76', 'DEMO-3-0076', 'Curated demo listing #76 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004d', 2, 3, 'Cecilia''s Item #77', 'DEMO-3-0077', 'Curated demo listing #77 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004e', 2, 3, 'Cecilia''s Item #78', 'DEMO-3-0078', 'Curated demo listing #78 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004f', 2, 3, 'Cecilia''s Item #79', 'DEMO-3-0079', 'Curated demo listing #79 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000050', 2, 3, 'Cecilia''s Item #80', 'DEMO-3-0080', 'Curated demo listing #80 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000051', 2, 3, 'Cecilia''s Item #81', 'DEMO-3-0081', 'Curated demo listing #81 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000052', 2, 3, 'Cecilia''s Item #82', 'DEMO-3-0082', 'Curated demo listing #82 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000053', 2, 3, 'Cecilia''s Item #83', 'DEMO-3-0083', 'Curated demo listing #83 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000054', 2, 3, 'Cecilia''s Item #84', 'DEMO-3-0084', 'Curated demo listing #84 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000055', 2, 3, 'Cecilia''s Item #85', 'DEMO-3-0085', 'Curated demo listing #85 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000056', 2, 3, 'Cecilia''s Item #86', 'DEMO-3-0086', 'Curated demo listing #86 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000057', 2, 3, 'Cecilia''s Item #87', 'DEMO-3-0087', 'Curated demo listing #87 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000058', 2, 3, 'Cecilia''s Item #88', 'DEMO-3-0088', 'Curated demo listing #88 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000059', 2, 3, 'Cecilia''s Item #89', 'DEMO-3-0089', 'Curated demo listing #89 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005a', 2, 3, 'Cecilia''s Item #90', 'DEMO-3-0090', 'Curated demo listing #90 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005b', 2, 3, 'Cecilia''s Item #91', 'DEMO-3-0091', 'Curated demo listing #91 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005c', 2, 3, 'Cecilia''s Item #92', 'DEMO-3-0092', 'Curated demo listing #92 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005d', 2, 3, 'Cecilia''s Item #93', 'DEMO-3-0093', 'Curated demo listing #93 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005e', 2, 3, 'Cecilia''s Item #94', 'DEMO-3-0094', 'Curated demo listing #94 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005f', 2, 3, 'Cecilia''s Item #95', 'DEMO-3-0095', 'Curated demo listing #95 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000060', 2, 3, 'Cecilia''s Item #96', 'DEMO-3-0096', 'Curated demo listing #96 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000061', 2, 3, 'Cecilia''s Item #97', 'DEMO-3-0097', 'Curated demo listing #97 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000062', 2, 3, 'Cecilia''s Item #98', 'DEMO-3-0098', 'Curated demo listing #98 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000063', 2, 3, 'Cecilia''s Item #99', 'DEMO-3-0099', 'Curated demo listing #99 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000064', 2, 3, 'Cecilia''s Item #100', 'DEMO-3-0100', 'Curated demo listing #100 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);
INSERT INTO public.listing VALUES ('f871bd3f-1abe-4f84-a4c5-2138a1be76b9', 2, 3, 'Báo mới', 'baomoi', 'bài báo mới', 'e0000000-0000-0000-0000-000000000004', NULL, 'new', NULL, NULL, '2026-03-14 16:53:03.385539+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 10, false, NULL, NULL, '2026-03-14 16:53:03.570133+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.listing VALUES ('67f00868-4f9d-4486-a63e-20d6e0a161b7', 2, 3, 'Bán tranh T1', 't16sa0', 'Tranh t1 5 skin', '20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001', 'New seal', NULL, NULL, '2026-03-14 16:55:48.154207+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 500, false, NULL, NULL, '2026-03-14 16:02:19.980505+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2026-03-14 16:55:48.165398+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false);


--
-- TOC entry 4324 (class 0 OID 33152)
-- Dependencies: 387
-- Data for Name: listing_id; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000019', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000001a', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000001b', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000001c', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000001d', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000001e', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000001f', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000020', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000021', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000022', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000023', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000024', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000025', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000026', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000027', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000028', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000029', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000002a', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000002b', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000002c', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000002d', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000002e', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000002f', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000030', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000031', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000032', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000033', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000034', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000035', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000036', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000037', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000038', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000039', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000003a', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000003b', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000003c', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000003d', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000003e', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000003f', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000040', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000041', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000042', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000043', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000044', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000045', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000046', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000047', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000048', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000049', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000004a', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000004b', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000004c', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000004d', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000004e', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000004f', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000050', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000051', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000052', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000053', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000054', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000055', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000056', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000057', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000058', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000059', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000005a', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000005b', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000005c', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000005d', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000005e', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-00000000005f', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000060', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000061', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000062', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000063', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('71000000-0000-0000-0000-000000000064', '70000000-0000-0000-0000-000000000001');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000019', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000001a', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000001b', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000001c', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000001d', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000001e', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000001f', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000020', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000021', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000022', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000023', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000024', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000025', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000026', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000027', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000028', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000029', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000002a', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000002b', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000002c', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000002d', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000002e', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000002f', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000030', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000031', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000032', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000033', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000034', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000035', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000036', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000037', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000038', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000039', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000003a', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000003b', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000003c', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000003d', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000003e', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000003f', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000040', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000041', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000042', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000043', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000044', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000045', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000046', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000047', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000048', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000049', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000004a', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000004b', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000004c', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000004d', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000004e', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000004f', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000050', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000051', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000052', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000053', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000054', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000055', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000056', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000057', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000058', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000059', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000005a', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000005b', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000005c', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000005d', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000005e', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-00000000005f', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000060', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000061', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000062', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000063', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('72000000-0000-0000-0000-000000000064', '70000000-0000-0000-0000-000000000002');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000019', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000001a', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000001b', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000001c', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000001d', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000001e', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000001f', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000020', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000021', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000022', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000023', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000024', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000025', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000026', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000027', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000028', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000029', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000002a', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000002b', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000002c', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000002d', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000002e', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000002f', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000030', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000031', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000032', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000033', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000034', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000035', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000036', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000037', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000038', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000039', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000003a', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000003b', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000003c', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000003d', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000003e', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000003f', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000040', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000041', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000042', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000043', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000044', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000045', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000046', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000047', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000048', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000049', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000004a', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000004b', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000004c', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000004d', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000004e', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000004f', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000050', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000051', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000052', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000053', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000054', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000055', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000056', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000057', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000058', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000059', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000005a', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000005b', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000005c', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000005d', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000005e', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-00000000005f', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000060', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000061', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000062', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000063', '70000000-0000-0000-0000-000000000003');
INSERT INTO public.listing_id VALUES ('73000000-0000-0000-0000-000000000064', '70000000-0000-0000-0000-000000000003');


--
-- TOC entry 4317 (class 0 OID 33085)
-- Dependencies: 380
-- Data for Name: listing_image; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000001', 1, 'https://picsum.photos/seed/1-1/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000002', 1, 'https://picsum.photos/seed/1-2/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000003', 1, 'https://picsum.photos/seed/1-3/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000004', 1, 'https://picsum.photos/seed/1-4/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000005', 1, 'https://picsum.photos/seed/1-5/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000006', 1, 'https://picsum.photos/seed/1-6/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000007', 1, 'https://picsum.photos/seed/1-7/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000008', 1, 'https://picsum.photos/seed/1-8/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000009', 1, 'https://picsum.photos/seed/1-9/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000000a', 1, 'https://picsum.photos/seed/1-10/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000000b', 1, 'https://picsum.photos/seed/1-11/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000000c', 1, 'https://picsum.photos/seed/1-12/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000000d', 1, 'https://picsum.photos/seed/1-13/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000000e', 1, 'https://picsum.photos/seed/1-14/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000000f', 1, 'https://picsum.photos/seed/1-15/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000010', 1, 'https://picsum.photos/seed/1-16/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000011', 1, 'https://picsum.photos/seed/1-17/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000012', 1, 'https://picsum.photos/seed/1-18/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000013', 1, 'https://picsum.photos/seed/1-19/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000014', 1, 'https://picsum.photos/seed/1-20/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000015', 1, 'https://picsum.photos/seed/1-21/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000016', 1, 'https://picsum.photos/seed/1-22/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000017', 1, 'https://picsum.photos/seed/1-23/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000018', 1, 'https://picsum.photos/seed/1-24/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000019', 1, 'https://picsum.photos/seed/1-25/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000001a', 1, 'https://picsum.photos/seed/1-26/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000001b', 1, 'https://picsum.photos/seed/1-27/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000001c', 1, 'https://picsum.photos/seed/1-28/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000001d', 1, 'https://picsum.photos/seed/1-29/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000001e', 1, 'https://picsum.photos/seed/1-30/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000001f', 1, 'https://picsum.photos/seed/1-31/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000020', 1, 'https://picsum.photos/seed/1-32/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000021', 1, 'https://picsum.photos/seed/1-33/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000022', 1, 'https://picsum.photos/seed/1-34/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000023', 1, 'https://picsum.photos/seed/1-35/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000024', 1, 'https://picsum.photos/seed/1-36/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000025', 1, 'https://picsum.photos/seed/1-37/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000026', 1, 'https://picsum.photos/seed/1-38/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000027', 1, 'https://picsum.photos/seed/1-39/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000028', 1, 'https://picsum.photos/seed/1-40/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000029', 1, 'https://picsum.photos/seed/1-41/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000002a', 1, 'https://picsum.photos/seed/1-42/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000002b', 1, 'https://picsum.photos/seed/1-43/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000002c', 1, 'https://picsum.photos/seed/1-44/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000002d', 1, 'https://picsum.photos/seed/1-45/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000002e', 1, 'https://picsum.photos/seed/1-46/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000002f', 1, 'https://picsum.photos/seed/1-47/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000030', 1, 'https://picsum.photos/seed/1-48/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000031', 1, 'https://picsum.photos/seed/1-49/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000032', 1, 'https://picsum.photos/seed/1-50/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000033', 1, 'https://picsum.photos/seed/1-51/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000034', 1, 'https://picsum.photos/seed/1-52/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000035', 1, 'https://picsum.photos/seed/1-53/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000036', 1, 'https://picsum.photos/seed/1-54/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000037', 1, 'https://picsum.photos/seed/1-55/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000038', 1, 'https://picsum.photos/seed/1-56/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000039', 1, 'https://picsum.photos/seed/1-57/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000003a', 1, 'https://picsum.photos/seed/1-58/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000003b', 1, 'https://picsum.photos/seed/1-59/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000003c', 1, 'https://picsum.photos/seed/1-60/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000003d', 1, 'https://picsum.photos/seed/1-61/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000003e', 1, 'https://picsum.photos/seed/1-62/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000003f', 1, 'https://picsum.photos/seed/1-63/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000040', 1, 'https://picsum.photos/seed/1-64/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000041', 1, 'https://picsum.photos/seed/1-65/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000042', 1, 'https://picsum.photos/seed/1-66/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000043', 1, 'https://picsum.photos/seed/1-67/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000044', 1, 'https://picsum.photos/seed/1-68/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000045', 1, 'https://picsum.photos/seed/1-69/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000046', 1, 'https://picsum.photos/seed/1-70/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000047', 1, 'https://picsum.photos/seed/1-71/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000048', 1, 'https://picsum.photos/seed/1-72/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000049', 1, 'https://picsum.photos/seed/1-73/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000004a', 1, 'https://picsum.photos/seed/1-74/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000004b', 1, 'https://picsum.photos/seed/1-75/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000004c', 1, 'https://picsum.photos/seed/1-76/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000004d', 1, 'https://picsum.photos/seed/1-77/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000004e', 1, 'https://picsum.photos/seed/1-78/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000004f', 1, 'https://picsum.photos/seed/1-79/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000050', 1, 'https://picsum.photos/seed/1-80/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000051', 1, 'https://picsum.photos/seed/1-81/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000052', 1, 'https://picsum.photos/seed/1-82/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000053', 1, 'https://picsum.photos/seed/1-83/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000054', 1, 'https://picsum.photos/seed/1-84/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000055', 1, 'https://picsum.photos/seed/1-85/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000056', 1, 'https://picsum.photos/seed/1-86/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000057', 1, 'https://picsum.photos/seed/1-87/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000058', 1, 'https://picsum.photos/seed/1-88/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000059', 1, 'https://picsum.photos/seed/1-89/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000005a', 1, 'https://picsum.photos/seed/1-90/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000005b', 1, 'https://picsum.photos/seed/1-91/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000005c', 1, 'https://picsum.photos/seed/1-92/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000005d', 1, 'https://picsum.photos/seed/1-93/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000005e', 1, 'https://picsum.photos/seed/1-94/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-00000000005f', 1, 'https://picsum.photos/seed/1-95/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000060', 1, 'https://picsum.photos/seed/1-96/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000061', 1, 'https://picsum.photos/seed/1-97/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000062', 1, 'https://picsum.photos/seed/1-98/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000063', 1, 'https://picsum.photos/seed/1-99/640/640', true);
INSERT INTO public.listing_image VALUES ('71000000-0000-0000-0000-000000000064', 1, 'https://picsum.photos/seed/1-100/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000001', 1, 'https://picsum.photos/seed/2-1/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000002', 1, 'https://picsum.photos/seed/2-2/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000003', 1, 'https://picsum.photos/seed/2-3/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000004', 1, 'https://picsum.photos/seed/2-4/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000005', 1, 'https://picsum.photos/seed/2-5/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000006', 1, 'https://picsum.photos/seed/2-6/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000007', 1, 'https://picsum.photos/seed/2-7/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000008', 1, 'https://picsum.photos/seed/2-8/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000009', 1, 'https://picsum.photos/seed/2-9/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000000a', 1, 'https://picsum.photos/seed/2-10/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000000b', 1, 'https://picsum.photos/seed/2-11/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000000c', 1, 'https://picsum.photos/seed/2-12/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000000d', 1, 'https://picsum.photos/seed/2-13/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000000e', 1, 'https://picsum.photos/seed/2-14/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000000f', 1, 'https://picsum.photos/seed/2-15/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000010', 1, 'https://picsum.photos/seed/2-16/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000011', 1, 'https://picsum.photos/seed/2-17/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000012', 1, 'https://picsum.photos/seed/2-18/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000013', 1, 'https://picsum.photos/seed/2-19/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000014', 1, 'https://picsum.photos/seed/2-20/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000015', 1, 'https://picsum.photos/seed/2-21/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000016', 1, 'https://picsum.photos/seed/2-22/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000017', 1, 'https://picsum.photos/seed/2-23/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000018', 1, 'https://picsum.photos/seed/2-24/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000019', 1, 'https://picsum.photos/seed/2-25/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000001a', 1, 'https://picsum.photos/seed/2-26/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000001b', 1, 'https://picsum.photos/seed/2-27/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000001c', 1, 'https://picsum.photos/seed/2-28/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000001d', 1, 'https://picsum.photos/seed/2-29/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000001e', 1, 'https://picsum.photos/seed/2-30/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000001f', 1, 'https://picsum.photos/seed/2-31/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000020', 1, 'https://picsum.photos/seed/2-32/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000021', 1, 'https://picsum.photos/seed/2-33/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000022', 1, 'https://picsum.photos/seed/2-34/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000023', 1, 'https://picsum.photos/seed/2-35/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000024', 1, 'https://picsum.photos/seed/2-36/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000025', 1, 'https://picsum.photos/seed/2-37/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000026', 1, 'https://picsum.photos/seed/2-38/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000027', 1, 'https://picsum.photos/seed/2-39/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000028', 1, 'https://picsum.photos/seed/2-40/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000029', 1, 'https://picsum.photos/seed/2-41/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000002a', 1, 'https://picsum.photos/seed/2-42/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000002b', 1, 'https://picsum.photos/seed/2-43/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000002c', 1, 'https://picsum.photos/seed/2-44/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000002d', 1, 'https://picsum.photos/seed/2-45/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000002e', 1, 'https://picsum.photos/seed/2-46/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000002f', 1, 'https://picsum.photos/seed/2-47/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000030', 1, 'https://picsum.photos/seed/2-48/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000031', 1, 'https://picsum.photos/seed/2-49/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000032', 1, 'https://picsum.photos/seed/2-50/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000033', 1, 'https://picsum.photos/seed/2-51/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000034', 1, 'https://picsum.photos/seed/2-52/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000035', 1, 'https://picsum.photos/seed/2-53/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000036', 1, 'https://picsum.photos/seed/2-54/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000037', 1, 'https://picsum.photos/seed/2-55/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000038', 1, 'https://picsum.photos/seed/2-56/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000039', 1, 'https://picsum.photos/seed/2-57/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000003a', 1, 'https://picsum.photos/seed/2-58/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000003b', 1, 'https://picsum.photos/seed/2-59/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000003c', 1, 'https://picsum.photos/seed/2-60/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000003d', 1, 'https://picsum.photos/seed/2-61/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000003e', 1, 'https://picsum.photos/seed/2-62/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000003f', 1, 'https://picsum.photos/seed/2-63/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000040', 1, 'https://picsum.photos/seed/2-64/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000041', 1, 'https://picsum.photos/seed/2-65/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000042', 1, 'https://picsum.photos/seed/2-66/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000043', 1, 'https://picsum.photos/seed/2-67/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000044', 1, 'https://picsum.photos/seed/2-68/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000045', 1, 'https://picsum.photos/seed/2-69/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000046', 1, 'https://picsum.photos/seed/2-70/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000047', 1, 'https://picsum.photos/seed/2-71/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000048', 1, 'https://picsum.photos/seed/2-72/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000049', 1, 'https://picsum.photos/seed/2-73/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000004a', 1, 'https://picsum.photos/seed/2-74/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000004b', 1, 'https://picsum.photos/seed/2-75/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000004c', 1, 'https://picsum.photos/seed/2-76/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000004d', 1, 'https://picsum.photos/seed/2-77/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000004e', 1, 'https://picsum.photos/seed/2-78/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000004f', 1, 'https://picsum.photos/seed/2-79/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000050', 1, 'https://picsum.photos/seed/2-80/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000051', 1, 'https://picsum.photos/seed/2-81/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000052', 1, 'https://picsum.photos/seed/2-82/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000053', 1, 'https://picsum.photos/seed/2-83/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000054', 1, 'https://picsum.photos/seed/2-84/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000055', 1, 'https://picsum.photos/seed/2-85/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000056', 1, 'https://picsum.photos/seed/2-86/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000057', 1, 'https://picsum.photos/seed/2-87/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000058', 1, 'https://picsum.photos/seed/2-88/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000059', 1, 'https://picsum.photos/seed/2-89/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000005a', 1, 'https://picsum.photos/seed/2-90/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000005b', 1, 'https://picsum.photos/seed/2-91/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000005c', 1, 'https://picsum.photos/seed/2-92/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000005d', 1, 'https://picsum.photos/seed/2-93/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000005e', 1, 'https://picsum.photos/seed/2-94/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-00000000005f', 1, 'https://picsum.photos/seed/2-95/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000060', 1, 'https://picsum.photos/seed/2-96/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000061', 1, 'https://picsum.photos/seed/2-97/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000062', 1, 'https://picsum.photos/seed/2-98/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000063', 1, 'https://picsum.photos/seed/2-99/640/640', true);
INSERT INTO public.listing_image VALUES ('72000000-0000-0000-0000-000000000064', 1, 'https://picsum.photos/seed/2-100/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000001', 1, 'https://picsum.photos/seed/3-1/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000002', 1, 'https://picsum.photos/seed/3-2/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000003', 1, 'https://picsum.photos/seed/3-3/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000004', 1, 'https://picsum.photos/seed/3-4/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000005', 1, 'https://picsum.photos/seed/3-5/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000006', 1, 'https://picsum.photos/seed/3-6/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000007', 1, 'https://picsum.photos/seed/3-7/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000008', 1, 'https://picsum.photos/seed/3-8/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000009', 1, 'https://picsum.photos/seed/3-9/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000000a', 1, 'https://picsum.photos/seed/3-10/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000000b', 1, 'https://picsum.photos/seed/3-11/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000000c', 1, 'https://picsum.photos/seed/3-12/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000000d', 1, 'https://picsum.photos/seed/3-13/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000000e', 1, 'https://picsum.photos/seed/3-14/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000000f', 1, 'https://picsum.photos/seed/3-15/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000010', 1, 'https://picsum.photos/seed/3-16/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000011', 1, 'https://picsum.photos/seed/3-17/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000012', 1, 'https://picsum.photos/seed/3-18/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000013', 1, 'https://picsum.photos/seed/3-19/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000014', 1, 'https://picsum.photos/seed/3-20/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000015', 1, 'https://picsum.photos/seed/3-21/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000016', 1, 'https://picsum.photos/seed/3-22/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000017', 1, 'https://picsum.photos/seed/3-23/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000018', 1, 'https://picsum.photos/seed/3-24/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000019', 1, 'https://picsum.photos/seed/3-25/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000001a', 1, 'https://picsum.photos/seed/3-26/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000001b', 1, 'https://picsum.photos/seed/3-27/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000001c', 1, 'https://picsum.photos/seed/3-28/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000001d', 1, 'https://picsum.photos/seed/3-29/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000001e', 1, 'https://picsum.photos/seed/3-30/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000001f', 1, 'https://picsum.photos/seed/3-31/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000020', 1, 'https://picsum.photos/seed/3-32/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000021', 1, 'https://picsum.photos/seed/3-33/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000022', 1, 'https://picsum.photos/seed/3-34/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000023', 1, 'https://picsum.photos/seed/3-35/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000024', 1, 'https://picsum.photos/seed/3-36/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000025', 1, 'https://picsum.photos/seed/3-37/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000026', 1, 'https://picsum.photos/seed/3-38/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000027', 1, 'https://picsum.photos/seed/3-39/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000028', 1, 'https://picsum.photos/seed/3-40/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000029', 1, 'https://picsum.photos/seed/3-41/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000002a', 1, 'https://picsum.photos/seed/3-42/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000002b', 1, 'https://picsum.photos/seed/3-43/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000002c', 1, 'https://picsum.photos/seed/3-44/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000002d', 1, 'https://picsum.photos/seed/3-45/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000002e', 1, 'https://picsum.photos/seed/3-46/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000002f', 1, 'https://picsum.photos/seed/3-47/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000030', 1, 'https://picsum.photos/seed/3-48/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000031', 1, 'https://picsum.photos/seed/3-49/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000032', 1, 'https://picsum.photos/seed/3-50/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000033', 1, 'https://picsum.photos/seed/3-51/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000034', 1, 'https://picsum.photos/seed/3-52/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000035', 1, 'https://picsum.photos/seed/3-53/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000036', 1, 'https://picsum.photos/seed/3-54/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000037', 1, 'https://picsum.photos/seed/3-55/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000038', 1, 'https://picsum.photos/seed/3-56/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000039', 1, 'https://picsum.photos/seed/3-57/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000003a', 1, 'https://picsum.photos/seed/3-58/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000003b', 1, 'https://picsum.photos/seed/3-59/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000003c', 1, 'https://picsum.photos/seed/3-60/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000003d', 1, 'https://picsum.photos/seed/3-61/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000003e', 1, 'https://picsum.photos/seed/3-62/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000003f', 1, 'https://picsum.photos/seed/3-63/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000040', 1, 'https://picsum.photos/seed/3-64/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000041', 1, 'https://picsum.photos/seed/3-65/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000042', 1, 'https://picsum.photos/seed/3-66/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000043', 1, 'https://picsum.photos/seed/3-67/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000044', 1, 'https://picsum.photos/seed/3-68/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000045', 1, 'https://picsum.photos/seed/3-69/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000046', 1, 'https://picsum.photos/seed/3-70/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000047', 1, 'https://picsum.photos/seed/3-71/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000048', 1, 'https://picsum.photos/seed/3-72/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000049', 1, 'https://picsum.photos/seed/3-73/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000004a', 1, 'https://picsum.photos/seed/3-74/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000004b', 1, 'https://picsum.photos/seed/3-75/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000004c', 1, 'https://picsum.photos/seed/3-76/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000004d', 1, 'https://picsum.photos/seed/3-77/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000004e', 1, 'https://picsum.photos/seed/3-78/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000004f', 1, 'https://picsum.photos/seed/3-79/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000050', 1, 'https://picsum.photos/seed/3-80/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000051', 1, 'https://picsum.photos/seed/3-81/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000052', 1, 'https://picsum.photos/seed/3-82/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000053', 1, 'https://picsum.photos/seed/3-83/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000054', 1, 'https://picsum.photos/seed/3-84/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000055', 1, 'https://picsum.photos/seed/3-85/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000056', 1, 'https://picsum.photos/seed/3-86/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000057', 1, 'https://picsum.photos/seed/3-87/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000058', 1, 'https://picsum.photos/seed/3-88/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000059', 1, 'https://picsum.photos/seed/3-89/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000005a', 1, 'https://picsum.photos/seed/3-90/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000005b', 1, 'https://picsum.photos/seed/3-91/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000005c', 1, 'https://picsum.photos/seed/3-92/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000005d', 1, 'https://picsum.photos/seed/3-93/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000005e', 1, 'https://picsum.photos/seed/3-94/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-00000000005f', 1, 'https://picsum.photos/seed/3-95/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000060', 1, 'https://picsum.photos/seed/3-96/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000061', 1, 'https://picsum.photos/seed/3-97/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000062', 1, 'https://picsum.photos/seed/3-98/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000063', 1, 'https://picsum.photos/seed/3-99/640/640', true);
INSERT INTO public.listing_image VALUES ('73000000-0000-0000-0000-000000000064', 1, 'https://picsum.photos/seed/3-100/640/640', true);
INSERT INTO public.listing_image VALUES ('f871bd3f-1abe-4f84-a4c5-2138a1be76b9', 2, 'https://res.cloudinary.com/djmftornv/image/upload/v1773507202/ebay-clone/baomoi_f0bcbd35-ba53-4e66-a581-ce98750e8c61.png', true);
INSERT INTO public.listing_image VALUES ('67f00868-4f9d-4486-a63e-20d6e0a161b7', 3, 'https://res.cloudinary.com/djmftornv/image/upload/v1773507408/ebay-clone/Full team_17d21900-8d74-42f1-9c09-86f88f814433.jpg', true);


--
-- TOC entry 4319 (class 0 OID 33098)
-- Dependencies: 382
-- Data for Name: listing_item_specific; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.listing_item_specific VALUES ('f871bd3f-1abe-4f84-a4c5-2138a1be76b9', 15, 'Fabric Type', 'Cotton');
INSERT INTO public.listing_item_specific VALUES ('67f00868-4f9d-4486-a63e-20d6e0a161b7', 16, 'Brand', 'Casio');
INSERT INTO public.listing_item_specific VALUES ('67f00868-4f9d-4486-a63e-20d6e0a161b7', 17, 'Movement', 'Automatic');


--
-- TOC entry 4307 (class 0 OID 33008)
-- Dependencies: 370
-- Data for Name: listing_template; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.listing_template VALUES ('81000000-0000-0000-0000-000000000001', 'Alice''s Starter Template', 'Reusable template seeded for demo purposes.', '{"price": 49.99, "title": "Sample Listing Template", "quantity": 5, "categoryId": "10000000-0000-0000-0000-000000000002", "conditionId": "40000000-0000-0000-0000-000000000001"}', 'Fixed Price', 'https://picsum.photos/seed/template-1/320/180', '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing_template VALUES ('82000000-0000-0000-0000-000000000001', 'Brian''s Starter Template', 'Reusable template seeded for demo purposes.', '{"price": 49.99, "title": "Sample Listing Template", "quantity": 5, "categoryId": "10000000-0000-0000-0000-000000000003", "conditionId": "40000000-0000-0000-0000-000000000001"}', 'Fixed Price', 'https://picsum.photos/seed/template-2/320/180', '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing_template VALUES ('83000000-0000-0000-0000-000000000001', 'Cecilia''s Starter Template', 'Reusable template seeded for demo purposes.', '{"price": 49.99, "title": "Sample Listing Template", "quantity": 5, "categoryId": "10000000-0000-0000-0000-000000000004", "conditionId": "40000000-0000-0000-0000-000000000001"}', 'Fixed Price', 'https://picsum.photos/seed/template-3/320/180', '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);


--
-- TOC entry 4343 (class 0 OID 33435)
-- Dependencies: 406
-- Data for Name: order_buyer_feedback; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4335 (class 0 OID 33331)
-- Dependencies: 398
-- Data for Name: order_cancellation_requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_cancellation_requests VALUES ('6f1f9f0c-898f-4c7b-bb38-1b689e9f7331', 'c721f605-43cb-4b1b-8f0c-b1c5833420a9', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 0, 1, 'Realized I ordered the wrong variation, please cancel.', NULL, '2025-10-13 14:15:00+00', '2025-10-15 14:15:00+00', NULL, NULL, NULL, 0, NULL, NULL, 114.69, 'USD', '2025-10-13 14:15:00+00', 'seed', '2025-10-13 14:15:00+00', 'seed', false);
INSERT INTO public.order_cancellation_requests VALUES ('c3c25c5b-f1a3-4e5f-9ccd-da6a46b91753', '973cac8a-9be0-44a0-90b7-fd8263f8e78a', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', 2, 99, NULL, 'Order auto-cancelled after missing shipping deadline.', '2025-10-30 09:00:00+00', NULL, NULL, '2025-10-30 09:00:00+00', '2025-10-30 09:00:00+00', 5, NULL, NULL, 97.37, 'USD', '2025-10-30 09:00:00+00', 'seed', '2025-10-30 09:00:00+00', 'seed', false);
INSERT INTO public.order_cancellation_requests VALUES ('d3f7d907-6b71-47d8-8651-922629540277', '7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 0, 3, 'Need to update the delivery address; requesting cancellation.', 'Approved – refund processing with payment provider.', '2025-10-19 12:00:00+00', '2025-10-21 12:00:00+00', '2025-10-19 18:00:00+00', NULL, NULL, 2, 87.83, 'USD', 87.83, 'USD', '2025-10-19 12:00:00+00', 'seed', '2025-10-19 18:00:00+00', 'seed', false);
INSERT INTO public.order_cancellation_requests VALUES ('5d4e7a11-0c4e-4a6f-9f2f-000000000004', '0f0c1a22-11aa-4c6d-8f10-000000000018', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 0, 4, 'Item still not handed to carrier, requesting cancellation.', 'Approved – refund issued to buyer''s original payment method.', '2025-11-06 18:00:00+00', '2025-11-08 18:00:00+00', '2025-11-07 09:30:00+00', NULL, NULL, 2, 107.23, 'USD', 107.23, 'USD', '2025-11-06 18:00:00+00', 'seed', '2025-11-07 09:30:00+00', 'seed', false);
INSERT INTO public.order_cancellation_requests VALUES ('5d4e7a11-0c4e-4a6f-9f2f-000000000005', '0f0c1a22-11aa-4c6d-8f10-00000000001b', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 0, 1, 'Accidentally placed duplicate order.', 'Seller did not respond within the 3 day window. Order must be fulfilled.', '2025-11-08 11:00:00+00', '2025-11-10 11:00:00+00', '2026-03-11 01:04:18.309614+00', NULL, '2026-03-11 01:04:18.309614+00', 4, NULL, NULL, 74.43, 'USD', '2025-11-08 11:00:00+00', 'seed', '2026-03-11 01:04:22.606698+00', 'System', false);


--
-- TOC entry 4339 (class 0 OID 33382)
-- Dependencies: 402
-- Data for Name: order_item_shipments; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4328 (class 0 OID 33206)
-- Dependencies: 391
-- Data for Name: order_items; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_items VALUES ('0a3e9070-0a5e-4114-8634-8e9353a5369e', '71000000-0000-0000-0000-000000000009', NULL, 'Alice''s Item #9', 'https://picsum.photos/seed/1-9/640/640', 'DEMO-1-0009', 1, 37.99, 'USD', 37.99, 'USD', '1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4', '2025-10-20 16:00:00+00', 'seed', '2025-10-21 16:00:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d', '71000000-0000-0000-0000-000000000003', NULL, 'Alice''s Item #3', 'https://picsum.photos/seed/1-3/640/640', 'DEMO-1-0003', 2, 31.99, 'USD', 63.98, 'USD', 'f6de3ce0-2d3d-4709-923d-cbb61f956947', '2025-10-12 10:30:00+00', 'seed', '2025-10-12 10:30:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('30f2c0f3-09bb-4f52-93a9-6e98b0171c3f', '73000000-0000-0000-0000-000000000003', NULL, 'Cecilia''s Item #3', 'https://picsum.photos/seed/3-3/640/640', 'DEMO-3-0003', 1, 47.99, 'USD', 47.99, 'USD', '1e86f219-1dd0-4cac-a545-cb98e65ce429', '2025-10-28 12:10:00+00', 'seed', '2025-10-31 12:10:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('3e54a8a8-3b35-4bdf-9d09-75042c7f7d4f', '71000000-0000-0000-0000-000000000004', NULL, 'Alice''s Item #4', 'https://picsum.photos/seed/1-4/640/640', 'DEMO-1-0004', 1, 32.99, 'USD', 32.99, 'USD', 'c721f605-43cb-4b1b-8f0c-b1c5833420a9', '2025-10-15 14:15:00+00', 'seed', '2025-10-16 00:15:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('4a1ab1de-4a10-4326-a0be-5d3ab27c9df7', '72000000-0000-0000-0000-000000000002', NULL, 'Brian''s Item #2', 'https://picsum.photos/seed/2-2/640/640', 'DEMO-2-0002', 1, 38.99, 'USD', 38.99, 'USD', 'd2ee4d4a-5be0-4d76-bce6-0b8578c87407', '2025-10-22 08:20:00+00', 'seed', '2025-10-22 17:20:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('55c9f2a2-dba1-4c66-9b83-a8b4c9e7a0d4', '73000000-0000-0000-0000-000000000007', NULL, 'Cecilia''s Item #7', 'https://picsum.photos/seed/3-7/640/640', 'DEMO-3-0007', 1, 51.99, 'USD', 51.99, 'USD', 'fa236302-3864-4e54-9e40-3ebdb4749734', '2025-10-30 09:05:00+00', 'seed', '2025-11-03 09:05:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('5f2f8987-3b95-4b9f-8cc0-0f7c4b8d3b92', '72000000-0000-0000-0000-000000000003', NULL, 'Brian''s Item #3', 'https://picsum.photos/seed/2-3/640/640', 'DEMO-2-0003', 2, 39.99, 'USD', 79.98, 'USD', '973cac8a-9be0-44a0-90b7-fd8263f8e78a', '2025-10-24 11:05:00+00', 'seed', '2025-10-24 11:05:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('6bd3f47d-4f1e-467f-8797-3b2a151dd09f', '73000000-0000-0000-0000-000000000004', NULL, 'Cecilia''s Item #4', 'https://picsum.photos/seed/3-4/640/640', 'DEMO-3-0004', 2, 48.99, 'USD', 97.98, 'USD', '1e86f219-1dd0-4cac-a545-cb98e65ce429', '2025-10-28 12:10:00+00', 'seed', '2025-10-31 12:10:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('6cbb0f3e-9fd9-4c83-b181-74d3432fb953', '71000000-0000-0000-0000-000000000001', NULL, 'Alice''s Item #1', 'https://picsum.photos/seed/1-1/640/640', 'DEMO-1-0001', 1, 29.99, 'USD', 29.99, 'USD', 'f6de3ce0-2d3d-4709-923d-cbb61f956947', '2025-10-12 10:30:00+00', 'seed', '2025-10-12 10:30:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('6ccf331f-2863-411a-8f9e-1a28857e2a31', '73000000-0000-0000-0000-000000000006', NULL, 'Cecilia''s Item #6', 'https://picsum.photos/seed/3-6/640/640', 'DEMO-3-0006', 1, 50.99, 'USD', 50.99, 'USD', 'fa236302-3864-4e54-9e40-3ebdb4749734', '2025-10-30 09:05:00+00', 'seed', '2025-11-03 09:05:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('7fdde15f-acca-41c7-97a3-e1df2c6a4b8d', '71000000-0000-0000-0000-000000000008', NULL, 'Alice''s Item #8', 'https://picsum.photos/seed/1-8/640/640', 'DEMO-1-0008', 1, 36.99, 'USD', 36.99, 'USD', '1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4', '2025-10-20 16:00:00+00', 'seed', '2025-10-21 16:00:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('8fb2678e-8b5d-4d1e-b079-0fb2aa3a055c', '73000000-0000-0000-0000-000000000001', NULL, 'Cecilia''s Item #1', 'https://picsum.photos/seed/3-1/640/640', 'DEMO-3-0001', 1, 45.99, 'USD', 45.99, 'USD', 'a4206ad5-6a35-43bb-8a8c-8c7b244594ac', '2025-10-26 18:40:00+00', 'seed', '2025-10-26 18:40:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('9be4d720-31f2-4456-94d7-2bf0c76fa0ec', '73000000-0000-0000-0000-000000000002', NULL, 'Cecilia''s Item #2', 'https://picsum.photos/seed/3-2/640/640', 'DEMO-3-0002', 1, 46.99, 'USD', 46.99, 'USD', 'a4206ad5-6a35-43bb-8a8c-8c7b244594ac', '2025-10-26 18:40:00+00', 'seed', '2025-10-26 18:40:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('a3d8f848-7cf3-4058-9f09-3a78d4d64a5d', '72000000-0000-0000-0000-000000000005', NULL, 'Brian''s Item #5', 'https://picsum.photos/seed/2-5/640/640', 'DEMO-2-0005', 1, 41.99, 'USD', 41.99, 'USD', 'bd34cf77-4551-4194-ad16-d20c94b58289', '2025-10-25 15:30:00+00', 'seed', '2025-10-28 15:30:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('a9d23977-7d99-4d44-bb79-4cff5ec2f56f', '71000000-0000-0000-0000-000000000005', NULL, 'Alice''s Item #5', 'https://picsum.photos/seed/1-5/640/640', 'DEMO-1-0005', 1, 33.99, 'USD', 33.99, 'USD', 'c721f605-43cb-4b1b-8f0c-b1c5833420a9', '2025-10-15 14:15:00+00', 'seed', '2025-10-16 00:15:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('b7fe44b8-3d3a-49f0-91c5-8ed5cb0c824a', '72000000-0000-0000-0000-000000000004', NULL, 'Brian''s Item #4', 'https://picsum.photos/seed/2-4/640/640', 'DEMO-2-0004', 1, 42.99, 'USD', 42.99, 'USD', 'bd34cf77-4551-4194-ad16-d20c94b58289', '2025-10-25 15:30:00+00', 'seed', '2025-10-28 15:30:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000021', '71000000-0000-0000-0000-00000000000a', NULL, 'Alice''s Item #10', 'https://picsum.photos/seed/1-10/640/640', 'DEMO-1-0010', 1, 58.99, 'USD', 58.99, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000011', '2025-11-01 09:15:00+00', 'seed', '2025-11-01 09:50:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000022', '72000000-0000-0000-0000-000000000006', NULL, 'Brian''s Item #6', 'https://picsum.photos/seed/2-6/640/640', 'DEMO-2-0006', 1, 64.5, 'USD', 64.5, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000012', '2025-11-02 13:30:00+00', 'seed', '2025-11-02 14:10:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000023', '73000000-0000-0000-0000-000000000008', NULL, 'Cecilia''s Item #8', 'https://picsum.photos/seed/3-8/640/640', 'DEMO-3-0008', 1, 72.0, 'USD', 72.0, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000013', '2025-11-03 17:05:00+00', 'seed', '2025-11-03 17:55:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000024', '71000000-0000-0000-0000-00000000000b', NULL, 'Alice''s Item #11', 'https://picsum.photos/seed/1-11/640/640', 'DEMO-1-0011', 1, 55.75, 'USD', 55.75, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000014', '2025-11-04 08:45:00+00', 'seed', '2025-11-04 09:13:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000025', '71000000-0000-0000-0000-00000000000c', NULL, 'Alice''s Item #12', 'https://picsum.photos/seed/1-12/640/640', 'DEMO-1-0012', 1, 62.75, 'USD', 62.75, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000015', '2025-11-05 10:00:00+00', 'seed', '2025-11-05 10:30:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000026', '72000000-0000-0000-0000-000000000007', NULL, 'Brian''s Item #7', 'https://picsum.photos/seed/2-7/640/640', 'DEMO-2-0007', 1, 48.4, 'USD', 48.4, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000016', '2025-11-05 11:30:00+00', 'seed', '2025-11-05 11:55:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000027', '73000000-0000-0000-0000-000000000009', NULL, 'Cecilia''s Item #9', 'https://picsum.photos/seed/3-9/640/640', 'DEMO-3-0009', 1, 79.9, 'USD', 79.9, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000017', '2025-11-06 09:20:00+00', 'seed', '2025-11-06 09:40:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000028', '71000000-0000-0000-0000-00000000000d', NULL, 'Alice''s Item #13', 'https://picsum.photos/seed/1-13/640/640', 'DEMO-1-0013', 1, 88.6, 'USD', 88.6, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000018', '2025-11-06 13:45:00+00', 'seed', '2025-11-06 14:20:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-000000000029', '72000000-0000-0000-0000-000000000008', NULL, 'Brian''s Item #8', 'https://picsum.photos/seed/2-8/640/640', 'DEMO-2-0008', 1, 71.25, 'USD', 71.25, 'USD', '0f0c1a22-11aa-4c6d-8f10-000000000019', '2025-11-07 08:10:00+00', 'seed', '2025-11-07 08:28:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-00000000002a', '73000000-0000-0000-0000-00000000000a', NULL, 'Cecilia''s Item #10', 'https://picsum.photos/seed/3-10/640/640', 'DEMO-3-0010', 1, 65.8, 'USD', 65.8, 'USD', '0f0c1a22-11aa-4c6d-8f10-00000000001a', '2025-11-07 15:25:00+00', 'seed', '2025-11-07 15:47:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-00000000002b', '71000000-0000-0000-0000-00000000000e', NULL, 'Alice''s Item #14', 'https://picsum.photos/seed/1-14/640/640', 'DEMO-1-0014', 1, 59.1, 'USD', 59.1, 'USD', '0f0c1a22-11aa-4c6d-8f10-00000000001b', '2025-11-08 10:40:00+00', 'seed', '2025-11-08 11:07:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-00000000002c', '72000000-0000-0000-0000-000000000009', NULL, 'Brian''s Item #9', 'https://picsum.photos/seed/2-9/640/640', 'DEMO-2-0009', 1, 83.45, 'USD', 83.45, 'USD', '0f0c1a22-11aa-4c6d-8f10-00000000001c', '2025-11-08 14:05:00+00', 'seed', '2025-11-08 14:37:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-00000000002d', '73000000-0000-0000-0000-00000000000b', NULL, 'Cecilia''s Item #11', 'https://picsum.photos/seed/3-11/640/640', 'DEMO-3-0011', 1, 90.3, 'USD', 90.3, 'USD', '0f0c1a22-11aa-4c6d-8f10-00000000001d', '2025-11-09 11:15:00+00', 'seed', '2025-11-09 11:39:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c579fb6b-b172-4e17-b610-00000000002e', '71000000-0000-0000-0000-00000000000f', NULL, 'Alice''s Item #15', 'https://picsum.photos/seed/1-15/640/640', 'DEMO-1-0015', 1, 74.95, 'USD', 74.95, 'USD', '0f0c1a22-11aa-4c6d-8f10-00000000001e', '2025-11-09 17:50:00+00', 'seed', '2025-11-09 18:19:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('c5b7436e-0ae9-4265-9f2b-7a1fd7e7d78f', '71000000-0000-0000-0000-000000000006', NULL, 'Alice''s Item #6', 'https://picsum.photos/seed/1-6/640/640', 'DEMO-1-0006', 1, 34.99, 'USD', 34.99, 'USD', '7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', '2025-10-18 09:45:00+00', 'seed', '2025-10-18 09:45:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('e1d40241-43f4-4d93-b9ed-4ac8c9e52088', '72000000-0000-0000-0000-000000000001', NULL, 'Brian''s Item #1', 'https://picsum.photos/seed/2-1/640/640', 'DEMO-2-0001', 1, 37.99, 'USD', 37.99, 'USD', 'd2ee4d4a-5be0-4d76-bce6-0b8578c87407', '2025-10-22 08:20:00+00', 'seed', '2025-10-22 17:20:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('e9ad9da9-07b8-42ae-9ce2-764f76d4b657', '73000000-0000-0000-0000-000000000005', NULL, 'Cecilia''s Item #5', 'https://picsum.photos/seed/3-5/640/640', 'DEMO-3-0005', 1, 49.99, 'USD', 49.99, 'USD', 'fa236302-3864-4e54-9e40-3ebdb4749734', '2025-10-30 09:05:00+00', 'seed', '2025-11-03 09:05:00+00', 'seed', false, NULL);
INSERT INTO public.order_items VALUES ('f2a8249e-2643-49b5-bd73-0cac89fb4fc5', '71000000-0000-0000-0000-000000000007', NULL, 'Alice''s Item #7', 'https://picsum.photos/seed/1-7/640/640', 'DEMO-1-0007', 1, 35.99, 'USD', 35.99, 'USD', '7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', '2025-10-18 09:45:00+00', 'seed', '2025-10-18 09:45:00+00', 'seed', false, NULL);


--
-- TOC entry 4336 (class 0 OID 33343)
-- Dependencies: 399
-- Data for Name: order_return_requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_return_requests VALUES ('8cb7ab44-0d7d-4d7d-9b24-1cc54d4da7bf', 'bd34cf77-4551-4194-ad16-d20c94b58289', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 0, 0, 'Item color differs from the listing photos.', NULL, '2025-10-29 10:00:00+00', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 108.88, 'USD', '2025-10-29 10:00:00+00', 'seed', '2025-10-29 10:00:00+00', 'seed', false);
INSERT INTO public.order_return_requests VALUES ('dc3329e1-14fb-4d00-a395-e76e25a6822b', 'fa236302-3864-4e54-9e40-3ebdb4749734', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 4, 3, 'Shoes run smaller than expected.', 'Refunded minus restocking fee.', '2025-11-04 17:45:00+00', '2025-11-04 20:30:00+00', '2025-11-09 23:59:00+00', '2025-11-06 09:10:00+00', '2025-11-08 16:20:00+00', '2025-11-09 14:00:00+00', '2025-11-09 14:00:00+00', 'USPS', '9405511899223857264837', 5, 150.00, 'USD', 5.00, 'USD', 177.37, 'USD', '2025-11-04 17:45:00+00', 'seed', '2025-11-09 14:00:00+00', 'seed', false);
INSERT INTO public.order_return_requests VALUES ('fd21bed5-6c0c-4bcf-b099-31c8b0d08f27', '1e86f219-1dd0-4cac-a545-cb98e65ce429', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 1, 1, 'Screen arrived cracked; requesting replacement.', 'Please return using the provided UPS label.', '2025-11-01 09:00:00+00', '2025-11-01 12:00:00+00', '2025-11-05 23:59:00+00', '2025-11-03 10:15:00+00', NULL, NULL, NULL, 'UPS', '1Z999AA10123456784', 2, NULL, NULL, NULL, NULL, 169.27, 'USD', '2025-11-01 09:00:00+00', 'seed', '2025-11-03 10:15:00+00', 'seed', false);
INSERT INTO public.order_return_requests VALUES ('9a7f6b12-5e2d-4d91-8c22-000000000004', '0f0c1a22-11aa-4c6d-8f10-00000000001a', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 2, 2, 'Received the 64GB variant instead of 128GB.', 'Exchange approved once return is in transit.', '2025-11-08 09:30:00+00', '2025-11-08 12:45:00+00', '2025-11-13 23:59:00+00', '2025-11-10 10:20:00+00', NULL, NULL, NULL, 'FedEx', '612999AA10NEWRT4', 2, NULL, NULL, NULL, NULL, 80.59, 'USD', '2025-11-08 09:30:00+00', 'seed', '2025-11-10 10:20:00+00', 'seed', false);
INSERT INTO public.order_return_requests VALUES ('9a7f6b12-5e2d-4d91-8c22-000000000005', '0f0c1a22-11aa-4c6d-8f10-00000000001d', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 6, 0, 'Decided to keep a different model instead.', 'Refund pending inspection of returned item.', '2025-11-09 15:00:00+00', '2025-11-09 17:15:00+00', '2025-11-14 23:59:00+00', '2025-11-11 08:40:00+00', '2025-11-13 16:05:00+00', NULL, NULL, 'USPS', '9405511899223857264999', 4, NULL, NULL, NULL, NULL, 112.57, 'USD', '2025-11-09 15:00:00+00', 'seed', '2025-11-13 16:05:00+00', 'seed', false);


--
-- TOC entry 4329 (class 0 OID 33218)
-- Dependencies: 392
-- Data for Name: order_shipping_labels; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4330 (class 0 OID 33230)
-- Dependencies: 393
-- Data for Name: order_status_histories; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_status_histories VALUES ('bfe7587a-409f-4456-b895-4c1e07d467ec', '0f0c1a22-11aa-4c6d-8f10-000000000019', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789812+00');
INSERT INTO public.order_status_histories VALUES ('a26baa41-60e0-422c-8888-aa733b1a72ed', '0f0c1a22-11aa-4c6d-8f10-00000000001d', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789824+00');
INSERT INTO public.order_status_histories VALUES ('6f979331-d89e-4954-a836-643cdffb649d', '0f0c1a22-11aa-4c6d-8f10-000000000013', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789793+00');
INSERT INTO public.order_status_histories VALUES ('8a191df9-68ee-4176-8d45-23b8a154b56b', '0f0c1a22-11aa-4c6d-8f10-000000000018', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789809+00');
INSERT INTO public.order_status_histories VALUES ('ad936d4b-cc68-4e8f-b287-360f47c939d7', '0f0c1a22-11aa-4c6d-8f10-000000000017', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789806+00');
INSERT INTO public.order_status_histories VALUES ('8d2587f1-43c2-4c1a-a664-60513b0aa925', '0f0c1a22-11aa-4c6d-8f10-00000000001c', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.78982+00');
INSERT INTO public.order_status_histories VALUES ('476899a0-9d36-46f2-af3e-3c3607caf5b1', '0f0c1a22-11aa-4c6d-8f10-000000000016', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789805+00');
INSERT INTO public.order_status_histories VALUES ('55270566-1f49-475e-9d00-e459e28c853a', '7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789834+00');
INSERT INTO public.order_status_histories VALUES ('a44a184a-0922-4bf4-b67e-d63f098781f9', 'f6de3ce0-2d3d-4709-923d-cbb61f956947', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789839+00');
INSERT INTO public.order_status_histories VALUES ('19df6c75-5dbd-4fef-a3f5-45b491d5ccd7', '0f0c1a22-11aa-4c6d-8f10-000000000015', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789803+00');
INSERT INTO public.order_status_histories VALUES ('b5bf42a4-9de0-4d12-a6ba-70a254c6c60d', '0f0c1a22-11aa-4c6d-8f10-00000000001a', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789815+00');
INSERT INTO public.order_status_histories VALUES ('32f700aa-d94f-4d87-8b0d-0eb4b98e8125', '0f0c1a22-11aa-4c6d-8f10-000000000012', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789772+00');
INSERT INTO public.order_status_histories VALUES ('d7c7c00a-0f28-44d9-95fd-180ffed832f1', '0f0c1a22-11aa-4c6d-8f10-00000000001b', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789818+00');
INSERT INTO public.order_status_histories VALUES ('423179e0-9e42-47d2-bf3c-169d20e865c9', '0f0c1a22-11aa-4c6d-8f10-000000000011', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.788202+00');
INSERT INTO public.order_status_histories VALUES ('17571f30-33dc-4527-9c9c-270ab601bbad', '0f0c1a22-11aa-4c6d-8f10-000000000014', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789798+00');
INSERT INTO public.order_status_histories VALUES ('49f5d786-4c31-4535-bdc6-6dc2c0896c15', '0f0c1a22-11aa-4c6d-8f10-00000000001e', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '2026-03-11 02:00:07.789828+00');


--
-- TOC entry 4322 (class 0 OID 33123)
-- Dependencies: 385
-- Data for Name: order_status_transitions; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_status_transitions VALUES ('2abdffad-037d-48a0-8c3d-a8dd0f00c5ba', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 'SELLER,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('3334f1c8-0fb7-4b17-974a-16f4f492ade4', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('3cf5a7f5-8f3f-4dcb-907e-e4d27744ef98', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', 'SELLER,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('42059f6f-8e43-4b6a-9b59-cf9670091b8f', '4d128ab1-64a7-4c65-b8f5-434a258f0c52', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('55b5fadc-7f2f-4f43-ac4c-c6eb6f633d58', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 'SELLER,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('5c76de08-97eb-43d5-9c01-8c7c6262ec66', '4d128ab1-64a7-4c65-b8f5-434a258f0c52', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', 'SELLER,BUYER,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('64648c83-2c87-47b8-8c2a-32e96c369f41', '859b47f4-0d05-4f43-8ff5-57acb8d5da1d', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 'SELLER,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('6cb6fa65-3d6c-45f0-9f27-cf5d292743ff', '970c8d97-6081-43db-9083-8f3c026ded84', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'SELLER,SUPPORT,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('6fbe36c4-98e4-4d1d-8c3c-1ea29fe8d08c', '5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('8ac18f4b-ea8d-4b72-b6cf-01c3d233cbea', '2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1', '4d128ab1-64a7-4c65-b8f5-434a258f0c52', 'SELLER,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('94b12ce3-6d7c-4ea1-86f9-72f65e75d8de', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '859b47f4-0d05-4f43-8ff5-57acb8d5da1d', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('a4c5df71-b5bb-4f13-9659-a5047cf4f087', '970c8d97-6081-43db-9083-8f3c026ded84', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', 'SELLER,SUPPORT,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('b62c4a77-6a54-47d9-8d09-af22bd0caf23', '2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', 'SELLER,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('b8fa2c60-13ad-4e83-9516-8f406bcf8414', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '970c8d97-6081-43db-9083-8f3c026ded84', 'SELLER,SUPPORT,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('c6a927ee-4fb6-48cc-bbf0-d2624de3458f', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('ce68729c-6df0-466b-ae26-737d1b10dd93', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930', '0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('d0cb2575-023a-45dc-840a-8e09b2f4c4c8', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('d10a4517-efbb-4b8d-af6f-baf2b022a850', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('ee0a6840-bf0b-46f3-9c41-96b5a91a02ab', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 'SELLER,SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('f5ceb762-3d65-4f6d-b052-053c55c1a08d', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', '0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('1bd31fd1-5a79-4a8e-9035-7cbc71dbb8b9', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('5a3f5769-6c6d-4b89-9347-118bd3fba3d6', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('7cf6e659-8025-49e8-94d5-3a4dd3b5a793', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', 'SYSTEM');
INSERT INTO public.order_status_transitions VALUES ('8c6f6f3e-18c6-4aa5-ba61-033fa3c0bb0e', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'SYSTEM');


--
-- TOC entry 4308 (class 0 OID 33015)
-- Dependencies: 371
-- Data for Name: order_statuses; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_statuses VALUES ('0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e', 'Archived', 'Archived', 'Order archived', '#64748b', 11);
INSERT INTO public.order_statuses VALUES ('2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1', 'Draft', 'Draft', 'Order created but not submitted', '#94a3b8', 0);
INSERT INTO public.order_statuses VALUES ('3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', 'AwaitingShipmentShipWithin24h', 'Ship within 24h', 'Must ship within 24 hours', '#fbbf24', 4);
INSERT INTO public.order_statuses VALUES ('4d128ab1-64a7-4c65-b8f5-434a258f0c52', 'AwaitingPayment', 'Awaiting payment', 'Order awaits buyer payment', '#fb923c', 1);
INSERT INTO public.order_statuses VALUES ('5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 'PaidAndShipped', 'Paid & shipped', 'Order shipped to buyer', '#10b981', 6);
INSERT INTO public.order_statuses VALUES ('5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9', 'PaidAwaitingFeedback', 'Awaiting feedback', 'Waiting for buyer feedback', '#a855f7', 7);
INSERT INTO public.order_statuses VALUES ('859b47f4-0d05-4f43-8ff5-57acb8d5da1d', 'AwaitingExpeditedShipment', 'Expedited shipment', 'Expedited shipping requested', '#22c55e', 5);
INSERT INTO public.order_statuses VALUES ('970c8d97-6081-43db-9083-8f3c026ded84', 'DeliveryFailed', 'Delivery failed', 'Delivery attempt unsuccessful', '#f97316', 10);
INSERT INTO public.order_statuses VALUES ('9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'AwaitingShipment', 'Awaiting shipment', 'Payment received; waiting to ship', '#3b82f6', 2);
INSERT INTO public.order_statuses VALUES ('ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', 'Cancelled', 'Cancelled', 'Order cancelled', '#ef4444', 12);
INSERT INTO public.order_statuses VALUES ('c21a6b64-f0e9-4947-8b1b-38ef45aa4930', 'ShippedAwaitingFeedback', 'Shipped - awaiting feedback', 'Shipped and awaiting buyer confirmation', '#38bdf8', 8);
INSERT INTO public.order_statuses VALUES ('dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 'AwaitingShipmentOverdue', 'Shipment overdue', 'Shipment overdue based on handling time', '#ef4444', 3);


--
-- TOC entry 4325 (class 0 OID 33162)
-- Dependencies: 388
-- Data for Name: orders; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.orders VALUES ('c721f605-43cb-4b1b-8f0c-b1c5833420a9', 'ORD-SEED-1002', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 66.98, 'USD', 12.0, 'USD', 3.35, 'USD', 5.36, 'USD', 5.0, 'USD', 82.69, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2025-10-15 14:15:00+00', '2025-10-15 15:15:00+00', '2025-10-16 00:15:00+00', NULL, NULL, NULL, 'OCTDEAL', NULL, '2025-10-15 14:15:00+00', 'seed', '2025-10-16 00:15:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('1e86f219-1dd0-4cac-a545-cb98e65ce429', 'ORD-SEED-1009', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 145.97, 'USD', 14.0, 'USD', 6.2, 'USD', 10.6, 'USD', 7.5, 'USD', 169.27, 'USD', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930', 2, 0, '2025-10-28 12:10:00+00', '2025-10-28 13:10:00+00', '2025-10-28 22:10:00+00', '2025-10-31 12:10:00+00', NULL, NULL, 'HOLIDAY10', NULL, '2025-10-28 12:10:00+00', 'seed', '2025-10-31 12:10:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4', 'ORD-SEED-1004', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 74.98, 'USD', 0.0, 'USD', 3.25, 'USD', 5.5, 'USD', 0.0, 'USD', 83.73, 'USD', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', 6, 0, '2025-10-20 16:00:00+00', NULL, NULL, NULL, NULL, '2025-10-21 16:00:00+00', NULL, NULL, '2025-10-20 16:00:00+00', 'seed', '2025-10-21 16:00:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('973cac8a-9be0-44a0-90b7-fd8263f8e78a', 'ORD-SEED-1006', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', 79.98, 'USD', 11.0, 'USD', 3.99, 'USD', 6.4, 'USD', 4.0, 'USD', 97.37, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-10-24 11:05:00+00', '2025-10-24 11:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-10-24 11:05:00+00', 'seed', '2025-10-24 11:50:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('a4206ad5-6a35-43bb-8a8c-8c7b244594ac', 'ORD-SEED-1008', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 92.98, 'USD', 13.0, 'USD', 4.6, 'USD', 8.6, 'USD', 0.0, 'USD', 119.18, 'USD', '859b47f4-0d05-4f43-8ff5-57acb8d5da1d', 0, 0, '2025-10-26 18:40:00+00', '2025-10-26 19:40:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-10-26 18:40:00+00', 'seed', '2025-10-26 19:40:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('bd34cf77-4551-4194-ad16-d20c94b58289', 'ORD-SEED-1007', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 84.98, 'USD', 12.5, 'USD', 4.2, 'USD', 7.2, 'USD', 0.0, 'USD', 108.88, 'USD', '5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9', 5, 0, '2025-10-25 15:30:00+00', '2025-10-25 17:30:00+00', '2025-10-26 15:30:00+00', '2025-10-28 15:30:00+00', NULL, NULL, NULL, NULL, '2025-10-25 15:30:00+00', 'seed', '2025-10-28 15:30:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('d2ee4d4a-5be0-4d76-bce6-0b8578c87407', 'ORD-SEED-1005', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 76.98, 'USD', 10.0, 'USD', 4.1, 'USD', 6.16, 'USD', 0.0, 'USD', 97.24, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2025-10-22 08:20:00+00', '2025-10-22 09:20:00+00', '2025-10-22 17:20:00+00', NULL, NULL, NULL, NULL, NULL, '2025-10-22 08:20:00+00', 'seed', '2025-10-22 17:20:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('fa236302-3864-4e54-9e40-3ebdb4749734', 'ORD-SEED-1010', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 152.97, 'USD', 15.0, 'USD', 7.2, 'USD', 12.2, 'USD', 10.0, 'USD', 177.37, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2025-10-30 09:05:00+00', '2025-10-30 10:05:00+00', '2025-10-30 22:05:00+00', '2025-11-03 09:05:00+00', NULL, NULL, 'BULKBUY', NULL, '2025-10-30 09:05:00+00', 'seed', '2025-11-03 09:05:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000011', 'ORD-SEED-1011', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 58.99, 'USD', 9.95, 'USD', 3.1, 'USD', 5.3, 'USD', 0.0, 'USD', 77.34, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-01 09:15:00+00', '2025-11-01 09:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-01 09:15:00+00', 'seed', '2026-03-11 02:00:07.8521+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000012', 'ORD-SEED-1012', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 64.5, 'USD', 8.25, 'USD', 3.45, 'USD', 4.86, 'USD', 2.5, 'USD', 78.56, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-02 13:30:00+00', '2025-11-02 14:10:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-02 13:30:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000013', 'ORD-SEED-1013', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 72.0, 'USD', 10.0, 'USD', 3.9, 'USD', 6.12, 'USD', 0.0, 'USD', 92.02, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-03 17:05:00+00', '2025-11-03 17:55:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-03 17:05:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000014', 'ORD-SEED-1014', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 55.75, 'USD', 7.8, 'USD', 2.95, 'USD', 4.46, 'USD', 1.2, 'USD', 69.76, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-04 08:45:00+00', '2025-11-04 09:13:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-04 08:45:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000015', 'ORD-SEED-1015', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 62.75, 'USD', 8.25, 'USD', 3.15, 'USD', 5.02, 'USD', 2.5, 'USD', 76.67, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-05 10:00:00+00', '2025-11-05 10:30:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-05 10:00:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000016', 'ORD-SEED-1016', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 48.4, 'USD', 7.5, 'USD', 2.45, 'USD', 3.87, 'USD', 0.0, 'USD', 62.22, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-05 11:30:00+00', '2025-11-05 11:55:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-05 11:30:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000017', 'ORD-SEED-1017', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 79.9, 'USD', 9.95, 'USD', 3.95, 'USD', 6.39, 'USD', 0.0, 'USD', 100.19, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-06 09:20:00+00', '2025-11-06 09:40:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-06 09:20:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000018', 'ORD-SEED-1018', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 88.6, 'USD', 10.25, 'USD', 4.3, 'USD', 7.08, 'USD', 3.0, 'USD', 107.23, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-06 13:45:00+00', '2025-11-06 14:20:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-06 13:45:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000019', 'ORD-SEED-1019', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', 71.25, 'USD', 8.75, 'USD', 3.55, 'USD', 5.7, 'USD', 0.0, 'USD', 89.25, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-07 08:10:00+00', '2025-11-07 08:28:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-07 08:10:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001a', 'ORD-SEED-1020', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 65.8, 'USD', 8.4, 'USD', 3.25, 'USD', 4.94, 'USD', 1.8, 'USD', 80.59, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-07 15:25:00+00', '2025-11-07 15:47:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-07 15:25:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001b', 'ORD-SEED-1021', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 59.1, 'USD', 7.95, 'USD', 2.95, 'USD', 4.43, 'USD', 0.0, 'USD', 74.43, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-08 10:40:00+00', '2025-11-08 11:07:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-08 10:40:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001c', 'ORD-SEED-1022', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 83.45, 'USD', 9.1, 'USD', 3.8, 'USD', 6.68, 'USD', 2.2, 'USD', 100.83, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-08 14:05:00+00', '2025-11-08 14:37:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-08 14:05:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001d', 'ORD-SEED-1023', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 90.3, 'USD', 10.6, 'USD', 4.45, 'USD', 7.22, 'USD', 0.0, 'USD', 112.57, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-09 11:15:00+00', '2025-11-09 11:39:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-09 11:15:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001e', 'ORD-SEED-1024', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 74.95, 'USD', 8.9, 'USD', 3.6, 'USD', 5.83, 'USD', 1.5, 'USD', 91.78, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-09 17:50:00+00', '2025-11-09 18:19:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-09 17:50:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);
INSERT INTO public.orders VALUES ('7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', 'ORD-SEED-1003', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 70.98, 'USD', 9.25, 'USD', 3.4, 'USD', 6.2, 'USD', 2.0, 'USD', 87.83, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-10-18 09:45:00+00', '2025-10-18 10:45:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-10-18 09:45:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);
INSERT INTO public.orders VALUES ('f6de3ce0-2d3d-4709-923d-cbb61f956947', 'ORD-SEED-1001', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 93.97, 'USD', 8.5, 'USD', 4.7, 'USD', 7.52, 'USD', 0.0, 'USD', 114.69, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-10-12 10:30:00+00', '2025-10-12 12:30:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-10-12 10:30:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);


--
-- TOC entry 4309 (class 0 OID 33022)
-- Dependencies: 372
-- Data for Name: otp; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.otp VALUES ('4cfa3900-bb5e-4502-a926-7fd68df8b96f', 'vtrgiangg2903@gmail.com', '964655', '2026-03-11 01:10:45.112721+00', true, 1, '2026-03-11 01:05:45.151092+00', 'System', '2026-03-11 01:06:14.236758+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', false);
INSERT INTO public.otp VALUES ('a608a141-6223-4bd1-a4f1-60846850903e', 'vtrgiangg2903@gmail.com', '830981', '2026-03-11 01:26:26.570142+00', true, 4, '2026-03-11 01:21:26.588662+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2026-03-11 01:22:14.31811+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', false);
INSERT INTO public.otp VALUES ('2751e22a-8406-4edd-bb8e-45ac3307c910', 'thinhndhe186687@fpt.edu.vn', '399464', '2026-03-14 15:04:39.260273+00', true, 1, '2026-03-14 14:59:39.303181+00', 'System', '2026-03-14 15:00:12.953522+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false);
INSERT INTO public.otp VALUES ('44cc55cd-ffeb-4c04-8c55-ef9f5d2d4c56', 'thinhndhe186687@fpt.edu.vn', '991737', '2026-03-14 15:05:42.465747+00', false, 4, '2026-03-14 15:00:42.466291+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.otp VALUES ('53a71f44-f059-4413-a56e-698632b001cc', 'hoapoki24@gmail.com', '310704', '2026-03-17 01:36:12.869019+00', true, 1, '2026-03-17 01:31:12.897927+00', 'System', '2026-03-17 01:31:42.888631+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.otp VALUES ('833cb503-5b04-4ce7-aba0-935bff819846', 'hoapoki24@gmail.com', '094545', '2026-03-17 01:37:36.914526+00', false, 4, '2026-03-17 01:32:36.914758+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', NULL, NULL, false);
INSERT INTO public.otp VALUES ('2509f23d-73ca-4d88-915e-1d74aef8e704', 'ngochai0217@gmail.com', '556723', '2026-03-17 02:33:27.087766+00', false, 1, '2026-03-17 02:28:27.128409+00', 'System', NULL, NULL, false);
INSERT INTO public.otp VALUES ('f44bf4c6-c310-407c-a001-ef8695718ffc', 'vietbhhe187397@fpt.edu.vn', '145069', '2026-03-17 04:13:55.57346+00', true, 1, '2026-03-17 04:08:55.59351+00', 'System', '2026-03-17 04:09:46.122307+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', false);
INSERT INTO public.otp VALUES ('906440ea-6df9-43cb-9481-2d9bfe096d74', 'vietbhhe187397@fpt.edu.vn', '792660', '2026-03-17 04:14:56.715328+00', true, 4, '2026-03-17 04:09:56.71569+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', '2026-03-17 04:10:20.743396+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', false);
INSERT INTO public.otp VALUES ('fa04b93d-7541-432d-b2d9-db5908c70354', 'test@example.com', '370263', '2026-03-17 04:28:56.368443+00', false, 1, '2026-03-17 04:23:56.371112+00', 'System', NULL, NULL, false);


--
-- TOC entry 4310 (class 0 OID 33029)
-- Dependencies: 373
-- Data for Name: outbox_message; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.outbox_message VALUES ('305d75f6-d558-43a7-ab1e-a628a93475c0', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"038a4659-83c4-456d-9bf9-e2036a53ad6c"},"FullName":"Vu Truong Giang","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"vtrgiangg2903@gmail.com"},"OccurredOn":"2026-03-11T01:05:42.4484174Z"}', '2026-03-11 01:05:42.448417+00', '2026-03-11 01:05:45.514331+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('30342f81-b314-49f6-bf39-93919af8a584', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"vtrgiangg2903@gmail.com","Code":"964655","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-11T01:05:45.1128542Z"}', '2026-03-11 01:05:45.112854+00', '2026-03-11 01:06:00.324831+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('35f4898e-0b12-4046-9725-d7f59f5798bc', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"vtrgiangg2903@gmail.com","Code":"830981","ExpiresInMinutes":5,"Type":4,"OccurredOn":"2026-03-11T01:21:26.5702717Z"}', '2026-03-11 01:21:26.570271+00', '2026-03-11 01:21:36.117742+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('95835112-02c0-4dd8-afe9-8c1d78f7f32b', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"c483385e-f1b9-40b4-9b95-e18f45e06a82"},"FullName":"Nguyễn Đức Thịnh","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"thinhndhe186687@fpt.edu.vn"},"OccurredOn":"2026-03-14T14:59:36.1090003Z"}', '2026-03-14 14:59:36.109+00', '2026-03-14 14:59:39.458196+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('4f90c1f1-92b4-41fb-9e77-4b74fd98e96e', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"thinhndhe186687@fpt.edu.vn","Code":"399464","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-14T14:59:39.2605258Z"}', '2026-03-14 14:59:39.260525+00', '2026-03-14 14:59:55.610957+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('0dbc64be-66b3-4b09-a507-eccad1ccc962', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"thinhndhe186687@fpt.edu.vn","Code":"991737","ExpiresInMinutes":5,"Type":4,"OccurredOn":"2026-03-14T15:00:42.4657502Z"}', '2026-03-14 15:00:42.46575+00', NULL, 3, 'MOCEAN API returned BadRequest: <?xml version="1.0"?><result><messages><message><status>4</status><receiver>84868169332</receiver><msgid>longriver1230314230216905049.0005</msgid><err_msg>Destination+number+is+not+whitelisted</err_msg></message></messages></result>');
INSERT INTO public.outbox_message VALUES ('974caa0f-f415-4846-8287-9556231bf05c', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c"},"FullName":"Do Huu Hoa","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"hoapoki24@gmail.com"},"OccurredOn":"2026-03-17T01:31:10.1572726Z"}', '2026-03-17 01:31:10.157272+00', '2026-03-17 01:31:13.336733+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('1b85c928-3a96-4677-86dd-35f927a663c6', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"hoapoki24@gmail.com","Code":"310704","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-17T01:31:12.8692528Z"}', '2026-03-17 01:31:12.869252+00', '2026-03-17 01:31:27.981206+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('0b0cb983-602f-4138-a018-777c6acc0880', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"hoapoki24@gmail.com","Code":"094545","ExpiresInMinutes":5,"Type":4,"OccurredOn":"2026-03-17T01:32:36.9145287Z"}', '2026-03-17 01:32:36.914528+00', NULL, 3, 'MOCEAN API returned BadRequest: <?xml version="1.0"?><result><messages><message><status>4</status><receiver>84362982702</receiver><msgid>longriver1230317093047575040.0003</msgid><err_msg>Destination+number+is+not+whitelisted</err_msg></message></messages></result>');
INSERT INTO public.outbox_message VALUES ('e3393e08-4551-423f-9af7-b09feaa6c90d', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"ngochai0217@gmail.com","Code":"556723","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-17T02:28:27.0879962Z"}', '2026-03-17 02:28:27.087996+00', '2026-03-17 02:26:23.765806+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('4acf1bda-1dae-4d23-a624-ac93fb3277fd', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"e8f83430-5d4d-4ac2-bb82-46797049535e"},"FullName":"Le Ngoc Hai","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"ngochai0217@gmail.com"},"OccurredOn":"2026-03-17T02:26:04.7690208Z"}', '2026-03-17 02:26:04.76902+00', '2026-03-17 02:28:27.564454+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('40cc7c0d-5422-4131-9ab3-0fb15296d936', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"e167368a-7042-45a4-8f1f-badba56b6b63"},"FullName":"Bui Hoang Viet","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"vietbhhe187397@fpt.edu.vn"},"OccurredOn":"2026-03-17T04:08:44.8990581Z"}', '2026-03-17 04:08:44.899058+00', '2026-03-17 04:08:55.954971+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('3ea72612-5643-4c9d-b51d-25fdfa836a3c', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"vietbhhe187397@fpt.edu.vn","Code":"145069","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-17T04:08:55.5735982Z"}', '2026-03-17 04:08:55.573598+00', '2026-03-17 04:09:09.375976+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('a4e58c65-bc31-47b9-b1ba-b2bd0339e83c', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"vietbhhe187397@fpt.edu.vn","Code":"792660","ExpiresInMinutes":5,"Type":4,"OccurredOn":"2026-03-17T04:09:56.7153318Z"}', '2026-03-17 04:09:56.715331+00', '2026-03-17 04:10:06.186149+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('3822012a-36d1-41b3-b12b-3c4b88f1f6d8', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"741eb167-fb61-4b47-a35a-90720c4949f7"},"FullName":"Test User","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"test@example.com"},"OccurredOn":"2026-03-17T04:23:53.4126092Z"}', '2026-03-17 04:23:53.412609+00', '2026-03-17 04:23:56.669789+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('92a9bca6-7184-4046-bb18-8adf461d413d', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"test@example.com","Code":"370263","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-17T04:23:56.3684604Z"}', '2026-03-17 04:23:56.36846+00', '2026-03-17 04:24:11.664227+00', 0, NULL);


--
-- TOC entry 4326 (class 0 OID 33179)
-- Dependencies: 389
-- Data for Name: refresh_token; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.refresh_token VALUES ('2e9bf52f-ed92-4399-a4d3-db65396ac023', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'hw0I/hZt5DTF3eIBE4qIQBIFIr3bo5YprUnEOZdxHKE=', '2026-03-18 01:05:42.604114+00', '2026-03-11 01:05:42.625854+00', 'System', '2026-03-11 01:21:12.421125+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('6ed4239c-b452-4d8d-a682-85ea1f1cc599', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2lZUya80ETAcuDOeRp2Mne66SoFX0q15Ma6hcaSpUrQ=', '2026-03-18 01:21:12.383606+00', '2026-03-11 01:21:12.421063+00', 'System', '2026-03-11 01:32:49.884558+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('e132e848-3feb-43c0-969c-ebf1f723038e', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'YIu05zs+29kmDGhts/WKb+Ymrtgpl39dkNUQABWdpBc=', '2026-03-18 01:32:49.883688+00', '2026-03-11 01:32:49.884549+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('ef3e5cbf-9d4f-412c-b691-f61b7ad5d836', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Cc5FUZ6WFAvgBJs0JKFErDoKAM021Pi7rS4LkegmVOE=', '2026-03-21 14:59:36.428945+00', '2026-03-14 14:59:36.533153+00', 'System', '2026-03-14 15:20:30.498896+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('1797ca00-783e-432e-8d86-30f5d3a673c7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'nG0BD7yXKSn7uZEOo7QfkkqhoGNOeC2MFFioRXPdmic=', '2026-03-21 15:20:30.498178+00', '2026-03-14 15:20:30.498877+00', 'System', '2026-03-14 15:36:09.860586+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('7470216d-4d44-41f6-8e6c-60e52952b15b', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'm3K4DrzopC4g/ERewetd2rHe7RaiBHS1+rSv/Wa0LEE=', '2026-03-21 15:36:09.765652+00', '2026-03-14 15:36:09.860457+00', 'System', '2026-03-14 15:48:56.487038+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('da94e9fd-ebe6-48d6-88b2-16db3f88f6c9', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'YO8em3j0mzQ1TJ8TkOpP3VsrNGbIcoMARHO0LEe/pww=', '2026-03-21 15:48:56.416144+00', '2026-03-14 15:48:56.486907+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('6d781e42-aea8-4db4-9f07-9ebe0ba53afe', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'V9mBCXTnIZAagQxMkEAehSaRNhobOs5sZeLhZkMHsNQ=', '2026-03-21 16:01:19.331284+00', '2026-03-14 16:01:19.426591+00', 'System', '2026-03-14 16:24:39.313178+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('90c178bc-6c2c-4528-b186-7c704a3720ad', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'YeFoFmBPYjaEXGECoxzXd4yzUHQF8FbdkSfDJ1fJsmQ=', '2026-03-21 16:24:39.312325+00', '2026-03-14 16:24:39.313158+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('dcac28a1-32b4-4e70-8930-49855bc1c2cd', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'ELTRq00rbTYLbBkQLeAgRgYRdYfvrOFjmkechZ/q/dE=', '2026-03-21 16:50:58.299327+00', '2026-03-14 16:50:58.399389+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c6c7f802-4e54-4939-bd2c-ebd4ebbd09a3', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'pORmnniYZvZpggtfyP/I6X6XdDk3RLafRmxAflaYF7Y=', '2026-03-22 09:24:25.731633+00', '2026-03-15 09:24:25.810809+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('b579e78d-ca87-4c9f-afe6-64b0699e81b7', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'a5Zn30lLgjTVIbpfkJFEZaq7yr4KAZn/g7yhXGsITZE=', '2026-03-24 01:31:10.350773+00', '2026-03-17 01:31:10.426928+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('0831c8f7-0045-4dd7-a012-37fb9a8db39a', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '8q9MOBQHLwKUmalTAA0JUmX5pxcUiuF0Kd1eeTlsfnw=', '2026-03-24 02:26:11.075529+00', '2026-03-17 02:26:11.147692+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('936541d2-6c31-4593-9989-26586ff7e0c2', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'z1OfEm00awKo6kYwWN7ealILQZvWfmzFYjRcNH00+ck=', '2026-03-24 02:27:50.921568+00', '2026-03-17 02:27:50.969437+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('070438b8-0861-4e9c-8717-49675cefef22', 'e8f83430-5d4d-4ac2-bb82-46797049535e', 'pPZWsmbjRwYXuQ5WnRCTKB2fR8pms44wPUkQR+jUdEk=', '2026-03-24 02:26:04.927875+00', '2026-03-17 02:26:04.977455+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('0b8b7961-e982-4507-b545-ef874ec6b38c', 'e167368a-7042-45a4-8f1f-badba56b6b63', 'oE6lWAYLOWs2v2IoXGx1L6S0OBK2QJMizXMm0gum3Ko=', '2026-03-24 04:08:44.975664+00', '2026-03-17 04:08:45.02803+00', 'System', '2026-03-17 04:20:21.530104+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('b5969b45-7549-4531-9d1a-6769098936dc', 'e167368a-7042-45a4-8f1f-badba56b6b63', 'EdjQSHcHMaylT+U2DcnVYNfMb0ic/ERUydBNkvyhTk0=', '2026-03-24 04:20:21.528811+00', '2026-03-17 04:20:21.530085+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('ccc099ae-86bb-4679-9386-08cce8ba4bb5', '741eb167-fb61-4b47-a35a-90720c4949f7', 'F7Wn/bOo72KeTeTU0SqUHZSPL4oUKWreIrE/AuJ1pIU=', '2026-03-24 04:23:53.41569+00', '2026-03-17 04:23:53.417406+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('a87d6606-7bd0-4b1a-aa76-5df904b23067', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'x2XKnml7MooOdvhW3GG4tNvrzJsF+f8TZsQMRG8rUvQ=', '2026-03-24 04:25:47.671606+00', '2026-03-17 04:25:47.67259+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f81b380a-a4b9-498b-ab67-3f8718a87b76', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'NcezSGiVcwX/mM+WnLOMcm3pM40GYpAKKJ+TdlXX5AI=', '2026-03-24 05:35:17.839104+00', '2026-03-17 05:35:17.840444+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('ae6dfca0-dbf7-4463-8d8b-114ae3608157', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'Khm6CxIQ6sMEdmW0j4g7Mfa/QxVGnN91MtpoSN+OF00=', '2026-03-24 05:42:44.750132+00', '2026-03-17 05:42:44.812049+00', 'System', NULL, NULL, false);


--
-- TOC entry 4337 (class 0 OID 33359)
-- Dependencies: 400
-- Data for Name: report_downloads; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4338 (class 0 OID 33366)
-- Dependencies: 401
-- Data for Name: report_schedules; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4357 (class 0 OID 34865)
-- Dependencies: 420
-- Data for Name: research_saved_category; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4331 (class 0 OID 33291)
-- Dependencies: 394
-- Data for Name: return_policy; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.return_policy VALUES ('8d1b7029-92ef-4ae8-b84d-df2d9f71b940', '65987e62-8992-4f8f-bb21-1e3eea0f453c', true, 30, 0, 0, '2026-03-17 04:13:56.529302+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', NULL, NULL, false);
INSERT INTO public.return_policy VALUES ('356b05e0-f5cb-47d6-9839-b66da0d280f9', 'e43d2fb2-d880-465d-87b6-3f72f60ece73', true, 30, 0, 0, '2026-03-17 05:36:33.170373+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false);


--
-- TOC entry 4351 (class 0 OID 33548)
-- Dependencies: 414
-- Data for Name: review; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4311 (class 0 OID 33036)
-- Dependencies: 374
-- Data for Name: role; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4323 (class 0 OID 33140)
-- Dependencies: 386
-- Data for Name: role_permissions; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4327 (class 0 OID 33191)
-- Dependencies: 390
-- Data for Name: role_user; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4344 (class 0 OID 33452)
-- Dependencies: 407
-- Data for Name: sale_event; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4345 (class 0 OID 33459)
-- Dependencies: 408
-- Data for Name: sale_event_discount_tier; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4346 (class 0 OID 33469)
-- Dependencies: 409
-- Data for Name: sale_event_listing; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4348 (class 0 OID 33509)
-- Dependencies: 411
-- Data for Name: seller_blocked_buyer; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4349 (class 0 OID 33520)
-- Dependencies: 412
-- Data for Name: seller_exempt_buyer; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4347 (class 0 OID 33487)
-- Dependencies: 410
-- Data for Name: seller_preference; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4332 (class 0 OID 33298)
-- Dependencies: 395
-- Data for Name: shipping_policy; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.shipping_policy VALUES ('d559b079-a7b8-4247-a2c4-25d8e50c2052', '65987e62-8992-4f8f-bb21-1e3eea0f453c', 22.00, 'USD', 1, false, '2026-03-17 04:13:50.90275+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', NULL, NULL, false, 'eBay', 'ABC');
INSERT INTO public.shipping_policy VALUES ('e507cbe7-4fc1-4e8a-a1d2-9a6e39fb0737', 'e43d2fb2-d880-465d-87b6-3f72f60ece73', 222.00, 'USD', 1, false, '2026-03-17 05:36:28.256075+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, 'eBay', 'ABC');


--
-- TOC entry 4312 (class 0 OID 33043)
-- Dependencies: 375
-- Data for Name: shipping_services; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.shipping_services VALUES ('5a4af094-9a6b-4d6f-9a19-9b5360f0a6ec', 'UPS', 'UPS Ground', 15.62, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 28 - Apr 2', false, 6, 3, 'Reliable ground service - Includes tracking', true, 'On eBay you save 21%', 'UPS_GROUND', 'ups-ground', false, '2025-01-01 00:00:00+00', 'seed');
INSERT INTO public.shipping_services VALUES ('6f7e3c0f-2bc6-4f1b-aa0b-4c1a9f76f950', 'USPS', 'USPS Ground Advantage', 11.45, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 28 - Apr 1', false, 5, 3, 'Max weight 70 lb - Max dimensions 130" (length + girth)', false, 'On eBay you save 28%', 'USPS_GROUND_ADVANTAGE', 'usps-ground', true, '2025-01-01 00:00:00+00', 'seed');
INSERT INTO public.shipping_services VALUES ('9e1f84fd-8c9c-459d-b2c5-bf6e47668f5d', 'FedEx', 'FedEx Ground Economy', 14.1, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 29 - Apr 3', false, 7, 4, '2-5 business days - Ideal for small parcels', true, 'On eBay you save 18%', 'FEDEX_GROUND_ECONOMY', 'fedex-ground', false, '2025-01-01 00:00:00+00', 'seed');
INSERT INTO public.shipping_services VALUES ('a1d9551e-5c5c-4ca6-9a0e-1aa855b77af7', 'USPS', 'USPS Priority Mail Flat Rate Legal Envelope', 9.05, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 27 - 31', false, 4, 2, 'Legal-size documents - Insured up to $100', true, 'On eBay you save 12%', 'USPS_PRIORITY_MAIL_FLAT_RATE_LEGAL_ENVELOPE', 'usps-priority-legal', false, '2025-01-01 00:00:00+00', 'seed');
INSERT INTO public.shipping_services VALUES ('c1d3c7f4-6ac1-4a7f-8a29-6dbaf9ecbb51', 'USPS', 'USPS Priority Mail Flat Rate Envelope', 8.75, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 27 - 31', false, 4, 2, 'Best for documents - Includes tracking', true, 'On eBay you save 13%', 'USPS_PRIORITY_MAIL_FLAT_RATE_ENVELOPE', 'usps-priority-flat', false, '2025-01-01 00:00:00+00', 'seed');


--
-- TOC entry 4333 (class 0 OID 33305)
-- Dependencies: 396
-- Data for Name: store; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.store VALUES ('65987e62-8992-4f8f-bb21-1e3eea0f453c', 'e167368a-7042-45a4-8f1f-badba56b6b63', 'Viet awn cut', 'viet-awn-cut', '123', '', '', 0, true, '2026-03-17 04:11:17.021816+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', '2026-03-17 04:15:18.454027+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', false, 'admin@foodsage.com', '+84323232212', NULL, NULL, '#c3b83c');
INSERT INTO public.store VALUES ('e43d2fb2-d880-465d-87b6-3f72f60ece73', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'Test Store 123', 'test-store-123', 'This is a test store for UI bug investigation.', NULL, NULL, 0, true, '2026-03-17 04:27:15.450997+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL);


--
-- TOC entry 4334 (class 0 OID 33312)
-- Dependencies: 397
-- Data for Name: store_subscription; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.store_subscription VALUES ('fcdbf605-2848-49d3-89ba-1965423fb4f0', '65987e62-8992-4f8f-bb21-1e3eea0f453c', 0, 0.00, 'USD', 12.90, 250, '2026-03-17 04:11:16.962456+00', NULL, 0);
INSERT INTO public.store_subscription VALUES ('4c914bbe-dd60-43c4-8d1d-f89997885188', 'e43d2fb2-d880-465d-87b6-3f72f60ece73', 0, 0.00, 'USD', 12.90, 250, '2026-03-17 04:27:15.431393+00', NULL, 0);


--
-- TOC entry 4313 (class 0 OID 33050)
-- Dependencies: 376
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."user" VALUES ('70000000-0000-0000-0000-000000000001', 'demo.seller1@example.com', 'Alice Johnson', 'demo.seller1@example.com', '$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq', true, true, 'TopRated', 11222.00, '2024-01-01 00:00:00+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('70000000-0000-0000-0000-000000000002', 'demo.seller2@example.com', 'Brian Carter', 'demo.seller2@example.com', '$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq', true, true, 'TopRated', 13622.00, '2024-01-01 00:00:00+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('70000000-0000-0000-0000-000000000003', 'demo.seller3@example.com', 'Cecilia Gomez', 'demo.seller3@example.com', '$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq', true, true, 'TopRated', 16022.00, '2024-01-01 00:00:00+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('038a4659-83c4-456d-9bf9-e2036a53ad6c', 'vtrgiangg2903@gmail.com', 'Vu Truong Giang', 'vtrgiangg2903@gmail.com', '$2a$11$QQ9amPErK0/8LKR.pi/OlOcFaaE997.dCDKDnR.hd..XHSjpnkh56', true, false, 'BelowStandard', 0.00, '2026-03-11 01:05:42.625772+00', 'System', '2026-03-11 01:22:18.636831+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', false, 'Ha noi', 'Vietnam', 'no', 'VietNam', 'Ha Noi', '10001', true, true, '+84362678790');
INSERT INTO public."user" VALUES ('e167368a-7042-45a4-8f1f-badba56b6b63', 'vietbhhe187397@fpt.edu.vn', 'Bui Hoang Viet', 'vietbhhe187397@fpt.edu.vn', '$2a$11$VBzRQNcJ2KSSi4aHRLqdTujdVQj72m3DeJQvJbhNVKac946v9NoOy', true, false, 'BelowStandard', 0.00, '2026-03-17 04:08:45.027904+00', 'System', '2026-03-17 04:10:23.623765+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', false, 'Ha noi', 'Vietnam', 'no', 'VietNam', 'Ha Noi', '10001', true, true, '+84362678790');
INSERT INTO public."user" VALUES ('741eb167-fb61-4b47-a35a-90720c4949f7', 'test@example.com', 'Test User', 'test@example.com', '$2a$11$s3Ebze9AOlhaLB0Bp/8cxOnvY6v9vFYzzrLUxf4I4iFFnQ6JjHuAi', false, false, 'BelowStandard', 0.00, '2026-03-17 04:23:53.417305+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('c483385e-f1b9-40b4-9b95-e18f45e06a82', 'thinhndhe186687@fpt.edu.vn', 'Nguyễn Đức Thịnh', 'thinhndhe186687@fpt.edu.vn', '$2a$11$WnwGMii.QR8M3FRHcT0nBez6L2NGeyBklBttVngGmr.OLXEvOrwXG', true, true, 'BelowStandard', 0.00, '2026-03-14 14:59:36.532877+00', 'System', '2026-03-14 15:00:42.466291+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false, NULL, NULL, NULL, NULL, NULL, NULL, true, true, '+84868169332');
INSERT INTO public."user" VALUES ('602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'hoapoki24@gmail.com', 'Do Huu Hoa', 'hoapoki24@gmail.com', '$2a$11$gEhJpCuRGD2XH0n.MnXtsuFI2nYsBXzuig7NLUV8OajwBWSpmUYc2', true, false, 'BelowStandard', 0.00, '2026-03-17 01:31:10.426807+00', 'System', '2026-03-17 01:32:36.914758+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false, NULL, NULL, NULL, NULL, NULL, NULL, true, true, '+84362982702');
INSERT INTO public."user" VALUES ('e8f83430-5d4d-4ac2-bb82-46797049535e', 'ngochai0217@gmail.com', 'Le Ngoc Hai', 'ngochai0217@gmail.com', '$2a$11$c9ebDeXWqx58qriiLDzjaeGEvu3ln0LP2ETijCv7mY2Oly29ti39O', false, false, 'BelowStandard', 0.00, '2026-03-17 02:26:04.977332+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);


--
-- TOC entry 4321 (class 0 OID 33111)
-- Dependencies: 384
-- Data for Name: variation; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4356 (class 0 OID 33615)
-- Dependencies: 419
-- Data for Name: voucher_transactions; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4355 (class 0 OID 33608)
-- Dependencies: 418
-- Data for Name: vouchers; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4421 (class 0 OID 0)
-- Dependencies: 379
-- Name: listing_image_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.listing_image_id_seq', 3, true);


--
-- TOC entry 4422 (class 0 OID 0)
-- Dependencies: 381
-- Name: listing_item_specific_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.listing_item_specific_id_seq', 17, true);


--
-- TOC entry 4423 (class 0 OID 0)
-- Dependencies: 383
-- Name: variation_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.variation_id_seq', 1, false);


--
-- TOC entry 3940 (class 2606 OID 32411)
-- Name: __EFMigrationsHistory pk___ef_migrations_history; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id);


--
-- TOC entry 3943 (class 2606 OID 32981)
-- Name: category pk_category; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT pk_category PRIMARY KEY (id);


--
-- TOC entry 3978 (class 2606 OID 33073)
-- Name: category_condition pk_category_condition; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_condition
    ADD CONSTRAINT pk_category_condition PRIMARY KEY (category_id, condition_id);


--
-- TOC entry 3975 (class 2606 OID 33063)
-- Name: category_specific pk_category_specific; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_specific
    ADD CONSTRAINT pk_category_specific PRIMARY KEY (id);


--
-- TOC entry 3945 (class 2606 OID 32993)
-- Name: condition pk_condition; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.condition
    ADD CONSTRAINT pk_condition PRIMARY KEY (id);


--
-- TOC entry 4055 (class 2606 OID 33416)
-- Name: coupon pk_coupon; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon
    ADD CONSTRAINT pk_coupon PRIMARY KEY (id);


--
-- TOC entry 4059 (class 2606 OID 33426)
-- Name: coupon_condition pk_coupon_condition; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_condition
    ADD CONSTRAINT pk_coupon_condition PRIMARY KEY (id);


--
-- TOC entry 4097 (class 2606 OID 33578)
-- Name: coupon_excluded_categories pk_coupon_excluded_categories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_excluded_categories
    ADD CONSTRAINT pk_coupon_excluded_categories PRIMARY KEY (id);


--
-- TOC entry 4100 (class 2606 OID 33590)
-- Name: coupon_excluded_items pk_coupon_excluded_items; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_excluded_items
    ADD CONSTRAINT pk_coupon_excluded_items PRIMARY KEY (id);


--
-- TOC entry 4103 (class 2606 OID 33602)
-- Name: coupon_target_audiences pk_coupon_target_audiences; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_target_audiences
    ADD CONSTRAINT pk_coupon_target_audiences PRIMARY KEY (id);


--
-- TOC entry 4052 (class 2606 OID 33408)
-- Name: coupon_type pk_coupon_type; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_type
    ADD CONSTRAINT pk_coupon_type PRIMARY KEY (id);


--
-- TOC entry 4088 (class 2606 OID 33542)
-- Name: dispute pk_dispute; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dispute
    ADD CONSTRAINT pk_dispute PRIMARY KEY (id);


--
-- TOC entry 3947 (class 2606 OID 33000)
-- Name: file_metadata pk_file_metadata; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.file_metadata
    ADD CONSTRAINT pk_file_metadata PRIMARY KEY (id);


--
-- TOC entry 3953 (class 2606 OID 33007)
-- Name: listing pk_listing; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing
    ADD CONSTRAINT pk_listing PRIMARY KEY (id);


--
-- TOC entry 3994 (class 2606 OID 33156)
-- Name: listing_id pk_listing_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_id
    ADD CONSTRAINT pk_listing_id PRIMARY KEY (listing_id);


--
-- TOC entry 3980 (class 2606 OID 33091)
-- Name: listing_image pk_listing_image; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_image
    ADD CONSTRAINT pk_listing_image PRIMARY KEY (listing_id, id);


--
-- TOC entry 3982 (class 2606 OID 33104)
-- Name: listing_item_specific pk_listing_item_specific; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_item_specific
    ADD CONSTRAINT pk_listing_item_specific PRIMARY KEY (listing_id, id);


--
-- TOC entry 3956 (class 2606 OID 33014)
-- Name: listing_template pk_listing_template; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_template
    ADD CONSTRAINT pk_listing_template PRIMARY KEY (id);


--
-- TOC entry 4062 (class 2606 OID 33439)
-- Name: order_buyer_feedback pk_order_buyer_feedback; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_buyer_feedback
    ADD CONSTRAINT pk_order_buyer_feedback PRIMARY KEY (id);


--
-- TOC entry 4035 (class 2606 OID 33337)
-- Name: order_cancellation_requests pk_order_cancellation_requests; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_cancellation_requests
    ADD CONSTRAINT pk_order_cancellation_requests PRIMARY KEY (id);


--
-- TOC entry 4050 (class 2606 OID 33387)
-- Name: order_item_shipments pk_order_item_shipments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_shipments
    ADD CONSTRAINT pk_order_item_shipments PRIMARY KEY (id);


--
-- TOC entry 4009 (class 2606 OID 33212)
-- Name: order_items pk_order_items; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT pk_order_items PRIMARY KEY (id);


--
-- TOC entry 4039 (class 2606 OID 33349)
-- Name: order_return_requests pk_order_return_requests; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_return_requests
    ADD CONSTRAINT pk_order_return_requests PRIMARY KEY (id);


--
-- TOC entry 4012 (class 2606 OID 33224)
-- Name: order_shipping_labels pk_order_shipping_labels; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_shipping_labels
    ADD CONSTRAINT pk_order_shipping_labels PRIMARY KEY (id);


--
-- TOC entry 4017 (class 2606 OID 33234)
-- Name: order_status_histories pk_order_status_histories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_histories
    ADD CONSTRAINT pk_order_status_histories PRIMARY KEY (id);


--
-- TOC entry 3989 (class 2606 OID 33129)
-- Name: order_status_transitions pk_order_status_transitions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_transitions
    ADD CONSTRAINT pk_order_status_transitions PRIMARY KEY (id);


--
-- TOC entry 3959 (class 2606 OID 33021)
-- Name: order_statuses pk_order_statuses; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_statuses
    ADD CONSTRAINT pk_order_statuses PRIMARY KEY (id);


--
-- TOC entry 3999 (class 2606 OID 33168)
-- Name: orders pk_orders; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT pk_orders PRIMARY KEY (id);


--
-- TOC entry 3962 (class 2606 OID 33028)
-- Name: otp pk_otp; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.otp
    ADD CONSTRAINT pk_otp PRIMARY KEY (id);


--
-- TOC entry 3964 (class 2606 OID 33035)
-- Name: outbox_message pk_outbox_message; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outbox_message
    ADD CONSTRAINT pk_outbox_message PRIMARY KEY (id);


--
-- TOC entry 4002 (class 2606 OID 33185)
-- Name: refresh_token pk_refresh_token; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.refresh_token
    ADD CONSTRAINT pk_refresh_token PRIMARY KEY (id);


--
-- TOC entry 4042 (class 2606 OID 33365)
-- Name: report_downloads pk_report_downloads; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.report_downloads
    ADD CONSTRAINT pk_report_downloads PRIMARY KEY (id);


--
-- TOC entry 4045 (class 2606 OID 33373)
-- Name: report_schedules pk_report_schedules; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.report_schedules
    ADD CONSTRAINT pk_report_schedules PRIMARY KEY (id);


--
-- TOC entry 4110 (class 2606 OID 34872)
-- Name: research_saved_category pk_research_saved_category; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.research_saved_category
    ADD CONSTRAINT pk_research_saved_category PRIMARY KEY (user_id, category_id);


--
-- TOC entry 4020 (class 2606 OID 33297)
-- Name: return_policy pk_return_policy; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.return_policy
    ADD CONSTRAINT pk_return_policy PRIMARY KEY (id);


--
-- TOC entry 4094 (class 2606 OID 33557)
-- Name: review pk_review; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT pk_review PRIMARY KEY (id);


--
-- TOC entry 3966 (class 2606 OID 33042)
-- Name: role pk_role; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role
    ADD CONSTRAINT pk_role PRIMARY KEY (id);


--
-- TOC entry 3991 (class 2606 OID 33146)
-- Name: role_permissions pk_role_permissions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_permissions
    ADD CONSTRAINT pk_role_permissions PRIMARY KEY (role_id, "Permission");


--
-- TOC entry 4005 (class 2606 OID 33195)
-- Name: role_user pk_role_user; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_user
    ADD CONSTRAINT pk_role_user PRIMARY KEY (roles_id, user_id);


--
-- TOC entry 4065 (class 2606 OID 33458)
-- Name: sale_event pk_sale_event; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event
    ADD CONSTRAINT pk_sale_event PRIMARY KEY (id);


--
-- TOC entry 4068 (class 2606 OID 33463)
-- Name: sale_event_discount_tier pk_sale_event_discount_tier; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_discount_tier
    ADD CONSTRAINT pk_sale_event_discount_tier PRIMARY KEY (id);


--
-- TOC entry 4072 (class 2606 OID 33473)
-- Name: sale_event_listing pk_sale_event_listing; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listing
    ADD CONSTRAINT pk_sale_event_listing PRIMARY KEY (id);


--
-- TOC entry 4078 (class 2606 OID 33514)
-- Name: seller_blocked_buyer pk_seller_blocked_buyer; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_blocked_buyer
    ADD CONSTRAINT pk_seller_blocked_buyer PRIMARY KEY (id);


--
-- TOC entry 4082 (class 2606 OID 33525)
-- Name: seller_exempt_buyer pk_seller_exempt_buyer; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_exempt_buyer
    ADD CONSTRAINT pk_seller_exempt_buyer PRIMARY KEY (id);


--
-- TOC entry 4074 (class 2606 OID 33508)
-- Name: seller_preference pk_seller_preference; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_preference
    ADD CONSTRAINT pk_seller_preference PRIMARY KEY (id);


--
-- TOC entry 4023 (class 2606 OID 33304)
-- Name: shipping_policy pk_shipping_policy; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.shipping_policy
    ADD CONSTRAINT pk_shipping_policy PRIMARY KEY (id);


--
-- TOC entry 3969 (class 2606 OID 33049)
-- Name: shipping_services pk_shipping_services; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.shipping_services
    ADD CONSTRAINT pk_shipping_services PRIMARY KEY (id);


--
-- TOC entry 4027 (class 2606 OID 33311)
-- Name: store pk_store; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.store
    ADD CONSTRAINT pk_store PRIMARY KEY (id);


--
-- TOC entry 4031 (class 2606 OID 33316)
-- Name: store_subscription pk_store_subscription; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.store_subscription
    ADD CONSTRAINT pk_store_subscription PRIMARY KEY (id);


--
-- TOC entry 3972 (class 2606 OID 33056)
-- Name: user pk_user; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT pk_user PRIMARY KEY (id);


--
-- TOC entry 3985 (class 2606 OID 33117)
-- Name: variation pk_variation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.variation
    ADD CONSTRAINT pk_variation PRIMARY KEY (id);


--
-- TOC entry 4108 (class 2606 OID 33621)
-- Name: voucher_transactions pk_voucher_transactions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.voucher_transactions
    ADD CONSTRAINT pk_voucher_transactions PRIMARY KEY (id);


--
-- TOC entry 4105 (class 2606 OID 33614)
-- Name: vouchers pk_vouchers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vouchers
    ADD CONSTRAINT pk_vouchers PRIMARY KEY (id);


--
-- TOC entry 4032 (class 1259 OID 33355)
-- Name: idx_cancellation_requests_order; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_cancellation_requests_order ON public.order_cancellation_requests USING btree (order_id);


--
-- TOC entry 4033 (class 1259 OID 33356)
-- Name: idx_cancellation_requests_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_cancellation_requests_status ON public.order_cancellation_requests USING btree (status);


--
-- TOC entry 3948 (class 1259 OID 33272)
-- Name: idx_listing_active_owner_sort; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_listing_active_owner_sort ON public.listing USING btree (created_by, start_date DESC, created_at DESC, id, category_id, format) WHERE (status = 3);


--
-- TOC entry 3949 (class 1259 OID 33273)
-- Name: idx_listing_owner_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_listing_owner_status ON public.listing USING btree (created_by, status);


--
-- TOC entry 3950 (class 1259 OID 33274)
-- Name: idx_listing_sku_trgm; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_listing_sku_trgm ON public.listing USING gin (sku public.gin_trgm_ops);


--
-- TOC entry 3951 (class 1259 OID 33275)
-- Name: idx_listing_title_trgm; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_listing_title_trgm ON public.listing USING gin (title public.gin_trgm_ops);


--
-- TOC entry 4006 (class 1259 OID 33271)
-- Name: idx_order_items_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_order_items_listing_id ON public.order_items USING btree (listing_id);


--
-- TOC entry 4018 (class 1259 OID 33322)
-- Name: idx_return_policy_store_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_return_policy_store_id ON public.return_policy USING btree (store_id);


--
-- TOC entry 4036 (class 1259 OID 33357)
-- Name: idx_return_requests_order; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_return_requests_order ON public.order_return_requests USING btree (order_id);


--
-- TOC entry 4037 (class 1259 OID 33358)
-- Name: idx_return_requests_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_return_requests_status ON public.order_return_requests USING btree (status);


--
-- TOC entry 4021 (class 1259 OID 33323)
-- Name: idx_shipping_policy_store_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_shipping_policy_store_id ON public.shipping_policy USING btree (store_id);


--
-- TOC entry 4024 (class 1259 OID 33324)
-- Name: idx_store_slug; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_store_slug ON public.store USING btree (slug);


--
-- TOC entry 4028 (class 1259 OID 33326)
-- Name: idx_store_subscription_store_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_store_subscription_store_id ON public.store_subscription USING btree (store_id);


--
-- TOC entry 4029 (class 1259 OID 33327)
-- Name: idx_store_subscription_store_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_store_subscription_store_status ON public.store_subscription USING btree (store_id, status);


--
-- TOC entry 4025 (class 1259 OID 33325)
-- Name: idx_store_user_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_store_user_id ON public.store USING btree (user_id);


--
-- TOC entry 3983 (class 1259 OID 33270)
-- Name: idx_variation_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_variation_listing_id ON public.variation USING btree (listing_id);


--
-- TOC entry 3976 (class 1259 OID 33251)
-- Name: ix_category_condition_condition_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_category_condition_condition_id ON public.category_condition USING btree (condition_id);


--
-- TOC entry 3941 (class 1259 OID 33250)
-- Name: ix_category_parent_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_category_parent_id ON public.category USING btree (parent_id);


--
-- TOC entry 3973 (class 1259 OID 33252)
-- Name: ix_category_specific_category_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_category_specific_category_id ON public.category_specific USING btree (category_id);


--
-- TOC entry 4057 (class 1259 OID 33434)
-- Name: ix_coupon_condition_coupon_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_condition_coupon_id ON public.coupon_condition USING btree (coupon_id);


--
-- TOC entry 4053 (class 1259 OID 33432)
-- Name: ix_coupon_coupon_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_coupon_type_id ON public.coupon USING btree (coupon_type_id);


--
-- TOC entry 4095 (class 1259 OID 33627)
-- Name: ix_coupon_excluded_categories_coupon_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_excluded_categories_coupon_id ON public.coupon_excluded_categories USING btree (coupon_id);


--
-- TOC entry 4098 (class 1259 OID 33628)
-- Name: ix_coupon_excluded_items_coupon_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_excluded_items_coupon_id ON public.coupon_excluded_items USING btree (coupon_id);


--
-- TOC entry 4101 (class 1259 OID 33629)
-- Name: ix_coupon_target_audiences_coupon_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_target_audiences_coupon_id ON public.coupon_target_audiences USING btree (coupon_id);


--
-- TOC entry 4084 (class 1259 OID 33563)
-- Name: ix_dispute_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_dispute_listing_id ON public.dispute USING btree (listing_id);


--
-- TOC entry 4085 (class 1259 OID 33564)
-- Name: ix_dispute_raised_by_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_dispute_raised_by_id ON public.dispute USING btree (raised_by_id);


--
-- TOC entry 4086 (class 1259 OID 33565)
-- Name: ix_dispute_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_dispute_status ON public.dispute USING btree (status);


--
-- TOC entry 3992 (class 1259 OID 33253)
-- Name: ix_listing_id_seller_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_listing_id_seller_id ON public.listing_id USING btree (seller_id);


--
-- TOC entry 3954 (class 1259 OID 33254)
-- Name: ix_listing_template_name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_listing_template_name ON public.listing_template USING btree (name);


--
-- TOC entry 4060 (class 1259 OID 33450)
-- Name: ix_order_buyer_feedback_buyer_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_buyer_feedback_buyer_id ON public.order_buyer_feedback USING btree (buyer_id);


--
-- TOC entry 4046 (class 1259 OID 33398)
-- Name: ix_order_item_shipments_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_item_shipments_order_id ON public.order_item_shipments USING btree (order_id);


--
-- TOC entry 4047 (class 1259 OID 33399)
-- Name: ix_order_item_shipments_order_item_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_item_shipments_order_item_id ON public.order_item_shipments USING btree (order_item_id);


--
-- TOC entry 4048 (class 1259 OID 33400)
-- Name: ix_order_item_shipments_shipping_label_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_item_shipments_shipping_label_id ON public.order_item_shipments USING btree (shipping_label_id);


--
-- TOC entry 4007 (class 1259 OID 33255)
-- Name: ix_order_items_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_items_order_id ON public.order_items USING btree (order_id);


--
-- TOC entry 4010 (class 1259 OID 33256)
-- Name: ix_order_shipping_labels_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_shipping_labels_order_id ON public.order_shipping_labels USING btree (order_id);


--
-- TOC entry 4013 (class 1259 OID 33257)
-- Name: ix_order_status_histories_from_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_histories_from_status_id ON public.order_status_histories USING btree (from_status_id);


--
-- TOC entry 4014 (class 1259 OID 33258)
-- Name: ix_order_status_histories_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_histories_order_id ON public.order_status_histories USING btree (order_id);


--
-- TOC entry 4015 (class 1259 OID 33259)
-- Name: ix_order_status_histories_to_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_histories_to_status_id ON public.order_status_histories USING btree (to_status_id);


--
-- TOC entry 3986 (class 1259 OID 33260)
-- Name: ix_order_status_transitions_from_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_transitions_from_status_id ON public.order_status_transitions USING btree (from_status_id);


--
-- TOC entry 3987 (class 1259 OID 33261)
-- Name: ix_order_status_transitions_to_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_transitions_to_status_id ON public.order_status_transitions USING btree (to_status_id);


--
-- TOC entry 3957 (class 1259 OID 33262)
-- Name: ix_order_statuses_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_order_statuses_code ON public.order_statuses USING btree (code);


--
-- TOC entry 3995 (class 1259 OID 33263)
-- Name: ix_orders_buyer_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_orders_buyer_id ON public.orders USING btree (buyer_id);


--
-- TOC entry 3996 (class 1259 OID 33264)
-- Name: ix_orders_order_number; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_orders_order_number ON public.orders USING btree (order_number);


--
-- TOC entry 3997 (class 1259 OID 33265)
-- Name: ix_orders_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_orders_status_id ON public.orders USING btree (status_id);


--
-- TOC entry 3960 (class 1259 OID 33266)
-- Name: ix_otp_email_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_otp_email_code ON public.otp USING btree (email, code);


--
-- TOC entry 4000 (class 1259 OID 33267)
-- Name: ix_refresh_token_user_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_refresh_token_user_id ON public.refresh_token USING btree (user_id);


--
-- TOC entry 4040 (class 1259 OID 33374)
-- Name: ix_report_downloads_user_id_reference_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_report_downloads_user_id_reference_code ON public.report_downloads USING btree (user_id, reference_code);


--
-- TOC entry 4043 (class 1259 OID 33375)
-- Name: ix_report_schedules_user_id_source_type_is_active; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_report_schedules_user_id_source_type_is_active ON public.report_schedules USING btree (user_id, source, type, is_active);


--
-- TOC entry 4089 (class 1259 OID 33566)
-- Name: ix_review_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_review_listing_id ON public.review USING btree (listing_id);


--
-- TOC entry 4090 (class 1259 OID 33567)
-- Name: ix_review_recipient_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_review_recipient_id ON public.review USING btree (recipient_id);


--
-- TOC entry 4091 (class 1259 OID 33568)
-- Name: ix_review_recipient_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_review_recipient_role ON public.review USING btree (recipient_role);


--
-- TOC entry 4092 (class 1259 OID 33569)
-- Name: ix_review_reviewer_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_review_reviewer_id ON public.review USING btree (reviewer_id);


--
-- TOC entry 4003 (class 1259 OID 33268)
-- Name: ix_role_user_user_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_role_user_user_id ON public.role_user USING btree (user_id);


--
-- TOC entry 4066 (class 1259 OID 33484)
-- Name: ix_sale_event_discount_tier_sale_event_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_event_discount_tier_sale_event_id ON public.sale_event_discount_tier USING btree (sale_event_id);


--
-- TOC entry 4069 (class 1259 OID 33485)
-- Name: ix_sale_event_listing_discount_tier_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_event_listing_discount_tier_id ON public.sale_event_listing USING btree (discount_tier_id);


--
-- TOC entry 4070 (class 1259 OID 33486)
-- Name: ix_sale_event_listing_sale_event_id_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_sale_event_listing_sale_event_id_listing_id ON public.sale_event_listing USING btree (sale_event_id, listing_id);


--
-- TOC entry 4076 (class 1259 OID 33531)
-- Name: ix_seller_blocked_buyer_seller_preference_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_seller_blocked_buyer_seller_preference_id ON public.seller_blocked_buyer USING btree (seller_preference_id);


--
-- TOC entry 4080 (class 1259 OID 33533)
-- Name: ix_seller_exempt_buyer_seller_preference_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_seller_exempt_buyer_seller_preference_id ON public.seller_exempt_buyer USING btree (seller_preference_id);


--
-- TOC entry 3967 (class 1259 OID 33290)
-- Name: ix_shipping_services_slug; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_shipping_services_slug ON public.shipping_services USING btree (slug);


--
-- TOC entry 3970 (class 1259 OID 33269)
-- Name: ix_user_email; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_user_email ON public."user" USING btree (email);


--
-- TOC entry 4106 (class 1259 OID 33630)
-- Name: ix_voucher_transactions_voucher_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_voucher_transactions_voucher_id ON public.voucher_transactions USING btree (voucher_id);


--
-- TOC entry 4063 (class 1259 OID 33451)
-- Name: ux_buyer_feedback_order; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_buyer_feedback_order ON public.order_buyer_feedback USING btree (order_id);


--
-- TOC entry 4056 (class 1259 OID 33433)
-- Name: ux_coupon_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_coupon_code ON public.coupon USING btree (code);


--
-- TOC entry 4079 (class 1259 OID 33532)
-- Name: ux_seller_blocked_buyer_identifier; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_seller_blocked_buyer_identifier ON public.seller_blocked_buyer USING btree (normalized_identifier, seller_preference_id);


--
-- TOC entry 4083 (class 1259 OID 33534)
-- Name: ux_seller_exempt_buyer_identifier; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_seller_exempt_buyer_identifier ON public.seller_exempt_buyer USING btree (normalized_identifier, seller_preference_id);


--
-- TOC entry 4075 (class 1259 OID 33535)
-- Name: ux_seller_preference_seller; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_seller_preference_seller ON public.seller_preference USING btree (seller_id);


--
-- TOC entry 4111 (class 2606 OID 32982)
-- Name: category fk_category_category_parent_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT fk_category_category_parent_id FOREIGN KEY (parent_id) REFERENCES public.category(id) ON DELETE RESTRICT;


--
-- TOC entry 4113 (class 2606 OID 33074)
-- Name: category_condition fk_category_condition_categories_category_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_condition
    ADD CONSTRAINT fk_category_condition_categories_category_id FOREIGN KEY (category_id) REFERENCES public.category(id) ON DELETE CASCADE;


--
-- TOC entry 4114 (class 2606 OID 33079)
-- Name: category_condition fk_category_condition_condition_condition_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_condition
    ADD CONSTRAINT fk_category_condition_condition_condition_id FOREIGN KEY (condition_id) REFERENCES public.condition(id) ON DELETE CASCADE;


--
-- TOC entry 4112 (class 2606 OID 33064)
-- Name: category_specific fk_category_specific_category_category_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_specific
    ADD CONSTRAINT fk_category_specific_category_category_id FOREIGN KEY (category_id) REFERENCES public.category(id);


--
-- TOC entry 4139 (class 2606 OID 33427)
-- Name: coupon_condition fk_coupon_condition_coupons_coupon_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_condition
    ADD CONSTRAINT fk_coupon_condition_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES public.coupon(id) ON DELETE CASCADE;


--
-- TOC entry 4138 (class 2606 OID 33417)
-- Name: coupon fk_coupon_coupon_types_coupon_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon
    ADD CONSTRAINT fk_coupon_coupon_types_coupon_type_id FOREIGN KEY (coupon_type_id) REFERENCES public.coupon_type(id) ON DELETE RESTRICT;


--
-- TOC entry 4149 (class 2606 OID 33579)
-- Name: coupon_excluded_categories fk_coupon_excluded_categories_coupons_coupon_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_excluded_categories
    ADD CONSTRAINT fk_coupon_excluded_categories_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES public.coupon(id) ON DELETE CASCADE;


--
-- TOC entry 4150 (class 2606 OID 33591)
-- Name: coupon_excluded_items fk_coupon_excluded_items_coupons_coupon_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_excluded_items
    ADD CONSTRAINT fk_coupon_excluded_items_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES public.coupon(id) ON DELETE CASCADE;


--
-- TOC entry 4151 (class 2606 OID 33603)
-- Name: coupon_target_audiences fk_coupon_target_audiences_coupons_coupon_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_target_audiences
    ADD CONSTRAINT fk_coupon_target_audiences_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES public.coupon(id) ON DELETE CASCADE;


--
-- TOC entry 4147 (class 2606 OID 33543)
-- Name: dispute fk_dispute_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dispute
    ADD CONSTRAINT fk_dispute_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4121 (class 2606 OID 33157)
-- Name: listing_id fk_listing_id_user_seller_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_id
    ADD CONSTRAINT fk_listing_id_user_seller_id FOREIGN KEY (seller_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4115 (class 2606 OID 33092)
-- Name: listing_image fk_listing_image_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_image
    ADD CONSTRAINT fk_listing_image_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4116 (class 2606 OID 33105)
-- Name: listing_item_specific fk_listing_item_specific_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_item_specific
    ADD CONSTRAINT fk_listing_item_specific_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4140 (class 2606 OID 33440)
-- Name: order_buyer_feedback fk_order_buyer_feedback_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_buyer_feedback
    ADD CONSTRAINT fk_order_buyer_feedback_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4141 (class 2606 OID 33445)
-- Name: order_buyer_feedback fk_order_buyer_feedback_user_buyer_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_buyer_feedback
    ADD CONSTRAINT fk_order_buyer_feedback_user_buyer_id FOREIGN KEY (buyer_id) REFERENCES public."user"(id) ON DELETE RESTRICT;


--
-- TOC entry 4134 (class 2606 OID 33338)
-- Name: order_cancellation_requests fk_order_cancellation_requests_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_cancellation_requests
    ADD CONSTRAINT fk_order_cancellation_requests_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4136 (class 2606 OID 33388)
-- Name: order_item_shipments fk_order_item_shipments_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_shipments
    ADD CONSTRAINT fk_order_item_shipments_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4137 (class 2606 OID 33393)
-- Name: order_item_shipments fk_order_item_shipments_shipping_labels_shipping_label_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_shipments
    ADD CONSTRAINT fk_order_item_shipments_shipping_labels_shipping_label_id FOREIGN KEY (shipping_label_id) REFERENCES public.order_shipping_labels(id) ON DELETE SET NULL;


--
-- TOC entry 4127 (class 2606 OID 33376)
-- Name: order_items fk_order_items_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT fk_order_items_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE RESTRICT;


--
-- TOC entry 4128 (class 2606 OID 33213)
-- Name: order_items fk_order_items_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT fk_order_items_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4135 (class 2606 OID 33350)
-- Name: order_return_requests fk_order_return_requests_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_return_requests
    ADD CONSTRAINT fk_order_return_requests_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4129 (class 2606 OID 33225)
-- Name: order_shipping_labels fk_order_shipping_labels_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_shipping_labels
    ADD CONSTRAINT fk_order_shipping_labels_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4130 (class 2606 OID 33235)
-- Name: order_status_histories fk_order_status_histories_order_statuses_from_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_histories
    ADD CONSTRAINT fk_order_status_histories_order_statuses_from_status_id FOREIGN KEY (from_status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4131 (class 2606 OID 33240)
-- Name: order_status_histories fk_order_status_histories_order_statuses_to_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_histories
    ADD CONSTRAINT fk_order_status_histories_order_statuses_to_status_id FOREIGN KEY (to_status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4132 (class 2606 OID 33245)
-- Name: order_status_histories fk_order_status_histories_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_histories
    ADD CONSTRAINT fk_order_status_histories_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4118 (class 2606 OID 33130)
-- Name: order_status_transitions fk_order_status_transitions_order_statuses_from_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_transitions
    ADD CONSTRAINT fk_order_status_transitions_order_statuses_from_status_id FOREIGN KEY (from_status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4119 (class 2606 OID 33135)
-- Name: order_status_transitions fk_order_status_transitions_order_statuses_to_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_transitions
    ADD CONSTRAINT fk_order_status_transitions_order_statuses_to_status_id FOREIGN KEY (to_status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4122 (class 2606 OID 33169)
-- Name: orders fk_orders_order_statuses_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_order_statuses_status_id FOREIGN KEY (status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4123 (class 2606 OID 33174)
-- Name: orders fk_orders_user_buyer_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_user_buyer_id FOREIGN KEY (buyer_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4124 (class 2606 OID 33186)
-- Name: refresh_token fk_refresh_token_user_user_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.refresh_token
    ADD CONSTRAINT fk_refresh_token_user_user_id FOREIGN KEY (user_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4148 (class 2606 OID 33558)
-- Name: review fk_review_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT fk_review_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4120 (class 2606 OID 33147)
-- Name: role_permissions fk_role_permissions_role_role_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_permissions
    ADD CONSTRAINT fk_role_permissions_role_role_id FOREIGN KEY (role_id) REFERENCES public.role(id) ON DELETE CASCADE;


--
-- TOC entry 4125 (class 2606 OID 33196)
-- Name: role_user fk_role_user_role_roles_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_user
    ADD CONSTRAINT fk_role_user_role_roles_id FOREIGN KEY (roles_id) REFERENCES public.role(id) ON DELETE CASCADE;


--
-- TOC entry 4126 (class 2606 OID 33201)
-- Name: role_user fk_role_user_user_user_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_user
    ADD CONSTRAINT fk_role_user_user_user_id FOREIGN KEY (user_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4142 (class 2606 OID 33464)
-- Name: sale_event_discount_tier fk_sale_event_discount_tier_sale_event_sale_event_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_discount_tier
    ADD CONSTRAINT fk_sale_event_discount_tier_sale_event_sale_event_id FOREIGN KEY (sale_event_id) REFERENCES public.sale_event(id) ON DELETE CASCADE;


--
-- TOC entry 4143 (class 2606 OID 33474)
-- Name: sale_event_listing fk_sale_event_listing_sale_event_discount_tier_discount_tier_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listing
    ADD CONSTRAINT fk_sale_event_listing_sale_event_discount_tier_discount_tier_id FOREIGN KEY (discount_tier_id) REFERENCES public.sale_event_discount_tier(id) ON DELETE CASCADE;


--
-- TOC entry 4144 (class 2606 OID 33479)
-- Name: sale_event_listing fk_sale_event_listing_sale_event_sale_event_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listing
    ADD CONSTRAINT fk_sale_event_listing_sale_event_sale_event_id FOREIGN KEY (sale_event_id) REFERENCES public.sale_event(id) ON DELETE CASCADE;


--
-- TOC entry 4145 (class 2606 OID 33515)
-- Name: seller_blocked_buyer fk_seller_blocked_buyer_seller_preference_seller_preference_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_blocked_buyer
    ADD CONSTRAINT fk_seller_blocked_buyer_seller_preference_seller_preference_id FOREIGN KEY (seller_preference_id) REFERENCES public.seller_preference(id) ON DELETE CASCADE;


--
-- TOC entry 4146 (class 2606 OID 33526)
-- Name: seller_exempt_buyer fk_seller_exempt_buyer_seller_preference_seller_preference_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_exempt_buyer
    ADD CONSTRAINT fk_seller_exempt_buyer_seller_preference_seller_preference_id FOREIGN KEY (seller_preference_id) REFERENCES public.seller_preference(id) ON DELETE CASCADE;


--
-- TOC entry 4133 (class 2606 OID 33317)
-- Name: store_subscription fk_store_subscription_store_store_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.store_subscription
    ADD CONSTRAINT fk_store_subscription_store_store_id FOREIGN KEY (store_id) REFERENCES public.store(id) ON DELETE CASCADE;


--
-- TOC entry 4117 (class 2606 OID 33118)
-- Name: variation fk_variation_fixed_price_listings_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.variation
    ADD CONSTRAINT fk_variation_fixed_price_listings_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4152 (class 2606 OID 33622)
-- Name: voucher_transactions fk_voucher_transactions_vouchers_voucher_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.voucher_transactions
    ADD CONSTRAINT fk_voucher_transactions_vouchers_voucher_id FOREIGN KEY (voucher_id) REFERENCES public.vouchers(id) ON DELETE CASCADE;


--
-- TOC entry 4364 (class 0 OID 0)
-- Dependencies: 70
-- Name: SCHEMA public; Type: ACL; Schema: -; Owner: pg_database_owner
--

REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;
GRANT USAGE ON SCHEMA public TO postgres;
GRANT USAGE ON SCHEMA public TO anon;
GRANT USAGE ON SCHEMA public TO authenticated;
GRANT USAGE ON SCHEMA public TO service_role;


--
-- TOC entry 4365 (class 0 OID 0)
-- Dependencies: 365
-- Name: TABLE "__EFMigrationsHistory"; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public."__EFMigrationsHistory" TO anon;
GRANT ALL ON TABLE public."__EFMigrationsHistory" TO authenticated;
GRANT ALL ON TABLE public."__EFMigrationsHistory" TO service_role;


--
-- TOC entry 4366 (class 0 OID 0)
-- Dependencies: 366
-- Name: TABLE category; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.category TO anon;
GRANT ALL ON TABLE public.category TO authenticated;
GRANT ALL ON TABLE public.category TO service_role;


--
-- TOC entry 4367 (class 0 OID 0)
-- Dependencies: 378
-- Name: TABLE category_condition; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.category_condition TO anon;
GRANT ALL ON TABLE public.category_condition TO authenticated;
GRANT ALL ON TABLE public.category_condition TO service_role;


--
-- TOC entry 4368 (class 0 OID 0)
-- Dependencies: 377
-- Name: TABLE category_specific; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.category_specific TO anon;
GRANT ALL ON TABLE public.category_specific TO authenticated;
GRANT ALL ON TABLE public.category_specific TO service_role;


--
-- TOC entry 4369 (class 0 OID 0)
-- Dependencies: 367
-- Name: TABLE condition; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.condition TO anon;
GRANT ALL ON TABLE public.condition TO authenticated;
GRANT ALL ON TABLE public.condition TO service_role;


--
-- TOC entry 4370 (class 0 OID 0)
-- Dependencies: 404
-- Name: TABLE coupon; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon TO anon;
GRANT ALL ON TABLE public.coupon TO authenticated;
GRANT ALL ON TABLE public.coupon TO service_role;


--
-- TOC entry 4371 (class 0 OID 0)
-- Dependencies: 405
-- Name: TABLE coupon_condition; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_condition TO anon;
GRANT ALL ON TABLE public.coupon_condition TO authenticated;
GRANT ALL ON TABLE public.coupon_condition TO service_role;


--
-- TOC entry 4372 (class 0 OID 0)
-- Dependencies: 415
-- Name: TABLE coupon_excluded_categories; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_excluded_categories TO anon;
GRANT ALL ON TABLE public.coupon_excluded_categories TO authenticated;
GRANT ALL ON TABLE public.coupon_excluded_categories TO service_role;


--
-- TOC entry 4373 (class 0 OID 0)
-- Dependencies: 416
-- Name: TABLE coupon_excluded_items; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_excluded_items TO anon;
GRANT ALL ON TABLE public.coupon_excluded_items TO authenticated;
GRANT ALL ON TABLE public.coupon_excluded_items TO service_role;


--
-- TOC entry 4374 (class 0 OID 0)
-- Dependencies: 417
-- Name: TABLE coupon_target_audiences; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_target_audiences TO anon;
GRANT ALL ON TABLE public.coupon_target_audiences TO authenticated;
GRANT ALL ON TABLE public.coupon_target_audiences TO service_role;


--
-- TOC entry 4375 (class 0 OID 0)
-- Dependencies: 403
-- Name: TABLE coupon_type; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_type TO anon;
GRANT ALL ON TABLE public.coupon_type TO authenticated;
GRANT ALL ON TABLE public.coupon_type TO service_role;


--
-- TOC entry 4376 (class 0 OID 0)
-- Dependencies: 413
-- Name: TABLE dispute; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.dispute TO anon;
GRANT ALL ON TABLE public.dispute TO authenticated;
GRANT ALL ON TABLE public.dispute TO service_role;


--
-- TOC entry 4377 (class 0 OID 0)
-- Dependencies: 368
-- Name: TABLE file_metadata; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.file_metadata TO anon;
GRANT ALL ON TABLE public.file_metadata TO authenticated;
GRANT ALL ON TABLE public.file_metadata TO service_role;


--
-- TOC entry 4378 (class 0 OID 0)
-- Dependencies: 369
-- Name: TABLE listing; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing TO anon;
GRANT ALL ON TABLE public.listing TO authenticated;
GRANT ALL ON TABLE public.listing TO service_role;


--
-- TOC entry 4379 (class 0 OID 0)
-- Dependencies: 387
-- Name: TABLE listing_id; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing_id TO anon;
GRANT ALL ON TABLE public.listing_id TO authenticated;
GRANT ALL ON TABLE public.listing_id TO service_role;


--
-- TOC entry 4380 (class 0 OID 0)
-- Dependencies: 380
-- Name: TABLE listing_image; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing_image TO anon;
GRANT ALL ON TABLE public.listing_image TO authenticated;
GRANT ALL ON TABLE public.listing_image TO service_role;


--
-- TOC entry 4381 (class 0 OID 0)
-- Dependencies: 379
-- Name: SEQUENCE listing_image_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.listing_image_id_seq TO anon;
GRANT ALL ON SEQUENCE public.listing_image_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.listing_image_id_seq TO service_role;


--
-- TOC entry 4382 (class 0 OID 0)
-- Dependencies: 382
-- Name: TABLE listing_item_specific; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing_item_specific TO anon;
GRANT ALL ON TABLE public.listing_item_specific TO authenticated;
GRANT ALL ON TABLE public.listing_item_specific TO service_role;


--
-- TOC entry 4383 (class 0 OID 0)
-- Dependencies: 381
-- Name: SEQUENCE listing_item_specific_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.listing_item_specific_id_seq TO anon;
GRANT ALL ON SEQUENCE public.listing_item_specific_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.listing_item_specific_id_seq TO service_role;


--
-- TOC entry 4384 (class 0 OID 0)
-- Dependencies: 370
-- Name: TABLE listing_template; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing_template TO anon;
GRANT ALL ON TABLE public.listing_template TO authenticated;
GRANT ALL ON TABLE public.listing_template TO service_role;


--
-- TOC entry 4385 (class 0 OID 0)
-- Dependencies: 406
-- Name: TABLE order_buyer_feedback; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_buyer_feedback TO anon;
GRANT ALL ON TABLE public.order_buyer_feedback TO authenticated;
GRANT ALL ON TABLE public.order_buyer_feedback TO service_role;


--
-- TOC entry 4386 (class 0 OID 0)
-- Dependencies: 398
-- Name: TABLE order_cancellation_requests; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_cancellation_requests TO anon;
GRANT ALL ON TABLE public.order_cancellation_requests TO authenticated;
GRANT ALL ON TABLE public.order_cancellation_requests TO service_role;


--
-- TOC entry 4387 (class 0 OID 0)
-- Dependencies: 402
-- Name: TABLE order_item_shipments; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_item_shipments TO anon;
GRANT ALL ON TABLE public.order_item_shipments TO authenticated;
GRANT ALL ON TABLE public.order_item_shipments TO service_role;


--
-- TOC entry 4388 (class 0 OID 0)
-- Dependencies: 391
-- Name: TABLE order_items; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_items TO anon;
GRANT ALL ON TABLE public.order_items TO authenticated;
GRANT ALL ON TABLE public.order_items TO service_role;


--
-- TOC entry 4389 (class 0 OID 0)
-- Dependencies: 399
-- Name: TABLE order_return_requests; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_return_requests TO anon;
GRANT ALL ON TABLE public.order_return_requests TO authenticated;
GRANT ALL ON TABLE public.order_return_requests TO service_role;


--
-- TOC entry 4390 (class 0 OID 0)
-- Dependencies: 392
-- Name: TABLE order_shipping_labels; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_shipping_labels TO anon;
GRANT ALL ON TABLE public.order_shipping_labels TO authenticated;
GRANT ALL ON TABLE public.order_shipping_labels TO service_role;


--
-- TOC entry 4391 (class 0 OID 0)
-- Dependencies: 393
-- Name: TABLE order_status_histories; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_status_histories TO anon;
GRANT ALL ON TABLE public.order_status_histories TO authenticated;
GRANT ALL ON TABLE public.order_status_histories TO service_role;


--
-- TOC entry 4392 (class 0 OID 0)
-- Dependencies: 385
-- Name: TABLE order_status_transitions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_status_transitions TO anon;
GRANT ALL ON TABLE public.order_status_transitions TO authenticated;
GRANT ALL ON TABLE public.order_status_transitions TO service_role;


--
-- TOC entry 4393 (class 0 OID 0)
-- Dependencies: 371
-- Name: TABLE order_statuses; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_statuses TO anon;
GRANT ALL ON TABLE public.order_statuses TO authenticated;
GRANT ALL ON TABLE public.order_statuses TO service_role;


--
-- TOC entry 4394 (class 0 OID 0)
-- Dependencies: 388
-- Name: TABLE orders; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.orders TO anon;
GRANT ALL ON TABLE public.orders TO authenticated;
GRANT ALL ON TABLE public.orders TO service_role;


--
-- TOC entry 4395 (class 0 OID 0)
-- Dependencies: 372
-- Name: TABLE otp; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.otp TO anon;
GRANT ALL ON TABLE public.otp TO authenticated;
GRANT ALL ON TABLE public.otp TO service_role;


--
-- TOC entry 4396 (class 0 OID 0)
-- Dependencies: 373
-- Name: TABLE outbox_message; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.outbox_message TO anon;
GRANT ALL ON TABLE public.outbox_message TO authenticated;
GRANT ALL ON TABLE public.outbox_message TO service_role;


--
-- TOC entry 4397 (class 0 OID 0)
-- Dependencies: 389
-- Name: TABLE refresh_token; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.refresh_token TO anon;
GRANT ALL ON TABLE public.refresh_token TO authenticated;
GRANT ALL ON TABLE public.refresh_token TO service_role;


--
-- TOC entry 4398 (class 0 OID 0)
-- Dependencies: 400
-- Name: TABLE report_downloads; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.report_downloads TO anon;
GRANT ALL ON TABLE public.report_downloads TO authenticated;
GRANT ALL ON TABLE public.report_downloads TO service_role;


--
-- TOC entry 4399 (class 0 OID 0)
-- Dependencies: 401
-- Name: TABLE report_schedules; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.report_schedules TO anon;
GRANT ALL ON TABLE public.report_schedules TO authenticated;
GRANT ALL ON TABLE public.report_schedules TO service_role;


--
-- TOC entry 4400 (class 0 OID 0)
-- Dependencies: 420
-- Name: TABLE research_saved_category; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.research_saved_category TO anon;
GRANT ALL ON TABLE public.research_saved_category TO authenticated;
GRANT ALL ON TABLE public.research_saved_category TO service_role;


--
-- TOC entry 4401 (class 0 OID 0)
-- Dependencies: 394
-- Name: TABLE return_policy; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.return_policy TO anon;
GRANT ALL ON TABLE public.return_policy TO authenticated;
GRANT ALL ON TABLE public.return_policy TO service_role;


--
-- TOC entry 4402 (class 0 OID 0)
-- Dependencies: 414
-- Name: TABLE review; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.review TO anon;
GRANT ALL ON TABLE public.review TO authenticated;
GRANT ALL ON TABLE public.review TO service_role;


--
-- TOC entry 4403 (class 0 OID 0)
-- Dependencies: 374
-- Name: TABLE role; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.role TO anon;
GRANT ALL ON TABLE public.role TO authenticated;
GRANT ALL ON TABLE public.role TO service_role;


--
-- TOC entry 4404 (class 0 OID 0)
-- Dependencies: 386
-- Name: TABLE role_permissions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.role_permissions TO anon;
GRANT ALL ON TABLE public.role_permissions TO authenticated;
GRANT ALL ON TABLE public.role_permissions TO service_role;


--
-- TOC entry 4405 (class 0 OID 0)
-- Dependencies: 390
-- Name: TABLE role_user; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.role_user TO anon;
GRANT ALL ON TABLE public.role_user TO authenticated;
GRANT ALL ON TABLE public.role_user TO service_role;


--
-- TOC entry 4406 (class 0 OID 0)
-- Dependencies: 407
-- Name: TABLE sale_event; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event TO anon;
GRANT ALL ON TABLE public.sale_event TO authenticated;
GRANT ALL ON TABLE public.sale_event TO service_role;


--
-- TOC entry 4407 (class 0 OID 0)
-- Dependencies: 408
-- Name: TABLE sale_event_discount_tier; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event_discount_tier TO anon;
GRANT ALL ON TABLE public.sale_event_discount_tier TO authenticated;
GRANT ALL ON TABLE public.sale_event_discount_tier TO service_role;


--
-- TOC entry 4408 (class 0 OID 0)
-- Dependencies: 409
-- Name: TABLE sale_event_listing; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event_listing TO anon;
GRANT ALL ON TABLE public.sale_event_listing TO authenticated;
GRANT ALL ON TABLE public.sale_event_listing TO service_role;


--
-- TOC entry 4409 (class 0 OID 0)
-- Dependencies: 411
-- Name: TABLE seller_blocked_buyer; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.seller_blocked_buyer TO anon;
GRANT ALL ON TABLE public.seller_blocked_buyer TO authenticated;
GRANT ALL ON TABLE public.seller_blocked_buyer TO service_role;


--
-- TOC entry 4410 (class 0 OID 0)
-- Dependencies: 412
-- Name: TABLE seller_exempt_buyer; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.seller_exempt_buyer TO anon;
GRANT ALL ON TABLE public.seller_exempt_buyer TO authenticated;
GRANT ALL ON TABLE public.seller_exempt_buyer TO service_role;


--
-- TOC entry 4411 (class 0 OID 0)
-- Dependencies: 410
-- Name: TABLE seller_preference; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.seller_preference TO anon;
GRANT ALL ON TABLE public.seller_preference TO authenticated;
GRANT ALL ON TABLE public.seller_preference TO service_role;


--
-- TOC entry 4412 (class 0 OID 0)
-- Dependencies: 395
-- Name: TABLE shipping_policy; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.shipping_policy TO anon;
GRANT ALL ON TABLE public.shipping_policy TO authenticated;
GRANT ALL ON TABLE public.shipping_policy TO service_role;


--
-- TOC entry 4413 (class 0 OID 0)
-- Dependencies: 375
-- Name: TABLE shipping_services; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.shipping_services TO anon;
GRANT ALL ON TABLE public.shipping_services TO authenticated;
GRANT ALL ON TABLE public.shipping_services TO service_role;


--
-- TOC entry 4414 (class 0 OID 0)
-- Dependencies: 396
-- Name: TABLE store; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.store TO anon;
GRANT ALL ON TABLE public.store TO authenticated;
GRANT ALL ON TABLE public.store TO service_role;


--
-- TOC entry 4415 (class 0 OID 0)
-- Dependencies: 397
-- Name: TABLE store_subscription; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.store_subscription TO anon;
GRANT ALL ON TABLE public.store_subscription TO authenticated;
GRANT ALL ON TABLE public.store_subscription TO service_role;


--
-- TOC entry 4416 (class 0 OID 0)
-- Dependencies: 376
-- Name: TABLE "user"; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public."user" TO anon;
GRANT ALL ON TABLE public."user" TO authenticated;
GRANT ALL ON TABLE public."user" TO service_role;


--
-- TOC entry 4417 (class 0 OID 0)
-- Dependencies: 384
-- Name: TABLE variation; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.variation TO anon;
GRANT ALL ON TABLE public.variation TO authenticated;
GRANT ALL ON TABLE public.variation TO service_role;


--
-- TOC entry 4418 (class 0 OID 0)
-- Dependencies: 383
-- Name: SEQUENCE variation_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.variation_id_seq TO anon;
GRANT ALL ON SEQUENCE public.variation_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.variation_id_seq TO service_role;


--
-- TOC entry 4419 (class 0 OID 0)
-- Dependencies: 419
-- Name: TABLE voucher_transactions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.voucher_transactions TO anon;
GRANT ALL ON TABLE public.voucher_transactions TO authenticated;
GRANT ALL ON TABLE public.voucher_transactions TO service_role;


--
-- TOC entry 4420 (class 0 OID 0)
-- Dependencies: 418
-- Name: TABLE vouchers; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.vouchers TO anon;
GRANT ALL ON TABLE public.vouchers TO authenticated;
GRANT ALL ON TABLE public.vouchers TO service_role;


--
-- TOC entry 2653 (class 826 OID 16494)
-- Name: DEFAULT PRIVILEGES FOR SEQUENCES; Type: DEFAULT ACL; Schema: public; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES TO service_role;


--
-- TOC entry 2654 (class 826 OID 16495)
-- Name: DEFAULT PRIVILEGES FOR SEQUENCES; Type: DEFAULT ACL; Schema: public; Owner: supabase_admin
--

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES TO service_role;


--
-- TOC entry 2652 (class 826 OID 16493)
-- Name: DEFAULT PRIVILEGES FOR FUNCTIONS; Type: DEFAULT ACL; Schema: public; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS TO service_role;


--
-- TOC entry 2656 (class 826 OID 16497)
-- Name: DEFAULT PRIVILEGES FOR FUNCTIONS; Type: DEFAULT ACL; Schema: public; Owner: supabase_admin
--

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS TO service_role;


--
-- TOC entry 2651 (class 826 OID 16492)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: public; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES TO service_role;


--
-- TOC entry 2655 (class 826 OID 16496)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: public; Owner: supabase_admin
--

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES TO service_role;


-- Completed on 2026-03-18 15:32:00

--
-- PostgreSQL database dump complete
--

\unrestrict JfJKwA3P3WTO8dpbe4neCGaPS4FQGFqf8Q09xkAEhFGa4KcLXMzAS8vJtcLc8xC

