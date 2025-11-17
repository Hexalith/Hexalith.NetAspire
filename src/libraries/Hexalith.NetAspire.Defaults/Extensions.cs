// <copyright file="Extensions.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Microsoft.Extensions.Hosting;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

/// <summary>
/// Provides extension methods for configuring common Aspire services, including service discovery, resilience, health
/// checks, and OpenTelemetry, in .NET applications.
/// </summary>
/// <remarks>These extension methods are intended to be used during application startup to apply recommended
/// defaults for distributed service scenarios. By referencing this class in each service project, you can ensure
/// consistent configuration of health checks, telemetry, and service discovery across your solution. For more
/// information on using these defaults, see https://aka.ms/dotnet/aspire/service-defaults.</remarks>
public static class Extensions
{
    private const string AlivenessEndpointPath = "/alive";
    private const string HealthEndpointPath = "/health";

    /// <summary>
    /// Adds default health checks to the application, including a liveness check.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder.</param>
    /// <returns>The updated host application builder.</returns>
    public static TBuilder AddDefaultHealthChecks<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        _ = builder.Services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]); // Add a default liveness check to ensure app is responsive

        return builder;
    }

    /// <summary>
    /// Adds a set of default services to the application, including OpenTelemetry, health checks,
    /// and service discovery.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder.</param>
    /// <returns>The updated host application builder.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "Ignore")]
    public static TBuilder AddServiceDefaults<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        _ = builder.ConfigureOpenTelemetry();

        _ = builder.AddDefaultHealthChecks();

        _ = builder.Services.AddServiceDiscovery();

        _ = builder.Services.ConfigureHttpClientDefaults(http =>
        {
            // Turn on resilience by default
            _ = http.AddStandardResilienceHandler();

            // Turn on service discovery by default
            _ = http.AddServiceDiscovery();
        });

        // Uncomment the following to restrict the allowed schemes for service discovery.
        // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
        // {
        //     options.AllowedSchemes = ["https"];
        // });
        return builder;
    }

    /// <summary>
    /// Configures OpenTelemetry for logging, metrics, and tracing in the application.
    /// </summary>
    /// <typeparam name="TBuilder">The type of the host application builder.</typeparam>
    /// <param name="builder">The host application builder.</param>
    /// <returns>The updated host application builder.</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1005:Single line comments should begin with single space", Justification = "Ignore")]
    public static TBuilder ConfigureOpenTelemetry<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        _ = builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        _ = builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics => _ = metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation())
            .WithTracing(tracing => _ = tracing.AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation(tracing =>

                        // Exclude health check requests from tracing
                        tracing.Filter = context =>
                            !context.Request.Path.StartsWithSegments(HealthEndpointPath, StringComparison.OrdinalIgnoreCase)
                            && !context.Request.Path.StartsWithSegments(AlivenessEndpointPath, StringComparison.OrdinalIgnoreCase)
                    )

                    // Uncomment the following line to enable gRPC instrumentation (requires the OpenTelemetry.Instrumentation.GrpcNetClient package)
                    //.AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation());

        _ = builder.AddOpenTelemetryExporters();

        return builder;
    }

    /// <summary>
    /// Maps default health check endpoints to the application, including readiness and liveness checks.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The updated web application.</returns>
    public static WebApplication MapDefaultEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        // Adding health checks endpoints to applications in non-development environments has security implications.
        // See https://aka.ms/dotnet/aspire/healthchecks for details before enabling these endpoints in non-development environments.
        if (app.Environment.IsDevelopment())
        {
            // All health checks must pass for app to be considered ready to accept traffic after starting
            _ = app.MapHealthChecks(HealthEndpointPath);

            // Only health checks tagged with the "live" tag must pass for app to be considered alive
            _ = app.MapHealthChecks(AlivenessEndpointPath, new HealthCheckOptions
            {
                Predicate = r => r.Tags.Contains("live"),
            });
        }

        return app;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "Ignore")]
    private static TBuilder AddOpenTelemetryExporters<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        bool useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            _ = builder.Services.AddOpenTelemetry().UseOtlpExporter();
        }

        // Uncomment the following lines to enable the Azure Monitor exporter (requires the Azure.Monitor.OpenTelemetry.AspNetCore package)
        // if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
        // {
        //    builder.Services.AddOpenTelemetry()
        //       .UseAzureMonitor();
        // }
        return builder;
    }
}