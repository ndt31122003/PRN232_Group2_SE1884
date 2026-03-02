using Microsoft.AspNetCore.Builder;
using PRN232_EbayClone.Infrastructure.RateLimitConfig;

namespace PRN232_EbayClone.Infrastructure.Extensions;

public static class SlidingWindowRateLimiterExtensions
{
    public static void UseSlidingWindowRateLimiter(this IApplicationBuilder builder)
        => builder.UseMiddleware<SlidingWindowRateLimiter>();
}
