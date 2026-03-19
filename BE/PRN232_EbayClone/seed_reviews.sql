-- Seed data for reviews
-- This script inserts sample reviews for testing the seller review functionality

-- Using existing users from database as buyers:
-- Alice Johnson (seller): 70000000-0000-0000-0000-000000000001
-- Brian Carter: 70000000-0000-0000-0000-000000000002  
-- Cecilia Gomez: 70000000-0000-0000-0000-000000000003
-- Vu Truong Giang: 038a4659-83c4-456d-9bf9-e2036a53ad6c
-- Bui Hoang Viet: e167368a-7042-45a4-8f1f-badba56b6b63
-- Test User: 741eb167-fb61-4b47-a35a-90720c4949f7
-- Nguyen Duc Thinh: c483385e-f1b9-40b4-9b95-e18f45e06a82
-- Do Huu Hoa: 602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c

-- Insert sample reviews for Alice Johnson's listings (seller_id: 70000000-0000-0000-0000-000000000001)
-- Based on the dump, Alice has listings: 71000000-0000-0000-0000-000000000001 through 71000000-0000-0000-0000-000000000010

-- Review 1: 5-star review with no reply
INSERT INTO public.review (
    id, 
    listing_id, 
    reviewer_id, 
    reviewer_role, 
    recipient_id, 
    recipient_role, 
    rating, 
    comment, 
    reply, 
    replied_at, 
    revision_status, 
    revision_requested_at, 
    created_at, 
    created_by, 
    updated_at, 
    updated_by, 
    is_deleted
) VALUES (
    gen_random_uuid(),
    '71000000-0000-0000-0000-000000000001',
    '70000000-0000-0000-0000-000000000002', -- Brian Carter as buyer
    'Buyer',
    '70000000-0000-0000-0000-000000000001', -- seller_id (Alice)
    'Seller',
    5,
    'Excellent product! Fast shipping and great quality.',
    NULL,
    NULL,
    'None',
    NULL,
    NOW() - INTERVAL '5 days',
    NULL,
    NULL,
    NULL,
    false
);

-- Review 2: 2-star review with no reply (should be prioritized)
INSERT INTO public.review (
    id, 
    listing_id, 
    reviewer_id, 
    reviewer_role, 
    recipient_id, 
    recipient_role, 
    rating, 
    comment, 
    reply, 
    replied_at, 
    revision_status, 
    revision_requested_at, 
    created_at, 
    created_by, 
    updated_at, 
    updated_by, 
    is_deleted
) VALUES (
    gen_random_uuid(),
    '71000000-0000-0000-0000-000000000002',
    '70000000-0000-0000-0000-000000000003', -- Cecilia Gomez as buyer
    'Buyer',
    '70000000-0000-0000-0000-000000000001', -- seller_id (Alice)
    'Seller',
    2,
    'Product was not as described. Disappointed with the quality.',
    NULL,
    NULL,
    'None',
    NULL,
    NOW() - INTERVAL '3 days',
    NULL,
    NULL,
    NULL,
    false
);

-- Review 3: 1-star review with no reply (should be prioritized)
INSERT INTO public.review (
    id, 
    listing_id, 
    reviewer_id, 
    reviewer_role, 
    recipient_id, 
    recipient_role, 
    rating, 
    comment, 
    reply, 
    replied_at, 
    revision_status, 
    revision_requested_at, 
    created_at, 
    created_by, 
    updated_at, 
    updated_by, 
    is_deleted
) VALUES (
    gen_random_uuid(),
    '71000000-0000-0000-0000-000000000003',
    '038a4659-83c4-456d-9bf9-e2036a53ad6c', -- Vu Truong Giang as buyer
    'Buyer',
    '70000000-0000-0000-0000-000000000001', -- seller_id (Alice)
    'Seller',
    1,
    'Terrible experience. Item never arrived and seller was unresponsive.',
    NULL,
    NULL,
    'None',
    NULL,
    NOW() - INTERVAL '7 days',
    NULL,
    NULL,
    NULL,
    false
);

-- Review 4: 4-star review with reply
INSERT INTO public.review (
    id, 
    listing_id, 
    reviewer_id, 
    reviewer_role, 
    recipient_id, 
    recipient_role, 
    rating, 
    comment, 
    reply, 
    replied_at, 
    revision_status, 
    revision_requested_at, 
    created_at, 
    created_by, 
    updated_at, 
    updated_by, 
    is_deleted
) VALUES (
    gen_random_uuid(),
    '71000000-0000-0000-0000-000000000004',
    'e167368a-7042-45a4-8f1f-badba56b6b63', -- Bui Hoang Viet as buyer
    'Buyer',
    '70000000-0000-0000-0000-000000000001', -- seller_id (Alice)
    'Seller',
    4,
    'Good product overall, but shipping took longer than expected.',
    'Thank you for your feedback! We apologize for the shipping delay and will work to improve our delivery times.',
    NOW() - INTERVAL '1 day',
    'None',
    NULL,
    NOW() - INTERVAL '2 days',
    NULL,
    NULL,
    NULL,
    false
);

