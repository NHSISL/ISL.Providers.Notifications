// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.Abstractions;

namespace ISL.Providers.Notifications.NotifyIntercept.Brokers
{
    internal class InterceptBroker : IInterceptBroker
    {
        private readonly INotificationProvider provider;

        public InterceptBroker(INotificationProvider provider)
        {
            this.provider = provider;
        }

        public async ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null)
        {
            return await this.provider.SendEmailAsync(
                templateId,
                toEmail,
                personalisation,
                clientReference);
        }

        public async ValueTask<string> SendSmsAsync(
           string mobileNumber,
           string templateId,
           Dictionary<string, dynamic> personalisation = null,
           string clientReference = null,
           string smsSenderId = null)
        {
            return await this.provider.SendSmsAsync(
                templateId,
                mobileNumber,
                personalisation);
        }

        public async ValueTask<string> SendLetterAsync(
            string templateId,
            string recipientName,
            string addressLine1,
            string addressLine2,
            string addressLine3,
            string addressLine4,
            string addressLine5,
            string postCode,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null)
        {
            return await this.provider.SendLetterAsync(
                templateId,
                recipientName,
                addressLine1,
                addressLine2,
                addressLine3,
                addressLine4,
                addressLine5,
                postCode,
                personalisation,
                clientReference);
        }
    }
}
