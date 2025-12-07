// <copyright file="AspireHostHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.NetAspire.Hosting.Helpers;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Eventing;
using Aspire.Hosting.Lifecycle;

/// <summary>
/// Helper class for managing Aspire host configuration.
/// </summary>
public static class AspireHostHelper
{
    /// <summary>
    /// Adds a subscriber that sets the ASPNETCORE_FORWARDEDHEADERS_ENABLED environment variable to true
    /// for all projects in the application.
    /// </summary>
    /// <param name="builder">The distributed application builder.</param>
    /// <returns>The updated application builder.</returns>
    public static IDistributedApplicationBuilder AddForwardedHeaders([NotNull] this IDistributedApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Replaces obsolete TryAddLifecycleHook<T>()
        builder.Services.TryAddEventingSubscriber<AddForwardHeadersSubscriber>();

        return builder;
    }

    private sealed class AddForwardHeadersSubscriber : IDistributedApplicationEventingSubscriber
    {
        public Task SubscribeAsync(
            IDistributedApplicationEventing eventing,
            DistributedApplicationExecutionContext executionContext,
            CancellationToken cancellationToken)
        {
            // Replaces obsolete IDistributedApplicationLifecycleHook.BeforeStartAsync(...)
            _ = eventing.Subscribe<BeforeStartEvent>((@event, _) =>
            {
                foreach (ProjectResource p in @event.Model.GetProjectResources())
                {
                    p.Annotations.Add(
                        new EnvironmentCallbackAnnotation(ctx =>
                            ctx.EnvironmentVariables["ASPNETCORE_FORWARDEDHEADERS_ENABLED"] = "true"));
                }

                return Task.CompletedTask;
            });

            return Task.CompletedTask;
        }
    }
}