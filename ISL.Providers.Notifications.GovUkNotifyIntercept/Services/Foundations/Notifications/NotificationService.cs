// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovUkNotifyIntercept.Brokers;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Services.Foundations.Notifications
{
    internal partial class NotificationService : INotificationService
    {
        private readonly IGovUkNotifyBroker govukNotifyBroker;
        private readonly NotifyConfigurations configurations;

        public NotificationService(IGovUkNotifyBroker govukNotifyBroker, NotifyConfigurations configurations)
        {
            this.govukNotifyBroker = govukNotifyBroker;
            this.configurations = configurations;
        }

        public ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null) =>
        TryCatch(async () =>
        {
            ValidateOnSendEmailWithTemplateId(toEmail, templateId, personalisation);
            string interceptEmail = configurations.InterceptingEmail;
            ValidateInterceptingEmail(interceptEmail);

            return await this.govukNotifyBroker.SendEmailAsync(
                templateId: templateId,
                toEmail: interceptEmail,
                personalisation: personalisation,
                clientReference: clientReference);
        });

        public ValueTask<string> SendSmsAsync(
            string templateId,
            string mobileNumber,
            Dictionary<string, dynamic> personalisation) =>
        TryCatch(async () =>
        {
            ValidateOnSendSms(templateId, mobileNumber, personalisation);
            string clientReference = GetValueOrNull(personalisation, "clientReference");
            string smsSenderId = GetValueOrNull(personalisation, "smsSenderId");
            string interceptingMobileNumber = configurations.InterceptingMobileNumber;
            ValidateInterceptingMobileNumberAsync(interceptingMobileNumber);

            return await this.govukNotifyBroker.SendSmsAsync(
                mobileNumber: interceptingMobileNumber,
                templateId: templateId,
                personalisation: personalisation,
                clientReference: clientReference,
                smsSenderId: smsSenderId);
        });

        public static dynamic GetValueOrNull(Dictionary<string, dynamic> dictionary, string key) =>
            dictionary.ContainsKey(key) ? dictionary[key] : null;
    }
}
