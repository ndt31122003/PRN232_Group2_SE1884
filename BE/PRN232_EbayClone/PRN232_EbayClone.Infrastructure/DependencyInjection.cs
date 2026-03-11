using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Application.Abstractions.Identity;
using PRN232_EbayClone.Application.Abstractions.Mail;
using PRN232_EbayClone.Application.Abstractions.Realtime;
using PRN232_EbayClone.Application.Abstractions.Shipping;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Reports.Downloads.Abstractions;
using PRN232_EbayClone.Application.Research.Abstractions;
using PRN232_EbayClone.Domain.Roles.Enums;
using PRN232_EbayClone.Domain.Shared.Constants;
using PRN232_EbayClone.Domain.Users.Services;
using PRN232_EbayClone.Infrastructure.BackgroundJobs;
using PRN232_EbayClone.Infrastructure.FileStorage;
using PRN232_EbayClone.Infrastructure.Identity;
using PRN232_EbayClone.Infrastructure.Mail;
using PRN232_EbayClone.Infrastructure.Outbox;
using PRN232_EbayClone.Infrastructure.Persistence;
using PRN232_EbayClone.Infrastructure.Persistence.Interceptors;
using PRN232_EbayClone.Infrastructure.Persistence.Repositories;
using PRN232_EbayClone.Infrastructure.Realtime;
using PRN232_EbayClone.Infrastructure.Reports;
using PRN232_EbayClone.Infrastructure.Research;
using PRN232_EbayClone.Infrastructure.Services;
using PRN232_EbayClone.Infrastructure.Services.ShippingProvider;
using PRN232_EbayClone.Infrastructure.Sms;
using PRN232_EbayClone.Application.Abstractions.Sms;
using Quartz;
using Quartz.Simpl;
using Serilog;
using StackExchange.Redis;
using System.Text;

namespace PRN232_EbayClone.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddDatabaseServices(configuration)
            .AddIdentityServices(configuration)
            .AddEmailServices(configuration)
            .AddSmsServices(configuration)
            .AddDomainServices()
            .AddBackgroundJobsServices(configuration)
            .AddInfrastructureHealthChecks()
            .AddFileManagerServices(configuration)
            .AddRedis(configuration)
            .AddSharedDataProtection()
            .AddShippingServices()
            .AddHub(configuration);

        services.AddScoped<ISourcingInsightsRepository, SourcingInsightsRepository>();

        return services;
    }

    private static IServiceCollection AddDatabaseServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")!;

        services.AddScoped<ISaveChangesInterceptor, SoftDeleteSaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, AuditSaveChangesInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, ConvertDomainEventsToOutboxMessageInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(connectionString)
                   .UseSnakeCaseNamingConvention()
                   .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging();
        });
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        services.AddScoped<IStoredProcedureExecutor, DapperStoredProcedureExecutor>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IOtpRepository, OtpRepository>();
        services.AddScoped<IFileMetadataRepository, FileMetadataRepository>();
        services.AddScoped<IListingRepository, ListingRepository>();
        services.AddScoped<IListingTemplateRepository, ListingTemplateRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IPerformanceRepository, PerformanceRepository>();
