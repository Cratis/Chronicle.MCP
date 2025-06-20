// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Chronicle.Mcp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var builder = Host.CreateApplicationBuilder(args);

string[] configSectionPath = ["Cratis", "Chronicle", "Mcp"];

var configSection = ConfigurationPath.Combine(configSectionPath);

builder.Services
    .AddOptions<McpServerOptions>()
    .BindConfiguration(configSection);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Logging.AddConsole(logging => logging.LogToStandardErrorThreshold = LogLevel.Trace);

builder.Services.AddCratisChronicleConnection(urlFactory: (sp) =>
{
    var options = sp.GetRequiredService<IOptions<McpServerOptions>>().Value;
    return options.ConnectionString;
});
builder.Services.AddCratisChronicleServices();

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

var host = builder.Build();
await host.RunAsync();
