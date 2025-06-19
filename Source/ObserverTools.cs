// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Cratis.Chronicle.Contracts.Observation;
using ModelContextProtocol.Server;

namespace Cratis.Chronicle.MCP;

/// <summary>
/// Represents a set of tools for working with observers in Chronicle.
/// </summary>
[McpServerToolType]
public static class ObserverTools
{
    /// <summary>
    /// Gets all observers for a specific event store.
    /// </summary>
    /// <param name="observers">The <see cref="IObservers"/> to use.</param>
    /// <param name="eventStore">The name of the event store to get observers from.</param>
    /// <returns>A collection of <see cref="ObserverInformation"/> for the specified
    [McpServerTool, Description("Gets all observers for a specific event store and optional namespace.")]
    public static Task<IEnumerable<ObserverInformation>> GetObservers(IObservers observers, string eventStore) =>
        GetObserversForNamespace(observers, eventStore);

    /// <summary>
    /// Gets all observers for a specific event store and optional namespace.
    /// </summary>
    /// <param name="observers">The <see cref="IObservers"/> to use.</param>
    /// <param name="eventStore">The name of the event store to get observers from.</param>
    /// <param name="namespace">The namespace to filter observers by.</param>
    /// <returns>A collection of <see cref="ObserverInformation"/> for the specified event store and namespace.</returns>
    [McpServerTool, Description("Gets all observers for a specific event store and optional namespace.")]
    public static async Task<IEnumerable<ObserverInformation>> GetObserversForNamespace(IObservers observers, string eventStore, string? @namespace = "Default")
    {
        return await observers.GetObservers(new AllObserversRequest
        {
            EventStore = eventStore,
            Namespace = @namespace ?? "Default"
        });
    }
}
