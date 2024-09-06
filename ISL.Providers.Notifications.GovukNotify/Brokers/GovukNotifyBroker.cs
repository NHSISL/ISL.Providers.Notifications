// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.GovukNotify.Models;
using Notify.Client;

namespace ISL.Providers.Notifications.GovukNotify.Brokers
{
    internal class GovukNotifyBroker : IGovukNotifyBroker
    {
        private readonly NotificationClient client;

        public GovukNotifyBroker(NotifyConfigurations configurations) =>
            this.client = new NotificationClient(configurations.ApiKey);

        public async ValueTask SendEmailAsync(
            string emailAddress,
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null,
            string emailReplyToId = null,
            string oneClickUnsubscribeURL = null)
        {
            await this.client.SendEmailAsync(
                emailAddress,
                templateId,
                personalisation,
                clientReference,
                emailReplyToId,
                oneClickUnsubscribeURL);
        }

        public async ValueTask SendSmsAsync(
            string mobileNumber,
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null,
            string smsSenderId = null)
        {
            await this.client.SendSmsAsync(
                mobileNumber,
                templateId,
                personalisation,
                clientReference,
                smsSenderId);
        }

        public async ValueTask SendLetterAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null)
        {
            await this.client.SendLetterAsync(
                templateId,
                personalisation,
                clientReference);
        }

        public async ValueTask SendPrecompiledLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null)
        {
            await this.client.SendPrecompiledLetterAsync(
                templateId,
                pdfContents,
                postage);
        }
    }
}
