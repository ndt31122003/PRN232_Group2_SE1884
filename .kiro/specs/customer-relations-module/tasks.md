# Implementation Plan: Customer Relations Module

## Overview

This implementation plan breaks down the Customer Relations Module into discrete coding tasks. The module includes three main features: Review/Rating management with seller replies, Dispute/Return management with state machine workflows, and Support Ticket system for seller-to-admin communication. The implementation follows CQRS patterns using MediatR, leverages PostgreSQL with EF Core, and integrates with existing order, listing, and user management systems.

## Tasks

- [x] 1. Database schema and migrations
  - [x] 1.1 Create EF Core entity classes for all new tables
    - Create `SellerReviewReply` entity with properties matching database schema
    - Create `Dispute` entity with status enum and state machine support
    - Create `DisputeMessage`, `DisputeEvidence`, `DisputeStatusHistory` entities
    - Create `SupportTicket`, `SupportTicketAttachment`, `SupportTicketResponse` entities
    - Add audit fields (created_at, created_by, updated_at, updated_by, is_deleted) to all entities
    - _Requirements: Database Schema section_
  
  - [x] 1.2 Add DbSet properties to ApplicationDbContext
    - Add DbSet for SellerReviewReply, Dispute, DisputeMessage, DisputeEvidence, DisputeStatusHistory
    - Add DbSet for SupportTicket, SupportTicketAttachment, SupportTicketResponse
    - Configure entity relationships and constraints using Fluent API
    - _Requirements: Database Schema section_
  
  - [x] 1.3 Create and apply EF Core migration
    - Generate migration script for all new tables
    - Add star_rating column to existing order_buyer_feedback table
    - Create indexes for performance optimization
    - Apply migration to database
    - _Requirements: Migration Strategy section_

- [x] 2. Domain layer - Enums and value objects
  - [x] 2.1 Create dispute status and type enums
    - Create `DisputeStatus` enum (OPEN, WAITING_SELLER, WAITING_BUYER, ESCALATED, RESOLVED, CLOSED)
    - Create `DisputeType` enum (RETURN_REQUEST, REFUND_REQUEST, ITEM_NOT_RECEIVED, ITEM_NOT_AS_DESCRIBED, OTHER)
    - Create `DisputeResolutionType` enum for resolution tracking
    - _Requirements: Database Schema - dispute table_
  
  - [x] 2.2 Create support ticket status and priority enums
    - Create `SupportTicketStatus` enum (OPEN, PENDING, IN_PROGRESS, WAITING_SELLER, RESOLVED, CLOSED)
    - Create `SupportTicketPriority` enum (LOW, NORMAL, HIGH, URGENT)
    - Create `SupportTicketCategory` enum for categorization
    - _Requirements: Database Schema - support_ticket table_
  
  - [x] 2.3 Implement dispute state machine class
    - Create `DisputeStateMachine` class with allowed transitions dictionary
    - Implement `IsTransitionAllowed(fromStatus, toStatus)` method
    - Add validation logic for role-based transitions
    - _Requirements: State Machine Flows - Dispute State Machine_
  
  - [x] 2.4 Implement support ticket state machine class
    - Create `SupportTicketStateMachine` class with allowed transitions dictionary
    - Implement `IsTransitionAllowed(fromStatus, toStatus)` method
    - Add auto-close logic validation
    - _Requirements: State Machine Flows - Support Ticket State Machine_

- [ ] 3. Domain layer - Domain events
  - [x] 3.1 Create review domain events
    - Create `ReviewRepliedDomainEvent` with review and seller details
    - Create `ReviewReplyEditedDomainEvent` for reply modifications
    - _Requirements: CQRS Implementation Structure_
  
  - [x] 3.2 Create dispute domain events
    - Create `DisputeCreatedDomainEvent` with dispute details
    - Create `DisputeStatusChangedDomainEvent` with status transition info
    - Create `DisputeEvidenceUploadedDomainEvent` for evidence tracking
    - Create `DisputeEscalatedDomainEvent` for admin notifications
    - _Requirements: CQRS Implementation Structure_
  
  - [ ] 3.3 Create support ticket domain events
    - Create `SupportTicketCreatedDomainEvent` with ticket details
    - Create `SupportTicketResponseAddedDomainEvent` for conversation tracking
    - Create `SupportTicketStatusChangedDomainEvent` for status updates
    - _Requirements: CQRS Implementation Structure_

