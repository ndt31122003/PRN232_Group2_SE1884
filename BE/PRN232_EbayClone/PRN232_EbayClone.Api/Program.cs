using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using System.IO;
using PRN232_EbayClone.Api;
using PRN232_EbayClone.Api.Infrastructure;
using PRN232_EbayClone.Api.Infrastructure.RateLimitConfig;
using PRN232_EbayClone.Application;
using PRN232_EbayClone.Infrastructure;
using PRN232_EbayClone.Infrastructure.Extensions;
using PRN232_EbayClone.Infrastructure.Realtime;
using PRN232_EbayClone.Infrastructure.BackgroundJobs;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web host");

    DotNetEnv.Env.Load();

    var builder = WebApplication.CreateBuilder(args);

    const string CorsPolicyName = "FrontendCors";

    builder.Services.AddCors(options =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        options.AddPolicy(CorsPolicyName, policy =>
        {
            if (allowedOrigins.Length > 0)
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }
            else
            {
                policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials();
            }
        });
    });

    builder.Host.UseSerilog((context, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration));

    builder.Services.Configure<List<RateLimitRule>>(builder.Configuration.GetSection("RedisRateLimits"));

    builder.Services
        .AddInfrastructureServices(builder.Configuration)
        .AddApplicationServices()
        .AddApiServices(builder.Configuration);

    builder.Services.AddControllers();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGenWithAuthentication();

    // ✅ Register Background Service for Monthly Seller Evaluation
    builder.Services.AddHostedService<SellerLevelEvaluationJob>();
builder.Services.AddHostedService<LowStockAlertWorker>();

    var app = builder.Build();

    var forwardedHeaderOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    };

    forwardedHeaderOptions.KnownNetworks.Clear();
    forwardedHeaderOptions.KnownProxies.Clear();

    app.UseForwardedHeaders(forwardedHeaderOptions);

    app.MapHealthChecks("/");

    app.UseExceptionHandler();

    // NOTE: UseHttpsRedirection is intentionally omitted for local dev.
    // In production, Cloudflare/Nginx handles SSL termination.
    // Enabling it locally breaks CORS preflight (OPTIONS) requests.
    if (!app.Environment.IsDevelopment())
    {
        app.UseHttpsRedirection();
    }

    app.UseMiddleware<RequestLogContextMiddleware>();

    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
        };
    });

    var storageRoot = Path.Combine(AppContext.BaseDirectory, "storage");
    if (!Directory.Exists(storageRoot))
    {
        Directory.CreateDirectory(storageRoot);
    }

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(storageRoot),
        RequestPath = "/storage",
        ServeUnknownFileTypes = true,
        DefaultContentType = "application/octet-stream"
    });

    app.UseCors(CorsPolicyName);

    app.UseRouting();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSlidingWindowRateLimiter();

    app.MapHub<NotificationHub>("/hub");

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}