services.AddScoped<ICouponRepository, CouponRepository>();
        services.AddScoped<ICouponTypeRepository, CouponTypeRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<ISaleEventRepository, SaleEventRepository>();
        services.AddScoped<IShippingLabelRepository, ShippingLabelRepository>();
        services.AddScoped<ICancellationRequestRepository, CancellationRequestRepository>();
        services.AddScoped<IReturnRequestRepository, ReturnRequestRepository>();
        services.AddScoped<IShippingServiceRepository, ShippingServiceRepository>();
        services.AddScoped<IReportDownloadRepository, ReportDownloadRepository>();
        services.AddScoped<IReportScheduleRepository, ReportScheduleRepository>();
        services.AddScoped<IReportDownloadGenerator, ReportDownloadGenerator>();
        services.AddScoped<IStoreRepository, StoreRepository>();
        services.AddScoped<IShippingPolicyRepository, ShippingPolicyRepository>();
        services.AddScoped<IReturnPolicyRepository, ReturnPolicyRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IDisputeRepository, DisputeRepository>();
        services.AddScoped<ISellerPreferenceRepository, SellerPreferenceRepository>();


        return services;
    }

    private static IServiceCollection AddIdentityServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtConfiguration>(configuration.GetSection("Jwt"));

        services.AddSingleton<ITokenProvider, JwtTokenProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IOtpGenerator, OtpGenerator>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddCookie(IdentityConstants.ExternalScheme, options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
        {
            googleOptions.ClientId = configuration["Google:ClientId"]!;
            googleOptions.ClientSecret = configuration["Google:ClientSecret"]!;
            googleOptions.CallbackPath = new PathString("/api/identity/google/callback");
            googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
            googleOptions.SaveTokens = true;
            googleOptions.Scope.Add("email");
            googleOptions.Scope.Add("profile");
            googleOptions.CorrelationCookie.SameSite = SameSiteMode.Lax;
            googleOptions.CorrelationCookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]!)
                ),
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options =>
        {
            foreach (var permission in Enum.GetNames(typeof(Permission)))
            {
                options.AddPolicy(permission, policy =>
                    policy.RequireClaim(CustomClaimTypes.Permission, permission));
            }
        });

        return services;
    }

    private static IServiceCollection AddEmailServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<EmailConfiguration>(configuration.GetSection("Email"));
        services.Configure<EmailTemplateConfiguration>(configuration.GetSection("EmailTemplate"));
        services.AddTransient<ITemplateRenderer, HtmlTemplateRenderer>();
        services.AddTransient<IEmailSender, EmailSender>();
        return services;
    }

    private static IServiceCollection AddSmsServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var smsProvider = configuration.GetValue<string>("Sms:Provider")?.ToLowerInvariant();

        switch (smsProvider)
        {
            case "mocean":
                services.Configure<MoceanConfiguration>(configuration.GetSection("Sms:Mocean"));
                services.AddTransient<ISmsSender, MoceanSmsSender>();
                break;

            case "twilio":
                services.Configure<TwilioConfiguration>(configuration.GetSection("Twilio"));
                services.AddTransient<ISmsSender, TwilioSmsSender>();
                break;

            default:
                services.AddTransient<ISmsSender, DevSmsSender>();
                break;
        }

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IEmailUniquenessChecker, UserService>();

        return services;
    }

    private static IServiceCollection AddBackgroundJobsServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        var useInMemoryStore = configuration.GetValue<bool>("Quartz:UseInMemoryStore", false);

        services.AddQuartz(configure =>
        {
            configure.SchedulerName = "EbayCloneSchedulerQuartz";
            configure.SchedulerId = "AUTO";

            if (useInMemoryStore)
            {
                Log.Warning("Quartz.NET using in-memory store (jobs won't persist across restarts)");
                configure.UseInMemoryStore();
            }
            else
            {
                configure.SetProperty("quartz.serializer.type", "json");
                configure.SetProperty("quartz.jobStore.performSchemaValidation", "false");

                configure.UsePersistentStore(store =>
                {
                    store.UsePostgres(connectionString);

                    store.UseClustering(options =>
                    {
                        options.CheckinInterval = TimeSpan.FromSeconds(15);
                        options.CheckinMisfireThreshold = TimeSpan.FromSeconds(60);
                    });

                    store.UseNewtonsoftJsonSerializer();
                });
            }

            void AddSimpleJob<TJob>(string jobName, int intervalSeconds)
                where TJob : IJob
            {
                var jobKey = new JobKey(jobName);
                configure.AddJob<TJob>(jobKey)
                         .AddTrigger(trigger => trigger
                             .ForJob(jobKey)
                             .WithSimpleSchedule(schedule => schedule
                                 .WithIntervalInSeconds(intervalSeconds)
                                 .RepeatForever()));
            }

            AddSimpleJob<ProcessOutboxMessagesJob>(nameof(ProcessOutboxMessagesJob), 10);
            AddSimpleJob<ActivateListingJob>(nameof(ActivateListingJob), 60);
            AddSimpleJob<EndListingJob>(nameof(EndListingJob), 60);
            AddSimpleJob<BuyerCancellationTimeoutJob>(nameof(BuyerCancellationTimeoutJob), 3600);

            // Cron job
            var awaitingShipmentJobKey = new JobKey(nameof(AwaitingShipmentStatusUpdateJob));
            configure.AddJob<AwaitingShipmentStatusUpdateJob>(awaitingShipmentJobKey)
                     .AddTrigger(trigger => trigger
                         .ForJob(awaitingShipmentJobKey)
                         .WithIdentity($"{nameof(AwaitingShipmentStatusUpdateJob)}-trigger")
                         .WithCronSchedule("0 0 2 * * ?", cron => cron.InTimeZone(TimeZoneInfo.Utc)));

            configure.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
            options.AwaitApplicationStarted = true;
            options.StartDelay = TimeSpan.FromSeconds(3);
        });


        return services;
    }


    private static IServiceCollection AddInfrastructureHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        return services;
    }

    private static IServiceCollection AddFileManagerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<CloudinaryConfiguration>(configuration.GetSection("Cloudinary"));
        services.AddTransient<IFileManager, CloudinaryFileManager>();
        return services;
    }

    private static IServiceCollection AddShippingServices(this IServiceCollection services)
    {
        services.AddSingleton<IShippingLabelRenderer, PdfShippingLabelRenderer>();
        services.AddSingleton<IShippingProvider, MockingShippingProvider>();
        return services;
    }

    private static bool TryConnectRedis(string? connectionString, out IConnectionMultiplexer? multiplexer)
    {
        multiplexer = null;
        if (string.IsNullOrWhiteSpace(connectionString) || connectionString.Contains("your_redis"))
            return false;

        try
        {
            multiplexer = ConnectionMultiplexer.Connect(connectionString);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("Redis");

        if (TryConnectRedis(redisConnection, out var redis))
        {
            services.AddSingleton(redis!);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "EbayClone_";
            });
        }
        else
        {
            services.AddDistributedMemoryCache();
        }

        return services;
    }

    private static IServiceCollection AddSharedDataProtection(this IServiceCollection services)
    {
        var dp = services
            .AddDataProtection()
            .SetApplicationName("EbayClone");

        var hasRedis = services.Any(d => d.ServiceType == typeof(IConnectionMultiplexer));
        if (hasRedis)
        {
            dp.PersistKeysToStackExchangeRedis(() =>
            {
                var provider = services.BuildServiceProvider();
                var redisConn = provider.GetRequiredService<IConnectionMultiplexer>();
                return redisConn.GetDatabase();
            }, "DataProtection-Keys");
        }

        return services;
    }

    private static IServiceCollection AddHub(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IRealtimeNotifier, RealtimeNotifier>();

        var redisConnection = configuration.GetConnectionString("Redis");
        var signalR = services.AddSignalR();

        if (!string.IsNullOrWhiteSpace(redisConnection) && !redisConnection.Contains("your_redis"))
        {
            try
            {
                signalR.AddStackExchangeRedis(redisConnection);
            }
            catch
            {
                // SignalR will use in-memory backplane
            }
        }

        return services;
    }
}