- [ ] 4. Application layer - Review management DTOs
  - [ ] 4.1 Create review query DTOs
    - Create `ReviewFilterDto` with filtering properties (seller, date range, rating, hasReply)
    - Create `ReviewListResponse` with pagination and average rating
    - Create `ReviewDto` with review and reply information
    - Create `ReviewDetailDto` with complete review context
    - Create `SellerReplyDto` for reply information
    - _Requirements: API Design - Reviews API_
  
  - [ ] 4.2 Create review command request models
    - Create `ReplyToReviewRequest` with reply text validation
    - Create `EditReviewReplyRequest` for reply modifications
    - _Requirements: API Design - Reviews API_

- [ ] 5. Application layer - Review commands and queries
  - [ ] 5.1 Implement GetReviewsQuery and handler
    - Create `GetReviewsQuery` record with ReviewFilterDto parameter
    - Implement query handler with filtering, pagination, and sorting
    - Join order_buyer_feedback with seller_review_reply (LEFT JOIN)
    - Calculate average rating for seller
    - Return ReviewListResponse with paginated results
    - _Requirements: API Design - GET /api/reviews_
  
  - [ ] 5.2 Implement GetReviewByIdQuery and handler
    - Create `GetReviewByIdQuery` record with reviewId parameter
    - Implement query handler with authorization check (seller ownership)
    - Include order, listing, buyer, and reply details
    - Return ReviewDetailDto with complete context
    - _Requirements: API Design - GET /api/reviews/{reviewId}_
  
  - [ ] 5.3 Implement ReplyToReviewCommand and handler
    - Create `ReplyToReviewCommand` record with reviewId and reply text
    - Implement command handler with validation (ownership, reply length, existing reply check)
    - Create or update SellerReviewReply entity
    - Raise ReviewRepliedDomainEvent for notifications
    - _Requirements: API Design - POST /api/reviews/{reviewId}/reply_
  
  - [ ]* 5.4 Add FluentValidation validators for review commands
    - Create `ReplyToReviewCommandValidator` (reply required, max 500 chars)
    - Validate reply is not empty or whitespace only
    - _Requirements: API Design - Reviews API validation rules_

- [ ] 6. Application layer - Dispute management DTOs
  - [ ] 6.1 Create dispute query DTOs
    - Create `DisputeFilterDto` with filtering properties (seller, status, type, deadline)
    - Create `DisputeListResponse` with pagination and dashboard metrics
    - Create `DisputeSummaryDto` for list view
    - Create `DisputeDetailDto` with complete dispute context
    - Create `DisputeMessageDto`, `DisputeEvidenceDto`, `DisputeStatusHistoryDto`
    - _Requirements: API Design - Disputes API_
  
  - [ ] 6.2 Create dispute command request models
    - Create `CreateDisputeRequest` with order, listing, type, and reason
    - Create `AcceptRefundRequest` with refund amount and message
    - Create `ProvideEvidenceRequest` for file uploads
    - Create `EscalateDisputeRequest` with escalation reason
    - Create `AddDisputeMessageRequest` for conversation
    - _Requirements: API Design - Disputes API_

