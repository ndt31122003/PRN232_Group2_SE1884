using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/notifications")]
[Authorize]
public sealed class NotificationsController : ApiController
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationsController(
        ISender sender,
        INotificationRepository notificationRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork) : base(sender)
    {
        _notificationRepository = notificationRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
            return Unauthorized();

        var notifications = await _notificationRepository
            .GetByUserIdAsync(userId, pageSize, cancellationToken);

        var result = notifications.Select(n => new
        {
            n.Id,
            n.Type,
            n.Title,
            n.Message,
            n.ReferenceId,
            n.IsRead,
            n.CreatedAt
        });

        return Ok(result);
    }

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
            return Unauthorized();

        var count = await _notificationRepository.GetUnreadCountAsync(userId, cancellationToken);
        return Ok(new { count });
    }

    [HttpPost("mark-all-read")]
    public async Task<IActionResult> MarkAllRead(CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
            return Unauthorized();

        await _notificationRepository.MarkAllReadAsync(userId, cancellationToken);
        return NoContent();
    }
}
