// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.ComponentModel;
using Cratis.Chronicle.Contracts.Events;
using ModelContextProtocol.Server;

namespace Cratis.Chronicle.MCP;

/// <summary>
/// Represents a set of tools for working with event types in Chronicle.
/// </summary>
[McpServerToolType]
public static class EventTypeTools
{
    /// <summary>
    /// Gets all the event types for a specific event store.
    /// </summary>
    /// <param name="eventTypes">The <see cref="IEventTypes"/> to use.</param>
    /// <param name="eventStore">The name of the event store to get from.</param>
    /// <returns>A collection of <see cref="EventType"/> for the specified event store.</returns>
    [McpServerTool, Description("Gets all the event types for a specific event store.")]
    public static async Task<IEnumerable<EventType>> GetEventTypes(IEventTypes eventTypes, string eventStore) =>
        await eventTypes.GetAll(new()
        {
            EventStore = eventStore,
        });
}