- [x] 7. Application layer - Dispute commands and queries
  - [x] 7.1 Implement GetDisputesQuery and handler
    - Create `GetDisputesQuery` record with DisputeFilterDto parameter
    - Implement query handler with filtering, pagination, and sorting
    - Calculate WaitingSellerCount and DeadlineApproachingCount metrics
    - Prioritize urgent cases (deadline approaching) in default sort
    - Return DisputeListResponse with paginated results
    - _Requirements: API Design - GET /api/disputes_
  
  - [x] 7.2 Implement GetDisputeByIdQuery and handler
    - Create `GetDisputeByIdQuery` record with disputeId parameter
    - Implement query handler with authorization check (seller or buyer)
    - Include order, listing, messages, evidence, and status history
    - Return DisputeDetailDto with complete context
    - _Requirements: API Design - GET /api/disputes/{disputeId}_
  
  - [x] 7.3 Implement CreateDisputeCommand and handler
    - Create `CreateDisputeCommand` record with order, listing, type, and reason
    - Implement command handler with validation (order exists, belongs to buyer)
    - Create Dispute entity with status OPEN
    - Automatically transition to WAITING_SELLER and set deadline (3 business days)
    - Create initial DisputeStatusHistory record
    - Raise DisputeCreatedDomainEvent for notifications
    - _Requirements: API Design - POST /api/disputes_
  
  - [x] 7.4 Implement AcceptRefundCommand and handler
    - Create `AcceptRefundCommand` record with disputeId, refund amount, and message
    - Implement command handler with validation (seller ownership, state transition, refund amount)
    - Validate state transition using DisputeStateMachine (WAITING_SELLER → RESOLVED)
    - Update dispute status to RESOLVED with refund details
    - Create DisputeStatusHistory record
    - Raise DisputeStatusChangedDomainEvent for notifications
    - _Requirements: API Design - POST /api/disputes/{disputeId}/accept-refund_
  
  - [x] 7.5 Implement ProvideEvidenceCommand and handler
    - Create `ProvideEvidenceCommand` record with disputeId, files, and description
    - Implement command handler with file validation (count, size, type)
    - Upload files to storage service and get URLs
    - Create DisputeEvidence records for each file
    - Update dispute status to WAITING_BUYER
    - Create DisputeStatusHistory record
    - Raise DisputeEvidenceUploadedDomainEvent for notifications
    - _Requirements: API Design - POST /api/disputes/{disputeId}/provide-evidence_
  
  - [x] 7.6 Implement EscalateDisputeCommand and handler
    - Create `EscalateDisputeCommand` record with disputeId and reason
    - Implement command handler with validation (seller ownership, state transition)
    - Validate state transition using DisputeStateMachine (current → ESCALATED)
    - Update dispute status to ESCALATED with escalated_at timestamp
    - Create DisputeStatusHistory record with reason
    - Raise DisputeEscalatedDomainEvent for admin notifications
    - _Requirements: API Design - POST /api/disputes/{disputeId}/escalate_
  
  - [x] 7.7 Implement AddDisputeMessageCommand and handler
    - Create `AddDisputeMessageCommand` record with disputeId and message
    - Implement command handler with validation (user is buyer or seller, dispute not closed)
    - Create DisputeMessage entity with sender role
    - Update dispute updated_at timestamp
    - Return DisputeMessageDto with message details
    - _Requirements: API Design - POST /api/disputes/{disputeId}/messages_
  
  - [ ]* 7.8 Add FluentValidation validators for dispute commands
    - Create validators for CreateDisputeCommand, AcceptRefundCommand, ProvideEvidenceCommand
    - Create validators for EscalateDisputeCommand, AddDisputeMessageCommand
    - Validate required fields, max lengths, enum values, file constraints
    - _Requirements: API Design - Disputes API validation rules_

- [ ] 8. Application layer - Support ticket management DTOs
  - [ ] 8.1 Create support ticket query DTOs
    - Create `SupportTicketFilterDto` with filtering properties (seller, status, category, priority)
    - Create `SupportTicketListResponse` with pagination and dashboard metrics
    - Create `SupportTicketSummaryDto` for list view
    - Create `SupportTicketDetailDto` with complete ticket context
    - Create `SupportTicketAttachmentDto`, `SupportTicketResponseDto`
    - _Requirements: API Design - Support Tickets API_
  
  - [ ] 8.2 Create support ticket command request models
    - Create `CreateSupportTicketRequest` with category, subject, message, priority, attachments
    - Create `AddTicketResponseRequest` for conversation
    - Create `UpdateTicketStatusRequest` for status changes
    - _Requirements: API Design - Support Tickets API_

- [x] 9. Application layer - Support ticket commands and queries
  - [x] 9.1 Implement GetSupportTicketsQuery and handler
    - Create `GetSupportTicketsQuery` record with SupportTicketFilterDto parameter
    - Implement query handler with filtering, pagination, and sorting
    - Calculate OpenCount and PendingCount metrics
    - Return SupportTicketListResponse with paginated results
    - _Requirements: API Design - GET /api/support-tickets_
  
  - [x] 9.2 Implement GetSupportTicketByIdQuery and handler
    - Create `GetSupportTicketByIdQuery` record with ticketId parameter
    - Implement query handler with authorization check (seller ownership or admin)
    - Include attachments and responses (filter out internal notes for sellers)
    - Return SupportTicketDetailDto with complete context
    - _Requirements: API Design - GET /api/support-tickets/{ticketId}_
  
  - [x] 9.3 Implement CreateSupportTicketCommand and handler
    - Create `CreateSupportTicketCommand` record with category, subject, message, priority, attachments
    - Implement command handler with validation (required fields, file constraints)
    - Generate unique ticket number (format: "TKT-YYYY-NNNNN")
    - Create SupportTicket entity with status OPEN
    - Upload attachments to storage and create SupportTicketAttachment records
    - Raise SupportTicketCreatedDomainEvent for notifications
    - Return ticket ID and ticket number
    - _Requirements: API Design - POST /api/support-tickets_
  
  - [x] 9.4 Implement AddTicketResponseCommand and handler
    - Create `AddTicketResponseCommand` record with ticketId and message
    - Implement command handler with validation (seller ownership, ticket not closed)
    - Create SupportTicketResponse entity with responder role
    - Update ticket updated_at timestamp
    - If ticket status is WAITING_SELLER, update to PENDING
    - Return SupportTicketResponseDto with response details
    - _Requirements: API Design - POST /api/support-tickets/{ticketId}/responses_
  
  - [ ]* 9.5 Add FluentValidation validators for support ticket commands
    - Create validators for CreateSupportTicketCommand, AddTicketResponseCommand
    - Validate required fields, max lengths, enum values, file constraints
    - _Requirements: API Design - Support Tickets API validation rules_

