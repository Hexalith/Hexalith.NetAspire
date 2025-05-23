// <copyright file="Program.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

await builder.Build().RunAsync().ConfigureAwait(false);