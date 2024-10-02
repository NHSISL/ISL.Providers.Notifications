// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.GovukNotify.Models;
using Notify.Client;
using Notify.Models.Responses;

namespace ISL.Providers.Notifications.GovukNotify.Brokers
{
    internal class GovukNotifyBroker : IGovukNotifyBroker
    {
        private readonly NotificationClient client;

        public GovukNotifyBroker(NotifyConfigurations configurations) =>
            this.client = new NotificationClient(configurations.ApiKey);

        public async ValueTask<string> SendEmailAsync(
            string emailAddress,
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null,
            string emailReplyToId = null,
            string oneClickUnsubscribeURL = null)
        {
            EmailNotificationResponse response = await this.client.SendEmailAsync(
                emailAddress,
                templateId,
                personalisation,
                clientReference,
                emailReplyToId,
                oneClickUnsubscribeURL);

            return response.id;
        }

        public async ValueTask<string> SendSmsAsync(
            string mobileNumber,
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null,
            string smsSenderId = null)
        {
            SmsNotificationResponse response = await this.client.SendSmsAsync(
                mobileNumber,
                templateId,
                personalisation,
                clientReference,
                smsSenderId);

            return response.id;
        }

        public async ValueTask<string> SendLetterAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null)
        {
            LetterNotificationResponse response = await this.client.SendLetterAsync(
                templateId,
                personalisation,
                clientReference);

            return response.id;
        }

        public async ValueTask<string> SendPrecompiledLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null)
        {
            LetterNotificationResponse response = await this.client.SendPrecompiledLetterAsync(
                templateId,
                pdfContents,
                postage);

            return response.id;
        }
    }
}
