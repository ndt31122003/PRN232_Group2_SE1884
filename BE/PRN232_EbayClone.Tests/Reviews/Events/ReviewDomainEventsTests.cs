using FluentAssertions;
using PRN232_EbayClone.Domain.Reviews.Events;
using Xunit;

namespace PRN232_EbayClone.Tests.Reviews.Events;

public sealed class ReviewDomainEventsTests
{
    [Fact]
    public void ReviewRepliedDomainEvent_ShouldBeCreatedWithCorrectProperties()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var sellerId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        var replyText = "Thank you for your feedback!";
        var repliedAt = DateTimeOffset.UtcNow;

        // Act
        var domainEvent = new ReviewRepliedDomainEvent(
            reviewId,
            sellerId,
            buyerId,
            replyText,
            repliedAt
        );

        // Assert
        domainEvent.ReviewId.Should().Be(reviewId);
        domainEvent.SellerId.Should().Be(sellerId);
        domainEvent.BuyerId.Should().Be(buyerId);
        domainEvent.ReplyText.Should().Be(replyText);
        domainEvent.RepliedAt.Should().Be(repliedAt);
        domainEvent.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ReviewReplyEditedDomainEvent_ShouldBeCreatedWithCorrectProperties()
    {
        // Arrange
        var reviewId = Guid.NewGuid();
        var sellerId = Guid.NewGuid();
        var buyerId = Guid.NewGuid();
        var newReplyText = "Updated: Thank you for your feedback!";
        var editedAt = DateTimeOffset.UtcNow;

        // Act
        var domainEvent = new ReviewReplyEditedDomainEvent(
            reviewId,
            sellerId,
            buyerId,
            newReplyText,
            editedAt
        );

        // Assert
        domainEvent.ReviewId.Should().Be(reviewId);
        domainEvent.SellerId.Should().Be(sellerId);
        domainEvent.BuyerId.Should().Be(buyerId);
        domainEvent.NewReplyText.Should().Be(newReplyText);
        domainEvent.EditedAt.Should().Be(editedAt);
        domainEvent.OccurredOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void ReviewRepliedDomainEvent_ShouldImplementIDomainEvent()
    {
        // Arrange
        var domainEvent = new ReviewRepliedDomainEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Test reply",
            DateTimeOffset.UtcNow
        );

        // Assert
        domainEvent.Should().BeAssignableTo<MediatR.INotification>();
    }

    [Fact]
    public void ReviewReplyEditedDomainEvent_ShouldImplementIDomainEvent()
    {
        // Arrange
        var domainEvent = new ReviewReplyEditedDomainEvent(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Updated reply",
            DateTimeOffset.UtcNow
        );

        // Assert
        domainEvent.Should().BeAssignableTo<MediatR.INotification>();
    }
}