-- Review 5: 3-star review with reply
INSERT INTO public.review (
    id, 
    listing_id, 
    reviewer_id, 
    reviewer_role, 
    recipient_id, 
    recipient_role, 
    rating, 
    comment, 
    reply, 
    replied_at, 
    revision_status, 
    revision_requested_at, 
    created_at, 
    created_by, 
    updated_at, 
    updated_by, 
    is_deleted
) VALUES (
    gen_random_uuid(),
    '71000000-0000-0000-0000-000000000005',
    '741eb167-fb61-4b47-a35a-90720c4949f7', -- Test User as buyer
    'Buyer',
    '70000000-0000-0000-0000-000000000001', -- seller_id (Alice)
    'Seller',
    3,
    'Average product. Works as expected but nothing special.',
    'We appreciate your honest review and will continue to improve our products.',
    NOW() - INTERVAL '2 hours',
    'None',
    NULL,
    NOW() - INTERVAL '4 days',
    NULL,
    NULL,
    NULL,
    false
);

-- Review 6: 5-star review with reply
INSERT INTO public.review (
    id, 
    listing_id, 
    reviewer_id, 
    reviewer_role, 
    recipient_id, 
    recipient_role, 
    rating, 
    comment, 
    reply, 
    replied_at, 
    revision_status, 
    revision_requested_at, 
    created_at, 
    created_by, 
    updated_at, 
    updated_by, 
    is_deleted
) VALUES (
    gen_random_uuid(),
    '71000000-0000-0000-0000-000000000006',
    'c483385e-f1b9-40b4-9b95-e18f45e06a82', -- Nguyen Duc Thinh as buyer
    'Buyer',
    '70000000-0000-0000-0000-000000000001', -- seller_id (Alice)
    'Seller',
    5,
    'Amazing quality and fast delivery! Highly recommend this seller.',
    'Thank you so much for your kind words! We really appreciate your business.',
    NOW() - INTERVAL '3 hours',
    'None',
    NULL,
    NOW() - INTERVAL '6 days',
    NULL,
    NULL,
    NULL,
    false
);

-- Review 7: 2-star review with no reply (should be prioritized)
INSERT INTO public.review (
    id, 
    listing_id, 
    reviewer_id, 
    reviewer_role, 
    recipient_id, 
    recipient_role, 
    rating, 
    comment, 
    reply, 
    replied_at, 
    revision_status, 
    revision_requested_at, 
    created_at, 
    created_by, 
    updated_at, 
    updated_by, 
    is_deleted
) VALUES (
    gen_random_uuid(),
    '71000000-0000-0000-0000-000000000007',
    '602c5ad1-90b9-4a63-8fcb-51e2d9c6ce2c', -- Do Huu Hoa as buyer
    'Buyer',
    '70000000-0000-0000-0000-000000000001', -- seller_id (Alice)
    'Seller',
    2,
    'Item had some defects. Not satisfied with the purchase.',
    NULL,
    NULL,
    'None',
    NULL,
    NOW() - INTERVAL '1 day',
    NULL,
    NULL,
    NULL,
    false
);

-- Review 8: 4-star review with no reply
INSERT INTO public.review (
    id, 
    listing_id, 
    reviewer_id, 
    reviewer_role, 
    recipient_id, 
    recipient_role, 
    rating, 
    comment, 
    reply, 
    replied_at, 
    revision_status, 
    revision_requested_at, 
    created_at, 
    created_by, 
    updated_at, 
    updated_by, 
    is_deleted
) VALUES (
    gen_random_uuid(),
    '71000000-0000-0000-0000-000000000008',
    'e8f83430-5d4d-4ac2-bb82-46797049535e', -- Le Ngoc Hai as buyer
    'Buyer',
    '70000000-0000-0000-0000-000000000001', -- seller_id (Alice)
    'Seller',
    4,
    'Good value for money. Would buy again.',
    NULL,
    NULL,
    'None',
    NULL,
    NOW() - INTERVAL '8 hours',
    NULL,
    NULL,
    NULL,
    false
);

-- Note: The reviews will be automatically sorted with priority given to:
-- 1. Rating <= 3 AND reply IS NULL (Reviews 2, 3, 7 should appear first)
-- 2. Then by created_at DESC (most recent first)

-- Expected sorting order when calling GET /api/seller/reviews:
-- 1. Review 7 (2-star, no reply, 1 day ago) - PRIORITY
-- 2. Review 2 (2-star, no reply, 3 days ago) - PRIORITY  
-- 3. Review 3 (1-star, no reply, 7 days ago) - PRIORITY
-- 4. Review 8 (4-star, no reply, 8 hours ago)
-- 5. Review 1 (5-star, no reply, 5 days ago)
-- 6. Review 4 (4-star, replied, 2 days ago)
-- 7. Review 5 (3-star, replied, 4 days ago)
-- 8. Review 6 (5-star, replied, 6 days ago)