using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ServiceBus.Samples
{
    using Microsoft.ServiceBus.Messaging;

    public class Manage
    {

        public static async Task<EventHubDescription> UpdateEventHub(string eventHubName, string consumerGroupName, NamespaceManager namespaceManager)
        {
            // Add a consumer group
            EventHubDescription ehd = await namespaceManager.GetEventHubAsync(eventHubName);
            await namespaceManager.CreateConsumerGroupIfNotExistsAsync(ehd.Path, consumerGroupName);

            // Create a customer SAS rule with Manage permissions
            ehd.UserMetadata = "Some updated info";
            string ruleName = "myeventhubmanagerule";
            string ruleKey = SharedAccessAuthorizationRule.GenerateRandomKey();
            ehd.Authorization.Add(new SharedAccessAuthorizationRule(ruleName, ruleKey, new AccessRights[] { AccessRights.Manage, AccessRights.Listen, AccessRights.Send }));

            EventHubDescription ehdUpdated = await namespaceManager.UpdateEventHubAsync(ehd);
            return ehd;
        }
    }
}
