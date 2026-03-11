CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE category (
        id uuid NOT NULL,
        name text NOT NULL,
        description text NOT NULL,
        parent_id uuid,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_category PRIMARY KEY (id),
        CONSTRAINT fk_category_category_parent_id FOREIGN KEY (parent_id) REFERENCES category (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE condition (
        id uuid NOT NULL,
        name text NOT NULL,
        description text NOT NULL,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_condition PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE file_metadata (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_file_metadata PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE listing (
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
        CONSTRAINT pk_listing PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE listing_template (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_listing_template PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE order_statuses (
        id uuid NOT NULL,
        code character varying(50) NOT NULL,
        name character varying(100) NOT NULL,
        description character varying(500) NOT NULL,
        color character varying(20) NOT NULL,
        sort_order integer NOT NULL,
        CONSTRAINT pk_order_statuses PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE otp (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_otp PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE outbox_message (
        id uuid NOT NULL,
        type text NOT NULL,
        content text NOT NULL,
        occurred_on timestamp with time zone NOT NULL,
        processed_on timestamp with time zone,
        retry_count integer NOT NULL,
        error text,
        CONSTRAINT pk_outbox_message PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE role (
        id uuid NOT NULL,
        name character varying(100) NOT NULL,
        description text NOT NULL,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_role PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE shipping_services (
        id uuid NOT NULL,
        carrier character varying(100) NOT NULL,
        service_name character varying(100) NOT NULL,
        base_cost_amount numeric NOT NULL,
        base_cost_currency character varying(3) NOT NULL,
        estimated_delivery_time interval NOT NULL,
        CONSTRAINT pk_shipping_services PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE "user" (
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
        CONSTRAINT pk_user PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE category_specific (
        id uuid NOT NULL,
        name text NOT NULL,
        is_required boolean NOT NULL,
        allow_multiple boolean NOT NULL,
        values jsonb NOT NULL,
        category_id uuid,
        CONSTRAINT pk_category_specific PRIMARY KEY (id),
        CONSTRAINT fk_category_specific_category_category_id FOREIGN KEY (category_id) REFERENCES category (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE category_condition (
        category_id uuid NOT NULL,
        condition_id uuid NOT NULL,
        CONSTRAINT pk_category_condition PRIMARY KEY (category_id, condition_id),
        CONSTRAINT fk_category_condition_categories_category_id FOREIGN KEY (category_id) REFERENCES category (id) ON DELETE CASCADE,
        CONSTRAINT fk_category_condition_condition_condition_id FOREIGN KEY (condition_id) REFERENCES condition (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE listing_image (
        listing_id uuid NOT NULL,
        id integer GENERATED BY DEFAULT AS IDENTITY,
        url text NOT NULL,
        is_primary boolean NOT NULL,
        CONSTRAINT pk_listing_image PRIMARY KEY (listing_id, id),
        CONSTRAINT fk_listing_image_listing_listing_id FOREIGN KEY (listing_id) REFERENCES listing (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE listing_item_specific (
        listing_id uuid NOT NULL,
        id integer GENERATED BY DEFAULT AS IDENTITY,
        name character varying(100) NOT NULL,
        val text NOT NULL,
        CONSTRAINT pk_listing_item_specific PRIMARY KEY (listing_id, id),
        CONSTRAINT fk_listing_item_specific_listing_listing_id FOREIGN KEY (listing_id) REFERENCES listing (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE variation (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        sku text NOT NULL,
        price numeric NOT NULL,
        quantity integer NOT NULL,
        specifics jsonb NOT NULL,
        images jsonb NOT NULL,
        listing_id uuid NOT NULL,
        CONSTRAINT pk_variation PRIMARY KEY (id),
        CONSTRAINT fk_variation_fixed_price_listings_listing_id FOREIGN KEY (listing_id) REFERENCES listing (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE order_status_transitions (
        id uuid NOT NULL,
        from_status_id uuid NOT NULL,
        to_status_id uuid NOT NULL,
        "AllowedRoles" text NOT NULL,
        CONSTRAINT pk_order_status_transitions PRIMARY KEY (id),
        CONSTRAINT fk_order_status_transitions_order_statuses_from_status_id FOREIGN KEY (from_status_id) REFERENCES order_statuses (id) ON DELETE CASCADE,
        CONSTRAINT fk_order_status_transitions_order_statuses_to_status_id FOREIGN KEY (to_status_id) REFERENCES order_statuses (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE role_permissions (
        "Permission" text NOT NULL,
        role_id uuid NOT NULL,
        CONSTRAINT pk_role_permissions PRIMARY KEY (role_id, "Permission"),
        CONSTRAINT fk_role_permissions_role_role_id FOREIGN KEY (role_id) REFERENCES role (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE listing_id (
        listing_id uuid NOT NULL,
        seller_id uuid NOT NULL,
        CONSTRAINT pk_listing_id PRIMARY KEY (listing_id),
        CONSTRAINT fk_listing_id_user_seller_id FOREIGN KEY (seller_id) REFERENCES "user" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE orders (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_orders PRIMARY KEY (id),
        CONSTRAINT fk_orders_order_statuses_status_id FOREIGN KEY (status_id) REFERENCES order_statuses (id) ON DELETE CASCADE,
        CONSTRAINT fk_orders_user_buyer_id FOREIGN KEY (buyer_id) REFERENCES "user" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE refresh_token (
        id uuid NOT NULL,
        user_id uuid NOT NULL,
        token character varying(200) NOT NULL,
        expires_on_utc timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_refresh_token PRIMARY KEY (id),
        CONSTRAINT fk_refresh_token_user_user_id FOREIGN KEY (user_id) REFERENCES "user" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE role_user (
        roles_id uuid NOT NULL,
        user_id uuid NOT NULL,
        CONSTRAINT pk_role_user PRIMARY KEY (roles_id, user_id),
        CONSTRAINT fk_role_user_role_roles_id FOREIGN KEY (roles_id) REFERENCES role (id) ON DELETE CASCADE,
        CONSTRAINT fk_role_user_user_user_id FOREIGN KEY (user_id) REFERENCES "user" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE order_items (
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
        CONSTRAINT pk_order_items PRIMARY KEY (id),
        CONSTRAINT fk_order_items_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE order_shipping_labels (
        id uuid NOT NULL,
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
        CONSTRAINT pk_order_shipping_labels PRIMARY KEY (id),
        CONSTRAINT fk_order_shipping_labels_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE TABLE order_status_histories (
        id uuid NOT NULL,
        order_id uuid NOT NULL,
        from_status_id uuid NOT NULL,
        to_status_id uuid NOT NULL,
        changed_at timestamp with time zone NOT NULL,
        CONSTRAINT pk_order_status_histories PRIMARY KEY (id),
        CONSTRAINT fk_order_status_histories_order_statuses_from_status_id FOREIGN KEY (from_status_id) REFERENCES order_statuses (id) ON DELETE CASCADE,
        CONSTRAINT fk_order_status_histories_order_statuses_to_status_id FOREIGN KEY (to_status_id) REFERENCES order_statuses (id) ON DELETE CASCADE,
        CONSTRAINT fk_order_status_histories_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Consumer electronics, components, and accessories.', FALSE, 'Electronics', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('20000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Apparel, shoes, and accessories.', FALSE, 'Fashion', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('30000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Home improvement, décor, and outdoor living.', FALSE, 'Home & Garden', NULL, NULL, NULL);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Factory sealed, unused item in original packaging.', FALSE, 'New', NULL, NULL);
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Professionally restored to working order by the manufacturer or certified provider.', FALSE, 'Manufacturer Refurbished', NULL, NULL);
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Restored to working order by a third-party seller.', FALSE, 'Seller Refurbished', NULL, NULL);
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Previously owned item that shows signs of use.', FALSE, 'Used', NULL, NULL);
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Item does not function as intended and is being sold for parts or repair.', FALSE, 'For parts or not working', NULL, NULL);
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Item is unused but the original packaging has been opened.', FALSE, 'Open box', NULL, NULL);
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Unused apparel item with original tags attached.', FALSE, 'New with tags', NULL, NULL);
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000008', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Unused apparel item missing the retail tags.', FALSE, 'New without tags', NULL, NULL);
    INSERT INTO condition (id, created_at, created_by, description, is_deleted, name, updated_at, updated_by)
    VALUES ('40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Previously worn item that remains in good condition.', FALSE, 'Pre-owned', NULL, NULL);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000001', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #1 for Alice Johnson.', NULL, 'DEMO-1-0001', TIMESTAMPTZ '2024-01-01T00:00:00Z', 3, 'Alice''s Item #1', 1, NULL, NULL, 2, 29.99, 1, TRUE, 29.99, 26.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000002', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-31T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #2 for Alice Johnson.', NULL, 'DEMO-1-0002', TIMESTAMPTZ '2023-12-31T00:00:00Z', 3, 'Alice''s Item #2', 1, NULL, NULL, 2, 30.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000003', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-30T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #3 for Alice Johnson.', NULL, 'DEMO-1-0003', TIMESTAMPTZ '2023-12-30T00:00:00Z', 3, 'Alice''s Item #3', 1, NULL, NULL, 2, 31.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000004', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-29T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #4 for Alice Johnson.', NULL, 'DEMO-1-0004', TIMESTAMPTZ '2023-12-29T00:00:00Z', 3, 'Alice''s Item #4', 1, NULL, NULL, 2, 32.99, 4, TRUE, 32.99, 29.69);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000005', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-28T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #5 for Alice Johnson.', NULL, 'DEMO-1-0005', TIMESTAMPTZ '2023-12-28T00:00:00Z', 3, 'Alice''s Item #5', 1, NULL, NULL, 2, 33.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000006', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-27T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #6 for Alice Johnson.', NULL, 'DEMO-1-0006', TIMESTAMPTZ '2023-12-27T00:00:00Z', 3, 'Alice''s Item #6', 1, NULL, NULL, 2, 34.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000007', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-26T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #7 for Alice Johnson.', NULL, 'DEMO-1-0007', TIMESTAMPTZ '2023-12-26T00:00:00Z', 3, 'Alice''s Item #7', 1, NULL, NULL, 2, 35.99, 2, TRUE, 35.99, 32.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000008', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-25T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #8 for Alice Johnson.', NULL, 'DEMO-1-0008', TIMESTAMPTZ '2023-12-25T00:00:00Z', 3, 'Alice''s Item #8', 1, NULL, NULL, 2, 36.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000009', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-24T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #9 for Alice Johnson.', NULL, 'DEMO-1-0009', TIMESTAMPTZ '2023-12-24T00:00:00Z', 3, 'Alice''s Item #9', 1, NULL, NULL, 2, 37.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000000a', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-23T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #10 for Alice Johnson.', NULL, 'DEMO-1-0010', TIMESTAMPTZ '2023-12-23T00:00:00Z', 3, 'Alice''s Item #10', 1, NULL, NULL, 2, 38.99, 5, TRUE, 38.99, 35.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000000b', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-22T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #11 for Alice Johnson.', NULL, 'DEMO-1-0011', TIMESTAMPTZ '2023-12-22T00:00:00Z', 3, 'Alice''s Item #11', 1, NULL, NULL, 2, 39.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000000c', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-21T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #12 for Alice Johnson.', NULL, 'DEMO-1-0012', TIMESTAMPTZ '2023-12-21T00:00:00Z', 3, 'Alice''s Item #12', 1, NULL, NULL, 2, 40.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000000d', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-20T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #13 for Alice Johnson.', NULL, 'DEMO-1-0013', TIMESTAMPTZ '2023-12-20T00:00:00Z', 3, 'Alice''s Item #13', 1, NULL, NULL, 2, 41.99, 3, TRUE, 41.99, 37.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000000e', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-19T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #14 for Alice Johnson.', NULL, 'DEMO-1-0014', TIMESTAMPTZ '2023-12-19T00:00:00Z', 3, 'Alice''s Item #14', 1, NULL, NULL, 2, 42.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000000f', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-18T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #15 for Alice Johnson.', NULL, 'DEMO-1-0015', TIMESTAMPTZ '2023-12-18T00:00:00Z', 3, 'Alice''s Item #15', 1, NULL, NULL, 2, 43.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000010', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-17T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #16 for Alice Johnson.', NULL, 'DEMO-1-0016', TIMESTAMPTZ '2023-12-17T00:00:00Z', 3, 'Alice''s Item #16', 1, NULL, NULL, 2, 29.99, 1, TRUE, 29.99, 26.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000011', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-16T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #17 for Alice Johnson.', NULL, 'DEMO-1-0017', TIMESTAMPTZ '2023-12-16T00:00:00Z', 3, 'Alice''s Item #17', 1, NULL, NULL, 2, 30.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000012', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-15T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #18 for Alice Johnson.', NULL, 'DEMO-1-0018', TIMESTAMPTZ '2023-12-15T00:00:00Z', 3, 'Alice''s Item #18', 1, NULL, NULL, 2, 31.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000013', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-14T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #19 for Alice Johnson.', NULL, 'DEMO-1-0019', TIMESTAMPTZ '2023-12-14T00:00:00Z', 3, 'Alice''s Item #19', 1, NULL, NULL, 2, 32.99, 4, TRUE, 32.99, 29.69);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000014', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-13T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #20 for Alice Johnson.', NULL, 'DEMO-1-0020', TIMESTAMPTZ '2023-12-13T00:00:00Z', 3, 'Alice''s Item #20', 1, NULL, NULL, 2, 33.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000015', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-12T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #21 for Alice Johnson.', NULL, 'DEMO-1-0021', TIMESTAMPTZ '2023-12-12T00:00:00Z', 3, 'Alice''s Item #21', 1, NULL, NULL, 2, 34.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000016', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-11T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #22 for Alice Johnson.', NULL, 'DEMO-1-0022', TIMESTAMPTZ '2023-12-11T00:00:00Z', 3, 'Alice''s Item #22', 1, NULL, NULL, 2, 35.99, 2, TRUE, 35.99, 32.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000017', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-10T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #23 for Alice Johnson.', NULL, 'DEMO-1-0023', TIMESTAMPTZ '2023-12-10T00:00:00Z', 3, 'Alice''s Item #23', 1, NULL, NULL, 2, 36.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000018', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-09T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #24 for Alice Johnson.', NULL, 'DEMO-1-0024', TIMESTAMPTZ '2023-12-09T00:00:00Z', 3, 'Alice''s Item #24', 1, NULL, NULL, 2, 37.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000019', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-08T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #25 for Alice Johnson.', NULL, 'DEMO-1-0025', TIMESTAMPTZ '2023-12-08T00:00:00Z', 3, 'Alice''s Item #25', 1, NULL, NULL, 2, 38.99, 5, TRUE, 38.99, 35.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000001a', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-07T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #26 for Alice Johnson.', NULL, 'DEMO-1-0026', TIMESTAMPTZ '2023-12-07T00:00:00Z', 3, 'Alice''s Item #26', 1, NULL, NULL, 2, 39.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000001b', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-06T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #27 for Alice Johnson.', NULL, 'DEMO-1-0027', TIMESTAMPTZ '2023-12-06T00:00:00Z', 3, 'Alice''s Item #27', 1, NULL, NULL, 2, 40.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000001c', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-05T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #28 for Alice Johnson.', NULL, 'DEMO-1-0028', TIMESTAMPTZ '2023-12-05T00:00:00Z', 3, 'Alice''s Item #28', 1, NULL, NULL, 2, 41.99, 3, TRUE, 41.99, 37.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000001d', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-04T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #29 for Alice Johnson.', NULL, 'DEMO-1-0029', TIMESTAMPTZ '2023-12-04T00:00:00Z', 3, 'Alice''s Item #29', 1, NULL, NULL, 2, 42.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000001e', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-03T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #30 for Alice Johnson.', NULL, 'DEMO-1-0030', TIMESTAMPTZ '2023-12-03T00:00:00Z', 3, 'Alice''s Item #30', 1, NULL, NULL, 2, 43.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000001f', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-02T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #31 for Alice Johnson.', NULL, 'DEMO-1-0031', TIMESTAMPTZ '2023-12-02T00:00:00Z', 3, 'Alice''s Item #31', 1, NULL, NULL, 2, 29.99, 1, TRUE, 29.99, 26.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000020', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-01T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #32 for Alice Johnson.', NULL, 'DEMO-1-0032', TIMESTAMPTZ '2023-12-01T00:00:00Z', 3, 'Alice''s Item #32', 1, NULL, NULL, 2, 30.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000021', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-30T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #33 for Alice Johnson.', NULL, 'DEMO-1-0033', TIMESTAMPTZ '2023-11-30T00:00:00Z', 3, 'Alice''s Item #33', 1, NULL, NULL, 2, 31.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000022', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-29T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #34 for Alice Johnson.', NULL, 'DEMO-1-0034', TIMESTAMPTZ '2023-11-29T00:00:00Z', 3, 'Alice''s Item #34', 1, NULL, NULL, 2, 32.99, 4, TRUE, 32.99, 29.69);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000023', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-28T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #35 for Alice Johnson.', NULL, 'DEMO-1-0035', TIMESTAMPTZ '2023-11-28T00:00:00Z', 3, 'Alice''s Item #35', 1, NULL, NULL, 2, 33.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000024', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-27T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #36 for Alice Johnson.', NULL, 'DEMO-1-0036', TIMESTAMPTZ '2023-11-27T00:00:00Z', 3, 'Alice''s Item #36', 1, NULL, NULL, 2, 34.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000025', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-26T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #37 for Alice Johnson.', NULL, 'DEMO-1-0037', TIMESTAMPTZ '2023-11-26T00:00:00Z', 3, 'Alice''s Item #37', 1, NULL, NULL, 2, 35.99, 2, TRUE, 35.99, 32.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000026', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-25T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #38 for Alice Johnson.', NULL, 'DEMO-1-0038', TIMESTAMPTZ '2023-11-25T00:00:00Z', 3, 'Alice''s Item #38', 1, NULL, NULL, 2, 36.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000027', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-24T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #39 for Alice Johnson.', NULL, 'DEMO-1-0039', TIMESTAMPTZ '2023-11-24T00:00:00Z', 3, 'Alice''s Item #39', 1, NULL, NULL, 2, 37.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000028', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-23T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #40 for Alice Johnson.', NULL, 'DEMO-1-0040', TIMESTAMPTZ '2023-11-23T00:00:00Z', 3, 'Alice''s Item #40', 1, NULL, NULL, 2, 38.99, 5, TRUE, 38.99, 35.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000029', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-22T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #41 for Alice Johnson.', NULL, 'DEMO-1-0041', TIMESTAMPTZ '2023-11-22T00:00:00Z', 3, 'Alice''s Item #41', 1, NULL, NULL, 2, 39.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000002a', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-21T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #42 for Alice Johnson.', NULL, 'DEMO-1-0042', TIMESTAMPTZ '2023-11-21T00:00:00Z', 3, 'Alice''s Item #42', 1, NULL, NULL, 2, 40.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000002b', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-20T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #43 for Alice Johnson.', NULL, 'DEMO-1-0043', TIMESTAMPTZ '2023-11-20T00:00:00Z', 3, 'Alice''s Item #43', 1, NULL, NULL, 2, 41.99, 3, TRUE, 41.99, 37.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000002c', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-19T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #44 for Alice Johnson.', NULL, 'DEMO-1-0044', TIMESTAMPTZ '2023-11-19T00:00:00Z', 3, 'Alice''s Item #44', 1, NULL, NULL, 2, 42.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000002d', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-18T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #45 for Alice Johnson.', NULL, 'DEMO-1-0045', TIMESTAMPTZ '2023-11-18T00:00:00Z', 3, 'Alice''s Item #45', 1, NULL, NULL, 2, 43.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000002e', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #46 for Alice Johnson.', NULL, 'DEMO-1-0046', TIMESTAMPTZ '2024-01-01T00:00:00Z', 3, 'Alice''s Item #46', 1, NULL, NULL, 2, 29.99, 1, TRUE, 29.99, 26.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000002f', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-31T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #47 for Alice Johnson.', NULL, 'DEMO-1-0047', TIMESTAMPTZ '2023-12-31T00:00:00Z', 3, 'Alice''s Item #47', 1, NULL, NULL, 2, 30.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000030', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-30T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #48 for Alice Johnson.', NULL, 'DEMO-1-0048', TIMESTAMPTZ '2023-12-30T00:00:00Z', 3, 'Alice''s Item #48', 1, NULL, NULL, 2, 31.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000031', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-29T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #49 for Alice Johnson.', NULL, 'DEMO-1-0049', TIMESTAMPTZ '2023-12-29T00:00:00Z', 3, 'Alice''s Item #49', 1, NULL, NULL, 2, 32.99, 4, TRUE, 32.99, 29.69);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000032', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-28T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #50 for Alice Johnson.', NULL, 'DEMO-1-0050', TIMESTAMPTZ '2023-12-28T00:00:00Z', 3, 'Alice''s Item #50', 1, NULL, NULL, 2, 33.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000033', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-27T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #51 for Alice Johnson.', NULL, 'DEMO-1-0051', TIMESTAMPTZ '2023-12-27T00:00:00Z', 3, 'Alice''s Item #51', 1, NULL, NULL, 2, 34.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000034', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-26T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #52 for Alice Johnson.', NULL, 'DEMO-1-0052', TIMESTAMPTZ '2023-12-26T00:00:00Z', 3, 'Alice''s Item #52', 1, NULL, NULL, 2, 35.99, 2, TRUE, 35.99, 32.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000035', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-25T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #53 for Alice Johnson.', NULL, 'DEMO-1-0053', TIMESTAMPTZ '2023-12-25T00:00:00Z', 3, 'Alice''s Item #53', 1, NULL, NULL, 2, 36.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000036', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-24T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #54 for Alice Johnson.', NULL, 'DEMO-1-0054', TIMESTAMPTZ '2023-12-24T00:00:00Z', 3, 'Alice''s Item #54', 1, NULL, NULL, 2, 37.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000037', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-23T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #55 for Alice Johnson.', NULL, 'DEMO-1-0055', TIMESTAMPTZ '2023-12-23T00:00:00Z', 3, 'Alice''s Item #55', 1, NULL, NULL, 2, 38.99, 5, TRUE, 38.99, 35.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000038', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-22T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #56 for Alice Johnson.', NULL, 'DEMO-1-0056', TIMESTAMPTZ '2023-12-22T00:00:00Z', 3, 'Alice''s Item #56', 1, NULL, NULL, 2, 39.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000039', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-21T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #57 for Alice Johnson.', NULL, 'DEMO-1-0057', TIMESTAMPTZ '2023-12-21T00:00:00Z', 3, 'Alice''s Item #57', 1, NULL, NULL, 2, 40.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000003a', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-20T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #58 for Alice Johnson.', NULL, 'DEMO-1-0058', TIMESTAMPTZ '2023-12-20T00:00:00Z', 3, 'Alice''s Item #58', 1, NULL, NULL, 2, 41.99, 3, TRUE, 41.99, 37.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000003b', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-19T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #59 for Alice Johnson.', NULL, 'DEMO-1-0059', TIMESTAMPTZ '2023-12-19T00:00:00Z', 3, 'Alice''s Item #59', 1, NULL, NULL, 2, 42.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000003c', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-18T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #60 for Alice Johnson.', NULL, 'DEMO-1-0060', TIMESTAMPTZ '2023-12-18T00:00:00Z', 3, 'Alice''s Item #60', 1, NULL, NULL, 2, 43.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000003d', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-17T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #61 for Alice Johnson.', NULL, 'DEMO-1-0061', TIMESTAMPTZ '2023-12-17T00:00:00Z', 3, 'Alice''s Item #61', 1, NULL, NULL, 2, 29.99, 1, TRUE, 29.99, 26.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000003e', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-16T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #62 for Alice Johnson.', NULL, 'DEMO-1-0062', TIMESTAMPTZ '2023-12-16T00:00:00Z', 3, 'Alice''s Item #62', 1, NULL, NULL, 2, 30.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000003f', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-15T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #63 for Alice Johnson.', NULL, 'DEMO-1-0063', TIMESTAMPTZ '2023-12-15T00:00:00Z', 3, 'Alice''s Item #63', 1, NULL, NULL, 2, 31.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000040', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-14T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #64 for Alice Johnson.', NULL, 'DEMO-1-0064', TIMESTAMPTZ '2023-12-14T00:00:00Z', 3, 'Alice''s Item #64', 1, NULL, NULL, 2, 32.99, 4, TRUE, 32.99, 29.69);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000041', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-13T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #65 for Alice Johnson.', NULL, 'DEMO-1-0065', TIMESTAMPTZ '2023-12-13T00:00:00Z', 3, 'Alice''s Item #65', 1, NULL, NULL, 2, 33.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000042', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-12T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #66 for Alice Johnson.', NULL, 'DEMO-1-0066', TIMESTAMPTZ '2023-12-12T00:00:00Z', 3, 'Alice''s Item #66', 1, NULL, NULL, 2, 34.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000043', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-11T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #67 for Alice Johnson.', NULL, 'DEMO-1-0067', TIMESTAMPTZ '2023-12-11T00:00:00Z', 3, 'Alice''s Item #67', 1, NULL, NULL, 2, 35.99, 2, TRUE, 35.99, 32.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000044', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-10T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #68 for Alice Johnson.', NULL, 'DEMO-1-0068', TIMESTAMPTZ '2023-12-10T00:00:00Z', 3, 'Alice''s Item #68', 1, NULL, NULL, 2, 36.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000045', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-09T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #69 for Alice Johnson.', NULL, 'DEMO-1-0069', TIMESTAMPTZ '2023-12-09T00:00:00Z', 3, 'Alice''s Item #69', 1, NULL, NULL, 2, 37.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000046', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-08T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #70 for Alice Johnson.', NULL, 'DEMO-1-0070', TIMESTAMPTZ '2023-12-08T00:00:00Z', 3, 'Alice''s Item #70', 1, NULL, NULL, 2, 38.99, 5, TRUE, 38.99, 35.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000047', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-07T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #71 for Alice Johnson.', NULL, 'DEMO-1-0071', TIMESTAMPTZ '2023-12-07T00:00:00Z', 3, 'Alice''s Item #71', 1, NULL, NULL, 2, 39.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000048', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-06T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #72 for Alice Johnson.', NULL, 'DEMO-1-0072', TIMESTAMPTZ '2023-12-06T00:00:00Z', 3, 'Alice''s Item #72', 1, NULL, NULL, 2, 40.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000049', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-05T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #73 for Alice Johnson.', NULL, 'DEMO-1-0073', TIMESTAMPTZ '2023-12-05T00:00:00Z', 3, 'Alice''s Item #73', 1, NULL, NULL, 2, 41.99, 3, TRUE, 41.99, 37.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000004a', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-04T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #74 for Alice Johnson.', NULL, 'DEMO-1-0074', TIMESTAMPTZ '2023-12-04T00:00:00Z', 3, 'Alice''s Item #74', 1, NULL, NULL, 2, 42.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000004b', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-03T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #75 for Alice Johnson.', NULL, 'DEMO-1-0075', TIMESTAMPTZ '2023-12-03T00:00:00Z', 3, 'Alice''s Item #75', 1, NULL, NULL, 2, 43.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000004c', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-02T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #76 for Alice Johnson.', NULL, 'DEMO-1-0076', TIMESTAMPTZ '2023-12-02T00:00:00Z', 3, 'Alice''s Item #76', 1, NULL, NULL, 2, 29.99, 1, TRUE, 29.99, 26.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000004d', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-01T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #77 for Alice Johnson.', NULL, 'DEMO-1-0077', TIMESTAMPTZ '2023-12-01T00:00:00Z', 3, 'Alice''s Item #77', 1, NULL, NULL, 2, 30.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000004e', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-30T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #78 for Alice Johnson.', NULL, 'DEMO-1-0078', TIMESTAMPTZ '2023-11-30T00:00:00Z', 3, 'Alice''s Item #78', 1, NULL, NULL, 2, 31.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000004f', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-29T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #79 for Alice Johnson.', NULL, 'DEMO-1-0079', TIMESTAMPTZ '2023-11-29T00:00:00Z', 3, 'Alice''s Item #79', 1, NULL, NULL, 2, 32.99, 4, TRUE, 32.99, 29.69);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000050', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-28T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #80 for Alice Johnson.', NULL, 'DEMO-1-0080', TIMESTAMPTZ '2023-11-28T00:00:00Z', 3, 'Alice''s Item #80', 1, NULL, NULL, 2, 33.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000051', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-27T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #81 for Alice Johnson.', NULL, 'DEMO-1-0081', TIMESTAMPTZ '2023-11-27T00:00:00Z', 3, 'Alice''s Item #81', 1, NULL, NULL, 2, 34.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000052', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-26T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #82 for Alice Johnson.', NULL, 'DEMO-1-0082', TIMESTAMPTZ '2023-11-26T00:00:00Z', 3, 'Alice''s Item #82', 1, NULL, NULL, 2, 35.99, 2, TRUE, 35.99, 32.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000053', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-25T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #83 for Alice Johnson.', NULL, 'DEMO-1-0083', TIMESTAMPTZ '2023-11-25T00:00:00Z', 3, 'Alice''s Item #83', 1, NULL, NULL, 2, 36.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000054', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-24T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #84 for Alice Johnson.', NULL, 'DEMO-1-0084', TIMESTAMPTZ '2023-11-24T00:00:00Z', 3, 'Alice''s Item #84', 1, NULL, NULL, 2, 37.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000055', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-23T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #85 for Alice Johnson.', NULL, 'DEMO-1-0085', TIMESTAMPTZ '2023-11-23T00:00:00Z', 3, 'Alice''s Item #85', 1, NULL, NULL, 2, 38.99, 5, TRUE, 38.99, 35.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000056', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-22T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #86 for Alice Johnson.', NULL, 'DEMO-1-0086', TIMESTAMPTZ '2023-11-22T00:00:00Z', 3, 'Alice''s Item #86', 1, NULL, NULL, 2, 39.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000057', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-21T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #87 for Alice Johnson.', NULL, 'DEMO-1-0087', TIMESTAMPTZ '2023-11-21T00:00:00Z', 3, 'Alice''s Item #87', 1, NULL, NULL, 2, 40.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000058', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-20T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #88 for Alice Johnson.', NULL, 'DEMO-1-0088', TIMESTAMPTZ '2023-11-20T00:00:00Z', 3, 'Alice''s Item #88', 1, NULL, NULL, 2, 41.99, 3, TRUE, 41.99, 37.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000059', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-19T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #89 for Alice Johnson.', NULL, 'DEMO-1-0089', TIMESTAMPTZ '2023-11-19T00:00:00Z', 3, 'Alice''s Item #89', 1, NULL, NULL, 2, 42.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000005a', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-18T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #90 for Alice Johnson.', NULL, 'DEMO-1-0090', TIMESTAMPTZ '2023-11-18T00:00:00Z', 3, 'Alice''s Item #90', 1, NULL, NULL, 2, 43.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000005b', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #91 for Alice Johnson.', NULL, 'DEMO-1-0091', TIMESTAMPTZ '2024-01-01T00:00:00Z', 3, 'Alice''s Item #91', 1, NULL, NULL, 2, 29.99, 1, TRUE, 29.99, 26.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000005c', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-31T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #92 for Alice Johnson.', NULL, 'DEMO-1-0092', TIMESTAMPTZ '2023-12-31T00:00:00Z', 3, 'Alice''s Item #92', 1, NULL, NULL, 2, 30.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000005d', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-30T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #93 for Alice Johnson.', NULL, 'DEMO-1-0093', TIMESTAMPTZ '2023-12-30T00:00:00Z', 3, 'Alice''s Item #93', 1, NULL, NULL, 2, 31.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000005e', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-29T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #94 for Alice Johnson.', NULL, 'DEMO-1-0094', TIMESTAMPTZ '2023-12-29T00:00:00Z', 3, 'Alice''s Item #94', 1, NULL, NULL, 2, 32.99, 4, TRUE, 32.99, 29.69);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-00000000005f', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-28T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #95 for Alice Johnson.', NULL, 'DEMO-1-0095', TIMESTAMPTZ '2023-12-28T00:00:00Z', 3, 'Alice''s Item #95', 1, NULL, NULL, 2, 33.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000060', '10000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-27T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #96 for Alice Johnson.', NULL, 'DEMO-1-0096', TIMESTAMPTZ '2023-12-27T00:00:00Z', 3, 'Alice''s Item #96', 1, NULL, NULL, 2, 34.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000061', '10000000-0000-0000-0000-000000000003', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-26T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #97 for Alice Johnson.', NULL, 'DEMO-1-0097', TIMESTAMPTZ '2023-12-26T00:00:00Z', 3, 'Alice''s Item #97', 1, NULL, NULL, 2, 35.99, 2, TRUE, 35.99, 32.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000062', '10000000-0000-0000-0000-000000000004', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-25T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #98 for Alice Johnson.', NULL, 'DEMO-1-0098', TIMESTAMPTZ '2023-12-25T00:00:00Z', 3, 'Alice''s Item #98', 1, NULL, NULL, 2, 36.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000063', '20000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-24T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #99 for Alice Johnson.', NULL, 'DEMO-1-0099', TIMESTAMPTZ '2023-12-24T00:00:00Z', 3, 'Alice''s Item #99', 1, NULL, NULL, 2, 37.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('71000000-0000-0000-0000-000000000064', '30000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-23T00:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #100 for Alice Johnson.', NULL, 'DEMO-1-0100', TIMESTAMPTZ '2023-12-23T00:00:00Z', 3, 'Alice''s Item #100', 1, NULL, NULL, 2, 38.99, 5, TRUE, 38.99, 35.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000001', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-22T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #1 for Brian Carter.', NULL, 'DEMO-2-0001', TIMESTAMPTZ '2023-12-22T00:00:00Z', 3, 'Brian''s Item #1', 1, NULL, NULL, 2, 37.99, 1, TRUE, 37.99, 34.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000002', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-21T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #2 for Brian Carter.', NULL, 'DEMO-2-0002', TIMESTAMPTZ '2023-12-21T00:00:00Z', 3, 'Brian''s Item #2', 1, NULL, NULL, 2, 38.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000003', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-20T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #3 for Brian Carter.', NULL, 'DEMO-2-0003', TIMESTAMPTZ '2023-12-20T00:00:00Z', 3, 'Brian''s Item #3', 1, NULL, NULL, 2, 39.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000004', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-19T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #4 for Brian Carter.', NULL, 'DEMO-2-0004', TIMESTAMPTZ '2023-12-19T00:00:00Z', 3, 'Brian''s Item #4', 1, NULL, NULL, 2, 40.99, 4, TRUE, 40.99, 36.89);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000005', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-18T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #5 for Brian Carter.', NULL, 'DEMO-2-0005', TIMESTAMPTZ '2023-12-18T00:00:00Z', 3, 'Brian''s Item #5', 1, NULL, NULL, 2, 41.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000006', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-17T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #6 for Brian Carter.', NULL, 'DEMO-2-0006', TIMESTAMPTZ '2023-12-17T00:00:00Z', 3, 'Brian''s Item #6', 1, NULL, NULL, 2, 42.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000007', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-16T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #7 for Brian Carter.', NULL, 'DEMO-2-0007', TIMESTAMPTZ '2023-12-16T00:00:00Z', 3, 'Brian''s Item #7', 1, NULL, NULL, 2, 43.99, 2, TRUE, 43.99, 39.59);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000008', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-15T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #8 for Brian Carter.', NULL, 'DEMO-2-0008', TIMESTAMPTZ '2023-12-15T00:00:00Z', 3, 'Brian''s Item #8', 1, NULL, NULL, 2, 44.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000009', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-14T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #9 for Brian Carter.', NULL, 'DEMO-2-0009', TIMESTAMPTZ '2023-12-14T00:00:00Z', 3, 'Brian''s Item #9', 1, NULL, NULL, 2, 45.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000000a', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-13T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #10 for Brian Carter.', NULL, 'DEMO-2-0010', TIMESTAMPTZ '2023-12-13T00:00:00Z', 3, 'Brian''s Item #10', 1, NULL, NULL, 2, 46.99, 5, TRUE, 46.99, 42.29);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000000b', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-12T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #11 for Brian Carter.', NULL, 'DEMO-2-0011', TIMESTAMPTZ '2023-12-12T00:00:00Z', 3, 'Brian''s Item #11', 1, NULL, NULL, 2, 47.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000000c', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-11T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #12 for Brian Carter.', NULL, 'DEMO-2-0012', TIMESTAMPTZ '2023-12-11T00:00:00Z', 3, 'Brian''s Item #12', 1, NULL, NULL, 2, 48.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000000d', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-10T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #13 for Brian Carter.', NULL, 'DEMO-2-0013', TIMESTAMPTZ '2023-12-10T00:00:00Z', 3, 'Brian''s Item #13', 1, NULL, NULL, 2, 49.99, 3, TRUE, 49.99, 44.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000000e', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-09T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #14 for Brian Carter.', NULL, 'DEMO-2-0014', TIMESTAMPTZ '2023-12-09T00:00:00Z', 3, 'Brian''s Item #14', 1, NULL, NULL, 2, 50.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000000f', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-08T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #15 for Brian Carter.', NULL, 'DEMO-2-0015', TIMESTAMPTZ '2023-12-08T00:00:00Z', 3, 'Brian''s Item #15', 1, NULL, NULL, 2, 51.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000010', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-07T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #16 for Brian Carter.', NULL, 'DEMO-2-0016', TIMESTAMPTZ '2023-12-07T00:00:00Z', 3, 'Brian''s Item #16', 1, NULL, NULL, 2, 37.99, 1, TRUE, 37.99, 34.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000011', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-06T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #17 for Brian Carter.', NULL, 'DEMO-2-0017', TIMESTAMPTZ '2023-12-06T00:00:00Z', 3, 'Brian''s Item #17', 1, NULL, NULL, 2, 38.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000012', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-05T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #18 for Brian Carter.', NULL, 'DEMO-2-0018', TIMESTAMPTZ '2023-12-05T00:00:00Z', 3, 'Brian''s Item #18', 1, NULL, NULL, 2, 39.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000013', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-04T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #19 for Brian Carter.', NULL, 'DEMO-2-0019', TIMESTAMPTZ '2023-12-04T00:00:00Z', 3, 'Brian''s Item #19', 1, NULL, NULL, 2, 40.99, 4, TRUE, 40.99, 36.89);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000014', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-03T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #20 for Brian Carter.', NULL, 'DEMO-2-0020', TIMESTAMPTZ '2023-12-03T00:00:00Z', 3, 'Brian''s Item #20', 1, NULL, NULL, 2, 41.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000015', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-02T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #21 for Brian Carter.', NULL, 'DEMO-2-0021', TIMESTAMPTZ '2023-12-02T00:00:00Z', 3, 'Brian''s Item #21', 1, NULL, NULL, 2, 42.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000016', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-01T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #22 for Brian Carter.', NULL, 'DEMO-2-0022', TIMESTAMPTZ '2023-12-01T00:00:00Z', 3, 'Brian''s Item #22', 1, NULL, NULL, 2, 43.99, 2, TRUE, 43.99, 39.59);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000017', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-30T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #23 for Brian Carter.', NULL, 'DEMO-2-0023', TIMESTAMPTZ '2023-11-30T00:00:00Z', 3, 'Brian''s Item #23', 1, NULL, NULL, 2, 44.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000018', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-29T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #24 for Brian Carter.', NULL, 'DEMO-2-0024', TIMESTAMPTZ '2023-11-29T00:00:00Z', 3, 'Brian''s Item #24', 1, NULL, NULL, 2, 45.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000019', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-28T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #25 for Brian Carter.', NULL, 'DEMO-2-0025', TIMESTAMPTZ '2023-11-28T00:00:00Z', 3, 'Brian''s Item #25', 1, NULL, NULL, 2, 46.99, 5, TRUE, 46.99, 42.29);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000001a', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-27T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #26 for Brian Carter.', NULL, 'DEMO-2-0026', TIMESTAMPTZ '2023-11-27T00:00:00Z', 3, 'Brian''s Item #26', 1, NULL, NULL, 2, 47.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000001b', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-26T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #27 for Brian Carter.', NULL, 'DEMO-2-0027', TIMESTAMPTZ '2023-11-26T00:00:00Z', 3, 'Brian''s Item #27', 1, NULL, NULL, 2, 48.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000001c', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-25T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #28 for Brian Carter.', NULL, 'DEMO-2-0028', TIMESTAMPTZ '2023-11-25T00:00:00Z', 3, 'Brian''s Item #28', 1, NULL, NULL, 2, 49.99, 3, TRUE, 49.99, 44.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000001d', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-24T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #29 for Brian Carter.', NULL, 'DEMO-2-0029', TIMESTAMPTZ '2023-11-24T00:00:00Z', 3, 'Brian''s Item #29', 1, NULL, NULL, 2, 50.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000001e', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-23T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #30 for Brian Carter.', NULL, 'DEMO-2-0030', TIMESTAMPTZ '2023-11-23T00:00:00Z', 3, 'Brian''s Item #30', 1, NULL, NULL, 2, 51.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000001f', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-22T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #31 for Brian Carter.', NULL, 'DEMO-2-0031', TIMESTAMPTZ '2023-11-22T00:00:00Z', 3, 'Brian''s Item #31', 1, NULL, NULL, 2, 37.99, 1, TRUE, 37.99, 34.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000020', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-21T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #32 for Brian Carter.', NULL, 'DEMO-2-0032', TIMESTAMPTZ '2023-11-21T00:00:00Z', 3, 'Brian''s Item #32', 1, NULL, NULL, 2, 38.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000021', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-20T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #33 for Brian Carter.', NULL, 'DEMO-2-0033', TIMESTAMPTZ '2023-11-20T00:00:00Z', 3, 'Brian''s Item #33', 1, NULL, NULL, 2, 39.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000022', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-19T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #34 for Brian Carter.', NULL, 'DEMO-2-0034', TIMESTAMPTZ '2023-11-19T00:00:00Z', 3, 'Brian''s Item #34', 1, NULL, NULL, 2, 40.99, 4, TRUE, 40.99, 36.89);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000023', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-18T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #35 for Brian Carter.', NULL, 'DEMO-2-0035', TIMESTAMPTZ '2023-11-18T00:00:00Z', 3, 'Brian''s Item #35', 1, NULL, NULL, 2, 41.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000024', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #36 for Brian Carter.', NULL, 'DEMO-2-0036', TIMESTAMPTZ '2024-01-01T00:00:00Z', 3, 'Brian''s Item #36', 1, NULL, NULL, 2, 42.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000025', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-31T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #37 for Brian Carter.', NULL, 'DEMO-2-0037', TIMESTAMPTZ '2023-12-31T00:00:00Z', 3, 'Brian''s Item #37', 1, NULL, NULL, 2, 43.99, 2, TRUE, 43.99, 39.59);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000026', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-30T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #38 for Brian Carter.', NULL, 'DEMO-2-0038', TIMESTAMPTZ '2023-12-30T00:00:00Z', 3, 'Brian''s Item #38', 1, NULL, NULL, 2, 44.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000027', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-29T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #39 for Brian Carter.', NULL, 'DEMO-2-0039', TIMESTAMPTZ '2023-12-29T00:00:00Z', 3, 'Brian''s Item #39', 1, NULL, NULL, 2, 45.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000028', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-28T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #40 for Brian Carter.', NULL, 'DEMO-2-0040', TIMESTAMPTZ '2023-12-28T00:00:00Z', 3, 'Brian''s Item #40', 1, NULL, NULL, 2, 46.99, 5, TRUE, 46.99, 42.29);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000029', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-27T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #41 for Brian Carter.', NULL, 'DEMO-2-0041', TIMESTAMPTZ '2023-12-27T00:00:00Z', 3, 'Brian''s Item #41', 1, NULL, NULL, 2, 47.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000002a', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-26T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #42 for Brian Carter.', NULL, 'DEMO-2-0042', TIMESTAMPTZ '2023-12-26T00:00:00Z', 3, 'Brian''s Item #42', 1, NULL, NULL, 2, 48.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000002b', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-25T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #43 for Brian Carter.', NULL, 'DEMO-2-0043', TIMESTAMPTZ '2023-12-25T00:00:00Z', 3, 'Brian''s Item #43', 1, NULL, NULL, 2, 49.99, 3, TRUE, 49.99, 44.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000002c', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-24T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #44 for Brian Carter.', NULL, 'DEMO-2-0044', TIMESTAMPTZ '2023-12-24T00:00:00Z', 3, 'Brian''s Item #44', 1, NULL, NULL, 2, 50.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000002d', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-23T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #45 for Brian Carter.', NULL, 'DEMO-2-0045', TIMESTAMPTZ '2023-12-23T00:00:00Z', 3, 'Brian''s Item #45', 1, NULL, NULL, 2, 51.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000002e', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-22T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #46 for Brian Carter.', NULL, 'DEMO-2-0046', TIMESTAMPTZ '2023-12-22T00:00:00Z', 3, 'Brian''s Item #46', 1, NULL, NULL, 2, 37.99, 1, TRUE, 37.99, 34.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000002f', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-21T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #47 for Brian Carter.', NULL, 'DEMO-2-0047', TIMESTAMPTZ '2023-12-21T00:00:00Z', 3, 'Brian''s Item #47', 1, NULL, NULL, 2, 38.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000030', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-20T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #48 for Brian Carter.', NULL, 'DEMO-2-0048', TIMESTAMPTZ '2023-12-20T00:00:00Z', 3, 'Brian''s Item #48', 1, NULL, NULL, 2, 39.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000031', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-19T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #49 for Brian Carter.', NULL, 'DEMO-2-0049', TIMESTAMPTZ '2023-12-19T00:00:00Z', 3, 'Brian''s Item #49', 1, NULL, NULL, 2, 40.99, 4, TRUE, 40.99, 36.89);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000032', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-18T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #50 for Brian Carter.', NULL, 'DEMO-2-0050', TIMESTAMPTZ '2023-12-18T00:00:00Z', 3, 'Brian''s Item #50', 1, NULL, NULL, 2, 41.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000033', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-17T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #51 for Brian Carter.', NULL, 'DEMO-2-0051', TIMESTAMPTZ '2023-12-17T00:00:00Z', 3, 'Brian''s Item #51', 1, NULL, NULL, 2, 42.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000034', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-16T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #52 for Brian Carter.', NULL, 'DEMO-2-0052', TIMESTAMPTZ '2023-12-16T00:00:00Z', 3, 'Brian''s Item #52', 1, NULL, NULL, 2, 43.99, 2, TRUE, 43.99, 39.59);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000035', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-15T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #53 for Brian Carter.', NULL, 'DEMO-2-0053', TIMESTAMPTZ '2023-12-15T00:00:00Z', 3, 'Brian''s Item #53', 1, NULL, NULL, 2, 44.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000036', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-14T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #54 for Brian Carter.', NULL, 'DEMO-2-0054', TIMESTAMPTZ '2023-12-14T00:00:00Z', 3, 'Brian''s Item #54', 1, NULL, NULL, 2, 45.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000037', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-13T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #55 for Brian Carter.', NULL, 'DEMO-2-0055', TIMESTAMPTZ '2023-12-13T00:00:00Z', 3, 'Brian''s Item #55', 1, NULL, NULL, 2, 46.99, 5, TRUE, 46.99, 42.29);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000038', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-12T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #56 for Brian Carter.', NULL, 'DEMO-2-0056', TIMESTAMPTZ '2023-12-12T00:00:00Z', 3, 'Brian''s Item #56', 1, NULL, NULL, 2, 47.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000039', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-11T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #57 for Brian Carter.', NULL, 'DEMO-2-0057', TIMESTAMPTZ '2023-12-11T00:00:00Z', 3, 'Brian''s Item #57', 1, NULL, NULL, 2, 48.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000003a', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-10T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #58 for Brian Carter.', NULL, 'DEMO-2-0058', TIMESTAMPTZ '2023-12-10T00:00:00Z', 3, 'Brian''s Item #58', 1, NULL, NULL, 2, 49.99, 3, TRUE, 49.99, 44.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000003b', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-09T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #59 for Brian Carter.', NULL, 'DEMO-2-0059', TIMESTAMPTZ '2023-12-09T00:00:00Z', 3, 'Brian''s Item #59', 1, NULL, NULL, 2, 50.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000003c', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-08T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #60 for Brian Carter.', NULL, 'DEMO-2-0060', TIMESTAMPTZ '2023-12-08T00:00:00Z', 3, 'Brian''s Item #60', 1, NULL, NULL, 2, 51.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000003d', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-07T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #61 for Brian Carter.', NULL, 'DEMO-2-0061', TIMESTAMPTZ '2023-12-07T00:00:00Z', 3, 'Brian''s Item #61', 1, NULL, NULL, 2, 37.99, 1, TRUE, 37.99, 34.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000003e', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-06T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #62 for Brian Carter.', NULL, 'DEMO-2-0062', TIMESTAMPTZ '2023-12-06T00:00:00Z', 3, 'Brian''s Item #62', 1, NULL, NULL, 2, 38.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000003f', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-05T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #63 for Brian Carter.', NULL, 'DEMO-2-0063', TIMESTAMPTZ '2023-12-05T00:00:00Z', 3, 'Brian''s Item #63', 1, NULL, NULL, 2, 39.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000040', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-04T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #64 for Brian Carter.', NULL, 'DEMO-2-0064', TIMESTAMPTZ '2023-12-04T00:00:00Z', 3, 'Brian''s Item #64', 1, NULL, NULL, 2, 40.99, 4, TRUE, 40.99, 36.89);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000041', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-03T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #65 for Brian Carter.', NULL, 'DEMO-2-0065', TIMESTAMPTZ '2023-12-03T00:00:00Z', 3, 'Brian''s Item #65', 1, NULL, NULL, 2, 41.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000042', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-02T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #66 for Brian Carter.', NULL, 'DEMO-2-0066', TIMESTAMPTZ '2023-12-02T00:00:00Z', 3, 'Brian''s Item #66', 1, NULL, NULL, 2, 42.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000043', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-01T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #67 for Brian Carter.', NULL, 'DEMO-2-0067', TIMESTAMPTZ '2023-12-01T00:00:00Z', 3, 'Brian''s Item #67', 1, NULL, NULL, 2, 43.99, 2, TRUE, 43.99, 39.59);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000044', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-30T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #68 for Brian Carter.', NULL, 'DEMO-2-0068', TIMESTAMPTZ '2023-11-30T00:00:00Z', 3, 'Brian''s Item #68', 1, NULL, NULL, 2, 44.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000045', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-29T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #69 for Brian Carter.', NULL, 'DEMO-2-0069', TIMESTAMPTZ '2023-11-29T00:00:00Z', 3, 'Brian''s Item #69', 1, NULL, NULL, 2, 45.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000046', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-28T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #70 for Brian Carter.', NULL, 'DEMO-2-0070', TIMESTAMPTZ '2023-11-28T00:00:00Z', 3, 'Brian''s Item #70', 1, NULL, NULL, 2, 46.99, 5, TRUE, 46.99, 42.29);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000047', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-27T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #71 for Brian Carter.', NULL, 'DEMO-2-0071', TIMESTAMPTZ '2023-11-27T00:00:00Z', 3, 'Brian''s Item #71', 1, NULL, NULL, 2, 47.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000048', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-26T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #72 for Brian Carter.', NULL, 'DEMO-2-0072', TIMESTAMPTZ '2023-11-26T00:00:00Z', 3, 'Brian''s Item #72', 1, NULL, NULL, 2, 48.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000049', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-25T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #73 for Brian Carter.', NULL, 'DEMO-2-0073', TIMESTAMPTZ '2023-11-25T00:00:00Z', 3, 'Brian''s Item #73', 1, NULL, NULL, 2, 49.99, 3, TRUE, 49.99, 44.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000004a', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-24T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #74 for Brian Carter.', NULL, 'DEMO-2-0074', TIMESTAMPTZ '2023-11-24T00:00:00Z', 3, 'Brian''s Item #74', 1, NULL, NULL, 2, 50.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000004b', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-23T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #75 for Brian Carter.', NULL, 'DEMO-2-0075', TIMESTAMPTZ '2023-11-23T00:00:00Z', 3, 'Brian''s Item #75', 1, NULL, NULL, 2, 51.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000004c', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-22T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #76 for Brian Carter.', NULL, 'DEMO-2-0076', TIMESTAMPTZ '2023-11-22T00:00:00Z', 3, 'Brian''s Item #76', 1, NULL, NULL, 2, 37.99, 1, TRUE, 37.99, 34.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000004d', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-21T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #77 for Brian Carter.', NULL, 'DEMO-2-0077', TIMESTAMPTZ '2023-11-21T00:00:00Z', 3, 'Brian''s Item #77', 1, NULL, NULL, 2, 38.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000004e', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-20T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #78 for Brian Carter.', NULL, 'DEMO-2-0078', TIMESTAMPTZ '2023-11-20T00:00:00Z', 3, 'Brian''s Item #78', 1, NULL, NULL, 2, 39.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000004f', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-19T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #79 for Brian Carter.', NULL, 'DEMO-2-0079', TIMESTAMPTZ '2023-11-19T00:00:00Z', 3, 'Brian''s Item #79', 1, NULL, NULL, 2, 40.99, 4, TRUE, 40.99, 36.89);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000050', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-18T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #80 for Brian Carter.', NULL, 'DEMO-2-0080', TIMESTAMPTZ '2023-11-18T00:00:00Z', 3, 'Brian''s Item #80', 1, NULL, NULL, 2, 41.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000051', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #81 for Brian Carter.', NULL, 'DEMO-2-0081', TIMESTAMPTZ '2024-01-01T00:00:00Z', 3, 'Brian''s Item #81', 1, NULL, NULL, 2, 42.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000052', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-31T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #82 for Brian Carter.', NULL, 'DEMO-2-0082', TIMESTAMPTZ '2023-12-31T00:00:00Z', 3, 'Brian''s Item #82', 1, NULL, NULL, 2, 43.99, 2, TRUE, 43.99, 39.59);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000053', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-30T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #83 for Brian Carter.', NULL, 'DEMO-2-0083', TIMESTAMPTZ '2023-12-30T00:00:00Z', 3, 'Brian''s Item #83', 1, NULL, NULL, 2, 44.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000054', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-29T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #84 for Brian Carter.', NULL, 'DEMO-2-0084', TIMESTAMPTZ '2023-12-29T00:00:00Z', 3, 'Brian''s Item #84', 1, NULL, NULL, 2, 45.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000055', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-28T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #85 for Brian Carter.', NULL, 'DEMO-2-0085', TIMESTAMPTZ '2023-12-28T00:00:00Z', 3, 'Brian''s Item #85', 1, NULL, NULL, 2, 46.99, 5, TRUE, 46.99, 42.29);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000056', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-27T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #86 for Brian Carter.', NULL, 'DEMO-2-0086', TIMESTAMPTZ '2023-12-27T00:00:00Z', 3, 'Brian''s Item #86', 1, NULL, NULL, 2, 47.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000057', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-26T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #87 for Brian Carter.', NULL, 'DEMO-2-0087', TIMESTAMPTZ '2023-12-26T00:00:00Z', 3, 'Brian''s Item #87', 1, NULL, NULL, 2, 48.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000058', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-25T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #88 for Brian Carter.', NULL, 'DEMO-2-0088', TIMESTAMPTZ '2023-12-25T00:00:00Z', 3, 'Brian''s Item #88', 1, NULL, NULL, 2, 49.99, 3, TRUE, 49.99, 44.99);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000059', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-24T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #89 for Brian Carter.', NULL, 'DEMO-2-0089', TIMESTAMPTZ '2023-12-24T00:00:00Z', 3, 'Brian''s Item #89', 1, NULL, NULL, 2, 50.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000005a', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-23T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #90 for Brian Carter.', NULL, 'DEMO-2-0090', TIMESTAMPTZ '2023-12-23T00:00:00Z', 3, 'Brian''s Item #90', 1, NULL, NULL, 2, 51.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000005b', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-22T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #91 for Brian Carter.', NULL, 'DEMO-2-0091', TIMESTAMPTZ '2023-12-22T00:00:00Z', 3, 'Brian''s Item #91', 1, NULL, NULL, 2, 37.99, 1, TRUE, 37.99, 34.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000005c', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-21T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #92 for Brian Carter.', NULL, 'DEMO-2-0092', TIMESTAMPTZ '2023-12-21T00:00:00Z', 3, 'Brian''s Item #92', 1, NULL, NULL, 2, 38.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000005d', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-20T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #93 for Brian Carter.', NULL, 'DEMO-2-0093', TIMESTAMPTZ '2023-12-20T00:00:00Z', 3, 'Brian''s Item #93', 1, NULL, NULL, 2, 39.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000005e', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-19T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #94 for Brian Carter.', NULL, 'DEMO-2-0094', TIMESTAMPTZ '2023-12-19T00:00:00Z', 3, 'Brian''s Item #94', 1, NULL, NULL, 2, 40.99, 4, TRUE, 40.99, 36.89);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-00000000005f', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-18T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #95 for Brian Carter.', NULL, 'DEMO-2-0095', TIMESTAMPTZ '2023-12-18T00:00:00Z', 3, 'Brian''s Item #95', 1, NULL, NULL, 2, 41.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000060', '10000000-0000-0000-0000-000000000003', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-17T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #96 for Brian Carter.', NULL, 'DEMO-2-0096', TIMESTAMPTZ '2023-12-17T00:00:00Z', 3, 'Brian''s Item #96', 1, NULL, NULL, 2, 42.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000061', '10000000-0000-0000-0000-000000000004', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-16T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #97 for Brian Carter.', NULL, 'DEMO-2-0097', TIMESTAMPTZ '2023-12-16T00:00:00Z', 3, 'Brian''s Item #97', 1, NULL, NULL, 2, 43.99, 2, TRUE, 43.99, 39.59);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000062', '20000000-0000-0000-0000-000000000002', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-15T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #98 for Brian Carter.', NULL, 'DEMO-2-0098', TIMESTAMPTZ '2023-12-15T00:00:00Z', 3, 'Brian''s Item #98', 1, NULL, NULL, 2, 44.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000063', '30000000-0000-0000-0000-000000000002', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-14T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #99 for Brian Carter.', NULL, 'DEMO-2-0099', TIMESTAMPTZ '2023-12-14T00:00:00Z', 3, 'Brian''s Item #99', 1, NULL, NULL, 2, 45.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('72000000-0000-0000-0000-000000000064', '10000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-13T00:00:00Z', '70000000-0000-0000-0000-000000000002', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #100 for Brian Carter.', NULL, 'DEMO-2-0100', TIMESTAMPTZ '2023-12-13T00:00:00Z', 3, 'Brian''s Item #100', 1, NULL, NULL, 2, 46.99, 5, TRUE, 46.99, 42.29);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000001', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-12T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #1 for Cecilia Gomez.', NULL, 'DEMO-3-0001', TIMESTAMPTZ '2023-12-12T00:00:00Z', 3, 'Cecilia''s Item #1', 1, NULL, NULL, 2, 45.99, 1, TRUE, 45.99, 41.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000002', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-11T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #2 for Cecilia Gomez.', NULL, 'DEMO-3-0002', TIMESTAMPTZ '2023-12-11T00:00:00Z', 3, 'Cecilia''s Item #2', 1, NULL, NULL, 2, 46.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000003', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-10T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #3 for Cecilia Gomez.', NULL, 'DEMO-3-0003', TIMESTAMPTZ '2023-12-10T00:00:00Z', 3, 'Cecilia''s Item #3', 1, NULL, NULL, 2, 47.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000004', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-09T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #4 for Cecilia Gomez.', NULL, 'DEMO-3-0004', TIMESTAMPTZ '2023-12-09T00:00:00Z', 3, 'Cecilia''s Item #4', 1, NULL, NULL, 2, 48.99, 4, TRUE, 48.99, 44.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000005', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-08T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #5 for Cecilia Gomez.', NULL, 'DEMO-3-0005', TIMESTAMPTZ '2023-12-08T00:00:00Z', 3, 'Cecilia''s Item #5', 1, NULL, NULL, 2, 49.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000006', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-07T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #6 for Cecilia Gomez.', NULL, 'DEMO-3-0006', TIMESTAMPTZ '2023-12-07T00:00:00Z', 3, 'Cecilia''s Item #6', 1, NULL, NULL, 2, 50.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000007', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-06T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #7 for Cecilia Gomez.', NULL, 'DEMO-3-0007', TIMESTAMPTZ '2023-12-06T00:00:00Z', 3, 'Cecilia''s Item #7', 1, NULL, NULL, 2, 51.99, 2, TRUE, 51.99, 46.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000008', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-05T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #8 for Cecilia Gomez.', NULL, 'DEMO-3-0008', TIMESTAMPTZ '2023-12-05T00:00:00Z', 3, 'Cecilia''s Item #8', 1, NULL, NULL, 2, 52.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000009', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-04T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #9 for Cecilia Gomez.', NULL, 'DEMO-3-0009', TIMESTAMPTZ '2023-12-04T00:00:00Z', 3, 'Cecilia''s Item #9', 1, NULL, NULL, 2, 53.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000000a', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-03T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #10 for Cecilia Gomez.', NULL, 'DEMO-3-0010', TIMESTAMPTZ '2023-12-03T00:00:00Z', 3, 'Cecilia''s Item #10', 1, NULL, NULL, 2, 54.99, 5, TRUE, 54.99, 49.49);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000000b', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-02T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #11 for Cecilia Gomez.', NULL, 'DEMO-3-0011', TIMESTAMPTZ '2023-12-02T00:00:00Z', 3, 'Cecilia''s Item #11', 1, NULL, NULL, 2, 55.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000000c', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-01T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #12 for Cecilia Gomez.', NULL, 'DEMO-3-0012', TIMESTAMPTZ '2023-12-01T00:00:00Z', 3, 'Cecilia''s Item #12', 1, NULL, NULL, 2, 56.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000000d', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-30T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #13 for Cecilia Gomez.', NULL, 'DEMO-3-0013', TIMESTAMPTZ '2023-11-30T00:00:00Z', 3, 'Cecilia''s Item #13', 1, NULL, NULL, 2, 57.99, 3, TRUE, 57.99, 52.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000000e', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-29T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #14 for Cecilia Gomez.', NULL, 'DEMO-3-0014', TIMESTAMPTZ '2023-11-29T00:00:00Z', 3, 'Cecilia''s Item #14', 1, NULL, NULL, 2, 58.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000000f', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-28T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #15 for Cecilia Gomez.', NULL, 'DEMO-3-0015', TIMESTAMPTZ '2023-11-28T00:00:00Z', 3, 'Cecilia''s Item #15', 1, NULL, NULL, 2, 59.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000010', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-27T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #16 for Cecilia Gomez.', NULL, 'DEMO-3-0016', TIMESTAMPTZ '2023-11-27T00:00:00Z', 3, 'Cecilia''s Item #16', 1, NULL, NULL, 2, 45.99, 1, TRUE, 45.99, 41.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000011', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-26T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #17 for Cecilia Gomez.', NULL, 'DEMO-3-0017', TIMESTAMPTZ '2023-11-26T00:00:00Z', 3, 'Cecilia''s Item #17', 1, NULL, NULL, 2, 46.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000012', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-25T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #18 for Cecilia Gomez.', NULL, 'DEMO-3-0018', TIMESTAMPTZ '2023-11-25T00:00:00Z', 3, 'Cecilia''s Item #18', 1, NULL, NULL, 2, 47.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000013', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-24T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #19 for Cecilia Gomez.', NULL, 'DEMO-3-0019', TIMESTAMPTZ '2023-11-24T00:00:00Z', 3, 'Cecilia''s Item #19', 1, NULL, NULL, 2, 48.99, 4, TRUE, 48.99, 44.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000014', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-23T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #20 for Cecilia Gomez.', NULL, 'DEMO-3-0020', TIMESTAMPTZ '2023-11-23T00:00:00Z', 3, 'Cecilia''s Item #20', 1, NULL, NULL, 2, 49.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000015', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-22T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #21 for Cecilia Gomez.', NULL, 'DEMO-3-0021', TIMESTAMPTZ '2023-11-22T00:00:00Z', 3, 'Cecilia''s Item #21', 1, NULL, NULL, 2, 50.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000016', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-21T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #22 for Cecilia Gomez.', NULL, 'DEMO-3-0022', TIMESTAMPTZ '2023-11-21T00:00:00Z', 3, 'Cecilia''s Item #22', 1, NULL, NULL, 2, 51.99, 2, TRUE, 51.99, 46.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000017', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-20T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #23 for Cecilia Gomez.', NULL, 'DEMO-3-0023', TIMESTAMPTZ '2023-11-20T00:00:00Z', 3, 'Cecilia''s Item #23', 1, NULL, NULL, 2, 52.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000018', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-19T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #24 for Cecilia Gomez.', NULL, 'DEMO-3-0024', TIMESTAMPTZ '2023-11-19T00:00:00Z', 3, 'Cecilia''s Item #24', 1, NULL, NULL, 2, 53.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000019', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-18T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #25 for Cecilia Gomez.', NULL, 'DEMO-3-0025', TIMESTAMPTZ '2023-11-18T00:00:00Z', 3, 'Cecilia''s Item #25', 1, NULL, NULL, 2, 54.99, 5, TRUE, 54.99, 49.49);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000001a', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #26 for Cecilia Gomez.', NULL, 'DEMO-3-0026', TIMESTAMPTZ '2024-01-01T00:00:00Z', 3, 'Cecilia''s Item #26', 1, NULL, NULL, 2, 55.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000001b', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-31T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #27 for Cecilia Gomez.', NULL, 'DEMO-3-0027', TIMESTAMPTZ '2023-12-31T00:00:00Z', 3, 'Cecilia''s Item #27', 1, NULL, NULL, 2, 56.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000001c', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-30T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #28 for Cecilia Gomez.', NULL, 'DEMO-3-0028', TIMESTAMPTZ '2023-12-30T00:00:00Z', 3, 'Cecilia''s Item #28', 1, NULL, NULL, 2, 57.99, 3, TRUE, 57.99, 52.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000001d', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-29T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #29 for Cecilia Gomez.', NULL, 'DEMO-3-0029', TIMESTAMPTZ '2023-12-29T00:00:00Z', 3, 'Cecilia''s Item #29', 1, NULL, NULL, 2, 58.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000001e', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-28T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #30 for Cecilia Gomez.', NULL, 'DEMO-3-0030', TIMESTAMPTZ '2023-12-28T00:00:00Z', 3, 'Cecilia''s Item #30', 1, NULL, NULL, 2, 59.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000001f', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-27T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #31 for Cecilia Gomez.', NULL, 'DEMO-3-0031', TIMESTAMPTZ '2023-12-27T00:00:00Z', 3, 'Cecilia''s Item #31', 1, NULL, NULL, 2, 45.99, 1, TRUE, 45.99, 41.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000020', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-26T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #32 for Cecilia Gomez.', NULL, 'DEMO-3-0032', TIMESTAMPTZ '2023-12-26T00:00:00Z', 3, 'Cecilia''s Item #32', 1, NULL, NULL, 2, 46.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000021', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-25T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #33 for Cecilia Gomez.', NULL, 'DEMO-3-0033', TIMESTAMPTZ '2023-12-25T00:00:00Z', 3, 'Cecilia''s Item #33', 1, NULL, NULL, 2, 47.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000022', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-24T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #34 for Cecilia Gomez.', NULL, 'DEMO-3-0034', TIMESTAMPTZ '2023-12-24T00:00:00Z', 3, 'Cecilia''s Item #34', 1, NULL, NULL, 2, 48.99, 4, TRUE, 48.99, 44.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000023', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-23T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #35 for Cecilia Gomez.', NULL, 'DEMO-3-0035', TIMESTAMPTZ '2023-12-23T00:00:00Z', 3, 'Cecilia''s Item #35', 1, NULL, NULL, 2, 49.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000024', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-22T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #36 for Cecilia Gomez.', NULL, 'DEMO-3-0036', TIMESTAMPTZ '2023-12-22T00:00:00Z', 3, 'Cecilia''s Item #36', 1, NULL, NULL, 2, 50.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000025', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-21T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #37 for Cecilia Gomez.', NULL, 'DEMO-3-0037', TIMESTAMPTZ '2023-12-21T00:00:00Z', 3, 'Cecilia''s Item #37', 1, NULL, NULL, 2, 51.99, 2, TRUE, 51.99, 46.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000026', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-20T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #38 for Cecilia Gomez.', NULL, 'DEMO-3-0038', TIMESTAMPTZ '2023-12-20T00:00:00Z', 3, 'Cecilia''s Item #38', 1, NULL, NULL, 2, 52.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000027', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-19T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #39 for Cecilia Gomez.', NULL, 'DEMO-3-0039', TIMESTAMPTZ '2023-12-19T00:00:00Z', 3, 'Cecilia''s Item #39', 1, NULL, NULL, 2, 53.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000028', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-18T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #40 for Cecilia Gomez.', NULL, 'DEMO-3-0040', TIMESTAMPTZ '2023-12-18T00:00:00Z', 3, 'Cecilia''s Item #40', 1, NULL, NULL, 2, 54.99, 5, TRUE, 54.99, 49.49);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000029', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-17T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #41 for Cecilia Gomez.', NULL, 'DEMO-3-0041', TIMESTAMPTZ '2023-12-17T00:00:00Z', 3, 'Cecilia''s Item #41', 1, NULL, NULL, 2, 55.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000002a', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-16T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #42 for Cecilia Gomez.', NULL, 'DEMO-3-0042', TIMESTAMPTZ '2023-12-16T00:00:00Z', 3, 'Cecilia''s Item #42', 1, NULL, NULL, 2, 56.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000002b', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-15T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #43 for Cecilia Gomez.', NULL, 'DEMO-3-0043', TIMESTAMPTZ '2023-12-15T00:00:00Z', 3, 'Cecilia''s Item #43', 1, NULL, NULL, 2, 57.99, 3, TRUE, 57.99, 52.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000002c', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-14T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #44 for Cecilia Gomez.', NULL, 'DEMO-3-0044', TIMESTAMPTZ '2023-12-14T00:00:00Z', 3, 'Cecilia''s Item #44', 1, NULL, NULL, 2, 58.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000002d', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-13T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #45 for Cecilia Gomez.', NULL, 'DEMO-3-0045', TIMESTAMPTZ '2023-12-13T00:00:00Z', 3, 'Cecilia''s Item #45', 1, NULL, NULL, 2, 59.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000002e', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-12T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #46 for Cecilia Gomez.', NULL, 'DEMO-3-0046', TIMESTAMPTZ '2023-12-12T00:00:00Z', 3, 'Cecilia''s Item #46', 1, NULL, NULL, 2, 45.99, 1, TRUE, 45.99, 41.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000002f', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-11T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #47 for Cecilia Gomez.', NULL, 'DEMO-3-0047', TIMESTAMPTZ '2023-12-11T00:00:00Z', 3, 'Cecilia''s Item #47', 1, NULL, NULL, 2, 46.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000030', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-10T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #48 for Cecilia Gomez.', NULL, 'DEMO-3-0048', TIMESTAMPTZ '2023-12-10T00:00:00Z', 3, 'Cecilia''s Item #48', 1, NULL, NULL, 2, 47.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000031', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-09T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #49 for Cecilia Gomez.', NULL, 'DEMO-3-0049', TIMESTAMPTZ '2023-12-09T00:00:00Z', 3, 'Cecilia''s Item #49', 1, NULL, NULL, 2, 48.99, 4, TRUE, 48.99, 44.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000032', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-08T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #50 for Cecilia Gomez.', NULL, 'DEMO-3-0050', TIMESTAMPTZ '2023-12-08T00:00:00Z', 3, 'Cecilia''s Item #50', 1, NULL, NULL, 2, 49.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000033', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-07T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #51 for Cecilia Gomez.', NULL, 'DEMO-3-0051', TIMESTAMPTZ '2023-12-07T00:00:00Z', 3, 'Cecilia''s Item #51', 1, NULL, NULL, 2, 50.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000034', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-06T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #52 for Cecilia Gomez.', NULL, 'DEMO-3-0052', TIMESTAMPTZ '2023-12-06T00:00:00Z', 3, 'Cecilia''s Item #52', 1, NULL, NULL, 2, 51.99, 2, TRUE, 51.99, 46.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000035', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-05T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #53 for Cecilia Gomez.', NULL, 'DEMO-3-0053', TIMESTAMPTZ '2023-12-05T00:00:00Z', 3, 'Cecilia''s Item #53', 1, NULL, NULL, 2, 52.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000036', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-04T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #54 for Cecilia Gomez.', NULL, 'DEMO-3-0054', TIMESTAMPTZ '2023-12-04T00:00:00Z', 3, 'Cecilia''s Item #54', 1, NULL, NULL, 2, 53.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000037', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-03T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #55 for Cecilia Gomez.', NULL, 'DEMO-3-0055', TIMESTAMPTZ '2023-12-03T00:00:00Z', 3, 'Cecilia''s Item #55', 1, NULL, NULL, 2, 54.99, 5, TRUE, 54.99, 49.49);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000038', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-02T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #56 for Cecilia Gomez.', NULL, 'DEMO-3-0056', TIMESTAMPTZ '2023-12-02T00:00:00Z', 3, 'Cecilia''s Item #56', 1, NULL, NULL, 2, 55.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000039', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-01T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #57 for Cecilia Gomez.', NULL, 'DEMO-3-0057', TIMESTAMPTZ '2023-12-01T00:00:00Z', 3, 'Cecilia''s Item #57', 1, NULL, NULL, 2, 56.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000003a', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-30T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #58 for Cecilia Gomez.', NULL, 'DEMO-3-0058', TIMESTAMPTZ '2023-11-30T00:00:00Z', 3, 'Cecilia''s Item #58', 1, NULL, NULL, 2, 57.99, 3, TRUE, 57.99, 52.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000003b', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-29T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #59 for Cecilia Gomez.', NULL, 'DEMO-3-0059', TIMESTAMPTZ '2023-11-29T00:00:00Z', 3, 'Cecilia''s Item #59', 1, NULL, NULL, 2, 58.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000003c', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-28T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #60 for Cecilia Gomez.', NULL, 'DEMO-3-0060', TIMESTAMPTZ '2023-11-28T00:00:00Z', 3, 'Cecilia''s Item #60', 1, NULL, NULL, 2, 59.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000003d', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-27T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #61 for Cecilia Gomez.', NULL, 'DEMO-3-0061', TIMESTAMPTZ '2023-11-27T00:00:00Z', 3, 'Cecilia''s Item #61', 1, NULL, NULL, 2, 45.99, 1, TRUE, 45.99, 41.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000003e', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-26T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #62 for Cecilia Gomez.', NULL, 'DEMO-3-0062', TIMESTAMPTZ '2023-11-26T00:00:00Z', 3, 'Cecilia''s Item #62', 1, NULL, NULL, 2, 46.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000003f', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-25T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #63 for Cecilia Gomez.', NULL, 'DEMO-3-0063', TIMESTAMPTZ '2023-11-25T00:00:00Z', 3, 'Cecilia''s Item #63', 1, NULL, NULL, 2, 47.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000040', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-24T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #64 for Cecilia Gomez.', NULL, 'DEMO-3-0064', TIMESTAMPTZ '2023-11-24T00:00:00Z', 3, 'Cecilia''s Item #64', 1, NULL, NULL, 2, 48.99, 4, TRUE, 48.99, 44.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000041', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-23T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #65 for Cecilia Gomez.', NULL, 'DEMO-3-0065', TIMESTAMPTZ '2023-11-23T00:00:00Z', 3, 'Cecilia''s Item #65', 1, NULL, NULL, 2, 49.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000042', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-11-22T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #66 for Cecilia Gomez.', NULL, 'DEMO-3-0066', TIMESTAMPTZ '2023-11-22T00:00:00Z', 3, 'Cecilia''s Item #66', 1, NULL, NULL, 2, 50.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000043', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-11-21T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #67 for Cecilia Gomez.', NULL, 'DEMO-3-0067', TIMESTAMPTZ '2023-11-21T00:00:00Z', 3, 'Cecilia''s Item #67', 1, NULL, NULL, 2, 51.99, 2, TRUE, 51.99, 46.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000044', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-11-20T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #68 for Cecilia Gomez.', NULL, 'DEMO-3-0068', TIMESTAMPTZ '2023-11-20T00:00:00Z', 3, 'Cecilia''s Item #68', 1, NULL, NULL, 2, 52.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000045', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-11-19T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #69 for Cecilia Gomez.', NULL, 'DEMO-3-0069', TIMESTAMPTZ '2023-11-19T00:00:00Z', 3, 'Cecilia''s Item #69', 1, NULL, NULL, 2, 53.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000046', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-11-18T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #70 for Cecilia Gomez.', NULL, 'DEMO-3-0070', TIMESTAMPTZ '2023-11-18T00:00:00Z', 3, 'Cecilia''s Item #70', 1, NULL, NULL, 2, 54.99, 5, TRUE, 54.99, 49.49);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000047', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #71 for Cecilia Gomez.', NULL, 'DEMO-3-0071', TIMESTAMPTZ '2024-01-01T00:00:00Z', 3, 'Cecilia''s Item #71', 1, NULL, NULL, 2, 55.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000048', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-31T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #72 for Cecilia Gomez.', NULL, 'DEMO-3-0072', TIMESTAMPTZ '2023-12-31T00:00:00Z', 3, 'Cecilia''s Item #72', 1, NULL, NULL, 2, 56.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000049', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-30T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #73 for Cecilia Gomez.', NULL, 'DEMO-3-0073', TIMESTAMPTZ '2023-12-30T00:00:00Z', 3, 'Cecilia''s Item #73', 1, NULL, NULL, 2, 57.99, 3, TRUE, 57.99, 52.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000004a', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-29T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #74 for Cecilia Gomez.', NULL, 'DEMO-3-0074', TIMESTAMPTZ '2023-12-29T00:00:00Z', 3, 'Cecilia''s Item #74', 1, NULL, NULL, 2, 58.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000004b', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-28T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #75 for Cecilia Gomez.', NULL, 'DEMO-3-0075', TIMESTAMPTZ '2023-12-28T00:00:00Z', 3, 'Cecilia''s Item #75', 1, NULL, NULL, 2, 59.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000004c', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-27T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #76 for Cecilia Gomez.', NULL, 'DEMO-3-0076', TIMESTAMPTZ '2023-12-27T00:00:00Z', 3, 'Cecilia''s Item #76', 1, NULL, NULL, 2, 45.99, 1, TRUE, 45.99, 41.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000004d', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-26T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #77 for Cecilia Gomez.', NULL, 'DEMO-3-0077', TIMESTAMPTZ '2023-12-26T00:00:00Z', 3, 'Cecilia''s Item #77', 1, NULL, NULL, 2, 46.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000004e', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-25T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #78 for Cecilia Gomez.', NULL, 'DEMO-3-0078', TIMESTAMPTZ '2023-12-25T00:00:00Z', 3, 'Cecilia''s Item #78', 1, NULL, NULL, 2, 47.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000004f', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-24T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #79 for Cecilia Gomez.', NULL, 'DEMO-3-0079', TIMESTAMPTZ '2023-12-24T00:00:00Z', 3, 'Cecilia''s Item #79', 1, NULL, NULL, 2, 48.99, 4, TRUE, 48.99, 44.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000050', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-23T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #80 for Cecilia Gomez.', NULL, 'DEMO-3-0080', TIMESTAMPTZ '2023-12-23T00:00:00Z', 3, 'Cecilia''s Item #80', 1, NULL, NULL, 2, 49.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000051', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-22T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #81 for Cecilia Gomez.', NULL, 'DEMO-3-0081', TIMESTAMPTZ '2023-12-22T00:00:00Z', 3, 'Cecilia''s Item #81', 1, NULL, NULL, 2, 50.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000052', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-21T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #82 for Cecilia Gomez.', NULL, 'DEMO-3-0082', TIMESTAMPTZ '2023-12-21T00:00:00Z', 3, 'Cecilia''s Item #82', 1, NULL, NULL, 2, 51.99, 2, TRUE, 51.99, 46.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000053', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-20T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #83 for Cecilia Gomez.', NULL, 'DEMO-3-0083', TIMESTAMPTZ '2023-12-20T00:00:00Z', 3, 'Cecilia''s Item #83', 1, NULL, NULL, 2, 52.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000054', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-19T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #84 for Cecilia Gomez.', NULL, 'DEMO-3-0084', TIMESTAMPTZ '2023-12-19T00:00:00Z', 3, 'Cecilia''s Item #84', 1, NULL, NULL, 2, 53.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000055', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-18T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #85 for Cecilia Gomez.', NULL, 'DEMO-3-0085', TIMESTAMPTZ '2023-12-18T00:00:00Z', 3, 'Cecilia''s Item #85', 1, NULL, NULL, 2, 54.99, 5, TRUE, 54.99, 49.49);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000056', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-17T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #86 for Cecilia Gomez.', NULL, 'DEMO-3-0086', TIMESTAMPTZ '2023-12-17T00:00:00Z', 3, 'Cecilia''s Item #86', 1, NULL, NULL, 2, 55.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000057', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-16T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #87 for Cecilia Gomez.', NULL, 'DEMO-3-0087', TIMESTAMPTZ '2023-12-16T00:00:00Z', 3, 'Cecilia''s Item #87', 1, NULL, NULL, 2, 56.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000058', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-15T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #88 for Cecilia Gomez.', NULL, 'DEMO-3-0088', TIMESTAMPTZ '2023-12-15T00:00:00Z', 3, 'Cecilia''s Item #88', 1, NULL, NULL, 2, 57.99, 3, TRUE, 57.99, 52.19);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000059', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-14T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #89 for Cecilia Gomez.', NULL, 'DEMO-3-0089', TIMESTAMPTZ '2023-12-14T00:00:00Z', 3, 'Cecilia''s Item #89', 1, NULL, NULL, 2, 58.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000005a', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-13T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #90 for Cecilia Gomez.', NULL, 'DEMO-3-0090', TIMESTAMPTZ '2023-12-13T00:00:00Z', 3, 'Cecilia''s Item #90', 1, NULL, NULL, 2, 59.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000005b', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-12T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #91 for Cecilia Gomez.', NULL, 'DEMO-3-0091', TIMESTAMPTZ '2023-12-12T00:00:00Z', 3, 'Cecilia''s Item #91', 1, NULL, NULL, 2, 45.99, 1, TRUE, 45.99, 41.39);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000005c', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-11T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #92 for Cecilia Gomez.', NULL, 'DEMO-3-0092', TIMESTAMPTZ '2023-12-11T00:00:00Z', 3, 'Cecilia''s Item #92', 1, NULL, NULL, 2, 46.99, 2, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000005d', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-10T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #93 for Cecilia Gomez.', NULL, 'DEMO-3-0093', TIMESTAMPTZ '2023-12-10T00:00:00Z', 3, 'Cecilia''s Item #93', 1, NULL, NULL, 2, 47.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000005e', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-09T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #94 for Cecilia Gomez.', NULL, 'DEMO-3-0094', TIMESTAMPTZ '2023-12-09T00:00:00Z', 3, 'Cecilia''s Item #94', 1, NULL, NULL, 2, 48.99, 4, TRUE, 48.99, 44.09);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-00000000005f', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-08T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #95 for Cecilia Gomez.', NULL, 'DEMO-3-0095', TIMESTAMPTZ '2023-12-08T00:00:00Z', 3, 'Cecilia''s Item #95', 1, NULL, NULL, 2, 49.99, 5, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000060', '10000000-0000-0000-0000-000000000004', 'New with retail tags attached.', '40000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2023-12-07T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #96 for Cecilia Gomez.', NULL, 'DEMO-3-0096', TIMESTAMPTZ '2023-12-07T00:00:00Z', 3, 'Cecilia''s Item #96', 1, NULL, NULL, 2, 50.99, 1, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000061', '20000000-0000-0000-0000-000000000002', 'Open box item inspected for quality.', '40000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2023-12-06T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #97 for Cecilia Gomez.', NULL, 'DEMO-3-0097', TIMESTAMPTZ '2023-12-06T00:00:00Z', 3, 'Cecilia''s Item #97', 1, NULL, NULL, 2, 51.99, 2, TRUE, 51.99, 46.79);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000062', '30000000-0000-0000-0000-000000000002', 'Gently used and fully functional.', '40000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2023-12-05T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #98 for Cecilia Gomez.', NULL, 'DEMO-3-0098', TIMESTAMPTZ '2023-12-05T00:00:00Z', 3, 'Cecilia''s Item #98', 1, NULL, NULL, 2, 52.99, 3, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000063', '10000000-0000-0000-0000-000000000002', 'Used item in working condition.', '40000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2023-12-04T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #99 for Cecilia Gomez.', NULL, 'DEMO-3-0099', TIMESTAMPTZ '2023-12-04T00:00:00Z', 3, 'Cecilia''s Item #99', 1, NULL, NULL, 2, 53.99, 4, FALSE, NULL, NULL);
    INSERT INTO listing (id, category_id, condition_description, condition_id, created_at, created_by, draft_expired_at, duration, end_date, format, is_deleted, listing_description, scheduled_start_time, sku, start_date, status, title, type, updated_at, updated_by, listing_format, pricing_price, pricing_quantity, offer_settings_allow_offers, offer_settings_auto_accept_offer, offer_settings_minimum_offer)
    VALUES ('73000000-0000-0000-0000-000000000064', '10000000-0000-0000-0000-000000000003', 'Brand new condition with original packaging.', '40000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2023-12-03T00:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, 0, NULL, 2, FALSE, 'Curated demo listing #100 for Cecilia Gomez.', NULL, 'DEMO-3-0100', TIMESTAMPTZ '2023-12-03T00:00:00Z', 3, 'Cecilia''s Item #100', 1, NULL, NULL, 2, 54.99, 5, TRUE, 54.99, 49.49);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO listing_template (id, created_at, created_by, description, format_label, is_deleted, name, payload_json, thumbnail_url, updated_at, updated_by)
    VALUES ('81000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000001', 'Reusable template seeded for demo purposes.', 'Fixed Price', FALSE, 'Alice''s Starter Template', '{"title":"Sample Listing Template","price":49.99,"quantity":5,"categoryId":"10000000-0000-0000-0000-000000000002","conditionId":"40000000-0000-0000-0000-000000000001"}', 'https://picsum.photos/seed/template-1/320/180', NULL, NULL);
    INSERT INTO listing_template (id, created_at, created_by, description, format_label, is_deleted, name, payload_json, thumbnail_url, updated_at, updated_by)
    VALUES ('82000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000002', 'Reusable template seeded for demo purposes.', 'Fixed Price', FALSE, 'Brian''s Starter Template', '{"title":"Sample Listing Template","price":49.99,"quantity":5,"categoryId":"10000000-0000-0000-0000-000000000003","conditionId":"40000000-0000-0000-0000-000000000001"}', 'https://picsum.photos/seed/template-2/320/180', NULL, NULL);
    INSERT INTO listing_template (id, created_at, created_by, description, format_label, is_deleted, name, payload_json, thumbnail_url, updated_at, updated_by)
    VALUES ('83000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', '70000000-0000-0000-0000-000000000003', 'Reusable template seeded for demo purposes.', 'Fixed Price', FALSE, 'Cecilia''s Starter Template', '{"title":"Sample Listing Template","price":49.99,"quantity":5,"categoryId":"10000000-0000-0000-0000-000000000004","conditionId":"40000000-0000-0000-0000-000000000001"}', 'https://picsum.photos/seed/template-3/320/180', NULL, NULL);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e', 'Archived', '#64748b', 'Order archived', 'Archived', 11);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1', 'Draft', '#94a3b8', 'Order created but not submitted', 'Draft', 0);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', 'AwaitingShipmentShipWithin24h', '#fbbf24', 'Must ship within 24 hours', 'Ship within 24h', 4);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('4d128ab1-64a7-4c65-b8f5-434a258f0c52', 'AwaitingPayment', '#fb923c', 'Order awaits buyer payment', 'Awaiting payment', 1);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 'PaidAndShipped', '#10b981', 'Order shipped to buyer', 'Paid & shipped', 6);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9', 'PaidAwaitingFeedback', '#a855f7', 'Waiting for buyer feedback', 'Awaiting feedback', 7);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('859b47f4-0d05-4f43-8ff5-57acb8d5da1d', 'AwaitingExpeditedShipment', '#22c55e', 'Expedited shipping requested', 'Expedited shipment', 5);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('949ce7f8-6d6b-4d65-9032-b9f51c4508eb', 'Delivered', '#0ea5e9', 'Package delivered to buyer', 'Delivered', 9);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('970c8d97-6081-43db-9083-8f3c026ded84', 'DeliveryFailed', '#f97316', 'Delivery attempt unsuccessful', 'Delivery failed', 10);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'AwaitingShipment', '#3b82f6', 'Payment received; waiting to ship', 'Awaiting shipment', 2);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', 'Cancelled', '#ef4444', 'Order cancelled', 'Cancelled', 12);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('c21a6b64-f0e9-4947-8b1b-38ef45aa4930', 'ShippedAwaitingFeedback', '#38bdf8', 'Shipped and awaiting buyer confirmation', 'Shipped - awaiting feedback', 8);
    INSERT INTO order_statuses (id, code, color, description, name, sort_order)
    VALUES ('dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', 'AwaitingShipmentOverdue', '#ef4444', 'Shipment overdue based on handling time', 'Shipment overdue', 3);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO "user" (id, created_at, created_by, email, full_name, is_deleted, is_email_verified, is_payment_verified, password_hash, performance_level, updated_at, updated_by, username, active_total_value)
    VALUES ('70000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', 'System', 'demo.seller1@example.com', 'Alice Johnson', FALSE, TRUE, TRUE, '$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq', 'TopRated', NULL, NULL, 'demo.seller1@example.com', 11222.0);
    INSERT INTO "user" (id, created_at, created_by, email, full_name, is_deleted, is_email_verified, is_payment_verified, password_hash, performance_level, updated_at, updated_by, username, active_total_value)
    VALUES ('70000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', 'System', 'demo.seller2@example.com', 'Brian Carter', FALSE, TRUE, TRUE, '$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq', 'TopRated', NULL, NULL, 'demo.seller2@example.com', 13622.0);
    INSERT INTO "user" (id, created_at, created_by, email, full_name, is_deleted, is_email_verified, is_payment_verified, password_hash, performance_level, updated_at, updated_by, username, active_total_value)
    VALUES ('70000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', 'System', 'demo.seller3@example.com', 'Cecilia Gomez', FALSE, TRUE, TRUE, '$2a$11$sEm9a1Ghk4K9ivLYrj2iS.JAQL1EsY2YnfaX8P4fhYVKlbP8GljJq', 'TopRated', NULL, NULL, 'demo.seller3@example.com', 16022.0);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Smartphones and cell phone devices.', FALSE, 'Cell Phones & Smartphones', '10000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Portable computers and accessories.', FALSE, 'Laptops & Netbooks', '10000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Digital cameras and photography equipment.', FALSE, 'Cameras & Photo', '10000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('20000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Performance and casual athletic footwear for men.', FALSE, 'Men''s Athletic Shoes', '20000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('20000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Dresses for every style and occasion.', FALSE, 'Women''s Dresses', '20000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('30000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Countertop appliances and kitchen helpers.', FALSE, 'Small Kitchen Appliances', '30000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('30000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Indoor and outdoor furniture collections.', FALSE, 'Furniture', '30000000-0000-0000-0000-000000000001', NULL, NULL);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000019', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000001a', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000001b', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000001c', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000001d', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000001e', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000001f', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000020', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000021', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000022', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000023', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000024', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000025', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000026', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000027', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000028', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000029', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000002a', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000002b', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000002c', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000002d', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000002e', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000002f', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000030', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000031', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000032', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000033', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000034', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000035', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000036', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000037', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000038', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000039', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000003a', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000003b', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000003c', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000003d', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000003e', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000003f', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000040', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000041', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000042', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000043', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000044', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000045', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000046', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000047', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000048', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000049', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000004a', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000004b', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000004c', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000004d', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000004e', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000004f', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000050', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000051', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000052', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000053', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000054', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000055', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000056', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000057', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000058', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000059', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000005a', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000005b', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000005c', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000005d', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000005e', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-00000000005f', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000060', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000061', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000062', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000063', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('71000000-0000-0000-0000-000000000064', '70000000-0000-0000-0000-000000000001');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000019', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000001a', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000001b', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000001c', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000001d', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000001e', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000001f', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000020', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000021', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000022', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000023', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000024', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000025', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000026', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000027', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000028', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000029', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000002a', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000002b', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000002c', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000002d', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000002e', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000002f', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000030', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000031', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000032', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000033', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000034', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000035', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000036', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000037', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000038', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000039', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000003a', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000003b', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000003c', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000003d', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000003e', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000003f', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000040', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000041', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000042', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000043', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000044', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000045', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000046', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000047', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000048', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000049', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000004a', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000004b', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000004c', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000004d', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000004e', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000004f', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000050', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000051', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000052', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000053', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000054', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000055', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000056', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000057', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000058', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000059', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000005a', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000005b', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000005c', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000005d', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000005e', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-00000000005f', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000060', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000061', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000062', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000063', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('72000000-0000-0000-0000-000000000064', '70000000-0000-0000-0000-000000000002');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000001', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000002', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000003', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000004', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000005', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000006', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000007', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000008', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000009', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000000a', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000000b', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000000c', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000000d', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000000e', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000000f', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000010', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000011', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000012', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000013', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000014', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000015', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000016', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000017', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000018', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000019', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000001a', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000001b', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000001c', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000001d', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000001e', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000001f', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000020', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000021', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000022', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000023', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000024', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000025', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000026', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000027', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000028', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000029', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000002a', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000002b', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000002c', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000002d', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000002e', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000002f', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000030', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000031', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000032', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000033', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000034', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000035', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000036', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000037', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000038', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000039', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000003a', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000003b', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000003c', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000003d', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000003e', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000003f', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000040', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000041', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000042', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000043', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000044', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000045', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000046', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000047', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000048', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000049', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000004a', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000004b', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000004c', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000004d', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000004e', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000004f', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000050', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000051', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000052', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000053', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000054', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000055', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000056', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000057', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000058', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000059', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000005a', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000005b', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000005c', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000005d', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000005e', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-00000000005f', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000060', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000061', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000062', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000063', '70000000-0000-0000-0000-000000000003');
    INSERT INTO listing_id (listing_id, seller_id)
    VALUES ('73000000-0000-0000-0000-000000000064', '70000000-0000-0000-0000-000000000003');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000001', TRUE, 'https://picsum.photos/seed/1-1/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000002', TRUE, 'https://picsum.photos/seed/1-2/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000003', TRUE, 'https://picsum.photos/seed/1-3/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000004', TRUE, 'https://picsum.photos/seed/1-4/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000005', TRUE, 'https://picsum.photos/seed/1-5/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000006', TRUE, 'https://picsum.photos/seed/1-6/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000007', TRUE, 'https://picsum.photos/seed/1-7/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000008', TRUE, 'https://picsum.photos/seed/1-8/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000009', TRUE, 'https://picsum.photos/seed/1-9/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000000a', TRUE, 'https://picsum.photos/seed/1-10/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000000b', TRUE, 'https://picsum.photos/seed/1-11/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000000c', TRUE, 'https://picsum.photos/seed/1-12/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000000d', TRUE, 'https://picsum.photos/seed/1-13/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000000e', TRUE, 'https://picsum.photos/seed/1-14/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000000f', TRUE, 'https://picsum.photos/seed/1-15/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000010', TRUE, 'https://picsum.photos/seed/1-16/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000011', TRUE, 'https://picsum.photos/seed/1-17/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000012', TRUE, 'https://picsum.photos/seed/1-18/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000013', TRUE, 'https://picsum.photos/seed/1-19/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000014', TRUE, 'https://picsum.photos/seed/1-20/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000015', TRUE, 'https://picsum.photos/seed/1-21/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000016', TRUE, 'https://picsum.photos/seed/1-22/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000017', TRUE, 'https://picsum.photos/seed/1-23/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000018', TRUE, 'https://picsum.photos/seed/1-24/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000019', TRUE, 'https://picsum.photos/seed/1-25/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000001a', TRUE, 'https://picsum.photos/seed/1-26/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000001b', TRUE, 'https://picsum.photos/seed/1-27/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000001c', TRUE, 'https://picsum.photos/seed/1-28/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000001d', TRUE, 'https://picsum.photos/seed/1-29/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000001e', TRUE, 'https://picsum.photos/seed/1-30/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000001f', TRUE, 'https://picsum.photos/seed/1-31/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000020', TRUE, 'https://picsum.photos/seed/1-32/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000021', TRUE, 'https://picsum.photos/seed/1-33/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000022', TRUE, 'https://picsum.photos/seed/1-34/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000023', TRUE, 'https://picsum.photos/seed/1-35/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000024', TRUE, 'https://picsum.photos/seed/1-36/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000025', TRUE, 'https://picsum.photos/seed/1-37/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000026', TRUE, 'https://picsum.photos/seed/1-38/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000027', TRUE, 'https://picsum.photos/seed/1-39/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000028', TRUE, 'https://picsum.photos/seed/1-40/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000029', TRUE, 'https://picsum.photos/seed/1-41/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000002a', TRUE, 'https://picsum.photos/seed/1-42/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000002b', TRUE, 'https://picsum.photos/seed/1-43/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000002c', TRUE, 'https://picsum.photos/seed/1-44/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000002d', TRUE, 'https://picsum.photos/seed/1-45/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000002e', TRUE, 'https://picsum.photos/seed/1-46/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000002f', TRUE, 'https://picsum.photos/seed/1-47/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000030', TRUE, 'https://picsum.photos/seed/1-48/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000031', TRUE, 'https://picsum.photos/seed/1-49/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000032', TRUE, 'https://picsum.photos/seed/1-50/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000033', TRUE, 'https://picsum.photos/seed/1-51/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000034', TRUE, 'https://picsum.photos/seed/1-52/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000035', TRUE, 'https://picsum.photos/seed/1-53/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000036', TRUE, 'https://picsum.photos/seed/1-54/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000037', TRUE, 'https://picsum.photos/seed/1-55/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000038', TRUE, 'https://picsum.photos/seed/1-56/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000039', TRUE, 'https://picsum.photos/seed/1-57/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000003a', TRUE, 'https://picsum.photos/seed/1-58/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000003b', TRUE, 'https://picsum.photos/seed/1-59/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000003c', TRUE, 'https://picsum.photos/seed/1-60/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000003d', TRUE, 'https://picsum.photos/seed/1-61/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000003e', TRUE, 'https://picsum.photos/seed/1-62/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000003f', TRUE, 'https://picsum.photos/seed/1-63/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000040', TRUE, 'https://picsum.photos/seed/1-64/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000041', TRUE, 'https://picsum.photos/seed/1-65/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000042', TRUE, 'https://picsum.photos/seed/1-66/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000043', TRUE, 'https://picsum.photos/seed/1-67/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000044', TRUE, 'https://picsum.photos/seed/1-68/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000045', TRUE, 'https://picsum.photos/seed/1-69/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000046', TRUE, 'https://picsum.photos/seed/1-70/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000047', TRUE, 'https://picsum.photos/seed/1-71/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000048', TRUE, 'https://picsum.photos/seed/1-72/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000049', TRUE, 'https://picsum.photos/seed/1-73/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000004a', TRUE, 'https://picsum.photos/seed/1-74/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000004b', TRUE, 'https://picsum.photos/seed/1-75/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000004c', TRUE, 'https://picsum.photos/seed/1-76/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000004d', TRUE, 'https://picsum.photos/seed/1-77/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000004e', TRUE, 'https://picsum.photos/seed/1-78/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000004f', TRUE, 'https://picsum.photos/seed/1-79/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000050', TRUE, 'https://picsum.photos/seed/1-80/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000051', TRUE, 'https://picsum.photos/seed/1-81/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000052', TRUE, 'https://picsum.photos/seed/1-82/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000053', TRUE, 'https://picsum.photos/seed/1-83/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000054', TRUE, 'https://picsum.photos/seed/1-84/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000055', TRUE, 'https://picsum.photos/seed/1-85/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000056', TRUE, 'https://picsum.photos/seed/1-86/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000057', TRUE, 'https://picsum.photos/seed/1-87/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000058', TRUE, 'https://picsum.photos/seed/1-88/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000059', TRUE, 'https://picsum.photos/seed/1-89/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000005a', TRUE, 'https://picsum.photos/seed/1-90/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000005b', TRUE, 'https://picsum.photos/seed/1-91/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000005c', TRUE, 'https://picsum.photos/seed/1-92/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000005d', TRUE, 'https://picsum.photos/seed/1-93/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000005e', TRUE, 'https://picsum.photos/seed/1-94/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-00000000005f', TRUE, 'https://picsum.photos/seed/1-95/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000060', TRUE, 'https://picsum.photos/seed/1-96/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000061', TRUE, 'https://picsum.photos/seed/1-97/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000062', TRUE, 'https://picsum.photos/seed/1-98/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000063', TRUE, 'https://picsum.photos/seed/1-99/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '71000000-0000-0000-0000-000000000064', TRUE, 'https://picsum.photos/seed/1-100/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000001', TRUE, 'https://picsum.photos/seed/2-1/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000002', TRUE, 'https://picsum.photos/seed/2-2/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000003', TRUE, 'https://picsum.photos/seed/2-3/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000004', TRUE, 'https://picsum.photos/seed/2-4/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000005', TRUE, 'https://picsum.photos/seed/2-5/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000006', TRUE, 'https://picsum.photos/seed/2-6/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000007', TRUE, 'https://picsum.photos/seed/2-7/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000008', TRUE, 'https://picsum.photos/seed/2-8/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000009', TRUE, 'https://picsum.photos/seed/2-9/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000000a', TRUE, 'https://picsum.photos/seed/2-10/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000000b', TRUE, 'https://picsum.photos/seed/2-11/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000000c', TRUE, 'https://picsum.photos/seed/2-12/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000000d', TRUE, 'https://picsum.photos/seed/2-13/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000000e', TRUE, 'https://picsum.photos/seed/2-14/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000000f', TRUE, 'https://picsum.photos/seed/2-15/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000010', TRUE, 'https://picsum.photos/seed/2-16/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000011', TRUE, 'https://picsum.photos/seed/2-17/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000012', TRUE, 'https://picsum.photos/seed/2-18/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000013', TRUE, 'https://picsum.photos/seed/2-19/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000014', TRUE, 'https://picsum.photos/seed/2-20/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000015', TRUE, 'https://picsum.photos/seed/2-21/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000016', TRUE, 'https://picsum.photos/seed/2-22/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000017', TRUE, 'https://picsum.photos/seed/2-23/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000018', TRUE, 'https://picsum.photos/seed/2-24/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000019', TRUE, 'https://picsum.photos/seed/2-25/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000001a', TRUE, 'https://picsum.photos/seed/2-26/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000001b', TRUE, 'https://picsum.photos/seed/2-27/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000001c', TRUE, 'https://picsum.photos/seed/2-28/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000001d', TRUE, 'https://picsum.photos/seed/2-29/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000001e', TRUE, 'https://picsum.photos/seed/2-30/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000001f', TRUE, 'https://picsum.photos/seed/2-31/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000020', TRUE, 'https://picsum.photos/seed/2-32/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000021', TRUE, 'https://picsum.photos/seed/2-33/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000022', TRUE, 'https://picsum.photos/seed/2-34/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000023', TRUE, 'https://picsum.photos/seed/2-35/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000024', TRUE, 'https://picsum.photos/seed/2-36/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000025', TRUE, 'https://picsum.photos/seed/2-37/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000026', TRUE, 'https://picsum.photos/seed/2-38/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000027', TRUE, 'https://picsum.photos/seed/2-39/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000028', TRUE, 'https://picsum.photos/seed/2-40/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000029', TRUE, 'https://picsum.photos/seed/2-41/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000002a', TRUE, 'https://picsum.photos/seed/2-42/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000002b', TRUE, 'https://picsum.photos/seed/2-43/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000002c', TRUE, 'https://picsum.photos/seed/2-44/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000002d', TRUE, 'https://picsum.photos/seed/2-45/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000002e', TRUE, 'https://picsum.photos/seed/2-46/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000002f', TRUE, 'https://picsum.photos/seed/2-47/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000030', TRUE, 'https://picsum.photos/seed/2-48/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000031', TRUE, 'https://picsum.photos/seed/2-49/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000032', TRUE, 'https://picsum.photos/seed/2-50/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000033', TRUE, 'https://picsum.photos/seed/2-51/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000034', TRUE, 'https://picsum.photos/seed/2-52/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000035', TRUE, 'https://picsum.photos/seed/2-53/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000036', TRUE, 'https://picsum.photos/seed/2-54/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000037', TRUE, 'https://picsum.photos/seed/2-55/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000038', TRUE, 'https://picsum.photos/seed/2-56/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000039', TRUE, 'https://picsum.photos/seed/2-57/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000003a', TRUE, 'https://picsum.photos/seed/2-58/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000003b', TRUE, 'https://picsum.photos/seed/2-59/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000003c', TRUE, 'https://picsum.photos/seed/2-60/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000003d', TRUE, 'https://picsum.photos/seed/2-61/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000003e', TRUE, 'https://picsum.photos/seed/2-62/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000003f', TRUE, 'https://picsum.photos/seed/2-63/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000040', TRUE, 'https://picsum.photos/seed/2-64/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000041', TRUE, 'https://picsum.photos/seed/2-65/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000042', TRUE, 'https://picsum.photos/seed/2-66/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000043', TRUE, 'https://picsum.photos/seed/2-67/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000044', TRUE, 'https://picsum.photos/seed/2-68/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000045', TRUE, 'https://picsum.photos/seed/2-69/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000046', TRUE, 'https://picsum.photos/seed/2-70/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000047', TRUE, 'https://picsum.photos/seed/2-71/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000048', TRUE, 'https://picsum.photos/seed/2-72/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000049', TRUE, 'https://picsum.photos/seed/2-73/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000004a', TRUE, 'https://picsum.photos/seed/2-74/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000004b', TRUE, 'https://picsum.photos/seed/2-75/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000004c', TRUE, 'https://picsum.photos/seed/2-76/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000004d', TRUE, 'https://picsum.photos/seed/2-77/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000004e', TRUE, 'https://picsum.photos/seed/2-78/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000004f', TRUE, 'https://picsum.photos/seed/2-79/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000050', TRUE, 'https://picsum.photos/seed/2-80/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000051', TRUE, 'https://picsum.photos/seed/2-81/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000052', TRUE, 'https://picsum.photos/seed/2-82/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000053', TRUE, 'https://picsum.photos/seed/2-83/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000054', TRUE, 'https://picsum.photos/seed/2-84/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000055', TRUE, 'https://picsum.photos/seed/2-85/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000056', TRUE, 'https://picsum.photos/seed/2-86/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000057', TRUE, 'https://picsum.photos/seed/2-87/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000058', TRUE, 'https://picsum.photos/seed/2-88/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000059', TRUE, 'https://picsum.photos/seed/2-89/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000005a', TRUE, 'https://picsum.photos/seed/2-90/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000005b', TRUE, 'https://picsum.photos/seed/2-91/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000005c', TRUE, 'https://picsum.photos/seed/2-92/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000005d', TRUE, 'https://picsum.photos/seed/2-93/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000005e', TRUE, 'https://picsum.photos/seed/2-94/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-00000000005f', TRUE, 'https://picsum.photos/seed/2-95/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000060', TRUE, 'https://picsum.photos/seed/2-96/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000061', TRUE, 'https://picsum.photos/seed/2-97/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000062', TRUE, 'https://picsum.photos/seed/2-98/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000063', TRUE, 'https://picsum.photos/seed/2-99/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '72000000-0000-0000-0000-000000000064', TRUE, 'https://picsum.photos/seed/2-100/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000001', TRUE, 'https://picsum.photos/seed/3-1/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000002', TRUE, 'https://picsum.photos/seed/3-2/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000003', TRUE, 'https://picsum.photos/seed/3-3/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000004', TRUE, 'https://picsum.photos/seed/3-4/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000005', TRUE, 'https://picsum.photos/seed/3-5/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000006', TRUE, 'https://picsum.photos/seed/3-6/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000007', TRUE, 'https://picsum.photos/seed/3-7/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000008', TRUE, 'https://picsum.photos/seed/3-8/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000009', TRUE, 'https://picsum.photos/seed/3-9/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000000a', TRUE, 'https://picsum.photos/seed/3-10/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000000b', TRUE, 'https://picsum.photos/seed/3-11/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000000c', TRUE, 'https://picsum.photos/seed/3-12/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000000d', TRUE, 'https://picsum.photos/seed/3-13/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000000e', TRUE, 'https://picsum.photos/seed/3-14/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000000f', TRUE, 'https://picsum.photos/seed/3-15/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000010', TRUE, 'https://picsum.photos/seed/3-16/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000011', TRUE, 'https://picsum.photos/seed/3-17/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000012', TRUE, 'https://picsum.photos/seed/3-18/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000013', TRUE, 'https://picsum.photos/seed/3-19/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000014', TRUE, 'https://picsum.photos/seed/3-20/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000015', TRUE, 'https://picsum.photos/seed/3-21/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000016', TRUE, 'https://picsum.photos/seed/3-22/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000017', TRUE, 'https://picsum.photos/seed/3-23/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000018', TRUE, 'https://picsum.photos/seed/3-24/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000019', TRUE, 'https://picsum.photos/seed/3-25/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000001a', TRUE, 'https://picsum.photos/seed/3-26/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000001b', TRUE, 'https://picsum.photos/seed/3-27/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000001c', TRUE, 'https://picsum.photos/seed/3-28/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000001d', TRUE, 'https://picsum.photos/seed/3-29/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000001e', TRUE, 'https://picsum.photos/seed/3-30/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000001f', TRUE, 'https://picsum.photos/seed/3-31/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000020', TRUE, 'https://picsum.photos/seed/3-32/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000021', TRUE, 'https://picsum.photos/seed/3-33/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000022', TRUE, 'https://picsum.photos/seed/3-34/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000023', TRUE, 'https://picsum.photos/seed/3-35/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000024', TRUE, 'https://picsum.photos/seed/3-36/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000025', TRUE, 'https://picsum.photos/seed/3-37/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000026', TRUE, 'https://picsum.photos/seed/3-38/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000027', TRUE, 'https://picsum.photos/seed/3-39/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000028', TRUE, 'https://picsum.photos/seed/3-40/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000029', TRUE, 'https://picsum.photos/seed/3-41/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000002a', TRUE, 'https://picsum.photos/seed/3-42/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000002b', TRUE, 'https://picsum.photos/seed/3-43/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000002c', TRUE, 'https://picsum.photos/seed/3-44/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000002d', TRUE, 'https://picsum.photos/seed/3-45/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000002e', TRUE, 'https://picsum.photos/seed/3-46/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000002f', TRUE, 'https://picsum.photos/seed/3-47/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000030', TRUE, 'https://picsum.photos/seed/3-48/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000031', TRUE, 'https://picsum.photos/seed/3-49/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000032', TRUE, 'https://picsum.photos/seed/3-50/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000033', TRUE, 'https://picsum.photos/seed/3-51/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000034', TRUE, 'https://picsum.photos/seed/3-52/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000035', TRUE, 'https://picsum.photos/seed/3-53/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000036', TRUE, 'https://picsum.photos/seed/3-54/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000037', TRUE, 'https://picsum.photos/seed/3-55/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000038', TRUE, 'https://picsum.photos/seed/3-56/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000039', TRUE, 'https://picsum.photos/seed/3-57/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000003a', TRUE, 'https://picsum.photos/seed/3-58/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000003b', TRUE, 'https://picsum.photos/seed/3-59/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000003c', TRUE, 'https://picsum.photos/seed/3-60/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000003d', TRUE, 'https://picsum.photos/seed/3-61/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000003e', TRUE, 'https://picsum.photos/seed/3-62/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000003f', TRUE, 'https://picsum.photos/seed/3-63/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000040', TRUE, 'https://picsum.photos/seed/3-64/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000041', TRUE, 'https://picsum.photos/seed/3-65/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000042', TRUE, 'https://picsum.photos/seed/3-66/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000043', TRUE, 'https://picsum.photos/seed/3-67/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000044', TRUE, 'https://picsum.photos/seed/3-68/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000045', TRUE, 'https://picsum.photos/seed/3-69/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000046', TRUE, 'https://picsum.photos/seed/3-70/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000047', TRUE, 'https://picsum.photos/seed/3-71/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000048', TRUE, 'https://picsum.photos/seed/3-72/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000049', TRUE, 'https://picsum.photos/seed/3-73/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000004a', TRUE, 'https://picsum.photos/seed/3-74/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000004b', TRUE, 'https://picsum.photos/seed/3-75/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000004c', TRUE, 'https://picsum.photos/seed/3-76/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000004d', TRUE, 'https://picsum.photos/seed/3-77/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000004e', TRUE, 'https://picsum.photos/seed/3-78/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000004f', TRUE, 'https://picsum.photos/seed/3-79/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000050', TRUE, 'https://picsum.photos/seed/3-80/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000051', TRUE, 'https://picsum.photos/seed/3-81/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000052', TRUE, 'https://picsum.photos/seed/3-82/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000053', TRUE, 'https://picsum.photos/seed/3-83/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000054', TRUE, 'https://picsum.photos/seed/3-84/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000055', TRUE, 'https://picsum.photos/seed/3-85/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000056', TRUE, 'https://picsum.photos/seed/3-86/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000057', TRUE, 'https://picsum.photos/seed/3-87/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000058', TRUE, 'https://picsum.photos/seed/3-88/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000059', TRUE, 'https://picsum.photos/seed/3-89/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000005a', TRUE, 'https://picsum.photos/seed/3-90/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000005b', TRUE, 'https://picsum.photos/seed/3-91/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000005c', TRUE, 'https://picsum.photos/seed/3-92/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000005d', TRUE, 'https://picsum.photos/seed/3-93/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000005e', TRUE, 'https://picsum.photos/seed/3-94/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-00000000005f', TRUE, 'https://picsum.photos/seed/3-95/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000060', TRUE, 'https://picsum.photos/seed/3-96/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000061', TRUE, 'https://picsum.photos/seed/3-97/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000062', TRUE, 'https://picsum.photos/seed/3-98/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000063', TRUE, 'https://picsum.photos/seed/3-99/640/640');
    INSERT INTO listing_image (id, listing_id, is_primary, url)
    VALUES (1, '73000000-0000-0000-0000-000000000064', TRUE, 'https://picsum.photos/seed/3-100/640/640');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('1b9c7e7f-9d15-41b0-9417-51c5723a7792', 'SELLER,SYSTEM', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '949ce7f8-6d6b-4d65-9032-b9f51c4508eb');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('2abdffad-037d-48a0-8c3d-a8dd0f00c5ba', 'SELLER,SYSTEM', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('3334f1c8-0fb7-4b17-974a-16f4f492ade4', 'SYSTEM', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('3cf5a7f5-8f3f-4dcb-907e-e4d27744ef98', 'SELLER,SYSTEM', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('42059f6f-8e43-4b6a-9b59-cf9670091b8f', 'SYSTEM', '4d128ab1-64a7-4c65-b8f5-434a258f0c52', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('55b5fadc-7f2f-4f43-ac4c-c6eb6f633d58', 'SELLER,SYSTEM', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('5c76de08-97eb-43d5-9c01-8c7c6262ec66', 'SELLER,BUYER,SYSTEM', '4d128ab1-64a7-4c65-b8f5-434a258f0c52', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('64648c83-2c87-47b8-8c2a-32e96c369f41', 'SELLER,SYSTEM', '859b47f4-0d05-4f43-8ff5-57acb8d5da1d', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('6cb6fa65-3d6c-45f0-9f27-cf5d292743ff', 'SELLER,SUPPORT,SYSTEM', '970c8d97-6081-43db-9083-8f3c026ded84', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('6fbe36c4-98e4-4d1d-8c3c-1ea29fe8d08c', 'SYSTEM', '5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('70e8c0f9-6fa8-4ff6-bb3f-5b53b22e2afd', 'SYSTEM', '949ce7f8-6d6b-4d65-9032-b9f51c4508eb', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('8ac18f4b-ea8d-4b72-b6cf-01c3d233cbea', 'SELLER,SYSTEM', '2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1', '4d128ab1-64a7-4c65-b8f5-434a258f0c52');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('94b12ce3-6d7c-4ea1-86f9-72f65e75d8de', 'SYSTEM', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '859b47f4-0d05-4f43-8ff5-57acb8d5da1d');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('a4c5df71-b5bb-4f13-9659-a5047cf4f087', 'SELLER,SUPPORT,SYSTEM', '970c8d97-6081-43db-9083-8f3c026ded84', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('b62c4a77-6a54-47d9-8d09-af22bd0caf23', 'SELLER,SYSTEM', '2e7f6b20-1b1f-4b7a-9de2-3c4a92f5e2a1', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('b8fa2c60-13ad-4e83-9516-8f406bcf8414', 'SELLER,SUPPORT,SYSTEM', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '970c8d97-6081-43db-9083-8f3c026ded84');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('c6a927ee-4fb6-48cc-bbf0-d2624de3458f', 'SYSTEM', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', '5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('ce68729c-6df0-466b-ae26-737d1b10dd93', 'SYSTEM', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930', '0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('d0cb2575-023a-45dc-840a-8e09b2f4c4c8', 'SYSTEM', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('d10a4517-efbb-4b8d-af6f-baf2b022a850', 'SYSTEM', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('ee0a6840-bf0b-46f3-9c41-96b5a91a02ab', 'SELLER,SYSTEM', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8');
    INSERT INTO order_status_transitions (id, "AllowedRoles", from_status_id, to_status_id)
    VALUES ('f5ceb762-3d65-4f6d-b052-053c55c1a08d', 'SYSTEM', 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', '0c6bd1f3-ac9c-4a68-92c5-efbc4dc91d3e');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('c721f605-43cb-4b1b-8f0c-b1c5833420a9', NULL, '70000000-0000-0000-0000-000000000001', NULL, 'OCTDEAL', TIMESTAMPTZ '2025-10-18T14:15:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1002', TIMESTAMPTZ '2025-10-18T14:15:00Z', TIMESTAMPTZ '2025-10-18T15:15:00Z', NULL, '70000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2025-10-19T02:15:00Z', 2, '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', TIMESTAMPTZ '2025-10-19T02:15:00Z', 'seed', 5.0, 'USD', 4.0, 'USD', 12.0, 'USD', 89.95, 'USD', 6.3, 'USD', 107.25, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('f6de3ce0-2d3d-4709-923d-cbb61f956947', NULL, '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-10-12T10:30:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1001', TIMESTAMPTZ '2025-10-12T10:30:00Z', TIMESTAMPTZ '2025-10-12T12:30:00Z', NULL, '70000000-0000-0000-0000-000000000001', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-10-12T12:30:00Z', 'seed', 0.0, 'USD', 5.25, 'USD', 8.5, 'USD', 104.99, 'USD', 7.1, 'USD', 125.84, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000002');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000003');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000005');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000002');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000003');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000005');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000005');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000007');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000008');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000007');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000008');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Apple","Samsung","Google","OnePlus","Motorola"]', '10000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10100000-0000-0000-0000-000000000002', FALSE, FALSE, 'Model', '["iPhone 15","Galaxy S24","Pixel 8","OnePlus 12","Moto G Power"]', '10000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10100000-0000-0000-0000-000000000003', FALSE, TRUE, 'Storage Capacity', '["64 GB","128 GB","256 GB","512 GB","1 TB"]', '10000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10100000-0000-0000-0000-000000000004', FALSE, FALSE, 'Color', '["Black","White","Blue","Red","Purple"]', '10000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10100000-0000-0000-0000-000000000005', FALSE, FALSE, 'Network', '["Unlocked","AT\u0026T","Verizon","T-Mobile","US Cellular"]', '10000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Apple","Dell","HP","Lenovo","ASUS"]', '10000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10200000-0000-0000-0000-000000000002', FALSE, TRUE, 'Processor', '["Intel Core i5","Intel Core i7","AMD Ryzen 5","AMD Ryzen 7","Apple M2"]', '10000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10200000-0000-0000-0000-000000000003', FALSE, TRUE, 'RAM Size', '["8 GB","16 GB","32 GB","64 GB"]', '10000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10200000-0000-0000-0000-000000000004', FALSE, FALSE, 'Storage Type', '["SSD","HDD","Hybrid"]', '10000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10200000-0000-0000-0000-000000000005', FALSE, FALSE, 'Screen Size', '["13 in","14 in","15.6 in","17 in"]', '10000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Canon","Nikon","Sony","Fujifilm","Panasonic"]', '10000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10300000-0000-0000-0000-000000000002', FALSE, TRUE, 'Camera Type', '["DSLR","Mirrorless","Point \u0026 Shoot","Action"]', '10000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10300000-0000-0000-0000-000000000003', FALSE, FALSE, 'Maximum Resolution', '["12 MP","16 MP","24 MP","32 MP"]', '10000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10300000-0000-0000-0000-000000000004', FALSE, FALSE, 'Optical Zoom', '["None","5x","10x","20x"]', '10000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10300000-0000-0000-0000-000000000005', FALSE, FALSE, 'Series', '["EOS","Alpha","X-T","Lumix"]', '10000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Nike","Adidas","New Balance","Puma","Under Armour"]', '20000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20100000-0000-0000-0000-000000000002', FALSE, TRUE, 'US Shoe Size', '["7","8","9","10","11","12","13"]', '20000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20100000-0000-0000-0000-000000000003', FALSE, FALSE, 'Color', '["Black","White","Red","Blue","Gray"]', '20000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20100000-0000-0000-0000-000000000004', FALSE, FALSE, 'Style', '["Low Top","Mid Top","High Top"]', '20000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20100000-0000-0000-0000-000000000005', FALSE, FALSE, 'Width', '["Standard","Wide","Extra Wide"]', '20000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["ASOS","Free People","H\u0026M","Reformation","Zara"]', '20000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20200000-0000-0000-0000-000000000002', FALSE, TRUE, 'Size Type', '["Regular","Petite","Plus","Tall"]', '20000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20200000-0000-0000-0000-000000000003', FALSE, FALSE, 'Dress Length', '["Mini","Knee Length","Midi","Maxi"]', '20000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20200000-0000-0000-0000-000000000004', FALSE, FALSE, 'Material', '["Cotton","Linen","Polyester","Silk"]', '20000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20200000-0000-0000-0000-000000000005', FALSE, FALSE, 'Pattern', '["Solid","Floral","Striped","Polka Dot","Animal Print"]', '20000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Breville","Cuisinart","Instant Pot","KitchenAid","Ninja"]', '30000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30100000-0000-0000-0000-000000000002', FALSE, TRUE, 'Appliance Type', '["Air Fryer","Blender","Coffee Maker","Mixer","Pressure Cooker"]', '30000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30100000-0000-0000-0000-000000000003', FALSE, FALSE, 'Color', '["Black","Red","Silver","Stainless Steel","White"]', '30000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30100000-0000-0000-0000-000000000004', FALSE, FALSE, 'Power Source', '["Electric","Battery","Manual"]', '30000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30100000-0000-0000-0000-000000000005', FALSE, FALSE, 'Capacity', '["2 qt","4 qt","6 qt","8 qt"]', '30000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Ashley","Crate \u0026 Barrel","IKEA","West Elm","Wayfair"]', '30000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30200000-0000-0000-0000-000000000002', FALSE, TRUE, 'Room', '["Bedroom","Dining Room","Home Office","Living Room","Patio"]', '30000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30200000-0000-0000-0000-000000000003', FALSE, FALSE, 'Material', '["Fabric","Glass","Leather","Metal","Wood"]', '30000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30200000-0000-0000-0000-000000000004', FALSE, FALSE, 'Color', '["Black","Gray","Natural","White","Walnut"]', '30000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30200000-0000-0000-0000-000000000005', FALSE, FALSE, 'Assembly Required', '["No","Yes"]', '30000000-0000-0000-0000-000000000003');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d', TIMESTAMPTZ '2025-10-12T10:30:00Z', 'seed', 'https://example.com/images/strap.jpg', FALSE, 'c1dbcf74-221e-4e10-9cd6-c4a4060b1baa', 'f6de3ce0-2d3d-4709-923d-cbb61f956947', 2, 'SKU-ACC-004', 'Camera strap pack', TIMESTAMPTZ '2025-10-12T10:30:00Z', 'seed', NULL, 45.0, 'USD', 22.5, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('6cbb0f3e-9fd9-4c83-b181-74d3432fb953', TIMESTAMPTZ '2025-10-12T10:30:00Z', 'seed', 'https://example.com/images/lens.jpg', FALSE, 'cbebba4e-72dc-4d5d-83b7-2fdd7ecb79d9', 'f6de3ce0-2d3d-4709-923d-cbb61f956947', 1, 'SKU-VCAM-001', 'Vintage camera lens', TIMESTAMPTZ '2025-10-12T10:30:00Z', 'seed', NULL, 59.99, 'USD', 59.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('964d1131-9f9c-4db8-8e6c-86fdb46f1520', TIMESTAMPTZ '2025-10-18T14:15:00Z', 'seed', 'https://example.com/images/tripod.jpg', FALSE, 'fbe6aa87-7114-4184-a4f5-89b2b36c27e3', 'c721f605-43cb-4b1b-8f0c-b1c5833420a9', 1, 'SKU-TRI-010', 'Travel tripod kit', TIMESTAMPTZ '2025-10-18T14:15:00Z', 'seed', NULL, 89.95, 'USD', 89.95, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_category_parent_id ON category (parent_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_category_condition_condition_id ON category_condition (condition_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_category_specific_category_id ON category_specific (category_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_listing_id_seller_id ON listing_id (seller_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_listing_template_name ON listing_template (name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_order_items_order_id ON order_items (order_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_order_shipping_labels_order_id ON order_shipping_labels (order_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_order_status_histories_from_status_id ON order_status_histories (from_status_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_order_status_histories_order_id ON order_status_histories (order_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_order_status_histories_to_status_id ON order_status_histories (to_status_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_order_status_transitions_from_status_id ON order_status_transitions (from_status_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_order_status_transitions_to_status_id ON order_status_transitions (to_status_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE UNIQUE INDEX ix_order_statuses_code ON order_statuses (code);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_orders_buyer_id ON orders (buyer_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE UNIQUE INDEX ix_orders_order_number ON orders (order_number);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_orders_status_id ON orders (status_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE UNIQUE INDEX ix_otp_email_code ON otp (email, code);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_refresh_token_user_id ON refresh_token (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_role_user_user_id ON role_user (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE UNIQUE INDEX ix_user_email ON "user" (email);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    CREATE INDEX ix_variation_listing_id ON variation (listing_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    PERFORM setval(
        pg_get_serial_sequence('listing_image', 'id'),
        GREATEST(
            (SELECT MAX(id) FROM listing_image) + 1,
            nextval(pg_get_serial_sequence('listing_image', 'id'))),
        false);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030032529_Add') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251030032529_Add', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030173809_changeColNameOfAllowedRoles') THEN
    ALTER TABLE order_status_transitions RENAME COLUMN "AllowedRoles" TO allowed_roles;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251030173809_changeColNameOfAllowedRoles') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251030173809_changeColNameOfAllowedRoles', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031130833_AddResearchIndexes') THEN
    ALTER INDEX ix_variation_listing_id RENAME TO idx_variation_listing_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031130833_AddResearchIndexes') THEN
    CREATE EXTENSION IF NOT EXISTS pg_trgm;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031130833_AddResearchIndexes') THEN
    CREATE INDEX idx_order_items_listing_id ON order_items (listing_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031130833_AddResearchIndexes') THEN
    CREATE INDEX idx_listing_active_owner_sort ON listing (created_by, start_date DESC, created_at DESC, id, category_id, format) WHERE status = 3;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031130833_AddResearchIndexes') THEN
    CREATE INDEX idx_listing_owner_status ON listing (created_by, status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031130833_AddResearchIndexes') THEN
    CREATE INDEX idx_listing_sku_trgm ON listing USING gin (sku gin_trgm_ops);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031130833_AddResearchIndexes') THEN
    CREATE INDEX idx_listing_title_trgm ON listing USING gin (title gin_trgm_ops);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031130833_AddResearchIndexes') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251031130833_AddResearchIndexes', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031173245_updateIdStatusHistory') THEN
    ALTER TABLE order_status_histories ALTER COLUMN id SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031173245_updateIdStatusHistory') THEN
    ALTER TABLE order_shipping_labels ALTER COLUMN order_id SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031173245_updateIdStatusHistory') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251031173245_updateIdStatusHistory', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services DROP COLUMN estimated_delivery_time;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD coverage_description character varying(150) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD created_at timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD created_by text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD delivery_window_label character varying(80) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD is_deleted boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD max_estimated_delivery_days integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD min_estimated_delivery_days integer NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD notes character varying(256) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD printer_required boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD savings_description character varying(150) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD service_code character varying(120) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD slug character varying(60) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD supports_qr_code boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD updated_at timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    ALTER TABLE shipping_services ADD updated_by text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    INSERT INTO shipping_services (id, carrier, coverage_description, created_at, created_by, delivery_window_label, is_deleted, max_estimated_delivery_days, min_estimated_delivery_days, notes, printer_required, savings_description, service_code, service_name, slug, updated_at, updated_by, base_cost_amount, base_cost_currency)
    VALUES ('5a4af094-9a6b-4d6f-9a19-9b5360f0a6ec', 'UPS', 'Up to $100.00', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 'Mar 28 - Apr 2', FALSE, 6, 3, 'Reliable ground service - Includes tracking', TRUE, 'On eBay you save 21%', 'UPS_GROUND', 'UPS Ground', 'ups-ground', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 15.62, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    INSERT INTO shipping_services (id, carrier, coverage_description, created_at, created_by, delivery_window_label, is_deleted, max_estimated_delivery_days, min_estimated_delivery_days, notes, savings_description, service_code, service_name, slug, supports_qr_code, updated_at, updated_by, base_cost_amount, base_cost_currency)
    VALUES ('6f7e3c0f-2bc6-4f1b-aa0b-4c1a9f76f950', 'USPS', 'Up to $100.00', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 'Mar 28 - Apr 1', FALSE, 5, 3, 'Max weight 70 lb - Max dimensions 130" (length + girth)', 'On eBay you save 28%', 'USPS_GROUND_ADVANTAGE', 'USPS Ground Advantage', 'usps-ground', TRUE, TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 11.45, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    INSERT INTO shipping_services (id, carrier, coverage_description, created_at, created_by, delivery_window_label, is_deleted, max_estimated_delivery_days, min_estimated_delivery_days, notes, printer_required, savings_description, service_code, service_name, slug, updated_at, updated_by, base_cost_amount, base_cost_currency)
    VALUES ('9e1f84fd-8c9c-459d-b2c5-bf6e47668f5d', 'FedEx', 'Up to $100.00', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 'Mar 29 - Apr 3', FALSE, 7, 4, '2-5 business days - Ideal for small parcels', TRUE, 'On eBay you save 18%', 'FEDEX_GROUND_ECONOMY', 'FedEx Ground Economy', 'fedex-ground', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 14.1, 'USD');
    INSERT INTO shipping_services (id, carrier, coverage_description, created_at, created_by, delivery_window_label, is_deleted, max_estimated_delivery_days, min_estimated_delivery_days, notes, printer_required, savings_description, service_code, service_name, slug, updated_at, updated_by, base_cost_amount, base_cost_currency)
    VALUES ('a1d9551e-5c5c-4ca6-9a0e-1aa855b77af7', 'USPS', 'Up to $100.00', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 'Mar 27 - 31', FALSE, 4, 2, 'Legal-size documents - Insured up to $100', TRUE, 'On eBay you save 12%', 'USPS_PRIORITY_MAIL_FLAT_RATE_LEGAL_ENVELOPE', 'USPS Priority Mail Flat Rate Legal Envelope', 'usps-priority-legal', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 9.05, 'USD');
    INSERT INTO shipping_services (id, carrier, coverage_description, created_at, created_by, delivery_window_label, is_deleted, max_estimated_delivery_days, min_estimated_delivery_days, notes, printer_required, savings_description, service_code, service_name, slug, updated_at, updated_by, base_cost_amount, base_cost_currency)
    VALUES ('c1d3c7f4-6ac1-4a7f-8a29-6dbaf9ecbb51', 'USPS', 'Up to $100.00', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 'Mar 27 - 31', FALSE, 4, 2, 'Best for documents - Includes tracking', TRUE, 'On eBay you save 13%', 'USPS_PRIORITY_MAIL_FLAT_RATE_ENVELOPE', 'USPS Priority Mail Flat Rate Envelope', 'usps-priority-flat', TIMESTAMPTZ '2025-01-01T00:00:00Z', 'seed', 8.75, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    CREATE UNIQUE INDEX ix_shipping_services_slug ON shipping_services (slug);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251031181927_addShippingServiceSeed') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251031181927_addShippingServiceSeed', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE TABLE return_policy (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_return_policy PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE TABLE shipping_policy (
        id uuid NOT NULL,
        store_id uuid NOT NULL,
        shipping_service_id uuid NOT NULL,
        name character varying(255) NOT NULL,
        cost_amount numeric(18,2) NOT NULL,
        cost_currency character varying(3) NOT NULL,
        handling_time_days integer NOT NULL,
        is_default boolean NOT NULL,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_shipping_policy PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE TABLE store (
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
        CONSTRAINT pk_store PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE TABLE store_subscription (
        id uuid NOT NULL,
        store_id uuid NOT NULL,
        subscription_type integer NOT NULL,
        monthly_fee numeric(18,2) NOT NULL,
        monthly_fee_currency character varying(3) NOT NULL,
        final_value_fee_percentage numeric(5,2) NOT NULL,
        listing_limit integer NOT NULL,
        start_date timestamp with time zone NOT NULL,
        end_date timestamp with time zone,
        status integer NOT NULL,
        CONSTRAINT pk_store_subscription PRIMARY KEY (id),
        CONSTRAINT fk_store_subscription_store_store_id FOREIGN KEY (store_id) REFERENCES store (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE UNIQUE INDEX idx_return_policy_store_id ON return_policy (store_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE INDEX idx_shipping_policy_store_id ON shipping_policy (store_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE UNIQUE INDEX idx_store_slug ON store (slug);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE INDEX idx_store_user_id ON store (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE INDEX idx_store_subscription_store_id ON store_subscription (store_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    CREATE INDEX idx_store_subscription_store_status ON store_subscription (store_id, status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101073256_AddStoreManagement') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251101073256_AddStoreManagement', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101075315_UpdateShippingPolicyStructure') THEN
    ALTER TABLE shipping_policy DROP COLUMN name;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101075315_UpdateShippingPolicyStructure') THEN
    ALTER TABLE shipping_policy DROP COLUMN shipping_service_id;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101075315_UpdateShippingPolicyStructure') THEN
    ALTER TABLE shipping_policy ADD carrier character varying(100) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101075315_UpdateShippingPolicyStructure') THEN
    ALTER TABLE shipping_policy ADD service_name character varying(100) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101075315_UpdateShippingPolicyStructure') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251101075315_UpdateShippingPolicyStructure', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    DELETE FROM order_status_transitions
    WHERE id = '1b9c7e7f-9d15-41b0-9417-51c5723a7792';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    DELETE FROM order_status_transitions
    WHERE id = '70e8c0f9-6fa8-4ff6-bb3f-5b53b22e2afd';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    DELETE FROM order_statuses
    WHERE id = '949ce7f8-6d6b-4d65-9032-b9f51c4508eb';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    ALTER TABLE order_shipping_labels ADD is_voided boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    ALTER TABLE order_shipping_labels ADD void_reason character varying(250);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    ALTER TABLE order_shipping_labels ADD voided_at timestamp with time zone;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    CREATE TABLE order_cancellation_requests (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_order_cancellation_requests PRIMARY KEY (id),
        CONSTRAINT fk_order_cancellation_requests_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    CREATE TABLE order_return_requests (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_order_return_requests PRIMARY KEY (id),
        CONSTRAINT fk_order_return_requests_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    CREATE INDEX idx_cancellation_requests_order ON order_cancellation_requests (order_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    CREATE INDEX idx_cancellation_requests_status ON order_cancellation_requests (status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    CREATE INDEX idx_return_requests_order ON order_return_requests (order_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    CREATE INDEX idx_return_requests_status ON order_return_requests (status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101173452_addCancellationsAndReturnRequest') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251101173452_addCancellationsAndReturnRequest', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101175728_addCancellationsAndReturnRequest3') THEN
    DELETE FROM order_cancellation_requests
    WHERE id = '0d5a8f1d-3c95-4dba-9d59-5d0f41ec134a';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101175728_addCancellationsAndReturnRequest3') THEN
    DELETE FROM order_cancellation_requests
    WHERE id = 'da7790f6-4c70-40ec-9fb4-1f1a3d41d3a9';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101175728_addCancellationsAndReturnRequest3') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251101175728_addCancellationsAndReturnRequest3', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    CREATE TABLE report_downloads (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_report_downloads PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    CREATE TABLE report_schedules (
        id uuid NOT NULL,
        user_id uuid NOT NULL,
        source character varying(64) NOT NULL,
        type character varying(128) NOT NULL,
        frequency character varying(32) NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        last_run_at_utc timestamp with time zone,
        next_run_at_utc timestamp with time zone,
        end_date_utc timestamp with time zone,
        is_active boolean NOT NULL DEFAULT TRUE,
        delivery_email character varying(256),
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_report_schedules PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Televisions, speakers, and streaming devices.', FALSE, 'TV, Video & Home Audio', '10000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Home and handheld gaming systems.', FALSE, 'Video Game Consoles', '10000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Smartwatches, fitness trackers, and smart eyewear.', FALSE, 'Wearable Technology', '10000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000008', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Connected home devices and automation hubs.', FALSE, 'Smart Home', '10000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('10000000-0000-0000-0000-000000000009', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Navigation, dash cams, and in-car entertainment.', FALSE, 'Vehicle Electronics & GPS', '10000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('20000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Casual, business, and formal apparel for men.', FALSE, 'Men''s Clothing', '20000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('20000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Designer totes, crossbody bags, and backpacks.', FALSE, 'Women''s Handbags & Bags', '20000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('20000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Heels, flats, and casual footwear.', FALSE, 'Women''s Shoes', '20000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('20000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Timepieces ranging from vintage to luxury.', FALSE, 'Watches', '20000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('20000000-0000-0000-0000-000000000008', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Rings, necklaces, and bracelets crafted in precious metals.', FALSE, 'Fine Jewelry', '20000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('30000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Interior accents, wall art, and lighting.', FALSE, 'Home Décor', '30000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('30000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Power tools and shop essentials.', FALSE, 'Tools & Workshop Equipment', '30000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('30000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Outdoor décor, landscaping, and patio gear.', FALSE, 'Yard, Garden & Outdoor Living', '30000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('30000000-0000-0000-0000-000000000007', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Building supplies, fixtures, and hardware.', FALSE, 'Home Improvement', '30000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('50000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Complete automotive marketplace for vehicles and parts.', FALSE, 'eBay Motors', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('60000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Treasures from pop culture, history, and fine art.', FALSE, 'Collectibles & Art', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('70000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Playsets, model kits, and collector favorites.', FALSE, 'Toys & Hobbies', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('80000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Gear for every sport, indoors and out.', FALSE, 'Sporting Goods', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('90000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Wellness essentials and personal care favorites.', FALSE, 'Health & Beauty', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('a0000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Equipment, supplies, and services for every trade.', FALSE, 'Business & Industrial', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('b0000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Instruments, pro audio, and stage equipment.', FALSE, 'Musical Instruments & Gear', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('c0000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Care essentials for pets large and small.', FALSE, 'Pet Supplies', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('d0000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Nursery gear, travel systems, and feeding must-haves.', FALSE, 'Baby Essentials', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('e0000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'DIY staples spanning every creative discipline.', FALSE, 'Crafts', NULL, NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('50000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'OEM and aftermarket components for every ride.', FALSE, 'Car Parts & Accessories', '50000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('50000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Upgrades and replacement parts for bikes.', FALSE, 'Motorcycle Parts', '50000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('50000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Garage lifts, diagnostics, and specialty tools.', FALSE, 'Automotive Tools & Supplies', '50000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('50000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Rims, tire sets, TPMS sensors, and more.', FALSE, 'Wheels, Tires & Parts', '50000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('60000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'TCG singles, sealed product, and memorabilia.', FALSE, 'Collectible Card Games', '60000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('60000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Golden Age through modern runs and collectibles.', FALSE, 'Comics & Graphic Novels', '60000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('60000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Limited editions, lithographs, and posters.', FALSE, 'Art Prints', '60000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('60000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Graded coins, bullion, and currency.', FALSE, 'Coins & Paper Money', '60000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('70000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Superheroes, anime, and pop-culture icons.', FALSE, 'Action Figures', '70000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('70000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Locomotives, rolling stock, and scenery kits.', FALSE, 'Model Railroads & Trains', '70000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('70000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Radio-controlled cars, drones, and planes.', FALSE, 'RC Model Vehicles & Kits', '70000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('70000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Barbie, Blythe, Build-A-Bear, and more.', FALSE, 'Dolls & Bears', '70000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('70000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Modular builds and sealed collectible sets.', FALSE, 'LEGO & Building Toys', '70000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('80000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Camping, hiking, hunting, and fishing gear.', FALSE, 'Outdoor Sports', '80000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('80000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Exercise machines, apparel, and accessories.', FALSE, 'Fitness, Running & Yoga', '80000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('80000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Bikes, parts, helmets, and apparel.', FALSE, 'Cycling', '80000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('80000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Clubs, balls, carts, and training aids.', FALSE, 'Golf', '80000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('90000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Cosmetics, palettes, and tools.', FALSE, 'Makeup', '90000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('90000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Serums, moisturizers, and devices.', FALSE, 'Skin Care', '90000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('90000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Wellness, immunity, and performance blends.', FALSE, 'Vitamins & Dietary Supplements', '90000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('90000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Styling tools, treatments, and color.', FALSE, 'Hair Care', '90000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('90000000-0000-0000-0000-000000000006', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Perfumes, colognes, and body mists.', FALSE, 'Fragrances', '90000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('a0000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Excavators, loaders, and industrial vehicles.', FALSE, 'Heavy Equipment', 'a0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('a0000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Maintenance, repair, and operations essentials.', FALSE, 'MRO & Industrial Supplies', 'a0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('a0000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Point-of-sale, signage, and consulting packages.', FALSE, 'Retail & Services', 'a0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('a0000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Printers, copiers, and office machines.', FALSE, 'Office Equipment', 'a0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('b0000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Electric, acoustic, and bass guitars.', FALSE, 'Guitars & Basses', 'b0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('b0000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Mixers, microphones, and studio gear.', FALSE, 'Pro Audio Equipment', 'b0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('b0000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Controllers, turntables, and lighting.', FALSE, 'DJ Equipment', 'b0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('b0000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Saxes, trumpets, clarinets, and accessories.', FALSE, 'Brass & Woodwind', 'b0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('c0000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Beds, crates, and training essentials.', FALSE, 'Dog Supplies', 'c0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('c0000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Litter, scratchers, and cat furniture.', FALSE, 'Cat Supplies', 'c0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('c0000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Aquariums, filtration, and décor.', FALSE, 'Fish & Aquarium', 'c0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('c0000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Habitat accessories for hamsters, rabbits, and more.', FALSE, 'Small Animal Supplies', 'c0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('d0000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Lightweight, jogging, and convertible options.', FALSE, 'Strollers & Travel Systems', 'd0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('d0000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Cribs, dressers, and gliders.', FALSE, 'Nursery Furniture', 'd0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('d0000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Monitors, gates, and proofing essentials.', FALSE, 'Baby Safety', 'd0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('d0000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Bottles, warmers, and nursing support.', FALSE, 'Baby Feeding', 'd0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('e0000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Stamps, dies, and embellishments.', FALSE, 'Scrapbooking & Paper Crafting', 'e0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('e0000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Paints, canvases, and studio tools.', FALSE, 'Art Supplies', 'e0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('e0000000-0000-0000-0000-000000000004', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Yardage, quilting, and upholstery textiles.', FALSE, 'Fabric', 'e0000000-0000-0000-0000-000000000001', NULL, NULL);
    INSERT INTO category (id, created_at, created_by, description, is_deleted, name, parent_id, updated_at, updated_by)
    VALUES ('e0000000-0000-0000-0000-000000000005', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Findings, gemstones, and tools.', FALSE, 'Beads & Jewelry Making', 'e0000000-0000-0000-0000-000000000001', NULL, NULL);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000005');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000009', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000009', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('10000000-0000-0000-0000-000000000009', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000007');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000008');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000007');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000008');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000007');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000008');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000007');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000008');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('20000000-0000-0000-0000-000000000008', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('30000000-0000-0000-0000-000000000007', '40000000-0000-0000-0000-000000000006');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["LG","Samsung","Sony","TCL","Vizio"]', '10000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10400000-0000-0000-0000-000000000002', FALSE, TRUE, 'Type', '["4K UHD TV","Soundbar","AV Receiver","Streaming Device"]', '10000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10400000-0000-0000-0000-000000000003', FALSE, FALSE, 'Smart Platform', '["Google TV","Roku","Fire TV","webOS","Tizen"]', '10000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10500000-0000-0000-0000-000000000001', FALSE, TRUE, 'Platform', '["Nintendo","PlayStation","Xbox","Steam Deck"]', '10000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10500000-0000-0000-0000-000000000002', FALSE, TRUE, 'Model', '["PlayStation 5","Xbox Series X","Nintendo Switch OLED","Steam Deck OLED"]', '10000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10500000-0000-0000-0000-000000000003', FALSE, FALSE, 'Storage Capacity', '["64 GB","512 GB","1 TB","2 TB"]', '10000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10600000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Apple","Fitbit","Garmin","Samsung","Withings"]', '10000000-0000-0000-0000-000000000007');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10600000-0000-0000-0000-000000000002', TRUE, FALSE, 'Features', '["GPS","Heart Rate Monitor","NFC","SpO2","Sleep Tracking"]', '10000000-0000-0000-0000-000000000007');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10700000-0000-0000-0000-000000000001', FALSE, TRUE, 'Device Type', '["Smart Speaker","Smart Display","Smart Lighting","Smart Thermostat","Security Camera"]', '10000000-0000-0000-0000-000000000008');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10700000-0000-0000-0000-000000000002', FALSE, FALSE, 'Ecosystem', '["Alexa","Apple Home","Google Home","Matter","SmartThings"]', '10000000-0000-0000-0000-000000000008');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10800000-0000-0000-0000-000000000001', FALSE, TRUE, 'Product Type', '["Dash Cam","GPS","Car Stereo","Backup Camera","Radar Detector"]', '10000000-0000-0000-0000-000000000009');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('10800000-0000-0000-0000-000000000002', TRUE, FALSE, 'Compatible Vehicle', '["Universal","Ford","GM","Toyota","Volkswagen"]', '10000000-0000-0000-0000-000000000009');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Banana Republic","Hugo Boss","Levi\u0027s","Nike","Ralph Lauren"]', '20000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20300000-0000-0000-0000-000000000002', FALSE, TRUE, 'Size', '["S","M","L","XL","XXL"]', '20000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Coach","Gucci","Kate Spade","Louis Vuitton","Tory Burch"]', '20000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20400000-0000-0000-0000-000000000002', TRUE, FALSE, 'Materials', '["Canvas","Leather","Nylon","Patent Leather","Vegan Leather"]', '20000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20400000-0000-0000-0000-000000000003', FALSE, FALSE, 'Style', '["Backpack","Crossbody","Satchel","Shoulder Bag","Tote"]', '20000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20500000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Birkenstock","Clarks","Dr. Martens","Sam Edelman","Steve Madden"]', '20000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20500000-0000-0000-0000-000000000002', FALSE, TRUE, 'US Shoe Size', '["5","6","7","8","9","10"]', '20000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20500000-0000-0000-0000-000000000003', FALSE, FALSE, 'Style', '["Boots","Flats","Heels","Sandals","Sneakers"]', '20000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20600000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Casio","Citizen","Omega","Rolex","Seiko"]', '20000000-0000-0000-0000-000000000007');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20600000-0000-0000-0000-000000000002', FALSE, FALSE, 'Movement', '["Automatic","Quartz","Mechanical","Solar"]', '20000000-0000-0000-0000-000000000007');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20700000-0000-0000-0000-000000000001', FALSE, TRUE, 'Metal', '["Gold","Platinum","Rose Gold","Sterling Silver","White Gold"]', '20000000-0000-0000-0000-000000000008');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('20700000-0000-0000-0000-000000000002', FALSE, TRUE, 'Jewelry Type', '["Bracelet","Earrings","Necklace","Ring"]', '20000000-0000-0000-0000-000000000008');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30100000-0000-0000-0000-000000000006', FALSE, FALSE, 'Style', '["Bohemian","Farmhouse","Mid-Century","Modern","Traditional"]', '30000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30100000-0000-0000-0000-000000000007', FALSE, TRUE, 'Room', '["Bedroom","Dining Room","Kitchen","Living Room","Office"]', '30000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30300000-0000-0000-0000-000000000001', FALSE, FALSE, 'Brand', '["Bosch","DeWalt","Hilti","Makita","Milwaukee"]', '30000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30300000-0000-0000-0000-000000000002', FALSE, TRUE, 'Power Source', '["Battery","Corded Electric","Compressed Air","Manual"]', '30000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Category', '["Flooring","Hardware","Lighting","Plumbing","Storage"]', '30000000-0000-0000-0000-000000000007');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('30400000-0000-0000-0000-000000000002', FALSE, FALSE, 'Finish', '["Brushed Nickel","Chrome","Matte Black","Oil-Rubbed Bronze"]', '30000000-0000-0000-0000-000000000007');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000002');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000003');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000005');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000005');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000005');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('50000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('60000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000005');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000009');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('70000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('80000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('90000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('90000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('90000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('90000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('90000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('90000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('90000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('90000000-0000-0000-0000-000000000006', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('a0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('b0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('c0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('d0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('e0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('e0000000-0000-0000-0000-000000000002', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('e0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('e0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000004');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('e0000000-0000-0000-0000-000000000003', '40000000-0000-0000-0000-000000000006');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('e0000000-0000-0000-0000-000000000004', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('e0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000001');
    INSERT INTO category_condition (category_id, condition_id)
    VALUES ('e0000000-0000-0000-0000-000000000005', '40000000-0000-0000-0000-000000000006');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('40100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Part Type', '["Brakes","Engine","Exterior","Interior","Suspension"]', '50000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('40100000-0000-0000-0000-000000000002', FALSE, FALSE, 'Brand', '["ACDelco","Bosch","Denso","Mopar","Motorcraft"]', '50000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('40100000-0000-0000-0000-000000000003', TRUE, FALSE, 'Compatible Make', '["Chevrolet","Ford","Honda","Toyota","Universal"]', '50000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('40200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Part Type', '["Body \u0026 Frame","Drivetrain","Electrical","Engine","Suspension"]', '50000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('40300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Tool Type', '["Diagnostic","Hand Tool","Lifts \u0026 Jacks","Power Tool","Specialty"]', '50000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('40400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Tire Type', '["All-Season","Performance","Snow/Winter","Off-Road"]', '50000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('40400000-0000-0000-0000-000000000002', FALSE, FALSE, 'Rim Diameter', '["16 in","17 in","18 in","19 in","20 in"]', '50000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Franchise', '["Magic: The Gathering","Pok\u00E9mon","Yu-Gi-Oh!","Marvel Snap","Disney Lorcana"]', '60000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60100000-0000-0000-0000-000000000002', FALSE, TRUE, 'Card Condition', '["Gem Mint","Near Mint","Lightly Played","Moderately Played","Heavily Played"]', '60000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60100000-0000-0000-0000-000000000003', FALSE, FALSE, 'Graded', '["BGS","CGC","PSA","SGC","Ungraded"]', '60000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Publisher', '["DC","Dark Horse","IDW","Image","Marvel"]', '60000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60200000-0000-0000-0000-000000000002', FALSE, FALSE, 'Era', '["Golden Age","Silver Age","Bronze Age","Modern"]', '60000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Artist', '["Andy Warhol","Banksy","Jean-Michel Basquiat","Salvador Dal\u00ED","Yoshitomo Nara"]', '60000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60300000-0000-0000-0000-000000000002', FALSE, FALSE, 'Medium', '["Gicl\u00E9e","Lithograph","Screenprint","Serigraph"]', '60000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60400000-0000-0000-0000-000000000001', FALSE, FALSE, 'Certification', '["ANACS","NGC","PCGS","PMG","Uncertified"]', '60000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('60400000-0000-0000-0000-000000000002', FALSE, FALSE, 'Grade', '["MS 70","MS 69","MS 65","AU 55","XF 45"]', '60000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('70100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Franchise', '["DC","Dragon Ball","Marvel","Star Wars","Transformers"]', '70000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('70100000-0000-0000-0000-000000000002', FALSE, FALSE, 'Scale', '["1:6","1:12","1:18","6 in","12 in"]', '70000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('70200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Scale', '["HO","N","O","G","Z"]', '70000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('70200000-0000-0000-0000-000000000002', FALSE, FALSE, 'Power Type', '["DC","DCC","Battery"]', '70000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('70300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Vehicle Type', '["Car","Truck","Boat","Plane","Drone"]', '70000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('70300000-0000-0000-0000-000000000002', FALSE, FALSE, 'Power', '["Electric","Gas","Nitro"]', '70000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('70400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Type', '["Barbie","Fashion Doll","Teddy Bear","Vintage Doll"]', '70000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('70500000-0000-0000-0000-000000000001', FALSE, FALSE, 'Theme', '["Architecture","City","Ideas","Star Wars","Technic"]', '70000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('80100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Sport', '["Camping","Climbing","Fishing","Hunting","Water Sports"]', '80000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('80200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Equipment Type', '["Cardio Machine","Free Weights","Resistance Bands","Yoga Mat"]', '80000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('80300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Bicycle Type', '["Mountain","Road","Hybrid","Gravel","Electric"]', '80000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('80300000-0000-0000-0000-000000000002', FALSE, FALSE, 'Frame Size', '["Small","Medium","Large","X-Large"]', '80000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('80400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Club Type', '["Driver","Fairway Wood","Hybrid","Iron Set","Putter"]', '80000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('80400000-0000-0000-0000-000000000002', FALSE, FALSE, 'Flex', '["Ladies","Regular","Stiff","Extra Stiff"]', '80000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Charlotte Tilbury","Dior","Fenty Beauty","MAC","Rare Beauty"]', '90000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90100000-0000-0000-0000-000000000002', FALSE, TRUE, 'Product Type', '["Foundation","Eyeshadow","Lipstick","Mascara","Primer"]', '90000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90100000-0000-0000-0000-000000000003', FALSE, FALSE, 'Shade', '["Fair","Light","Medium","Tan","Deep"]', '90000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["CeraVe","Dermalogica","Drunk Elephant","La Roche-Posay","Tatcha"]', '90000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90200000-0000-0000-0000-000000000002', FALSE, TRUE, 'Skin Type', '["Dry","Normal","Oily","Combination","Sensitive"]', '90000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90200000-0000-0000-0000-000000000003', TRUE, FALSE, 'Concern', '["Acne","Anti-Aging","Brightening","Hydration","Sun Protection"]', '90000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Formulation', '["Capsule","Gummy","Powder","Softgel","Tablet"]', '90000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90300000-0000-0000-0000-000000000002', TRUE, FALSE, 'Main Purpose', '["Energy","General Wellness","Immune Support","Joint Health","Sleep"]', '90000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Amika","Dyson","GHD","Olaplex","Redken"]', '90000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90400000-0000-0000-0000-000000000002', TRUE, FALSE, 'Hair Type', '["Coily","Curly","Straight","Wavy","Fine"]', '90000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90500000-0000-0000-0000-000000000001', FALSE, TRUE, 'Fragrance Type', '["Eau de Parfum","Eau de Toilette","Eau de Cologne","Perfume Oil"]', '90000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('90500000-0000-0000-0000-000000000002', FALSE, FALSE, 'Volume', '["30 ml","50 ml","75 ml","100 ml"]', '90000000-0000-0000-0000-000000000006');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('a0100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Equipment Type', '["Backhoe","Bulldozer","Excavator","Forklift","Skid Steer"]', 'a0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('a0100000-0000-0000-0000-000000000002', FALSE, FALSE, 'Hours', '["0-500","501-1500","1501-3000","3001\u002B"]', 'a0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('a0200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Supply Type', '["Fasteners","Hydraulics","HVAC","Pneumatics","Safety"]', 'a0000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('a0300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Service Type', '["Consulting","Installation","Maintenance","Marketing","Training"]', 'a0000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('a0400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Equipment Type', '["3D Printer","Laser Printer","Multifunction","Scanner","Shredder"]', 'a0000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('b0100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Fender","Gibson","Ibanez","PRS","Taylor"]', 'b0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('b0100000-0000-0000-0000-000000000002', FALSE, FALSE, 'Body Type', '["Solid","Semi-Hollow","Hollow"]', 'b0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('b0200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Type', '["Mixer","Microphone","Monitor","Interface","Outboard Gear"]', 'b0000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('b0300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Type', '["Controller","Mixer","Turntable","Lighting"]', 'b0000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('b0400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Instrument', '["Clarinet","Flute","Saxophone","Trombone","Trumpet"]', 'b0000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('c0100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Product Type', '["Apparel","Crates","Food","Grooming","Toys"]', 'c0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('c0100000-0000-0000-0000-000000000002', FALSE, FALSE, 'Size', '["XS","S","M","L","XL"]', 'c0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('c0200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Product Type', '["Food","Litter","Scratchers","Toys"]', 'c0000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('c0300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Aquarium Type', '["Freshwater","Marine","Reef","Brackish"]', 'c0000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('c0300000-0000-0000-0000-000000000002', FALSE, FALSE, 'Tank Capacity', '["1-10 gal","11-30 gal","31-55 gal","56\u002B gal"]', 'c0000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('c0400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Product Type', '["Bedding","Cages","Food","Toys"]', 'c0000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('d0100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Brand', '["Baby Jogger","Bugaboo","Chicco","Graco","UPPAbaby"]', 'd0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('d0100000-0000-0000-0000-000000000002', FALSE, TRUE, 'Stroller Type', '["Full-Size","Jogging","Travel System","Umbrella"]', 'd0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('d0200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Furniture Piece', '["Crib","Changing Table","Glider","Dresser"]', 'd0000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('d0300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Product Type', '["Baby Monitor","Safety Gate","Outlet Cover","Cabinet Lock"]', 'd0000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('d0400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Feeding Type', '["Bottle","Breastfeeding","Solid Food","Toddler"]', 'd0000000-0000-0000-0000-000000000005');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('e0100000-0000-0000-0000-000000000001', FALSE, TRUE, 'Product Type', '["Adhesives","Dies","Paper","Stamps"]', 'e0000000-0000-0000-0000-000000000002');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('e0200000-0000-0000-0000-000000000001', FALSE, TRUE, 'Medium', '["Acrylic","Oil","Watercolor","Pastel"]', 'e0000000-0000-0000-0000-000000000003');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('e0300000-0000-0000-0000-000000000001', FALSE, TRUE, 'Fabric Type', '["Cotton","Denim","Fleece","Linen","Silk"]', 'e0000000-0000-0000-0000-000000000004');
    INSERT INTO category_specific (id, allow_multiple, is_required, name, values, category_id)
    VALUES ('e0400000-0000-0000-0000-000000000001', FALSE, TRUE, 'Material', '["Glass","Gemstone","Metal","Resin","Wood"]', 'e0000000-0000-0000-0000-000000000005');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    CREATE UNIQUE INDEX ix_report_downloads_user_id_reference_code ON report_downloads (user_id, reference_code);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    CREATE INDEX ix_report_schedules_user_id_source_type_is_active ON report_schedules (user_id, source, type, is_active);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251101192218_AddReportsModule') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251101192218_AddReportsModule', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    DELETE FROM order_items
    WHERE id = '964d1131-9f9c-4db8-8e6c-86fdb46f1520';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    DELETE FROM order_return_requests
    WHERE id = '1c41a5ab-d8cb-4525-b2cf-4188c49dd9b2';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    DELETE FROM order_return_requests
    WHERE id = '8fd217f6-9356-4d3e-bf68-5cb15f2a1d86';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    DELETE FROM orders
    WHERE id = 'c721f605-43cb-4b1b-8f0c-b1c5833420a9';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    UPDATE order_items SET image_url = 'https://picsum.photos/seed/1-3/640/640', listing_id = '71000000-0000-0000-0000-000000000003', sku = 'DEMO-1-0003', title = 'Alice''s Item #3', total_price_amount = 63.98, unit_price_amount = 31.99
    WHERE id = '1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    UPDATE order_items SET image_url = 'https://picsum.photos/seed/1-1/640/640', listing_id = '71000000-0000-0000-0000-000000000001', sku = 'DEMO-1-0001', title = 'Alice''s Item #1', total_price_amount = 29.99, unit_price_amount = 29.99
    WHERE id = '6cbb0f3e-9fd9-4c83-b181-74d3432fb953';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('c721f605-43cb-4b1b-8f0c-b1c5833420a9', NULL, '70000000-0000-0000-0000-000000000003', NULL, 'OCTDEAL', TIMESTAMPTZ '2025-10-15T14:15:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1002', TIMESTAMPTZ '2025-10-15T14:15:00Z', TIMESTAMPTZ '2025-10-15T15:15:00Z', NULL, '70000000-0000-0000-0000-000000000001', TIMESTAMPTZ '2025-10-16T00:15:00Z', 2, '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', TIMESTAMPTZ '2025-10-16T00:15:00Z', 'seed', 5.0, 'USD', 3.35, 'USD', 12.0, 'USD', 66.98, 'USD', 5.36, 'USD', 82.69, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('3e54a8a8-3b35-4bdf-9d09-75042c7f7d4f', TIMESTAMPTZ '2025-10-15T14:15:00Z', 'seed', 'https://picsum.photos/seed/1-4/640/640', FALSE, '71000000-0000-0000-0000-000000000004', 'c721f605-43cb-4b1b-8f0c-b1c5833420a9', 1, 'DEMO-1-0004', 'Alice''s Item #4', TIMESTAMPTZ '2025-10-16T00:15:00Z', 'seed', NULL, 32.99, 'USD', 32.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('a9d23977-7d99-4d44-bb79-4cff5ec2f56f', TIMESTAMPTZ '2025-10-15T14:15:00Z', 'seed', 'https://picsum.photos/seed/1-5/640/640', FALSE, '71000000-0000-0000-0000-000000000005', 'c721f605-43cb-4b1b-8f0c-b1c5833420a9', 1, 'DEMO-1-0005', 'Alice''s Item #5', TIMESTAMPTZ '2025-10-16T00:15:00Z', 'seed', NULL, 33.99, 'USD', 33.99, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    UPDATE orders SET buyer_id = '70000000-0000-0000-0000-000000000002', platform_fee_amount = 4.7, sub_total_amount = 93.97, tax_amount = 7.52, total_amount = 114.69
    WHERE id = 'f6de3ce0-2d3d-4709-923d-cbb61f956947';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('1e86f219-1dd0-4cac-a545-cb98e65ce429', NULL, '70000000-0000-0000-0000-000000000001', NULL, 'HOLIDAY10', TIMESTAMPTZ '2025-10-28T12:10:00Z', 'seed', TIMESTAMPTZ '2025-10-31T12:10:00Z', 0, FALSE, 'ORD-SEED-1009', TIMESTAMPTZ '2025-10-28T12:10:00Z', TIMESTAMPTZ '2025-10-28T13:10:00Z', NULL, '70000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2025-10-28T22:10:00Z', 2, 'c21a6b64-f0e9-4947-8b1b-38ef45aa4930', TIMESTAMPTZ '2025-10-31T12:10:00Z', 'seed', 7.5, 'USD', 6.2, 'USD', 14.0, 'USD', 145.97, 'USD', 10.6, 'USD', 169.27, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4', NULL, '70000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2025-10-21T16:00:00Z', NULL, TIMESTAMPTZ '2025-10-20T16:00:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1004', TIMESTAMPTZ '2025-10-20T16:00:00Z', NULL, NULL, '70000000-0000-0000-0000-000000000001', NULL, 6, 'ab0ecf06-0e67-4a5d-9820-3a276f59a4fd', TIMESTAMPTZ '2025-10-21T16:00:00Z', 'seed', 0.0, 'USD', 3.25, 'USD', 0.0, 'USD', 74.98, 'USD', 5.5, 'USD', 83.73, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-10-18T09:45:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1003', TIMESTAMPTZ '2025-10-18T09:45:00Z', TIMESTAMPTZ '2025-10-18T10:45:00Z', NULL, '70000000-0000-0000-0000-000000000001', NULL, 0, '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', TIMESTAMPTZ '2025-10-18T10:45:00Z', 'seed', 2.0, 'USD', 3.4, 'USD', 9.25, 'USD', 70.98, 'USD', 6.2, 'USD', 87.83, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('973cac8a-9be0-44a0-90b7-fd8263f8e78a', NULL, '70000000-0000-0000-0000-000000000003', NULL, NULL, TIMESTAMPTZ '2025-10-24T11:05:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1006', TIMESTAMPTZ '2025-10-24T11:05:00Z', TIMESTAMPTZ '2025-10-24T11:50:00Z', NULL, '70000000-0000-0000-0000-000000000002', NULL, 0, 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', TIMESTAMPTZ '2025-10-24T11:50:00Z', 'seed', 4.0, 'USD', 3.99, 'USD', 11.0, 'USD', 79.98, 'USD', 6.4, 'USD', 97.37, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('a4206ad5-6a35-43bb-8a8c-8c7b244594ac', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-10-26T18:40:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1008', TIMESTAMPTZ '2025-10-26T18:40:00Z', TIMESTAMPTZ '2025-10-26T19:40:00Z', NULL, '70000000-0000-0000-0000-000000000003', NULL, 0, '859b47f4-0d05-4f43-8ff5-57acb8d5da1d', TIMESTAMPTZ '2025-10-26T19:40:00Z', 'seed', 0.0, 'USD', 4.6, 'USD', 13.0, 'USD', 92.98, 'USD', 8.6, 'USD', 119.18, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('bd34cf77-4551-4194-ad16-d20c94b58289', NULL, '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-10-25T15:30:00Z', 'seed', TIMESTAMPTZ '2025-10-28T15:30:00Z', 0, FALSE, 'ORD-SEED-1007', TIMESTAMPTZ '2025-10-25T15:30:00Z', TIMESTAMPTZ '2025-10-25T17:30:00Z', NULL, '70000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2025-10-26T15:30:00Z', 5, '5f5d9f3a-35fd-4f66-a25d-10a5f64f86f9', TIMESTAMPTZ '2025-10-28T15:30:00Z', 'seed', 0.0, 'USD', 4.2, 'USD', 12.5, 'USD', 84.98, 'USD', 7.2, 'USD', 108.88, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('d2ee4d4a-5be0-4d76-bce6-0b8578c87407', NULL, '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-10-22T08:20:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1005', TIMESTAMPTZ '2025-10-22T08:20:00Z', TIMESTAMPTZ '2025-10-22T09:20:00Z', NULL, '70000000-0000-0000-0000-000000000002', TIMESTAMPTZ '2025-10-22T17:20:00Z', 2, '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', TIMESTAMPTZ '2025-10-22T17:20:00Z', 'seed', 0.0, 'USD', 4.1, 'USD', 10.0, 'USD', 76.98, 'USD', 6.16, 'USD', 97.24, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('fa236302-3864-4e54-9e40-3ebdb4749734', NULL, '70000000-0000-0000-0000-000000000002', NULL, 'BULKBUY', TIMESTAMPTZ '2025-10-30T09:05:00Z', 'seed', TIMESTAMPTZ '2025-11-03T09:05:00Z', 0, FALSE, 'ORD-SEED-1010', TIMESTAMPTZ '2025-10-30T09:05:00Z', TIMESTAMPTZ '2025-10-30T10:05:00Z', NULL, '70000000-0000-0000-0000-000000000003', TIMESTAMPTZ '2025-10-30T22:05:00Z', 2, '5d29d462-fd9d-4a91-9dd8-92b93cbd4cc8', TIMESTAMPTZ '2025-11-03T09:05:00Z', 'seed', 10.0, 'USD', 7.2, 'USD', 15.0, 'USD', 152.97, 'USD', 12.2, 'USD', 177.37, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('0a3e9070-0a5e-4114-8634-8e9353a5369e', TIMESTAMPTZ '2025-10-20T16:00:00Z', 'seed', 'https://picsum.photos/seed/1-9/640/640', FALSE, '71000000-0000-0000-0000-000000000009', '1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4', 1, 'DEMO-1-0009', 'Alice''s Item #9', TIMESTAMPTZ '2025-10-21T16:00:00Z', 'seed', NULL, 37.99, 'USD', 37.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('30f2c0f3-09bb-4f52-93a9-6e98b0171c3f', TIMESTAMPTZ '2025-10-28T12:10:00Z', 'seed', 'https://picsum.photos/seed/3-3/640/640', FALSE, '73000000-0000-0000-0000-000000000003', '1e86f219-1dd0-4cac-a545-cb98e65ce429', 1, 'DEMO-3-0003', 'Cecilia''s Item #3', TIMESTAMPTZ '2025-10-31T12:10:00Z', 'seed', NULL, 47.99, 'USD', 47.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('4a1ab1de-4a10-4326-a0be-5d3ab27c9df7', TIMESTAMPTZ '2025-10-22T08:20:00Z', 'seed', 'https://picsum.photos/seed/2-2/640/640', FALSE, '72000000-0000-0000-0000-000000000002', 'd2ee4d4a-5be0-4d76-bce6-0b8578c87407', 1, 'DEMO-2-0002', 'Brian''s Item #2', TIMESTAMPTZ '2025-10-22T17:20:00Z', 'seed', NULL, 38.99, 'USD', 38.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('55c9f2a2-dba1-4c66-9b83-a8b4c9e7a0d4', TIMESTAMPTZ '2025-10-30T09:05:00Z', 'seed', 'https://picsum.photos/seed/3-7/640/640', FALSE, '73000000-0000-0000-0000-000000000007', 'fa236302-3864-4e54-9e40-3ebdb4749734', 1, 'DEMO-3-0007', 'Cecilia''s Item #7', TIMESTAMPTZ '2025-11-03T09:05:00Z', 'seed', NULL, 51.99, 'USD', 51.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('5f2f8987-3b95-4b9f-8cc0-0f7c4b8d3b92', TIMESTAMPTZ '2025-10-24T11:05:00Z', 'seed', 'https://picsum.photos/seed/2-3/640/640', FALSE, '72000000-0000-0000-0000-000000000003', '973cac8a-9be0-44a0-90b7-fd8263f8e78a', 2, 'DEMO-2-0003', 'Brian''s Item #3', TIMESTAMPTZ '2025-10-24T11:05:00Z', 'seed', NULL, 79.98, 'USD', 39.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('6bd3f47d-4f1e-467f-8797-3b2a151dd09f', TIMESTAMPTZ '2025-10-28T12:10:00Z', 'seed', 'https://picsum.photos/seed/3-4/640/640', FALSE, '73000000-0000-0000-0000-000000000004', '1e86f219-1dd0-4cac-a545-cb98e65ce429', 2, 'DEMO-3-0004', 'Cecilia''s Item #4', TIMESTAMPTZ '2025-10-31T12:10:00Z', 'seed', NULL, 97.98, 'USD', 48.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('6ccf331f-2863-411a-8f9e-1a28857e2a31', TIMESTAMPTZ '2025-10-30T09:05:00Z', 'seed', 'https://picsum.photos/seed/3-6/640/640', FALSE, '73000000-0000-0000-0000-000000000006', 'fa236302-3864-4e54-9e40-3ebdb4749734', 1, 'DEMO-3-0006', 'Cecilia''s Item #6', TIMESTAMPTZ '2025-11-03T09:05:00Z', 'seed', NULL, 50.99, 'USD', 50.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('7fdde15f-acca-41c7-97a3-e1df2c6a4b8d', TIMESTAMPTZ '2025-10-20T16:00:00Z', 'seed', 'https://picsum.photos/seed/1-8/640/640', FALSE, '71000000-0000-0000-0000-000000000008', '1f3c8b2a-8d14-4a32-9f71-6a9b9f5dd9c4', 1, 'DEMO-1-0008', 'Alice''s Item #8', TIMESTAMPTZ '2025-10-21T16:00:00Z', 'seed', NULL, 36.99, 'USD', 36.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('8fb2678e-8b5d-4d1e-b079-0fb2aa3a055c', TIMESTAMPTZ '2025-10-26T18:40:00Z', 'seed', 'https://picsum.photos/seed/3-1/640/640', FALSE, '73000000-0000-0000-0000-000000000001', 'a4206ad5-6a35-43bb-8a8c-8c7b244594ac', 1, 'DEMO-3-0001', 'Cecilia''s Item #1', TIMESTAMPTZ '2025-10-26T18:40:00Z', 'seed', NULL, 45.99, 'USD', 45.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('9be4d720-31f2-4456-94d7-2bf0c76fa0ec', TIMESTAMPTZ '2025-10-26T18:40:00Z', 'seed', 'https://picsum.photos/seed/3-2/640/640', FALSE, '73000000-0000-0000-0000-000000000002', 'a4206ad5-6a35-43bb-8a8c-8c7b244594ac', 1, 'DEMO-3-0002', 'Cecilia''s Item #2', TIMESTAMPTZ '2025-10-26T18:40:00Z', 'seed', NULL, 46.99, 'USD', 46.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('a3d8f848-7cf3-4058-9f09-3a78d4d64a5d', TIMESTAMPTZ '2025-10-25T15:30:00Z', 'seed', 'https://picsum.photos/seed/2-5/640/640', FALSE, '72000000-0000-0000-0000-000000000005', 'bd34cf77-4551-4194-ad16-d20c94b58289', 1, 'DEMO-2-0005', 'Brian''s Item #5', TIMESTAMPTZ '2025-10-28T15:30:00Z', 'seed', NULL, 41.99, 'USD', 41.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('b7fe44b8-3d3a-49f0-91c5-8ed5cb0c824a', TIMESTAMPTZ '2025-10-25T15:30:00Z', 'seed', 'https://picsum.photos/seed/2-4/640/640', FALSE, '72000000-0000-0000-0000-000000000004', 'bd34cf77-4551-4194-ad16-d20c94b58289', 1, 'DEMO-2-0004', 'Brian''s Item #4', TIMESTAMPTZ '2025-10-28T15:30:00Z', 'seed', NULL, 42.99, 'USD', 42.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c5b7436e-0ae9-4265-9f2b-7a1fd7e7d78f', TIMESTAMPTZ '2025-10-18T09:45:00Z', 'seed', 'https://picsum.photos/seed/1-6/640/640', FALSE, '71000000-0000-0000-0000-000000000006', '7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', 1, 'DEMO-1-0006', 'Alice''s Item #6', TIMESTAMPTZ '2025-10-18T09:45:00Z', 'seed', NULL, 34.99, 'USD', 34.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('e1d40241-43f4-4d93-b9ed-4ac8c9e52088', TIMESTAMPTZ '2025-10-22T08:20:00Z', 'seed', 'https://picsum.photos/seed/2-1/640/640', FALSE, '72000000-0000-0000-0000-000000000001', 'd2ee4d4a-5be0-4d76-bce6-0b8578c87407', 1, 'DEMO-2-0001', 'Brian''s Item #1', TIMESTAMPTZ '2025-10-22T17:20:00Z', 'seed', NULL, 37.99, 'USD', 37.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('e9ad9da9-07b8-42ae-9ce2-764f76d4b657', TIMESTAMPTZ '2025-10-30T09:05:00Z', 'seed', 'https://picsum.photos/seed/3-5/640/640', FALSE, '73000000-0000-0000-0000-000000000005', 'fa236302-3864-4e54-9e40-3ebdb4749734', 1, 'DEMO-3-0005', 'Cecilia''s Item #5', TIMESTAMPTZ '2025-11-03T09:05:00Z', 'seed', NULL, 49.99, 'USD', 49.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('f2a8249e-2643-49b5-bd73-0cac89fb4fc5', TIMESTAMPTZ '2025-10-18T09:45:00Z', 'seed', 'https://picsum.photos/seed/1-7/640/640', FALSE, '71000000-0000-0000-0000-000000000007', '7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', 1, 'DEMO-1-0007', 'Alice''s Item #7', TIMESTAMPTZ '2025-10-18T09:45:00Z', 'seed', NULL, 35.99, 'USD', 35.99, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    ALTER TABLE order_items ADD CONSTRAINT fk_order_items_listing_listing_id FOREIGN KEY (listing_id) REFERENCES listing (id) ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251102111951_UpdateOrderSeeds') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251102111951_UpdateOrderSeeds', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    ALTER TABLE order_shipping_labels ALTER COLUMN order_id DROP DEFAULT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    ALTER TABLE order_shipping_labels ALTER COLUMN id SET DEFAULT (gen_random_uuid());
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    CREATE TABLE order_item_shipments (
        id uuid NOT NULL DEFAULT (gen_random_uuid()),
        order_id uuid NOT NULL,
        order_item_id uuid NOT NULL,
        shipping_label_id uuid,
        tracking_number character varying(120) NOT NULL,
        carrier character varying(120) NOT NULL,
        shipped_at timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_order_item_shipments PRIMARY KEY (id),
        CONSTRAINT fk_order_item_shipments_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE,
        CONSTRAINT fk_order_item_shipments_shipping_labels_shipping_label_id FOREIGN KEY (shipping_label_id) REFERENCES order_shipping_labels (id) ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    INSERT INTO order_cancellation_requests (id, auto_closed_at, buyer_id, buyer_note, completed_at, created_at, created_by, initiated_by, is_deleted, order_id, reason, requested_at, seller_id, seller_note, seller_responded_at, seller_response_deadline_utc, status, updated_at, updated_by, order_total_amount, order_total_currency)
    VALUES ('6f1f9f0c-898f-4c7b-bb38-1b689e9f7331', NULL, '70000000-0000-0000-0000-000000000002', 'Realized I ordered the wrong variation, please cancel.', NULL, TIMESTAMPTZ '2025-10-13T14:15:00Z', 'seed', 0, FALSE, 'c721f605-43cb-4b1b-8f0c-b1c5833420a9', 1, TIMESTAMPTZ '2025-10-13T14:15:00Z', '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-10-15T14:15:00Z', 0, TIMESTAMPTZ '2025-10-13T14:15:00Z', 'seed', 114.69, 'USD');
    INSERT INTO order_cancellation_requests (id, auto_closed_at, buyer_id, buyer_note, completed_at, created_at, created_by, initiated_by, is_deleted, order_id, reason, requested_at, seller_id, seller_note, seller_responded_at, seller_response_deadline_utc, status, updated_at, updated_by, order_total_amount, order_total_currency)
    VALUES ('c3c25c5b-f1a3-4e5f-9ccd-da6a46b91753', TIMESTAMPTZ '2025-10-30T09:00:00Z', '70000000-0000-0000-0000-000000000003', NULL, TIMESTAMPTZ '2025-10-30T09:00:00Z', TIMESTAMPTZ '2025-10-30T09:00:00Z', 'seed', 2, FALSE, '973cac8a-9be0-44a0-90b7-fd8263f8e78a', 99, TIMESTAMPTZ '2025-10-30T09:00:00Z', '70000000-0000-0000-0000-000000000002', 'Order auto-cancelled after missing shipping deadline.', NULL, NULL, 5, TIMESTAMPTZ '2025-10-30T09:00:00Z', 'seed', 97.37, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    INSERT INTO order_cancellation_requests (id, auto_closed_at, buyer_id, buyer_note, completed_at, created_at, created_by, initiated_by, is_deleted, order_id, reason, requested_at, seller_id, seller_note, seller_responded_at, seller_response_deadline_utc, status, updated_at, updated_by, order_total_amount, order_total_currency, refund_amount, refund_currency)
    VALUES ('d3f7d907-6b71-47d8-8651-922629540277', NULL, '70000000-0000-0000-0000-000000000002', 'Need to update the delivery address; requesting cancellation.', NULL, TIMESTAMPTZ '2025-10-19T12:00:00Z', 'seed', 0, FALSE, '7b3b557a-d7cf-4e06-9cbe-6b9968e5a67a', 3, TIMESTAMPTZ '2025-10-19T12:00:00Z', '70000000-0000-0000-0000-000000000001', 'Approved – refund processing with payment provider.', TIMESTAMPTZ '2025-10-19T18:00:00Z', TIMESTAMPTZ '2025-10-21T12:00:00Z', 2, TIMESTAMPTZ '2025-10-19T18:00:00Z', 'seed', 87.83, 'USD', 87.83, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    INSERT INTO order_return_requests (id, buyer_id, buyer_note, buyer_return_due_at, buyer_shipped_at, closed_at, created_at, created_by, delivered_at, is_deleted, order_id, preferred_resolution, reason, refund_issued_at, requested_at, return_carrier, seller_id, seller_note, seller_responded_at, status, tracking_number, updated_at, updated_by, order_total_amount, order_total_currency)
    VALUES ('8cb7ab44-0d7d-4d7d-9b24-1cc54d4da7bf', '70000000-0000-0000-0000-000000000001', 'Item color differs from the listing photos.', NULL, NULL, NULL, TIMESTAMPTZ '2025-10-29T10:00:00Z', 'seed', NULL, FALSE, 'bd34cf77-4551-4194-ad16-d20c94b58289', 0, 0, NULL, TIMESTAMPTZ '2025-10-29T10:00:00Z', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, 0, NULL, TIMESTAMPTZ '2025-10-29T10:00:00Z', 'seed', 108.88, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    INSERT INTO order_return_requests (id, buyer_id, buyer_note, buyer_return_due_at, buyer_shipped_at, closed_at, created_at, created_by, delivered_at, is_deleted, order_id, preferred_resolution, reason, refund_issued_at, requested_at, return_carrier, seller_id, seller_note, seller_responded_at, status, tracking_number, updated_at, updated_by, order_total_amount, order_total_currency, refund_amount, refund_currency, restocking_fee_amount, restocking_fee_currency)
    VALUES ('dc3329e1-14fb-4d00-a395-e76e25a6822b', '70000000-0000-0000-0000-000000000002', 'Shoes run smaller than expected.', TIMESTAMPTZ '2025-11-09T23:59:00Z', TIMESTAMPTZ '2025-11-06T09:10:00Z', TIMESTAMPTZ '2025-11-09T14:00:00Z', TIMESTAMPTZ '2025-11-04T17:45:00Z', 'seed', TIMESTAMPTZ '2025-11-08T16:20:00Z', FALSE, 'fa236302-3864-4e54-9e40-3ebdb4749734', 3, 4, TIMESTAMPTZ '2025-11-09T14:00:00Z', TIMESTAMPTZ '2025-11-04T17:45:00Z', 'USPS', '70000000-0000-0000-0000-000000000003', 'Refunded minus restocking fee.', TIMESTAMPTZ '2025-11-04T20:30:00Z', 5, '9405511899223857264837', TIMESTAMPTZ '2025-11-09T14:00:00Z', 'seed', 177.37, 'USD', 150.0, 'USD', 5.0, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    INSERT INTO order_return_requests (id, buyer_id, buyer_note, buyer_return_due_at, buyer_shipped_at, closed_at, created_at, created_by, delivered_at, is_deleted, order_id, preferred_resolution, reason, refund_issued_at, requested_at, return_carrier, seller_id, seller_note, seller_responded_at, status, tracking_number, updated_at, updated_by, order_total_amount, order_total_currency)
    VALUES ('fd21bed5-6c0c-4bcf-b099-31c8b0d08f27', '70000000-0000-0000-0000-000000000001', 'Screen arrived cracked; requesting replacement.', TIMESTAMPTZ '2025-11-05T23:59:00Z', TIMESTAMPTZ '2025-11-03T10:15:00Z', NULL, TIMESTAMPTZ '2025-11-01T09:00:00Z', 'seed', NULL, FALSE, '1e86f219-1dd0-4cac-a545-cb98e65ce429', 1, 1, NULL, TIMESTAMPTZ '2025-11-01T09:00:00Z', 'UPS', '70000000-0000-0000-0000-000000000003', 'Please return using the provided UPS label.', TIMESTAMPTZ '2025-11-01T12:00:00Z', 2, '1Z999AA10123456784', TIMESTAMPTZ '2025-11-03T10:15:00Z', 'seed', 169.27, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    CREATE INDEX ix_order_item_shipments_order_id ON order_item_shipments (order_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    CREATE INDEX ix_order_item_shipments_order_item_id ON order_item_shipments (order_item_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    CREATE INDEX ix_order_item_shipments_shipping_label_id ON order_item_shipments (shipping_label_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105122228_AddOrderItemShipments') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251105122228_AddOrderItemShipments', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105173454_AddSellerCoupons') THEN
    CREATE TABLE coupon_type (
        id uuid NOT NULL,
        name character varying(100) NOT NULL,
        description character varying(255),
        is_active boolean NOT NULL,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL DEFAULT FALSE,
        CONSTRAINT pk_coupon_type PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105173454_AddSellerCoupons') THEN
    CREATE TABLE coupon (
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
        is_deleted boolean NOT NULL DEFAULT FALSE,
        CONSTRAINT pk_coupon PRIMARY KEY (id),
        CONSTRAINT fk_coupon_coupon_types_coupon_type_id FOREIGN KEY (coupon_type_id) REFERENCES coupon_type (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105173454_AddSellerCoupons') THEN
    CREATE TABLE coupon_condition (
        id uuid NOT NULL,
        coupon_id uuid NOT NULL,
        buy_quantity integer,
        get_quantity integer,
        get_discount_percent numeric(5,2),
        save_every_amount numeric(10,2),
        save_every_items integer,
        condition_description character varying(255),
        CONSTRAINT pk_coupon_condition PRIMARY KEY (id),
        CONSTRAINT fk_coupon_condition_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES coupon (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105173454_AddSellerCoupons') THEN
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('0d0c32fe-349c-4857-b20a-2d3f8db91ed4', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Percentage discount when buying Y or more items', TRUE, 'Extra % off Y or more items', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('2c5a6a6a-fe7e-4813-a134-70572b5ab90a', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Percentage discount when total order value reaches a minimum value', TRUE, 'Extra % off $ or more', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('3b980145-62b6-4ae6-9cf8-7838bc7b84e0', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Save a fixed amount for every X items purchased', TRUE, 'Save $ for every X items', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('51f2ed38-06bb-496e-b5cb-7aa3057c21b7', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Save a fixed amount for every dollar spent', TRUE, 'Save $ for every $ spent', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('773f8d9b-eb8e-4ff4-a21e-4bb2fa5407f4', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Fixed amount discount when buying X or more items', TRUE, 'Extra $ off X or more items', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('7a5a0b7a-ed8f-4b91-a7c3-59e5363b76f3', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Fixed amount discount for each item purchased', TRUE, 'Extra $ off each item', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('7eaa19cf-6b36-4a1c-b7b5-a9abcb7eeff2', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Buy X items and get Y items at a percentage discount', TRUE, 'Buy X get Y at % off', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('990c28b3-753e-41b1-a798-965cf46b7dcd', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Fixed amount discount on all eligible items', TRUE, 'Extra $ off', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('9e1d4ea5-5b09-48be-be90-e2790f6ba537', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Percentage discount on all eligible items', TRUE, 'Extra % off', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('cfa2e0f1-b720-4590-a7d4-4ce0844f9671', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Fixed amount discount when order value reaches a minimum threshold', TRUE, 'Extra $ off $ or more', NULL, NULL);
    INSERT INTO coupon_type (id, created_at, created_by, description, is_active, name, updated_at, updated_by)
    VALUES ('ed9d5151-6f8c-4628-a5a9-4c24867e5673', TIMESTAMPTZ '2024-01-01T00:00:00Z', NULL, 'Buy X items and get Y items for free', TRUE, 'Buy X get Y free', NULL, NULL);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105173454_AddSellerCoupons') THEN
    CREATE INDEX ix_coupon_coupon_type_id ON coupon (coupon_type_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105173454_AddSellerCoupons') THEN
    CREATE UNIQUE INDEX ux_coupon_code ON coupon (code);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105173454_AddSellerCoupons') THEN
    CREATE INDEX ix_coupon_condition_coupon_id ON coupon_condition (coupon_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251105173454_AddSellerCoupons') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251105173454_AddSellerCoupons', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251107182651_AddBuyerFeedback') THEN
    CREATE TABLE order_buyer_feedback (
        id uuid NOT NULL,
        order_id uuid NOT NULL,
        seller_id uuid NOT NULL,
        buyer_id uuid NOT NULL,
        comment character varying(80) NOT NULL,
        uses_stored_comment boolean NOT NULL,
        stored_comment_key character varying(100),
        created_at timestamp with time zone NOT NULL,
        follow_up_comment character varying(80),
        follow_up_commented_at timestamp with time zone,
        CONSTRAINT pk_order_buyer_feedback PRIMARY KEY (id),
        CONSTRAINT fk_order_buyer_feedback_orders_order_id FOREIGN KEY (order_id) REFERENCES orders (id) ON DELETE CASCADE,
        CONSTRAINT fk_order_buyer_feedback_user_buyer_id FOREIGN KEY (buyer_id) REFERENCES "user" (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251107182651_AddBuyerFeedback') THEN
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000011', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-11-01T09:15:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1011', TIMESTAMPTZ '2025-11-01T09:15:00Z', TIMESTAMPTZ '2025-11-01T09:50:00Z', NULL, '70000000-0000-0000-0000-000000000001', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-01T09:50:00Z', 'seed', 0.0, 'USD', 3.1, 'USD', 9.95, 'USD', 58.99, 'USD', 5.3, 'USD', 77.34, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000012', NULL, '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-11-02T13:30:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1012', TIMESTAMPTZ '2025-11-02T13:30:00Z', TIMESTAMPTZ '2025-11-02T14:10:00Z', NULL, '70000000-0000-0000-0000-000000000002', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-02T14:10:00Z', 'seed', 2.5, 'USD', 3.45, 'USD', 8.25, 'USD', 64.5, 'USD', 4.86, 'USD', 78.56, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000013', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-11-03T17:05:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1013', TIMESTAMPTZ '2025-11-03T17:05:00Z', TIMESTAMPTZ '2025-11-03T17:55:00Z', NULL, '70000000-0000-0000-0000-000000000003', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-03T17:55:00Z', 'seed', 0.0, 'USD', 3.9, 'USD', 10.0, 'USD', 72.0, 'USD', 6.12, 'USD', 92.02, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000014', NULL, '70000000-0000-0000-0000-000000000003', NULL, NULL, TIMESTAMPTZ '2025-11-04T08:45:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1014', TIMESTAMPTZ '2025-11-04T08:45:00Z', TIMESTAMPTZ '2025-11-04T09:13:00Z', NULL, '70000000-0000-0000-0000-000000000001', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-04T09:13:00Z', 'seed', 1.2, 'USD', 2.95, 'USD', 7.8, 'USD', 55.75, 'USD', 4.46, 'USD', 69.76, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251107182651_AddBuyerFeedback') THEN
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000021', TIMESTAMPTZ '2025-11-01T09:15:00Z', 'seed', 'https://picsum.photos/seed/1-10/640/640', FALSE, '71000000-0000-0000-0000-00000000000a', '0f0c1a22-11aa-4c6d-8f10-000000000011', 1, 'DEMO-1-0010', 'Alice''s Item #10', TIMESTAMPTZ '2025-11-01T09:50:00Z', 'seed', NULL, 58.99, 'USD', 58.99, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000022', TIMESTAMPTZ '2025-11-02T13:30:00Z', 'seed', 'https://picsum.photos/seed/2-6/640/640', FALSE, '72000000-0000-0000-0000-000000000006', '0f0c1a22-11aa-4c6d-8f10-000000000012', 1, 'DEMO-2-0006', 'Brian''s Item #6', TIMESTAMPTZ '2025-11-02T14:10:00Z', 'seed', NULL, 64.5, 'USD', 64.5, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000023', TIMESTAMPTZ '2025-11-03T17:05:00Z', 'seed', 'https://picsum.photos/seed/3-8/640/640', FALSE, '73000000-0000-0000-0000-000000000008', '0f0c1a22-11aa-4c6d-8f10-000000000013', 1, 'DEMO-3-0008', 'Cecilia''s Item #8', TIMESTAMPTZ '2025-11-03T17:55:00Z', 'seed', NULL, 72.0, 'USD', 72.0, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000024', TIMESTAMPTZ '2025-11-04T08:45:00Z', 'seed', 'https://picsum.photos/seed/1-11/640/640', FALSE, '71000000-0000-0000-0000-00000000000b', '0f0c1a22-11aa-4c6d-8f10-000000000014', 1, 'DEMO-1-0011', 'Alice''s Item #11', TIMESTAMPTZ '2025-11-04T09:13:00Z', 'seed', NULL, 55.75, 'USD', 55.75, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251107182651_AddBuyerFeedback') THEN
    CREATE INDEX ix_order_buyer_feedback_buyer_id ON order_buyer_feedback (buyer_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251107182651_AddBuyerFeedback') THEN
    CREATE UNIQUE INDEX ux_buyer_feedback_order ON order_buyer_feedback (order_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251107182651_AddBuyerFeedback') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251107182651_AddBuyerFeedback', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251111172411_AddSaleEventsModule') THEN
    CREATE TABLE sale_event (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_sale_event PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251111172411_AddSaleEventsModule') THEN
    CREATE TABLE sale_event_discount_tier (
        id uuid NOT NULL,
        sale_event_id uuid NOT NULL,
        discount_type character varying(20) NOT NULL,
        discount_value numeric(10,2) NOT NULL,
        priority integer NOT NULL,
        label character varying(100),
        CONSTRAINT pk_sale_event_discount_tier PRIMARY KEY (id),
        CONSTRAINT fk_sale_event_discount_tier_sale_event_sale_event_id FOREIGN KEY (sale_event_id) REFERENCES sale_event (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251111172411_AddSaleEventsModule') THEN
    CREATE TABLE sale_event_listing (
        id uuid NOT NULL,
        sale_event_id uuid NOT NULL,
        discount_tier_id uuid NOT NULL,
        listing_id uuid NOT NULL,
        CONSTRAINT pk_sale_event_listing PRIMARY KEY (id),
        CONSTRAINT fk_sale_event_listing_sale_event_discount_tier_discount_tier_id FOREIGN KEY (discount_tier_id) REFERENCES sale_event_discount_tier (id) ON DELETE CASCADE,
        CONSTRAINT fk_sale_event_listing_sale_event_sale_event_id FOREIGN KEY (sale_event_id) REFERENCES sale_event (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251111172411_AddSaleEventsModule') THEN
    CREATE INDEX ix_sale_event_discount_tier_sale_event_id ON sale_event_discount_tier (sale_event_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251111172411_AddSaleEventsModule') THEN
    CREATE INDEX ix_sale_event_listing_discount_tier_id ON sale_event_listing (discount_tier_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251111172411_AddSaleEventsModule') THEN
    CREATE UNIQUE INDEX ix_sale_event_listing_sale_event_id_listing_id ON sale_event_listing (sale_event_id, listing_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251111172411_AddSaleEventsModule') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251111172411_AddSaleEventsModule', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    CREATE TABLE seller_preference (
        id uuid NOT NULL,
        seller_id uuid NOT NULL,
        listings_stay_active_when_out_of_stock boolean NOT NULL DEFAULT FALSE,
        show_exact_quantity_available boolean NOT NULL DEFAULT TRUE,
        buyers_can_see_vat_number boolean NOT NULL DEFAULT FALSE,
        vat_number character varying(200),
        block_unpaid_item_strikes boolean NOT NULL DEFAULT FALSE,
        unpaid_item_strikes_count integer NOT NULL DEFAULT 0,
        unpaid_item_strikes_period_months integer NOT NULL DEFAULT 0,
        block_primary_address_outside_shipping_location boolean NOT NULL DEFAULT TRUE,
        block_max_items_last_ten_days boolean NOT NULL DEFAULT FALSE,
        max_items_last_ten_days integer,
        apply_feedback_score_threshold boolean NOT NULL DEFAULT FALSE,
        feedback_score_threshold integer,
        update_block_settings_active_listings boolean NOT NULL DEFAULT FALSE,
        require_payment_method_before_bid boolean NOT NULL DEFAULT TRUE,
        require_payment_method_before_offer boolean NOT NULL DEFAULT TRUE,
        prevent_blocked_buyers_contacting boolean NOT NULL DEFAULT TRUE,
        invoice_format integer NOT NULL,
        invoice_send_email_copy boolean NOT NULL DEFAULT TRUE,
        invoice_apply_credits_automatically boolean NOT NULL DEFAULT TRUE,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_seller_preference PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    CREATE TABLE seller_blocked_buyer (
        id uuid NOT NULL,
        identifier character varying(200) NOT NULL,
        normalized_identifier character varying(200) NOT NULL,
        created_at_utc timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        seller_preference_id uuid NOT NULL,
        CONSTRAINT pk_seller_blocked_buyer PRIMARY KEY (id),
        CONSTRAINT fk_seller_blocked_buyer_seller_preference_seller_preference_id FOREIGN KEY (seller_preference_id) REFERENCES seller_preference (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    CREATE TABLE seller_exempt_buyer (
        id uuid NOT NULL,
        identifier character varying(200) NOT NULL,
        normalized_identifier character varying(200) NOT NULL,
        created_at_utc timestamp with time zone NOT NULL DEFAULT (now() at time zone 'utc'),
        seller_preference_id uuid NOT NULL,
        CONSTRAINT pk_seller_exempt_buyer PRIMARY KEY (id),
        CONSTRAINT fk_seller_exempt_buyer_seller_preference_seller_preference_id FOREIGN KEY (seller_preference_id) REFERENCES seller_preference (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    CREATE INDEX ix_seller_blocked_buyer_seller_preference_id ON seller_blocked_buyer (seller_preference_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    CREATE UNIQUE INDEX ux_seller_blocked_buyer_identifier ON seller_blocked_buyer (normalized_identifier, seller_preference_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    CREATE INDEX ix_seller_exempt_buyer_seller_preference_id ON seller_exempt_buyer (seller_preference_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    CREATE UNIQUE INDEX ux_seller_exempt_buyer_identifier ON seller_exempt_buyer (normalized_identifier, seller_preference_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    CREATE UNIQUE INDEX ux_seller_preference_seller ON seller_preference (seller_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135441_SellerPreferenceIndexFix') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251112135441_SellerPreferenceIndexFix', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251112135954_SellerPreferenceIndexFix1') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251112135954_SellerPreferenceIndexFix1', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    INSERT INTO order_status_transitions (id, allowed_roles, from_status_id, to_status_id)
    VALUES ('1bd31fd1-5a79-4a8e-9035-7cbc71dbb8b9', 'SYSTEM', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91');
    INSERT INTO order_status_transitions (id, allowed_roles, from_status_id, to_status_id)
    VALUES ('5a3f5769-6c6d-4b89-9347-118bd3fba3d6', 'SYSTEM', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad');
    INSERT INTO order_status_transitions (id, allowed_roles, from_status_id, to_status_id)
    VALUES ('7cf6e659-8025-49e8-94d5-3a4dd3b5a793', 'SYSTEM', 'dd9f2c6a-8991-4af2-8d80-c1a3c1cdd1ad', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060');
    INSERT INTO order_status_transitions (id, allowed_roles, from_status_id, to_status_id)
    VALUES ('8c6f6f3e-18c6-4aa5-ba61-033fa3c0bb0e', 'SYSTEM', '3c8a4f5d-1b89-4a5e-bc53-2612b72d3060', '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000015', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-11-05T10:00:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1015', TIMESTAMPTZ '2025-11-05T10:00:00Z', TIMESTAMPTZ '2025-11-05T10:30:00Z', NULL, '70000000-0000-0000-0000-000000000001', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-05T10:30:00Z', 'seed', 2.5, 'USD', 3.15, 'USD', 8.25, 'USD', 62.75, 'USD', 5.02, 'USD', 76.67, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000016', NULL, '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-11-05T11:30:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1016', TIMESTAMPTZ '2025-11-05T11:30:00Z', TIMESTAMPTZ '2025-11-05T11:55:00Z', NULL, '70000000-0000-0000-0000-000000000002', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-05T11:55:00Z', 'seed', 0.0, 'USD', 2.45, 'USD', 7.5, 'USD', 48.4, 'USD', 3.87, 'USD', 62.22, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000017', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-11-06T09:20:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1017', TIMESTAMPTZ '2025-11-06T09:20:00Z', TIMESTAMPTZ '2025-11-06T09:40:00Z', NULL, '70000000-0000-0000-0000-000000000003', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-06T09:40:00Z', 'seed', 0.0, 'USD', 3.95, 'USD', 9.95, 'USD', 79.9, 'USD', 6.39, 'USD', 100.19, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000018', NULL, '70000000-0000-0000-0000-000000000003', NULL, NULL, TIMESTAMPTZ '2025-11-06T13:45:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1018', TIMESTAMPTZ '2025-11-06T13:45:00Z', TIMESTAMPTZ '2025-11-06T14:20:00Z', NULL, '70000000-0000-0000-0000-000000000001', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-06T14:20:00Z', 'seed', 3.0, 'USD', 4.3, 'USD', 10.25, 'USD', 88.6, 'USD', 7.08, 'USD', 107.23, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-000000000019', NULL, '70000000-0000-0000-0000-000000000003', NULL, NULL, TIMESTAMPTZ '2025-11-07T08:10:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1019', TIMESTAMPTZ '2025-11-07T08:10:00Z', TIMESTAMPTZ '2025-11-07T08:28:00Z', NULL, '70000000-0000-0000-0000-000000000002', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-07T08:28:00Z', 'seed', 0.0, 'USD', 3.55, 'USD', 8.75, 'USD', 71.25, 'USD', 5.7, 'USD', 89.25, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001a', NULL, '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-11-07T15:25:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1020', TIMESTAMPTZ '2025-11-07T15:25:00Z', TIMESTAMPTZ '2025-11-07T15:47:00Z', NULL, '70000000-0000-0000-0000-000000000003', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-07T15:47:00Z', 'seed', 1.8, 'USD', 3.25, 'USD', 8.4, 'USD', 65.8, 'USD', 4.94, 'USD', 80.59, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001b', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-11-08T10:40:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1021', TIMESTAMPTZ '2025-11-08T10:40:00Z', TIMESTAMPTZ '2025-11-08T11:07:00Z', NULL, '70000000-0000-0000-0000-000000000001', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-08T11:07:00Z', 'seed', 0.0, 'USD', 2.95, 'USD', 7.95, 'USD', 59.1, 'USD', 4.43, 'USD', 74.43, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001c', NULL, '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-11-08T14:05:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1022', TIMESTAMPTZ '2025-11-08T14:05:00Z', TIMESTAMPTZ '2025-11-08T14:37:00Z', NULL, '70000000-0000-0000-0000-000000000002', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-08T14:37:00Z', 'seed', 2.2, 'USD', 3.8, 'USD', 9.1, 'USD', 83.45, 'USD', 6.68, 'USD', 100.83, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001d', NULL, '70000000-0000-0000-0000-000000000002', NULL, NULL, TIMESTAMPTZ '2025-11-09T11:15:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1023', TIMESTAMPTZ '2025-11-09T11:15:00Z', TIMESTAMPTZ '2025-11-09T11:39:00Z', NULL, '70000000-0000-0000-0000-000000000003', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-09T11:39:00Z', 'seed', 0.0, 'USD', 4.45, 'USD', 10.6, 'USD', 90.3, 'USD', 7.22, 'USD', 112.57, 'USD');
    INSERT INTO orders (id, archived_at, buyer_id, cancelled_at, coupon_code, created_at, created_by, delivered_at, fulfillment_type, is_deleted, order_number, ordered_at, paid_at, promotion_id, seller_id, shipped_at, shipping_status, status_id, updated_at, updated_by, discount_amount, discount_currency, platform_fee_amount, platform_fee_currency, shipping_cost_amount, shipping_cost_currency, sub_total_amount, sub_total_currency, tax_amount, tax_currency, total_amount, total_currency)
    VALUES ('0f0c1a22-11aa-4c6d-8f10-00000000001e', NULL, '70000000-0000-0000-0000-000000000003', NULL, NULL, TIMESTAMPTZ '2025-11-09T17:50:00Z', 'seed', NULL, 0, FALSE, 'ORD-SEED-1024', TIMESTAMPTZ '2025-11-09T17:50:00Z', TIMESTAMPTZ '2025-11-09T18:19:00Z', NULL, '70000000-0000-0000-0000-000000000001', NULL, 0, '9b2cf3b0-0b4a-4e14-bc05-5a2f87a4ef91', TIMESTAMPTZ '2025-11-09T18:19:00Z', 'seed', 1.5, 'USD', 3.6, 'USD', 8.9, 'USD', 74.95, 'USD', 5.83, 'USD', 91.78, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    INSERT INTO order_cancellation_requests (id, auto_closed_at, buyer_id, buyer_note, completed_at, created_at, created_by, initiated_by, is_deleted, order_id, reason, requested_at, seller_id, seller_note, seller_responded_at, seller_response_deadline_utc, status, updated_at, updated_by, order_total_amount, order_total_currency, refund_amount, refund_currency)
    VALUES ('5d4e7a11-0c4e-4a6f-9f2f-000000000004', NULL, '70000000-0000-0000-0000-000000000003', 'Item still not handed to carrier, requesting cancellation.', NULL, TIMESTAMPTZ '2025-11-06T18:00:00Z', 'seed', 0, FALSE, '0f0c1a22-11aa-4c6d-8f10-000000000018', 4, TIMESTAMPTZ '2025-11-06T18:00:00Z', '70000000-0000-0000-0000-000000000001', 'Approved – refund issued to buyer''s original payment method.', TIMESTAMPTZ '2025-11-07T09:30:00Z', TIMESTAMPTZ '2025-11-08T18:00:00Z', 2, TIMESTAMPTZ '2025-11-07T09:30:00Z', 'seed', 107.23, 'USD', 107.23, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    INSERT INTO order_cancellation_requests (id, auto_closed_at, buyer_id, buyer_note, completed_at, created_at, created_by, initiated_by, is_deleted, order_id, reason, requested_at, seller_id, seller_note, seller_responded_at, seller_response_deadline_utc, status, updated_at, updated_by, order_total_amount, order_total_currency)
    VALUES ('5d4e7a11-0c4e-4a6f-9f2f-000000000005', NULL, '70000000-0000-0000-0000-000000000002', 'Accidentally placed duplicate order.', NULL, TIMESTAMPTZ '2025-11-08T11:00:00Z', 'seed', 0, FALSE, '0f0c1a22-11aa-4c6d-8f10-00000000001b', 1, TIMESTAMPTZ '2025-11-08T11:00:00Z', '70000000-0000-0000-0000-000000000001', NULL, NULL, TIMESTAMPTZ '2025-11-10T11:00:00Z', 0, TIMESTAMPTZ '2025-11-08T11:00:00Z', 'seed', 74.43, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000025', TIMESTAMPTZ '2025-11-05T10:00:00Z', 'seed', 'https://picsum.photos/seed/1-12/640/640', FALSE, '71000000-0000-0000-0000-00000000000c', '0f0c1a22-11aa-4c6d-8f10-000000000015', 1, 'DEMO-1-0012', 'Alice''s Item #12', TIMESTAMPTZ '2025-11-05T10:30:00Z', 'seed', NULL, 62.75, 'USD', 62.75, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000026', TIMESTAMPTZ '2025-11-05T11:30:00Z', 'seed', 'https://picsum.photos/seed/2-7/640/640', FALSE, '72000000-0000-0000-0000-000000000007', '0f0c1a22-11aa-4c6d-8f10-000000000016', 1, 'DEMO-2-0007', 'Brian''s Item #7', TIMESTAMPTZ '2025-11-05T11:55:00Z', 'seed', NULL, 48.4, 'USD', 48.4, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000027', TIMESTAMPTZ '2025-11-06T09:20:00Z', 'seed', 'https://picsum.photos/seed/3-9/640/640', FALSE, '73000000-0000-0000-0000-000000000009', '0f0c1a22-11aa-4c6d-8f10-000000000017', 1, 'DEMO-3-0009', 'Cecilia''s Item #9', TIMESTAMPTZ '2025-11-06T09:40:00Z', 'seed', NULL, 79.9, 'USD', 79.9, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000028', TIMESTAMPTZ '2025-11-06T13:45:00Z', 'seed', 'https://picsum.photos/seed/1-13/640/640', FALSE, '71000000-0000-0000-0000-00000000000d', '0f0c1a22-11aa-4c6d-8f10-000000000018', 1, 'DEMO-1-0013', 'Alice''s Item #13', TIMESTAMPTZ '2025-11-06T14:20:00Z', 'seed', NULL, 88.6, 'USD', 88.6, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-000000000029', TIMESTAMPTZ '2025-11-07T08:10:00Z', 'seed', 'https://picsum.photos/seed/2-8/640/640', FALSE, '72000000-0000-0000-0000-000000000008', '0f0c1a22-11aa-4c6d-8f10-000000000019', 1, 'DEMO-2-0008', 'Brian''s Item #8', TIMESTAMPTZ '2025-11-07T08:28:00Z', 'seed', NULL, 71.25, 'USD', 71.25, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-00000000002a', TIMESTAMPTZ '2025-11-07T15:25:00Z', 'seed', 'https://picsum.photos/seed/3-10/640/640', FALSE, '73000000-0000-0000-0000-00000000000a', '0f0c1a22-11aa-4c6d-8f10-00000000001a', 1, 'DEMO-3-0010', 'Cecilia''s Item #10', TIMESTAMPTZ '2025-11-07T15:47:00Z', 'seed', NULL, 65.8, 'USD', 65.8, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-00000000002b', TIMESTAMPTZ '2025-11-08T10:40:00Z', 'seed', 'https://picsum.photos/seed/1-14/640/640', FALSE, '71000000-0000-0000-0000-00000000000e', '0f0c1a22-11aa-4c6d-8f10-00000000001b', 1, 'DEMO-1-0014', 'Alice''s Item #14', TIMESTAMPTZ '2025-11-08T11:07:00Z', 'seed', NULL, 59.1, 'USD', 59.1, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-00000000002c', TIMESTAMPTZ '2025-11-08T14:05:00Z', 'seed', 'https://picsum.photos/seed/2-9/640/640', FALSE, '72000000-0000-0000-0000-000000000009', '0f0c1a22-11aa-4c6d-8f10-00000000001c', 1, 'DEMO-2-0009', 'Brian''s Item #9', TIMESTAMPTZ '2025-11-08T14:37:00Z', 'seed', NULL, 83.45, 'USD', 83.45, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-00000000002d', TIMESTAMPTZ '2025-11-09T11:15:00Z', 'seed', 'https://picsum.photos/seed/3-11/640/640', FALSE, '73000000-0000-0000-0000-00000000000b', '0f0c1a22-11aa-4c6d-8f10-00000000001d', 1, 'DEMO-3-0011', 'Cecilia''s Item #11', TIMESTAMPTZ '2025-11-09T11:39:00Z', 'seed', NULL, 90.3, 'USD', 90.3, 'USD');
    INSERT INTO order_items (id, created_at, created_by, image_url, is_deleted, listing_id, order_id, quantity, sku, title, updated_at, updated_by, variation_id, total_price_amount, total_price_currency, unit_price_amount, unit_price_currency)
    VALUES ('c579fb6b-b172-4e17-b610-00000000002e', TIMESTAMPTZ '2025-11-09T17:50:00Z', 'seed', 'https://picsum.photos/seed/1-15/640/640', FALSE, '71000000-0000-0000-0000-00000000000f', '0f0c1a22-11aa-4c6d-8f10-00000000001e', 1, 'DEMO-1-0015', 'Alice''s Item #15', TIMESTAMPTZ '2025-11-09T18:19:00Z', 'seed', NULL, 74.95, 'USD', 74.95, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    INSERT INTO order_return_requests (id, buyer_id, buyer_note, buyer_return_due_at, buyer_shipped_at, closed_at, created_at, created_by, delivered_at, is_deleted, order_id, preferred_resolution, reason, refund_issued_at, requested_at, return_carrier, seller_id, seller_note, seller_responded_at, status, tracking_number, updated_at, updated_by, order_total_amount, order_total_currency)
    VALUES ('9a7f6b12-5e2d-4d91-8c22-000000000004', '70000000-0000-0000-0000-000000000001', 'Received the 64GB variant instead of 128GB.', TIMESTAMPTZ '2025-11-13T23:59:00Z', TIMESTAMPTZ '2025-11-10T10:20:00Z', NULL, TIMESTAMPTZ '2025-11-08T09:30:00Z', 'seed', NULL, FALSE, '0f0c1a22-11aa-4c6d-8f10-00000000001a', 2, 2, NULL, TIMESTAMPTZ '2025-11-08T09:30:00Z', 'FedEx', '70000000-0000-0000-0000-000000000003', 'Exchange approved once return is in transit.', TIMESTAMPTZ '2025-11-08T12:45:00Z', 2, '612999AA10NEWRT4', TIMESTAMPTZ '2025-11-10T10:20:00Z', 'seed', 80.59, 'USD');
    INSERT INTO order_return_requests (id, buyer_id, buyer_note, buyer_return_due_at, buyer_shipped_at, closed_at, created_at, created_by, delivered_at, is_deleted, order_id, preferred_resolution, reason, refund_issued_at, requested_at, return_carrier, seller_id, seller_note, seller_responded_at, status, tracking_number, updated_at, updated_by, order_total_amount, order_total_currency)
    VALUES ('9a7f6b12-5e2d-4d91-8c22-000000000005', '70000000-0000-0000-0000-000000000002', 'Decided to keep a different model instead.', TIMESTAMPTZ '2025-11-14T23:59:00Z', TIMESTAMPTZ '2025-11-11T08:40:00Z', NULL, TIMESTAMPTZ '2025-11-09T15:00:00Z', 'seed', TIMESTAMPTZ '2025-11-13T16:05:00Z', FALSE, '0f0c1a22-11aa-4c6d-8f10-00000000001d', 0, 6, NULL, TIMESTAMPTZ '2025-11-09T15:00:00Z', 'USPS', '70000000-0000-0000-0000-000000000003', 'Refund pending inspection of returned item.', TIMESTAMPTZ '2025-11-09T17:15:00Z', 4, '9405511899223857264999', TIMESTAMPTZ '2025-11-13T16:05:00Z', 'seed', 112.57, 'USD');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    CREATE INDEX ix_dispute_listing_id ON dispute (listing_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    CREATE INDEX ix_dispute_raised_by_id ON dispute (raised_by_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    CREATE INDEX ix_dispute_status ON dispute (status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    CREATE INDEX ix_review_listing_id ON review (listing_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    CREATE INDEX ix_review_recipient_id ON review (recipient_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    CREATE INDEX ix_review_recipient_role ON review (recipient_role);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    CREATE INDEX ix_review_reviewer_id ON review (reviewer_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20251113202832_AddOrderSeed2s') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20251113202832_AddOrderSeed2s', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD business_city character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD business_country character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD business_name character varying(200);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD business_state character varying(100);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD business_street character varying(300);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD business_zip_code character varying(20);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD is_business_verified boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD is_phone_verified boolean NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    ALTER TABLE "user" ADD phone_number character varying(20);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    UPDATE "user" SET business_name = NULL, is_business_verified = FALSE, is_phone_verified = FALSE, phone_number = NULL
    WHERE id = '70000000-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    UPDATE "user" SET business_name = NULL, is_business_verified = FALSE, is_phone_verified = FALSE, phone_number = NULL
    WHERE id = '70000000-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    UPDATE "user" SET business_name = NULL, is_business_verified = FALSE, is_phone_verified = FALSE, phone_number = NULL
    WHERE id = '70000000-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260306132214_AddSellerVerificationFields') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260306132214_AddSellerVerificationFields', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE TABLE coupon_excluded_categories (
        id uuid NOT NULL,
        coupon_id uuid NOT NULL,
        category_id uuid NOT NULL,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_coupon_excluded_categories PRIMARY KEY (id),
        CONSTRAINT fk_coupon_excluded_categories_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES coupon (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE TABLE coupon_excluded_items (
        id uuid NOT NULL,
        coupon_id uuid NOT NULL,
        item_id uuid NOT NULL,
        created_at timestamp with time zone NOT NULL,
        created_by text,
        updated_at timestamp with time zone,
        updated_by text,
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_coupon_excluded_items PRIMARY KEY (id),
        CONSTRAINT fk_coupon_excluded_items_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES coupon (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE TABLE coupon_target_audiences (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_coupon_target_audiences PRIMARY KEY (id),
        CONSTRAINT fk_coupon_target_audiences_coupons_coupon_id FOREIGN KEY (coupon_id) REFERENCES coupon (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE TABLE vouchers (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_vouchers PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE TABLE voucher_transactions (
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
        is_deleted boolean NOT NULL,
        CONSTRAINT pk_voucher_transactions PRIMARY KEY (id),
        CONSTRAINT fk_voucher_transactions_vouchers_voucher_id FOREIGN KEY (voucher_id) REFERENCES vouchers (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE INDEX ix_coupon_excluded_categories_coupon_id ON coupon_excluded_categories (coupon_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE INDEX ix_coupon_excluded_items_coupon_id ON coupon_excluded_items (coupon_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE INDEX ix_coupon_target_audiences_coupon_id ON coupon_target_audiences (coupon_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    CREATE INDEX ix_voucher_transactions_voucher_id ON voucher_transactions (voucher_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307061213_AddVoucherAndCouponEnhancement') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260307061213_AddVoucherAndCouponEnhancement', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    ALTER TABLE order_items ADD category_id uuid;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '0a3e9070-0a5e-4114-8634-8e9353a5369e';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '1b1eaa3e-0e34-4df1-8c5a-4035ef7aad6d';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '30f2c0f3-09bb-4f52-93a9-6e98b0171c3f';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '3e54a8a8-3b35-4bdf-9d09-75042c7f7d4f';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '4a1ab1de-4a10-4326-a0be-5d3ab27c9df7';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '55c9f2a2-dba1-4c66-9b83-a8b4c9e7a0d4';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '5f2f8987-3b95-4b9f-8cc0-0f7c4b8d3b92';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '6bd3f47d-4f1e-467f-8797-3b2a151dd09f';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '6cbb0f3e-9fd9-4c83-b181-74d3432fb953';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '6ccf331f-2863-411a-8f9e-1a28857e2a31';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '7fdde15f-acca-41c7-97a3-e1df2c6a4b8d';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '8fb2678e-8b5d-4d1e-b079-0fb2aa3a055c';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = '9be4d720-31f2-4456-94d7-2bf0c76fa0ec';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'a3d8f848-7cf3-4058-9f09-3a78d4d64a5d';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'a9d23977-7d99-4d44-bb79-4cff5ec2f56f';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'b7fe44b8-3d3a-49f0-91c5-8ed5cb0c824a';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000021';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000022';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000023';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000024';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000025';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000026';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000027';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000028';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-000000000029';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-00000000002a';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-00000000002b';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-00000000002c';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-00000000002d';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c579fb6b-b172-4e17-b610-00000000002e';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'c5b7436e-0ae9-4265-9f2b-7a1fd7e7d78f';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'e1d40241-43f4-4d93-b9ed-4ac8c9e52088';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'e9ad9da9-07b8-42ae-9ce2-764f76d4b657';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    UPDATE order_items SET category_id = NULL
    WHERE id = 'f2a8249e-2643-49b5-bd73-0cac89fb4fc5';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062047_AddCategoryIdToOrderItem') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260307062047_AddCategoryIdToOrderItem', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307062735_AddVoucherTables') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260307062735_AddVoucherTables', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307105552_AddStoreExtensibility') THEN
    ALTER TABLE store ADD contact_email character varying(255);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307105552_AddStoreExtensibility') THEN
    ALTER TABLE store ADD contact_phone character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307105552_AddStoreExtensibility') THEN
    ALTER TABLE store ADD layout_config text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307105552_AddStoreExtensibility') THEN
    ALTER TABLE store ADD social_links text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307105552_AddStoreExtensibility') THEN
    ALTER TABLE store ADD theme_color character varying(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260307105552_AddStoreExtensibility') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260307105552_AddStoreExtensibility', '9.0.8');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260308190334_AddStoreExtensibility2') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260308190334_AddStoreExtensibility2', '9.0.8');
    END IF;
END $EF$;
COMMIT;

