// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Chronicle.Mcp;

/// <summary>
/// Represents options for configuring the Chronicle MCP server.
/// </summary>
public class McpServerOptions
{
    /// <summary>
    /// Gets or sets the connection string for the Chronicle server.
    /// </summary>
    public string ConnectionString { get; set; } = "chronicle://localhost:35000";
}
