// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovUkNotifyIntercept.Models;
using Notify.Client;
using Notify.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Brokers
{
    internal class GovukNotifyBroker : IGovukNotifyBroker
    {
        private readonly NotificationClient client;

        public GovukNotifyBroker(NotifyConfigurations configurations) =>
            this.client = new NotificationClient(configurations.ApiKey);

        public async ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null)
        {
            EmailNotificationResponse response = await this.client.SendEmailAsync(
                toEmail,
                templateId,
                personalisation,
                clientReference);

            return response.id;
        }
    }
}
