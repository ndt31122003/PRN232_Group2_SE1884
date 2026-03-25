using Microsoft.Extensions.DependencyInjection;
using PRN232_EbayClone.Application.Behaviors;
using PRN232_EbayClone.Application.Disputes.Services;
using PRN232_EbayClone.Application.Coupons;
using PRN232_EbayClone.Application.Listings.Inventory.Services;
using PRN232_EbayClone.Application.OrderDiscounts.Services;
using PRN232_EbayClone.Application.SaleEvents.Services;
using System.Reflection;

namespace PRN232_EbayClone.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(RequestLoggingPipelineBahavior<,>));
        });

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationPipelineBehavior<,>));

        // Register dispute state machine
        services.AddScoped<IDisputeStateMachine, DisputeStateMachine>();
        // Register application services
        services.AddScoped<ICouponCalculationService, CouponCalculationService>();
        services.AddScoped<ISaleEventPriceCalculator, SaleEventPriceCalculator>();
        services.AddScoped<ISaleEventEligibilityValidator, SaleEventEligibilityValidator>();
        services.AddScoped<IDiscountPriorityService, DiscountPriorityService>();
        services.AddScoped<IPriceIncreaseValidator, PriceIncreaseValidator>();
        services.AddScoped<IInventoryLowStockNotifier, InventoryLowStockNotifier>();

        return services;
    }
}
