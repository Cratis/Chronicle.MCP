// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Connections;
using Cratis.Chronicle.Contracts;
using Cratis.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(logging => logging.LogToStandardErrorThreshold = LogLevel.Trace);
builder.Services.TryAddSingleton<ICorrelationIdAccessor, CorrelationIdAccessor>();
builder.Services.AddSingleton<IChronicleConnection>(sp =>
{
    var lifetime = sp.GetRequiredService<IHostApplicationLifetime>();
    var connectionLifecycle = new ConnectionLifecycle(sp.GetRequiredService<ILogger<ConnectionLifecycle>>());
    var correlationIdAccessor = sp.GetRequiredService<ICorrelationIdAccessor>();
    return new ChronicleConnection(
        "chronicle://localhost:35000",
        5,
        connectionLifecycle,
        new Cratis.Tasks.TaskFactory(),
        correlationIdAccessor,
        sp.GetRequiredService<ILogger<ChronicleConnection>>(),
        lifetime.ApplicationStopping);
});

builder.Services.AddSingleton(sp =>
{
    var connection = sp.GetRequiredService<IChronicleConnection>();
    var properties = typeof(ChronicleConnection).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).ToArray();
    var property = properties.FirstOrDefault(p => p.Name.EndsWith("Services"));
    var services = property?.GetMethod?.Invoke(connection, null) as IServices;
    return services!;
});

builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().EventStores);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Namespaces);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Recommendations);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Identities);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().EventSequences);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().EventTypes);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Constraints);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Observers);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().FailedPartitions);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Reactors);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Reducers);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Projections);
builder.Services.AddSingleton(sp => sp.GetRequiredService<IServices>().Jobs);

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var host = builder.Build();
await host.RunAsync();