- [x] 10. API layer - Controllers and endpoints
  - [x] 10.1 Update ReviewsController with remaining endpoints
    - Add GET /api/reviews/{reviewId} endpoint with GetReviewByIdQuery
    - Add PUT /api/reviews/{reviewId}/reply endpoint for editing replies
    - Add authorization attributes and rate limiting
    - _Requirements: API Design - Reviews API_
  
  - [x] 10.2 Update DisputesController with remaining endpoints
    - Add POST /api/disputes/{disputeId}/accept-refund endpoint
    - Add POST /api/disputes/{disputeId}/provide-evidence endpoint with file upload
    - Add GET /api/disputes/{disputeId}/messages endpoint
    - Add GET /api/disputes/{disputeId}/evidence endpoint
    - Add authorization attributes and rate limiting
    - _Requirements: API Design - Disputes API_
  
  - [x] 10.3 Create SupportTicketsController with all endpoints
    - Create controller inheriting from ApiController
    - Add GET /api/support-tickets endpoint with filtering
    - Add GET /api/support-tickets/{ticketId} endpoint
    - Add POST /api/support-tickets endpoint with file upload support
    - Add POST /api/support-tickets/{ticketId}/responses endpoint
    - Add GET /api/support-tickets/{ticketId}/responses endpoint
    - Add authorization attributes (Seller role) and rate limiting
    - _Requirements: API Design - Support Tickets API_

- [x] 11. Infrastructure - File storage service
  - [x] 11.1 Implement file upload service interface and implementation
    - Create `IFileStorageService` interface with UploadAsync, DeleteAsync, GetUrlAsync methods
    - Implement file storage service using Azure Blob Storage or AWS S3
    - Add file validation (size, type, extension) in service layer
    - Implement streaming upload to avoid memory buffering
    - _Requirements: Integration Points - File Storage Service_
  
  - [x] 11.2 Add file security and validation
    - Implement `FileUploadValidator` class with allowed extensions and MIME types
    - Add file size validation (max 10MB per file)
    - Add virus scanning integration (optional)
    - Generate unique file names to prevent collisions
    - _Requirements: Security Considerations - File Upload Security_

- [ ] 12. Infrastructure - Notification services
  - [ ] 12.1 Create email notification templates
    - Create review reply email template for buyers
    - Create dispute status change email templates (for each status)
    - Create support ticket confirmation email template for sellers
    - Create support ticket admin notification email template
    - _Requirements: Notification Strategy - Email Notifications_
  
  - [ ] 12.2 Implement email notification service
    - Create email service implementation using SendGrid or MailKit
    - Implement methods for each notification type (review reply, dispute status, ticket created)
    - Add email template rendering with dynamic data
    - Add error handling and retry logic
    - _Requirements: Notification Strategy - Email Notifications_
  
  - [ ] 12.3 Implement real-time notification service
    - Create notification hub using SignalR
    - Implement SendToUser method for targeted notifications
    - Create notification entity for persistence
    - Add push notification support for mobile devices (optional)
    - _Requirements: Notification Strategy - Real-Time Notifications_
  
  - [ ] 12.4 Implement domain event handlers for notifications
    - Create ReviewRepliedDomainEventHandler to send buyer notifications
    - Create DisputeStatusChangedDomainEventHandler to send buyer/seller/admin notifications
    - Create DisputeEscalatedDomainEventHandler for urgent admin notifications
    - Create SupportTicketCreatedDomainEventHandler to send seller confirmation and admin notification
    - _Requirements: Notification Strategy - Notification Triggers_

