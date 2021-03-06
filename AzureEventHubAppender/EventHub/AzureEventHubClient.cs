﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace Logging.EventHub
{
    [ExcludeFromCodeCoverage]

    /// <summary>
    /// Azure implementation of IEventHubClient - this is just a thin implementaiton to enable unit testing
    /// </summary>
    class AzureEventHubClient : IEventHubClient
    {
        private EventHubClient EventHubClient  { get; }
        private AzureEventHubClient(string ConnectionString, string EventHubName)
        {
            EventHubClient = EventHubClient.CreateFromConnectionString(ConnectionString, EventHubName);
        }


        public static IEventHubClient CreateFromConnectionString(string ConnectionString, string EventHubName)
        {
            return new AzureEventHubClient(ConnectionString, EventHubName);
        }


        #region IEventHubClient

        //
        // Summary:
        //     Asynchronously sends a batch of event data.
        //
        // Parameters:
        //   eventDataList:
        //     An IEnumerable object containing event data instances.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task.
        //
        // Exceptions:
        //   T:Microsoft.ServiceBus.Messaging.MessageSizeExceededException:
        //     Thrown if the total serialized size of eventDataList exceeds the allowed size
        //     limit for one event transmission (256k by default).
        //
        // Remarks:
        //     User should make sure the total serialized size of eventDataList should be under
        //     the size limit of one event data transmission, which is 256k by default. Also
        //     note that there will be some overhead to form the batch.
        public async Task SendBatchAsync(IEnumerable<EventData> eventDataList)
        {
            await EventHubClient.SendBatchAsync(eventDataList).ConfigureAwait(false);
        }

        //
        // Summary:
        //     Sends a cleanup message asynchronously to Service Bus to signal the completion
        //     of the usage of an entity.
        //
        // Returns:
        //     If an exception occurs, this method performs an abort operation on the entity
        //     before throwing the exception.
        //
        // Remarks:
        //     In the event of any exceptions, the method will perform an abort on the entity
        //     before throwing the exception.
        public async Task CloseAsync()
        {
            await EventHubClient.CloseAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
