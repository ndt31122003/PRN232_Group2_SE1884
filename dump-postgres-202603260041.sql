--
-- PostgreSQL database dump
--

\restrict BE4SJx2GYbGFofUhRpdoJwDTrtCzZI5sgpo4ODCXovsInnroraLlDN714v2dziG

-- Dumped from database version 17.6
-- Dumped by pg_dump version 18.1

-- Started on 2026-03-26 00:41:37

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
-- TOC entry 4646 (class 0 OID 0)
-- Dependencies: 70
-- Name: SCHEMA public; Type: COMMENT; Schema: -; Owner: pg_database_owner
--

COMMENT ON SCHEMA public IS 'standard public schema';


--
-- TOC entry 526 (class 1255 OID 36025)
-- Name: update_support_tickets_updated_at(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.update_support_tickets_updated_at() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.update_support_tickets_updated_at() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 387 (class 1259 OID 32407)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- TOC entry 446 (class 1259 OID 36198)
-- Name: applied_order_discounts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.applied_order_discounts (
    id uuid NOT NULL,
    order_id uuid NOT NULL,
    order_discount_id uuid NOT NULL,
    discount_amount numeric(18,2) NOT NULL,
    applied_tier_id uuid,
    applied_at timestamp with time zone NOT NULL
);


ALTER TABLE public.applied_order_discounts OWNER TO postgres;

--
-- TOC entry 454 (class 1259 OID 37436)
-- Name: applied_sale_events; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.applied_sale_events (
    id uuid NOT NULL,
    order_id uuid NOT NULL,
    sale_event_id uuid NOT NULL,
    discount_tier_id uuid,
    discount_amount numeric(18,2) NOT NULL,
    applied_at timestamp with time zone NOT NULL
);


ALTER TABLE public.applied_sale_events OWNER TO postgres;

--
-- TOC entry 452 (class 1259 OID 37388)
-- Name: bids; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bids (
    id uuid NOT NULL,
    listing_id uuid NOT NULL,
    bidder_id character varying(450) NOT NULL,
    amount numeric(18,2) NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.bids OWNER TO postgres;

--
-- TOC entry 388 (class 1259 OID 32975)
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
-- TOC entry 400 (class 1259 OID 33069)
-- Name: category_condition; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.category_condition (
    category_id uuid NOT NULL,
    condition_id uuid NOT NULL
);


ALTER TABLE public.category_condition OWNER TO postgres;

--
-- TOC entry 399 (class 1259 OID 33057)
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
-- TOC entry 389 (class 1259 OID 32987)
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
-- TOC entry 426 (class 1259 OID 33409)
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
-- TOC entry 427 (class 1259 OID 33422)
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
-- TOC entry 434 (class 1259 OID 33572)
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
-- TOC entry 435 (class 1259 OID 33584)
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
-- TOC entry 436 (class 1259 OID 33596)
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
-- TOC entry 425 (class 1259 OID 33401)
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
-- TOC entry 432 (class 1259 OID 33536)
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
-- TOC entry 466 (class 1259 OID 37682)
-- Name: dispute_response; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dispute_response (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    dispute_id uuid NOT NULL,
    responder_id uuid NOT NULL,
    message text NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.dispute_response OWNER TO postgres;

--
-- TOC entry 4664 (class 0 OID 0)
-- Dependencies: 466
-- Name: TABLE dispute_response; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON TABLE public.dispute_response IS 'Stores conversation threads for disputes between buyers and sellers';


--
-- TOC entry 4665 (class 0 OID 0)
-- Dependencies: 466
-- Name: COLUMN dispute_response.dispute_id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.dispute_response.dispute_id IS 'Reference to the parent dispute';


--
-- TOC entry 4666 (class 0 OID 0)
-- Dependencies: 466
-- Name: COLUMN dispute_response.responder_id; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.dispute_response.responder_id IS 'User ID of the person responding (can be buyer or seller)';


--
-- TOC entry 4667 (class 0 OID 0)
-- Dependencies: 466
-- Name: COLUMN dispute_response.message; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.dispute_response.message IS 'The response message content';


--
-- TOC entry 4668 (class 0 OID 0)
-- Dependencies: 466
-- Name: COLUMN dispute_response.created_at; Type: COMMENT; Schema: public; Owner: postgres
--

COMMENT ON COLUMN public.dispute_response.created_at IS 'Timestamp when the response was created';


--
-- TOC entry 390 (class 1259 OID 32994)
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
-- TOC entry 443 (class 1259 OID 36126)
-- Name: inventory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inventory (
    id uuid NOT NULL,
    listing_id uuid NOT NULL,
    seller_id uuid NOT NULL,
    total_quantity integer NOT NULL,
    available_quantity integer NOT NULL,
    reserved_quantity integer NOT NULL,
    sold_quantity integer NOT NULL,
    threshold_quantity integer,
    is_low_stock boolean DEFAULT false NOT NULL,
    last_low_stock_notification_at timestamp with time zone,
    last_updated_at timestamp with time zone NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL,
    low_stock_email_enabled boolean DEFAULT false NOT NULL,
    additional_low_stock_emails character varying(1000) DEFAULT ''::character varying NOT NULL
);


ALTER TABLE public.inventory OWNER TO postgres;

--
-- TOC entry 444 (class 1259 OID 36134)
-- Name: inventory_adjustment; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inventory_adjustment (
    id uuid NOT NULL,
    inventory_id uuid NOT NULL,
    seller_id uuid NOT NULL,
    adjustment_type smallint NOT NULL,
    quantity_change integer NOT NULL,
    reason text,
    adjusted_at timestamp with time zone NOT NULL,
    adjusted_by text
);


ALTER TABLE public.inventory_adjustment OWNER TO postgres;

--
-- TOC entry 445 (class 1259 OID 36141)
-- Name: inventory_reservation; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.inventory_reservation (
    id uuid NOT NULL,
    inventory_id uuid NOT NULL,
    order_id uuid,
    buyer_id uuid NOT NULL,
    reservation_type smallint NOT NULL,
    quantity integer NOT NULL,
    reserved_at timestamp with time zone NOT NULL,
    expires_at timestamp with time zone NOT NULL,
    released_at timestamp with time zone,
    committed_at timestamp with time zone
);


ALTER TABLE public.inventory_reservation OWNER TO postgres;

--
-- TOC entry 391 (class 1259 OID 33001)
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
    is_deleted boolean NOT NULL,
    bids_count integer,
    return_policy_id uuid,
    shipping_policy_id uuid,
    watchers_count integer DEFAULT 0 NOT NULL,
    last_price_change_date timestamp with time zone
);


ALTER TABLE public.listing OWNER TO postgres;

--
-- TOC entry 409 (class 1259 OID 33152)
-- Name: listing_id; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.listing_id (
    listing_id uuid NOT NULL,
    seller_id uuid NOT NULL
);


ALTER TABLE public.listing_id OWNER TO postgres;

--
-- TOC entry 402 (class 1259 OID 33085)
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
-- TOC entry 401 (class 1259 OID 33084)
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
-- TOC entry 404 (class 1259 OID 33098)
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
-- TOC entry 403 (class 1259 OID 33097)
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
-- TOC entry 392 (class 1259 OID 33008)
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
-- TOC entry 467 (class 1259 OID 37705)
-- Name: notification; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.notification (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    user_id uuid NOT NULL,
    type character varying(50) NOT NULL,
    title character varying(255) NOT NULL,
    message character varying(1000) NOT NULL,
    reference_id uuid,
    is_read boolean DEFAULT false NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.notification OWNER TO postgres;

--
-- TOC entry 453 (class 1259 OID 37400)
-- Name: offers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.offers (
    id uuid NOT NULL,
    listing_id uuid NOT NULL,
    buyer_id character varying(450) NOT NULL,
    amount numeric(18,2) NOT NULL,
    status integer NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.offers OWNER TO postgres;

--
-- TOC entry 428 (class 1259 OID 33435)
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
-- TOC entry 420 (class 1259 OID 33331)
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
-- TOC entry 449 (class 1259 OID 36221)
-- Name: order_discount_category_rules; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_discount_category_rules (
    id uuid NOT NULL,
    order_discount_id uuid NOT NULL,
    category_id uuid NOT NULL,
    is_exclusion boolean NOT NULL
);


ALTER TABLE public.order_discount_category_rules OWNER TO postgres;

--
-- TOC entry 450 (class 1259 OID 36231)
-- Name: order_discount_item_rules; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_discount_item_rules (
    id uuid NOT NULL,
    order_discount_id uuid NOT NULL,
    listing_id uuid NOT NULL,
    is_exclusion boolean NOT NULL
);


ALTER TABLE public.order_discount_item_rules OWNER TO postgres;

--
-- TOC entry 447 (class 1259 OID 36203)
-- Name: order_discount_performance_metrics; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_discount_performance_metrics (
    id uuid NOT NULL,
    order_discount_id uuid NOT NULL,
    order_count integer DEFAULT 0 NOT NULL,
    total_discount_amount numeric(18,2) DEFAULT 0.0 NOT NULL,
    total_sales_revenue numeric(18,2) DEFAULT 0.0 NOT NULL,
    total_items_sold integer DEFAULT 0 NOT NULL,
    last_updated timestamp with time zone NOT NULL
);


ALTER TABLE public.order_discount_performance_metrics OWNER TO postgres;

--
-- TOC entry 451 (class 1259 OID 36241)
-- Name: order_discount_tiers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_discount_tiers (
    id uuid NOT NULL,
    order_discount_id uuid NOT NULL,
    threshold_value numeric(18,2) NOT NULL,
    discount_value numeric(18,2) NOT NULL,
    tier_order integer NOT NULL
);


ALTER TABLE public.order_discount_tiers OWNER TO postgres;

--
-- TOC entry 448 (class 1259 OID 36212)
-- Name: order_discounts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_discounts (
    id uuid NOT NULL,
    seller_id uuid NOT NULL,
    name character varying(200) NOT NULL,
    description character varying(1000),
    threshold_type integer NOT NULL,
    threshold_amount numeric(18,2),
    threshold_quantity integer,
    discount_value numeric(18,2) NOT NULL,
    discount_unit integer NOT NULL,
    max_discount numeric(18,2),
    apply_to_all_items boolean DEFAULT true NOT NULL,
    start_date timestamp with time zone NOT NULL,
    end_date timestamp with time zone NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean NOT NULL
);


ALTER TABLE public.order_discounts OWNER TO postgres;

--
-- TOC entry 424 (class 1259 OID 33382)
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
-- TOC entry 413 (class 1259 OID 33206)
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
-- TOC entry 421 (class 1259 OID 33343)
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
-- TOC entry 414 (class 1259 OID 33218)
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
-- TOC entry 415 (class 1259 OID 33230)
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
-- TOC entry 407 (class 1259 OID 33123)
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
-- TOC entry 393 (class 1259 OID 33015)
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
-- TOC entry 410 (class 1259 OID 33162)
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
-- TOC entry 394 (class 1259 OID 33022)
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
-- TOC entry 395 (class 1259 OID 33029)
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
-- TOC entry 411 (class 1259 OID 33179)
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
-- TOC entry 422 (class 1259 OID 33359)
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
-- TOC entry 423 (class 1259 OID 33366)
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
-- TOC entry 439 (class 1259 OID 34865)
-- Name: research_saved_category; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.research_saved_category (
    user_id uuid NOT NULL,
    category_id text NOT NULL,
    created_at timestamp with time zone DEFAULT now() NOT NULL
);


ALTER TABLE public.research_saved_category OWNER TO postgres;

--
-- TOC entry 416 (class 1259 OID 33291)
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
-- TOC entry 433 (class 1259 OID 33548)
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
-- TOC entry 396 (class 1259 OID 33036)
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
-- TOC entry 408 (class 1259 OID 33140)
-- Name: role_permissions; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.role_permissions (
    "Permission" text NOT NULL,
    role_id uuid NOT NULL
);


ALTER TABLE public.role_permissions OWNER TO postgres;

--
-- TOC entry 412 (class 1259 OID 33191)
-- Name: role_user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.role_user (
    roles_id uuid NOT NULL,
    user_id uuid NOT NULL
);


ALTER TABLE public.role_user OWNER TO postgres;

--
-- TOC entry 460 (class 1259 OID 37518)
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
    offer_free_shipping boolean DEFAULT false NOT NULL,
    include_skipped_items boolean DEFAULT false NOT NULL,
    block_price_increase_revisions boolean DEFAULT false NOT NULL,
    highlight_percentage numeric(5,2),
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean DEFAULT false NOT NULL
);


ALTER TABLE public.sale_event OWNER TO postgres;

--
-- TOC entry 461 (class 1259 OID 37529)
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
-- TOC entry 458 (class 1259 OID 37458)
-- Name: sale_event_discount_tiers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sale_event_discount_tiers (
    id uuid NOT NULL,
    sale_event_id uuid NOT NULL,
    discount_type integer NOT NULL,
    discount_value numeric(18,2) NOT NULL,
    priority integer NOT NULL,
    label character varying(100) NOT NULL
);


ALTER TABLE public.sale_event_discount_tiers OWNER TO postgres;

--
-- TOC entry 462 (class 1259 OID 37539)
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
-- TOC entry 459 (class 1259 OID 37468)
-- Name: sale_event_listings; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sale_event_listings (
    id uuid NOT NULL,
    sale_event_id uuid NOT NULL,
    discount_tier_id uuid NOT NULL,
    listing_id uuid NOT NULL,
    assigned_at timestamp with time zone NOT NULL
);


ALTER TABLE public.sale_event_listings OWNER TO postgres;

--
-- TOC entry 455 (class 1259 OID 37441)
-- Name: sale_event_performance_metrics; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sale_event_performance_metrics (
    id uuid NOT NULL,
    sale_event_id uuid NOT NULL,
    order_count integer NOT NULL,
    total_discount_amount numeric(18,2) NOT NULL,
    total_sales_revenue numeric(18,2) NOT NULL,
    total_items_sold integer NOT NULL,
    last_updated timestamp with time zone NOT NULL
);


ALTER TABLE public.sale_event_performance_metrics OWNER TO postgres;

--
-- TOC entry 456 (class 1259 OID 37446)
-- Name: sale_event_price_snapshots; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sale_event_price_snapshots (
    id uuid NOT NULL,
    sale_event_id uuid NOT NULL,
    listing_id uuid NOT NULL,
    original_price numeric(18,2) NOT NULL,
    snapshot_at timestamp with time zone NOT NULL
);


ALTER TABLE public.sale_event_price_snapshots OWNER TO postgres;

--
-- TOC entry 457 (class 1259 OID 37451)
-- Name: sale_events; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sale_events (
    id uuid NOT NULL,
    name character varying(200) NOT NULL,
    description character varying(1000),
    seller_id uuid NOT NULL,
    start_date timestamp with time zone NOT NULL,
    end_date timestamp with time zone NOT NULL,
    mode integer NOT NULL,
    highlight_percentage numeric(5,2),
    offer_free_shipping boolean NOT NULL,
    block_price_increase_revisions boolean NOT NULL,
    include_skipped_items boolean NOT NULL,
    buyer_message_label character varying(200),
    status integer NOT NULL,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone
);


ALTER TABLE public.sale_events OWNER TO postgres;

--
-- TOC entry 430 (class 1259 OID 33509)
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
-- TOC entry 431 (class 1259 OID 33520)
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
-- TOC entry 429 (class 1259 OID 33487)
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
-- TOC entry 463 (class 1259 OID 37607)
-- Name: shipping_discounts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.shipping_discounts (
    id uuid NOT NULL,
    seller_id uuid NOT NULL,
    name character varying(200) NOT NULL,
    description character varying(1000),
    discount_value numeric(18,2) NOT NULL,
    discount_unit integer NOT NULL,
    is_free_shipping boolean DEFAULT false NOT NULL,
    minimum_order_value numeric(18,2),
    start_date timestamp with time zone NOT NULL,
    end_date timestamp with time zone NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean DEFAULT false NOT NULL
);


ALTER TABLE public.shipping_discounts OWNER TO postgres;

--
-- TOC entry 417 (class 1259 OID 33298)
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
-- TOC entry 397 (class 1259 OID 33043)
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
-- TOC entry 418 (class 1259 OID 33305)
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
-- TOC entry 419 (class 1259 OID 33312)
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
-- TOC entry 440 (class 1259 OID 36007)
-- Name: support_tickets; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.support_tickets (
    id uuid DEFAULT gen_random_uuid() NOT NULL,
    seller_id character varying(255) NOT NULL,
    category character varying(100) NOT NULL,
    subject character varying(255) NOT NULL,
    message text NOT NULL,
    status character varying(50) DEFAULT 'OPEN'::character varying NOT NULL,
    created_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    updated_at timestamp with time zone DEFAULT CURRENT_TIMESTAMP,
    is_deleted boolean DEFAULT false
);


ALTER TABLE public.support_tickets OWNER TO postgres;

--
-- TOC entry 398 (class 1259 OID 33050)
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
-- TOC entry 406 (class 1259 OID 33111)
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
-- TOC entry 405 (class 1259 OID 33110)
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
-- TOC entry 465 (class 1259 OID 37626)
-- Name: volume_pricing_tiers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.volume_pricing_tiers (
    id uuid NOT NULL,
    volume_pricing_id uuid NOT NULL,
    min_quantity integer NOT NULL,
    discount_value numeric(18,2) NOT NULL,
    discount_unit integer NOT NULL
);


ALTER TABLE public.volume_pricing_tiers OWNER TO postgres;

--
-- TOC entry 464 (class 1259 OID 37617)
-- Name: volume_pricings; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.volume_pricings (
    id uuid NOT NULL,
    seller_id uuid NOT NULL,
    listing_id uuid,
    name character varying(200) NOT NULL,
    description character varying(1000),
    start_date timestamp with time zone NOT NULL,
    end_date timestamp with time zone NOT NULL,
    is_active boolean DEFAULT true NOT NULL,
    created_at timestamp with time zone NOT NULL,
    created_by text,
    updated_at timestamp with time zone,
    updated_by text,
    is_deleted boolean DEFAULT false NOT NULL
);


ALTER TABLE public.volume_pricings OWNER TO postgres;

--
-- TOC entry 438 (class 1259 OID 33615)
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
-- TOC entry 437 (class 1259 OID 33608)
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
-- TOC entry 4562 (class 0 OID 32407)
-- Dependencies: 387
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
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260317020159_AddInventoryManagement', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260321150920_AddOrderDiscountSystem', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260321152107_FixOrderDiscountPendingChanges', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260321153313_AddListingPoliciesAndCounters', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260322094532_AddOffersAndBids', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260323074729_AddLastPriceChangeDateToListing', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260323160208_AddShippingDiscountAndVolumePricing', '9.0.8');
INSERT INTO public."__EFMigrationsHistory" VALUES ('20260325092437_AddPerformanceIndexes', '9.0.8');


--
-- TOC entry 4619 (class 0 OID 36198)
-- Dependencies: 446
-- Data for Name: applied_order_discounts; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4627 (class 0 OID 37436)
-- Dependencies: 454
-- Data for Name: applied_sale_events; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4625 (class 0 OID 37388)
-- Dependencies: 452
-- Data for Name: bids; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.bids VALUES ('2f06099e-4887-4303-875f-99d2341b5e32', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 110.00, '2026-03-22 11:15:37.294825+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('68e16d1e-3620-488f-86a6-5c482bf31e6e', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 120.00, '2026-03-22 11:36:42.133654+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('c2c183dc-4503-43d8-a45c-4d85805d8680', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 130.00, '2026-03-24 01:30:07.56819+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('77cabe49-fa23-4d39-9d66-bd451c5b6809', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 135.00, '2026-03-24 01:36:16.054091+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('f6ebbb1d-1940-4694-ac40-b64c681399ea', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 140.00, '2026-03-24 01:57:34.151306+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('a2b70717-98ae-4316-8bf5-94bcd1d50bda', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 145.00, '2026-03-24 01:58:39.204471+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('00da3c78-ec66-4f0b-847b-58618f0595ba', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 150.00, '2026-03-24 02:01:47.847741+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('4d9f1999-101e-475c-84cb-831676de0af7', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 155.00, '2026-03-24 02:21:07.410398+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('b6d842d8-e2d2-40f7-bcda-5597315e62d5', '81680948-d9af-4a81-a649-4ce9e3a63071', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 160.00, '2026-03-24 02:21:36.934018+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('05bf43d4-dec9-49b7-a88c-a9bec3a32edb', '87233f8b-3f4c-427b-a300-83720258ed62', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 600.00, '2026-03-25 10:04:17.003099+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('e0c8764a-59ae-4e62-88fb-98cb3e8bd320', '87233f8b-3f4c-427b-a300-83720258ed62', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 610.00, '2026-03-25 15:25:45.659579+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('9b08ce61-5cf9-4a1d-bf71-e1a3c1cc7d14', '87233f8b-3f4c-427b-a300-83720258ed62', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 615.00, '2026-03-25 15:26:34.560631+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.bids VALUES ('262d23a2-7847-45d1-b20c-e9a8c8135c9f', '6d52344e-32cc-4444-a35d-25340a46f590', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 110.00, '2026-03-25 17:11:34.384982+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);


--
-- TOC entry 4563 (class 0 OID 32975)
-- Dependencies: 388
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
-- TOC entry 4575 (class 0 OID 33069)
-- Dependencies: 400
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
-- TOC entry 4574 (class 0 OID 33057)
-- Dependencies: 399
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
-- TOC entry 4564 (class 0 OID 32987)
-- Dependencies: 389
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
-- TOC entry 4601 (class 0 OID 33409)
-- Dependencies: 426
-- Data for Name: coupon; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.coupon VALUES ('d5e03492-fd5b-4293-978e-b17a483874d8', 'cfa2e0f1-b720-4590-a7d4-4ce0844f9671', '10000000-0000-0000-0000-000000000001', '6a0bf519-a43b-4f4c-8029-350483e7ffae', 'test', 'TEST123', 2.00, 'AMOUNT', NULL, '2026-03-20 14:38:00+00', '2026-04-03 14:38:00+00', NULL, NULL, 20.00, NULL, NULL, true, '2026-03-20 14:29:36.783272+00', '6a0bf519-a43b-4f4c-8029-350483e7ffae', '2026-03-20 14:30:06.008022+00', '6a0bf519-a43b-4f4c-8029-350483e7ffae', false);
INSERT INTO public.coupon VALUES ('5ee7fa35-ba42-48c7-80e9-69c4868a37e2', '51f2ed38-06bb-496e-b5cb-7aa3057c21b7', NULL, 'db062e86-b522-41c2-b3b9-df461d830de8', 'test', 'TESST', 4.00, 'AMOUNT', NULL, '2026-03-21 15:38:00+00', '2026-04-04 15:38:00+00', NULL, NULL, NULL, NULL, NULL, true, '2026-03-21 15:28:49.873763+00', 'db062e86-b522-41c2-b3b9-df461d830de8', '2026-03-21 15:36:37.11355+00', 'db062e86-b522-41c2-b3b9-df461d830de8', false);
INSERT INTO public.coupon VALUES ('0d0d8d12-9e1e-43a7-9d4c-807cbcaff367', '9e1d4ea5-5b09-48be-be90-e2790f6ba537', '10000000-0000-0000-0000-000000000001', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Summer Holiday', 'HAILN', 10.00, 'PERCENT', 30.00, '2026-03-23 15:27:00+00', '2026-04-06 15:27:00+00', NULL, NULL, NULL, NULL, NULL, true, '2026-03-23 15:18:57.883405+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.coupon VALUES ('0a90075b-9bc0-4f95-abe6-ffa81d2b73a0', '9e1d4ea5-5b09-48be-be90-e2790f6ba537', NULL, '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'aaa', 'AAA', 33.00, 'PERCENT', 33.00, '2026-03-23 15:04:00+00', '2026-04-06 15:04:00+00', 333, 3, NULL, 33.00, 333.00, true, '2026-03-23 14:55:14.184369+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2026-03-23 16:32:10.448417+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', false);
INSERT INTO public.coupon VALUES ('b3badc49-eb7d-4324-8199-2a69a62ef8e8', '3b980145-62b6-4ae6-9cf8-7838bc7b84e0', NULL, 'd9814bda-f007-413a-9d73-256bbf43ced4', 'aa', 'AAAA', 2.00, 'AMOUNT', NULL, '2026-03-25 17:43:00+00', '2026-04-08 17:43:00+00', NULL, NULL, NULL, NULL, NULL, true, '2026-03-25 17:33:32.543455+00', 'd9814bda-f007-413a-9d73-256bbf43ced4', '2026-03-25 17:33:41.595525+00', 'd9814bda-f007-413a-9d73-256bbf43ced4', true);


--
-- TOC entry 4602 (class 0 OID 33422)
-- Dependencies: 427
-- Data for Name: coupon_condition; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.coupon_condition VALUES ('a25f67ac-1b04-4a95-bd23-b0b7576e3a3d', '5ee7fa35-ba42-48c7-80e9-69c4868a37e2', NULL, NULL, NULL, 3.00, NULL, 'teesst');


--
-- TOC entry 4609 (class 0 OID 33572)
-- Dependencies: 434
-- Data for Name: coupon_excluded_categories; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4610 (class 0 OID 33584)
-- Dependencies: 435
-- Data for Name: coupon_excluded_items; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4611 (class 0 OID 33596)
-- Dependencies: 436
-- Data for Name: coupon_target_audiences; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4600 (class 0 OID 33401)
-- Dependencies: 425
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
-- TOC entry 4607 (class 0 OID 33536)
-- Dependencies: 432
-- Data for Name: dispute; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.dispute VALUES ('450e8400-e29b-41d4-a716-446655440004', '72000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000001', 'Sản phẩm giao hàng chậm hơn 2 tuần so với cam kết. Đã liên hệ seller nhiều lần nhưng không có phản hồi thỏa đáng.', 'Escalated', '2026-03-12 18:07:30.820331+00', NULL, '2026-03-16 18:07:30.820331+00', NULL, false);
INSERT INTO public.dispute VALUES ('450e8400-e29b-41d4-a716-446655440005', '72000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000003', 'Vấn đề về màu sắc sản phẩm không đúng như hình ảnh. Seller đã đồng ý hoàn tiền một phần và khách hàng chấp nhận.', 'Resolved', '2026-03-05 18:07:30.820331+00', NULL, '2026-03-12 18:07:30.820331+00', NULL, false);
INSERT INTO public.dispute VALUES ('450e8400-e29b-41d4-a716-446655440006', '73000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000001', 'Khiếu nại về chất lượng đóng gói. Sản phẩm bị hư hại trong quá trình vận chuyển. Đã được giải quyết thông qua bảo hiểm vận chuyển.', 'Closed', '2026-02-26 18:07:30.820331+00', NULL, '2026-03-05 18:07:30.820331+00', NULL, false);
INSERT INTO public.dispute VALUES ('450e8400-e29b-41d4-a716-446655440007', '73000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002', 'Sản phẩm không hoạt động như mô tả. Đã thử theo hướng dẫn nhưng vẫn không được. Yêu cầu seller kiểm tra và hỗ trợ.', 'Open', '2026-03-19 10:07:30.820331+00', NULL, '2026-03-19 10:07:30.820331+00', NULL, false);
INSERT INTO public.dispute VALUES ('450e8400-e29b-41d4-a716-446655440002', '71000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000003', 'Laptop không khởi động được sau khi nhận hàng. Đã thử nhiều cách nhưng không thể sử dụng. Nghi ngờ có vấn đề về phần cứng. Cần được hoàn tiền.', 'Resolved', '2026-03-18 18:07:30.820331+00', NULL, '2026-03-23 22:58:22.889706+00', '70000000-0000-0000-0000-000000000001', false);
INSERT INTO public.dispute VALUES ('450e8400-e29b-41d4-a716-446655440003', '71000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002', 'Máy pha cà phê bị hỏng motor sau 2 ngày sử dụng. Seller đã cung cấp video hướng dẫn sử dụng nhưng vấn đề vẫn không được giải quyết.', 'Resolved', '2026-03-14 18:07:30.820331+00', NULL, '2026-03-24 00:55:55.856885+00', '70000000-0000-0000-0000-000000000002', false);
INSERT INTO public.dispute VALUES ('450e8400-e29b-41d4-a716-446655440001', '71000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 'Sản phẩm nhận được không đúng như mô tả. Điện thoại có vết xước nhưng được quảng cáo là "Brand new condition". Yêu cầu hoàn tiền hoặc đổi sản phẩm mới.', 'WaitingSeller', '2026-03-17 18:07:30.820331+00', NULL, '2026-03-24 01:06:06.961377+00', '70000000-0000-0000-0000-000000000002', false);


--
-- TOC entry 4639 (class 0 OID 37682)
-- Dependencies: 466
-- Data for Name: dispute_response; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.dispute_response VALUES ('550e8400-e29b-41d4-a716-446655440001', '450e8400-e29b-41d4-a716-446655440001', '70000000-0000-0000-0000-000000000001', 'Xin lỗi về sự bất tiện này. Tôi đã kiểm tra lại sản phẩm trước khi gửi và không thấy vết xước nào. Có thể bị hư hại trong quá trình vận chuyển. Tôi sẵn sàng hoàn tiền hoặc gửi sản phẩm thay thế.', '2026-03-22 22:25:24.582675+00');
INSERT INTO public.dispute_response VALUES ('550e8400-e29b-41d4-a716-446655440002', '450e8400-e29b-41d4-a716-446655440001', '70000000-0000-0000-0000-000000000002', 'Cảm ơn bạn đã phản hồi. Tôi muốn được hoàn tiền vì tôi đã tìm được sản phẩm khác rồi. Xin vui lòng xử lý hoàn tiền cho tôi.', '2026-03-23 10:25:24.582675+00');
INSERT INTO public.dispute_response VALUES ('550e8400-e29b-41d4-a716-446655440003', '450e8400-e29b-41d4-a716-446655440002', '70000000-0000-0000-0000-000000000001', 'Tôi rất tiếc về vấn đề này. Laptop đã được test kỹ trước khi gửi. Tôi sẽ gửi video hướng dẫn khắc phục sự cố. Nếu vẫn không được, tôi sẽ chấp nhận hoàn tiền.', '2026-03-23 16:25:24.582675+00');
INSERT INTO public.dispute_response VALUES ('550e8400-e29b-41d4-a716-446655440004', '450e8400-e29b-41d4-a716-446655440004', '70000000-0000-0000-0000-000000000002', 'Xin lỗi về việc giao hàng chậm. Đã có sự cố với đơn vị vận chuyển. Tôi sẵn sàng bồi thường một phần chi phí vận chuyển cho bạn.', '2026-03-19 22:25:24.582675+00');
INSERT INTO public.dispute_response VALUES ('f2faf330-12c0-4a38-8765-ae27d7117864', '450e8400-e29b-41d4-a716-446655440002', '70000000-0000-0000-0000-000000000001', 'Bạn xem kĩ lại giúp tôi xem', '2026-03-23 22:27:35.119883+00');
INSERT INTO public.dispute_response VALUES ('487ea8f5-ec35-4761-b2b7-94c02a48d082', '450e8400-e29b-41d4-a716-446655440002', '70000000-0000-0000-0000-000000000001', 'bạn để mình kiểm tra lại cho mình nhé', '2026-03-23 22:30:55.501719+00');
INSERT INTO public.dispute_response VALUES ('fb130a13-a72b-4f60-925d-099a7b17b306', '450e8400-e29b-41d4-a716-446655440003', '70000000-0000-0000-0000-000000000001', 'sorry bạn nha. Bạn có thể cung cấp thêm thông tin để mình kiểm tra lại đc không', '2026-03-23 23:12:55.783667+00');
INSERT INTO public.dispute_response VALUES ('293ee698-0eba-47f2-b7e6-932f34caa7ca', '450e8400-e29b-41d4-a716-446655440003', '70000000-0000-0000-0000-000000000002', 'ok sốp ơi', '2026-03-23 23:13:33.323784+00');
INSERT INTO public.dispute_response VALUES ('7b909b7d-0d43-439f-b567-0aebca75b785', '450e8400-e29b-41d4-a716-446655440003', '70000000-0000-0000-0000-000000000002', 'hello sốp', '2026-03-23 23:19:09.802228+00');
INSERT INTO public.dispute_response VALUES ('39cf88b5-b19e-4d33-8957-3160baa2f86d', '450e8400-e29b-41d4-a716-446655440003', '70000000-0000-0000-0000-000000000001', 'hello bạn', '2026-03-23 23:20:06.208235+00');
INSERT INTO public.dispute_response VALUES ('91243f2f-8b50-4a79-9c9b-8e65bf54427d', '450e8400-e29b-41d4-a716-446655440003', '70000000-0000-0000-0000-000000000002', 'hello', '2026-03-23 23:25:26.146124+00');
INSERT INTO public.dispute_response VALUES ('78588cc9-61fc-417d-aa91-88c3802a1978', '450e8400-e29b-41d4-a716-446655440003', '70000000-0000-0000-0000-000000000001', 'Hello buyer', '2026-03-23 23:26:31.652425+00');
INSERT INTO public.dispute_response VALUES ('fd58717b-8412-4f8f-800a-8cd3401339d3', '450e8400-e29b-41d4-a716-446655440003', '70000000-0000-0000-0000-000000000002', 'Hello 123', '2026-03-23 23:39:10.130559+00');


--
-- TOC entry 4565 (class 0 OID 32994)
-- Dependencies: 390
-- Data for Name: file_metadata; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.file_metadata VALUES ('660e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440001', '/uploads/support/upload_error_screenshot.png', 'upload_error_screenshot.png', 'image/png', 245760, '2026-03-19 16:06:40.616869+00', NULL, '2026-03-19 16:06:40.616869+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('660e8400-e29b-41d4-a716-446655440002', '550e8400-e29b-41d4-a716-446655440003', '/uploads/support/tracking_details.pdf', 'tracking_details.pdf', 'application/pdf', 102400, '2026-03-16 18:06:40.616869+00', NULL, '2026-03-16 18:06:40.616869+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('660e8400-e29b-41d4-a716-446655440003', '550e8400-e29b-41d4-a716-446655440003', '/uploads/support/customer_complaint_email.pdf', 'customer_complaint_email.pdf', 'application/pdf', 87500, '2026-03-16 18:06:40.616869+00', NULL, '2026-03-16 18:06:40.616869+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('660e8400-e29b-41d4-a716-446655440004', '550e8400-e29b-41d4-a716-446655440006', '/uploads/support/template_error_log.txt', 'template_error_log.txt', 'text/plain', 15600, '2026-03-17 18:06:40.616869+00', NULL, '2026-03-17 18:06:40.616869+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('660e8400-e29b-41d4-a716-446655440005', '550e8400-e29b-41d4-a716-446655440007', '/uploads/support/return_request_email.pdf', 'return_request_email.pdf', 'application/pdf', 125000, '2026-03-19 06:06:40.616869+00', NULL, '2026-03-19 06:06:40.616869+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440001', '450e8400-e29b-41d4-a716-446655440001', '/uploads/disputes/product_damage_photo1.jpg', 'product_damage_photo1.jpg', 'image/jpeg', 1024000, '2026-03-17 18:07:31.008479+00', NULL, '2026-03-17 18:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440002', '450e8400-e29b-41d4-a716-446655440001', '/uploads/disputes/product_damage_photo2.jpg', 'product_damage_photo2.jpg', 'image/jpeg', 987500, '2026-03-17 18:07:31.008479+00', NULL, '2026-03-17 18:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440003', '450e8400-e29b-41d4-a716-446655440002', '/uploads/disputes/laptop_error_video.mp4', 'laptop_error_video.mp4', 'video/mp4', 15600000, '2026-03-18 18:07:31.008479+00', NULL, '2026-03-18 18:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440004', '450e8400-e29b-41d4-a716-446655440002', '/uploads/disputes/purchase_receipt.pdf', 'purchase_receipt.pdf', 'application/pdf', 245000, '2026-03-18 18:07:31.008479+00', NULL, '2026-03-18 18:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440005', '450e8400-e29b-41d4-a716-446655440003', '/uploads/disputes/usage_instructions_video.mp4', 'usage_instructions_video.mp4', 'video/mp4', 8900000, '2026-03-16 18:07:31.008479+00', NULL, '2026-03-16 18:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440006', '450e8400-e29b-41d4-a716-446655440003', '/uploads/disputes/warranty_document.pdf', 'warranty_document.pdf', 'application/pdf', 156000, '2026-03-16 18:07:31.008479+00', NULL, '2026-03-16 18:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440007', '450e8400-e29b-41d4-a716-446655440004', '/uploads/disputes/shipping_tracking_history.pdf', 'shipping_tracking_history.pdf', 'application/pdf', 89000, '2026-03-12 18:07:31.008479+00', NULL, '2026-03-12 18:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440008', '450e8400-e29b-41d4-a716-446655440004', '/uploads/disputes/email_correspondence.pdf', 'email_correspondence.pdf', 'application/pdf', 234000, '2026-03-14 18:07:31.008479+00', NULL, '2026-03-14 18:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('770e8400-e29b-41d4-a716-446655440009', '450e8400-e29b-41d4-a716-446655440007', '/uploads/disputes/product_test_video.mp4', 'product_test_video.mp4', 'video/mp4', 12500000, '2026-03-19 10:07:31.008479+00', NULL, '2026-03-19 10:07:31.008479+00', NULL, false);
INSERT INTO public.file_metadata VALUES ('b9b6935e-af08-4f1b-9648-70a6ded1ca01', '450e8400-e29b-41d4-a716-446655440003', '/storage/uploads/8d5c7b73-89d2-4d0d-8c64-a49cbcb6f4fa_6ab6262f-84e8-46ce-86c3-2f2a1786d744.jpg', '8d5c7b73-89d2-4d0d-8c64-a49cbcb6f4fa.jpg', 'image/jpeg', 181169, '2026-03-23 23:57:05.95452+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.file_metadata VALUES ('10f1384f-e3e0-4dc6-829a-78a26a5dcb6c', '68deaa3f-f804-4336-9e4a-45589ac1165c', '/storage/uploads/8d5c7b73-89d2-4d0d-8c64-a49cbcb6f4fa_cc969d10-2bdd-42fe-8542-7aad15b2d434.jpg', '8d5c7b73-89d2-4d0d-8c64-a49cbcb6f4fa.jpg', 'image/jpeg', 181169, '2026-03-24 01:54:45.254961+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);


--
-- TOC entry 4616 (class 0 OID 36126)
-- Dependencies: 443
-- Data for Name: inventory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.inventory VALUES ('91000000-0000-0000-0000-000000000002', '71000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 10, 3, 2, 5, 4, true, '2026-03-25 17:15:54.382299+00', '2026-03-25 17:15:54.382299+00', '2026-03-25 15:59:26.261044+00', 'seed', '2026-03-25 17:15:54.54914+00', 'System', false, true, '');
INSERT INTO public.inventory VALUES ('91000000-0000-0000-0000-000000000001', '71000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000001', 24, 18, 4, 2, 6, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 15:59:26.261044+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000003', '71000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 20, 0, 0, 20, 2, true, '2026-03-25 05:10:08.457371+00', '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91000000-0000-0000-0000-000000000004', '72000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000002', 10, 3, 2, 5, 4, true, '2026-03-25 17:15:58.983627+00', '2026-03-25 17:15:58.983627+00', '2026-03-25 15:59:26.261044+00', 'seed', '2026-03-25 17:15:58.990641+00', 'System', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000006a', '72000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000002', 30, 10, 10, 10, 12, true, '2026-03-25 17:16:12.729074+00', '2026-03-25 17:16:12.729074+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:16:12.730171+00', 'System', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000000b', '71000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000001', 22, 17, 1, 4, 5, false, NULL, '2026-03-25 17:19:28.671986+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:19:28.672206+00', '70000000-0000-0000-0000-000000000001', false, true, 'thanhhui1709@gmail.com');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000006', '71000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000001', 35, 15, 10, 10, 12, true, '2026-03-25 17:16:07.660853+00', '2026-03-25 17:34:52.867363+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:34:52.927031+00', '70000000-0000-0000-0000-000000000001', false, true, 'kophaithanhhui@gmail.com');
INSERT INTO public.inventory VALUES ('9cf5b530-169c-4ece-98c8-fdbd33a96287', 'b9cddcbe-3170-4592-a266-d1d609f57384', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 11010, 11010, 0, 0, NULL, false, NULL, '2026-03-25 16:15:28.543399+00', '2026-03-25 16:15:29.833655+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, false, '');
INSERT INTO public.inventory VALUES ('afb54866-0722-4e8f-81eb-25a4578563dd', '6d52344e-32cc-4444-a35d-25340a46f590', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 1, 1, 0, 0, NULL, false, NULL, '2026-03-25 16:15:30.496217+00', '2026-03-25 16:15:31.914845+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000004', '71000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000001', 16, 4, 5, 7, 6, true, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000005', '71000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000001', 14, 9, 0, 5, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000007', '71000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000001', 24, 24, 0, 0, 8, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000008', '71000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000001', 25, 23, 1, 1, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000009', '71000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000001', 26, 24, 0, 2, 8, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000000a', '71000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000001', 18, 15, 0, 3, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000000c', '71000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000001', 20, 13, 2, 5, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000000d', '71000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000001', 21, 21, 0, 0, 8, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000000e', '71000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000001', 22, 20, 1, 1, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000000f', '71000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000001', 23, 19, 2, 2, 8, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000010', '71000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000001', 24, 21, 0, 3, 5, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000011', '71000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000001', 25, 21, 0, 4, 8, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000012', '71000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000001', 26, 19, 2, 5, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000013', '71000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000001', 18, 18, 0, 0, 8, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000014', '71000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000001', 19, 17, 1, 1, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000015', '71000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000001', 20, 18, 0, 2, 5, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000016', '71000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000001', 21, 18, 0, 3, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000017', '71000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000001', 22, 17, 1, 4, 8, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000018', '71000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000001', 23, 16, 2, 5, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91000000-0000-0000-0000-000000000003', '72000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 24, 18, 4, 2, 6, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 15:59:26.261044+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000067', '72000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', 20, 0, 0, 20, 2, true, '2026-03-25 05:10:08.457371+00', '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000068', '72000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000002', 16, 4, 5, 7, 6, true, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000069', '72000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002', 14, 9, 0, 5, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000006b', '72000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000002', 28, 26, 1, 1, 9, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000006c', '72000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000002', 29, 25, 2, 2, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000006d', '72000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000002', 30, 27, 0, 3, 9, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000006e', '72000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000002', 22, 17, 1, 4, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000006f', '72000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000002', 23, 16, 2, 5, 6, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000070', '72000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000002', 24, 15, 3, 6, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000071', '72000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000002', 25, 24, 0, 1, 9, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000072', '72000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000002', 26, 22, 2, 2, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000073', '72000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000002', 27, 21, 3, 3, 9, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000074', '72000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000002', 28, 23, 1, 4, 6, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000075', '72000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000002', 29, 24, 0, 5, 9, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000076', '72000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000002', 30, 21, 3, 6, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000077', '72000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000002', 22, 20, 1, 1, 9, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000078', '72000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000002', 23, 19, 2, 2, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-000000000079', '72000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000002', 24, 21, 0, 3, 6, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000007a', '72000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000002', 25, 20, 1, 4, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000007b', '72000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000002', 26, 19, 2, 5, 9, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-00000000007c', '72000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000002', 27, 18, 3, 6, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91000000-0000-0000-0000-000000000005', '73000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 24, 18, 4, 2, 6, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 15:59:26.261044+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000cb', '73000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000003', 20, 0, 0, 20, 2, true, '2026-03-25 05:10:08.457371+00', '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000cc', '73000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000003', 16, 4, 5, 7, 6, true, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000cd', '73000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000003', 14, 9, 0, 5, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000cf', '73000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000003', 32, 28, 2, 2, 10, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d0', '73000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000003', 33, 27, 3, 3, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d1', '73000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000003', 34, 30, 0, 4, 10, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d2', '73000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000003', 26, 19, 2, 5, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d3', '73000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000003', 27, 18, 3, 6, 7, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d4', '73000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000003', 28, 17, 4, 7, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d5', '73000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000003', 29, 27, 0, 2, 10, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d6', '73000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000003', 30, 24, 3, 3, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d7', '73000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000003', 31, 23, 4, 4, 10, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d8', '73000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000003', 32, 25, 2, 5, 7, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000d9', '73000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000003', 33, 27, 0, 6, 10, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000da', '73000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000003', 34, 23, 4, 7, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000db', '73000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000003', 26, 22, 2, 2, 10, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000dc', '73000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000003', 27, 21, 3, 3, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000dd', '73000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000003', 28, 24, 0, 4, 7, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000de', '73000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000003', 29, 22, 2, 5, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000df', '73000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000003', 30, 21, 3, 6, 10, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000e0', '73000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000003', 31, 20, 4, 7, NULL, false, NULL, '2026-03-25 17:10:08.457371+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:10:08.457371+00', 'seed', false, false, '');
INSERT INTO public.inventory VALUES ('91000000-0000-0000-0000-000000000006', '73000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 10, 3, 2, 5, 4, true, '2026-03-25 17:16:03.421138+00', '2026-03-25 17:16:03.421138+00', '2026-03-25 15:59:26.261044+00', 'seed', '2026-03-25 17:16:03.423487+00', 'System', false, true, '');
INSERT INTO public.inventory VALUES ('91500000-0000-0000-0000-0000000000ce', '73000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000003', 30, 10, 10, 10, 12, true, '2026-03-25 17:16:16.212291+00', '2026-03-25 17:16:16.212291+00', '2026-03-25 16:05:23.795666+00', 'seed', '2026-03-25 17:16:16.213171+00', 'System', false, true, '');


--
-- TOC entry 4617 (class 0 OID 36134)
-- Dependencies: 444
-- Data for Name: inventory_adjustment; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4618 (class 0 OID 36141)
-- Dependencies: 445
-- Data for Name: inventory_reservation; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000001', '91000000-0000-0000-0000-000000000001', NULL, '70000000-0000-0000-0000-000000000002', 0, 2, '2026-03-25 16:55:08.457371+00', '2026-03-25 17:30:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000002', '91000000-0000-0000-0000-000000000001', NULL, '70000000-0000-0000-0000-000000000003', 0, 2, '2026-03-25 16:48:08.457371+00', '2026-03-25 17:40:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000000b', '91000000-0000-0000-0000-000000000002', NULL, '70000000-0000-0000-0000-000000000003', 0, 2, '2026-03-25 16:54:08.457371+00', '2026-03-25 17:31:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000001f', '91500000-0000-0000-0000-000000000004', NULL, '70000000-0000-0000-0000-000000000002', 0, 2, '2026-03-25 16:52:08.457371+00', '2026-03-25 17:33:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000020', '91500000-0000-0000-0000-000000000004', NULL, '70000000-0000-0000-0000-000000000003', 0, 3, '2026-03-25 16:45:08.457371+00', '2026-03-25 17:43:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000033', '91500000-0000-0000-0000-000000000006', NULL, '70000000-0000-0000-0000-000000000001', 0, 5, '2026-03-25 16:50:08.457371+00', '2026-03-25 17:35:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000034', '91500000-0000-0000-0000-000000000006', NULL, '70000000-0000-0000-0000-000000000002', 0, 5, '2026-03-25 16:43:08.457371+00', '2026-03-25 17:45:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000047', '91500000-0000-0000-0000-000000000008', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:48:08.457371+00', '2026-03-25 17:37:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000065', '91500000-0000-0000-0000-00000000000b', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:45:08.457371+00', '2026-03-25 17:40:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000006f', '91500000-0000-0000-0000-00000000000c', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:44:08.457371+00', '2026-03-25 17:41:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000083', '91500000-0000-0000-0000-00000000000e', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:42:08.457371+00', '2026-03-25 17:43:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000008d', '91500000-0000-0000-0000-00000000000f', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:41:08.457371+00', '2026-03-25 17:44:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000000ab', '91500000-0000-0000-0000-000000000012', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:38:08.457371+00', '2026-03-25 17:47:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000000bf', '91500000-0000-0000-0000-000000000014', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:36:08.457371+00', '2026-03-25 17:49:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000000dd', '91500000-0000-0000-0000-000000000017', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:33:08.457371+00', '2026-03-25 17:52:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000000e7', '91500000-0000-0000-0000-000000000018', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:32:08.457371+00', '2026-03-25 17:53:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000003e9', '91000000-0000-0000-0000-000000000003', NULL, '70000000-0000-0000-0000-000000000003', 0, 2, '2026-03-25 16:55:08.457371+00', '2026-03-25 17:30:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000003ea', '91000000-0000-0000-0000-000000000003', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:48:08.457371+00', '2026-03-25 17:40:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000003f3', '91000000-0000-0000-0000-000000000004', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:54:08.457371+00', '2026-03-25 17:31:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000407', '91500000-0000-0000-0000-000000000068', NULL, '70000000-0000-0000-0000-000000000003', 0, 2, '2026-03-25 16:52:08.457371+00', '2026-03-25 17:33:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000408', '91500000-0000-0000-0000-000000000068', NULL, '70000000-0000-0000-0000-000000000001', 0, 3, '2026-03-25 16:45:08.457371+00', '2026-03-25 17:43:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000041b', '91500000-0000-0000-0000-00000000006a', NULL, '70000000-0000-0000-0000-000000000002', 0, 5, '2026-03-25 16:50:08.457371+00', '2026-03-25 17:35:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000041c', '91500000-0000-0000-0000-00000000006a', NULL, '70000000-0000-0000-0000-000000000003', 0, 5, '2026-03-25 16:43:08.457371+00', '2026-03-25 17:45:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000425', '91500000-0000-0000-0000-00000000006b', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:49:08.457371+00', '2026-03-25 17:36:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000042f', '91500000-0000-0000-0000-00000000006c', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:48:08.457371+00', '2026-03-25 17:37:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000443', '91500000-0000-0000-0000-00000000006e', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:46:08.457371+00', '2026-03-25 17:39:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000044d', '91500000-0000-0000-0000-00000000006f', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:45:08.457371+00', '2026-03-25 17:40:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000457', '91500000-0000-0000-0000-000000000070', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:44:08.457371+00', '2026-03-25 17:41:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000046b', '91500000-0000-0000-0000-000000000072', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:42:08.457371+00', '2026-03-25 17:43:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000475', '91500000-0000-0000-0000-000000000073', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:41:08.457371+00', '2026-03-25 17:44:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000047f', '91500000-0000-0000-0000-000000000074', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:40:08.457371+00', '2026-03-25 17:45:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000493', '91500000-0000-0000-0000-000000000076', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:38:08.457371+00', '2026-03-25 17:47:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000049d', '91500000-0000-0000-0000-000000000077', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:37:08.457371+00', '2026-03-25 17:48:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000004a7', '91500000-0000-0000-0000-000000000078', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:36:08.457371+00', '2026-03-25 17:49:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000004bb', '91500000-0000-0000-0000-00000000007a', NULL, '70000000-0000-0000-0000-000000000003', 0, 1, '2026-03-25 16:34:08.457371+00', '2026-03-25 17:51:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000004c5', '91500000-0000-0000-0000-00000000007b', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:33:08.457371+00', '2026-03-25 17:52:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000004cf', '91500000-0000-0000-0000-00000000007c', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:32:08.457371+00', '2026-03-25 17:53:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000007d1', '91000000-0000-0000-0000-000000000005', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:55:08.457371+00', '2026-03-25 17:30:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000007d2', '91000000-0000-0000-0000-000000000005', NULL, '70000000-0000-0000-0000-000000000002', 0, 2, '2026-03-25 16:48:08.457371+00', '2026-03-25 17:40:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000007db', '91000000-0000-0000-0000-000000000006', NULL, '70000000-0000-0000-0000-000000000002', 0, 2, '2026-03-25 16:54:08.457371+00', '2026-03-25 17:31:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000007ef', '91500000-0000-0000-0000-0000000000cc', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:52:08.457371+00', '2026-03-25 17:33:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000007f0', '91500000-0000-0000-0000-0000000000cc', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:45:08.457371+00', '2026-03-25 17:43:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000803', '91500000-0000-0000-0000-0000000000ce', NULL, '70000000-0000-0000-0000-000000000003', 0, 5, '2026-03-25 16:50:08.457371+00', '2026-03-25 17:35:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000804', '91500000-0000-0000-0000-0000000000ce', NULL, '70000000-0000-0000-0000-000000000001', 0, 5, '2026-03-25 16:43:08.457371+00', '2026-03-25 17:45:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000080d', '91500000-0000-0000-0000-0000000000cf', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:49:08.457371+00', '2026-03-25 17:36:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000817', '91500000-0000-0000-0000-0000000000d0', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:48:08.457371+00', '2026-03-25 17:37:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000082b', '91500000-0000-0000-0000-0000000000d2', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:46:08.457371+00', '2026-03-25 17:39:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000835', '91500000-0000-0000-0000-0000000000d3', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:45:08.457371+00', '2026-03-25 17:40:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000083f', '91500000-0000-0000-0000-0000000000d4', NULL, '70000000-0000-0000-0000-000000000003', 0, 2, '2026-03-25 16:44:08.457371+00', '2026-03-25 17:41:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000840', '91500000-0000-0000-0000-0000000000d4', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:37:08.457371+00', '2026-03-25 17:51:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000853', '91500000-0000-0000-0000-0000000000d6', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:42:08.457371+00', '2026-03-25 17:43:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000085d', '91500000-0000-0000-0000-0000000000d7', NULL, '70000000-0000-0000-0000-000000000003', 0, 2, '2026-03-25 16:41:08.457371+00', '2026-03-25 17:44:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000085e', '91500000-0000-0000-0000-0000000000d7', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:34:08.457371+00', '2026-03-25 17:54:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000867', '91500000-0000-0000-0000-0000000000d8', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:40:08.457371+00', '2026-03-25 17:45:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000087b', '91500000-0000-0000-0000-0000000000da', NULL, '70000000-0000-0000-0000-000000000003', 0, 2, '2026-03-25 16:38:08.457371+00', '2026-03-25 17:47:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000087c', '91500000-0000-0000-0000-0000000000da', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:31:08.457371+00', '2026-03-25 17:57:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-000000000885', '91500000-0000-0000-0000-0000000000db', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:37:08.457371+00', '2026-03-25 17:48:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-00000000088f', '91500000-0000-0000-0000-0000000000dc', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:36:08.457371+00', '2026-03-25 17:49:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000008a3', '91500000-0000-0000-0000-0000000000de', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:34:08.457371+00', '2026-03-25 17:51:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000008ad', '91500000-0000-0000-0000-0000000000df', NULL, '70000000-0000-0000-0000-000000000002', 0, 3, '2026-03-25 16:33:08.457371+00', '2026-03-25 17:52:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000008b7', '91500000-0000-0000-0000-0000000000e0', NULL, '70000000-0000-0000-0000-000000000003', 0, 2, '2026-03-25 16:32:08.457371+00', '2026-03-25 17:53:08.457371+00', NULL, NULL);
INSERT INTO public.inventory_reservation VALUES ('94000000-0000-0000-0000-0000000008b8', '91500000-0000-0000-0000-0000000000e0', NULL, '70000000-0000-0000-0000-000000000001', 0, 2, '2026-03-25 16:25:08.457371+00', '2026-03-25 18:03:08.457371+00', NULL, NULL);


--
-- TOC entry 4566 (class 0 OID 33001)
-- Dependencies: 391
-- Data for Name: listing; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.listing VALUES ('14845ea2-0f3c-4838-8501-0091d26cdf3a', 1, 3, 'Bán laptop sieu cap vip pro', 'DELL G5515', 'Laptop siêu bền siêu mượt siêu khỏe', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'New seal', NULL, '2026-06-04 16:05:55.207996+00', NULL, NULL, 3, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 16:05:50.269304+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2026-03-22 09:30:32.810906+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', true, 0, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('81680948-d9af-4a81-a649-4ce9e3a63071', 1, 4, 'Bán laptop sieu cap vip pro', 'DELL G5515', 'Laptop siêu bền siêu mượt siêu khỏe', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'New seal', NULL, NULL, '2026-03-21 16:09:39.494602+00', '2026-03-24 16:09:39.494602+00', 3, 1, 100, 150, 300, 1, NULL, NULL, NULL, NULL, NULL, '2026-03-21 16:01:33.379865+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2026-03-24 19:26:56.396762+00', 'System', false, 0, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('1a7ed4c4-ff65-48cb-b4ea-c8d39d33db89', 2, 3, 'Bán tranh sieu cap vipproo', 't16sa0', 'Tranh t1 5 skin', '20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001', 'New seal', NULL, NULL, '2026-03-23 15:17:07.981811+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 500, false, NULL, NULL, '2026-03-21 16:06:55.729932+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2026-03-23 15:17:08.052189+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('8beb97de-aa33-4beb-832b-b1e9feaeb42f', 2, 4, 'Bán tranh T1111111111111111111111', 't16sa0', 'Tranh t1 5 skin', '20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001', 'New seal', NULL, '2026-06-04 16:05:35.48055+00', '2026-03-21 16:06:29.592871+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 500, false, NULL, NULL, '2026-03-21 16:05:25.926833+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2026-03-21 16:06:45.018917+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000021', 2, 3, 'Cecilia''s Item #33', 'DEMO-3-0033', 'Curated demo listing #33 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000022', 2, 3, 'Cecilia''s Item #34', 'DEMO-3-0034', 'Curated demo listing #34 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000023', 2, 3, 'Cecilia''s Item #35', 'DEMO-3-0035', 'Curated demo listing #35 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('67f00868-4f9d-4486-a63e-20d6e0a161b7', 2, 3, 'Bán tranh T1', 't16sa0', 'Tranh t1 5 skin', '20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001', 'New seal', NULL, NULL, '2026-03-22 09:39:42.138113+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 500, true, 450, NULL, '2026-03-14 16:02:19.980505+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2026-03-22 09:39:42.183226+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000024', 2, 3, 'Cecilia''s Item #36', 'DEMO-3-0036', 'Curated demo listing #36 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000025', 2, 3, 'Cecilia''s Item #37', 'DEMO-3-0037', 'Curated demo listing #37 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000026', 2, 3, 'Cecilia''s Item #38', 'DEMO-3-0038', 'Curated demo listing #38 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('bc616caf-bfe1-44d5-b173-9fd2fe28e97b', 1, 3, 'iPhone 15 Pro Max 256GB Black', 'SKU-IPHONE-TEST-001', 'Test listing for sale event', '10000000-0000-0000-0000-000000000001', NULL, 'New', NULL, NULL, NULL, NULL, 30, 1, NULL, NULL, NULL, 5, 1, 999.99, NULL, NULL, NULL, '2026-03-13 15:04:46.291669+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('d6409c09-5820-4e4a-949c-0d03c98e7d92', 1, 3, 'MacBook Air M3 Midnight 16GB', 'SKU-MACBOOK-TEST-001', 'Test listing for sale event', '10000000-0000-0000-0000-000000000001', NULL, 'New', NULL, NULL, NULL, NULL, 30, 1, NULL, NULL, NULL, 2, 1, 1299.99, NULL, NULL, NULL, '2026-03-13 15:04:46.291669+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('53c73084-1223-4a54-85f9-9cb1cf13828c', 1, 3, 'AirPods Pro 2nd Generation', 'SKU-AIRPODS-TEST-001', 'Test listing for sale event', '10000000-0000-0000-0000-000000000001', NULL, 'New', NULL, NULL, NULL, NULL, 30, 1, NULL, NULL, NULL, 10, 1, 249.99, NULL, NULL, NULL, '2026-03-13 15:04:46.291669+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('b9cddcbe-3170-4592-a266-d1d609f57384', 2, 3, 'Mèo con cute', '100', '64566', '20000000-0000-0000-0000-000000000004', NULL, '1000', NULL, NULL, '2026-03-25 16:15:28.512991+00', NULL, 0, 2, NULL, NULL, NULL, 11010, 1, 1000, false, NULL, NULL, '2026-03-25 16:15:29.833595+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('6d52344e-32cc-4444-a35d-25340a46f590', 1, 3, 'Iphone', 'IOS15', 'Iphone 15 PROMAX', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'New seal', NULL, NULL, '2026-03-25 16:15:30.496197+00', '2026-03-28 16:15:30.496197+00', 3, 1, 100, 300, 500, 1, NULL, NULL, NULL, NULL, NULL, '2026-03-25 16:15:31.914773+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false, 0, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000004', 2, 3, 'StreetFlex Running Shoes Lite', 'SHO-1-0004', 'StreetFlex Running Shoes Lite curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000002', 2, 3, 'Vertex Ultrabook RTX Ready', 'LTP-1-0002', 'Vertex Ultrabook RTX Ready curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000005', 2, 3, 'SteamWorks Air Fryer Compact', 'HOM-1-0005', 'SteamWorks Air Fryer Compact curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000006', 2, 3, 'Astra Pocket Camera Phone Pro Edition', 'MBL-1-0006', 'Astra Pocket Camera Phone Pro Edition curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000007', 2, 3, 'Vertex Laptop 13-inch', 'LTP-1-0007', 'Vertex Laptop 13-inch curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000008', 2, 3, 'FramePro Action Camera 4K Edition', 'CAM-1-0008', 'FramePro Action Camera 4K Edition curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000009', 2, 3, 'StreetFlex Athletic Sneakers Men''s', 'SHO-1-0009', 'StreetFlex Athletic Sneakers Men''s curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000a', 2, 3, 'SteamWorks Espresso Machine Premium', 'HOM-1-0010', 'SteamWorks Espresso Machine Premium curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000b', 2, 3, 'Astra 5G Phone 128GB', 'MBL-1-0011', 'Astra 5G Phone 128GB curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000d', 2, 3, 'FramePro Mirrorless Camera Starter Kit', 'CAM-1-0013', 'FramePro Mirrorless Camera Starter Kit curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000e', 2, 3, 'StreetFlex Training Shoes Lite', 'SHO-1-0014', 'StreetFlex Training Shoes Lite curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000f', 2, 3, 'SteamWorks Blender Compact', 'HOM-1-0015', 'SteamWorks Blender Compact curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000010', 2, 3, 'Astra Smartphone Pro Edition', 'MBL-1-0016', 'Astra Smartphone Pro Edition curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000011', 2, 3, 'Vertex Ultrabook 13-inch', 'LTP-1-0017', 'Vertex Ultrabook 13-inch curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000012', 2, 3, 'FramePro Vlog Camera 4K Edition', 'CAM-1-0018', 'FramePro Vlog Camera 4K Edition curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000013', 2, 3, 'StreetFlex Running Shoes Men''s', 'SHO-1-0019', 'StreetFlex Running Shoes Men''s curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000014', 2, 3, 'SteamWorks Rice Cooker Premium', 'HOM-1-0020', 'SteamWorks Rice Cooker Premium curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000015', 2, 3, 'Astra Pocket Camera Phone 128GB', 'MBL-1-0021', 'Astra Pocket Camera Phone 128GB curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000016', 2, 3, 'Vertex Laptop RTX Ready', 'LTP-1-0022', 'Vertex Laptop RTX Ready curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000017', 2, 3, 'FramePro Action Camera Starter Kit', 'CAM-1-0023', 'FramePro Action Camera Starter Kit curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000018', 2, 3, 'StreetFlex Athletic Sneakers Lite', 'SHO-1-0024', 'StreetFlex Athletic Sneakers Lite curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000001', 2, 3, 'Vertex Creator Laptop 15-inch', 'LTP-2-0001', 'Vertex Creator Laptop 15-inch curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000002', 2, 3, 'FramePro Mirrorless Camera Travel Pack', 'CAM-2-0002', 'FramePro Mirrorless Camera Travel Pack curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000003', 2, 3, 'StreetFlex Training Shoes Women''s', 'SHO-2-0003', 'StreetFlex Training Shoes Women''s curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000004', 2, 3, 'SteamWorks Espresso Machine Stainless', 'HOM-2-0004', 'SteamWorks Espresso Machine Stainless curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000005', 2, 3, 'Astra Smartphone 256GB', 'MBL-2-0005', 'Astra Smartphone 256GB curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000006', 2, 3, 'Vertex Ultrabook Workstation', 'LTP-2-0006', 'Vertex Ultrabook Workstation curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000007', 2, 3, 'FramePro Vlog Camera Dual Lens', 'CAM-2-0007', 'FramePro Vlog Camera Dual Lens curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000008', 2, 3, 'StreetFlex Running Shoes All-Terrain', 'SHO-2-0008', 'StreetFlex Running Shoes All-Terrain curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000009', 2, 3, 'SteamWorks Blender Family Size', 'HOM-2-0009', 'SteamWorks Blender Family Size curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000a', 2, 3, 'Astra Pocket Camera Phone Travel Bundle', 'MBL-2-0010', 'Astra Pocket Camera Phone Travel Bundle curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000b', 2, 3, 'Vertex Laptop 15-inch', 'LTP-2-0011', 'Vertex Laptop 15-inch curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000c', 2, 3, 'FramePro Action Camera Travel Pack', 'CAM-2-0012', 'FramePro Action Camera Travel Pack curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000d', 2, 3, 'StreetFlex Athletic Sneakers Women''s', 'SHO-2-0013', 'StreetFlex Athletic Sneakers Women''s curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000e', 2, 3, 'SteamWorks Rice Cooker Stainless', 'HOM-2-0014', 'SteamWorks Rice Cooker Stainless curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000000f', 2, 3, 'Astra 5G Phone 256GB', 'MBL-2-0015', 'Astra 5G Phone 256GB curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000010', 2, 3, 'Vertex Creator Laptop Workstation', 'LTP-2-0016', 'Vertex Creator Laptop Workstation curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000011', 2, 3, 'FramePro Mirrorless Camera Dual Lens', 'CAM-2-0017', 'FramePro Mirrorless Camera Dual Lens curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000012', 2, 3, 'StreetFlex Training Shoes All-Terrain', 'SHO-2-0018', 'StreetFlex Training Shoes All-Terrain curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000013', 2, 3, 'SteamWorks Air Fryer Family Size', 'HOM-2-0019', 'SteamWorks Air Fryer Family Size curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000014', 2, 3, 'Astra Smartphone Travel Bundle', 'MBL-2-0020', 'Astra Smartphone Travel Bundle curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000015', 2, 3, 'Vertex Ultrabook 15-inch', 'LTP-2-0021', 'Vertex Ultrabook 15-inch curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000016', 2, 3, 'FramePro Vlog Camera Travel Pack', 'CAM-2-0022', 'FramePro Vlog Camera Travel Pack curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000017', 2, 3, 'StreetFlex Running Shoes Women''s', 'SHO-2-0023', 'StreetFlex Running Shoes Women''s curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000018', 2, 3, 'SteamWorks Espresso Machine Stainless', 'HOM-2-0024', 'SteamWorks Espresso Machine Stainless curated by Brian for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000002', 2, 3, 'StreetFlex Athletic Sneakers Men''s', 'SHO-3-0002', 'StreetFlex Athletic Sneakers Men''s curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000003', 2, 3, 'SteamWorks Blender Premium', 'HOM-3-0003', 'SteamWorks Blender Premium curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000004', 2, 3, 'Astra 5G Phone 128GB', 'MBL-3-0004', 'Astra 5G Phone 128GB curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000005', 2, 3, 'Vertex Creator Laptop RTX Ready', 'LTP-3-0005', 'Vertex Creator Laptop RTX Ready curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000c', 2, 3, 'StreetFlex Running Shoes Men''s', 'SHO-3-0012', 'StreetFlex Running Shoes Men''s curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000d', 2, 3, 'SteamWorks Air Fryer Premium', 'HOM-3-0013', 'SteamWorks Air Fryer Premium curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000e', 2, 3, 'Astra Pocket Camera Phone 128GB', 'MBL-3-0014', 'Astra Pocket Camera Phone 128GB curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000f', 2, 3, 'Vertex Laptop RTX Ready', 'LTP-3-0015', 'Vertex Laptop RTX Ready curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000010', 2, 3, 'FramePro Action Camera Starter Kit', 'CAM-3-0016', 'FramePro Action Camera Starter Kit curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000011', 2, 3, 'StreetFlex Athletic Sneakers Lite', 'SHO-3-0017', 'StreetFlex Athletic Sneakers Lite curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000012', 2, 3, 'SteamWorks Espresso Machine Compact', 'HOM-3-0018', 'SteamWorks Espresso Machine Compact curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000013', 2, 3, 'Astra 5G Phone Pro Edition', 'MBL-3-0019', 'Astra 5G Phone Pro Edition curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000014', 2, 3, 'Vertex Creator Laptop 13-inch', 'LTP-3-0020', 'Vertex Creator Laptop 13-inch curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000009', 2, 3, 'Astra Smartphone Pro Edition', 'MBL-3-0009', 'Astra Smartphone Pro Edition curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000a', 2, 3, 'Vertex Ultrabook 13-inch', 'LTP-3-0010', 'Vertex Ultrabook 13-inch curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000000b', 2, 3, 'FramePro Vlog Camera 4K Edition', 'CAM-3-0011', 'FramePro Vlog Camera 4K Edition curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000015', 2, 3, 'FramePro Mirrorless Camera 4K Edition', 'CAM-3-0021', 'FramePro Mirrorless Camera 4K Edition curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000016', 2, 3, 'StreetFlex Training Shoes Men''s', 'SHO-3-0022', 'StreetFlex Training Shoes Men''s curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000017', 2, 3, 'SteamWorks Blender Premium', 'HOM-3-0023', 'SteamWorks Blender Premium curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000018', 2, 3, 'Astra Smartphone 128GB', 'MBL-3-0024', 'Astra Smartphone 128GB curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('86093ccd-8258-420c-8081-3a21501e7575', 1, 4, 'Mock Product for hoapoki24', 'MOCK-SKU-123', 'Dummy desc', '10000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', 'New', NULL, NULL, NULL, NULL, 30, 1, 500, NULL, NULL, 10, 1, 500, NULL, NULL, NULL, '2026-03-21 10:48:41.827+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2026-03-25 17:39:03.380755+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false, 0, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000027', 2, 3, 'Cecilia''s Item #39', 'DEMO-3-0039', 'Curated demo listing #39 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000028', 2, 3, 'Cecilia''s Item #40', 'DEMO-3-0040', 'Curated demo listing #40 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000029', 2, 3, 'Cecilia''s Item #41', 'DEMO-3-0041', 'Curated demo listing #41 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002a', 2, 3, 'Cecilia''s Item #42', 'DEMO-3-0042', 'Curated demo listing #42 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002b', 2, 3, 'Cecilia''s Item #43', 'DEMO-3-0043', 'Curated demo listing #43 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002c', 2, 3, 'Cecilia''s Item #44', 'DEMO-3-0044', 'Curated demo listing #44 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002d', 2, 3, 'Cecilia''s Item #45', 'DEMO-3-0045', 'Curated demo listing #45 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002e', 2, 3, 'Cecilia''s Item #46', 'DEMO-3-0046', 'Curated demo listing #46 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000002f', 2, 3, 'Cecilia''s Item #47', 'DEMO-3-0047', 'Curated demo listing #47 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000030', 2, 3, 'Cecilia''s Item #48', 'DEMO-3-0048', 'Curated demo listing #48 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000031', 2, 3, 'Cecilia''s Item #49', 'DEMO-3-0049', 'Curated demo listing #49 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000032', 2, 3, 'Cecilia''s Item #50', 'DEMO-3-0050', 'Curated demo listing #50 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000033', 2, 3, 'Cecilia''s Item #51', 'DEMO-3-0051', 'Curated demo listing #51 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000034', 2, 3, 'Cecilia''s Item #52', 'DEMO-3-0052', 'Curated demo listing #52 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000035', 2, 3, 'Cecilia''s Item #53', 'DEMO-3-0053', 'Curated demo listing #53 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000036', 2, 3, 'Cecilia''s Item #54', 'DEMO-3-0054', 'Curated demo listing #54 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000037', 2, 3, 'Cecilia''s Item #55', 'DEMO-3-0055', 'Curated demo listing #55 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000038', 2, 3, 'Cecilia''s Item #56', 'DEMO-3-0056', 'Curated demo listing #56 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000039', 2, 3, 'Cecilia''s Item #57', 'DEMO-3-0057', 'Curated demo listing #57 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003a', 2, 3, 'Cecilia''s Item #58', 'DEMO-3-0058', 'Curated demo listing #58 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003b', 2, 3, 'Cecilia''s Item #59', 'DEMO-3-0059', 'Curated demo listing #59 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003c', 2, 3, 'Cecilia''s Item #60', 'DEMO-3-0060', 'Curated demo listing #60 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003d', 2, 3, 'Cecilia''s Item #61', 'DEMO-3-0061', 'Curated demo listing #61 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003e', 2, 3, 'Cecilia''s Item #62', 'DEMO-3-0062', 'Curated demo listing #62 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000003f', 2, 3, 'Cecilia''s Item #63', 'DEMO-3-0063', 'Curated demo listing #63 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000040', 2, 3, 'Cecilia''s Item #64', 'DEMO-3-0064', 'Curated demo listing #64 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000041', 2, 3, 'Cecilia''s Item #65', 'DEMO-3-0065', 'Curated demo listing #65 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000042', 2, 3, 'Cecilia''s Item #66', 'DEMO-3-0066', 'Curated demo listing #66 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000043', 2, 3, 'Cecilia''s Item #67', 'DEMO-3-0067', 'Curated demo listing #67 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000044', 2, 3, 'Cecilia''s Item #68', 'DEMO-3-0068', 'Curated demo listing #68 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000045', 2, 3, 'Cecilia''s Item #69', 'DEMO-3-0069', 'Curated demo listing #69 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000046', 2, 3, 'Cecilia''s Item #70', 'DEMO-3-0070', 'Curated demo listing #70 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000047', 2, 3, 'Cecilia''s Item #71', 'DEMO-3-0071', 'Curated demo listing #71 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000048', 2, 3, 'Cecilia''s Item #72', 'DEMO-3-0072', 'Curated demo listing #72 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000049', 2, 3, 'Cecilia''s Item #73', 'DEMO-3-0073', 'Curated demo listing #73 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004a', 2, 3, 'Cecilia''s Item #74', 'DEMO-3-0074', 'Curated demo listing #74 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004b', 2, 3, 'Cecilia''s Item #75', 'DEMO-3-0075', 'Curated demo listing #75 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004c', 2, 3, 'Cecilia''s Item #76', 'DEMO-3-0076', 'Curated demo listing #76 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004d', 2, 3, 'Cecilia''s Item #77', 'DEMO-3-0077', 'Curated demo listing #77 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004e', 2, 3, 'Cecilia''s Item #78', 'DEMO-3-0078', 'Curated demo listing #78 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000004f', 2, 3, 'Cecilia''s Item #79', 'DEMO-3-0079', 'Curated demo listing #79 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000019', 2, 3, 'Alice''s Item #25', 'DEMO-1-0025', 'Curated demo listing #25 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001a', 2, 3, 'Alice''s Item #26', 'DEMO-1-0026', 'Curated demo listing #26 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001b', 2, 3, 'Alice''s Item #27', 'DEMO-1-0027', 'Curated demo listing #27 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001c', 2, 3, 'Alice''s Item #28', 'DEMO-1-0028', 'Curated demo listing #28 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001d', 2, 3, 'Alice''s Item #29', 'DEMO-1-0029', 'Curated demo listing #29 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001e', 2, 3, 'Alice''s Item #30', 'DEMO-1-0030', 'Curated demo listing #30 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000001f', 2, 3, 'Alice''s Item #31', 'DEMO-1-0031', 'Curated demo listing #31 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000020', 2, 3, 'Alice''s Item #32', 'DEMO-1-0032', 'Curated demo listing #32 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000021', 2, 3, 'Alice''s Item #33', 'DEMO-1-0033', 'Curated demo listing #33 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000022', 2, 3, 'Alice''s Item #34', 'DEMO-1-0034', 'Curated demo listing #34 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000023', 2, 3, 'Alice''s Item #35', 'DEMO-1-0035', 'Curated demo listing #35 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000024', 2, 3, 'Alice''s Item #36', 'DEMO-1-0036', 'Curated demo listing #36 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000025', 2, 3, 'Alice''s Item #37', 'DEMO-1-0037', 'Curated demo listing #37 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000000c', 2, 3, 'Vertex Creator Laptop RTX Ready', 'LTP-1-0012', 'Vertex Creator Laptop RTX Ready curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000026', 2, 3, 'Alice''s Item #38', 'DEMO-1-0038', 'Curated demo listing #38 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000027', 2, 3, 'Alice''s Item #39', 'DEMO-1-0039', 'Curated demo listing #39 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000028', 2, 3, 'Alice''s Item #40', 'DEMO-1-0040', 'Curated demo listing #40 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000029', 2, 3, 'Alice''s Item #41', 'DEMO-1-0041', 'Curated demo listing #41 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002a', 2, 3, 'Alice''s Item #42', 'DEMO-1-0042', 'Curated demo listing #42 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002b', 2, 3, 'Alice''s Item #43', 'DEMO-1-0043', 'Curated demo listing #43 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002c', 2, 3, 'Alice''s Item #44', 'DEMO-1-0044', 'Curated demo listing #44 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002d', 2, 3, 'Alice''s Item #45', 'DEMO-1-0045', 'Curated demo listing #45 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002e', 2, 3, 'Alice''s Item #46', 'DEMO-1-0046', 'Curated demo listing #46 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000002f', 2, 3, 'Alice''s Item #47', 'DEMO-1-0047', 'Curated demo listing #47 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000030', 2, 3, 'Alice''s Item #48', 'DEMO-1-0048', 'Curated demo listing #48 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000031', 2, 3, 'Alice''s Item #49', 'DEMO-1-0049', 'Curated demo listing #49 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000032', 2, 3, 'Alice''s Item #50', 'DEMO-1-0050', 'Curated demo listing #50 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000033', 2, 3, 'Alice''s Item #51', 'DEMO-1-0051', 'Curated demo listing #51 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000034', 2, 3, 'Alice''s Item #52', 'DEMO-1-0052', 'Curated demo listing #52 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000035', 2, 3, 'Alice''s Item #53', 'DEMO-1-0053', 'Curated demo listing #53 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000036', 2, 3, 'Alice''s Item #54', 'DEMO-1-0054', 'Curated demo listing #54 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000037', 2, 3, 'Alice''s Item #55', 'DEMO-1-0055', 'Curated demo listing #55 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000038', 2, 3, 'Alice''s Item #56', 'DEMO-1-0056', 'Curated demo listing #56 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000039', 2, 3, 'Alice''s Item #57', 'DEMO-1-0057', 'Curated demo listing #57 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003a', 2, 3, 'Alice''s Item #58', 'DEMO-1-0058', 'Curated demo listing #58 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003b', 2, 3, 'Alice''s Item #59', 'DEMO-1-0059', 'Curated demo listing #59 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003c', 2, 3, 'Alice''s Item #60', 'DEMO-1-0060', 'Curated demo listing #60 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003d', 2, 3, 'Alice''s Item #61', 'DEMO-1-0061', 'Curated demo listing #61 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003e', 2, 3, 'Alice''s Item #62', 'DEMO-1-0062', 'Curated demo listing #62 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000003f', 2, 3, 'Alice''s Item #63', 'DEMO-1-0063', 'Curated demo listing #63 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000040', 2, 3, 'Alice''s Item #64', 'DEMO-1-0064', 'Curated demo listing #64 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000041', 2, 3, 'Alice''s Item #65', 'DEMO-1-0065', 'Curated demo listing #65 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000042', 2, 3, 'Alice''s Item #66', 'DEMO-1-0066', 'Curated demo listing #66 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000043', 2, 3, 'Alice''s Item #67', 'DEMO-1-0067', 'Curated demo listing #67 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000044', 2, 3, 'Alice''s Item #68', 'DEMO-1-0068', 'Curated demo listing #68 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000045', 2, 3, 'Alice''s Item #69', 'DEMO-1-0069', 'Curated demo listing #69 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000046', 2, 3, 'Alice''s Item #70', 'DEMO-1-0070', 'Curated demo listing #70 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000047', 2, 3, 'Alice''s Item #71', 'DEMO-1-0071', 'Curated demo listing #71 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000048', 2, 3, 'Alice''s Item #72', 'DEMO-1-0072', 'Curated demo listing #72 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000049', 2, 3, 'Alice''s Item #73', 'DEMO-1-0073', 'Curated demo listing #73 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004a', 2, 3, 'Alice''s Item #74', 'DEMO-1-0074', 'Curated demo listing #74 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004b', 2, 3, 'Alice''s Item #75', 'DEMO-1-0075', 'Curated demo listing #75 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004c', 2, 3, 'Alice''s Item #76', 'DEMO-1-0076', 'Curated demo listing #76 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004d', 2, 3, 'Alice''s Item #77', 'DEMO-1-0077', 'Curated demo listing #77 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004e', 2, 3, 'Alice''s Item #78', 'DEMO-1-0078', 'Curated demo listing #78 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000004f', 2, 3, 'Alice''s Item #79', 'DEMO-1-0079', 'Curated demo listing #79 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000050', 2, 3, 'Alice''s Item #80', 'DEMO-1-0080', 'Curated demo listing #80 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000051', 2, 3, 'Alice''s Item #81', 'DEMO-1-0081', 'Curated demo listing #81 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000052', 2, 3, 'Alice''s Item #82', 'DEMO-1-0082', 'Curated demo listing #82 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000053', 2, 3, 'Alice''s Item #83', 'DEMO-1-0083', 'Curated demo listing #83 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000054', 2, 3, 'Alice''s Item #84', 'DEMO-1-0084', 'Curated demo listing #84 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000055', 2, 3, 'Alice''s Item #85', 'DEMO-1-0085', 'Curated demo listing #85 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000056', 2, 3, 'Alice''s Item #86', 'DEMO-1-0086', 'Curated demo listing #86 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 39.99, false, NULL, NULL, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000057', 2, 3, 'Alice''s Item #87', 'DEMO-1-0087', 'Curated demo listing #87 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 40.99, false, NULL, NULL, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000058', 2, 3, 'Alice''s Item #88', 'DEMO-1-0088', 'Curated demo listing #88 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 41.99, true, 37.79, 41.99, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000059', 2, 3, 'Alice''s Item #89', 'DEMO-1-0089', 'Curated demo listing #89 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 42.99, false, NULL, NULL, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005a', 2, 3, 'Alice''s Item #90', 'DEMO-1-0090', 'Curated demo listing #90 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 43.99, false, NULL, NULL, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005b', 2, 3, 'Alice''s Item #91', 'DEMO-1-0091', 'Curated demo listing #91 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005c', 2, 3, 'Alice''s Item #92', 'DEMO-1-0092', 'Curated demo listing #92 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 30.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005d', 2, 3, 'Alice''s Item #93', 'DEMO-1-0093', 'Curated demo listing #93 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005e', 2, 3, 'Alice''s Item #94', 'DEMO-1-0094', 'Curated demo listing #94 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 32.99, true, 29.69, 32.99, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-00000000005f', 2, 3, 'Alice''s Item #95', 'DEMO-1-0095', 'Curated demo listing #95 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 33.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000060', 2, 3, 'Alice''s Item #96', 'DEMO-1-0096', 'Curated demo listing #96 for Alice Johnson.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 34.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000061', 2, 3, 'Alice''s Item #97', 'DEMO-1-0097', 'Curated demo listing #97 for Alice Johnson.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 35.99, true, 32.39, 35.99, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000062', 2, 3, 'Alice''s Item #98', 'DEMO-1-0098', 'Curated demo listing #98 for Alice Johnson.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 36.99, false, NULL, NULL, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000063', 2, 3, 'Alice''s Item #99', 'DEMO-1-0099', 'Curated demo listing #99 for Alice Johnson.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 37.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000064', 2, 3, 'Alice''s Item #100', 'DEMO-1-0100', 'Curated demo listing #100 for Alice Johnson.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 38.99, true, 35.09, 38.99, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000019', 2, 3, 'Brian''s Item #25', 'DEMO-2-0025', 'Curated demo listing #25 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001a', 2, 3, 'Brian''s Item #26', 'DEMO-2-0026', 'Curated demo listing #26 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001b', 2, 3, 'Brian''s Item #27', 'DEMO-2-0027', 'Curated demo listing #27 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001c', 2, 3, 'Brian''s Item #28', 'DEMO-2-0028', 'Curated demo listing #28 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001d', 2, 3, 'Brian''s Item #29', 'DEMO-2-0029', 'Curated demo listing #29 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001e', 2, 3, 'Brian''s Item #30', 'DEMO-2-0030', 'Curated demo listing #30 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000001f', 2, 3, 'Brian''s Item #31', 'DEMO-2-0031', 'Curated demo listing #31 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000020', 2, 3, 'Brian''s Item #32', 'DEMO-2-0032', 'Curated demo listing #32 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000021', 2, 3, 'Brian''s Item #33', 'DEMO-2-0033', 'Curated demo listing #33 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000022', 2, 3, 'Brian''s Item #34', 'DEMO-2-0034', 'Curated demo listing #34 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000023', 2, 3, 'Brian''s Item #35', 'DEMO-2-0035', 'Curated demo listing #35 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000024', 2, 3, 'Brian''s Item #36', 'DEMO-2-0036', 'Curated demo listing #36 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000025', 2, 3, 'Brian''s Item #37', 'DEMO-2-0037', 'Curated demo listing #37 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000026', 2, 3, 'Brian''s Item #38', 'DEMO-2-0038', 'Curated demo listing #38 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000027', 2, 3, 'Brian''s Item #39', 'DEMO-2-0039', 'Curated demo listing #39 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000028', 2, 3, 'Brian''s Item #40', 'DEMO-2-0040', 'Curated demo listing #40 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000029', 2, 3, 'Brian''s Item #41', 'DEMO-2-0041', 'Curated demo listing #41 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002a', 2, 3, 'Brian''s Item #42', 'DEMO-2-0042', 'Curated demo listing #42 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002b', 2, 3, 'Brian''s Item #43', 'DEMO-2-0043', 'Curated demo listing #43 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002c', 2, 3, 'Brian''s Item #44', 'DEMO-2-0044', 'Curated demo listing #44 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002d', 2, 3, 'Brian''s Item #45', 'DEMO-2-0045', 'Curated demo listing #45 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002e', 2, 3, 'Brian''s Item #46', 'DEMO-2-0046', 'Curated demo listing #46 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000002f', 2, 3, 'Brian''s Item #47', 'DEMO-2-0047', 'Curated demo listing #47 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000030', 2, 3, 'Brian''s Item #48', 'DEMO-2-0048', 'Curated demo listing #48 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000031', 2, 3, 'Brian''s Item #49', 'DEMO-2-0049', 'Curated demo listing #49 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000032', 2, 3, 'Brian''s Item #50', 'DEMO-2-0050', 'Curated demo listing #50 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000033', 2, 3, 'Brian''s Item #51', 'DEMO-2-0051', 'Curated demo listing #51 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000034', 2, 3, 'Brian''s Item #52', 'DEMO-2-0052', 'Curated demo listing #52 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000035', 2, 3, 'Brian''s Item #53', 'DEMO-2-0053', 'Curated demo listing #53 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000036', 2, 3, 'Brian''s Item #54', 'DEMO-2-0054', 'Curated demo listing #54 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000037', 2, 3, 'Brian''s Item #55', 'DEMO-2-0055', 'Curated demo listing #55 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000038', 2, 3, 'Brian''s Item #56', 'DEMO-2-0056', 'Curated demo listing #56 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000039', 2, 3, 'Brian''s Item #57', 'DEMO-2-0057', 'Curated demo listing #57 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003a', 2, 3, 'Brian''s Item #58', 'DEMO-2-0058', 'Curated demo listing #58 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003b', 2, 3, 'Brian''s Item #59', 'DEMO-2-0059', 'Curated demo listing #59 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003c', 2, 3, 'Brian''s Item #60', 'DEMO-2-0060', 'Curated demo listing #60 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003d', 2, 3, 'Brian''s Item #61', 'DEMO-2-0061', 'Curated demo listing #61 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003e', 2, 3, 'Brian''s Item #62', 'DEMO-2-0062', 'Curated demo listing #62 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000003f', 2, 3, 'Brian''s Item #63', 'DEMO-2-0063', 'Curated demo listing #63 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000040', 2, 3, 'Brian''s Item #64', 'DEMO-2-0064', 'Curated demo listing #64 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000041', 2, 3, 'Brian''s Item #65', 'DEMO-2-0065', 'Curated demo listing #65 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000042', 2, 3, 'Brian''s Item #66', 'DEMO-2-0066', 'Curated demo listing #66 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-02 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-02 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000043', 2, 3, 'Brian''s Item #67', 'DEMO-2-0067', 'Curated demo listing #67 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000044', 2, 3, 'Brian''s Item #68', 'DEMO-2-0068', 'Curated demo listing #68 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-11-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000045', 2, 3, 'Brian''s Item #69', 'DEMO-2-0069', 'Curated demo listing #69 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-11-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000046', 2, 3, 'Brian''s Item #70', 'DEMO-2-0070', 'Curated demo listing #70 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-11-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000047', 2, 3, 'Brian''s Item #71', 'DEMO-2-0071', 'Curated demo listing #71 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-11-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000048', 2, 3, 'Brian''s Item #72', 'DEMO-2-0072', 'Curated demo listing #72 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-11-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000049', 2, 3, 'Brian''s Item #73', 'DEMO-2-0073', 'Curated demo listing #73 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-11-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004a', 2, 3, 'Brian''s Item #74', 'DEMO-2-0074', 'Curated demo listing #74 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-11-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004b', 2, 3, 'Brian''s Item #75', 'DEMO-2-0075', 'Curated demo listing #75 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-11-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004c', 2, 3, 'Brian''s Item #76', 'DEMO-2-0076', 'Curated demo listing #76 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-11-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-11-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004d', 2, 3, 'Brian''s Item #77', 'DEMO-2-0077', 'Curated demo listing #77 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-11-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-11-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004e', 2, 3, 'Brian''s Item #78', 'DEMO-2-0078', 'Curated demo listing #78 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-11-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000004f', 2, 3, 'Brian''s Item #79', 'DEMO-2-0079', 'Curated demo listing #79 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-11-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-11-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000050', 2, 3, 'Brian''s Item #80', 'DEMO-2-0080', 'Curated demo listing #80 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000051', 2, 3, 'Brian''s Item #81', 'DEMO-2-0081', 'Curated demo listing #81 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000052', 2, 3, 'Brian''s Item #82', 'DEMO-2-0082', 'Curated demo listing #82 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000053', 2, 3, 'Brian''s Item #83', 'DEMO-2-0083', 'Curated demo listing #83 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000054', 2, 3, 'Brian''s Item #84', 'DEMO-2-0084', 'Curated demo listing #84 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000055', 2, 3, 'Brian''s Item #85', 'DEMO-2-0085', 'Curated demo listing #85 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000056', 2, 3, 'Brian''s Item #86', 'DEMO-2-0086', 'Curated demo listing #86 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 47.99, false, NULL, NULL, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000057', 2, 3, 'Brian''s Item #87', 'DEMO-2-0087', 'Curated demo listing #87 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 48.99, false, NULL, NULL, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000058', 2, 3, 'Brian''s Item #88', 'DEMO-2-0088', 'Curated demo listing #88 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-25 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 49.99, true, 44.99, 49.99, '2023-12-25 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000059', 2, 3, 'Brian''s Item #89', 'DEMO-2-0089', 'Curated demo listing #89 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-24 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 50.99, false, NULL, NULL, '2023-12-24 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005a', 2, 3, 'Brian''s Item #90', 'DEMO-2-0090', 'Curated demo listing #90 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 51.99, false, NULL, NULL, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005b', 2, 3, 'Brian''s Item #91', 'DEMO-2-0091', 'Curated demo listing #91 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 37.99, true, 34.19, 37.99, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005c', 2, 3, 'Brian''s Item #92', 'DEMO-2-0092', 'Curated demo listing #92 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 38.99, false, NULL, NULL, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000001', 2, 3, 'Astra Smartphone 128GB', 'MBL-1-0001', 'Astra Smartphone 128GB curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 29.99, true, 26.99, 29.99, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005d', 2, 3, 'Brian''s Item #93', 'DEMO-2-0093', 'Curated demo listing #93 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 39.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005e', 2, 3, 'Brian''s Item #94', 'DEMO-2-0094', 'Curated demo listing #94 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 40.99, true, 36.89, 40.99, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-00000000005f', 2, 3, 'Brian''s Item #95', 'DEMO-2-0095', 'Curated demo listing #95 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 41.99, false, NULL, NULL, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000060', 2, 3, 'Brian''s Item #96', 'DEMO-2-0096', 'Curated demo listing #96 for Brian Carter.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 42.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000019', 2, 3, 'Cecilia''s Item #25', 'DEMO-3-0025', 'Curated demo listing #25 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-11-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-11-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001a', 2, 3, 'Cecilia''s Item #26', 'DEMO-3-0026', 'Curated demo listing #26 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2024-01-01 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001b', 2, 3, 'Cecilia''s Item #27', 'DEMO-3-0027', 'Curated demo listing #27 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-31 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-31 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('f1210d09-a873-47b2-90cf-c9541b6f4a0e', 1, 3, 'Mock Product for hoapoki24', 'MOCK-SKU-123', 'Dummy desc', '10000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', 'New', NULL, NULL, NULL, NULL, 30, 1, 500, NULL, NULL, 10, 1, 500, NULL, NULL, NULL, '2026-03-21 10:49:50.33+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001c', 2, 3, 'Cecilia''s Item #28', 'DEMO-3-0028', 'Curated demo listing #28 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001d', 2, 3, 'Cecilia''s Item #29', 'DEMO-3-0029', 'Curated demo listing #29 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-29 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-12-29 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001e', 2, 3, 'Cecilia''s Item #30', 'DEMO-3-0030', 'Curated demo listing #30 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-28 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-12-28 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000061', 2, 3, 'Brian''s Item #97', 'DEMO-2-0097', 'Curated demo listing #97 for Brian Carter.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 43.99, true, 39.59, 43.99, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000062', 2, 3, 'Brian''s Item #98', 'DEMO-2-0098', 'Curated demo listing #98 for Brian Carter.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 44.99, false, NULL, NULL, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000063', 2, 3, 'Brian''s Item #99', 'DEMO-2-0099', 'Curated demo listing #99 for Brian Carter.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 45.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('72000000-0000-0000-0000-000000000064', 2, 3, 'Brian''s Item #100', 'DEMO-2-0100', 'Curated demo listing #100 for Brian Carter.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 46.99, true, 42.29, 46.99, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000053', 2, 3, 'Cecilia''s Item #83', 'DEMO-3-0083', 'Curated demo listing #83 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-20 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-20 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000001f', 2, 3, 'Cecilia''s Item #31', 'DEMO-3-0031', 'Curated demo listing #31 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-27 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-27 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('71000000-0000-0000-0000-000000000003', 2, 3, 'FramePro Vlog Camera Starter Kit', 'CAM-1-0003', 'FramePro Vlog Camera Starter Kit curated by Alice for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-30 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 31.99, false, NULL, NULL, '2023-12-30 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000001', 2, 3, 'FramePro Action Camera 4K Edition', 'CAM-3-0001', 'FramePro Action Camera 4K Edition curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('87233f8b-3f4c-427b-a300-83720258ed62', 1, 3, 'Mock Product for hoapoki24', 'MOCK-SKU-123', 'Dummy desc', '10000000-0000-0000-0000-000000000001', '40000000-0000-0000-0000-000000000001', 'New', NULL, NULL, NULL, NULL, 30, 1, 500, NULL, NULL, 10, 1, 505, NULL, NULL, NULL, '2026-03-21 10:50:29.42+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('f871bd3f-1abe-4f84-a4c5-2138a1be76b9', 2, 3, 'Báo mới', 'baomoi', 'bài báo mới', 'e0000000-0000-0000-0000-000000000004', NULL, 'new', NULL, NULL, '2026-03-25 17:10:34.532501+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 10, true, 8, NULL, '2026-03-14 16:53:03.570133+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2026-03-25 17:10:34.633134+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000006', 2, 3, 'FramePro Mirrorless Camera Starter Kit', 'CAM-3-0006', 'FramePro Mirrorless Camera Starter Kit curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000007', 2, 3, 'StreetFlex Training Shoes Lite', 'SHO-3-0007', 'StreetFlex Training Shoes Lite curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000008', 2, 3, 'SteamWorks Rice Cooker Compact', 'HOM-3-0008', 'SteamWorks Rice Cooker Compact curated by Cecilia for the demo storefront. Includes seeded stock and alert scenarios for testing.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', '2026-03-25 17:10:08.457371+00', 'seed', false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000020', 2, 3, 'Cecilia''s Item #32', 'DEMO-3-0032', 'Curated demo listing #32 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-26 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-26 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000054', 2, 3, 'Cecilia''s Item #84', 'DEMO-3-0084', 'Curated demo listing #84 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-19 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-19 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000055', 2, 3, 'Cecilia''s Item #85', 'DEMO-3-0085', 'Curated demo listing #85 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-18 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-18 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000056', 2, 3, 'Cecilia''s Item #86', 'DEMO-3-0086', 'Curated demo listing #86 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-17 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 55.99, false, NULL, NULL, '2023-12-17 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000057', 2, 3, 'Cecilia''s Item #87', 'DEMO-3-0087', 'Curated demo listing #87 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-16 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 56.99, false, NULL, NULL, '2023-12-16 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000058', 2, 3, 'Cecilia''s Item #88', 'DEMO-3-0088', 'Curated demo listing #88 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-15 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 57.99, true, 52.19, 57.99, '2023-12-15 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000059', 2, 3, 'Cecilia''s Item #89', 'DEMO-3-0089', 'Curated demo listing #89 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-14 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 58.99, false, NULL, NULL, '2023-12-14 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005a', 2, 3, 'Cecilia''s Item #90', 'DEMO-3-0090', 'Curated demo listing #90 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-13 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 59.99, false, NULL, NULL, '2023-12-13 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005c', 2, 3, 'Cecilia''s Item #92', 'DEMO-3-0092', 'Curated demo listing #92 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-11 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 46.99, false, NULL, NULL, '2023-12-11 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005b', 2, 3, 'Cecilia''s Item #91', 'DEMO-3-0091', 'Curated demo listing #91 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-12 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 45.99, true, 41.39, 45.99, '2023-12-12 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005d', 2, 3, 'Cecilia''s Item #93', 'DEMO-3-0093', 'Curated demo listing #93 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-10 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 47.99, false, NULL, NULL, '2023-12-10 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005e', 2, 3, 'Cecilia''s Item #94', 'DEMO-3-0094', 'Curated demo listing #94 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-09 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 48.99, true, 44.09, 48.99, '2023-12-09 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-00000000005f', 2, 3, 'Cecilia''s Item #95', 'DEMO-3-0095', 'Curated demo listing #95 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-08 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-08 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000060', 2, 3, 'Cecilia''s Item #96', 'DEMO-3-0096', 'Curated demo listing #96 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-07 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-07 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000061', 2, 3, 'Cecilia''s Item #97', 'DEMO-3-0097', 'Curated demo listing #97 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-06 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-06 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000062', 2, 3, 'Cecilia''s Item #98', 'DEMO-3-0098', 'Curated demo listing #98 for Cecilia Gomez.', '30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009', 'Gently used and fully functional.', NULL, NULL, '2023-12-05 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 3, 1, 52.99, false, NULL, NULL, '2023-12-05 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000063', 2, 3, 'Cecilia''s Item #99', 'DEMO-3-0099', 'Curated demo listing #99 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004', 'Used item in working condition.', NULL, NULL, '2023-12-04 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 4, 1, 53.99, false, NULL, NULL, '2023-12-04 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000064', 2, 3, 'Cecilia''s Item #100', 'DEMO-3-0100', 'Curated demo listing #100 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-03 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 54.99, true, 49.49, 54.99, '2023-12-03 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000050', 2, 3, 'Cecilia''s Item #80', 'DEMO-3-0080', 'Curated demo listing #80 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001', 'Brand new condition with original packaging.', NULL, NULL, '2023-12-23 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 5, 1, 49.99, false, NULL, NULL, '2023-12-23 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000051', 2, 3, 'Cecilia''s Item #81', 'DEMO-3-0081', 'Curated demo listing #81 for Cecilia Gomez.', '10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007', 'New with retail tags attached.', NULL, NULL, '2023-12-22 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 1, 1, 50.99, false, NULL, NULL, '2023-12-22 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);
INSERT INTO public.listing VALUES ('73000000-0000-0000-0000-000000000052', 2, 3, 'Cecilia''s Item #82', 'DEMO-3-0082', 'Curated demo listing #82 for Cecilia Gomez.', '20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006', 'Open box item inspected for quality.', NULL, NULL, '2023-12-21 00:00:00+00', NULL, 0, 2, NULL, NULL, NULL, 2, 1, 51.99, true, 46.79, 51.99, '2023-12-21 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false, NULL, NULL, NULL, 0, NULL);


--
-- TOC entry 4584 (class 0 OID 33152)
-- Dependencies: 409
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
-- TOC entry 4577 (class 0 OID 33085)
-- Dependencies: 402
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
INSERT INTO public.listing_image VALUES ('8beb97de-aa33-4beb-832b-b1e9feaeb42f', 9, 'https://res.cloudinary.com/djmftornv/image/upload/v1773507408/ebay-clone/Full team_17d21900-8d74-42f1-9c09-86f88f814433.jpg', true);
INSERT INTO public.listing_image VALUES ('81680948-d9af-4a81-a649-4ce9e3a63071', 11, 'https://res.cloudinary.com/djmftornv/image/upload/v1774108440/ebay-clone/laptop-1_9aea42c8-98b2-4664-b0d8-5b88e664755f.jpg', true);
INSERT INTO public.listing_image VALUES ('67f00868-4f9d-4486-a63e-20d6e0a161b7', 12, 'https://res.cloudinary.com/djmftornv/image/upload/v1773507408/ebay-clone/Full team_17d21900-8d74-42f1-9c09-86f88f814433.jpg', true);
INSERT INTO public.listing_image VALUES ('1a7ed4c4-ff65-48cb-b4ea-c8d39d33db89', 13, 'https://res.cloudinary.com/djmftornv/image/upload/v1773507408/ebay-clone/Full team_17d21900-8d74-42f1-9c09-86f88f814433.jpg', true);
INSERT INTO public.listing_image VALUES ('b9cddcbe-3170-4592-a266-d1d609f57384', 14, 'https://res.cloudinary.com/djmftornv/image/upload/v1774455279/ebay-clone/meo` ngo%27_eea41590-df05-445c-84c7-52b238551a72.jpg', true);
INSERT INTO public.listing_image VALUES ('6d52344e-32cc-4444-a35d-25340a46f590', 15, 'https://res.cloudinary.com/djmftornv/image/upload/v1774455236/ebay-clone/IOS_2272b42a-f63a-4bd6-a8a7-35df9e3addb9.jpg', true);
INSERT INTO public.listing_image VALUES ('f871bd3f-1abe-4f84-a4c5-2138a1be76b9', 16, 'https://res.cloudinary.com/djmftornv/image/upload/v1773507202/ebay-clone/baomoi_f0bcbd35-ba53-4e66-a581-ce98750e8c61.png', true);


--
-- TOC entry 4579 (class 0 OID 33098)
-- Dependencies: 404
-- Data for Name: listing_item_specific; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.listing_item_specific VALUES ('8beb97de-aa33-4beb-832b-b1e9feaeb42f', 37, 'Brand', 'Casio');
INSERT INTO public.listing_item_specific VALUES ('8beb97de-aa33-4beb-832b-b1e9feaeb42f', 38, 'Movement', 'Automatic');
INSERT INTO public.listing_item_specific VALUES ('81680948-d9af-4a81-a649-4ce9e3a63071', 41, 'Brand', 'Dell');
INSERT INTO public.listing_item_specific VALUES ('81680948-d9af-4a81-a649-4ce9e3a63071', 42, 'Processor', 'Intel Core i5');
INSERT INTO public.listing_item_specific VALUES ('81680948-d9af-4a81-a649-4ce9e3a63071', 43, 'RAM Size', '16 GB');
INSERT INTO public.listing_item_specific VALUES ('81680948-d9af-4a81-a649-4ce9e3a63071', 44, 'Storage Type', 'SSD');
INSERT INTO public.listing_item_specific VALUES ('81680948-d9af-4a81-a649-4ce9e3a63071', 45, 'Screen Size', '15.6 in');
INSERT INTO public.listing_item_specific VALUES ('67f00868-4f9d-4486-a63e-20d6e0a161b7', 46, 'Brand', 'Casio');
INSERT INTO public.listing_item_specific VALUES ('67f00868-4f9d-4486-a63e-20d6e0a161b7', 47, 'Movement', 'Automatic');
INSERT INTO public.listing_item_specific VALUES ('1a7ed4c4-ff65-48cb-b4ea-c8d39d33db89', 48, 'Brand', 'Casio');
INSERT INTO public.listing_item_specific VALUES ('1a7ed4c4-ff65-48cb-b4ea-c8d39d33db89', 49, 'Movement', 'Automatic');
INSERT INTO public.listing_item_specific VALUES ('b9cddcbe-3170-4592-a266-d1d609f57384', 50, 'Brand', 'Nike');
INSERT INTO public.listing_item_specific VALUES ('b9cddcbe-3170-4592-a266-d1d609f57384', 51, 'Size', 'L');
INSERT INTO public.listing_item_specific VALUES ('b9cddcbe-3170-4592-a266-d1d609f57384', 52, 'Giang', '100');
INSERT INTO public.listing_item_specific VALUES ('6d52344e-32cc-4444-a35d-25340a46f590', 53, 'Brand', 'Apple');
INSERT INTO public.listing_item_specific VALUES ('6d52344e-32cc-4444-a35d-25340a46f590', 54, 'Model', 'iPhone 15');
INSERT INTO public.listing_item_specific VALUES ('6d52344e-32cc-4444-a35d-25340a46f590', 55, 'Storage Capacity', '512 GB');
INSERT INTO public.listing_item_specific VALUES ('6d52344e-32cc-4444-a35d-25340a46f590', 56, 'Color', 'Black');
INSERT INTO public.listing_item_specific VALUES ('6d52344e-32cc-4444-a35d-25340a46f590', 57, 'Network', 'Verizon');
INSERT INTO public.listing_item_specific VALUES ('f871bd3f-1abe-4f84-a4c5-2138a1be76b9', 58, 'Fabric Type', 'Cotton');


--
-- TOC entry 4567 (class 0 OID 33008)
-- Dependencies: 392
-- Data for Name: listing_template; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.listing_template VALUES ('81000000-0000-0000-0000-000000000001', 'Alice''s Starter Template', 'Reusable template seeded for demo purposes.', '{"price": 49.99, "title": "Sample Listing Template", "quantity": 5, "categoryId": "10000000-0000-0000-0000-000000000002", "conditionId": "40000000-0000-0000-0000-000000000001"}', 'Fixed Price', 'https://picsum.photos/seed/template-1/320/180', '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);
INSERT INTO public.listing_template VALUES ('82000000-0000-0000-0000-000000000001', 'Brian''s Starter Template', 'Reusable template seeded for demo purposes.', '{"price": 49.99, "title": "Sample Listing Template", "quantity": 5, "categoryId": "10000000-0000-0000-0000-000000000003", "conditionId": "40000000-0000-0000-0000-000000000001"}', 'Fixed Price', 'https://picsum.photos/seed/template-2/320/180', '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000002', NULL, NULL, false);
INSERT INTO public.listing_template VALUES ('83000000-0000-0000-0000-000000000001', 'Cecilia''s Starter Template', 'Reusable template seeded for demo purposes.', '{"price": 49.99, "title": "Sample Listing Template", "quantity": 5, "categoryId": "10000000-0000-0000-0000-000000000004", "conditionId": "40000000-0000-0000-0000-000000000001"}', 'Fixed Price', 'https://picsum.photos/seed/template-3/320/180', '2024-01-01 00:00:00+00', '70000000-0000-0000-0000-000000000003', NULL, NULL, false);


--
-- TOC entry 4640 (class 0 OID 37705)
-- Dependencies: 467
-- Data for Name: notification; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.notification VALUES ('ab13edf0-00d4-459c-a5f5-7ce908819867', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $130.00 on "Bán laptop sieu cap vip pro".', '81680948-d9af-4a81-a649-4ce9e3a63071', false, '2026-03-24 01:30:07.574503+00');
INSERT INTO public.notification VALUES ('a6f80f51-6be8-478f-8d79-73381a6a5693', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $135.00 on "Bán laptop sieu cap vip pro".', '81680948-d9af-4a81-a649-4ce9e3a63071', false, '2026-03-24 01:36:16.054367+00');
INSERT INTO public.notification VALUES ('8bfafa13-f458-4869-a0b3-b530ac168544', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $140.00 on "Bán laptop sieu cap vip pro".', '81680948-d9af-4a81-a649-4ce9e3a63071', false, '2026-03-24 01:57:34.151623+00');
INSERT INTO public.notification VALUES ('bd187b60-ae19-4e9b-839c-e7e4df3d7e01', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $145.00 on "Bán laptop sieu cap vip pro".', '81680948-d9af-4a81-a649-4ce9e3a63071', false, '2026-03-24 01:58:39.204712+00');
INSERT INTO public.notification VALUES ('f7141309-568c-43cc-a4d9-990390e4e44b', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $150.00 on "Bán laptop sieu cap vip pro".', '81680948-d9af-4a81-a649-4ce9e3a63071', false, '2026-03-24 02:01:47.847948+00');
INSERT INTO public.notification VALUES ('efbfdf78-97ad-4522-8fd7-660769327498', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $155.00 on "Bán laptop sieu cap vip pro".', '81680948-d9af-4a81-a649-4ce9e3a63071', false, '2026-03-24 02:21:07.410732+00');
INSERT INTO public.notification VALUES ('f6251dba-04eb-4bd6-b9a3-68d8f78fa96f', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $160.00 on "Bán laptop sieu cap vip pro".', '81680948-d9af-4a81-a649-4ce9e3a63071', false, '2026-03-24 02:21:36.934241+00');
INSERT INTO public.notification VALUES ('dc8614b4-c0ef-41c5-88f0-7b34203295af', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $600.00 on "Mock Product for hoapoki24".', '87233f8b-3f4c-427b-a300-83720258ed62', false, '2026-03-25 10:04:17.031516+00');
INSERT INTO public.notification VALUES ('eb7c9c4f-e9bf-4ce9-ac3a-b2eab935a364', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewOffer', 'New offer received!', 'Nguyễn Đức Thịnh made an offer of $490.00 on "Bán tranh T1".', '67f00868-4f9d-4486-a63e-20d6e0a161b7', false, '2026-03-25 13:57:27.746375+00');
INSERT INTO public.notification VALUES ('c6837a18-5ba8-4257-b454-80e63c5d0c97', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewOffer', 'New offer received!', 'Nguyễn Đức Thịnh made an offer of $490.00 on "Bán tranh T1".', '67f00868-4f9d-4486-a63e-20d6e0a161b7', false, '2026-03-25 13:57:43.677999+00');
INSERT INTO public.notification VALUES ('9853afda-c9eb-459c-aa59-833dd43be373', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewOffer', 'New offer received!', 'Nguyễn Đức Thịnh made an offer of $490.00 on "Bán tranh T1".', '67f00868-4f9d-4486-a63e-20d6e0a161b7', false, '2026-03-25 13:57:47.692015+00');
INSERT INTO public.notification VALUES ('08a9c00e-e0a3-44e7-bd3c-9abcd32c9994', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewOffer', 'New offer received!', 'Nguyễn Đức Thịnh made an offer of $490.00 on "Bán tranh T1".', '67f00868-4f9d-4486-a63e-20d6e0a161b7', false, '2026-03-25 13:57:49.697321+00');
INSERT INTO public.notification VALUES ('f0c2a598-d474-475f-bc5f-964e12f5fb7f', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $610.00 on "Mock Product for hoapoki24".', '87233f8b-3f4c-427b-a300-83720258ed62', false, '2026-03-25 15:25:45.665199+00');
INSERT INTO public.notification VALUES ('a2213b20-89be-4298-a234-80ca692ad5f1', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $615.00 on "Mock Product for hoapoki24".', '87233f8b-3f4c-427b-a300-83720258ed62', false, '2026-03-25 15:26:34.561062+00');
INSERT INTO public.notification VALUES ('53317c3c-e488-4124-b090-fa43e018f9f3', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewOffer', 'New offer received!', 'Nguyễn Đức Thịnh made an offer of $9.00 on "Báo mới".', 'f871bd3f-1abe-4f84-a4c5-2138a1be76b9', false, '2026-03-25 17:10:55.196026+00');
INSERT INTO public.notification VALUES ('0d948556-9820-4c3c-8b6e-c14d7f85d570', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NewBid', 'New bid placed!', 'Nguyễn Đức Thịnh placed a bid of $110.00 on "Iphone".', '6d52344e-32cc-4444-a35d-25340a46f590', false, '2026-03-25 17:11:34.441319+00');


--
-- TOC entry 4626 (class 0 OID 37400)
-- Dependencies: 453
-- Data for Name: offers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.offers VALUES ('5922b539-26c5-48d0-a928-0b37734f86d0', '67f00868-4f9d-4486-a63e-20d6e0a161b7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 480.00, 0, '2026-03-22 09:53:18.818615+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.offers VALUES ('4ad3e811-7c82-48ba-afc4-c0b0361b9aa2', '67f00868-4f9d-4486-a63e-20d6e0a161b7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 495.00, 0, '2026-03-22 09:54:10.195068+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.offers VALUES ('2b22f530-28ab-4bbf-9c92-06663903f9c8', '67f00868-4f9d-4486-a63e-20d6e0a161b7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 490.00, 0, '2026-03-25 13:57:27.715545+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.offers VALUES ('96857d3a-ccf4-431e-a8d2-61d0e60006f1', '67f00868-4f9d-4486-a63e-20d6e0a161b7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 490.00, 0, '2026-03-25 13:57:43.677089+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.offers VALUES ('44bf41b5-be33-403f-a902-4c53dbbf43a7', '67f00868-4f9d-4486-a63e-20d6e0a161b7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 490.00, 0, '2026-03-25 13:57:47.691795+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.offers VALUES ('743d11a8-679f-473c-9cb4-2d14b18b4396', '67f00868-4f9d-4486-a63e-20d6e0a161b7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 490.00, 0, '2026-03-25 13:57:49.697069+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);
INSERT INTO public.offers VALUES ('2b4191ac-efab-4e6d-a878-e17ef4081139', 'f871bd3f-1abe-4f84-a4c5-2138a1be76b9', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 9.00, 0, '2026-03-25 17:10:55.13079+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);


--
-- TOC entry 4603 (class 0 OID 33435)
-- Dependencies: 428
-- Data for Name: order_buyer_feedback; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4595 (class 0 OID 33331)
-- Dependencies: 420
-- Data for Name: order_cancellation_requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_cancellation_requests VALUES ('6f1f9f0c-898f-4c7b-bb38-1b689e9f7331', 'c721f605-43cb-4b1b-8f0c-b1c5833420a9', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 0, 1, 'Realized I ordered the wrong variation, please cancel.', NULL, '2025-10-13 14:15:00+00', '2025-10-15 14:15:00+00', NULL, NULL, NULL, 0, NULL, NULL, 114.69, 'USD', '2025-10-13 14:15:00+00', 'seed', '2025-10-13 14:15:00+00', 'seed', false);
INSERT INTO public.order_cancellation_requests VALUES ('c3c25c5b-f1a3-4e5f-9ccd-da6a46b91753', '973cac8a-9be0-44a0-90b7-fd8263f8e78a', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', 2, 99, NULL, 'Order auto-cancelled after missing shipping deadline.', '2025-10-30 09:00:00+00', NULL, NULL, '2025-10-30 09:00:00+00', '2025-10-30 09:00:00+00', 5, NULL, NULL, 97.37, 'USD', '2025-10-30 09:00:00+00', 'seed', '2025-10-30 09:00:00+00', 'seed', false);
INSERT INTO public.order_cancellation_requests VALUES ('d3f7d907-6b71-47d8-8651-922629540277', '7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 0, 3, 'Need to update the delivery address; requesting cancellation.', 'Approved – refund processing with payment provider.', '2025-10-19 12:00:00+00', '2025-10-21 12:00:00+00', '2025-10-19 18:00:00+00', NULL, NULL, 2, 87.83, 'USD', 87.83, 'USD', '2025-10-19 12:00:00+00', 'seed', '2025-10-19 18:00:00+00', 'seed', false);
INSERT INTO public.order_cancellation_requests VALUES ('5d4e7a11-0c4e-4a6f-9f2f-000000000004', '0f0c1a22-11aa-4c6d-8f10-000000000018', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 0, 4, 'Item still not handed to carrier, requesting cancellation.', 'Approved – refund issued to buyer''s original payment method.', '2025-11-06 18:00:00+00', '2025-11-08 18:00:00+00', '2025-11-07 09:30:00+00', NULL, NULL, 2, 107.23, 'USD', 107.23, 'USD', '2025-11-06 18:00:00+00', 'seed', '2025-11-07 09:30:00+00', 'seed', false);
INSERT INTO public.order_cancellation_requests VALUES ('5d4e7a11-0c4e-4a6f-9f2f-000000000005', '0f0c1a22-11aa-4c6d-8f10-00000000001b', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 0, 1, 'Accidentally placed duplicate order.', 'Seller did not respond within the 3 day window. Order must be fulfilled.', '2025-11-08 11:00:00+00', '2025-11-10 11:00:00+00', '2026-03-11 01:04:18.309614+00', NULL, '2026-03-11 01:04:18.309614+00', 4, NULL, NULL, 74.43, 'USD', '2025-11-08 11:00:00+00', 'seed', '2026-03-11 01:04:22.606698+00', 'System', false);
INSERT INTO public.order_cancellation_requests VALUES ('1c68a39c-2075-4e2c-afb7-fa4e6e5915d5', 'f6de3ce0-2d3d-4709-923d-cbb61f956947', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 1, 3, NULL, 'a', '2026-03-25 17:13:44.787938+00', NULL, NULL, NULL, NULL, 1, NULL, NULL, 114.69, 'USD', '2026-03-25 17:13:44.917656+00', '70000000-0000-0000-0000-000000000001', NULL, NULL, false);


--
-- TOC entry 4622 (class 0 OID 36221)
-- Dependencies: 449
-- Data for Name: order_discount_category_rules; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4623 (class 0 OID 36231)
-- Dependencies: 450
-- Data for Name: order_discount_item_rules; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4620 (class 0 OID 36203)
-- Dependencies: 447
-- Data for Name: order_discount_performance_metrics; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4624 (class 0 OID 36241)
-- Dependencies: 451
-- Data for Name: order_discount_tiers; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4621 (class 0 OID 36212)
-- Dependencies: 448
-- Data for Name: order_discounts; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_discounts VALUES ('e6db1f44-bc8e-4228-bd3f-e84c537f15c3', '00000000-0000-0000-0000-000000000000', 'test', 'test', 1, 10.00, NULL, 20.00, 1, 29.98, true, '2026-03-21 15:26:00+00', '2026-03-22 15:26:00+00', true, '2026-03-21 15:26:47.494724+00', 'db062e86-b522-41c2-b3b9-df461d830de8', NULL, NULL, false);


--
-- TOC entry 4599 (class 0 OID 33382)
-- Dependencies: 424
-- Data for Name: order_item_shipments; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_item_shipments VALUES ('e27a2e68-d264-461d-8a33-1886f635a645', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '22ff7156-f661-41dc-857b-a51dc251e790', '84d91ddf-a018-4bef-808b-ff680af34cd9', 'MOCKTRACK3967667', 'FedEx', '2026-03-21 10:59:20.73857+00', '2026-03-21 10:59:20.744449+00', NULL, false);
INSERT INTO public.order_item_shipments VALUES ('07613982-6a3c-4f72-8430-bbb074849859', 'ff697aba-9bf9-4782-98a1-55ad7961fb43', 'e8c04872-6a5e-4f47-8259-316dade7a8e0', NULL, '12345', 'USPSD', '2026-03-23 16:59:59.999+00', '2026-03-23 16:11:53.460894+00', NULL, false);
INSERT INTO public.order_item_shipments VALUES ('9ae92bec-7545-43d1-8760-d3e1e7e911c0', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '22ff7156-f661-41dc-857b-a51dc251e790', '3d2d88a9-6f6f-4bbc-b111-52b9d0b70838', 'MOCKTRACK6313238', 'FedEx', '2026-03-23 16:28:29.864964+00', '2026-03-23 16:28:29.870132+00', NULL, false);
INSERT INTO public.order_item_shipments VALUES ('b91d8ccc-4a33-47fc-a301-6741b0a4a6c9', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '22ff7156-f661-41dc-857b-a51dc251e790', 'be821324-7379-42c6-b639-f860b896b640', 'MOCKTRACK8993921', 'FedEx', '2026-03-23 18:24:14.536409+00', '2026-03-23 18:24:14.537151+00', NULL, false);
INSERT INTO public.order_item_shipments VALUES ('af0f5318-bdd9-4518-b89c-b4da3364133c', 'c14b68ae-d6d3-488f-831f-7eb17941bcf6', '20ecf467-7bc4-495b-829a-a90b7aee8c65', 'c9039a0a-bfbd-4e71-98fe-0a76926cd9f1', 'MOCKTRACK3239498', 'FedEx', '2026-03-23 18:56:11.240411+00', '2026-03-23 18:56:11.242115+00', NULL, false);
INSERT INTO public.order_item_shipments VALUES ('0455035d-b9b5-4afb-b08b-ffa5dfd5db7d', '8284e833-b4a9-431c-a134-174fa6ae045e', 'bd2e73be-266a-45cd-a95e-cd98d1fa310a', 'c5e280cb-3c1b-4c16-8102-40657ac1a0c3', 'MOCKTRACK6231408', 'USPS', '2026-03-24 01:07:34.784703+00', '2026-03-24 01:07:34.789266+00', NULL, false);
INSERT INTO public.order_item_shipments VALUES ('430ddb22-f5dc-4417-9fbf-db7ca5e5c853', 'ef2bc7c5-ceaa-4e29-b24c-eaa300a9d746', '9a8511ac-a090-4c13-b74f-4d920a39bd04', '1ff2ae6a-515e-4342-bc2b-c369235daa83', 'MOCKTRACK8812499', 'FedEx', '2026-03-24 01:55:34.264154+00', '2026-03-24 01:55:34.269537+00', NULL, false);


--
-- TOC entry 4588 (class 0 OID 33206)
-- Dependencies: 413
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
INSERT INTO public.order_items VALUES ('e8c04872-6a5e-4f47-8259-316dade7a8e0', 'f1210d09-a873-47b2-90cf-c9541b6f4a0e', NULL, 'Mock Product for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-123', 1, 500, 'USD', 500, 'USD', 'ff697aba-9bf9-4782-98a1-55ad7961fb43', '2026-03-21 10:49:50.496+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('22ff7156-f661-41dc-857b-a51dc251e790', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-123', 1, 500, 'USD', 500, 'USD', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '2026-03-21 10:50:29.582+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('699c605e-9928-47c9-aad2-3370d665c1e7', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', '728589dc-fb29-4c19-9dca-08efb03c4663', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('aba31811-c6c6-4f00-beef-2713bdc98abd', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', 'edc6fce4-c8cb-41be-951b-1fb584d4f173', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('20ecf467-7bc4-495b-829a-a90b7aee8c65', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', 'c14b68ae-d6d3-488f-831f-7eb17941bcf6', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('82f078e7-3908-4efb-a0af-b9d335207111', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', '121625ec-dff6-45f4-ae5e-e752b91081b5', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('3cf0e51f-9932-4160-bc43-7ff101a8a902', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', 'adf8ae42-9035-4fb4-bf08-fa97313c0095', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('2dfcbe22-5b3c-41a8-a2a8-1340c67536b1', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', '3ee1df27-e113-4282-9dfd-f319120272b4', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('9a8511ac-a090-4c13-b74f-4d920a39bd04', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', 'ef2bc7c5-ceaa-4e29-b24c-eaa300a9d746', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('8eadf31f-94f2-4387-8ec9-1d4936100d73', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', 'a4854812-e84b-419b-a7d1-38bca47f5abd', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('1f7f444b-c3a0-4f6e-9b55-8a3564d08808', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', '226764aa-a866-4359-8f82-9eab975c8580', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('1e70d971-99c4-42af-bdb2-a0a8dcb377a1', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', '5a14a2e5-b905-4f03-a540-d47aa0a2888e', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('bd2e73be-266a-45cd-a95e-cd98d1fa310a', '87233f8b-3f4c-427b-a300-83720258ed62', NULL, 'Mock Product 2 for hoapoki24', 'https://picsum.photos/200/300', 'MOCK-SKU-456', 1, 300, 'USD', 300, 'USD', '8284e833-b4a9-431c-a134-174fa6ae045e', '2026-03-21 10:50:29.82+00', NULL, NULL, NULL, false, '10000000-0000-0000-0000-000000000001');
INSERT INTO public.order_items VALUES ('ed80899c-6abd-4342-ac5e-e2acddc68657', '1a7ed4c4-ff65-48cb-b4ea-c8d39d33db89', NULL, 'Bán tranh sieu cap vipproo', 'https://res.cloudinary.com/djmftornv/image/upload/v1773507408/ebay-clone/Full team_17d21900-8d74-42f1-9c09-86f88f814433.jpg', 't16sa0', 1, 500, 'USD', 500, 'USD', 'c3ca850a-8a36-4ba6-91f0-eecfd794e46e', '2026-03-25 15:47:21.249706+00', 'system', '2026-03-25 15:47:21.249706+00', 'system', false, '20000000-0000-0000-0000-000000000007');


--
-- TOC entry 4596 (class 0 OID 33343)
-- Dependencies: 421
-- Data for Name: order_return_requests; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_return_requests VALUES ('8cb7ab44-0d7d-4d7d-9b24-1cc54d4da7bf', 'bd34cf77-4551-4194-ad16-d20c94b58289', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 0, 0, 'Item color differs from the listing photos.', NULL, '2025-10-29 10:00:00+00', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL, 108.88, 'USD', '2025-10-29 10:00:00+00', 'seed', '2025-10-29 10:00:00+00', 'seed', false);
INSERT INTO public.order_return_requests VALUES ('dc3329e1-14fb-4d00-a395-e76e25a6822b', 'fa236302-3864-4e54-9e40-3ebdb4749734', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 4, 3, 'Shoes run smaller than expected.', 'Refunded minus restocking fee.', '2025-11-04 17:45:00+00', '2025-11-04 20:30:00+00', '2025-11-09 23:59:00+00', '2025-11-06 09:10:00+00', '2025-11-08 16:20:00+00', '2025-11-09 14:00:00+00', '2025-11-09 14:00:00+00', 'USPS', '9405511899223857264837', 5, 150.00, 'USD', 5.00, 'USD', 177.37, 'USD', '2025-11-04 17:45:00+00', 'seed', '2025-11-09 14:00:00+00', 'seed', false);
INSERT INTO public.order_return_requests VALUES ('fd21bed5-6c0c-4bcf-b099-31c8b0d08f27', '1e86f219-1dd0-4cac-a545-cb98e65ce429', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 1, 1, 'Screen arrived cracked; requesting replacement.', 'Please return using the provided UPS label.', '2025-11-01 09:00:00+00', '2025-11-01 12:00:00+00', '2025-11-05 23:59:00+00', '2025-11-03 10:15:00+00', NULL, NULL, NULL, 'UPS', '1Z999AA10123456784', 2, NULL, NULL, NULL, NULL, 169.27, 'USD', '2025-11-01 09:00:00+00', 'seed', '2025-11-03 10:15:00+00', 'seed', false);
INSERT INTO public.order_return_requests VALUES ('9a7f6b12-5e2d-4d91-8c22-000000000004', '0f0c1a22-11aa-4c6d-8f10-00000000001a', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 2, 2, 'Received the 64GB variant instead of 128GB.', 'Exchange approved once return is in transit.', '2025-11-08 09:30:00+00', '2025-11-08 12:45:00+00', '2025-11-13 23:59:00+00', '2025-11-10 10:20:00+00', NULL, NULL, NULL, 'FedEx', '612999AA10NEWRT4', 2, NULL, NULL, NULL, NULL, 80.59, 'USD', '2025-11-08 09:30:00+00', 'seed', '2025-11-10 10:20:00+00', 'seed', false);
INSERT INTO public.order_return_requests VALUES ('9a7f6b12-5e2d-4d91-8c22-000000000005', '0f0c1a22-11aa-4c6d-8f10-00000000001d', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 6, 0, 'Decided to keep a different model instead.', 'Refund pending inspection of returned item.', '2025-11-09 15:00:00+00', '2025-11-09 17:15:00+00', '2025-11-14 23:59:00+00', '2025-11-11 08:40:00+00', '2025-11-13 16:05:00+00', NULL, NULL, 'USPS', '9405511899223857264999', 4, NULL, NULL, NULL, NULL, 112.57, 'USD', '2025-11-09 15:00:00+00', 'seed', '2025-11-13 16:05:00+00', 'seed', false);


--
-- TOC entry 4589 (class 0 OID 33218)
-- Dependencies: 414
-- Data for Name: order_shipping_labels; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_shipping_labels VALUES ('84d91ddf-a018-4bef-808b-ff680af34cd9', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK3967667', '/storage/shipping-labels/MOCKTRACK3967667_20260321105920.pdf', 'MOCKTRACK3967667_20260321105920.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-21 10:59:20.604928+00', NULL, '8bf8eece-62cb-4af6-a5cd-33f8f1c2c771', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('3d2d88a9-6f6f-4bbc-b111-52b9d0b70838', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK6313238', '/storage/shipping-labels/MOCKTRACK6313238_20260323162829.pdf', 'MOCKTRACK6313238_20260323162829.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 16:28:29.813051+00', NULL, 'f1667a37-5439-4632-be1e-e5c1a19e9dc6', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('1d88bc07-7067-47b8-b851-84df500f7f22', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK9926463', '/storage/shipping-labels/MOCKTRACK9926463_20260323181610.pdf', 'MOCKTRACK9926463_20260323181610.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:16:10.025633+00', NULL, '355d6cd4-93f4-4306-8c61-96594ff6e5ab', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('1c753644-3818-4a81-8102-291c95d00d19', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK4737881', '/storage/shipping-labels/MOCKTRACK4737881_20260323181628.pdf', 'MOCKTRACK4737881_20260323181628.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:16:28.099111+00', NULL, '41ed8a40-0ad3-4ef7-9ce1-2bab05baa884', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('b7d00c04-5b85-475f-a8b0-fc4eb6939802', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK9799307', '/storage/shipping-labels/MOCKTRACK9799307_20260323181712.pdf', 'MOCKTRACK9799307_20260323181712.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:17:12.422295+00', NULL, 'e34b2da4-7f72-4f8c-b858-82049e691e0b', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('799622bb-8c20-407b-98d9-44c27d7a8111', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK9558126', '/storage/shipping-labels/MOCKTRACK9558126_20260323181737.pdf', 'MOCKTRACK9558126_20260323181737.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:17:37.432543+00', NULL, 'ca9a564a-77a4-44ab-842b-a6b6a309dbb2', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('92d29e06-9471-4969-b2d8-6f24bcfe713e', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK9371718', '/storage/shipping-labels/MOCKTRACK9371718_20260323181816.pdf', 'MOCKTRACK9371718_20260323181816.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:18:16.011384+00', NULL, 'baa620d6-340a-43cf-966a-45e01d46e463', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('9909dac0-7073-45e6-9382-6022f5666f0c', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '00000000-0000-0000-0000-000000000000', 'USPS', 'USPS_PRIORITY_MAIL_FLAT_RATE_LEGAL_ENVELOPE', 'FedEx Ground Economy', 'MOCKTRACK7297685', '/storage/shipping-labels/MOCKTRACK7297685_20260323181850.pdf', 'MOCKTRACK7297685_20260323181850.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:18:50.00098+00', NULL, 'fcef0dba-a6bc-42d2-9ff8-ed37fedfe482', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('0959c321-deab-46c8-b5ef-efff743c5079', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK5540514', '/storage/shipping-labels/MOCKTRACK5540514_20260323182018.pdf', 'MOCKTRACK5540514_20260323182018.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:20:18.928729+00', NULL, '4993e3e7-11c0-45df-bd42-07894cd7ef60', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('be821324-7379-42c6-b639-f860b896b640', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK8993921', '/storage/shipping-labels/MOCKTRACK8993921_20260323182414.pdf', 'MOCKTRACK8993921_20260323182414.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:24:14.532519+00', NULL, '25784ffc-2fc6-4204-b6e1-7d9c25e0a7a5', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('6c5753db-83ef-4020-a1da-4bc1233f20ff', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK7994749', '/storage/shipping-labels/MOCKTRACK7994749_20260323183105.pdf', 'MOCKTRACK7994749_20260323183105.pdf', 10.00, 'VNĐ', 510.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:31:05.864592+00', NULL, '2a30a263-d03e-4d08-b0f0-91f0ebb24b60', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('c9039a0a-bfbd-4e71-98fe-0a76926cd9f1', 'c14b68ae-d6d3-488f-831f-7eb17941bcf6', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK3239498', '/storage/shipping-labels/MOCKTRACK3239498_20260323185611.pdf', 'MOCKTRACK3239498_20260323185611.pdf', 10.00, 'VNĐ', 315.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-23 18:56:11.237326+00', NULL, '1596dcb4-a1cf-4c0d-8aa0-b33e03239155', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('c5e280cb-3c1b-4c16-8102-40657ac1a0c3', '8284e833-b4a9-431c-a134-174fa6ae045e', '00000000-0000-0000-0000-000000000000', 'USPS', 'USPS_PRIORITY_MAIL_FLAT_RATE_LEGAL_ENVELOPE', 'USPS Priority Mail Flat Rate Legal Envelope', 'MOCKTRACK6231408', '/storage/shipping-labels/MOCKTRACK6231408_20260324010734.pdf', 'MOCKTRACK6231408_20260324010734.pdf', 10.00, 'VNĐ', 315.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-24 01:07:34.736954+00', NULL, '438e3582-9cac-414e-a94b-6e95314e0981', false, NULL, NULL);
INSERT INTO public.order_shipping_labels VALUES ('1ff2ae6a-515e-4342-bc2b-c369235daa83', 'ef2bc7c5-ceaa-4e29-b24c-eaa300a9d746', '00000000-0000-0000-0000-000000000000', 'FedEx', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'MOCKTRACK8812499', '/storage/shipping-labels/MOCKTRACK8812499_20260324015534.pdf', 'MOCKTRACK8812499_20260324015534.pdf', 10.00, 'VNĐ', 315.00, 'USD', 'MockBox', 16.00, 12.00, 10.00, 4.00, '2026-03-24 01:55:34.225197+00', NULL, 'c34c225f-1186-43b9-8dc8-b4e573b8df1f', false, NULL, NULL);


--
-- TOC entry 4590 (class 0 OID 33230)
-- Dependencies: 415
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
INSERT INTO public.order_status_histories VALUES ('d5f94301-bdcb-49a1-a954-3cfcd529fb10', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '2026-03-21 10:50:29.662+00');
INSERT INTO public.order_status_histories VALUES ('3da4a93a-efd7-4d6a-9d90-5793ce4d171a', '728589dc-fb29-4c19-9dca-08efb03c4663', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-21 10:50:29.905+00');
INSERT INTO public.order_status_histories VALUES ('cf97fd59-fe28-40cc-80af-1297e56f7f99', '5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-21 10:59:20.870679+00');
INSERT INTO public.order_status_histories VALUES ('925245d9-5836-430f-b628-ab00f149fb46', '728589dc-fb29-4c19-9dca-08efb03c4663', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '970c8d97-6081-43db-9083-8f3c026ded84', '2026-03-21 11:08:05.897961+00');
INSERT INTO public.order_status_histories VALUES ('c77635d9-da2c-46ff-89ab-6f72ebe97e24', 'ff697aba-9bf9-4782-98a1-55ad7961fb43', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-23 16:11:53.562293+00');
INSERT INTO public.order_status_histories VALUES ('b5e83ffb-b96d-44a1-9852-051b43eb6bc0', 'fe55bfdf-86b7-4a37-9750-ed0d68612313', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-23 18:31:06.01695+00');
INSERT INTO public.order_status_histories VALUES ('0a29553b-3c84-4679-a7c7-647acff52c74', 'c14b68ae-d6d3-488f-831f-7eb17941bcf6', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-23 18:56:11.315469+00');
INSERT INTO public.order_status_histories VALUES ('e7e8f98e-fe73-4825-b536-d53cfff82b3e', '8284e833-b4a9-431c-a134-174fa6ae045e', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-24 01:07:35.010243+00');
INSERT INTO public.order_status_histories VALUES ('5b02b27b-0ee9-4344-af9d-5eab5b3ea84c', 'ef2bc7c5-ceaa-4e29-b24c-eaa300a9d746', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-24 01:55:34.458739+00');
INSERT INTO public.order_status_histories VALUES ('04f572b3-a2c5-4efe-8949-7cb5c0cb6fb5', 'c14b68ae-d6d3-488f-831f-7eb17941bcf6', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '970c8d97-6081-43db-9083-8f3c026ded84', '2026-03-24 02:01:21.874869+00');
INSERT INTO public.order_status_histories VALUES ('9d468af4-8fed-4f27-9a53-5ea0d6699557', '5a14a2e5-b905-4f03-a540-d47aa0a2888e', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-25 16:27:19.152133+00');
INSERT INTO public.order_status_histories VALUES ('82951243-9630-490a-8dd9-b19284369379', 'ff697aba-9bf9-4782-98a1-55ad7961fb43', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '970c8d97-6081-43db-9083-8f3c026ded84', '2026-03-25 16:27:51.357655+00');
INSERT INTO public.order_status_histories VALUES ('e55f7c46-b5e1-4d49-96e9-2d9d1c6984f2', '121625ec-dff6-45f4-ae5e-e752b91081b5', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-25 16:28:37.875148+00');
INSERT INTO public.order_status_histories VALUES ('68cdc2ed-c166-4403-b780-9b59e56105ae', 'f6de3ce0-2d3d-4709-923d-cbb61f956947', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '2026-03-25 17:14:03.902857+00');


--
-- TOC entry 4582 (class 0 OID 33123)
-- Dependencies: 407
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
-- TOC entry 4568 (class 0 OID 33015)
-- Dependencies: 393
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
-- TOC entry 4585 (class 0 OID 33162)
-- Dependencies: 410
-- Data for Name: orders; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.orders VALUES ('1e86f219-1dd0-4cac-a545-cb98e65ce429', 'ORD-SEED-1009', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 145.97, 'USD', 14.0, 'USD', 6.2, 'USD', 10.6, 'USD', 7.5, 'USD', 169.27, 'USD', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930', 2, 0, '2025-10-28 12:10:00+00', '2025-10-28 13:10:00+00', '2025-10-28 22:10:00+00', '2025-10-31 12:10:00+00', NULL, NULL, 'HOLIDAY10', NULL, '2025-10-28 12:10:00+00', 'seed', '2025-10-31 12:10:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('973cac8a-9be0-44a0-90b7-fd8263f8e78a', 'ORD-SEED-1006', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', 79.98, 'USD', 11.0, 'USD', 3.99, 'USD', 6.4, 'USD', 4.0, 'USD', 97.37, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-10-24 11:05:00+00', '2025-10-24 11:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-10-24 11:05:00+00', 'seed', '2025-10-24 11:50:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('a4206ad5-6a35-43bb-8a8c-8c7b244594ac', 'ORD-SEED-1008', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 92.98, 'USD', 13.0, 'USD', 4.6, 'USD', 8.6, 'USD', 0.0, 'USD', 119.18, 'USD', '859b47f4-0d05-4f43-8ff5-57acb8d5da1d', 0, 0, '2025-10-26 18:40:00+00', '2025-10-26 19:40:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-10-26 18:40:00+00', 'seed', '2025-10-26 19:40:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('bd34cf77-4551-4194-ad16-d20c94b58289', 'ORD-SEED-1007', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 84.98, 'USD', 12.5, 'USD', 4.2, 'USD', 7.2, 'USD', 0.0, 'USD', 108.88, 'USD', '5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9', 5, 0, '2025-10-25 15:30:00+00', '2025-10-25 17:30:00+00', '2025-10-26 15:30:00+00', '2025-10-28 15:30:00+00', NULL, NULL, NULL, NULL, '2025-10-25 15:30:00+00', 'seed', '2025-10-28 15:30:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('d2ee4d4a-5be0-4d76-bce6-0b8578c87407', 'ORD-SEED-1005', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 76.98, 'USD', 10.0, 'USD', 4.1, 'USD', 6.16, 'USD', 0.0, 'USD', 97.24, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2025-10-22 08:20:00+00', '2025-10-22 09:20:00+00', '2025-10-22 17:20:00+00', NULL, NULL, NULL, NULL, NULL, '2025-10-22 08:20:00+00', 'seed', '2025-10-22 17:20:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('fa236302-3864-4e54-9e40-3ebdb4749734', 'ORD-SEED-1010', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 152.97, 'USD', 15.0, 'USD', 7.2, 'USD', 12.2, 'USD', 10.0, 'USD', 177.37, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2025-10-30 09:05:00+00', '2025-10-30 10:05:00+00', '2025-10-30 22:05:00+00', '2025-11-03 09:05:00+00', NULL, NULL, 'BULKBUY', NULL, '2025-10-30 09:05:00+00', 'seed', '2025-11-03 09:05:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000012', 'ORD-SEED-1012', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 64.5, 'USD', 8.25, 'USD', 3.45, 'USD', 4.86, 'USD', 2.5, 'USD', 78.56, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-02 13:30:00+00', '2025-11-02 14:10:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-02 13:30:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000013', 'ORD-SEED-1013', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 72.0, 'USD', 10.0, 'USD', 3.9, 'USD', 6.12, 'USD', 0.0, 'USD', 92.02, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-03 17:05:00+00', '2025-11-03 17:55:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-03 17:05:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000016', 'ORD-SEED-1016', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 48.4, 'USD', 7.5, 'USD', 2.45, 'USD', 3.87, 'USD', 0.0, 'USD', 62.22, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-05 11:30:00+00', '2025-11-05 11:55:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-05 11:30:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000017', 'ORD-SEED-1017', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 79.9, 'USD', 9.95, 'USD', 3.95, 'USD', 6.39, 'USD', 0.0, 'USD', 100.19, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-06 09:20:00+00', '2025-11-06 09:40:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-06 09:20:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000019', 'ORD-SEED-1019', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002', 71.25, 'USD', 8.75, 'USD', 3.55, 'USD', 5.7, 'USD', 0.0, 'USD', 89.25, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-07 08:10:00+00', '2025-11-07 08:28:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-07 08:10:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001a', 'ORD-SEED-1020', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003', 65.8, 'USD', 8.4, 'USD', 3.25, 'USD', 4.94, 'USD', 1.8, 'USD', 80.59, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-07 15:25:00+00', '2025-11-07 15:47:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-07 15:25:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('c721f605-43cb-4b1b-8f0c-b1c5833420a9', 'ORD-SEED-1002', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 66.98, 'USD', 12.0, 'USD', 3.35, 'USD', 5.36, 'USD', 5.0, 'USD', 82.69, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', '2025-10-16 00:15:00+00', NULL, NULL, NULL, 'OCTDEAL', NULL, '2025-10-15 14:15:00+00', 'seed', '2025-10-16 00:15:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4', 'ORD-SEED-1004', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 74.98, 'USD', 0.0, 'USD', 3.25, 'USD', 5.5, 'USD', 0.0, 'USD', 83.73, 'USD', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', 6, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, '2025-10-21 16:00:00+00', NULL, NULL, '2025-10-20 16:00:00+00', 'seed', '2025-10-21 16:00:00+00', 'seed', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000011', 'ORD-SEED-1011', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 58.99, 'USD', 9.95, 'USD', 3.1, 'USD', 5.3, 'USD', 0.0, 'USD', 77.34, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-01 09:15:00+00', 'seed', '2026-03-11 02:00:07.8521+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000014', 'ORD-SEED-1014', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 55.75, 'USD', 7.8, 'USD', 2.95, 'USD', 4.46, 'USD', 1.2, 'USD', 69.76, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-04 08:45:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000015', 'ORD-SEED-1015', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 62.75, 'USD', 8.25, 'USD', 3.15, 'USD', 5.02, 'USD', 2.5, 'USD', 76.67, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-05 10:00:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000018', 'ORD-SEED-1018', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 88.6, 'USD', 10.25, 'USD', 4.3, 'USD', 7.08, 'USD', 3.0, 'USD', 107.23, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-06 13:45:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001c', 'ORD-SEED-1022', '70000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 83.45, 'USD', 9.1, 'USD', 3.8, 'USD', 6.68, 'USD', 2.2, 'USD', 100.83, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-08 14:05:00+00', '2025-11-08 14:37:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-08 14:05:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001d', 'ORD-SEED-1023', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 90.3, 'USD', 10.6, 'USD', 4.45, 'USD', 7.22, 'USD', 0.0, 'USD', 112.57, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2025-11-09 11:15:00+00', '2025-11-09 11:39:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-09 11:15:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);
INSERT INTO public.orders VALUES ('5bc5ea1f-cfd3-4213-8a43-fb49c1e071d2', 'ORD-1DE37ADB', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 500, 'USD', 10, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 510, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2026-03-21 10:50:29.504+00', '2026-03-21 10:50:29.504+00', '2026-03-21 10:59:20.73857+00', '2026-03-21 10:59:20.871786+00', NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.504+00', NULL, '2026-03-21 10:59:44.988177+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('728589dc-fb29-4c19-9dca-08efb03c4663', 'ORD-A358BB61', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '970c8d97-6081-43db-9083-8f3c026ded84', 6, 0, '2026-03-21 10:50:29.742+00', '2026-03-21 10:50:29.742+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-21 11:08:05.898699+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('fe55bfdf-86b7-4a37-9750-ed0d68612313', 'ORD-0CE7A212', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 500, 'USD', 10, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 510, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 0, 0, '2026-03-21 10:48:41.948+00', '2026-03-21 10:48:41.948+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:48:41.948+00', NULL, '2026-03-23 18:31:09.417685+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('edc6fce4-c8cb-41be-951b-1fb584d4f173', 'ORD-SEED-20260323185257-01-F1D0', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 6, 0, '2026-03-23 18:45:58.00546+00', '2026-03-23 18:45:58.00546+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-21 11:08:05.898699+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('adf8ae42-9035-4fb4-bf08-fa97313c0095', 'ORD-SEED-20260323185258-04-3CCF', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 6, 0, '2026-03-23 18:24:58.466932+00', '2026-03-23 18:24:58.466932+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-21 11:08:05.898699+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('3ee1df27-e113-4282-9dfd-f319120272b4', 'ORD-SEED-20260323185258-05-884B', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 6, 0, '2026-03-23 18:17:58.612828+00', '2026-03-23 18:17:58.612828+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-21 11:08:05.898699+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('a4854812-e84b-419b-a7d1-38bca47f5abd', 'ORD-SEED-20260323185258-07-34AB', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 6, 0, '2026-03-23 18:03:58.907833+00', '2026-03-23 18:03:58.907833+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-21 11:08:05.898699+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('226764aa-a866-4359-8f82-9eab975c8580', 'ORD-SEED-20260323185259-08-32BE', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 6, 0, '2026-03-23 17:56:59.054966+00', '2026-03-23 17:56:59.054966+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-21 11:08:05.898699+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('8284e833-b4a9-431c-a134-174fa6ae045e', 'ORD-SEED-20260323185259-10-86F8', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2026-03-23 17:42:59.34959+00', '2026-03-23 17:42:59.34959+00', '2026-03-24 01:07:34.784703+00', NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-24 01:07:39.249717+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('ef2bc7c5-ceaa-4e29-b24c-eaa300a9d746', 'ORD-SEED-20260323185258-06-EFC3', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 2, 0, '2026-03-23 18:10:58.760538+00', '2026-03-23 18:10:58.760538+00', '2026-03-24 01:55:34.264154+00', NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-24 01:55:37.931513+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('c14b68ae-d6d3-488f-831f-7eb17941bcf6', 'ORD-SEED-20260323185258-02-ADAE', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '970c8d97-6081-43db-9083-8f3c026ded84', 6, 0, '2026-03-23 18:38:58.17463+00', '2026-03-23 18:38:58.17463+00', '2026-03-23 18:56:11.240411+00', NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-24 02:01:21.876153+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001b', 'ORD-SEED-1021', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 59.1, 'USD', 7.95, 'USD', 2.95, 'USD', 4.43, 'USD', 0.0, 'USD', 74.43, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-08 10:40:00+00', 'seed', '2026-03-11 02:00:07.852137+00', 'System', false);
INSERT INTO public.orders VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001e', 'ORD-SEED-1024', '70000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001', 74.95, 'USD', 8.9, 'USD', 3.6, 'USD', 5.83, 'USD', 1.5, 'USD', 91.78, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-11-09 17:50:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);
INSERT INTO public.orders VALUES ('7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', 'ORD-SEED-1003', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 70.98, 'USD', 9.25, 'USD', 3.4, 'USD', 6.2, 'USD', 2.0, 'USD', 87.83, 'USD', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 0, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-10-18 09:45:00+00', 'seed', '2026-03-11 02:00:07.852138+00', 'System', false);
INSERT INTO public.orders VALUES ('c3ca850a-8a36-4ba6-91f0-eecfd794e46e', 'ORD-2D54B1B2', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 500, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 500, 'USD', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 0, 0, '2026-03-25 15:47:21.249706+00', '2026-03-25 15:47:21.249706+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-25 15:47:21.249706+00', 'system', '2026-03-25 15:47:21.249706+00', 'system', false);
INSERT INTO public.orders VALUES ('5a14a2e5-b905-4f03-a540-d47aa0a2888e', 'ORD-SEED-20260323185259-09-2E33', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 0, 0, '2026-03-23 17:49:59.202701+00', '2026-03-23 17:49:59.202701+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-25 16:27:19.248054+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('ff697aba-9bf9-4782-98a1-55ad7961fb43', 'ORD-1859D13E', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 500, 'USD', 10, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 510, 'USD', '970c8d97-6081-43db-9083-8f3c026ded84', 6, 0, '2026-03-21 10:49:50.415+00', '2026-03-21 10:49:50.415+00', '2026-03-23 16:59:59.999+00', NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:49:50.415+00', NULL, '2026-03-25 16:27:51.373844+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('121625ec-dff6-45f4-ae5e-e752b91081b5', 'ORD-SEED-20260323185258-03-0F57', '70000000-0000-0000-0000-000000000002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 300, 'USD', 15, 'USD', 0, 'USD', 0, 'USD', 0, 'USD', 315, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 0, 0, '2026-03-23 18:31:58.320676+00', '2026-03-23 18:31:58.320676+00', NULL, NULL, NULL, NULL, NULL, NULL, '2026-03-21 10:50:29.742+00', NULL, '2026-03-25 16:28:37.875933+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false);
INSERT INTO public.orders VALUES ('f6de3ce0-2d3d-4709-923d-cbb61f956947', 'ORD-SEED-1001', '70000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001', 93.97, 'USD', 8.5, 'USD', 4.7, 'USD', 7.52, 'USD', 0.0, 'USD', 114.69, 'USD', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 0, 0, '2026-03-20 17:50:00+00', '2026-03-20 17:50:00+00', NULL, NULL, NULL, NULL, NULL, NULL, '2025-10-12 10:30:00+00', 'seed', '2026-03-25 17:14:03.91347+00', '70000000-0000-0000-0000-000000000001', false);


--
-- TOC entry 4569 (class 0 OID 33022)
-- Dependencies: 394
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
INSERT INTO public.otp VALUES ('00765380-4894-487a-b164-e9dd0a8a9d3b', 'haingoc0217@gmail.com', '287239', '2026-03-20 05:27:05.681769+00', true, 1, '2026-03-20 05:22:05.700803+00', 'System', '2026-03-20 05:22:27.961191+00', 'db062e86-b522-41c2-b3b9-df461d830de8', false);
INSERT INTO public.otp VALUES ('ff9f5eeb-d98e-4123-bc90-11aa714fd669', 'haingoc02217@gmail.com', '629886', '2026-03-20 14:30:27.40584+00', false, 1, '2026-03-20 14:25:27.429881+00', 'System', NULL, NULL, false);
INSERT INTO public.otp VALUES ('6436da4f-b388-407d-8a4f-d3afd0811f00', 'vtrgiangg2903@gmail.com', '594805', '2026-03-25 14:13:58.785261+00', false, 2, '2026-03-25 14:08:58.859017+00', 'System', NULL, NULL, false);
INSERT INTO public.otp VALUES ('9d94a05e-b5c8-406e-b823-1e6be117a140', 'vtrgiangg2903@gmail.com', '939398', '2026-03-25 14:16:11.658069+00', true, 3, '2026-03-25 14:11:11.65857+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2026-03-25 14:15:01.345957+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', false);
INSERT INTO public.otp VALUES ('358583bf-a018-4a75-8e66-fb353640cee0', 'lengochai012004@gmail.com', '220138', '2026-03-25 15:42:34.207723+00', false, 1, '2026-03-25 15:37:34.274004+00', 'System', NULL, NULL, false);
INSERT INTO public.otp VALUES ('00a1a6f8-50a5-4b92-acb1-9badbfd72fe9', 'vtrgiangg2903@gmail.com', '739170', '2026-03-25 16:13:30.454836+00', false, 2, '2026-03-25 16:08:30.476411+00', 'System', NULL, NULL, false);
INSERT INTO public.otp VALUES ('67c846ed-e2dd-4b94-897e-89ab6920b06a', 'aaa@gmail.com', '136940', '2026-03-25 17:37:02.1874+00', false, 1, '2026-03-25 17:32:02.23447+00', 'System', NULL, NULL, false);


--
-- TOC entry 4570 (class 0 OID 33029)
-- Dependencies: 395
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
INSERT INTO public.outbox_message VALUES ('cb9a00f8-84e4-4b18-8929-210411358ac6', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"db062e86-b522-41c2-b3b9-df461d830de8"},"FullName":"Le Hai","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"haingoc0217@gmail.com"},"OccurredOn":"2026-03-20T05:21:58.2380376Z"}', '2026-03-20 05:21:58.238037+00', '2026-03-20 05:22:05.858221+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('aababe55-a28e-4ccc-a6a1-1b9b6113f0f8', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"haingoc0217@gmail.com","Code":"287239","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-20T05:22:05.6818463Z"}', '2026-03-20 05:22:05.681846+00', '2026-03-20 05:22:19.933276+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('3deeac72-eba2-4afd-9b55-ba697a6d7900', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"6a0bf519-a43b-4f4c-8029-350483e7ffae"},"FullName":"Le Hai","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"haingoc02217@gmail.com"},"OccurredOn":"2026-03-20T14:25:17.3034327Z"}', '2026-03-20 14:25:17.303432+00', '2026-03-20 14:25:27.593247+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('3078f04f-8b80-44bb-8851-0f075eb68d2a', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"haingoc02217@gmail.com","Code":"629886","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-20T14:25:27.4060093Z"}', '2026-03-20 14:25:27.406009+00', '2026-03-20 14:25:42.1431+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('6ed40c09-b91b-4ed5-b0d4-c901b183eb8b', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"87b82f07-53c2-46a4-bdde-f1589b3aa372"},"FullName":"haitest","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"lengochai012004@gmail.com"},"OccurredOn":"2026-03-25T15:37:33.3977723Z"}', '2026-03-25 15:37:33.397772+00', '2026-03-25 15:37:34.60168+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('6ba12b86-45a3-4173-8515-eceb469880da', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"vtrgiangg2903@gmail.com","Code":"594805","ExpiresInMinutes":5,"Type":2,"OccurredOn":"2026-03-25T14:08:58.7856898Z"}', '2026-03-25 14:08:58.785689+00', '2026-03-25 14:09:09.784378+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('ca1c7881-a1c3-4284-b099-5105a8ce22dc', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"vtrgiangg2903@gmail.com","Code":"739170","ExpiresInMinutes":5,"Type":2,"OccurredOn":"2026-03-25T16:08:30.4551225Z"}', '2026-03-25 16:08:30.455122+00', '2026-03-25 16:08:31.341227+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('2f06048c-02ba-4582-bb44-7db054ec72e6', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"vtrgiangg2903@gmail.com","Code":"939398","ExpiresInMinutes":5,"Type":3,"OccurredOn":"2026-03-25T14:11:11.6580723Z"}', '2026-03-25 14:11:11.658072+00', '2026-03-25 14:11:19.882776+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('55c9ae34-685a-455a-9587-6d0310d339e2', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"lengochai012004@gmail.com","Code":"220138","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-25T15:37:34.2080309Z"}', '2026-03-25 15:37:34.20803+00', '2026-03-25 15:40:16.058212+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('a3a17e40-a3ec-447f-b964-a33a4e3c0eba', 'PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Users.Events.UserRegisteredDomainEvent, PRN232_EbayClone.Domain","UserId":{"$type":"PRN232_EbayClone.Domain.Users.ValueObjects.UserId, PRN232_EbayClone.Domain","Value":"d9814bda-f007-413a-9d73-256bbf43ced4"},"FullName":"aaaaaa","Email":{"$type":"PRN232_EbayClone.Domain.Shared.ValueObjects.Email, PRN232_EbayClone.Domain","Value":"aaa@gmail.com"},"OccurredOn":"2026-03-25T17:32:00.4066884Z"}', '2026-03-25 17:32:00.406688+00', '2026-03-25 17:32:02.10389+00', 0, NULL);
INSERT INTO public.outbox_message VALUES ('da07dba6-ab35-47a8-89d0-32c70e1b141e', 'PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null', '{"$type":"PRN232_EbayClone.Domain.Identity.Events.OtpGeneratedDomainEvent, PRN232_EbayClone.Domain","Email":"aaa@gmail.com","Code":"136940","ExpiresInMinutes":5,"Type":1,"OccurredOn":"2026-03-25T17:32:02.187552Z"}', '2026-03-25 17:32:02.187552+00', '2026-03-25 17:32:04.647482+00', 0, NULL);


--
-- TOC entry 4586 (class 0 OID 33179)
-- Dependencies: 411
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
INSERT INTO public.refresh_token VALUES ('911c24ff-f1ba-4e62-b989-ac78ab161ef6', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'yZ8rqWLkO4lFdWMFDMmUAR7s6ImW4tbg7b7YIVwM7KI=', '2026-03-25 15:50:13.861328+00', '2026-03-18 15:50:14.183049+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('4087279d-fbc0-48d6-8ac2-f7b5101421cf', '70000000-0000-0000-0000-000000000001', 'gBsvaSBKeNr7e63cRP7cXeIKNiEJ91FlCMte2/zFZoo=', '2026-03-25 15:52:08.876768+00', '2026-03-18 15:52:08.89167+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c2cb1502-e93f-4167-a702-4e51f6e3d194', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '3EkalklwuFnI0MAgCaPCFo1jRGRHdE0ccNV0DASIh7k=', '2026-03-26 02:32:16.856955+00', '2026-03-19 02:32:16.972511+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c6b28b35-3558-447f-a543-fae65d88962d', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'RTT2evUgvK/7i6VLGlzJVx8TFgbW8ALE7kj9vYOSWyE=', '2026-03-26 02:38:29.428524+00', '2026-03-19 02:38:29.537687+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('dd49881c-5a83-4a97-b499-7b8b04d1c1c5', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'JzbhaGClJL0tMJW/pMddjWUTQ3+dP2k19rcR88+48bQ=', '2026-03-26 03:06:34.205411+00', '2026-03-19 03:06:34.287056+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('11ddeb47-5a92-440a-be1d-443ec80e65f4', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'qcrWTlC9G44di5gfTQgmD8PxhzhJNIfcXBp/cfd+5nY=', '2026-03-26 03:07:22.239416+00', '2026-03-19 03:07:22.243423+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('22785478-0863-4b4b-8759-5c75e3e85ae2', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '54DWqEm5i0pfegK3swx+ovNWbRLQ2DTGwEqp+U4RjZE=', '2026-03-26 03:09:39.125492+00', '2026-03-19 03:09:39.125944+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f71cf48f-5db6-44aa-9519-737708aa9b6b', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'RiScO5ynPLC8eJewi3oBZKtDIlU2JvoivwB2LqWLjKg=', '2026-03-26 03:55:09.316849+00', '2026-03-19 03:55:09.370826+00', 'System', '2026-03-19 04:05:37.159791+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('1ae00c06-20dc-4327-abac-a72a6cd6dbe8', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'qhexGsDDzw9QCLgR3wSzReT5FJd5RjtfmUONHAzWDm0=', '2026-03-26 04:05:37.158683+00', '2026-03-19 04:05:37.159777+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('3730c023-c7a1-4753-8d60-0eac4b6fa167', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'oOaMMS9ymN/wfHYg55Hnd5Y7V1iR6vZ5yi4zUnNh3X0=', '2026-03-26 04:22:37.773964+00', '2026-03-19 04:22:37.77427+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('d8f71b35-997f-4ba6-99a1-9caf0272effa', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'eU21Ksw9GJQ/x24M3ATVEmW7Ux1PAj24XdXlfN5aSXg=', '2026-03-26 04:27:17.806048+00', '2026-03-19 04:27:17.806394+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('2869cb4d-2a17-4855-94a0-045f46fb1e80', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'OlJgkKi3+RRew24xgW6hnzDIrcESHLvkBS/SoXJC2jk=', '2026-03-26 04:35:46.827926+00', '2026-03-19 04:35:46.828626+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('33173fc6-ede5-41e8-8df9-4119b4a21fdf', 'db062e86-b522-41c2-b3b9-df461d830de8', 'HzzvW0x5ptRynXiOQwKzpjl8S3znm5xlMrx+SYBB5DE=', '2026-03-27 05:21:58.398378+00', '2026-03-20 05:21:58.468368+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('5146ef41-57c8-4ed6-b3ac-0a76f064b821', '6a0bf519-a43b-4f4c-8029-350483e7ffae', 'pN1J3PUU6y8yRJkU/bz7vRU9DWumWxqYis7mJG7uDY4=', '2026-03-27 14:25:17.370565+00', '2026-03-20 14:25:17.417169+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('ec1c60e9-5894-4fce-b092-f8f6cf74be5c', 'db062e86-b522-41c2-b3b9-df461d830de8', 'nIzE966AHLfsRiHuGc4Flkt/OtqkgwHUcfelvKYdea0=', '2026-03-28 05:55:11.489966+00', '2026-03-21 05:55:11.548979+00', 'System', '2026-03-21 06:07:26.019538+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('225a641a-1493-427c-a3b2-ea98d9321ae1', 'db062e86-b522-41c2-b3b9-df461d830de8', 'rQ9rYmSQbsU/V/6crPs3M4Ct8S3MxmqP25JwEJ9OJmg=', '2026-03-28 06:07:26.018542+00', '2026-03-21 06:07:26.019524+00', 'System', '2026-03-21 06:17:54.02643+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('047cca41-a2a6-4407-938f-6ec6785fd95b', 'db062e86-b522-41c2-b3b9-df461d830de8', 'YgMkfPz6zX0HGqlzO5iNAy3tqpv75O1fZbQpoKvSkGg=', '2026-03-28 06:17:54.025921+00', '2026-03-21 06:17:54.02643+00', 'System', '2026-03-21 06:30:36.55613+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('5521fa1e-1410-4b1a-8ade-f0573fa8cf17', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'CrTIToI6Yr8YCKZqMBjjBCOdOTI4PUUKnf40tBuzDJo=', '2026-03-28 10:07:58.09995+00', '2026-03-21 10:07:58.185562+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('b3f9a2a5-db24-4bc2-a4ed-40b293325f85', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '6MvpjLjAPhTaRrPhc6dH2Rd1yEenkfajYILRgH30Hd4=', '2026-03-28 10:18:50.348654+00', '2026-03-21 10:18:50.471934+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('a6a815c5-4c48-4188-8d9e-9a6210ea8de7', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'wJVe73LoGEysX7WxM65XbEPBRlqRhqL82osndgjr0i8=', '2026-03-28 10:25:31.626154+00', '2026-03-21 10:25:31.690417+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('26096742-9926-46c6-aa2a-f33944d57e40', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'pD+1sAWS/C0mNvHYGEZxRiMSLfqkYormLo17wthetFQ=', '2026-03-28 10:55:19.363016+00', '2026-03-21 10:55:19.428085+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c24d10ab-3f65-46c8-a4d4-a7a2dff3159f', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'ccQ5/KYc1BX7vRjMqI70vKs2zf5sb6IJc7bY2CkOu7g=', '2026-03-28 10:58:01.525585+00', '2026-03-21 10:58:01.739913+00', 'System', '2026-03-21 11:08:02.598013+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('09e8c306-5e1e-4bc4-a191-fd2a097f8e61', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'gPXMjvAZI+8iAfWuxkBpsSI5HAroXeUcrVyWiWcbFpk=', '2026-03-28 11:08:02.596293+00', '2026-03-21 11:08:02.597996+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('364845fe-dec4-4bbe-90b3-80667291ee05', 'db062e86-b522-41c2-b3b9-df461d830de8', 'aeOnDtV/ycG7kKxUwTVrtRNfZJt2X4Uwejv/EPAd8eg=', '2026-03-28 06:30:36.555693+00', '2026-03-21 06:30:36.55613+00', 'System', '2026-03-21 14:30:14.843607+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('3b2cf8f4-1020-4e3e-8047-87b45ece7f69', 'db062e86-b522-41c2-b3b9-df461d830de8', 'PRTdi0xSojWOY9NuryvrQ3Expq36Lo4HeFRbZsdwDD8=', '2026-03-28 14:30:14.842381+00', '2026-03-21 14:30:14.843599+00', 'System', '2026-03-21 14:56:39.926597+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('eada4547-4638-41c6-816b-62c10f3e457f', 'db062e86-b522-41c2-b3b9-df461d830de8', 'Fwvu3tkLRzFYofoUG2jtfvG0dzsPBKZSDiiNMx1ZisE=', '2026-03-28 14:56:39.925977+00', '2026-03-21 14:56:39.926597+00', 'System', '2026-03-21 15:18:24.941243+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('705f54d8-da2c-42ec-88cf-6433be2e46bc', 'db062e86-b522-41c2-b3b9-df461d830de8', 'KB3rLClg6OKHVljasyZx0LnTSkBzQd8QtbbDvqZeOBQ=', '2026-03-28 15:18:24.900737+00', '2026-03-21 15:18:24.941177+00', 'System', '2026-03-21 15:28:43.81948+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('7940c30b-876a-4026-a582-c7e401f85789', 'db062e86-b522-41c2-b3b9-df461d830de8', 'GpCXFXmU9ishW+iVKP/P6JESFXZmA4O6CeyfBY1U68k=', '2026-03-28 15:28:43.805064+00', '2026-03-21 15:28:43.819471+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('cacb8f54-04f7-4841-bf3d-759700685422', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'xarZNsi+i4ClK1zoVvoxZUr/WvBQl+Abpio45GPqCLA=', '2026-03-28 15:37:03.438602+00', '2026-03-21 15:37:03.539348+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('b12716da-fdf2-40db-838a-e1945ba3ca83', '70000000-0000-0000-0000-000000000001', '6vqbwYLIgEqtuG9V3P/kUJytSskGf2M2W65pmaMzYSA=', '2026-03-28 15:41:04.302153+00', '2026-03-21 15:41:04.307293+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('2aac33d4-881d-4829-ac1a-988f2602b66e', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'DmL6UGf/HVwOgMdzMBfNMfLnnKqo059o+tf090YWkbE=', '2026-03-28 15:42:18.593058+00', '2026-03-21 15:42:18.593663+00', 'System', '2026-03-21 15:52:44.996338+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('63a9121e-2ffa-42b6-a407-b361641f32fd', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'ldpWBFw1f4uhisrsbEv82pQt5wTETn1TFiW8ERhSxF0=', '2026-03-28 15:52:44.920236+00', '2026-03-21 15:52:44.996219+00', 'System', '2026-03-21 16:03:38.274445+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('9bc24e04-fdfc-4d91-a9fe-56b80141f464', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '012E19L3r0QLU4kFQy57QOAoE5U5hPuuifoW0UUKb5o=', '2026-03-28 16:03:38.273888+00', '2026-03-21 16:03:38.274416+00', 'System', '2026-03-21 16:14:29.452164+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d0830e75-4104-4ed1-9482-261212b74658', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'cPsuxdpFDALFDmYAC1ruXQB3ddgFfaTWnAui8hnj9Fc=', '2026-03-28 16:14:29.451238+00', '2026-03-21 16:14:29.452163+00', 'System', '2026-03-21 16:35:13.678416+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d0ce29b8-4103-4f23-ab08-66eae55df2aa', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Y0ORyNjQhd29ditD0pXR4AkIRCpMgRpUNUNTr17CQK8=', '2026-03-28 16:35:13.678135+00', '2026-03-21 16:35:13.678415+00', 'System', '2026-03-21 16:49:19.367001+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('02925389-763f-4816-b0e4-145c2a4b7221', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'pClLeR1kzyLcaeiRImH42hIRspv81gDiDd/MLyEd1sU=', '2026-03-28 16:49:19.366507+00', '2026-03-21 16:49:19.367+00', 'System', '2026-03-21 17:27:15.663964+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('64466982-f326-434c-9a4a-b7c1cc498458', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'D64C4kLLvpV23zDnRf1XesAGrlnoOCjrZGtAul/O7Hw=', '2026-03-28 17:27:15.586458+00', '2026-03-21 17:27:15.663839+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('892a84af-134f-4aca-926a-c28630367edb', '70000000-0000-0000-0000-000000000001', 'JyJk4owz8wd9hfgHuQ0VE2YU0Xymf3uLHrHzqiTyCmI=', '2026-03-28 17:33:45.262056+00', '2026-03-21 17:33:45.26658+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('0dbadfc4-8229-45e5-ab1e-54c72801bc1b', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'LviedznZeaeBTKwmN4w9w4GCWGWi//RZDPgo/SrqmWk=', '2026-03-28 17:34:41.502833+00', '2026-03-21 17:34:41.503672+00', 'System', '2026-03-21 17:47:22.398357+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('5dcfa6d2-1789-49a0-8980-cba9437f4361', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'RZj0dzvCnIUbROnMuAtS4Zp/9wgG7Ug0e+bat4tKgcg=', '2026-03-28 17:47:22.397942+00', '2026-03-21 17:47:22.398356+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('1efc5590-7411-4e00-901a-1fe0adc4c2ab', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '/BLaMPjz4hnWMudEVkRCkW3yS1+eql4ZZBtu3bfqZkI=', '2026-03-29 08:36:58.122003+00', '2026-03-22 08:36:58.235614+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c76f787f-2a9e-4d36-961b-489c7877e118', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '/tLPn8mGD8ZxNLlktFtnHN5xyfbzczVbvEO+ZTtyIII=', '2026-03-29 08:45:25.557145+00', '2026-03-22 08:45:25.658163+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('44ea3b16-293f-4977-b928-6cd81d9d612d', '70000000-0000-0000-0000-000000000001', 'xc6WFxZz0unnDa0OO3yKhSuB2EkhJITOW+M5ErYLmSA=', '2026-03-29 08:52:52.195445+00', '2026-03-22 08:52:52.1999+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('1cbf346e-7401-4acb-8817-fba688da2c34', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Qwf610MQY6VFLK6FnhJnxC5LZsDPIFV5llZ7XLvfVV8=', '2026-03-29 08:53:07.524445+00', '2026-03-22 08:53:07.525055+00', 'System', '2026-03-22 09:12:48.158406+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('188ed339-3fd8-4e8e-afad-ae5b66e25f3d', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'q4y2rOQ2EaG2WP+bx9dNWYjOEAppZ5B/Q387vVwE5aA=', '2026-03-29 09:12:48.157512+00', '2026-03-22 09:12:48.158405+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('07b2fd64-d706-4dd2-ae21-391561c24792', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'SYEVpVFKNRq0+vkV+cO288tRFwh6HqL8omO78swhVuo=', '2026-03-29 09:15:53.066673+00', '2026-03-22 09:15:53.067035+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('d47f2885-2680-4e6f-bdcd-ca5d413bf489', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'cW3xj/F9HgOysW1ukp2PDJHFgNDbNNLvyrYK9NOnW2U=', '2026-03-29 09:25:41.084974+00', '2026-03-22 09:25:41.206152+00', 'System', '2026-03-22 09:38:04.810668+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('40704b00-ae25-42d9-abb7-3bd2d53df241', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '2e0zKx/qKuqTM0PHGIS60qICykUA72E3S8W8znvlXnI=', '2026-03-29 09:38:04.809223+00', '2026-03-22 09:38:04.810638+00', 'System', '2026-03-22 09:53:04.911042+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('f10acce0-2b19-4d71-bc7b-5f7b258117c5', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'ASY5zr3ajZLqDqHl9NREzTpURMUHfytR7Si/yj26+P8=', '2026-03-29 09:53:04.823965+00', '2026-03-22 09:53:04.91091+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('74119f8e-2891-4429-bd0c-dad3e2ad72ce', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Pmq4VVff5ePh70iDUPiwyniAphEEiFUsCQIet9/WnBU=', '2026-03-29 09:53:36.019831+00', '2026-03-22 09:53:36.021114+00', 'System', '2026-03-22 10:14:23.5318+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('c77fc2a2-872f-4f00-83b1-766a439da6f7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'g8fyGc1MWZI+cKar3/PPdTGL52RQ04hdqAUOTM9QLeg=', '2026-03-29 10:14:23.531307+00', '2026-03-22 10:14:23.531799+00', 'System', '2026-03-22 11:06:35.540319+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('fd3e3937-66b7-4b70-8c91-7ce604173180', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NuTPQ9OpL3RTMIi3HHxYKV+b+VP/DWgCiG0lFZpXYRk=', '2026-03-29 11:06:35.461693+00', '2026-03-22 11:06:35.54017+00', 'System', '2026-03-22 11:23:09.079309+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('2a4c2525-766e-4177-a148-0183df29fd98', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Fm9zm5NMkV8tPm87Iu7wWqV27d89ftbPl8pXl7d/Y/o=', '2026-03-29 11:23:09.078669+00', '2026-03-22 11:23:09.079288+00', 'System', '2026-03-22 11:36:01.571215+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('5ea9003c-daa3-4b6d-882e-a8a4dea81b2b', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'fDWI6qxW5wo+vzFRd8D+627EhQ0sG0IHXI75xGIVA6I=', '2026-03-29 11:36:01.473183+00', '2026-03-22 11:36:01.571063+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('fba54c7a-91a3-4c82-9822-109c849aa813', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'xfKL6+s4HiNKqR1O5PJUZPUQvbrE98SXm59bYjRakYw=', '2026-03-30 06:33:06.587964+00', '2026-03-23 06:33:06.642417+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('5c1309d1-3b02-4030-aaed-16dacf3854f6', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'zxtIMrmPhyXxq31i7D3jkAW6dKjaEoi8pI1e28Fe61c=', '2026-03-30 09:21:24.095881+00', '2026-03-23 09:21:24.170922+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('13213f2a-85f6-4f0b-8817-e29f6a61e5b8', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'lDbIkz/eblkhaFIlYf1Uz5RVtpCg9uWmteM+2b05PDc=', '2026-03-30 09:25:56.700394+00', '2026-03-23 09:25:56.70148+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('1d26175d-f31b-4995-b5ad-f21c34f8a751', '70000000-0000-0000-0000-000000000002', '9FyqS22+fYz5Tq1/qusiq12iH5hX8HkSHGIsKvPELVU=', '2026-03-30 09:26:29.629547+00', '2026-03-23 09:26:29.631612+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('a259edff-c2ab-4330-a963-e3f5d68e4f52', '70000000-0000-0000-0000-000000000001', '3AuzTMWEK5+08kwHaPC4MsJRVlPQqB0qZ0N26MU30y8=', '2026-03-30 09:27:45.393776+00', '2026-03-23 09:27:45.394609+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9ab14186-f43a-4732-ad75-802e206f6038', '741eb167-fb61-4b47-a35a-90720c4949f7', 'zPpRJdYACnmHLXdxU6U5tdCGUbqq8YQcv+fDQuA6UCA=', '2026-03-30 09:28:37.063186+00', '2026-03-23 09:28:37.063684+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('8d4c45c8-d9c0-47a4-b51d-113904e9350c', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'FycXM7PXHvC8cpMUWCo+2dYXfeul0L9RxsfgT44OcOs=', '2026-03-30 09:50:12.571637+00', '2026-03-23 09:50:12.619115+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c7771e42-d407-472f-8cc7-166a45c5538a', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Uq7ZZblqfCLKbsETO3DP++X72c2fmJlQO4n1ZvZME2Q=', '2026-03-30 09:30:16.202966+00', '2026-03-23 09:30:16.203548+00', 'System', '2026-03-23 10:02:25.477815+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('a95f6ccd-340f-4634-8b42-1d417310b060', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'M9yOH2M5r5Ngfgi6mKjSpjlnf1oWPUo84FcGquGTDeo=', '2026-03-30 10:02:25.47708+00', '2026-03-23 10:02:25.477814+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('452aca70-12c9-46d0-b6a8-1e292ce1b420', '70000000-0000-0000-0000-000000000001', 'N7J2JtfNVMZg7JfghAfieb6E3VNurY/yq9q4jlB/srg=', '2026-03-30 10:02:44.557037+00', '2026-03-23 10:02:44.557731+00', 'System', '2026-03-23 10:13:58.043273+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0a7696ed-9fd1-4eaa-ba48-1f4a6d7dc952', '70000000-0000-0000-0000-000000000001', 'aLYBIDnnoAdXAknVZA6KGjOaVg6e1Q+j19pj6/RODiM=', '2026-03-30 10:13:58.042801+00', '2026-03-23 10:13:58.043272+00', 'System', '2026-03-23 10:20:15.105482+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('198bcf1a-bee3-436b-9b70-f84917e2c3d1', '70000000-0000-0000-0000-000000000001', 'DvepVuD41wq36riudYThE0csiaJ9ydEPqsf5ym16LLU=', '2026-03-30 10:20:15.105163+00', '2026-03-23 10:20:15.105482+00', 'System', '2026-03-23 10:20:15.873873+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('224451d8-ed57-4270-a63b-7f472ac30b76', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'WxdqwAgD08kQOq5nm6woz1qvElPuguad+fSC5E6AZsY=', '2026-03-30 10:52:03.628497+00', '2026-03-23 10:52:03.73451+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('19afb4f1-ad3f-4a2b-8c9b-f319c1fae324', '70000000-0000-0000-0000-000000000003', '8qEQ/5sghAlRxumgEuhKg7DieXz4hpOvl0Ize/mYiuY=', '2026-03-30 10:38:15.585562+00', '2026-03-23 10:38:15.586022+00', 'System', '2026-03-23 10:56:56.976917+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d5d5db12-13cf-47fb-9bcd-e0ce1cd64fb3', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'u7zTw6nKHyMcgCP37gmU2Y1kjL5tuRNgtoQox0DrZuw=', '2026-03-30 10:33:11.883433+00', '2026-03-23 10:33:11.929676+00', 'System', '2026-03-23 11:01:19.488949+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('f6ce4c89-29da-4043-8021-6baefa174a6b', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'fYLfnDsGwkYF+IkwTLsXhHbP2M1MjnWmOQDwjc7ShOE=', '2026-03-30 11:01:19.448529+00', '2026-03-23 11:01:19.488892+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('e3e558e5-c098-4395-91be-76bb29a1aa84', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'wFr2pwYQlmeqdiIRUNto3rMFgoRsOzvBRvXrngWVKrQ=', '2026-03-30 11:02:03.820449+00', '2026-03-23 11:02:03.822049+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('bfc2bad3-59cc-4fff-b933-60896c956606', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '+SUWLHi4hk5OwQchy0WQ3zJFYAuqV39wipTD8ri7xWA=', '2026-03-30 11:04:24.322026+00', '2026-03-23 11:04:24.322716+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('fbe05322-5376-491c-a833-619d44d138ba', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'kUQ6amGrGyM59zNk+LoPeR/qb0nVVCo6M6rxExyyzeI=', '2026-03-30 11:15:26.846539+00', '2026-03-23 11:15:26.959118+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('5b691399-750f-446f-9c76-940e9e9b719a', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'Xqkustz0rb2yC0fcNeDVhPdKzjmZHHCyIIMYOCMCroU=', '2026-03-30 12:02:40.35485+00', '2026-03-23 12:02:40.355778+00', 'System', '2026-03-23 12:15:42.850785+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('364f78b0-8536-4687-88f6-10483636dbf3', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'q6WrchbHTzL1o9FMqztKs5wqlYFOaJo4oJi89uPfrso=', '2026-03-30 12:15:42.833631+00', '2026-03-23 12:15:42.850727+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c49f75fe-d941-4df0-b319-0786260dde7a', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'DWeun8M3JwrZJfzG7Cny9aE8ysCNL015kAaATt9Uoi0=', '2026-03-30 10:06:35.43753+00', '2026-03-23 10:06:35.482728+00', 'System', '2026-03-23 12:23:02.853854+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('72529784-8118-42e4-9870-297c3b424e2c', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '8JV2wY6VFl/efXRVBz5iaCN3scFAAMxnaIHaQ9U1oXw=', '2026-03-30 12:23:02.852874+00', '2026-03-23 12:23:02.853843+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('11db3451-96e4-4613-bd63-1cee9bc2d72b', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'uUeKcQb8bofZO7CdEfI5FZzUtEW2qTdDC+FZm6fNqfo=', '2026-03-30 12:31:16.209245+00', '2026-03-23 12:31:16.268738+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('d1b81b15-3e2c-47d6-8f9e-65718867dc34', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '0PI3G1xEnduOCxQTephwrHwKnbcVeVO+kEt80a7m8SU=', '2026-03-30 12:37:14.613862+00', '2026-03-23 12:37:14.680147+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('08164292-06ea-4c52-9f5b-5a2b80d8ae65', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'l7LUolNuc/6mkcH6Sth7+tItX1ab8AExTzpnqJ/+OO8=', '2026-03-30 12:44:54.637608+00', '2026-03-23 12:44:54.638918+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9d0c5e6f-0a57-44a9-a2d4-1a0883fcb15e', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'NJO6Ag2PjOrklg0r6Pxbs/BXOnOU7n8AaM9mt3vRS2U=', '2026-03-30 13:19:44.582319+00', '2026-03-23 13:19:44.661552+00', 'System', '2026-03-23 13:36:16.212443+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('afbd7fd2-07cf-4248-8778-69d291634a0c', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '9m/owXrZNBi1T3EMLS47MDOFDzuhyJSpIbRJwMacwVI=', '2026-03-30 13:45:57.105134+00', '2026-03-23 13:45:57.169195+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('d70d1d49-a220-4320-b07c-747b7bb0d57f', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'j0wR2exrR+28MFsHM4zkQJgMSRTNwNQkY0fJ3078XT4=', '2026-03-30 14:03:40.668613+00', '2026-03-23 14:03:40.774278+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('84cb55ea-1239-48db-88e4-b4827c468992', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '1Rc+kZqUibtcJmglqI2jRXBhTEWFzEaa8lYUevHalH8=', '2026-03-30 14:19:14.669686+00', '2026-03-23 14:19:14.772307+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('7470e7e3-4dad-44f3-837d-b2df80b691d9', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'hfPOXB1xXCajE9MWpvKYFeP19L0/dbbJbHb9QskTxTg=', '2026-03-30 13:36:16.149465+00', '2026-03-23 13:36:16.212308+00', 'System', '2026-03-23 14:27:13.689839+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('f53895c3-e658-401c-b340-e79565cbac07', '70000000-0000-0000-0000-000000000001', 'MuLicfPXYsfv3FZykQAS2NAYYhRC/IbQPYdMNXhfAPM=', '2026-03-30 14:20:23.142266+00', '2026-03-23 14:20:23.146896+00', 'System', '2026-03-23 14:31:47.462372+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('c8d4eff8-3989-4340-8ace-612d04db5920', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'eeDrZ07hnFCi5yh89PWWpMcGNC/dNPsDyE9gIda/LbM=', '2026-03-30 14:27:13.623927+00', '2026-03-23 14:27:13.689704+00', 'System', '2026-03-23 14:51:50.89622+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('6b9961a1-60c2-4583-bad9-335b5e0057d5', '70000000-0000-0000-0000-000000000003', '+Br9+VqHTyeWpO34eG5/USRNBRFeIjKwAuKulMTP4O4=', '2026-03-30 10:56:56.97662+00', '2026-03-23 10:56:56.976916+00', 'System', '2026-03-23 15:57:57.73882+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('603a26fd-10aa-4e0d-a8f5-d837cbdf1ae9', '70000000-0000-0000-0000-000000000001', 'VEfmhAoHGMljEJ2yPrna9jnznvjsxewDrppalPb/zmM=', '2026-03-30 10:20:15.873349+00', '2026-03-23 10:20:15.873873+00', 'System', '2026-03-23 15:58:04.285618+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('23dcea67-11be-44e4-af05-029e1b6022d2', '70000000-0000-0000-0000-000000000001', 'p5+tdU+I7LyXAkotuMw2psfye8DCYJKzIC+1eZmpx+g=', '2026-03-30 14:31:47.460888+00', '2026-03-23 14:31:47.462372+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('5234d8c4-ad4c-43ee-8274-ac4a3e7f1b81', '70000000-0000-0000-0000-000000000001', 'BNo3RW1jtuLIUsTClyOwZrJM8u24IOx7WXU+qhS8lq0=', '2026-03-30 14:31:53.498409+00', '2026-03-23 14:31:53.499452+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('2477ebd6-dea8-4a66-9cd2-58a1e1dd6b78', '70000000-0000-0000-0000-000000000001', 'U+LwaSBtruYqnyVrx0DwT51brBopgJ+RC7BCvuBa5L4=', '2026-03-30 14:35:47.635404+00', '2026-03-23 14:35:47.636453+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('7873e93e-5fa7-4125-a3e9-0cd58ea5be27', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'A0qARpZLgN4j0OzgEs8K343RJdZanzdkcL0sxBDgl5s=', '2026-03-30 14:36:00.949717+00', '2026-03-23 14:36:00.950092+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('1ea9dff1-0e8f-43a3-b7b7-4f336d9a2ae4', '70000000-0000-0000-0000-000000000001', 'UqF+b/Pd0foBVIaZODlcLgjR9mST9g5QQFmk853SgBs=', '2026-03-30 14:45:54.328323+00', '2026-03-23 14:45:54.3291+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('98bc9344-a71b-4768-88a8-f72228163745', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'hZPNL2mITgb+CFLQfEbZ+LHkEtkZQmsmmPWnJ2iR3YA=', '2026-03-30 14:46:18.82858+00', '2026-03-23 14:46:18.82906+00', 'System', '2026-03-23 14:59:10.03199+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('91f960de-e671-466d-b95f-fbdc5469806f', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '6FRSSgcHXxKjMQfB8P2oqIw4stG7dJYFSEv/rU/9TWI=', '2026-03-30 14:51:50.843257+00', '2026-03-23 14:51:50.896085+00', 'System', '2026-03-23 15:04:56.483696+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('e97a3680-39bd-407e-861a-34d078a831fe', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'M3gQmIaZ4V5iSkQ4PQrC9OWLKkqH12ZAUwEtGZlMx8g=', '2026-03-30 15:04:56.482522+00', '2026-03-23 15:04:56.483685+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f1e362fe-1f8d-4782-9295-cf255d0fe718', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'VUTqx4yo5kZHj1c4jKIWWJW7k5IYqBJFT4/Ha7pkLAg=', '2026-03-30 14:52:39.430125+00', '2026-03-23 14:52:39.53691+00', 'System', '2026-03-23 15:07:49.607577+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('a94667d3-06f6-48dc-bba9-e38ea832eb5e', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'RJFSRzoNa3MMFZsmv+ZpE3hPzichpiXwbcLGc6R7ad0=', '2026-03-30 14:59:10.031443+00', '2026-03-23 14:59:10.031989+00', 'System', '2026-03-23 15:15:09.185846+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('7c949724-f159-468f-af17-7038e54f5660', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '6XupSpu3VOhA6ryeKwe8DPSfe8onBlwJiVlqeUdLUoU=', '2026-03-30 15:26:15.199732+00', '2026-03-23 15:26:15.266613+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('677a1b0d-3d66-485d-9c55-b6a33fda2246', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'sWZ1BRa+qzn7y/If0IJtUR9MRCOzVg3QnUYJR2hJMKA=', '2026-03-30 15:15:09.183783+00', '2026-03-23 15:15:09.185845+00', 'System', '2026-03-23 15:26:17.059715+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('7923303c-a66c-4741-9b0e-5c516fe6ff6f', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'CvgoHNbWD7pMlygK9XWCScFfs/+As2HLL02rEq28yOM=', '2026-03-30 15:37:07.304778+00', '2026-03-23 15:37:07.367363+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9ec2f401-5735-439d-a6f9-ecefa26571ba', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'UJcqDxnlpmPxOBSN1GqME7qYPZvImn+cErGs80gXlmc=', '2026-03-30 15:07:49.547132+00', '2026-03-23 15:07:49.607452+00', 'System', '2026-03-23 15:39:40.450731+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d689bd66-e4bf-42a2-a318-0739277689d5', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '//qaxOahMFTFDUhcUduefv04GEs8BgATpLmfKJNstnI=', '2026-03-30 15:39:40.386533+00', '2026-03-23 15:39:40.450574+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('5b4f9788-dd3e-4231-bd56-084e37434469', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'xpSOh1r01fIpwFrCf7qRdaL5B2MjT/VsZJ/lqWDuos8=', '2026-03-30 15:26:17.057714+00', '2026-03-23 15:26:17.059713+00', 'System', '2026-03-23 15:55:22.054073+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('8c9aed09-18eb-421a-9a47-fe55f5b43e56', '70000000-0000-0000-0000-000000000001', 'TzutJ7pOlUxRJDYPU02iAGkfQlUnMSeZCFiQweIql/Q=', '2026-03-30 15:58:04.284727+00', '2026-03-23 15:58:04.285616+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('08031889-fb6d-4c99-bcd7-e9c945e46c32', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'b9H8LPBNr/Fhr2IgH0LF1Dy9jiaWm5ynK8iFiRe+3nk=', '2026-03-30 16:07:17.286414+00', '2026-03-23 16:07:17.381508+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('55e5072c-4e62-45d9-a414-1667d289cffd', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '5SWTnNdnTLQ8y5iSFQ3OLBak3meqwq287QTWQMGY5Fo=', '2026-03-30 16:03:13.427741+00', '2026-03-23 16:03:13.502418+00', 'System', '2026-03-23 16:14:44.099305+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('169d4f8d-c9b5-474a-ac06-3ffbc27ed14d', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'v2j16uC20s5mFKLQwBr6uxS2MSSQuXcL2Tvp27Su98o=', '2026-03-30 15:55:22.053417+00', '2026-03-23 15:55:22.054071+00', 'System', '2026-03-23 16:17:44.116093+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0f0d36a0-4e77-47fa-9949-7d400e38180b', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'mCsMSqLTQ5gAQdkFz8vyJYuf7nYaw8eckYpmXuDTFH8=', '2026-03-30 16:14:44.097315+00', '2026-03-23 16:14:44.099282+00', 'System', '2026-03-23 16:24:59.887043+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('a2f43efe-7736-417c-a8f8-0df072b6951a', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2XLZgUzUThAUmnq3sawvn1Q6goWw+f0uY7gcUsr7DoA=', '2026-03-30 16:24:59.886716+00', '2026-03-23 16:24:59.887042+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('29ac2043-71e1-4a7b-8cb4-a029657909e1', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '1zMAQ/1UI2AStGMepI3txHCPR+4wuk9fU6JeHvLkqO0=', '2026-03-30 16:17:44.115832+00', '2026-03-23 16:17:44.116093+00', 'System', '2026-03-23 16:29:34.28807+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('94aea84c-2a98-494d-8060-946e10957b1e', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'nIA5TOI9B0cBw6SQBI2A7NShH2F5uiXAwF2Bjgy3F7Q=', '2026-03-30 16:29:34.287685+00', '2026-03-23 16:29:34.288069+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('e02ee990-ad03-4149-9793-6d31e414b4c3', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '2Gm89zj3HLyjLnpwl0veayvjFSNX7j2XIx8hwJqW5lY=', '2026-03-30 16:23:22.943945+00', '2026-03-23 16:23:23.039592+00', 'System', '2026-03-23 16:33:27.722385+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('32bffbfb-5a57-4cdb-a1dc-7887fdf88376', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '92fsAvN8fRq0dZCjqJ12vhdZcNqhs61aJcMQhOlPmxU=', '2026-03-30 16:33:27.713714+00', '2026-03-23 16:33:27.722211+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('8ea26704-934f-42dc-85c0-4564eb67a090', '70000000-0000-0000-0000-000000000003', '/bWQpurvvIAjjotU8gKjwLL9yNpXtA5rCMkLerrxUjE=', '2026-03-30 15:57:57.737668+00', '2026-03-23 15:57:57.73882+00', 'System', '2026-03-23 16:33:31.478501+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('58e3cb32-cb40-4f4d-be40-f0ff5d2cf3e6', '70000000-0000-0000-0000-000000000002', 'be9IjqzdxtngwiEYUJ9VtLyZ4Ycdpu068D3m4Kymr60=', '2026-03-30 16:32:36.054057+00', '2026-03-23 16:32:36.116376+00', 'System', '2026-03-23 16:46:24.261219+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('5d842edb-ed80-4649-b5d0-c90d8bc943d3', '70000000-0000-0000-0000-000000000002', 'SYChz86Y0xj+P3CV2hYhH4ZZQUkIPlZLmQ3YT4omUa8=', '2026-03-30 16:46:24.259878+00', '2026-03-23 16:46:24.261218+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('a560864b-dd80-46b3-90b1-6b3ae2b6c2bd', '70000000-0000-0000-0000-000000000003', 'F/VFnhrczmnMa7yDSgcKZd1kuttcc9Uekpd9xrNpur0=', '2026-03-30 16:33:31.477333+00', '2026-03-23 16:33:31.478489+00', 'System', '2026-03-23 16:47:22.17201+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('8d96d947-a11f-4533-afa2-d0157f8f7364', '70000000-0000-0000-0000-000000000003', 'Cx1a4jypcyBa5N9ZPtcq8rUcQbDlKB0IlP2r2HK8t1A=', '2026-03-30 16:47:22.169374+00', '2026-03-23 16:47:22.172008+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('ab562c83-91a4-4542-a75b-c1948a747ffd', '70000000-0000-0000-0000-000000000001', 'f8T+Cd4Eg7EWogD2pXaFDbi7gfboyHcd2FREKuJnPxE=', '2026-03-30 16:57:42.621134+00', '2026-03-23 16:57:42.830737+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('4e3f962b-4d45-4917-b317-aa3703699dbc', '70000000-0000-0000-0000-000000000001', 'wmLvaOgQbpGXRal+PVhFK2wRHL4Qcm/4siL9/k/vFJo=', '2026-03-30 16:46:46.550141+00', '2026-03-23 16:46:46.550652+00', 'System', '2026-03-23 17:02:25.799736+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('3ce07b23-3015-4092-bdd7-a5f05f7f2d97', '70000000-0000-0000-0000-000000000003', 'is3PXZaXCJcos+7OPHK8q9rGUxQQ3V/6JIEdBExKFuc=', '2026-03-30 16:47:39.734161+00', '2026-03-23 16:47:39.7347+00', 'System', '2026-03-23 17:02:25.941907+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0a24a306-4ea7-4056-acac-bf6ec3cb7cf2', '70000000-0000-0000-0000-000000000001', 'eYYes5R3lMEC17D8Rv1hzmsR745gEHA6vwtXcjpocRQ=', '2026-03-30 16:59:08.345351+00', '2026-03-23 16:59:08.570517+00', 'System', '2026-03-23 17:10:48.982286+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('51fb556e-bda6-4b90-a7c0-e96727b84fa5', '70000000-0000-0000-0000-000000000001', 'zxSdet0i7fYCVcsn71zS58xfUUk6cUjpoXw9ssdVw/k=', '2026-03-30 17:10:48.979024+00', '2026-03-23 17:10:48.982256+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('773c94c6-47d7-4a95-828b-96437c59e6a0', '70000000-0000-0000-0000-000000000001', 'FqF0jrbZoZX9Qho4WTw80SXYOBHpNOQVfLH6begaXoc=', '2026-03-30 17:02:25.799365+00', '2026-03-23 17:02:25.799736+00', 'System', '2026-03-23 17:14:25.961211+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('66d1f686-0bb3-4594-b08a-2c1982f35df9', '70000000-0000-0000-0000-000000000001', 'ViDb7xyRL3Us8a9u5YKfdY0ZYIwgMDbI0xXMj16vsVE=', '2026-03-30 17:14:25.960479+00', '2026-03-23 17:14:25.96121+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('880056e8-9170-432b-a198-f94a7740842b', '70000000-0000-0000-0000-000000000001', '1jahiTF4r8TJ1S4XutIKiwICLaJYJtl5C20nBKNN518=', '2026-03-30 17:14:52.867354+00', '2026-03-23 17:14:52.867877+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('cd0b9eb9-db4e-477d-8f2a-7e7ea3fc6455', '70000000-0000-0000-0000-000000000001', 'Ixa/kIYrI/iod2qADCOddLE2BRTRvLPRlcONKQ+QxUQ=', '2026-03-30 17:13:24.113982+00', '2026-03-23 17:13:24.119381+00', 'System', '2026-03-23 17:30:39.995175+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('b17743c3-a96f-4472-be02-d43a89b45cda', '70000000-0000-0000-0000-000000000001', 'dN+KA6pSWivXzL2yTKqRA8gqOhVp4PVCpAuTs9G7HjE=', '2026-03-30 17:30:39.883931+00', '2026-03-23 17:30:39.994928+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('df120ae4-0e24-412f-a939-c263eb07df72', '70000000-0000-0000-0000-000000000002', 'c7ardqTKMb/88YYyp7FKmqVp2CbUfqK2zGRrA00FAH4=', '2026-03-30 17:15:26.912032+00', '2026-03-23 17:15:26.912469+00', 'System', '2026-03-23 17:35:42.09702+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('5a927f25-0b21-47c8-a98c-18333537127e', '70000000-0000-0000-0000-000000000002', '9yg9Myc8cYqWLv8uytK09PhwD5Tx3mtS67Dw93N9/3o=', '2026-03-30 17:35:42.051898+00', '2026-03-23 17:35:42.096944+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c39fc54f-dcc7-4e85-bfab-b9c93a39395f', '70000000-0000-0000-0000-000000000003', 'EU/ArpYrFPwZTa9jf3txKwzJiTlar/p9Ul9xtNB8PZ8=', '2026-03-30 17:02:25.941541+00', '2026-03-23 17:02:25.941907+00', 'System', '2026-03-23 17:37:22.110414+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('ad475d48-1ef3-4a60-821e-cfac49cbf45e', '70000000-0000-0000-0000-000000000003', 'QOWiDmGf8MvLhA4riw4R8cqjTav6HrpiD/iVfl+uIZw=', '2026-03-30 17:37:22.106765+00', '2026-03-23 17:37:22.110413+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('bd85522d-aff6-4875-8433-3e1703556bc2', '70000000-0000-0000-0000-000000000002', 'Uti23p/ZT+EkHNkfivAyGD5xyGpR5Yg526LbA+NJ0dk=', '2026-03-30 17:37:44.191587+00', '2026-03-23 17:37:44.192361+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('05340994-9b40-4382-9293-55a2e902ad93', '70000000-0000-0000-0000-000000000001', 'p6PkG7/P0QgeRngc8Yin26lTVA27dLyMhQ6xLx8A7tw=', '2026-03-30 17:35:57.931176+00', '2026-03-23 17:35:57.932722+00', 'System', '2026-03-23 17:47:58.467853+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('32779bc1-60f6-4d62-a02f-675d5585f605', '70000000-0000-0000-0000-000000000001', 'Nt9sw+q9Fm6NJ2Kqb7bpNNYFvH40kiYxam+JmraF+qU=', '2026-03-30 17:47:58.411322+00', '2026-03-23 17:47:58.467767+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('6d0be404-fd8a-49f8-8732-0b5c72ef5a86', '70000000-0000-0000-0000-000000000003', 'ioPTDhgJK+++eYDXYaNEMP5IE/TgYf2Djwhp4/wPor0=', '2026-03-30 17:38:04.868697+00', '2026-03-23 17:38:04.869306+00', 'System', '2026-03-23 17:48:38.218122+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('5b4a223c-89ff-44bd-9670-be422b707dc3', '70000000-0000-0000-0000-000000000003', '3e/6F8AyndVo1bydZSxqEkP79Cf74uFJfYk3xh95nhs=', '2026-03-30 17:48:38.216887+00', '2026-03-23 17:48:38.218122+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('3cf83922-add7-4d19-a1ea-d172d0dc0c29', '70000000-0000-0000-0000-000000000002', 't08Elw8n1SU2/HWFd/WtsIeFrH+h2m3cow3BuTR+zkU=', '2026-03-30 17:48:56.310547+00', '2026-03-23 17:48:56.311265+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9f533bee-4a24-4945-a443-8fa1f736cd42', '70000000-0000-0000-0000-000000000001', 'cyC+4j29k5jEEM/IpIMJ6RhOpcpwHKvbPLVSB7YHuig=', '2026-03-30 18:19:48.269914+00', '2026-03-23 18:19:48.446444+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c9feed1c-5c49-4bde-ae21-2bc0232c6e12', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '2G6sXkmZ/eoae5fds0a12tTkxsebRlDtrlGSXXu/OrI=', '2026-03-30 18:23:10.49983+00', '2026-03-23 18:23:10.500242+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('0c303416-aa1f-4543-9eed-5f0c0eb5311c', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'Ao+Mb9GzlaBmXUqrkZbUrZfJyMT7RnCkUT7WS8hrr6o=', '2026-03-30 18:14:44.515691+00', '2026-03-23 18:14:44.700804+00', 'System', '2026-03-23 18:25:16.310886+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('6208930a-a5d9-4e97-9245-18b27bf879c9', '70000000-0000-0000-0000-000000000001', 'qavhamUnep6NnMNrH6edAlyPEj4qgbOADUtJzjYhvak=', '2026-03-30 17:48:13.326272+00', '2026-03-23 17:48:13.327595+00', 'System', '2026-03-23 21:55:19.497329+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('71a0f69b-a5ae-4fcb-acfe-46989be96ab4', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'cGcMu6LUU/ny34/+Eiyy+wa3QSxHOk8eM+p7xwR16HM=', '2026-03-30 18:25:16.310545+00', '2026-03-23 18:25:16.310885+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('beb53dd3-6b7f-489f-ab56-c4885ca1c2e0', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'OwrEkWAYAYp5gRDlTJLvWnRwawKlGv3l6A+UPYHTFto=', '2026-03-30 18:55:21.715052+00', '2026-03-23 18:55:21.742939+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('93109e2f-d360-459e-b6cc-6330cdf9e64c', '70000000-0000-0000-0000-000000000001', 'dbYp+h8hVkQkVOkPNQEzc1IhLL2zsEaVWMRwg9luhvM=', '2026-03-30 21:55:19.49654+00', '2026-03-23 21:55:19.497329+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('1916e929-7f3f-416c-b087-cde72acc1686', '70000000-0000-0000-0000-000000000001', '6WCWa3+wvjUlTi1Lzw/SaY2yUttpbBPMB2DjWHCMi0w=', '2026-03-30 21:56:14.968059+00', '2026-03-23 21:56:14.968468+00', 'System', '2026-03-23 22:08:14.233255+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d959355a-56c4-4340-afbf-aaf3cd103de8', '70000000-0000-0000-0000-000000000001', '0v0g6k3QVlyYSi7LrZW6iiBZUCugXY4NKanx85W7MrI=', '2026-03-30 22:08:14.232891+00', '2026-03-23 22:08:14.233255+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('a7dd0f69-968c-4410-b56c-82c203a91b41', '70000000-0000-0000-0000-000000000001', 'ZpdK1SJ4AVltzbQAbA2ksFMDzat/YDEu0Dq1CmIVo7U=', '2026-03-30 22:08:47.735126+00', '2026-03-23 22:08:47.735481+00', 'System', '2026-03-23 22:26:18.893399+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0b166e6f-835f-446e-95a8-436e14e92e89', '70000000-0000-0000-0000-000000000001', 'FztLnxVKZZCZZRMgSso8cKbI7msPmONswJCxWZsw2+A=', '2026-03-30 22:26:18.835208+00', '2026-03-23 22:26:18.893291+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('8a6eab5a-a386-4cf6-bbbd-dcb9c52d11d0', '70000000-0000-0000-0000-000000000003', 'J4893vkLBXLB2iCkUFfcpEDpkfCuazFMlJRBuuKx8SQ=', '2026-03-30 22:06:44.376687+00', '2026-03-23 22:06:44.377131+00', 'System', '2026-03-23 22:26:49.929987+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('2e09550a-1421-4136-9353-f0076a2c6947', '70000000-0000-0000-0000-000000000003', '8BoYviKs0+2SoA0/Qmi3onsYtu4ixUNsf4EOx7vyS5w=', '2026-03-30 22:26:49.926251+00', '2026-03-23 22:26:49.929986+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('5755034c-9b55-4f17-ac74-fbc4553a922b', '70000000-0000-0000-0000-000000000001', '3kGZ+V2FoqDA252Lm9xfOw5GxaTPj/fexCgF3gtr9Mk=', '2026-03-30 22:26:35.119205+00', '2026-03-23 22:26:35.120981+00', 'System', '2026-03-23 22:37:30.703001+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('7603f57c-b6c4-4e35-a85a-88c4e901980d', '70000000-0000-0000-0000-000000000001', 'lz+TJgq5VKoVVjd1o5obvV1oT0Cyfxgva32IDqnR0/o=', '2026-03-30 22:37:30.64158+00', '2026-03-23 22:37:30.702894+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('160796f5-ac02-4a0a-9ed9-655d466cd5a4', '70000000-0000-0000-0000-000000000001', 'GvbFydPp7QGE9im6dU+It9FL0e/PL2V6GFUxVfdvgMg=', '2026-03-30 22:38:25.234014+00', '2026-03-23 22:38:25.235077+00', 'System', '2026-03-23 22:48:33.686514+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('21e413e7-dd59-44e2-8701-8e82e39e4968', '70000000-0000-0000-0000-000000000003', '7zUKk3FD24I+lGSYl3DXzhq12K6mHzfSIvd0A2MAAO4=', '2026-03-30 22:27:13.79166+00', '2026-03-23 22:27:13.792325+00', 'System', '2026-03-23 22:50:19.726465+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0ccb8b27-0122-4673-ab67-06fac96a50af', '70000000-0000-0000-0000-000000000003', 'Iic+IdtkF9zP2lgz7EiJRmhovyn4IxYP5LJzOT/8J/4=', '2026-03-30 22:50:19.725804+00', '2026-03-23 22:50:19.726464+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('456889e7-7725-49b8-92e6-c0b98cf51877', '70000000-0000-0000-0000-000000000003', '1Ha2bxHIC2adj3FikOLCroFzUgwt2x8XmCMFexiy0Us=', '2026-03-30 22:50:48.086983+00', '2026-03-23 22:50:48.087473+00', 'System', '2026-03-23 23:06:29.001477+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('201a2bed-3fc9-4238-9498-8416be925e82', '70000000-0000-0000-0000-000000000003', 'licHDzcca5OBqPWWtRQT8f3fmZx3VeyO+96Rr2yH130=', '2026-03-30 23:06:28.947448+00', '2026-03-23 23:06:29.001411+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('54e3852d-ee24-438d-84a3-e6c5f066be17', '70000000-0000-0000-0000-000000000001', 'mnBIF7pnEgfOkGlHIOAl0SnLNEPd3UcysEaJ21Wamw0=', '2026-03-30 22:48:33.68576+00', '2026-03-23 22:48:33.686513+00', 'System', '2026-03-23 23:06:41.364794+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d2442df4-23fe-441d-aaad-cc8afc5f9479', '70000000-0000-0000-0000-000000000001', 'aZ9nQ9RdZTxxNtc2NJSnStxj//nOh/KqC5EGV3t8itU=', '2026-03-30 23:06:41.36078+00', '2026-03-23 23:06:41.364763+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('e291e9ca-c8ae-4f85-9957-d25271a71962', '70000000-0000-0000-0000-000000000001', 'OT3A96TznKVl1+UGTmASth7crS/u4A2u+GnPGfLzywE=', '2026-03-30 23:07:09.838794+00', '2026-03-23 23:07:09.839415+00', 'System', '2026-03-23 23:18:31.722926+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('15f67ed4-f6a8-4837-97bc-933904a7ca90', '70000000-0000-0000-0000-000000000001', 'pNVcPp6sfZGNMHhjasSpwzpbljPpLEPxLOpIx468sUw=', '2026-03-30 23:18:31.671888+00', '2026-03-23 23:18:31.722874+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('1e94be4a-6811-4cd1-9163-bfbd99c3bf2b', '70000000-0000-0000-0000-000000000002', 'dCOhvE11UY7V/NqLLJkDFCz3/ryfUP69Se8cGttIAhg=', '2026-03-30 23:11:49.615188+00', '2026-03-23 23:11:49.66956+00', 'System', '2026-03-23 23:23:51.320367+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('410b5892-6b79-4c8c-9ae3-527d696d7430', '70000000-0000-0000-0000-000000000002', 'hY9PNRJZvJ70aEa1NTCU7R+WF+E+buzZjkxTlwk8ovo=', '2026-03-30 23:23:51.318989+00', '2026-03-23 23:23:51.320367+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('d58e33cb-efaf-43e8-8f7f-78778dee6965', '70000000-0000-0000-0000-000000000001', 'M4rE2NfXjCxsmmELv0yP07CxGHKrbSatpVTzpMgsy6Y=', '2026-03-30 23:19:46.177497+00', '2026-03-23 23:19:46.179336+00', 'System', '2026-03-23 23:30:49.939875+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('88919bb4-28b8-428d-a0fc-f1e96b4d09b6', '70000000-0000-0000-0000-000000000001', '9ac48iZSE38/7bKahtotslNt4Bl9rr2qd+zBvuv9dRg=', '2026-03-30 23:30:49.939153+00', '2026-03-23 23:30:49.939874+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('b2250bb0-d838-4bc1-b6f8-930537f5e6e3', '70000000-0000-0000-0000-000000000001', 'M+GLhXXoODYwxysmHJL49y1rSTB/mVr6c+Kug5gC5RM=', '2026-03-30 23:31:11.56739+00', '2026-03-23 23:31:11.568003+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('cd4dcd89-2d4d-4517-acbd-dc66af8a8774', '70000000-0000-0000-0000-000000000002', 'VLhwOXYiR3Fh2Qso8/L7HFWrN5QMkeTicB+VriTCs1o=', '2026-03-30 23:25:04.021342+00', '2026-03-23 23:25:04.021772+00', 'System', '2026-03-23 23:37:44.002769+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('f873617a-54aa-4537-a6e9-464c0a9b9607', '70000000-0000-0000-0000-000000000002', 'E9/YrMEfW2jvsDg3qdtsjsSWKMfTgYYRsqJoN8b7F80=', '2026-03-30 23:37:43.93418+00', '2026-03-23 23:37:44.002632+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('83368a4a-77c5-48eb-a40d-38f63340151d', '70000000-0000-0000-0000-000000000001', 'pGyE5eUo9D7Z84y5CeeDKjmnrqmx+up3nFAetXy6w9k=', '2026-03-30 23:34:35.659929+00', '2026-03-23 23:34:35.660521+00', 'System', '2026-03-23 23:44:52.648479+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('5542f84e-3353-418b-ae6b-ae9a846d74e7', '70000000-0000-0000-0000-000000000001', 'ZGIrjSuyvkKMBFOd0mW7IOH2VB6gzYJmXyt3DI9xFKk=', '2026-03-30 23:44:52.602953+00', '2026-03-23 23:44:52.648404+00', 'System', '2026-03-23 23:55:50.648816+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('9a0cb98b-431c-4621-a6ea-05568777d908', '70000000-0000-0000-0000-000000000001', 'xj2s+2Y8EN8O76b4q0dUBVreRdO+iXWbDE3Lv7hDwwo=', '2026-03-30 23:55:50.583001+00', '2026-03-23 23:55:50.648664+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('ce74a3ee-3e98-4989-9dd2-52d43b13acdc', '70000000-0000-0000-0000-000000000002', '0QY7oyvB47zVVuHhdp41BfPdIP1ez4x9xQiUJG9Awe8=', '2026-03-30 23:38:43.798648+00', '2026-03-23 23:38:43.800353+00', 'System', '2026-03-23 23:56:29.081833+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('b37333fa-8c04-41bc-9cb4-ae7a292e503a', '70000000-0000-0000-0000-000000000002', '575G0iqI59t1Tiz+vpxmRr3BHJ9qddcFXBiQz1/dDxs=', '2026-03-30 23:56:29.079206+00', '2026-03-23 23:56:29.081832+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('bb2844f4-1c0f-4af6-a43a-1e2ca3105622', '70000000-0000-0000-0000-000000000002', 'IbPnbE047ElbqUNYDo59IZrniX54RE5TKSpMUbjrY5E=', '2026-03-30 23:56:50.63013+00', '2026-03-23 23:56:50.630787+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('4b5e264f-502c-43f7-9c4b-639b9bbb68c5', '70000000-0000-0000-0000-000000000001', 'j1GaVAiD0lL28figYlORWrq8Y+fFSImpwNEKqX6sK4o=', '2026-03-30 23:56:15.649544+00', '2026-03-23 23:56:15.652333+00', 'System', '2026-03-24 00:11:33.006483+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('6049bc97-5de5-4d00-bd6e-ca3488fb2546', '70000000-0000-0000-0000-000000000001', 'qIsAelxSoU5BFc+uUDogd+e69Wm5dYXldyarSp6rcgY=', '2026-03-31 00:11:32.94348+00', '2026-03-24 00:11:33.006382+00', 'System', '2026-03-24 00:21:45.039584+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('c34b37b8-c102-437c-a23f-24b4b5c67c1c', '70000000-0000-0000-0000-000000000001', 'hxaJL6v+Ln28HmTlVsgrgIv3G6zFzF5YfyTmgb2ZvrQ=', '2026-03-31 00:21:45.038162+00', '2026-03-24 00:21:45.039583+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('ac4a6fb9-ed00-4f87-bfb7-97dd240f676a', '70000000-0000-0000-0000-000000000002', 'CEqbv76Jeocom1SQ56IPa9DWeVZrfSYH40Y5ppujrT8=', '2026-03-31 00:11:40.743317+00', '2026-03-24 00:11:40.744429+00', 'System', '2026-03-24 00:22:28.443344+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('e42aff16-a947-423d-8d28-069f8a0eafe4', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'nJJcVlAAcOPzPzQFRO6MDyuFmQrAVK8nD8g4tTlpfw8=', '2026-03-31 00:45:17.815162+00', '2026-03-24 00:45:17.893402+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('8ab8aa0e-3d63-4089-ad17-5ce6f03d5640', '70000000-0000-0000-0000-000000000002', 'A7JlYvdwOYwKVPiGr4Zpy1UpyJEfIVlX1UBOssU9FG4=', '2026-03-31 00:22:28.441774+00', '2026-03-24 00:22:28.443343+00', 'System', '2026-03-24 00:45:19.207525+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('1d5436cf-fd81-43f7-af1d-11b487da4bd4', '70000000-0000-0000-0000-000000000002', 'cIkqmwpputnZ+AuwuB5Jw56qKqsAsewSyO6bNnScdqE=', '2026-03-31 00:45:19.206169+00', '2026-03-24 00:45:19.207523+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('2faad96a-0d7d-4833-bca4-7e549ed6df54', '70000000-0000-0000-0000-000000000002', 'jdlY+di23gDaKqyF1O1ViqLVcEPmAOTOQq1/zyMfqs8=', '2026-03-31 00:45:37.563392+00', '2026-03-24 00:45:37.564551+00', 'System', '2026-03-24 00:55:53.914179+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('c7dd243a-0262-4367-bee2-c9722c8bc055', '70000000-0000-0000-0000-000000000001', 'fTCQXJ5H895x3lOeaRdAYSkhm6nX2zatdlNSmpa9p2E=', '2026-03-31 00:45:02.066094+00', '2026-03-24 00:45:02.067679+00', 'System', '2026-03-24 00:55:58.828923+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('9500c50e-ec91-4e7a-8b5f-86f0510f22d7', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'VIHYO/Db0XbOCKK4uUEMAyhyHud6n4OoRiJq0jFRC/c=', '2026-03-31 01:02:55.176476+00', '2026-03-24 01:02:55.251969+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('0094bfb5-0f1c-436d-b1b7-c30ff588857a', '70000000-0000-0000-0000-000000000002', 'm2x/pQC4z1WboPLjo4J9fKnzoXXJn7hOiWAg1zbnP/M=', '2026-03-31 00:55:53.765544+00', '2026-03-24 00:55:53.913782+00', 'System', '2026-03-24 01:06:05.17103+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('23d92d79-94de-49bd-b191-72968cd3b7b8', '70000000-0000-0000-0000-000000000001', 'bGmQ3GuL/hOqD+BbCf0CE+TFhH3Sf/EabW9WTdZqo0g=', '2026-03-31 00:55:58.823997+00', '2026-03-24 00:55:58.828765+00', 'System', '2026-03-24 01:06:07.906843+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0e3aaf4d-fb73-4963-bd3a-c7a111953474', '70000000-0000-0000-0000-000000000002', 'ew9nkXN1IK5BG86O63GJJhcGw+oP0A4ELoEr/7Vxtz8=', '2026-03-31 01:06:05.012322+00', '2026-03-24 01:06:05.170771+00', 'System', '2026-03-24 01:16:56.932821+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('ec597195-4309-4870-a8fe-6cc5b2be7a3a', '70000000-0000-0000-0000-000000000001', '28ozuG6dWcptw2M8CiJa7r7f3XMUhs+6jToe91ljyys=', '2026-03-31 01:06:07.902606+00', '2026-03-24 01:06:07.906802+00', 'System', '2026-03-24 01:20:26.914237+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('89b04886-b01c-4688-9182-aa4e521faef0', '70000000-0000-0000-0000-000000000002', 'N0x4PCwTOv66Kj5Y+0ZtErrPU8z1yu/R6sYzvi2ZNPo=', '2026-03-31 01:16:56.930294+00', '2026-03-24 01:16:56.93282+00', 'System', '2026-03-24 01:27:01.968158+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('a0651213-4594-440e-9d44-35a956c171c5', '70000000-0000-0000-0000-000000000002', 'F0ICgvKpNTDSuNR1VmYUDDS7hCa0kGxmbZovlp/boyg=', '2026-03-31 01:27:01.966359+00', '2026-03-24 01:27:01.968157+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('59888523-24a6-4d9f-bfa5-4727b4a7de53', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'Qer/bnMwI8YdYF1SLkJKv/PUDPU9qGqDP0Hi2PXqL8c=', '2026-03-31 01:30:43.314487+00', '2026-03-24 01:30:43.411249+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f4d1b2b9-fc12-49b4-aa0b-ab93b8bf8f7d', '70000000-0000-0000-0000-000000000001', 'AhPqJo7NuzTmn4z/jYEh2wrHD0lWE8jgVnMDmmfgR4M=', '2026-03-31 01:38:01.48063+00', '2026-03-24 01:38:01.484745+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c327e039-3e7b-40c5-8137-a767b9f44cf5', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '70YHFwXW6xotcqviCHqu6ADpzY4HrNGxXKLXsS9vU4k=', '2026-03-31 01:25:26.157331+00', '2026-03-24 01:25:26.271836+00', 'System', '2026-03-24 01:35:58.909466+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('2215f566-7d04-44a0-a226-e1a241c78ab9', '70000000-0000-0000-0000-000000000002', 'zLpz7a8sieSzyy8GnJENy3WxXX3XWSnY6Ncvb7aEW80=', '2026-03-31 01:27:24.806221+00', '2026-03-24 01:27:24.807715+00', 'System', '2026-03-24 01:51:50.874572+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0659df30-cbf4-46e5-b6d2-5eed139b9678', '70000000-0000-0000-0000-000000000001', 'lifa60N7znkTYPW2zTZCNGVXsgqkZXMU5nHNwvjt9iQ=', '2026-03-31 01:20:26.912385+00', '2026-03-24 01:20:26.914236+00', 'System', '2026-03-24 02:12:30.343643+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d00dd127-80e1-4e44-9072-c477d0534d12', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '0sY6yyPUxETSVhpA0XdkoOaRFXOs3iTwp7YJ3XvRpmE=', '2026-03-31 01:41:11.556263+00', '2026-03-24 01:41:11.55677+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('80d663bd-8541-4979-acf7-c2c021e3b0e8', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'POeSlHHxa5FYiIKCQpdhzDRseZm0bMQTwBYgv3IIjEU=', '2026-03-31 01:28:17.102864+00', '2026-03-24 01:28:17.104526+00', 'System', '2026-03-24 01:40:09.641866+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('c6cfdf3d-4548-40e1-a9cf-1c41f40bf8c7', '70000000-0000-0000-0000-000000000001', '7oMa8zlDqStxIUyYHKBQEBt+xPluaiVlUo1DsyhPcO8=', '2026-03-31 01:50:58.785196+00', '2026-03-24 01:50:59.071799+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('71cf2568-5957-46de-88f0-8a037e417c09', '70000000-0000-0000-0000-000000000002', 'Q30BzM6Ov/MDQ2AWAGF9YSV5MEJ5k/WymwCUi+X0lrM=', '2026-03-31 01:51:50.721999+00', '2026-03-24 01:51:50.874236+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('255be923-5e7b-477e-88e8-56dbb3cb5c20', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'DMMqWuczK1T9QDkyxvF31QHrnCe7so2EFPmX8ZNu/MI=', '2026-03-31 01:48:11.134364+00', '2026-03-24 01:48:11.134713+00', 'System', '2026-03-24 01:58:17.348281+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('de8e629b-be4d-44b8-88e5-8a5c441a0075', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'BxA6XcOQKsS8w0tVpeVnWtJaApCQSEdMMEwVZNEgmlE=', '2026-03-31 01:58:17.347853+00', '2026-03-24 01:58:17.34828+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('8fa6bfc5-a249-40b6-9435-bb62e9aac2b4', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'tCyHvVkaxocxyIbNFVQxJSFtu/9PfKIbAKwIArb+Szk=', '2026-03-31 01:35:58.908722+00', '2026-03-24 01:35:58.909465+00', 'System', '2026-03-24 01:55:03.748881+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('98a3f31a-9a7b-46e2-be14-a684e0df1425', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '8FV/VKtUhhnvgso9WKRWywTlJJgh47ArbLzQWntzYZU=', '2026-03-31 01:40:09.641219+00', '2026-03-24 01:40:09.641865+00', 'System', '2026-03-24 01:55:03.799535+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('eac01a43-8133-44d0-ba4f-3a0c2e2204b3', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '30mfiHay9luTeA5vaqRiv2hzcPR40R0gHrenSknq77E=', '2026-03-31 01:56:54.562738+00', '2026-03-24 01:56:54.764621+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9d216b97-cb63-4ea3-8a15-4ba758164149', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'YxGQIshYLYUMlD/GAO11OebloKf6jmnFF2f/xS4H0lY=', '2026-03-31 01:55:03.748558+00', '2026-03-24 01:55:03.74888+00', 'System', '2026-03-24 02:05:10.589785+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('41264353-7e2a-4482-90b6-13f902a0bd43', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'FmoXM46XIDFBrNOEhSIVHpwvfKtzfMGzMoW51cihRa8=', '2026-03-31 01:55:03.79917+00', '2026-03-24 01:55:03.799534+00', 'System', '2026-03-24 02:05:15.638629+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('572962a5-7b72-4605-8a66-102fdbf8cc11', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'ZmWHx4UFeg926Oz2Wt++SLy0fu1o0uxwNlgUFYVmY2k=', '2026-03-31 01:57:43.764343+00', '2026-03-24 01:57:43.767557+00', 'System', '2026-03-24 02:11:06.599642+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('9b02abf9-47b1-439e-9163-38d1c3dd33e7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'FMsjvFp/gYF1PVzQa/v8TMmJcvPLfjh/q3sdNM2HqZQ=', '2026-03-31 02:11:06.597664+00', '2026-03-24 02:11:06.599641+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('715387f0-14b6-4960-88ec-84a10c663724', '70000000-0000-0000-0000-000000000002', 'Tj0kOI2+w8bT0iB2yIDQG3KLuN9qCkmhSXc78ARJFFE=', '2026-03-31 01:52:16.906543+00', '2026-03-24 01:52:16.911737+00', 'System', '2026-03-24 02:12:30.215522+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('252f8638-b97b-40ff-877c-8643a74252b0', '70000000-0000-0000-0000-000000000002', 'QLD37xHKt/sRV4VrcxDSk0F1C/2cvHxS/keWAu2qBAM=', '2026-03-31 02:12:30.212851+00', '2026-03-24 02:12:30.21552+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('fce84f75-82ab-4b80-bf1a-7dd8d213743a', '70000000-0000-0000-0000-000000000001', 'KRcI4cIGRHQ6Qxrz1t2ZJN//NCIOBLNI0BySCXXp7sw=', '2026-03-31 02:12:30.342153+00', '2026-03-24 02:12:30.343642+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('af60604b-842c-460a-9cd5-ee6882bd2dcf', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'm1pli3/NzDxr2TSKtYsatp+nYz9Ernf1upBLJ5s8xQ4=', '2026-03-31 02:05:15.638376+00', '2026-03-24 02:05:15.638628+00', 'System', '2026-03-24 02:15:15.715683+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('18e7c186-ebfd-4e87-8a7d-10260f34b5a6', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'TnLzn9G5b4lzYTPlUVx306bZioUMKv+MZ9i+Br+ly3k=', '2026-03-31 02:05:10.589484+00', '2026-03-24 02:05:10.589785+00', 'System', '2026-03-24 02:15:22.708409+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('8380ec1f-e4c0-45f8-a356-1cd28f33370c', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'jglzzSk6/oi24f7WJ8OGQnPeDi/7V8vn3o8ctnPzXhA=', '2026-03-31 02:15:15.715177+00', '2026-03-24 02:15:15.715682+00', 'System', '2026-03-24 02:25:16.609778+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('41ac5be7-aef3-4963-817c-a29f98c8f6f3', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'DBLOKDwQqNl4gjRmeeqOwkfVG7FRHOaMffJwYFqNdd0=', '2026-03-31 02:15:22.708148+00', '2026-03-24 02:15:22.708407+00', 'System', '2026-03-24 02:25:22.544802+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('034f1207-00b1-4504-967c-e5daebb0544f', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '8P0ybU5XuylGNzoufOocXNbJkW7ZpJTdtPXJlT0mgyM=', '2026-03-31 02:17:16.387649+00', '2026-03-24 02:17:16.574012+00', 'System', '2026-03-24 02:27:33.133519+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('581c0ea1-adfa-4b3f-a3b9-2fd4400d908a', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'fwMXcGUSE2Xukih/C/R1MCZT7pE9sXAsssV/bmueVaI=', '2026-03-31 02:27:33.129232+00', '2026-03-24 02:27:33.133444+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('d412c413-cf45-4adf-a11e-ba907e77b82e', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'GT2BNvcCwo+zt3mYzn2YPoDki7Bei1+OqVxLeMbmxO8=', '2026-03-31 02:25:22.544519+00', '2026-03-24 02:25:22.544801+00', 'System', '2026-03-24 02:36:15.614381+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('2ec78f6a-7850-47bd-bbcd-c9de2254d62a', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'sD5VkUdWb8uBcfwtxtCjOHpjlONkX/nxPxXi4kJIt5M=', '2026-03-31 02:36:15.614135+00', '2026-03-24 02:36:15.614381+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f50fa29c-4cc6-48ec-9b90-07a9e01cdfa8', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'jrek/w9wRsrJ00r/7tIoktpXqVqjfUwQZevt3dCRy5E=', '2026-03-31 02:25:16.609415+00', '2026-03-24 02:25:16.609778+00', 'System', '2026-03-24 02:36:15.681728+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('58e06c8b-f127-41cc-9dbe-277a10aec14d', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Sktix/sHorW2axKQQjvfLpAVoNQ8ZhyRm4fTbk0QG9Y=', '2026-03-31 02:36:15.681443+00', '2026-03-24 02:36:15.681727+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('85f4ac2e-1848-4099-924b-71330abdeebc', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'iiBxz1fh8/mQ93RfTmk8sc9uPcbJL+kh9x1q9FYLJsg=', '2026-03-31 19:27:44.156205+00', '2026-03-24 19:27:44.236+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('df261a93-7292-40e7-8569-59cf92b3fe9c', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'pYAapFz5FIb7iBxkwOhOarBLl/KvjHkxb7jPozHOXLU=', '2026-03-31 19:42:27.807584+00', '2026-03-24 19:42:27.872639+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('e5733036-2f1c-41f7-a79c-f138642bc80b', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'ZxbW8YWbntScVqISIuUZOh6i3IIPM0OqtA3sDRiA32k=', '2026-03-31 20:07:47.003338+00', '2026-03-24 20:07:47.06649+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9fac6216-f80f-4995-bb3c-739336d6cbc9', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'iV4K4GX5ybFrEuZMHWIn9CsSRntulYK+Nm3t+dmgx3s=', '2026-03-31 20:14:23.730737+00', '2026-03-24 20:14:23.802641+00', 'System', '2026-03-24 20:28:51.306676+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('4d9eb2c3-0f24-4b3d-a689-166af696689c', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'kbwCb7fRgekRblVUKgt9y+5AlmS6kNgzDXYo8MWtQBI=', '2026-03-31 20:28:51.302973+00', '2026-03-24 20:28:51.306634+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('e9ac8edc-a05b-428b-a37d-04d1ff97b073', '70000000-0000-0000-0000-000000000001', 'E6AYsjw9e6a2QCJClbY9R+yrafZBpxnIF+nxSj9TH5s=', '2026-03-31 20:32:23.49783+00', '2026-03-24 20:32:23.500272+00', 'System', '2026-03-24 20:49:49.223672+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('befe1600-db27-4f46-8401-4d5fd536562a', '70000000-0000-0000-0000-000000000001', '2nF49rXYNreOjGYf5AMFxIrwQVS8MPHZZmwFCoo7hbk=', '2026-03-31 20:49:49.221972+00', '2026-03-24 20:49:49.22367+00', 'System', '2026-03-24 21:15:15.769068+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0aff14fe-898b-4e30-b6a0-6f43e7f09512', '70000000-0000-0000-0000-000000000001', '4O3bPPHh8KTEZ0V3AbGvZITBIOtWPKLzGCaauBIIZGs=', '2026-03-31 21:15:15.767978+00', '2026-03-24 21:15:15.769063+00', 'System', '2026-03-24 21:25:26.340878+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('c16d9046-3ef8-4f41-b954-bbf5d9f0b060', '70000000-0000-0000-0000-000000000001', 'ESiuBzoBDU2pL2/ummDSp6oTj8p26gqouGdsfXObzvk=', '2026-03-31 21:25:26.339734+00', '2026-03-24 21:25:26.340876+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('937a1deb-0c82-4dd6-9681-883292fc6d52', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '8W8aqQ8Far8/2yqnt9LdWp85wTGZGy+YPz657ytKdkk=', '2026-04-01 03:42:02.725387+00', '2026-03-25 03:42:02.852988+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('24d0e486-8d01-4f64-b513-cc551d92cf2b', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '5KaAiAxv/UVcFyUMMqaKIDCMxuRzu8Qe8YGXV8uhDaU=', '2026-04-01 04:15:01.719887+00', '2026-03-25 04:15:01.915358+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('5ba8adad-fe07-491a-b46c-694dcb7a842f', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'fakcFZjXnX7FjzkKpy4Avw2f3a+SN3gtmh1LMBGJOUs=', '2026-04-01 04:21:03.862405+00', '2026-03-25 04:21:04.124141+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f5d22c12-329c-4cb1-9cdc-64895be35b42', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 's9AH7xJp7EZKdeArHBrRpW3/QpaYUGHrerN+rffMixw=', '2026-04-01 08:38:15.821053+00', '2026-03-25 08:38:15.940479+00', 'System', '2026-03-25 08:48:44.43365+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('a63c1627-a10c-4bec-827a-698b77fd7f75', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'BmcptaZGzVMlhxQshE2dYhJOiWg3bGLqd9PYYaJfaio=', '2026-04-01 08:48:44.431913+00', '2026-03-25 08:48:44.433632+00', 'System', '2026-03-25 09:01:33.56666+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('fdc51945-aa3c-4de1-8434-9d156801183e', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'dgYpuNeB54O7IQ0Evie+XneL979FQkwe1C5IssGMsKs=', '2026-04-01 09:01:33.566145+00', '2026-03-25 09:01:33.566659+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('bd8e5edc-3dfa-4447-8cc0-6790185f8223', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '6gOVXYzHuQfSOrjHopKi+iIt1ubwoy75uyDVKmLHHbE=', '2026-04-01 09:38:15.777475+00', '2026-03-25 09:38:15.894286+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('66c62dcf-6309-4314-af65-0c000550f4a3', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'WhAOc5ADFkXlwuX/gTlxDmzIKDk+7sUwtqDEBzf7Pl4=', '2026-04-01 09:51:21.021054+00', '2026-03-25 09:51:21.145811+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('206207ac-78bf-43a4-b6dd-ac9a68c8ba49', '70000000-0000-0000-0000-000000000001', 'j5b5mywtJuLXvXnV8IFxWt8mTZCACY1iUyuVf7x3jlw=', '2026-04-01 09:57:14.472326+00', '2026-03-25 09:57:14.477093+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f81888b9-ff2b-459c-86dc-205f05cc251c', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '/rHIf1Z37MGd4vd5jnjwhJ0rFLdWUhnQDulBKanwU/8=', '2026-04-01 09:59:42.709106+00', '2026-03-25 09:59:42.709851+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('427831b8-5876-4903-b8a3-3e5501b91e35', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'IkNasD4IOUqNm7lRt4cKl5JLFkXh8dgIKTKSRA0OEWM=', '2026-04-01 10:01:34.681641+00', '2026-03-25 10:01:34.682185+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('73ff1111-4f72-4cd9-beb5-bf2915dd2aa2', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '8EJhfsDQpLYTx3+St61raecqe7gHVkou+43lFWVRzzk=', '2026-04-01 10:11:12.988833+00', '2026-03-25 10:11:13.108798+00', 'System', '2026-03-25 10:21:41.868337+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('1388dec3-8e8c-41b8-a5a1-4be4dc8a77c7', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'vHOeN0hOKyahJlBY89st5nzHY6pQhwuTvi1TQNVhpig=', '2026-04-01 10:21:41.834373+00', '2026-03-25 10:21:41.868312+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('cc06ddc9-9816-48c6-9cd1-47ff75aee593', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'fNYLYVjNVUUMz/L5TAKTQY+zkwANrrpaxqBT2kD1UOc=', '2026-04-01 10:24:54.061151+00', '2026-03-25 10:24:54.061923+00', 'System', '2026-03-25 10:35:08.42043+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('597b4f57-5ba1-4e8c-80a7-c9cdc40d5566', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '5gGdmcIHi0LGb1NIaUgI5cag1VbKH8StqhN/QXG6vRY=', '2026-04-01 10:35:08.419875+00', '2026-03-25 10:35:08.42043+00', 'System', '2026-03-25 10:35:19.825063+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('20adad45-fc4e-49c4-9b24-342215af9f15', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Z2C4Nff5Hcoh82j4enb4j1Kw1YGtTL3ztsvSNGDGmao=', '2026-04-01 10:35:19.824463+00', '2026-03-25 10:35:19.825062+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('25095655-a8dc-4e9b-9291-3aa4b0ab3c41', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'mJj5ymIPSoPfh3+Gr0Jd0BBfO0TTQSelvuewwg9WjAU=', '2026-04-01 10:15:10.089831+00', '2026-03-25 10:15:10.308058+00', 'System', '2026-03-25 10:37:03.976443+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('a6c8c2d1-4637-48e8-a9b6-fcf5f2d1e8fb', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'CUMigdKoh2au4pZ5f81/Vi7rYHl6fAlNhdY5iftwcGE=', '2026-04-01 10:37:03.970675+00', '2026-03-25 10:37:03.976367+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('225150fa-1075-4c7f-b650-439f8311b2fa', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'Wyli66URhlxwaOApl1HKgvBhxjPHazmoiNWsujZRij8=', '2026-04-01 11:19:27.264107+00', '2026-03-25 11:19:27.381977+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('13fb21c3-0143-4d31-857b-82e7ad261b9e', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'JhM9pFVZrC9TpHhv7uhjSQaWzLJgNy+mRKu2ZqJ+qgw=', '2026-04-01 11:27:05.506817+00', '2026-03-25 11:27:05.508517+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('87bd78c2-4bb3-4c93-8a23-cbdf6c7fcd89', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'iPHLALHH+4qW/Hen2OPpVQzP98rMdAaTQCTFd+LWvR8=', '2026-04-01 11:58:18.845173+00', '2026-03-25 11:58:18.845661+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('80d88342-9f0c-41a1-a0c2-11900c6e223b', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'nynmUm8rVaKawtaHV6+RZJUd8ByrkcVEKJngt6pgb4k=', '2026-04-01 12:33:03.016976+00', '2026-03-25 12:33:03.132991+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('3ce66a0c-1fbe-468e-901f-5268e44d7436', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '/1/d5mwHSMgOzlWtrAPu6FlIdkrrND4JFqCbfYHMNxA=', '2026-04-01 12:49:57.937865+00', '2026-03-25 12:49:58.051151+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9d9a52ff-2d18-48a2-81c2-eb0d1859434f', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'yLn6LKlMvpgO980j1CQwyU0kIaNMKsv/EtegFAZQUo4=', '2026-04-01 13:07:52.688733+00', '2026-03-25 13:07:52.689963+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('b17c1117-32a6-4750-8f1d-6ad0c06ec79c', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'mFy1ztVjwGkEDJrrQfHheh+lq4OgpToECdahOocvE34=', '2026-04-01 13:41:00.946465+00', '2026-03-25 13:41:01.046232+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('59485a19-7450-437d-8925-4ff760146123', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'sK92K2RakJfA3kVk0qS2vtYiVgK968iBctRcmAt6/60=', '2026-04-01 13:28:26.509721+00', '2026-03-25 13:28:26.510134+00', 'System', '2026-03-25 13:38:54.109003+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('912b071d-cf6d-4c64-aa6a-870da36caeb6', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'rqFQfMZ2tMpwesYqTQryRQzoJ0ciIbAb6txiPlfBxnM=', '2026-04-01 13:38:54.107633+00', '2026-03-25 13:38:54.109001+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('1c35c7ce-e521-47c7-ad9e-c8db3a912709', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'fRijKqfIxtPxjvEva0/Qr7WyhFxeotKEIbs8QBGyoqA=', '2026-04-01 13:42:39.270057+00', '2026-03-25 13:42:39.270351+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('cf5f2224-86f9-4248-925d-193fcadf1896', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'uX0SuzvgdNDi1GeQGHgmMHLl1/2NIma2UPGRVPuQqTo=', '2026-04-01 13:50:52.076289+00', '2026-03-25 13:50:52.07691+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9e260181-f1f3-4049-b92b-350e6641df41', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'aiq3topF+JNCq2Ion6zvqPtBqqAQ3nYv3L6vHd9R+x4=', '2026-04-01 13:57:58.897942+00', '2026-03-25 13:57:58.900413+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('7ff1fdcd-00cc-4b4d-80e0-0938658bad35', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'Cx0OOV0wSa+o2kKtjM9xPsPS2wmeoHK7p81tmeakOq4=', '2026-04-01 14:04:26.607908+00', '2026-03-25 14:04:26.608227+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('0151c83a-42c3-4e00-ac79-f39cd3538177', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'dj3gHdZnfpheQzs2dKx06VZF01rq51wXY6BAOVh6KHw=', '2026-04-01 13:54:07.702878+00', '2026-03-25 13:54:07.845743+00', 'System', '2026-03-25 14:04:30.772107+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('2d390cda-2ebb-41f3-b834-261e515853f3', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'QsmvU84ev4mhNCGtq3wyEmWYw+8xS2J01XwRgqrlEwA=', '2026-04-01 14:04:30.771123+00', '2026-03-25 14:04:30.77207+00', 'System', '2026-03-25 14:04:52.724437+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('e532058c-51c7-49bb-b623-3f0b2b5e43b9', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'vFQhhIwDorAejsjSGlFJOgMjkQkApxG9ewjEnbinJbM=', '2026-04-01 14:10:53.16807+00', '2026-03-25 14:10:53.168433+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('2efe35cc-b6ba-406c-86ac-a47abf13d709', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '8o6ZR2n+PPEgoZdIUn1FLClfeKm/vY5sggmTXUoxs9w=', '2026-04-01 14:04:38.331846+00', '2026-03-25 14:04:38.337871+00', 'System', '2026-03-25 14:18:26.537012+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d462753e-d7bc-4686-a613-b9b886833c7d', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'VfUEOS/WTRPJaHI8u5IJMmNnaee3fN52FmXUq9t49LU=', '2026-04-01 14:18:26.535793+00', '2026-03-25 14:18:26.537012+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('22ae40b7-0cc2-4797-b40b-6c196f26660b', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '+eQT2UC2ijzsbbiCBPoKh+qoK+mS7Dt4RTGzl7IDpFQ=', '2026-04-01 14:04:52.724051+00', '2026-03-25 14:04:52.724436+00', 'System', '2026-03-25 14:15:07.682739+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('586d7d08-45dc-449b-abc0-fe3f70296991', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'qPSF5P4Q2D7Wqahm7oEDUYGkVTB+nG+q7JS15uq8Yrs=', '2026-04-01 14:15:07.682304+00', '2026-03-25 14:15:07.682738+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('0b2c9ecf-6cf4-4e0a-96e6-9f85df011698', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'WCm2xjsw74Iv4iJtwpC0PNf3sJezJjAkLkCRxLqDEjw=', '2026-04-01 14:21:47.923913+00', '2026-03-25 14:21:47.924298+00', 'System', '2026-03-25 14:36:16.994367+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('fd91f87a-d30e-4da3-8c4d-42f362831a70', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'aeBwhxe+z+dGPriF2KJQicqm228Yl0koIlOftefhGGE=', '2026-04-01 14:36:21.995379+00', '2026-03-25 14:36:21.995644+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('64921bd5-747d-4a5d-8d52-b6a1f7cd53ca', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'lyWtXl6Av0B0PimOmouvbp0gNNxqAMIuo4aRkij5MfM=', '2026-04-01 14:23:39.173144+00', '2026-03-25 14:23:39.173412+00', 'System', '2026-03-25 14:43:28.600362+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('f9274fff-7fd3-4537-beda-149c6845f688', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'BEWPgLXn2YoW2sXSERhqyIp8b8R9DOy27+Sn3c8vViI=', '2026-04-01 14:43:28.599957+00', '2026-03-25 14:43:28.600361+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('6d091447-f0f9-473a-bb75-ff62035de5d0', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'OWFJpZVbvV7GVSW9B7wQceX6WFjl91smq6PJpdFw0CY=', '2026-04-01 14:36:16.993997+00', '2026-03-25 14:36:16.994366+00', 'System', '2026-03-25 14:46:19.42262+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('45cca8aa-9b45-41d1-b699-f5b28d8e9cb2', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'J3CLPu2Iu0N4vSy2anqy0Wp/CTN9pD1nZ1cvqyZtd2c=', '2026-04-01 14:46:19.422389+00', '2026-03-25 14:46:19.422619+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('2fd9a146-3110-4dd9-ba13-ef8272444dbc', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'zWDJTiIOql9RhYj7R7+nfi15zdG6W0HQo8DP7/0t8wM=', '2026-04-01 14:48:39.377813+00', '2026-03-25 14:48:39.378193+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('b7939a0c-9a34-4493-bcec-18984362ba75', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'vBobzmtoRowqSlQ9Qit+MUwnO8Z5EDuc2g+tcLnZnN4=', '2026-04-01 15:06:46.558917+00', '2026-03-25 15:06:46.618569+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('6abea7e4-9d55-4fb9-bbad-4d86d41fe926', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'd6dZk3ZznA2Rdkn0GH8sckn3M+PaR89u0nPH9Gh3POs=', '2026-04-01 15:23:42.589048+00', '2026-03-25 15:23:42.590835+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c3eb324d-a977-4808-86f4-558b1e20810a', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '3jceCubyJxD+TbMtY73OHwo3QypGhprUMtmuN5f6OFE=', '2026-04-01 15:18:41.499758+00', '2026-03-25 15:18:41.580746+00', 'System', '2026-03-25 15:28:59.763099+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('0bd768af-b472-4812-af08-82c457a0bc56', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'fJlH5LG1Ls9C1kAR/vo9YNHMP050ttMnuCCWSHnlRGs=', '2026-04-01 15:28:59.574317+00', '2026-03-25 15:28:59.762879+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('5ed3f8d2-d1a7-4fce-971a-9f277e7b93d5', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NIClJ0iqtGp2SBs+8gNShlXuU9RKKHY5PSW2iyqsV+0=', '2026-04-01 15:20:39.786963+00', '2026-03-25 15:20:39.921328+00', 'System', '2026-03-25 15:30:55.677127+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('fd5ca393-5d8a-407e-b73b-16f0273a5571', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'm+u73FNaN0DbX3AgT5nWqEOz5GIAthd1G6bJv1XLRdA=', '2026-04-01 15:30:55.676448+00', '2026-03-25 15:30:55.677126+00', 'System', '2026-03-25 15:31:17.552545+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('759b6712-b8bd-44f8-aeb8-7df1294a7748', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'OkXvrBA520qRRkYGSea8yZbVL3+lCh1LOc/sgtZ29kU=', '2026-04-01 15:31:17.55176+00', '2026-03-25 15:31:17.552544+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('13025b37-989f-4fa7-afe1-09f37eb10db8', '87b82f07-53c2-46a4-bdde-f1589b3aa372', 'xzTWhTRJdRwwkzqjkUejUngifM18kMjCyHigOL9N7Iw=', '2026-04-01 15:37:33.430678+00', '2026-03-25 15:37:33.471885+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c18eae71-654f-4fc6-b230-34143510c1dd', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'NeO8Hfb4ijuj5u9deB2Il5e8rn6HLD0N8jLLvelnDLs=', '2026-04-01 15:30:36.588659+00', '2026-03-25 15:30:36.589118+00', 'System', '2026-03-25 15:43:45.766084+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('eae7edc3-72ff-48b0-982d-8a80922fa51d', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '8key7CP1jmY9T6PRJbbrgoEoQO7uZqe90MSR/tqf8gE=', '2026-04-01 15:43:45.742636+00', '2026-03-25 15:43:45.765904+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('4417d3e0-1279-48c6-b824-a0fa442c7d74', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'WMI7ZFppBiAW0iKQgs9PO+YbKWIKapAbghLEQ7mZfCw=', '2026-04-01 15:31:23.643789+00', '2026-03-25 15:31:23.755107+00', 'System', '2026-03-25 15:41:42.416235+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d5356b35-46bf-4cec-a5ec-7e49a2463cc4', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'KjJINqO0c5CpN7eDWZ5Jn2zXuXtkjDa+l3prJ9VqNsM=', '2026-04-01 15:41:42.331325+00', '2026-03-25 15:41:42.416094+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('3fcea27a-7df8-4004-aada-1c1994a67b0a', '87b82f07-53c2-46a4-bdde-f1589b3aa372', 'Y9oCwgtUuhvUaM4Urn0gPLEeQi2E9iEqVjxzw4HJn/Q=', '2026-04-01 15:42:07.267711+00', '2026-03-25 15:42:07.269188+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('731e9a44-3e09-4d60-a897-f74d06a4a64b', '70000000-0000-0000-0000-000000000001', 'VI5zcbtK6C4lpCB/d3y+6H5rxdVB9wpUMnYZFjVCTWM=', '2026-04-01 15:37:23.464749+00', '2026-03-25 15:37:23.643417+00', 'System', '2026-03-25 15:55:20.532089+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('e3af2f5a-3557-434c-ae2d-50ce301e670a', '70000000-0000-0000-0000-000000000001', 'ksOoJ1Mdpy5Gz8OY2CQA5CsZy48GuzMCyj4sJ+uNDno=', '2026-04-01 15:55:20.523052+00', '2026-03-25 15:55:20.53204+00', 'System', '2026-03-25 15:55:32.434093+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('4f52f9a6-5152-4493-898c-12c04f7451da', '70000000-0000-0000-0000-000000000001', 'iwsAteWCI4ohrAmDgPnhq8i7IEpS+QTxxG9XhcUezzk=', '2026-04-01 15:55:32.433373+00', '2026-03-25 15:55:32.434091+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('c10376a3-f00d-49a3-b021-2b425aec0880', '70000000-0000-0000-0000-000000000001', 'UCCHt58X8GJpsZlerMhLPhtJ7LHO7E1uRiWdgmczmE8=', '2026-04-01 15:47:49.636558+00', '2026-03-25 15:47:49.64203+00', 'System', '2026-03-25 15:58:25.862813+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('8ebedbe4-e61c-41c3-ada8-3ee301b437d5', '70000000-0000-0000-0000-000000000001', 'fc1FYoWtWiQItTtWowNZk7Gvqx71AuqP35ncVc4ExSU=', '2026-04-01 15:58:25.759339+00', '2026-03-25 15:58:25.862665+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('8428fa3e-c0ca-4d1e-8db7-7f145f86aad4', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'ikpS6z+oNiR9GhkHCuiYjlMVoBUuA5gTpSNQs1yljzU=', '2026-04-01 16:09:15.640033+00', '2026-03-25 16:09:15.640667+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('2915b06b-87f4-4e62-9e08-a4656634b7dd', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'P9rirymgPmllLHQfaCY2m37I+e64X3WqvygZBGiUQ8M=', '2026-04-01 16:09:45.299812+00', '2026-03-25 16:09:45.300282+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('990ba166-8980-4f52-aa5e-aed92c81931f', '70000000-0000-0000-0000-000000000001', 'DNCAbB2D5/6MlTGDL1AEprPMhZWujUzyotbBsd8WPNc=', '2026-04-01 16:01:52.923146+00', '2026-03-25 16:01:53.037816+00', 'System', '2026-03-25 16:12:17.209758+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('642f531d-7910-4eeb-ae90-953df8336b57', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Go/ELny13r7PwUq3b3a2/H3QfZ89DkSXfTwEqbmJXl0=', '2026-04-01 16:02:11.598957+00', '2026-03-25 16:02:11.602267+00', 'System', '2026-03-25 16:12:20.398046+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('fbde71ca-8035-4047-96ac-d3e0c2349bb8', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'I5vxvUF0hxMkUFsk57LLUfHis4Tk2LB2jM4BnKc//mA=', '2026-04-01 16:08:57.893256+00', '2026-03-25 16:08:58.023437+00', 'System', '2026-03-25 16:22:30.582764+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('4a47dad0-1730-48f3-935a-6c66fbfa79ab', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'GwAs5tdCUQ4K5jeRyPpPOEz2uAm9vgmJZx6FxrX8Iv0=', '2026-04-01 16:22:30.536046+00', '2026-03-25 16:22:30.582617+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('23cb4146-3d10-4b70-b635-52b3288d8526', '70000000-0000-0000-0000-000000000001', 'DHwrIvZ3L0zn+LhWYsG6ckdRtUVEciD1nN7Vns7Q8SE=', '2026-04-01 16:12:17.207632+00', '2026-03-25 16:12:17.209755+00', 'System', '2026-03-25 16:22:25.620594+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('5bc1895e-93e1-4a7d-ae22-bb461b515b26', '70000000-0000-0000-0000-000000000001', '/iVh7HpPUXTKCJhiFXzXt+PDwPAuuKC3taekHrCNIxo=', '2026-04-01 16:22:25.522139+00', '2026-03-25 16:22:25.620471+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('e398cf51-396d-442b-a179-089cfe34b827', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'uCH3FYbHJsVL49SMWytiSWheYRG1Q75ttAu2oxcU3dc=', '2026-04-01 16:12:20.397387+00', '2026-03-25 16:12:20.398044+00', 'System', '2026-03-25 16:22:34.949955+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('026082e2-1f35-43db-a3df-01e8d578c002', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'Gzq+fdcH4yFxvw+E3pbO9RAWbMv2FqUG2S9nZFLEPFY=', '2026-04-01 16:26:57.314223+00', '2026-03-25 16:26:57.429418+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('6625ebd3-3403-4426-b48a-59201a5a932b', '70000000-0000-0000-0000-000000000002', 'dGwGzUH65Ns3+u9NijV7KJimOHZcWEJXzz2NXE+SJAE=', '2026-04-01 16:26:02.326841+00', '2026-03-25 16:26:02.328534+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('e76ba030-ef75-49bd-bcc2-a08795c1a296', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Wc24qVd0tzJ5S96gYx7v6fAAkXKWqdscOLVaCpdYu7Y=', '2026-04-01 16:22:34.943265+00', '2026-03-25 16:22:34.94991+00', 'System', '2026-03-25 16:35:57.47025+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d46ac18f-0eeb-4252-b9fd-23707f9de7a1', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'nLRB/qkhbdIk2o07/Fh6pl0DbqsPsC0XT/adTUbdr6w=', '2026-04-01 16:35:57.36731+00', '2026-03-25 16:35:57.470118+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('8be972c5-ec19-4c35-bc29-bcf9bc7e34ca', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'svAIwjC7uMbVNAGxLnToverze4YLAfd+aEC2wsrxJjc=', '2026-04-01 16:36:41.43694+00', '2026-03-25 16:36:41.442828+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('07fa6fc6-4215-4afc-ba73-d6109cad5b7f', '70000000-0000-0000-0000-000000000003', 'euCq/P9qAM811Vc6mmMBjvnrsj/emYIRq1AJEjYNLto=', '2026-04-01 16:27:14.617883+00', '2026-03-25 16:27:14.618761+00', 'System', '2026-03-25 16:37:43.394922+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d73c7b02-ee11-4f89-9fd7-5c403afc031a', '70000000-0000-0000-0000-000000000003', '2buVr9dZym98D3q7Dv0EQjlPn5Uk/Z2EnBXTdf8ZIxU=', '2026-04-01 16:37:43.385875+00', '2026-03-25 16:37:43.39492+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('bd8c841f-d4f0-41ca-9fb5-e8092ac75800', '70000000-0000-0000-0000-000000000001', 'jcg15o98kDygitaeECaT2xVPgDhgAJal6VRamsUJmpU=', '2026-04-01 16:45:04.36673+00', '2026-03-25 16:45:04.41713+00', 'System', '2026-03-25 16:45:24.225525+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('68efa0c6-cd3a-4753-abf9-a3cae517768a', '70000000-0000-0000-0000-000000000003', 'wFjy7pTWk3WiDqQEO7i3vgHqgEfh+QdWA+Pji328aN4=', '2026-04-01 16:44:29.942256+00', '2026-03-25 16:44:29.944388+00', 'System', '2026-03-25 16:54:32.279375+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d5f9df77-389d-43e8-9f85-c0638ac56452', '70000000-0000-0000-0000-000000000001', 'Xu54hYj3N3IMdbrkGXbHZVm/MUBcEABjffq8Gm00wW4=', '2026-04-01 16:45:24.22305+00', '2026-03-25 16:45:24.225496+00', 'System', '2026-03-25 16:55:50.9804+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('8b5e5158-e90f-45f8-8ebf-5618f357113a', '70000000-0000-0000-0000-000000000003', 'aj9ha3epn3r/qi0ED+bKaGpu0a+uE2ijIAomDPlsi4Q=', '2026-04-01 16:54:32.153474+00', '2026-03-25 16:54:32.279237+00', 'System', '2026-03-25 17:05:31.364506+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('f6ae36d2-34c9-4b3e-b16e-ab9c1fc83808', '70000000-0000-0000-0000-000000000001', 'oKg6TtDF8gewiDiqUDFTvlm3KfgcIgx7f7w3jip2FLM=', '2026-04-01 16:55:50.979558+00', '2026-03-25 16:55:50.980399+00', 'System', '2026-03-25 17:06:25.934764+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('df27ed22-4c54-4d4c-a367-d34305ab1923', '70000000-0000-0000-0000-000000000001', 'XRd5P3fcKdk4hoeuIWtY2mDPgIDgDo363YWmMETfluY=', '2026-04-01 17:06:25.925318+00', '2026-03-25 17:06:25.934763+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('a2fd9e89-975a-4d00-9f7e-01d6c108543e', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'CD4JdXuwrdUFsD0z1VIL/fgByacO+0MFHGlwZdnCuqU=', '2026-04-01 17:08:20.809641+00', '2026-03-25 17:08:20.81206+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('6b65b320-b66a-440f-851a-12e7e501c0fa', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', '3HNUwOv3lTN4oQZ+QPuxLmHLZQdwt5l3vDXK3ci8wVA=', '2026-04-01 17:09:47.323078+00', '2026-03-25 17:09:47.323471+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('23d94dd7-7a06-449b-94b4-7e211c25d310', '87b82f07-53c2-46a4-bdde-f1589b3aa372', 'Lasqzr11P2bbr/MuEKTtUf6Y0676z61oxvzq5+aRz2A=', '2026-04-01 17:10:02.035083+00', '2026-03-25 17:10:02.03537+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('08ddb124-1261-4c3a-862b-383471c5aaf9', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'lhN+Regq2Izeeik1WMMt+oc58qNi36EBFhVsxrPJxo0=', '2026-04-01 17:10:11.971454+00', '2026-03-25 17:10:11.971874+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('eaab0464-f4f9-4888-a38d-3b1ec9f43f69', '70000000-0000-0000-0000-000000000001', 'EmQ+FA4JXw2CFYUxtDTfwO+f1YzXM6kGYvDlF5CbqjI=', '2026-04-01 17:13:24.697744+00', '2026-03-25 17:13:24.786416+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('96a4345d-a05f-46cd-b11c-da4829322d39', '70000000-0000-0000-0000-000000000002', 'g90vGEgjfCowM489P14yysKYMYRpNbjYqSm+SIhVoig=', '2026-04-01 17:15:19.707354+00', '2026-03-25 17:15:19.708441+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('b02eb777-3250-4cbc-81c3-d86d3ed624c9', '70000000-0000-0000-0000-000000000003', 'XKAihNUEE0qRMFl4aSPUhZrjUdVEbeiHHlYFskUUqtA=', '2026-04-01 17:18:46.706091+00', '2026-03-25 17:18:46.706699+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('7f6e02aa-64dd-40a2-ba92-f685d6c1322d', '70000000-0000-0000-0000-000000000003', 'RM+tS95UM+nq9SpHaM61kpgbE832xi0VJFMY4+X6k5g=', '2026-04-01 17:05:31.355813+00', '2026-03-25 17:05:31.364466+00', 'System', '2026-03-25 17:16:31.095594+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('10040f89-5d84-42a2-9aa3-bff874148419', '70000000-0000-0000-0000-000000000001', '0EnhL+C2pF9LtwYiY2UDazoKtVL88wjVom9hBnJIJsA=', '2026-04-01 17:17:51.587579+00', '2026-03-25 17:17:51.644101+00', 'System', '2026-03-25 17:18:03.68121+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('602db9bf-72e4-4e63-8ab7-f187e85054cf', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'RliC2JKCwIupIucVojAMTRC1FtcltAdKFVRnMI6pqA8=', '2026-04-01 17:08:24.138665+00', '2026-03-25 17:08:24.139266+00', 'System', '2026-03-25 17:18:28.231953+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('10ec8329-4ac8-406c-9e51-6083483bbd7b', '70000000-0000-0000-0000-000000000001', '6gMQt8uXF5uMP8OG6FluZKI5I/XEB+d0IJbm/VTaZ7U=', '2026-04-01 17:08:37.456777+00', '2026-03-25 17:08:37.458472+00', 'System', '2026-03-25 17:19:06.273104+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('d874f15f-ba6b-4698-b4d9-b234cd152932', '70000000-0000-0000-0000-000000000003', 'JVQTX+MQ/vIlZ5RnyZL1D2CgJeIHY92rs+aHiqR8HNg=', '2026-04-01 17:16:31.094664+00', '2026-03-25 17:16:31.095593+00', 'System', '2026-03-25 17:27:31.116017+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('ada362c7-21b6-471b-aadd-93de3a1c6d0c', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'dwIcr/E/fQkGgCFpSLV4qmENowlY3MXSi2P5kvOwsME=', '2026-04-01 17:18:28.231412+00', '2026-03-25 17:18:28.231951+00', 'System', '2026-03-25 17:28:28.609292+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('37130302-c86b-4b79-927e-227a66f3e81b', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'InHilCiS5Isp6CkFaNWkxXfNnOZ4DolvYELBGBLIaDM=', '2026-04-01 17:28:28.608926+00', '2026-03-25 17:28:28.609291+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('81c80977-7c0c-4491-ba82-690c9c41099c', '70000000-0000-0000-0000-000000000001', 'KkiHx472TFlZMby/MXvJWxBnsvuk8LQpDRwER5UTSHw=', '2026-04-01 17:19:06.272022+00', '2026-03-25 17:19:06.273103+00', 'System', '2026-03-25 17:29:06.532659+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('8bb4be7a-0e14-4b84-9545-2247e8d2372a', '70000000-0000-0000-0000-000000000001', 'rBjkdRlcLnwoMKzbM1uA6Fyvq56/x/m+fyb6Yz0ncHE=', '2026-04-01 17:18:03.678662+00', '2026-03-25 17:18:03.681177+00', 'System', '2026-03-25 17:29:25.051798+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('7a1d2de0-b513-4f1d-b79c-3c044e704b43', '70000000-0000-0000-0000-000000000001', '1Oq6nCPJCDQ5wxHur5VZXqFyZXUy79EnuiAjHSBTBkY=', '2026-04-01 17:29:24.873056+00', '2026-03-25 17:29:25.05155+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f502b5e9-c1cf-4ac4-bbaa-16acd167d11d', '70000000-0000-0000-0000-000000000001', 'HBMqMTwBnHp8ENKVU5TSqZ8t2bDC+dqMkzbZpioAcZ8=', '2026-04-01 17:31:39.182544+00', '2026-03-25 17:31:39.186699+00', 'System', '2026-03-25 17:31:53.807295+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('f0a70a96-991b-4487-aba7-ee45875e48a9', '70000000-0000-0000-0000-000000000001', 'vksRjcn1reGqMOHOvynl10z6FgQSpcZg+rx6vLQsdm8=', '2026-04-01 17:31:53.806164+00', '2026-03-25 17:31:53.807294+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('9be8aafb-f746-45b6-990d-0ae73a5aa802', 'd9814bda-f007-413a-9d73-256bbf43ced4', '2VD0yP/N94rGSmGJSgo9V5MRkw13RUTEVZFUAR4iOVk=', '2026-04-01 17:32:00.442376+00', '2026-03-25 17:32:00.531691+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('f0e4072d-0c65-43bb-94c1-1d1f23899c06', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'QSGyaWIjKIC2ZPgus03BQZCBDG2r1tm7FBXtJUE87nw=', '2026-04-01 17:32:12.962489+00', '2026-03-25 17:32:12.968832+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('2f762db4-8c99-4e6b-aebd-08ee2688eb43', '70000000-0000-0000-0000-000000000001', 'jgvQC3jJU4d0V7lzsFEgH1Sq2QeL3JtSesXMMJajI+A=', '2026-04-01 17:35:27.545455+00', '2026-03-25 17:35:27.549471+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('8ddaadfb-0f60-4ebe-b191-f6fc57ee5aa1', '70000000-0000-0000-0000-000000000003', '25hHyf4M47s0uYdaE+prBPuf4FsDrKhgioXakKJf0Mc=', '2026-04-01 17:27:31.114153+00', '2026-03-25 17:27:31.116016+00', 'System', '2026-03-25 17:38:30.981423+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('3b21d8fb-4784-4f53-bc79-3b83baf15c1a', '70000000-0000-0000-0000-000000000003', 'n8vkMBbn3Uhz00NgFS6SWbPTqSKyEQC4ca9Rc3CN3oA=', '2026-04-01 17:38:30.980103+00', '2026-03-25 17:38:30.981421+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('a438d72f-a095-4529-a92e-ebd12954d6e5', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'OiN/GenP4PScnccED5afqp6liKAG7RdnDfJ/DrNA8/4=', '2026-04-01 17:38:31.295856+00', '2026-03-25 17:38:31.29612+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('35363d53-08ce-46de-8e11-a4baa688bfac', '70000000-0000-0000-0000-000000000003', 'qFT1jpXrtTpXn5gY6SRiv06jSH/fY70xNHAmiq2hrv4=', '2026-04-01 17:39:15.646041+00', '2026-03-25 17:39:15.646952+00', 'System', NULL, NULL, false);
INSERT INTO public.refresh_token VALUES ('a31d1833-4fc4-457e-bcbf-bfc06560fbab', '70000000-0000-0000-0000-000000000001', 'pkmWlxTKc3wQU4O89Zu7q8ZddE4nIbAaHZKbC58Uwq8=', '2026-04-01 17:29:06.460198+00', '2026-03-25 17:29:06.532658+00', 'System', '2026-03-25 17:40:06.075895+00', 'System', true);
INSERT INTO public.refresh_token VALUES ('666d1378-9910-4242-a19e-9393597ae248', '70000000-0000-0000-0000-000000000001', 'ZWJ60GGvAl7cdSH7sY0y8oWo7wS2a5MofcOvKuDweu4=', '2026-04-01 17:40:06.075015+00', '2026-03-25 17:40:06.075893+00', 'System', NULL, NULL, false);


--
-- TOC entry 4597 (class 0 OID 33359)
-- Dependencies: 422
-- Data for Name: report_downloads; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4598 (class 0 OID 33366)
-- Dependencies: 423
-- Data for Name: report_schedules; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4614 (class 0 OID 34865)
-- Dependencies: 439
-- Data for Name: research_saved_category; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4591 (class 0 OID 33291)
-- Dependencies: 416
-- Data for Name: return_policy; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.return_policy VALUES ('8d1b7029-92ef-4ae8-b84d-df2d9f71b940', '65987e62-8992-4f8f-bb21-1e3eea0f453c', true, 30, 0, 0, '2026-03-17 04:13:56.529302+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', NULL, NULL, false);
INSERT INTO public.return_policy VALUES ('356b05e0-f5cb-47d6-9839-b66da0d280f9', 'e43d2fb2-d880-465d-87b6-3f72f60ece73', true, 30, 0, 0, '2026-03-17 05:36:33.170373+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false);
INSERT INTO public.return_policy VALUES ('4ebc058e-3d30-4486-a53b-ad81b16b1da8', '5cfe4d19-77f5-4594-849f-577db1f97b92', true, 30, 0, 0, '2026-03-25 16:10:14.443778+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false);


--
-- TOC entry 4608 (class 0 OID 33548)
-- Dependencies: 433
-- Data for Name: review; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.review VALUES ('1eb04868-a0b2-4a23-a4c7-cbc7f3bc9b1e', '71000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002', 'Buyer', '70000000-0000-0000-0000-000000000001', 'Seller', 5, 'Excellent product! Fast shipping and great quality.', NULL, NULL, 'None', NULL, '2026-03-13 10:04:29.899213+00', NULL, NULL, NULL, false);
INSERT INTO public.review VALUES ('8ce91900-d5fa-4060-afe1-29f42feca72e', '71000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003', 'Buyer', '70000000-0000-0000-0000-000000000001', 'Seller', 2, 'Product was not as described. Disappointed with the quality.', NULL, NULL, 'None', NULL, '2026-03-15 10:04:30.051648+00', NULL, NULL, NULL, false);
INSERT INTO public.review VALUES ('18721aa4-45d3-44b3-96c4-c9c58ab20404', '71000000-0000-0000-0000-000000000003', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'Buyer', '70000000-0000-0000-0000-000000000001', 'Seller', 1, 'Terrible experience. Item never arrived and seller was unresponsive.', NULL, NULL, 'None', NULL, '2026-03-11 10:04:30.159683+00', NULL, NULL, NULL, false);
INSERT INTO public.review VALUES ('cf257032-f5d1-4000-89af-62b18173e578', '71000000-0000-0000-0000-000000000004', 'e167368a-7042-45a4-8f1f-badba56b6b63', 'Buyer', '70000000-0000-0000-0000-000000000001', 'Seller', 4, 'Good product overall, but shipping took longer than expected.', 'Thank you for your feedback! We apologize for the shipping delay and will work to improve our delivery times.', '2026-03-17 10:04:30.262289+00', 'None', NULL, '2026-03-16 10:04:30.262289+00', NULL, NULL, NULL, false);
INSERT INTO public.review VALUES ('c193c70c-76d5-452a-9e52-2517ecf90372', '71000000-0000-0000-0000-000000000005', '741eb167-fb61-4b47-a35a-90720c4949f7', 'Buyer', '70000000-0000-0000-0000-000000000001', 'Seller', 3, 'Average product. Works as expected but nothing special.', 'We appreciate your honest review and will continue to improve our products.', '2026-03-18 08:04:30.365932+00', 'None', NULL, '2026-03-14 10:04:30.365932+00', NULL, NULL, NULL, false);
INSERT INTO public.review VALUES ('17480161-022c-4aa1-8897-cbf1964f78b9', '71000000-0000-0000-0000-000000000006', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Buyer', '70000000-0000-0000-0000-000000000001', 'Seller', 5, 'Amazing quality and fast delivery! Highly recommend this seller.', 'Thank you so much for your kind words! We really appreciate your business.', '2026-03-18 07:04:30.473542+00', 'None', NULL, '2026-03-12 10:04:30.473542+00', NULL, NULL, NULL, false);
INSERT INTO public.review VALUES ('b25c269d-249f-499c-971d-856d9220067f', '71000000-0000-0000-0000-000000000008', 'e8f83430-5d4d-4ac2-bb82-46797049535e', 'Buyer', '70000000-0000-0000-0000-000000000001', 'Seller', 4, 'Good value for money. Would buy again.', NULL, NULL, 'Pending', '2026-03-23 10:03:39.667752+00', '2026-03-18 02:04:30.680801+00', NULL, '2026-03-23 10:03:39.687177+00', '70000000-0000-0000-0000-000000000001', false);
INSERT INTO public.review VALUES ('2c3ffc14-c497-4e98-ad4a-9446b72f26e2', '71000000-0000-0000-0000-000000000007', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'Buyer', '70000000-0000-0000-0000-000000000001', 'Seller', 2, 'Item had some defects. Not satisfied with the purchase.', 'Sorry about that', '2026-03-23 10:04:05.094685+00', 'None', NULL, '2026-03-17 10:04:30.575824+00', NULL, '2026-03-23 10:04:05.095728+00', '70000000-0000-0000-0000-000000000001', false);


--
-- TOC entry 4571 (class 0 OID 33036)
-- Dependencies: 396
-- Data for Name: role; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4583 (class 0 OID 33140)
-- Dependencies: 408
-- Data for Name: role_permissions; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4587 (class 0 OID 33191)
-- Dependencies: 412
-- Data for Name: role_user; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4633 (class 0 OID 37518)
-- Dependencies: 460
-- Data for Name: sale_event; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.sale_event VALUES ('585d523c-31b1-4fa6-82fa-6ca359ffb9bd', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'aaa', 'aa', 'DiscountAndSaleEvent', 'Scheduled', '2026-03-23 15:34:00+00', '2026-03-30 15:34:00+00', false, true, true, NULL, '2026-03-23 15:05:16.55821+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false);
INSERT INTO public.sale_event VALUES ('0f6abdde-c96f-43b4-a48a-60dd0c4745db', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', 'Back to school', 'Sale for student', 'DiscountAndSaleEvent', 'Scheduled', '2026-03-23 15:49:00+00', '2026-03-30 15:49:00+00', true, true, true, NULL, '2026-03-23 15:20:52.417126+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', NULL, NULL, false);


--
-- TOC entry 4634 (class 0 OID 37529)
-- Dependencies: 461
-- Data for Name: sale_event_discount_tier; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.sale_event_discount_tier VALUES ('4d5594ae-3c8f-4e51-89cb-a315f88e8c8d', '585d523c-31b1-4fa6-82fa-6ca359ffb9bd', 'Percent', 10.97, 1, 'aaa');
INSERT INTO public.sale_event_discount_tier VALUES ('681969e7-4f1b-4398-b9e6-98078c124710', '0f6abdde-c96f-43b4-a48a-60dd0c4745db', 'Percent', 10.00, 1, NULL);


--
-- TOC entry 4631 (class 0 OID 37458)
-- Dependencies: 458
-- Data for Name: sale_event_discount_tiers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.sale_event_discount_tiers VALUES ('ec2f53ff-000b-4f14-b6f5-e81e0766a664', '5eb4a40c-01da-4df0-bd90-89f2aebb281e', 1, 1.00, 1, 'a');
INSERT INTO public.sale_event_discount_tiers VALUES ('b970de86-64fc-488d-9651-dac2599cfabc', '125d4b88-bdbe-4211-a971-c21a916b7fe2', 1, 2.00, 1, 'aa');
INSERT INTO public.sale_event_discount_tiers VALUES ('675bad0d-1462-40fb-8c6d-791fc2c4b878', '263e3985-e795-4258-a36b-2b4ff43205d9', 1, 11.00, 1, 'aa1');


--
-- TOC entry 4635 (class 0 OID 37539)
-- Dependencies: 462
-- Data for Name: sale_event_listing; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.sale_event_listing VALUES ('19553da5-75f3-44fd-94b6-3946d29d27b7', '585d523c-31b1-4fa6-82fa-6ca359ffb9bd', '4d5594ae-3c8f-4e51-89cb-a315f88e8c8d', '53c73084-1223-4a54-85f9-9cb1cf13828c');
INSERT INTO public.sale_event_listing VALUES ('2efca414-773e-4f2b-a72b-a0f555dde1f5', '0f6abdde-c96f-43b4-a48a-60dd0c4745db', '681969e7-4f1b-4398-b9e6-98078c124710', '1a7ed4c4-ff65-48cb-b4ea-c8d39d33db89');


--
-- TOC entry 4632 (class 0 OID 37468)
-- Dependencies: 459
-- Data for Name: sale_event_listings; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.sale_event_listings VALUES ('0c95bbe8-438f-4ac4-a1ff-e2246cce4be6', '5eb4a40c-01da-4df0-bd90-89f2aebb281e', 'ec2f53ff-000b-4f14-b6f5-e81e0766a664', '71000000-0000-0000-0000-00000000005c', '2026-03-23 12:03:31.459776+00');
INSERT INTO public.sale_event_listings VALUES ('e2c7e849-7dd9-4afe-818e-435ffc917ffd', '5eb4a40c-01da-4df0-bd90-89f2aebb281e', 'ec2f53ff-000b-4f14-b6f5-e81e0766a664', '67f00868-4f9d-4486-a63e-20d6e0a161b7', '2026-03-23 12:03:31.459308+00');
INSERT INTO public.sale_event_listings VALUES ('07b6a479-3490-46ba-8cab-60cf63dfd15d', '125d4b88-bdbe-4211-a971-c21a916b7fe2', 'b970de86-64fc-488d-9651-dac2599cfabc', '67f00868-4f9d-4486-a63e-20d6e0a161b7', '2026-03-23 12:04:13.785353+00');
INSERT INTO public.sale_event_listings VALUES ('de1b5d20-ec16-4bd0-8b1e-31b616d7e7b5', '263e3985-e795-4258-a36b-2b4ff43205d9', '675bad0d-1462-40fb-8c6d-791fc2c4b878', '67f00868-4f9d-4486-a63e-20d6e0a161b7', '2026-03-23 12:06:09.304349+00');


--
-- TOC entry 4628 (class 0 OID 37441)
-- Dependencies: 455
-- Data for Name: sale_event_performance_metrics; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4629 (class 0 OID 37446)
-- Dependencies: 456
-- Data for Name: sale_event_price_snapshots; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4630 (class 0 OID 37451)
-- Dependencies: 457
-- Data for Name: sale_events; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.sale_events VALUES ('5eb4a40c-01da-4df0-bd90-89f2aebb281e', 'aaa', NULL, '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2026-03-23 12:32:00+00', '2026-03-30 12:32:00+00', 1, NULL, false, true, true, NULL, 1, '2026-03-23 12:03:31.457902+00', NULL);
INSERT INTO public.sale_events VALUES ('125d4b88-bdbe-4211-a971-c21a916b7fe2', 'aa', NULL, '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2026-03-23 12:33:00+00', '2026-03-30 12:33:00+00', 1, NULL, false, true, true, NULL, 1, '2026-03-23 12:04:13.785318+00', NULL);
INSERT INTO public.sale_events VALUES ('263e3985-e795-4258-a36b-2b4ff43205d9', 'aaaaa', NULL, '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2026-03-23 12:35:00+00', '2026-03-30 12:35:00+00', 1, NULL, false, true, true, NULL, 1, '2026-03-23 12:06:09.304333+00', NULL);


--
-- TOC entry 4605 (class 0 OID 33509)
-- Dependencies: 430
-- Data for Name: seller_blocked_buyer; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4606 (class 0 OID 33520)
-- Dependencies: 431
-- Data for Name: seller_exempt_buyer; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4604 (class 0 OID 33487)
-- Dependencies: 429
-- Data for Name: seller_preference; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4636 (class 0 OID 37607)
-- Dependencies: 463
-- Data for Name: shipping_discounts; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.shipping_discounts VALUES ('ec3e9b5d-9a06-4206-b56d-a5c1cf935811', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'aaaa', 'aaaaaa', 100.00, 1, true, 111.00, '2026-03-23 16:27:00+00', '2026-03-24 16:27:00+00', true, '2026-03-23 16:27:42.233651+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false);


--
-- TOC entry 4592 (class 0 OID 33298)
-- Dependencies: 417
-- Data for Name: shipping_policy; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.shipping_policy VALUES ('d559b079-a7b8-4247-a2c4-25d8e50c2052', '65987e62-8992-4f8f-bb21-1e3eea0f453c', 22.00, 'USD', 1, false, '2026-03-17 04:13:50.90275+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', NULL, NULL, false, 'eBay', 'ABC');
INSERT INTO public.shipping_policy VALUES ('e507cbe7-4fc1-4e8a-a1d2-9a6e39fb0737', 'e43d2fb2-d880-465d-87b6-3f72f60ece73', 222.00, 'USD', 1, false, '2026-03-17 05:36:28.256075+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, 'eBay', 'ABC');
INSERT INTO public.shipping_policy VALUES ('9a96b788-5f61-47e1-981d-0c2d4a7dab3e', '5cfe4d19-77f5-4594-849f-577db1f97b92', 0.03, 'USD', 1, false, '2026-03-25 16:10:11.361129+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', NULL, NULL, false, 'eBay', 'ABC');


--
-- TOC entry 4572 (class 0 OID 33043)
-- Dependencies: 397
-- Data for Name: shipping_services; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.shipping_services VALUES ('5a4af094-9a6b-4d6f-9a19-9b5360f0a6ec', 'UPS', 'UPS Ground', 15.62, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 28 - Apr 2', false, 6, 3, 'Reliable ground service - Includes tracking', true, 'On eBay you save 21%', 'UPS_GROUND', 'ups-ground', false, '2025-01-01 00:00:00+00', 'seed');
INSERT INTO public.shipping_services VALUES ('6f7e3c0f-2bc6-4f1b-aa0b-4c1a9f76f950', 'USPS', 'USPS Ground Advantage', 11.45, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 28 - Apr 1', false, 5, 3, 'Max weight 70 lb - Max dimensions 130" (length + girth)', false, 'On eBay you save 28%', 'USPS_GROUND_ADVANTAGE', 'usps-ground', true, '2025-01-01 00:00:00+00', 'seed');
INSERT INTO public.shipping_services VALUES ('9e1f84fd-8c9c-459d-b2c5-bf6e47668f5d', 'FedEx', 'FedEx Ground Economy', 14.1, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 29 - Apr 3', false, 7, 4, '2-5 business days - Ideal for small parcels', true, 'On eBay you save 18%', 'FEDEX_GROUND_ECONOMY', 'fedex-ground', false, '2025-01-01 00:00:00+00', 'seed');
INSERT INTO public.shipping_services VALUES ('a1d9551e-5c5c-4ca6-9a0e-1aa855b77af7', 'USPS', 'USPS Priority Mail Flat Rate Legal Envelope', 9.05, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 27 - 31', false, 4, 2, 'Legal-size documents - Insured up to $100', true, 'On eBay you save 12%', 'USPS_PRIORITY_MAIL_FLAT_RATE_LEGAL_ENVELOPE', 'usps-priority-legal', false, '2025-01-01 00:00:00+00', 'seed');
INSERT INTO public.shipping_services VALUES ('c1d3c7f4-6ac1-4a7f-8a29-6dbaf9ecbb51', 'USPS', 'USPS Priority Mail Flat Rate Envelope', 8.75, 'USD', 'Up to $100.00', '2025-01-01 00:00:00+00', 'seed', 'Mar 27 - 31', false, 4, 2, 'Best for documents - Includes tracking', true, 'On eBay you save 13%', 'USPS_PRIORITY_MAIL_FLAT_RATE_ENVELOPE', 'usps-priority-flat', false, '2025-01-01 00:00:00+00', 'seed');


--
-- TOC entry 4593 (class 0 OID 33305)
-- Dependencies: 418
-- Data for Name: store; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.store VALUES ('65987e62-8992-4f8f-bb21-1e3eea0f453c', 'e167368a-7042-45a4-8f1f-badba56b6b63', 'Viet awn cut', 'viet-awn-cut', '123', '', '', 0, true, '2026-03-17 04:11:17.021816+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', '2026-03-17 04:15:18.454027+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', false, 'admin@foodsage.com', '+84323232212', NULL, NULL, '#c3b83c');
INSERT INTO public.store VALUES ('962d1163-aa65-4c31-872a-13893433b7da', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'Test456', 'test456', 'Demo', '', '', 0, true, '2026-03-19 03:10:00.031346+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', '2026-03-19 03:10:11.465685+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false, '', '', NULL, NULL, '#c53bf7');
INSERT INTO public.store VALUES ('e43d2fb2-d880-465d-87b6-3f72f60ece73', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'Test Store 123', 'test-store-123', 'This is a test store for UI bug investigation.', 'https://res.cloudinary.com/djmftornv/image/upload/v1774269843/ebay-clone/vitenam_1924d918-aa77-4343-8b96-3707a030de58.png', 'https://res.cloudinary.com/djmftornv/image/upload/v1774269851/ebay-clone/image-removebg-preview %2840%29_ed0a698b-885a-4f6b-b9af-7da2f88a9a71.png', 0, true, '2026-03-17 04:27:15.450997+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2026-03-23 12:44:25.712294+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', false, '', '', NULL, NULL, '#3b82f6');
INSERT INTO public.store VALUES ('5cfe4d19-77f5-4594-849f-577db1f97b92', '038a4659-83c4-456d-9bf9-e2036a53ad6c', 'xc', 'xc', 'cx', 'https://res.cloudinary.com/djmftornv/image/upload/v1774454973/ebay-clone/Gemini_Generated_Image_fhmrqwfhmrqwfhmr_be868c49-fc47-4295-8009-79367872a1e3.jpg', 'https://res.cloudinary.com/djmftornv/image/upload/v1774454983/ebay-clone/LongRiver-2903_50f23e98-4bf5-4950-9a3b-7f3619f1fa29.jpg', 1, true, '2026-03-25 12:50:07.327873+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', '2026-03-25 16:10:01.752778+00', '038a4659-83c4-456d-9bf9-e2036a53ad6c', false, 'vtrgiangg2903@gmail.com', '+84323232212', NULL, NULL, '#e62605');


--
-- TOC entry 4594 (class 0 OID 33312)
-- Dependencies: 419
-- Data for Name: store_subscription; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.store_subscription VALUES ('fcdbf605-2848-49d3-89ba-1965423fb4f0', '65987e62-8992-4f8f-bb21-1e3eea0f453c', 0, 0.00, 'USD', 12.90, 250, '2026-03-17 04:11:16.962456+00', NULL, 0);
INSERT INTO public.store_subscription VALUES ('4c914bbe-dd60-43c4-8d1d-f89997885188', 'e43d2fb2-d880-465d-87b6-3f72f60ece73', 0, 0.00, 'USD', 12.90, 250, '2026-03-17 04:27:15.431393+00', NULL, 0);
INSERT INTO public.store_subscription VALUES ('6e9da6c8-0b6a-427c-b249-707885b1a3ac', '962d1163-aa65-4c31-872a-13893433b7da', 0, 0.00, 'USD', 12.90, 250, '2026-03-19 03:10:00.000972+00', NULL, 0);
INSERT INTO public.store_subscription VALUES ('a6e8d43e-26f0-4283-837a-b5066aea402f', '5cfe4d19-77f5-4594-849f-577db1f97b92', 1, 21.95, 'USD', 10.90, 1000, '2026-03-25 12:50:07.244608+00', NULL, 0);


--
-- TOC entry 4615 (class 0 OID 36007)
-- Dependencies: 440
-- Data for Name: support_tickets; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.support_tickets VALUES ('550e8400-e29b-41d4-a716-446655440001', '70000000-0000-0000-0000-000000000001', 'Technical', 'Không thể upload hình ảnh sản phẩm', 'Tôi đang gặp vấn đề khi upload hình ảnh cho listing mới. Hệ thống báo lỗi "File too large" nhưng file chỉ có 2MB. Xin hỗ trợ giải quyết vấn đề này.', 'Open', '2026-03-19 16:06:40.345639+00', '2026-03-19 16:06:40.345639+00', false);
INSERT INTO public.support_tickets VALUES ('550e8400-e29b-41d4-a716-446655440002', '70000000-0000-0000-0000-000000000001', 'Billing', 'Thắc mắc về phí bán hàng', 'Tôi muốn hiểu rõ hơn về cách tính phí bán hàng trên platform. Có thể cung cấp bảng phí chi tiết không? Đặc biệt là phí cho các danh mục khác nhau.', 'Open', '2026-03-18 18:06:40.345639+00', '2026-03-18 18:06:40.345639+00', false);
INSERT INTO public.support_tickets VALUES ('550e8400-e29b-41d4-a716-446655440003', '70000000-0000-0000-0000-000000000001', 'Shipping', 'Vấn đề với dịch vụ vận chuyển', 'Khách hàng phản ánh không nhận được hàng nhưng tracking cho thấy đã giao thành công. Tôi cần hỗ trợ để xác minh tình trạng đơn hàng và giải quyết khiếu nại của khách.', 'Pending', '2026-03-16 18:06:40.345639+00', '2026-03-18 18:06:40.345639+00', false);
INSERT INTO public.support_tickets VALUES ('550e8400-e29b-41d4-a716-446655440004', '70000000-0000-0000-0000-000000000001', 'Account', 'Cập nhật thông tin tài khoản', 'Tôi cần thay đổi địa chỉ email liên kết với tài khoản seller và cập nhật thông tin thanh toán.', 'Resolved', '2026-03-12 18:06:40.345639+00', '2026-03-17 18:06:40.345639+00', false);
INSERT INTO public.support_tickets VALUES ('550e8400-e29b-41d4-a716-446655440005', '70000000-0000-0000-0000-000000000002', 'General', 'Hướng dẫn sử dụng tính năng Sale Event', 'Tôi muốn tạo một sale event cho các sản phẩm của mình nhưng không biết cách sử dụng. Có thể hướng dẫn chi tiết không?', 'Open', '2026-03-19 12:06:40.345639+00', '2026-03-19 12:06:40.345639+00', false);
INSERT INTO public.support_tickets VALUES ('550e8400-e29b-41d4-a716-446655440006', '70000000-0000-0000-0000-000000000002', 'Technical', 'Lỗi khi tạo listing template', 'Hệ thống báo lỗi khi tôi cố gắng lưu listing template. Error message: "Template validation failed". Cần hỗ trợ khắc phục.', 'Pending', '2026-03-17 18:06:40.345639+00', '2026-03-19 14:06:40.345639+00', false);
INSERT INTO public.support_tickets VALUES ('550e8400-e29b-41d4-a716-446655440007', '70000000-0000-0000-0000-000000000003', 'Returns', 'Xử lý yêu cầu return từ khách hàng', 'Khách hàng yêu cầu return sản phẩm sau 25 ngày mua. Return policy của tôi là 30 ngày. Làm thế nào để xử lý case này?', 'Open', '2026-03-19 06:06:40.345639+00', '2026-03-19 06:06:40.345639+00', false);
INSERT INTO public.support_tickets VALUES ('550e8400-e29b-41d4-a716-446655440008', '70000000-0000-0000-0000-000000000003', 'Billing', 'Câu hỏi về voucher và coupon', 'Tôi muốn tạo coupon cho khách hàng VIP nhưng không hiểu rõ về các loại discount. Có thể giải thích sự khác biệt giữa các loại coupon không?', 'Resolved', '2026-03-14 18:06:40.345639+00', '2026-03-18 18:06:40.345639+00', false);
INSERT INTO public.support_tickets VALUES ('68deaa3f-f804-4336-9e4a-45589ac1165c', '70000000-0000-0000-0000-000000000002', 'Technical Support', 'Restore access email', 'HHEHEHHEHEHEHEHHEHEHHEHEHE', 'Open', '2026-03-24 01:54:44.888503+00', NULL, false);


--
-- TOC entry 4573 (class 0 OID 33050)
-- Dependencies: 398
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."user" VALUES ('e167368a-7042-45a4-8f1f-badba56b6b63', 'vietbhhe187397@fpt.edu.vn', 'Bui Hoang Viet', 'vietbhhe187397@fpt.edu.vn', '$2a$11$VBzRQNcJ2KSSi4aHRLqdTujdVQj72m3DeJQvJbhNVKac946v9NoOy', true, false, 'BelowStandard', 0.00, '2026-03-17 04:08:45.027904+00', 'System', '2026-03-17 04:10:23.623765+00', 'e167368a-7042-45a4-8f1f-badba56b6b63', false, 'Ha noi', 'Vietnam', 'no', 'VietNam', 'Ha Noi', '10001', true, true, '+84362678790');
INSERT INTO public."user" VALUES ('c483385e-f1b9-40b4-9b95-e18f45e06a82', 'thinhndhe186687@fpt.edu.vn', 'Nguyễn Đức Thịnh', 'thinhndhe186687@fpt.edu.vn', '$2a$11$WnwGMii.QR8M3FRHcT0nBez6L2NGeyBklBttVngGmr.OLXEvOrwXG', true, true, 'BelowStandard', 0.00, '2026-03-14 14:59:36.532877+00', 'System', '2026-03-14 15:00:42.466291+00', 'c483385e-f1b9-40b4-9b95-e18f45e06a82', false, NULL, NULL, NULL, NULL, NULL, NULL, true, true, '+84868169332');
INSERT INTO public."user" VALUES ('70000000-0000-0000-0000-000000000001', 'demo.seller1@example.com', 'Alice Johnson', 'demo.seller1@example.com', '$2a$11$WnwGMii.QR8M3FRHcT0nBez6L2NGeyBklBttVngGmr.OLXEvOrwXG', true, true, 'TopRated', 11222.00, '2024-01-01 00:00:00+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('db062e86-b522-41c2-b3b9-df461d830de8', 'haingoc0217@gmail.com', 'Le Hai', 'haingoc0217@gmail.com', '$2a$11$ZXQjMSEi/6.5/.XY2I.qGOLY7tTXIm23nM990t7a5.oUzPttFz.De', true, false, 'BelowStandard', 0.00, '2026-03-20 05:21:58.468301+00', 'System', '2026-03-20 05:22:27.961263+00', 'db062e86-b522-41c2-b3b9-df461d830de8', false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('6a0bf519-a43b-4f4c-8029-350483e7ffae', 'haingoc02217@gmail.com', 'Le Hai', 'haingoc02217@gmail.com', '$2a$11$I8xv8HGedUT/lTYq1KSuuescLPIVUV1kY8waOam7csunwaTjMdTu6', false, false, 'BelowStandard', 0.00, '2026-03-20 14:25:17.41708+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('70000000-0000-0000-0000-000000000002', 'demo.seller2@example.com', 'Brian Carter', 'demo.seller2@example.com', '$2a$11$WnwGMii.QR8M3FRHcT0nBez6L2NGeyBklBttVngGmr.OLXEvOrwXG', true, true, 'TopRated', 13622.00, '2024-01-01 00:00:00+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('70000000-0000-0000-0000-000000000003', 'demo.seller3@example.com', 'Cecilia Gomez', 'demo.seller3@example.com', '$2a$11$WnwGMii.QR8M3FRHcT0nBez6L2NGeyBklBttVngGmr.OLXEvOrwXG', true, true, 'TopRated', 16022.00, '2024-01-01 00:00:00+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('741eb167-fb61-4b47-a35a-90720c4949f7', 'test@example.com', 'Test User', 'test@example.com', '$2a$11$WnwGMii.QR8M3FRHcT0nBez6L2NGeyBklBttVngGmr.OLXEvOrwXG', false, false, 'BelowStandard', 0.00, '2026-03-17 04:23:53.417305+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('87b82f07-53c2-46a4-bdde-f1589b3aa372', 'lengochai012004@gmail.com', 'haitest', 'lengochai012004@gmail.com', '$2a$11$XPwpajDwsEmeCPV1UlH1IOrlbFzrFUIM919cI3AVlCN2AxP/1DUOC', false, false, 'BelowStandard', 0.00, '2026-03-25 15:37:33.471827+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', 'hoapoki24@gmail.com', 'Do Huu Hoa', 'hoapoki24@gmail.com', '$2a$11$gEhJpCuRGD2XH0n.MnXtsuFI2nYsBXzuig7NLUV8OajwBWSpmUYc2', true, false, 'BelowStandard', 0.00, '2026-03-17 01:31:10.426807+00', 'System', '2026-03-17 01:32:36.914758+00', '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', false, NULL, NULL, NULL, NULL, NULL, NULL, true, true, '+84362982702');
INSERT INTO public."user" VALUES ('e8f83430-5d4d-4ac2-bb82-46797049535e', 'ngochai0217@gmail.com', 'Le Ngoc Hai', 'ngochai0217@gmail.com', '$2a$11$c9ebDeXWqx58qriiLDzjaeGEvu3ln0LP2ETijCv7mY2Oly29ti39O', false, false, 'BelowStandard', 0.00, '2026-03-17 02:26:04.977332+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);
INSERT INTO public."user" VALUES ('038a4659-83c4-456d-9bf9-e2036a53ad6c', 'vtrgiangg2903@gmail.com', 'Vu Truong Giang', 'vtrgiangg2903@gmail.com', '$2a$11$mRf3DlRtEJibC/PyvC04zu8dKBZbQ1WmC6hhYO50mUDwjesxz8Lda', true, true, 'BelowStandard', 0.00, '2026-03-11 01:05:42.625772+00', 'System', '2026-03-25 16:09:09.507427+00', 'System', false, 'Ha noi', 'Vietnam', 'no', 'VietNam', 'Ha Noi', '10001', true, true, '+84362678790');
INSERT INTO public."user" VALUES ('d9814bda-f007-413a-9d73-256bbf43ced4', 'aaa@gmail.com', 'aaaaaa', 'aaa@gmail.com', '$2a$11$irA4tgUu7gAxPVN4x1rqcu8YZ34p2.MeNhowyHb2jZ8i/3h4kv.5S', false, false, 'BelowStandard', 0.00, '2026-03-25 17:32:00.531581+00', 'System', NULL, NULL, false, NULL, NULL, NULL, NULL, NULL, NULL, false, false, NULL);


--
-- TOC entry 4581 (class 0 OID 33111)
-- Dependencies: 406
-- Data for Name: variation; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4638 (class 0 OID 37626)
-- Dependencies: 465
-- Data for Name: volume_pricing_tiers; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4637 (class 0 OID 37617)
-- Dependencies: 464
-- Data for Name: volume_pricings; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.volume_pricings VALUES ('0ced7559-7437-4cb6-8015-efa3b1985d84', 'd9814bda-f007-413a-9d73-256bbf43ced4', NULL, 'aaaaaaa', 'aaaaaaaaa', '2026-03-25 17:34:00+00', '2026-03-26 17:34:00+00', false, '2026-03-25 17:34:07.196506+00', 'd9814bda-f007-413a-9d73-256bbf43ced4', '2026-03-25 17:34:23.906257+00', 'd9814bda-f007-413a-9d73-256bbf43ced4', true);


--
-- TOC entry 4613 (class 0 OID 33615)
-- Dependencies: 438
-- Data for Name: voucher_transactions; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4612 (class 0 OID 33608)
-- Dependencies: 437
-- Data for Name: vouchers; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4733 (class 0 OID 0)
-- Dependencies: 401
-- Name: listing_image_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.listing_image_id_seq', 16, true);


--
-- TOC entry 4734 (class 0 OID 0)
-- Dependencies: 403
-- Name: listing_item_specific_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.listing_item_specific_id_seq', 58, true);


--
-- TOC entry 4735 (class 0 OID 0)
-- Dependencies: 405
-- Name: variation_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.variation_id_seq', 1, false);


--
-- TOC entry 4348 (class 2606 OID 37690)
-- Name: dispute_response dispute_response_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dispute_response
    ADD CONSTRAINT dispute_response_pkey PRIMARY KEY (id);


--
-- TOC entry 4357 (class 2606 OID 37714)
-- Name: notification notification_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.notification
    ADD CONSTRAINT notification_pkey PRIMARY KEY (id);


--
-- TOC entry 4090 (class 2606 OID 32411)
-- Name: __EFMigrationsHistory pk___ef_migrations_history; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id);


--
-- TOC entry 4278 (class 2606 OID 36202)
-- Name: applied_order_discounts pk_applied_order_discounts; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.applied_order_discounts
    ADD CONSTRAINT pk_applied_order_discounts PRIMARY KEY (id);


--
-- TOC entry 4306 (class 2606 OID 37440)
-- Name: applied_sale_events pk_applied_sale_events; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.applied_sale_events
    ADD CONSTRAINT pk_applied_sale_events PRIMARY KEY (id);


--
-- TOC entry 4298 (class 2606 OID 37394)
-- Name: bids pk_bids; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bids
    ADD CONSTRAINT pk_bids PRIMARY KEY (id);


--
-- TOC entry 4095 (class 2606 OID 32981)
-- Name: category pk_category; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT pk_category PRIMARY KEY (id);


--
-- TOC entry 4130 (class 2606 OID 33073)
-- Name: category_condition pk_category_condition; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_condition
    ADD CONSTRAINT pk_category_condition PRIMARY KEY (category_id, condition_id);


--
-- TOC entry 4127 (class 2606 OID 33063)
-- Name: category_specific pk_category_specific; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_specific
    ADD CONSTRAINT pk_category_specific PRIMARY KEY (id);


--
-- TOC entry 4097 (class 2606 OID 32993)
-- Name: condition pk_condition; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.condition
    ADD CONSTRAINT pk_condition PRIMARY KEY (id);


--
-- TOC entry 4207 (class 2606 OID 33416)
-- Name: coupon pk_coupon; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon
    ADD CONSTRAINT pk_coupon PRIMARY KEY (id);


--
-- TOC entry 4211 (class 2606 OID 33426)
-- Name: coupon_condition pk_coupon_condition; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_condition
    ADD CONSTRAINT pk_coupon_condition PRIMARY KEY (id);


--
-- TOC entry 4240 (class 2606 OID 33578)
-- Name: coupon_excluded_categories pk_coupon_excluded_categories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_excluded_categories
    ADD CONSTRAINT pk_coupon_excluded_categories PRIMARY KEY (id);


--
-- TOC entry 4243 (class 2606 OID 33590)
-- Name: coupon_excluded_items pk_coupon_excluded_items; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_excluded_items
    ADD CONSTRAINT pk_coupon_excluded_items PRIMARY KEY (id);


--
-- TOC entry 4246 (class 2606 OID 33602)
-- Name: coupon_target_audiences pk_coupon_target_audiences; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_target_audiences
    ADD CONSTRAINT pk_coupon_target_audiences PRIMARY KEY (id);


--
-- TOC entry 4204 (class 2606 OID 33408)
-- Name: coupon_type pk_coupon_type; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_type
    ADD CONSTRAINT pk_coupon_type PRIMARY KEY (id);


--
-- TOC entry 4231 (class 2606 OID 33542)
-- Name: dispute pk_dispute; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dispute
    ADD CONSTRAINT pk_dispute PRIMARY KEY (id);


--
-- TOC entry 4099 (class 2606 OID 33000)
-- Name: file_metadata pk_file_metadata; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.file_metadata
    ADD CONSTRAINT pk_file_metadata PRIMARY KEY (id);


--
-- TOC entry 4266 (class 2606 OID 36133)
-- Name: inventory pk_inventory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inventory
    ADD CONSTRAINT pk_inventory PRIMARY KEY (id);


--
-- TOC entry 4270 (class 2606 OID 36140)
-- Name: inventory_adjustment pk_inventory_adjustment; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inventory_adjustment
    ADD CONSTRAINT pk_inventory_adjustment PRIMARY KEY (id);


--
-- TOC entry 4274 (class 2606 OID 36145)
-- Name: inventory_reservation pk_inventory_reservation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inventory_reservation
    ADD CONSTRAINT pk_inventory_reservation PRIMARY KEY (id);


--
-- TOC entry 4105 (class 2606 OID 33007)
-- Name: listing pk_listing; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing
    ADD CONSTRAINT pk_listing PRIMARY KEY (id);


--
-- TOC entry 4146 (class 2606 OID 33156)
-- Name: listing_id pk_listing_id; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_id
    ADD CONSTRAINT pk_listing_id PRIMARY KEY (listing_id);


--
-- TOC entry 4132 (class 2606 OID 33091)
-- Name: listing_image pk_listing_image; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_image
    ADD CONSTRAINT pk_listing_image PRIMARY KEY (listing_id, id);


--
-- TOC entry 4134 (class 2606 OID 33104)
-- Name: listing_item_specific pk_listing_item_specific; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_item_specific
    ADD CONSTRAINT pk_listing_item_specific PRIMARY KEY (listing_id, id);


--
-- TOC entry 4108 (class 2606 OID 33014)
-- Name: listing_template pk_listing_template; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_template
    ADD CONSTRAINT pk_listing_template PRIMARY KEY (id);


--
-- TOC entry 4302 (class 2606 OID 37406)
-- Name: offers pk_offers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.offers
    ADD CONSTRAINT pk_offers PRIMARY KEY (id);


--
-- TOC entry 4214 (class 2606 OID 33439)
-- Name: order_buyer_feedback pk_order_buyer_feedback; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_buyer_feedback
    ADD CONSTRAINT pk_order_buyer_feedback PRIMARY KEY (id);


--
-- TOC entry 4187 (class 2606 OID 33337)
-- Name: order_cancellation_requests pk_order_cancellation_requests; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_cancellation_requests
    ADD CONSTRAINT pk_order_cancellation_requests PRIMARY KEY (id);


--
-- TOC entry 4287 (class 2606 OID 36225)
-- Name: order_discount_category_rules pk_order_discount_category_rules; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_discount_category_rules
    ADD CONSTRAINT pk_order_discount_category_rules PRIMARY KEY (id);


--
-- TOC entry 4291 (class 2606 OID 36235)
-- Name: order_discount_item_rules pk_order_discount_item_rules; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_discount_item_rules
    ADD CONSTRAINT pk_order_discount_item_rules PRIMARY KEY (id);


--
-- TOC entry 4280 (class 2606 OID 36211)
-- Name: order_discount_performance_metrics pk_order_discount_performance_metrics; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_discount_performance_metrics
    ADD CONSTRAINT pk_order_discount_performance_metrics PRIMARY KEY (id);


--
-- TOC entry 4294 (class 2606 OID 36245)
-- Name: order_discount_tiers pk_order_discount_tiers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_discount_tiers
    ADD CONSTRAINT pk_order_discount_tiers PRIMARY KEY (id);


--
-- TOC entry 4284 (class 2606 OID 36220)
-- Name: order_discounts pk_order_discounts; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_discounts
    ADD CONSTRAINT pk_order_discounts PRIMARY KEY (id);


--
-- TOC entry 4202 (class 2606 OID 33387)
-- Name: order_item_shipments pk_order_item_shipments; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_shipments
    ADD CONSTRAINT pk_order_item_shipments PRIMARY KEY (id);


--
-- TOC entry 4161 (class 2606 OID 33212)
-- Name: order_items pk_order_items; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT pk_order_items PRIMARY KEY (id);


--
-- TOC entry 4191 (class 2606 OID 33349)
-- Name: order_return_requests pk_order_return_requests; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_return_requests
    ADD CONSTRAINT pk_order_return_requests PRIMARY KEY (id);


--
-- TOC entry 4164 (class 2606 OID 33224)
-- Name: order_shipping_labels pk_order_shipping_labels; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_shipping_labels
    ADD CONSTRAINT pk_order_shipping_labels PRIMARY KEY (id);


--
-- TOC entry 4169 (class 2606 OID 33234)
-- Name: order_status_histories pk_order_status_histories; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_histories
    ADD CONSTRAINT pk_order_status_histories PRIMARY KEY (id);


--
-- TOC entry 4141 (class 2606 OID 33129)
-- Name: order_status_transitions pk_order_status_transitions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_transitions
    ADD CONSTRAINT pk_order_status_transitions PRIMARY KEY (id);


--
-- TOC entry 4111 (class 2606 OID 33021)
-- Name: order_statuses pk_order_statuses; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_statuses
    ADD CONSTRAINT pk_order_statuses PRIMARY KEY (id);


--
-- TOC entry 4151 (class 2606 OID 33168)
-- Name: orders pk_orders; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT pk_orders PRIMARY KEY (id);


--
-- TOC entry 4114 (class 2606 OID 33028)
-- Name: otp pk_otp; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.otp
    ADD CONSTRAINT pk_otp PRIMARY KEY (id);


--
-- TOC entry 4116 (class 2606 OID 33035)
-- Name: outbox_message pk_outbox_message; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.outbox_message
    ADD CONSTRAINT pk_outbox_message PRIMARY KEY (id);


--
-- TOC entry 4154 (class 2606 OID 33185)
-- Name: refresh_token pk_refresh_token; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.refresh_token
    ADD CONSTRAINT pk_refresh_token PRIMARY KEY (id);


--
-- TOC entry 4194 (class 2606 OID 33365)
-- Name: report_downloads pk_report_downloads; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.report_downloads
    ADD CONSTRAINT pk_report_downloads PRIMARY KEY (id);


--
-- TOC entry 4197 (class 2606 OID 33373)
-- Name: report_schedules pk_report_schedules; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.report_schedules
    ADD CONSTRAINT pk_report_schedules PRIMARY KEY (id);


--
-- TOC entry 4253 (class 2606 OID 34872)
-- Name: research_saved_category pk_research_saved_category; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.research_saved_category
    ADD CONSTRAINT pk_research_saved_category PRIMARY KEY (user_id, category_id);


--
-- TOC entry 4172 (class 2606 OID 33297)
-- Name: return_policy pk_return_policy; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.return_policy
    ADD CONSTRAINT pk_return_policy PRIMARY KEY (id);


--
-- TOC entry 4237 (class 2606 OID 33557)
-- Name: review pk_review; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT pk_review PRIMARY KEY (id);


--
-- TOC entry 4118 (class 2606 OID 33042)
-- Name: role pk_role; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role
    ADD CONSTRAINT pk_role PRIMARY KEY (id);


--
-- TOC entry 4143 (class 2606 OID 33146)
-- Name: role_permissions pk_role_permissions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_permissions
    ADD CONSTRAINT pk_role_permissions PRIMARY KEY (role_id, "Permission");


--
-- TOC entry 4157 (class 2606 OID 33195)
-- Name: role_user pk_role_user; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_user
    ADD CONSTRAINT pk_role_user PRIMARY KEY (roles_id, user_id);


--
-- TOC entry 4320 (class 2606 OID 37462)
-- Name: sale_event_discount_tiers pk_sale_event_discount_tiers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_discount_tiers
    ADD CONSTRAINT pk_sale_event_discount_tiers PRIMARY KEY (id);


--
-- TOC entry 4325 (class 2606 OID 37472)
-- Name: sale_event_listings pk_sale_event_listings; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listings
    ADD CONSTRAINT pk_sale_event_listings PRIMARY KEY (id);


--
-- TOC entry 4309 (class 2606 OID 37445)
-- Name: sale_event_performance_metrics pk_sale_event_performance_metrics; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_performance_metrics
    ADD CONSTRAINT pk_sale_event_performance_metrics PRIMARY KEY (id);


--
-- TOC entry 4312 (class 2606 OID 37450)
-- Name: sale_event_price_snapshots pk_sale_event_price_snapshots; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_price_snapshots
    ADD CONSTRAINT pk_sale_event_price_snapshots PRIMARY KEY (id);


--
-- TOC entry 4317 (class 2606 OID 37457)
-- Name: sale_events pk_sale_events; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_events
    ADD CONSTRAINT pk_sale_events PRIMARY KEY (id);


--
-- TOC entry 4221 (class 2606 OID 33514)
-- Name: seller_blocked_buyer pk_seller_blocked_buyer; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_blocked_buyer
    ADD CONSTRAINT pk_seller_blocked_buyer PRIMARY KEY (id);


--
-- TOC entry 4225 (class 2606 OID 33525)
-- Name: seller_exempt_buyer pk_seller_exempt_buyer; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_exempt_buyer
    ADD CONSTRAINT pk_seller_exempt_buyer PRIMARY KEY (id);


--
-- TOC entry 4217 (class 2606 OID 33508)
-- Name: seller_preference pk_seller_preference; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_preference
    ADD CONSTRAINT pk_seller_preference PRIMARY KEY (id);


--
-- TOC entry 4338 (class 2606 OID 37616)
-- Name: shipping_discounts pk_shipping_discounts; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.shipping_discounts
    ADD CONSTRAINT pk_shipping_discounts PRIMARY KEY (id);


--
-- TOC entry 4175 (class 2606 OID 33304)
-- Name: shipping_policy pk_shipping_policy; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.shipping_policy
    ADD CONSTRAINT pk_shipping_policy PRIMARY KEY (id);


--
-- TOC entry 4121 (class 2606 OID 33049)
-- Name: shipping_services pk_shipping_services; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.shipping_services
    ADD CONSTRAINT pk_shipping_services PRIMARY KEY (id);


--
-- TOC entry 4179 (class 2606 OID 33311)
-- Name: store pk_store; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.store
    ADD CONSTRAINT pk_store PRIMARY KEY (id);


--
-- TOC entry 4183 (class 2606 OID 33316)
-- Name: store_subscription pk_store_subscription; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.store_subscription
    ADD CONSTRAINT pk_store_subscription PRIMARY KEY (id);


--
-- TOC entry 4124 (class 2606 OID 33056)
-- Name: user pk_user; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT pk_user PRIMARY KEY (id);


--
-- TOC entry 4137 (class 2606 OID 33117)
-- Name: variation pk_variation; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.variation
    ADD CONSTRAINT pk_variation PRIMARY KEY (id);


--
-- TOC entry 4346 (class 2606 OID 37630)
-- Name: volume_pricing_tiers pk_volume_pricing_tiers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volume_pricing_tiers
    ADD CONSTRAINT pk_volume_pricing_tiers PRIMARY KEY (id);


--
-- TOC entry 4343 (class 2606 OID 37625)
-- Name: volume_pricings pk_volume_pricings; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volume_pricings
    ADD CONSTRAINT pk_volume_pricings PRIMARY KEY (id);


--
-- TOC entry 4251 (class 2606 OID 33621)
-- Name: voucher_transactions pk_voucher_transactions; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.voucher_transactions
    ADD CONSTRAINT pk_voucher_transactions PRIMARY KEY (id);


--
-- TOC entry 4248 (class 2606 OID 33614)
-- Name: vouchers pk_vouchers; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.vouchers
    ADD CONSTRAINT pk_vouchers PRIMARY KEY (id);


--
-- TOC entry 4330 (class 2606 OID 37533)
-- Name: sale_event_discount_tier sale_event_discount_tier_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_discount_tier
    ADD CONSTRAINT sale_event_discount_tier_pkey PRIMARY KEY (id);


--
-- TOC entry 4334 (class 2606 OID 37543)
-- Name: sale_event_listing sale_event_listing_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listing
    ADD CONSTRAINT sale_event_listing_pkey PRIMARY KEY (id);


--
-- TOC entry 4327 (class 2606 OID 37528)
-- Name: sale_event sale_event_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event
    ADD CONSTRAINT sale_event_pkey PRIMARY KEY (id);


--
-- TOC entry 4261 (class 2606 OID 36018)
-- Name: support_tickets support_tickets_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.support_tickets
    ADD CONSTRAINT support_tickets_pkey PRIMARY KEY (id);


--
-- TOC entry 4184 (class 1259 OID 33355)
-- Name: idx_cancellation_requests_order; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_cancellation_requests_order ON public.order_cancellation_requests USING btree (order_id);


--
-- TOC entry 4185 (class 1259 OID 33356)
-- Name: idx_cancellation_requests_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_cancellation_requests_status ON public.order_cancellation_requests USING btree (status);


--
-- TOC entry 4349 (class 1259 OID 37703)
-- Name: idx_dispute_response_created_at; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_dispute_response_created_at ON public.dispute_response USING btree (created_at DESC);


--
-- TOC entry 4350 (class 1259 OID 37701)
-- Name: idx_dispute_response_dispute_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_dispute_response_dispute_id ON public.dispute_response USING btree (dispute_id);


--
-- TOC entry 4351 (class 1259 OID 37702)
-- Name: idx_dispute_response_responder_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_dispute_response_responder_id ON public.dispute_response USING btree (responder_id);


--
-- TOC entry 4268 (class 1259 OID 36155)
-- Name: idx_inventory_adjustment_inventory_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_inventory_adjustment_inventory_id ON public.inventory_adjustment USING btree (inventory_id);


--
-- TOC entry 4262 (class 1259 OID 36151)
-- Name: idx_inventory_is_low_stock; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_inventory_is_low_stock ON public.inventory USING btree (seller_id, is_low_stock);


--
-- TOC entry 4271 (class 1259 OID 36156)
-- Name: idx_inventory_reservation_expires_at; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_inventory_reservation_expires_at ON public.inventory_reservation USING btree (expires_at);


--
-- TOC entry 4263 (class 1259 OID 36152)
-- Name: idx_inventory_seller_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_inventory_seller_id ON public.inventory USING btree (seller_id);


--
-- TOC entry 4264 (class 1259 OID 36153)
-- Name: idx_inventory_updated_at; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_inventory_updated_at ON public.inventory USING btree (last_updated_at DESC);


--
-- TOC entry 4100 (class 1259 OID 33272)
-- Name: idx_listing_active_owner_sort; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_listing_active_owner_sort ON public.listing USING btree (created_by, start_date DESC, created_at DESC, id, category_id, format) WHERE (status = 3);


--
-- TOC entry 4101 (class 1259 OID 33273)
-- Name: idx_listing_owner_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_listing_owner_status ON public.listing USING btree (created_by, status);


--
-- TOC entry 4102 (class 1259 OID 33274)
-- Name: idx_listing_sku_trgm; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_listing_sku_trgm ON public.listing USING gin (sku public.gin_trgm_ops);


--
-- TOC entry 4103 (class 1259 OID 33275)
-- Name: idx_listing_title_trgm; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_listing_title_trgm ON public.listing USING gin (title public.gin_trgm_ops);


--
-- TOC entry 4352 (class 1259 OID 37715)
-- Name: idx_notification_user_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_notification_user_id ON public.notification USING btree (user_id);


--
-- TOC entry 4353 (class 1259 OID 37716)
-- Name: idx_notification_user_unread; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_notification_user_unread ON public.notification USING btree (user_id, is_read);


--
-- TOC entry 4158 (class 1259 OID 33271)
-- Name: idx_order_items_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_order_items_listing_id ON public.order_items USING btree (listing_id);


--
-- TOC entry 4170 (class 1259 OID 33322)
-- Name: idx_return_policy_store_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_return_policy_store_id ON public.return_policy USING btree (store_id);


--
-- TOC entry 4188 (class 1259 OID 33357)
-- Name: idx_return_requests_order; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_return_requests_order ON public.order_return_requests USING btree (order_id);


--
-- TOC entry 4189 (class 1259 OID 33358)
-- Name: idx_return_requests_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_return_requests_status ON public.order_return_requests USING btree (status);


--
-- TOC entry 4173 (class 1259 OID 33323)
-- Name: idx_shipping_policy_store_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_shipping_policy_store_id ON public.shipping_policy USING btree (store_id);


--
-- TOC entry 4176 (class 1259 OID 33324)
-- Name: idx_store_slug; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX idx_store_slug ON public.store USING btree (slug);


--
-- TOC entry 4180 (class 1259 OID 33326)
-- Name: idx_store_subscription_store_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_store_subscription_store_id ON public.store_subscription USING btree (store_id);


--
-- TOC entry 4181 (class 1259 OID 33327)
-- Name: idx_store_subscription_store_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_store_subscription_store_status ON public.store_subscription USING btree (store_id, status);


--
-- TOC entry 4177 (class 1259 OID 33325)
-- Name: idx_store_user_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_store_user_id ON public.store USING btree (user_id);


--
-- TOC entry 4254 (class 1259 OID 36021)
-- Name: idx_support_tickets_category; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_support_tickets_category ON public.support_tickets USING btree (category);


--
-- TOC entry 4255 (class 1259 OID 36022)
-- Name: idx_support_tickets_created_at; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_support_tickets_created_at ON public.support_tickets USING btree (created_at);


--
-- TOC entry 4256 (class 1259 OID 36023)
-- Name: idx_support_tickets_is_deleted; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_support_tickets_is_deleted ON public.support_tickets USING btree (is_deleted);


--
-- TOC entry 4257 (class 1259 OID 36019)
-- Name: idx_support_tickets_seller_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_support_tickets_seller_id ON public.support_tickets USING btree (seller_id);


--
-- TOC entry 4258 (class 1259 OID 36024)
-- Name: idx_support_tickets_seller_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_support_tickets_seller_status ON public.support_tickets USING btree (seller_id, status) WHERE (is_deleted = false);


--
-- TOC entry 4259 (class 1259 OID 36020)
-- Name: idx_support_tickets_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_support_tickets_status ON public.support_tickets USING btree (status);


--
-- TOC entry 4135 (class 1259 OID 33270)
-- Name: idx_variation_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_variation_listing_id ON public.variation USING btree (listing_id);


--
-- TOC entry 4275 (class 1259 OID 36251)
-- Name: ix_applied_order_discounts_order_discount_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_applied_order_discounts_order_discount_id ON public.applied_order_discounts USING btree (order_discount_id);


--
-- TOC entry 4276 (class 1259 OID 36252)
-- Name: ix_applied_order_discounts_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_applied_order_discounts_order_id ON public.applied_order_discounts USING btree (order_id);


--
-- TOC entry 4303 (class 1259 OID 37483)
-- Name: ix_applied_sale_events_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_applied_sale_events_order_id ON public.applied_sale_events USING btree (order_id);


--
-- TOC entry 4304 (class 1259 OID 37484)
-- Name: ix_applied_sale_events_sale_event_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_applied_sale_events_sale_event_id ON public.applied_sale_events USING btree (sale_event_id);


--
-- TOC entry 4295 (class 1259 OID 37412)
-- Name: ix_bids_bidder_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_bids_bidder_id ON public.bids USING btree (bidder_id);


--
-- TOC entry 4296 (class 1259 OID 37413)
-- Name: ix_bids_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_bids_listing_id ON public.bids USING btree (listing_id);


--
-- TOC entry 4128 (class 1259 OID 33251)
-- Name: ix_category_condition_condition_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_category_condition_condition_id ON public.category_condition USING btree (condition_id);


--
-- TOC entry 4091 (class 1259 OID 38860)
-- Name: ix_category_name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_category_name ON public.category USING btree (name);


--
-- TOC entry 4092 (class 1259 OID 33250)
-- Name: ix_category_parent_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_category_parent_id ON public.category USING btree (parent_id);


--
-- TOC entry 4093 (class 1259 OID 38859)
-- Name: ix_category_parent_id_is_deleted; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_category_parent_id_is_deleted ON public.category USING btree (parent_id, is_deleted);


--
-- TOC entry 4125 (class 1259 OID 33252)
-- Name: ix_category_specific_category_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_category_specific_category_id ON public.category_specific USING btree (category_id);


--
-- TOC entry 4209 (class 1259 OID 33434)
-- Name: ix_coupon_condition_coupon_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_condition_coupon_id ON public.coupon_condition USING btree (coupon_id);


--
-- TOC entry 4205 (class 1259 OID 33432)
-- Name: ix_coupon_coupon_type_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_coupon_type_id ON public.coupon USING btree (coupon_type_id);


--
-- TOC entry 4238 (class 1259 OID 33627)
-- Name: ix_coupon_excluded_categories_coupon_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_excluded_categories_coupon_id ON public.coupon_excluded_categories USING btree (coupon_id);


--
-- TOC entry 4241 (class 1259 OID 33628)
-- Name: ix_coupon_excluded_items_coupon_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_excluded_items_coupon_id ON public.coupon_excluded_items USING btree (coupon_id);


--
-- TOC entry 4244 (class 1259 OID 33629)
-- Name: ix_coupon_target_audiences_coupon_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_coupon_target_audiences_coupon_id ON public.coupon_target_audiences USING btree (coupon_id);


--
-- TOC entry 4227 (class 1259 OID 33563)
-- Name: ix_dispute_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_dispute_listing_id ON public.dispute USING btree (listing_id);


--
-- TOC entry 4228 (class 1259 OID 33564)
-- Name: ix_dispute_raised_by_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_dispute_raised_by_id ON public.dispute USING btree (raised_by_id);


--
-- TOC entry 4229 (class 1259 OID 33565)
-- Name: ix_dispute_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_dispute_status ON public.dispute USING btree (status);


--
-- TOC entry 4272 (class 1259 OID 36157)
-- Name: ix_inventory_reservation_inventory_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_inventory_reservation_inventory_id ON public.inventory_reservation USING btree (inventory_id);


--
-- TOC entry 4144 (class 1259 OID 33253)
-- Name: ix_listing_id_seller_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_listing_id_seller_id ON public.listing_id USING btree (seller_id);


--
-- TOC entry 4106 (class 1259 OID 33254)
-- Name: ix_listing_template_name; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_listing_template_name ON public.listing_template USING btree (name);


--
-- TOC entry 4354 (class 1259 OID 38857)
-- Name: ix_notification_user_id_created_at; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_notification_user_id_created_at ON public.notification USING btree (user_id, created_at DESC);


--
-- TOC entry 4355 (class 1259 OID 38858)
-- Name: ix_notification_user_id_is_read; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_notification_user_id_is_read ON public.notification USING btree (user_id, is_read);


--
-- TOC entry 4299 (class 1259 OID 37414)
-- Name: ix_offers_buyer_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_offers_buyer_id ON public.offers USING btree (buyer_id);


--
-- TOC entry 4300 (class 1259 OID 37415)
-- Name: ix_offers_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_offers_listing_id ON public.offers USING btree (listing_id);


--
-- TOC entry 4212 (class 1259 OID 33450)
-- Name: ix_order_buyer_feedback_buyer_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_buyer_feedback_buyer_id ON public.order_buyer_feedback USING btree (buyer_id);


--
-- TOC entry 4285 (class 1259 OID 36253)
-- Name: ix_order_discount_category_rules_order_discount_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_discount_category_rules_order_discount_id ON public.order_discount_category_rules USING btree (order_discount_id);


--
-- TOC entry 4288 (class 1259 OID 36254)
-- Name: ix_order_discount_item_rules_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_discount_item_rules_listing_id ON public.order_discount_item_rules USING btree (listing_id);


--
-- TOC entry 4289 (class 1259 OID 36255)
-- Name: ix_order_discount_item_rules_order_discount_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_discount_item_rules_order_discount_id ON public.order_discount_item_rules USING btree (order_discount_id);


--
-- TOC entry 4292 (class 1259 OID 36256)
-- Name: ix_order_discount_tiers_order_discount_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_discount_tiers_order_discount_id ON public.order_discount_tiers USING btree (order_discount_id);


--
-- TOC entry 4281 (class 1259 OID 36257)
-- Name: ix_order_discounts_active; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_discounts_active ON public.order_discounts USING btree (is_active, start_date, end_date);


--
-- TOC entry 4282 (class 1259 OID 36258)
-- Name: ix_order_discounts_seller_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_discounts_seller_id ON public.order_discounts USING btree (seller_id);


--
-- TOC entry 4198 (class 1259 OID 33398)
-- Name: ix_order_item_shipments_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_item_shipments_order_id ON public.order_item_shipments USING btree (order_id);


--
-- TOC entry 4199 (class 1259 OID 33399)
-- Name: ix_order_item_shipments_order_item_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_item_shipments_order_item_id ON public.order_item_shipments USING btree (order_item_id);


--
-- TOC entry 4200 (class 1259 OID 33400)
-- Name: ix_order_item_shipments_shipping_label_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_item_shipments_shipping_label_id ON public.order_item_shipments USING btree (shipping_label_id);


--
-- TOC entry 4159 (class 1259 OID 33255)
-- Name: ix_order_items_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_items_order_id ON public.order_items USING btree (order_id);


--
-- TOC entry 4162 (class 1259 OID 33256)
-- Name: ix_order_shipping_labels_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_shipping_labels_order_id ON public.order_shipping_labels USING btree (order_id);


--
-- TOC entry 4165 (class 1259 OID 33257)
-- Name: ix_order_status_histories_from_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_histories_from_status_id ON public.order_status_histories USING btree (from_status_id);


--
-- TOC entry 4166 (class 1259 OID 33258)
-- Name: ix_order_status_histories_order_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_histories_order_id ON public.order_status_histories USING btree (order_id);


--
-- TOC entry 4167 (class 1259 OID 33259)
-- Name: ix_order_status_histories_to_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_histories_to_status_id ON public.order_status_histories USING btree (to_status_id);


--
-- TOC entry 4138 (class 1259 OID 33260)
-- Name: ix_order_status_transitions_from_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_transitions_from_status_id ON public.order_status_transitions USING btree (from_status_id);


--
-- TOC entry 4139 (class 1259 OID 33261)
-- Name: ix_order_status_transitions_to_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_order_status_transitions_to_status_id ON public.order_status_transitions USING btree (to_status_id);


--
-- TOC entry 4109 (class 1259 OID 33262)
-- Name: ix_order_statuses_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_order_statuses_code ON public.order_statuses USING btree (code);


--
-- TOC entry 4147 (class 1259 OID 33263)
-- Name: ix_orders_buyer_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_orders_buyer_id ON public.orders USING btree (buyer_id);


--
-- TOC entry 4148 (class 1259 OID 33264)
-- Name: ix_orders_order_number; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_orders_order_number ON public.orders USING btree (order_number);


--
-- TOC entry 4149 (class 1259 OID 33265)
-- Name: ix_orders_status_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_orders_status_id ON public.orders USING btree (status_id);


--
-- TOC entry 4112 (class 1259 OID 33266)
-- Name: ix_otp_email_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_otp_email_code ON public.otp USING btree (email, code);


--
-- TOC entry 4152 (class 1259 OID 33267)
-- Name: ix_refresh_token_user_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_refresh_token_user_id ON public.refresh_token USING btree (user_id);


--
-- TOC entry 4192 (class 1259 OID 33374)
-- Name: ix_report_downloads_user_id_reference_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_report_downloads_user_id_reference_code ON public.report_downloads USING btree (user_id, reference_code);


--
-- TOC entry 4195 (class 1259 OID 33375)
-- Name: ix_report_schedules_user_id_source_type_is_active; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_report_schedules_user_id_source_type_is_active ON public.report_schedules USING btree (user_id, source, type, is_active);


--
-- TOC entry 4232 (class 1259 OID 33566)
-- Name: ix_review_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_review_listing_id ON public.review USING btree (listing_id);


--
-- TOC entry 4233 (class 1259 OID 33567)
-- Name: ix_review_recipient_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_review_recipient_id ON public.review USING btree (recipient_id);


--
-- TOC entry 4234 (class 1259 OID 33568)
-- Name: ix_review_recipient_role; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_review_recipient_role ON public.review USING btree (recipient_role);


--
-- TOC entry 4235 (class 1259 OID 33569)
-- Name: ix_review_reviewer_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_review_reviewer_id ON public.review USING btree (reviewer_id);


--
-- TOC entry 4155 (class 1259 OID 33268)
-- Name: ix_role_user_user_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_role_user_user_id ON public.role_user USING btree (user_id);


--
-- TOC entry 4328 (class 1259 OID 37554)
-- Name: ix_sale_event_discount_tier_sale_event_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_event_discount_tier_sale_event_id ON public.sale_event_discount_tier USING btree (sale_event_id);


--
-- TOC entry 4318 (class 1259 OID 37485)
-- Name: ix_sale_event_discount_tiers_sale_event_id_priority; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_sale_event_discount_tiers_sale_event_id_priority ON public.sale_event_discount_tiers USING btree (sale_event_id, priority);


--
-- TOC entry 4331 (class 1259 OID 37555)
-- Name: ix_sale_event_listing_discount_tier_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_event_listing_discount_tier_id ON public.sale_event_listing USING btree (discount_tier_id);


--
-- TOC entry 4332 (class 1259 OID 37556)
-- Name: ix_sale_event_listing_sale_event_id_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_sale_event_listing_sale_event_id_listing_id ON public.sale_event_listing USING btree (sale_event_id, listing_id);


--
-- TOC entry 4321 (class 1259 OID 37486)
-- Name: ix_sale_event_listings_discount_tier_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_event_listings_discount_tier_id ON public.sale_event_listings USING btree (discount_tier_id);


--
-- TOC entry 4322 (class 1259 OID 37487)
-- Name: ix_sale_event_listings_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_event_listings_listing_id ON public.sale_event_listings USING btree (listing_id);


--
-- TOC entry 4323 (class 1259 OID 37488)
-- Name: ix_sale_event_listings_sale_event_id_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_sale_event_listings_sale_event_id_listing_id ON public.sale_event_listings USING btree (sale_event_id, listing_id);


--
-- TOC entry 4307 (class 1259 OID 37489)
-- Name: ix_sale_event_performance_metrics_sale_event_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_sale_event_performance_metrics_sale_event_id ON public.sale_event_performance_metrics USING btree (sale_event_id);


--
-- TOC entry 4310 (class 1259 OID 37490)
-- Name: ix_sale_event_price_snapshots_sale_event_id_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_sale_event_price_snapshots_sale_event_id_listing_id ON public.sale_event_price_snapshots USING btree (sale_event_id, listing_id);


--
-- TOC entry 4313 (class 1259 OID 37491)
-- Name: ix_sale_events_seller_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_events_seller_id ON public.sale_events USING btree (seller_id);


--
-- TOC entry 4314 (class 1259 OID 37492)
-- Name: ix_sale_events_start_date_end_date; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_events_start_date_end_date ON public.sale_events USING btree (start_date, end_date);


--
-- TOC entry 4315 (class 1259 OID 37493)
-- Name: ix_sale_events_status; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_sale_events_status ON public.sale_events USING btree (status);


--
-- TOC entry 4219 (class 1259 OID 33531)
-- Name: ix_seller_blocked_buyer_seller_preference_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_seller_blocked_buyer_seller_preference_id ON public.seller_blocked_buyer USING btree (seller_preference_id);


--
-- TOC entry 4223 (class 1259 OID 33533)
-- Name: ix_seller_exempt_buyer_seller_preference_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_seller_exempt_buyer_seller_preference_id ON public.seller_exempt_buyer USING btree (seller_preference_id);


--
-- TOC entry 4335 (class 1259 OID 37636)
-- Name: ix_shipping_discounts_active; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_shipping_discounts_active ON public.shipping_discounts USING btree (is_active, start_date, end_date);


--
-- TOC entry 4336 (class 1259 OID 37637)
-- Name: ix_shipping_discounts_seller_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_shipping_discounts_seller_id ON public.shipping_discounts USING btree (seller_id);


--
-- TOC entry 4119 (class 1259 OID 33290)
-- Name: ix_shipping_services_slug; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_shipping_services_slug ON public.shipping_services USING btree (slug);


--
-- TOC entry 4122 (class 1259 OID 33269)
-- Name: ix_user_email; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ix_user_email ON public."user" USING btree (email);


--
-- TOC entry 4344 (class 1259 OID 37638)
-- Name: ix_volume_pricing_tiers_volume_pricing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_volume_pricing_tiers_volume_pricing_id ON public.volume_pricing_tiers USING btree (volume_pricing_id);


--
-- TOC entry 4339 (class 1259 OID 37639)
-- Name: ix_volume_pricings_active; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_volume_pricings_active ON public.volume_pricings USING btree (is_active, start_date, end_date);


--
-- TOC entry 4340 (class 1259 OID 37640)
-- Name: ix_volume_pricings_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_volume_pricings_listing_id ON public.volume_pricings USING btree (listing_id);


--
-- TOC entry 4341 (class 1259 OID 37641)
-- Name: ix_volume_pricings_seller_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_volume_pricings_seller_id ON public.volume_pricings USING btree (seller_id);


--
-- TOC entry 4249 (class 1259 OID 33630)
-- Name: ix_voucher_transactions_voucher_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX ix_voucher_transactions_voucher_id ON public.voucher_transactions USING btree (voucher_id);


--
-- TOC entry 4267 (class 1259 OID 36154)
-- Name: uk_inventory_listing_id; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX uk_inventory_listing_id ON public.inventory USING btree (listing_id);


--
-- TOC entry 4215 (class 1259 OID 33451)
-- Name: ux_buyer_feedback_order; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_buyer_feedback_order ON public.order_buyer_feedback USING btree (order_id);


--
-- TOC entry 4208 (class 1259 OID 33433)
-- Name: ux_coupon_code; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_coupon_code ON public.coupon USING btree (code);


--
-- TOC entry 4222 (class 1259 OID 33532)
-- Name: ux_seller_blocked_buyer_identifier; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_seller_blocked_buyer_identifier ON public.seller_blocked_buyer USING btree (normalized_identifier, seller_preference_id);


--
-- TOC entry 4226 (class 1259 OID 33534)
-- Name: ux_seller_exempt_buyer_identifier; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_seller_exempt_buyer_identifier ON public.seller_exempt_buyer USING btree (normalized_identifier, seller_preference_id);


--
-- TOC entry 4218 (class 1259 OID 33535)
-- Name: ux_seller_preference_seller; Type: INDEX; Schema: public; Owner: postgres
--

CREATE UNIQUE INDEX ux_seller_preference_seller ON public.seller_preference USING btree (seller_id);


--
-- TOC entry 4412 (class 2620 OID 36026)
-- Name: support_tickets trigger_support_tickets_updated_at; Type: TRIGGER; Schema: public; Owner: postgres
--

CREATE TRIGGER trigger_support_tickets_updated_at BEFORE UPDATE ON public.support_tickets FOR EACH ROW EXECUTE FUNCTION public.update_support_tickets_updated_at();


--
-- TOC entry 4401 (class 2606 OID 37395)
-- Name: bids fk_bids_listings_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bids
    ADD CONSTRAINT fk_bids_listings_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4358 (class 2606 OID 32982)
-- Name: category fk_category_category_parent_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category
    ADD CONSTRAINT fk_category_category_parent_id FOREIGN KEY (parent_id) REFERENCES public.category(id) ON DELETE RESTRICT;


--
-- TOC entry 4360 (class 2606 OID 33074)
-- Name: category_condition fk_category_condition_categories_category_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_condition
    ADD CONSTRAINT fk_category_condition_categories_category_id FOREIGN KEY (category_id) REFERENCES public.category(id) ON DELETE CASCADE;


--
-- TOC entry 4361 (class 2606 OID 33079)
-- Name: category_condition fk_category_condition_condition_condition_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_condition
    ADD CONSTRAINT fk_category_condition_condition_condition_id FOREIGN KEY (condition_id) REFERENCES public.condition(id) ON DELETE CASCADE;


--
-- TOC entry 4359 (class 2606 OID 33064)
-- Name: category_specific fk_category_specific_category_category_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.category_specific
    ADD CONSTRAINT fk_category_specific_category_category_id FOREIGN KEY (category_id) REFERENCES public.category(id);


--
-- TOC entry 4386 (class 2606 OID 33427)
-- Name: coupon_condition fk_coupon_condition_coupons_coupon_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_condition
    ADD CONSTRAINT fk_coupon_condition_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES public.coupon(id) ON DELETE CASCADE;


--
-- TOC entry 4385 (class 2606 OID 33417)
-- Name: coupon fk_coupon_coupon_types_coupon_type_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon
    ADD CONSTRAINT fk_coupon_coupon_types_coupon_type_id FOREIGN KEY (coupon_type_id) REFERENCES public.coupon_type(id) ON DELETE RESTRICT;


--
-- TOC entry 4393 (class 2606 OID 33579)
-- Name: coupon_excluded_categories fk_coupon_excluded_categories_coupons_coupon_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_excluded_categories
    ADD CONSTRAINT fk_coupon_excluded_categories_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES public.coupon(id) ON DELETE CASCADE;


--
-- TOC entry 4394 (class 2606 OID 33591)
-- Name: coupon_excluded_items fk_coupon_excluded_items_coupons_coupon_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_excluded_items
    ADD CONSTRAINT fk_coupon_excluded_items_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES public.coupon(id) ON DELETE CASCADE;


--
-- TOC entry 4395 (class 2606 OID 33603)
-- Name: coupon_target_audiences fk_coupon_target_audiences_coupons_coupon_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.coupon_target_audiences
    ADD CONSTRAINT fk_coupon_target_audiences_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES public.coupon(id) ON DELETE CASCADE;


--
-- TOC entry 4391 (class 2606 OID 33543)
-- Name: dispute fk_dispute_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dispute
    ADD CONSTRAINT fk_dispute_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4410 (class 2606 OID 37691)
-- Name: dispute_response fk_dispute_response_dispute; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dispute_response
    ADD CONSTRAINT fk_dispute_response_dispute FOREIGN KEY (dispute_id) REFERENCES public.dispute(id) ON DELETE CASCADE;


--
-- TOC entry 4411 (class 2606 OID 37696)
-- Name: dispute_response fk_dispute_response_user; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dispute_response
    ADD CONSTRAINT fk_dispute_response_user FOREIGN KEY (responder_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4397 (class 2606 OID 36146)
-- Name: inventory_reservation fk_inventory_reservation_inventory_inventory_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.inventory_reservation
    ADD CONSTRAINT fk_inventory_reservation_inventory_inventory_id FOREIGN KEY (inventory_id) REFERENCES public.inventory(id) ON DELETE CASCADE;


--
-- TOC entry 4368 (class 2606 OID 33157)
-- Name: listing_id fk_listing_id_user_seller_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_id
    ADD CONSTRAINT fk_listing_id_user_seller_id FOREIGN KEY (seller_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4362 (class 2606 OID 33092)
-- Name: listing_image fk_listing_image_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_image
    ADD CONSTRAINT fk_listing_image_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4363 (class 2606 OID 33105)
-- Name: listing_item_specific fk_listing_item_specific_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.listing_item_specific
    ADD CONSTRAINT fk_listing_item_specific_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4402 (class 2606 OID 37407)
-- Name: offers fk_offers_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.offers
    ADD CONSTRAINT fk_offers_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4387 (class 2606 OID 33440)
-- Name: order_buyer_feedback fk_order_buyer_feedback_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_buyer_feedback
    ADD CONSTRAINT fk_order_buyer_feedback_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4388 (class 2606 OID 33445)
-- Name: order_buyer_feedback fk_order_buyer_feedback_user_buyer_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_buyer_feedback
    ADD CONSTRAINT fk_order_buyer_feedback_user_buyer_id FOREIGN KEY (buyer_id) REFERENCES public."user"(id) ON DELETE RESTRICT;


--
-- TOC entry 4381 (class 2606 OID 33338)
-- Name: order_cancellation_requests fk_order_cancellation_requests_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_cancellation_requests
    ADD CONSTRAINT fk_order_cancellation_requests_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4398 (class 2606 OID 36226)
-- Name: order_discount_category_rules fk_order_discount_category_rules_order_discounts_order_discoun; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_discount_category_rules
    ADD CONSTRAINT fk_order_discount_category_rules_order_discounts_order_discoun FOREIGN KEY (order_discount_id) REFERENCES public.order_discounts(id) ON DELETE CASCADE;


--
-- TOC entry 4399 (class 2606 OID 36236)
-- Name: order_discount_item_rules fk_order_discount_item_rules_order_discounts_order_discount_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_discount_item_rules
    ADD CONSTRAINT fk_order_discount_item_rules_order_discounts_order_discount_id FOREIGN KEY (order_discount_id) REFERENCES public.order_discounts(id) ON DELETE CASCADE;


--
-- TOC entry 4400 (class 2606 OID 36246)
-- Name: order_discount_tiers fk_order_discount_tiers_order_discounts_order_discount_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_discount_tiers
    ADD CONSTRAINT fk_order_discount_tiers_order_discounts_order_discount_id FOREIGN KEY (order_discount_id) REFERENCES public.order_discounts(id) ON DELETE CASCADE;


--
-- TOC entry 4383 (class 2606 OID 33388)
-- Name: order_item_shipments fk_order_item_shipments_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_shipments
    ADD CONSTRAINT fk_order_item_shipments_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4384 (class 2606 OID 33393)
-- Name: order_item_shipments fk_order_item_shipments_shipping_labels_shipping_label_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_item_shipments
    ADD CONSTRAINT fk_order_item_shipments_shipping_labels_shipping_label_id FOREIGN KEY (shipping_label_id) REFERENCES public.order_shipping_labels(id) ON DELETE SET NULL;


--
-- TOC entry 4374 (class 2606 OID 33376)
-- Name: order_items fk_order_items_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT fk_order_items_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE RESTRICT;


--
-- TOC entry 4375 (class 2606 OID 33213)
-- Name: order_items fk_order_items_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT fk_order_items_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4382 (class 2606 OID 33350)
-- Name: order_return_requests fk_order_return_requests_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_return_requests
    ADD CONSTRAINT fk_order_return_requests_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4376 (class 2606 OID 33225)
-- Name: order_shipping_labels fk_order_shipping_labels_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_shipping_labels
    ADD CONSTRAINT fk_order_shipping_labels_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4377 (class 2606 OID 33235)
-- Name: order_status_histories fk_order_status_histories_order_statuses_from_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_histories
    ADD CONSTRAINT fk_order_status_histories_order_statuses_from_status_id FOREIGN KEY (from_status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4378 (class 2606 OID 33240)
-- Name: order_status_histories fk_order_status_histories_order_statuses_to_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_histories
    ADD CONSTRAINT fk_order_status_histories_order_statuses_to_status_id FOREIGN KEY (to_status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4379 (class 2606 OID 33245)
-- Name: order_status_histories fk_order_status_histories_orders_order_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_histories
    ADD CONSTRAINT fk_order_status_histories_orders_order_id FOREIGN KEY (order_id) REFERENCES public.orders(id) ON DELETE CASCADE;


--
-- TOC entry 4365 (class 2606 OID 33130)
-- Name: order_status_transitions fk_order_status_transitions_order_statuses_from_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_transitions
    ADD CONSTRAINT fk_order_status_transitions_order_statuses_from_status_id FOREIGN KEY (from_status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4366 (class 2606 OID 33135)
-- Name: order_status_transitions fk_order_status_transitions_order_statuses_to_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_status_transitions
    ADD CONSTRAINT fk_order_status_transitions_order_statuses_to_status_id FOREIGN KEY (to_status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4369 (class 2606 OID 33169)
-- Name: orders fk_orders_order_statuses_status_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_order_statuses_status_id FOREIGN KEY (status_id) REFERENCES public.order_statuses(id) ON DELETE CASCADE;


--
-- TOC entry 4370 (class 2606 OID 33174)
-- Name: orders fk_orders_user_buyer_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_user_buyer_id FOREIGN KEY (buyer_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4371 (class 2606 OID 33186)
-- Name: refresh_token fk_refresh_token_user_user_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.refresh_token
    ADD CONSTRAINT fk_refresh_token_user_user_id FOREIGN KEY (user_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4392 (class 2606 OID 33558)
-- Name: review fk_review_listing_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.review
    ADD CONSTRAINT fk_review_listing_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4367 (class 2606 OID 33147)
-- Name: role_permissions fk_role_permissions_role_role_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_permissions
    ADD CONSTRAINT fk_role_permissions_role_role_id FOREIGN KEY (role_id) REFERENCES public.role(id) ON DELETE CASCADE;


--
-- TOC entry 4372 (class 2606 OID 33196)
-- Name: role_user fk_role_user_role_roles_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_user
    ADD CONSTRAINT fk_role_user_role_roles_id FOREIGN KEY (roles_id) REFERENCES public.role(id) ON DELETE CASCADE;


--
-- TOC entry 4373 (class 2606 OID 33201)
-- Name: role_user fk_role_user_user_user_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.role_user
    ADD CONSTRAINT fk_role_user_user_user_id FOREIGN KEY (user_id) REFERENCES public."user"(id) ON DELETE CASCADE;


--
-- TOC entry 4403 (class 2606 OID 37463)
-- Name: sale_event_discount_tiers fk_sale_event_discount_tiers_sale_events_sale_event_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_discount_tiers
    ADD CONSTRAINT fk_sale_event_discount_tiers_sale_events_sale_event_id FOREIGN KEY (sale_event_id) REFERENCES public.sale_events(id) ON DELETE CASCADE;


--
-- TOC entry 4404 (class 2606 OID 37473)
-- Name: sale_event_listings fk_sale_event_listings_sale_event_discount_tiers_discount_tier; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listings
    ADD CONSTRAINT fk_sale_event_listings_sale_event_discount_tiers_discount_tier FOREIGN KEY (discount_tier_id) REFERENCES public.sale_event_discount_tiers(id) ON DELETE CASCADE;


--
-- TOC entry 4405 (class 2606 OID 37478)
-- Name: sale_event_listings fk_sale_event_listings_sale_events_sale_event_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listings
    ADD CONSTRAINT fk_sale_event_listings_sale_events_sale_event_id FOREIGN KEY (sale_event_id) REFERENCES public.sale_events(id) ON DELETE CASCADE;


--
-- TOC entry 4389 (class 2606 OID 33515)
-- Name: seller_blocked_buyer fk_seller_blocked_buyer_seller_preference_seller_preference_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_blocked_buyer
    ADD CONSTRAINT fk_seller_blocked_buyer_seller_preference_seller_preference_id FOREIGN KEY (seller_preference_id) REFERENCES public.seller_preference(id) ON DELETE CASCADE;


--
-- TOC entry 4390 (class 2606 OID 33526)
-- Name: seller_exempt_buyer fk_seller_exempt_buyer_seller_preference_seller_preference_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.seller_exempt_buyer
    ADD CONSTRAINT fk_seller_exempt_buyer_seller_preference_seller_preference_id FOREIGN KEY (seller_preference_id) REFERENCES public.seller_preference(id) ON DELETE CASCADE;


--
-- TOC entry 4380 (class 2606 OID 33317)
-- Name: store_subscription fk_store_subscription_store_store_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.store_subscription
    ADD CONSTRAINT fk_store_subscription_store_store_id FOREIGN KEY (store_id) REFERENCES public.store(id) ON DELETE CASCADE;


--
-- TOC entry 4364 (class 2606 OID 33118)
-- Name: variation fk_variation_fixed_price_listings_listing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.variation
    ADD CONSTRAINT fk_variation_fixed_price_listings_listing_id FOREIGN KEY (listing_id) REFERENCES public.listing(id) ON DELETE CASCADE;


--
-- TOC entry 4409 (class 2606 OID 37631)
-- Name: volume_pricing_tiers fk_volume_pricing_tiers_volume_pricings_volume_pricing_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.volume_pricing_tiers
    ADD CONSTRAINT fk_volume_pricing_tiers_volume_pricings_volume_pricing_id FOREIGN KEY (volume_pricing_id) REFERENCES public.volume_pricings(id) ON DELETE CASCADE;


--
-- TOC entry 4396 (class 2606 OID 33622)
-- Name: voucher_transactions fk_voucher_transactions_vouchers_voucher_id; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.voucher_transactions
    ADD CONSTRAINT fk_voucher_transactions_vouchers_voucher_id FOREIGN KEY (voucher_id) REFERENCES public.vouchers(id) ON DELETE CASCADE;


--
-- TOC entry 4406 (class 2606 OID 37534)
-- Name: sale_event_discount_tier sale_event_discount_tier_sale_event_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_discount_tier
    ADD CONSTRAINT sale_event_discount_tier_sale_event_id_fkey FOREIGN KEY (sale_event_id) REFERENCES public.sale_event(id) ON DELETE CASCADE;


--
-- TOC entry 4407 (class 2606 OID 37549)
-- Name: sale_event_listing sale_event_listing_discount_tier_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listing
    ADD CONSTRAINT sale_event_listing_discount_tier_id_fkey FOREIGN KEY (discount_tier_id) REFERENCES public.sale_event_discount_tier(id) ON DELETE CASCADE;


--
-- TOC entry 4408 (class 2606 OID 37544)
-- Name: sale_event_listing sale_event_listing_sale_event_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sale_event_listing
    ADD CONSTRAINT sale_event_listing_sale_event_id_fkey FOREIGN KEY (sale_event_id) REFERENCES public.sale_event(id) ON DELETE CASCADE;


--
-- TOC entry 4647 (class 0 OID 0)
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
-- TOC entry 4648 (class 0 OID 0)
-- Dependencies: 526
-- Name: FUNCTION update_support_tickets_updated_at(); Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON FUNCTION public.update_support_tickets_updated_at() TO anon;
GRANT ALL ON FUNCTION public.update_support_tickets_updated_at() TO authenticated;
GRANT ALL ON FUNCTION public.update_support_tickets_updated_at() TO service_role;


--
-- TOC entry 4649 (class 0 OID 0)
-- Dependencies: 387
-- Name: TABLE "__EFMigrationsHistory"; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public."__EFMigrationsHistory" TO anon;
GRANT ALL ON TABLE public."__EFMigrationsHistory" TO authenticated;
GRANT ALL ON TABLE public."__EFMigrationsHistory" TO service_role;


--
-- TOC entry 4650 (class 0 OID 0)
-- Dependencies: 446
-- Name: TABLE applied_order_discounts; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.applied_order_discounts TO anon;
GRANT ALL ON TABLE public.applied_order_discounts TO authenticated;
GRANT ALL ON TABLE public.applied_order_discounts TO service_role;


--
-- TOC entry 4651 (class 0 OID 0)
-- Dependencies: 454
-- Name: TABLE applied_sale_events; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.applied_sale_events TO anon;
GRANT ALL ON TABLE public.applied_sale_events TO authenticated;
GRANT ALL ON TABLE public.applied_sale_events TO service_role;


--
-- TOC entry 4652 (class 0 OID 0)
-- Dependencies: 452
-- Name: TABLE bids; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.bids TO anon;
GRANT ALL ON TABLE public.bids TO authenticated;
GRANT ALL ON TABLE public.bids TO service_role;


--
-- TOC entry 4653 (class 0 OID 0)
-- Dependencies: 388
-- Name: TABLE category; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.category TO anon;
GRANT ALL ON TABLE public.category TO authenticated;
GRANT ALL ON TABLE public.category TO service_role;


--
-- TOC entry 4654 (class 0 OID 0)
-- Dependencies: 400
-- Name: TABLE category_condition; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.category_condition TO anon;
GRANT ALL ON TABLE public.category_condition TO authenticated;
GRANT ALL ON TABLE public.category_condition TO service_role;


--
-- TOC entry 4655 (class 0 OID 0)
-- Dependencies: 399
-- Name: TABLE category_specific; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.category_specific TO anon;
GRANT ALL ON TABLE public.category_specific TO authenticated;
GRANT ALL ON TABLE public.category_specific TO service_role;


--
-- TOC entry 4656 (class 0 OID 0)
-- Dependencies: 389
-- Name: TABLE condition; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.condition TO anon;
GRANT ALL ON TABLE public.condition TO authenticated;
GRANT ALL ON TABLE public.condition TO service_role;


--
-- TOC entry 4657 (class 0 OID 0)
-- Dependencies: 426
-- Name: TABLE coupon; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon TO anon;
GRANT ALL ON TABLE public.coupon TO authenticated;
GRANT ALL ON TABLE public.coupon TO service_role;


--
-- TOC entry 4658 (class 0 OID 0)
-- Dependencies: 427
-- Name: TABLE coupon_condition; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_condition TO anon;
GRANT ALL ON TABLE public.coupon_condition TO authenticated;
GRANT ALL ON TABLE public.coupon_condition TO service_role;


--
-- TOC entry 4659 (class 0 OID 0)
-- Dependencies: 434
-- Name: TABLE coupon_excluded_categories; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_excluded_categories TO anon;
GRANT ALL ON TABLE public.coupon_excluded_categories TO authenticated;
GRANT ALL ON TABLE public.coupon_excluded_categories TO service_role;


--
-- TOC entry 4660 (class 0 OID 0)
-- Dependencies: 435
-- Name: TABLE coupon_excluded_items; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_excluded_items TO anon;
GRANT ALL ON TABLE public.coupon_excluded_items TO authenticated;
GRANT ALL ON TABLE public.coupon_excluded_items TO service_role;


--
-- TOC entry 4661 (class 0 OID 0)
-- Dependencies: 436
-- Name: TABLE coupon_target_audiences; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_target_audiences TO anon;
GRANT ALL ON TABLE public.coupon_target_audiences TO authenticated;
GRANT ALL ON TABLE public.coupon_target_audiences TO service_role;


--
-- TOC entry 4662 (class 0 OID 0)
-- Dependencies: 425
-- Name: TABLE coupon_type; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.coupon_type TO anon;
GRANT ALL ON TABLE public.coupon_type TO authenticated;
GRANT ALL ON TABLE public.coupon_type TO service_role;


--
-- TOC entry 4663 (class 0 OID 0)
-- Dependencies: 432
-- Name: TABLE dispute; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.dispute TO anon;
GRANT ALL ON TABLE public.dispute TO authenticated;
GRANT ALL ON TABLE public.dispute TO service_role;


--
-- TOC entry 4669 (class 0 OID 0)
-- Dependencies: 466
-- Name: TABLE dispute_response; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.dispute_response TO anon;
GRANT ALL ON TABLE public.dispute_response TO authenticated;
GRANT ALL ON TABLE public.dispute_response TO service_role;


--
-- TOC entry 4670 (class 0 OID 0)
-- Dependencies: 390
-- Name: TABLE file_metadata; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.file_metadata TO anon;
GRANT ALL ON TABLE public.file_metadata TO authenticated;
GRANT ALL ON TABLE public.file_metadata TO service_role;


--
-- TOC entry 4671 (class 0 OID 0)
-- Dependencies: 443
-- Name: TABLE inventory; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.inventory TO anon;
GRANT ALL ON TABLE public.inventory TO authenticated;
GRANT ALL ON TABLE public.inventory TO service_role;


--
-- TOC entry 4672 (class 0 OID 0)
-- Dependencies: 444
-- Name: TABLE inventory_adjustment; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.inventory_adjustment TO anon;
GRANT ALL ON TABLE public.inventory_adjustment TO authenticated;
GRANT ALL ON TABLE public.inventory_adjustment TO service_role;


--
-- TOC entry 4673 (class 0 OID 0)
-- Dependencies: 445
-- Name: TABLE inventory_reservation; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.inventory_reservation TO anon;
GRANT ALL ON TABLE public.inventory_reservation TO authenticated;
GRANT ALL ON TABLE public.inventory_reservation TO service_role;


--
-- TOC entry 4674 (class 0 OID 0)
-- Dependencies: 391
-- Name: TABLE listing; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing TO anon;
GRANT ALL ON TABLE public.listing TO authenticated;
GRANT ALL ON TABLE public.listing TO service_role;


--
-- TOC entry 4675 (class 0 OID 0)
-- Dependencies: 409
-- Name: TABLE listing_id; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing_id TO anon;
GRANT ALL ON TABLE public.listing_id TO authenticated;
GRANT ALL ON TABLE public.listing_id TO service_role;


--
-- TOC entry 4676 (class 0 OID 0)
-- Dependencies: 402
-- Name: TABLE listing_image; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing_image TO anon;
GRANT ALL ON TABLE public.listing_image TO authenticated;
GRANT ALL ON TABLE public.listing_image TO service_role;


--
-- TOC entry 4677 (class 0 OID 0)
-- Dependencies: 401
-- Name: SEQUENCE listing_image_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.listing_image_id_seq TO anon;
GRANT ALL ON SEQUENCE public.listing_image_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.listing_image_id_seq TO service_role;


--
-- TOC entry 4678 (class 0 OID 0)
-- Dependencies: 404
-- Name: TABLE listing_item_specific; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing_item_specific TO anon;
GRANT ALL ON TABLE public.listing_item_specific TO authenticated;
GRANT ALL ON TABLE public.listing_item_specific TO service_role;


--
-- TOC entry 4679 (class 0 OID 0)
-- Dependencies: 403
-- Name: SEQUENCE listing_item_specific_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.listing_item_specific_id_seq TO anon;
GRANT ALL ON SEQUENCE public.listing_item_specific_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.listing_item_specific_id_seq TO service_role;


--
-- TOC entry 4680 (class 0 OID 0)
-- Dependencies: 392
-- Name: TABLE listing_template; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.listing_template TO anon;
GRANT ALL ON TABLE public.listing_template TO authenticated;
GRANT ALL ON TABLE public.listing_template TO service_role;


--
-- TOC entry 4681 (class 0 OID 0)
-- Dependencies: 467
-- Name: TABLE notification; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.notification TO anon;
GRANT ALL ON TABLE public.notification TO authenticated;
GRANT ALL ON TABLE public.notification TO service_role;


--
-- TOC entry 4682 (class 0 OID 0)
-- Dependencies: 453
-- Name: TABLE offers; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.offers TO anon;
GRANT ALL ON TABLE public.offers TO authenticated;
GRANT ALL ON TABLE public.offers TO service_role;


--
-- TOC entry 4683 (class 0 OID 0)
-- Dependencies: 428
-- Name: TABLE order_buyer_feedback; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_buyer_feedback TO anon;
GRANT ALL ON TABLE public.order_buyer_feedback TO authenticated;
GRANT ALL ON TABLE public.order_buyer_feedback TO service_role;


--
-- TOC entry 4684 (class 0 OID 0)
-- Dependencies: 420
-- Name: TABLE order_cancellation_requests; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_cancellation_requests TO anon;
GRANT ALL ON TABLE public.order_cancellation_requests TO authenticated;
GRANT ALL ON TABLE public.order_cancellation_requests TO service_role;


--
-- TOC entry 4685 (class 0 OID 0)
-- Dependencies: 449
-- Name: TABLE order_discount_category_rules; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_discount_category_rules TO anon;
GRANT ALL ON TABLE public.order_discount_category_rules TO authenticated;
GRANT ALL ON TABLE public.order_discount_category_rules TO service_role;


--
-- TOC entry 4686 (class 0 OID 0)
-- Dependencies: 450
-- Name: TABLE order_discount_item_rules; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_discount_item_rules TO anon;
GRANT ALL ON TABLE public.order_discount_item_rules TO authenticated;
GRANT ALL ON TABLE public.order_discount_item_rules TO service_role;


--
-- TOC entry 4687 (class 0 OID 0)
-- Dependencies: 447
-- Name: TABLE order_discount_performance_metrics; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_discount_performance_metrics TO anon;
GRANT ALL ON TABLE public.order_discount_performance_metrics TO authenticated;
GRANT ALL ON TABLE public.order_discount_performance_metrics TO service_role;


--
-- TOC entry 4688 (class 0 OID 0)
-- Dependencies: 451
-- Name: TABLE order_discount_tiers; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_discount_tiers TO anon;
GRANT ALL ON TABLE public.order_discount_tiers TO authenticated;
GRANT ALL ON TABLE public.order_discount_tiers TO service_role;


--
-- TOC entry 4689 (class 0 OID 0)
-- Dependencies: 448
-- Name: TABLE order_discounts; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_discounts TO anon;
GRANT ALL ON TABLE public.order_discounts TO authenticated;
GRANT ALL ON TABLE public.order_discounts TO service_role;


--
-- TOC entry 4690 (class 0 OID 0)
-- Dependencies: 424
-- Name: TABLE order_item_shipments; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_item_shipments TO anon;
GRANT ALL ON TABLE public.order_item_shipments TO authenticated;
GRANT ALL ON TABLE public.order_item_shipments TO service_role;


--
-- TOC entry 4691 (class 0 OID 0)
-- Dependencies: 413
-- Name: TABLE order_items; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_items TO anon;
GRANT ALL ON TABLE public.order_items TO authenticated;
GRANT ALL ON TABLE public.order_items TO service_role;


--
-- TOC entry 4692 (class 0 OID 0)
-- Dependencies: 421
-- Name: TABLE order_return_requests; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_return_requests TO anon;
GRANT ALL ON TABLE public.order_return_requests TO authenticated;
GRANT ALL ON TABLE public.order_return_requests TO service_role;


--
-- TOC entry 4693 (class 0 OID 0)
-- Dependencies: 414
-- Name: TABLE order_shipping_labels; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_shipping_labels TO anon;
GRANT ALL ON TABLE public.order_shipping_labels TO authenticated;
GRANT ALL ON TABLE public.order_shipping_labels TO service_role;


--
-- TOC entry 4694 (class 0 OID 0)
-- Dependencies: 415
-- Name: TABLE order_status_histories; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_status_histories TO anon;
GRANT ALL ON TABLE public.order_status_histories TO authenticated;
GRANT ALL ON TABLE public.order_status_histories TO service_role;


--
-- TOC entry 4695 (class 0 OID 0)
-- Dependencies: 407
-- Name: TABLE order_status_transitions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_status_transitions TO anon;
GRANT ALL ON TABLE public.order_status_transitions TO authenticated;
GRANT ALL ON TABLE public.order_status_transitions TO service_role;


--
-- TOC entry 4696 (class 0 OID 0)
-- Dependencies: 393
-- Name: TABLE order_statuses; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.order_statuses TO anon;
GRANT ALL ON TABLE public.order_statuses TO authenticated;
GRANT ALL ON TABLE public.order_statuses TO service_role;


--
-- TOC entry 4697 (class 0 OID 0)
-- Dependencies: 410
-- Name: TABLE orders; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.orders TO anon;
GRANT ALL ON TABLE public.orders TO authenticated;
GRANT ALL ON TABLE public.orders TO service_role;


--
-- TOC entry 4698 (class 0 OID 0)
-- Dependencies: 394
-- Name: TABLE otp; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.otp TO anon;
GRANT ALL ON TABLE public.otp TO authenticated;
GRANT ALL ON TABLE public.otp TO service_role;


--
-- TOC entry 4699 (class 0 OID 0)
-- Dependencies: 395
-- Name: TABLE outbox_message; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.outbox_message TO anon;
GRANT ALL ON TABLE public.outbox_message TO authenticated;
GRANT ALL ON TABLE public.outbox_message TO service_role;


--
-- TOC entry 4700 (class 0 OID 0)
-- Dependencies: 411
-- Name: TABLE refresh_token; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.refresh_token TO anon;
GRANT ALL ON TABLE public.refresh_token TO authenticated;
GRANT ALL ON TABLE public.refresh_token TO service_role;


--
-- TOC entry 4701 (class 0 OID 0)
-- Dependencies: 422
-- Name: TABLE report_downloads; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.report_downloads TO anon;
GRANT ALL ON TABLE public.report_downloads TO authenticated;
GRANT ALL ON TABLE public.report_downloads TO service_role;


--
-- TOC entry 4702 (class 0 OID 0)
-- Dependencies: 423
-- Name: TABLE report_schedules; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.report_schedules TO anon;
GRANT ALL ON TABLE public.report_schedules TO authenticated;
GRANT ALL ON TABLE public.report_schedules TO service_role;


--
-- TOC entry 4703 (class 0 OID 0)
-- Dependencies: 439
-- Name: TABLE research_saved_category; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.research_saved_category TO anon;
GRANT ALL ON TABLE public.research_saved_category TO authenticated;
GRANT ALL ON TABLE public.research_saved_category TO service_role;


--
-- TOC entry 4704 (class 0 OID 0)
-- Dependencies: 416
-- Name: TABLE return_policy; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.return_policy TO anon;
GRANT ALL ON TABLE public.return_policy TO authenticated;
GRANT ALL ON TABLE public.return_policy TO service_role;


--
-- TOC entry 4705 (class 0 OID 0)
-- Dependencies: 433
-- Name: TABLE review; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.review TO anon;
GRANT ALL ON TABLE public.review TO authenticated;
GRANT ALL ON TABLE public.review TO service_role;


--
-- TOC entry 4706 (class 0 OID 0)
-- Dependencies: 396
-- Name: TABLE role; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.role TO anon;
GRANT ALL ON TABLE public.role TO authenticated;
GRANT ALL ON TABLE public.role TO service_role;


--
-- TOC entry 4707 (class 0 OID 0)
-- Dependencies: 408
-- Name: TABLE role_permissions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.role_permissions TO anon;
GRANT ALL ON TABLE public.role_permissions TO authenticated;
GRANT ALL ON TABLE public.role_permissions TO service_role;


--
-- TOC entry 4708 (class 0 OID 0)
-- Dependencies: 412
-- Name: TABLE role_user; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.role_user TO anon;
GRANT ALL ON TABLE public.role_user TO authenticated;
GRANT ALL ON TABLE public.role_user TO service_role;


--
-- TOC entry 4709 (class 0 OID 0)
-- Dependencies: 460
-- Name: TABLE sale_event; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event TO anon;
GRANT ALL ON TABLE public.sale_event TO authenticated;
GRANT ALL ON TABLE public.sale_event TO service_role;


--
-- TOC entry 4710 (class 0 OID 0)
-- Dependencies: 461
-- Name: TABLE sale_event_discount_tier; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event_discount_tier TO anon;
GRANT ALL ON TABLE public.sale_event_discount_tier TO authenticated;
GRANT ALL ON TABLE public.sale_event_discount_tier TO service_role;


--
-- TOC entry 4711 (class 0 OID 0)
-- Dependencies: 458
-- Name: TABLE sale_event_discount_tiers; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event_discount_tiers TO anon;
GRANT ALL ON TABLE public.sale_event_discount_tiers TO authenticated;
GRANT ALL ON TABLE public.sale_event_discount_tiers TO service_role;


--
-- TOC entry 4712 (class 0 OID 0)
-- Dependencies: 462
-- Name: TABLE sale_event_listing; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event_listing TO anon;
GRANT ALL ON TABLE public.sale_event_listing TO authenticated;
GRANT ALL ON TABLE public.sale_event_listing TO service_role;


--
-- TOC entry 4713 (class 0 OID 0)
-- Dependencies: 459
-- Name: TABLE sale_event_listings; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event_listings TO anon;
GRANT ALL ON TABLE public.sale_event_listings TO authenticated;
GRANT ALL ON TABLE public.sale_event_listings TO service_role;


--
-- TOC entry 4714 (class 0 OID 0)
-- Dependencies: 455
-- Name: TABLE sale_event_performance_metrics; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event_performance_metrics TO anon;
GRANT ALL ON TABLE public.sale_event_performance_metrics TO authenticated;
GRANT ALL ON TABLE public.sale_event_performance_metrics TO service_role;


--
-- TOC entry 4715 (class 0 OID 0)
-- Dependencies: 456
-- Name: TABLE sale_event_price_snapshots; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_event_price_snapshots TO anon;
GRANT ALL ON TABLE public.sale_event_price_snapshots TO authenticated;
GRANT ALL ON TABLE public.sale_event_price_snapshots TO service_role;


--
-- TOC entry 4716 (class 0 OID 0)
-- Dependencies: 457
-- Name: TABLE sale_events; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.sale_events TO anon;
GRANT ALL ON TABLE public.sale_events TO authenticated;
GRANT ALL ON TABLE public.sale_events TO service_role;


--
-- TOC entry 4717 (class 0 OID 0)
-- Dependencies: 430
-- Name: TABLE seller_blocked_buyer; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.seller_blocked_buyer TO anon;
GRANT ALL ON TABLE public.seller_blocked_buyer TO authenticated;
GRANT ALL ON TABLE public.seller_blocked_buyer TO service_role;


--
-- TOC entry 4718 (class 0 OID 0)
-- Dependencies: 431
-- Name: TABLE seller_exempt_buyer; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.seller_exempt_buyer TO anon;
GRANT ALL ON TABLE public.seller_exempt_buyer TO authenticated;
GRANT ALL ON TABLE public.seller_exempt_buyer TO service_role;


--
-- TOC entry 4719 (class 0 OID 0)
-- Dependencies: 429
-- Name: TABLE seller_preference; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.seller_preference TO anon;
GRANT ALL ON TABLE public.seller_preference TO authenticated;
GRANT ALL ON TABLE public.seller_preference TO service_role;


--
-- TOC entry 4720 (class 0 OID 0)
-- Dependencies: 463
-- Name: TABLE shipping_discounts; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.shipping_discounts TO anon;
GRANT ALL ON TABLE public.shipping_discounts TO authenticated;
GRANT ALL ON TABLE public.shipping_discounts TO service_role;


--
-- TOC entry 4721 (class 0 OID 0)
-- Dependencies: 417
-- Name: TABLE shipping_policy; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.shipping_policy TO anon;
GRANT ALL ON TABLE public.shipping_policy TO authenticated;
GRANT ALL ON TABLE public.shipping_policy TO service_role;


--
-- TOC entry 4722 (class 0 OID 0)
-- Dependencies: 397
-- Name: TABLE shipping_services; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.shipping_services TO anon;
GRANT ALL ON TABLE public.shipping_services TO authenticated;
GRANT ALL ON TABLE public.shipping_services TO service_role;


--
-- TOC entry 4723 (class 0 OID 0)
-- Dependencies: 418
-- Name: TABLE store; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.store TO anon;
GRANT ALL ON TABLE public.store TO authenticated;
GRANT ALL ON TABLE public.store TO service_role;


--
-- TOC entry 4724 (class 0 OID 0)
-- Dependencies: 419
-- Name: TABLE store_subscription; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.store_subscription TO anon;
GRANT ALL ON TABLE public.store_subscription TO authenticated;
GRANT ALL ON TABLE public.store_subscription TO service_role;


--
-- TOC entry 4725 (class 0 OID 0)
-- Dependencies: 440
-- Name: TABLE support_tickets; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.support_tickets TO anon;
GRANT ALL ON TABLE public.support_tickets TO authenticated;
GRANT ALL ON TABLE public.support_tickets TO service_role;


--
-- TOC entry 4726 (class 0 OID 0)
-- Dependencies: 398
-- Name: TABLE "user"; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public."user" TO anon;
GRANT ALL ON TABLE public."user" TO authenticated;
GRANT ALL ON TABLE public."user" TO service_role;


--
-- TOC entry 4727 (class 0 OID 0)
-- Dependencies: 406
-- Name: TABLE variation; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.variation TO anon;
GRANT ALL ON TABLE public.variation TO authenticated;
GRANT ALL ON TABLE public.variation TO service_role;


--
-- TOC entry 4728 (class 0 OID 0)
-- Dependencies: 405
-- Name: SEQUENCE variation_id_seq; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON SEQUENCE public.variation_id_seq TO anon;
GRANT ALL ON SEQUENCE public.variation_id_seq TO authenticated;
GRANT ALL ON SEQUENCE public.variation_id_seq TO service_role;


--
-- TOC entry 4729 (class 0 OID 0)
-- Dependencies: 465
-- Name: TABLE volume_pricing_tiers; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.volume_pricing_tiers TO anon;
GRANT ALL ON TABLE public.volume_pricing_tiers TO authenticated;
GRANT ALL ON TABLE public.volume_pricing_tiers TO service_role;


--
-- TOC entry 4730 (class 0 OID 0)
-- Dependencies: 464
-- Name: TABLE volume_pricings; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.volume_pricings TO anon;
GRANT ALL ON TABLE public.volume_pricings TO authenticated;
GRANT ALL ON TABLE public.volume_pricings TO service_role;


--
-- TOC entry 4731 (class 0 OID 0)
-- Dependencies: 438
-- Name: TABLE voucher_transactions; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.voucher_transactions TO anon;
GRANT ALL ON TABLE public.voucher_transactions TO authenticated;
GRANT ALL ON TABLE public.voucher_transactions TO service_role;


--
-- TOC entry 4732 (class 0 OID 0)
-- Dependencies: 437
-- Name: TABLE vouchers; Type: ACL; Schema: public; Owner: postgres
--

GRANT ALL ON TABLE public.vouchers TO anon;
GRANT ALL ON TABLE public.vouchers TO authenticated;
GRANT ALL ON TABLE public.vouchers TO service_role;


--
-- TOC entry 2774 (class 826 OID 16494)
-- Name: DEFAULT PRIVILEGES FOR SEQUENCES; Type: DEFAULT ACL; Schema: public; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON SEQUENCES TO service_role;


--
-- TOC entry 2775 (class 826 OID 16495)
-- Name: DEFAULT PRIVILEGES FOR SEQUENCES; Type: DEFAULT ACL; Schema: public; Owner: supabase_admin
--

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON SEQUENCES TO service_role;


--
-- TOC entry 2773 (class 826 OID 16493)
-- Name: DEFAULT PRIVILEGES FOR FUNCTIONS; Type: DEFAULT ACL; Schema: public; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON FUNCTIONS TO service_role;


--
-- TOC entry 2777 (class 826 OID 16497)
-- Name: DEFAULT PRIVILEGES FOR FUNCTIONS; Type: DEFAULT ACL; Schema: public; Owner: supabase_admin
--

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON FUNCTIONS TO service_role;


--
-- TOC entry 2772 (class 826 OID 16492)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: public; Owner: postgres
--

ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE postgres IN SCHEMA public GRANT ALL ON TABLES TO service_role;


--
-- TOC entry 2776 (class 826 OID 16496)
-- Name: DEFAULT PRIVILEGES FOR TABLES; Type: DEFAULT ACL; Schema: public; Owner: supabase_admin
--

ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES TO postgres;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES TO anon;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES TO authenticated;
ALTER DEFAULT PRIVILEGES FOR ROLE supabase_admin IN SCHEMA public GRANT ALL ON TABLES TO service_role;


-- Completed on 2026-03-26 00:42:15

--
-- PostgreSQL database dump complete
--

\unrestrict BE4SJx2GYbGFofUhRpdoJwDTrtCzZI5sgpo4ODCXovsInnroraLlDN714v2dziG