- [ ] 13. Infrastructure - Background jobs
  - [ ] 13.1 Implement dispute deadline monitoring job
    - Create `CheckDisputeDeadlinesJob` scheduled to run every hour
    - Query disputes with status WAITING_SELLER and deadline_at set
    - Set is_deadline_approaching flag for disputes within 24 hours of deadline
    - Send urgent notifications to sellers for approaching deadlines
    - Auto-escalate disputes where deadline has passed
    - _Requirements: Performance Considerations - Background Jobs_
  
  - [ ] 13.2 Implement support ticket auto-close job
    - Create `AutoCloseSupportTicketsJob` scheduled to run daily
    - Close tickets with status WAITING_SELLER and no response for 7 days
    - Close tickets with status RESOLVED for 48 hours
    - Create status history records for auto-closed tickets
    - _Requirements: Performance Considerations - Background Jobs_
  
  - [ ] 13.3 Configure Hangfire or Quartz.NET scheduling
    - Add Hangfire or Quartz.NET NuGet packages
    - Configure job scheduling in DependencyInjection
    - Set up recurring jobs with appropriate intervals
    - Add job monitoring and logging
    - _Requirements: Dependencies - External Libraries_

- [ ] 14. Checkpoint - Core functionality complete
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 15. Authorization and security
  - [ ] 15.1 Implement authorization handlers
    - Create authorization handler for review reply (seller ownership validation)
    - Create authorization handler for dispute access (seller or buyer validation)
    - Create authorization handler for support ticket access (seller ownership or admin)
    - Add role-based authorization for admin-only operations
    - _Requirements: Security Considerations - Authorization Rules_
  
  - [ ] 15.2 Add rate limiting middleware
    - Configure rate limiting for review endpoints (20 replies per hour)
    - Configure rate limiting for dispute endpoints (10 disputes per day, 50 messages per hour)
    - Configure rate limiting for support ticket endpoints (5 tickets per day, 20 responses per hour)
    - Add rate limit attributes to controller actions
    - _Requirements: Security Considerations - Rate Limiting_
  
  - [ ] 15.3 Implement audit logging
    - Create AuditLog entity for sensitive operations
    - Add audit logging for dispute status changes
    - Add audit logging for support ticket access and modifications
    - Log user ID, role, IP address, user agent, and timestamp
    - _Requirements: Security Considerations - Audit Logging_

- [ ] 16. Performance optimization
  - [ ] 16.1 Add database indexes
    - Create composite indexes for common query patterns (seller_id + status)
    - Create indexes for deadline_approaching queries
    - Create indexes for message and response queries with sorting
    - Verify index usage with query execution plans
    - _Requirements: Performance Considerations - Database Indexing Strategy_
  
  - [ ] 16.2 Implement caching strategy
    - Add caching for seller average rating (TTL: 1 hour)
    - Add caching for dispute counts (TTL: 5 minutes)
    - Add caching for ticket counts (TTL: 5 minutes)
    - Implement cache invalidation on relevant domain events
    - _Requirements: Performance Considerations - Caching Strategy_

- [ ] 17. Error handling and validation
  - [ ] 17.1 Create custom error types
    - Create ReviewErrors class with NotFound, Unauthorized, ReplyExists, InvalidReply
    - Create DisputeErrors class with InvalidTransition, DeadlineExceeded, InvalidRefundAmount, FileUploadFailed
    - Create SupportTicketErrors class with InvalidCategory, Closed, NumberGenerationFailed
    - _Requirements: Error Handling - Error Scenarios_
  
  - [ ] 17.2 Implement global exception handler
    - Update GlobalExceptionHandler to handle custom error types
    - Return ProblemDetails RFC 7807 format for all errors
    - Add validation error formatting with multiple field errors
    - _Requirements: Error Handling - Error Response Format_

- [ ] 18. Integration and wiring
  - [ ] 18.1 Register services in DependencyInjection
    - Register file storage service
    - Register email notification service
    - Register real-time notification service
    - Register background jobs
    - Register domain event handlers
    - _Requirements: Dependencies - Internal Dependencies_
  
  - [ ] 18.2 Update database context configuration
    - Configure entity relationships and constraints
    - Add snake_case naming convention for PostgreSQL
    - Configure cascade delete behaviors
    - Apply migrations to database
    - _Requirements: Database Schema section_

- [ ] 19. Final checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional and can be skipped for faster MVP
- Each task references specific sections from the design document for traceability
- Checkpoints ensure incremental validation at major milestones
- The implementation follows CQRS patterns with MediatR for command/query handling
- All file uploads use streaming to avoid memory issues
- State machines enforce valid status transitions for disputes and support tickets
- Background jobs handle automated workflows (deadline monitoring, auto-close)
- Notifications are sent via email and real-time channels (SignalR)
