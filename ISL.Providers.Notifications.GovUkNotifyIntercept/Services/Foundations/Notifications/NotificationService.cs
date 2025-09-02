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
        private readonly IGovukNotifyBroker govukNotifyBroker;
        private readonly NotifyConfigurations configurations;

        public NotificationService(IGovukNotifyBroker govukNotifyBroker, NotifyConfigurations configurations)
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
            await ValidateOnSendEmailWithTemplateIdAsync(toEmail, templateId, personalisation);
            await ValidateDictionaryOnSendEmailWithTemplateIdAsync(personalisation);

            string interceptEmail = configurations.InterceptingEmail;

            await ValidateInterceptingEmailAsync(interceptEmail);

            return await this.govukNotifyBroker.SendEmailAsync(
                interceptEmail,
                templateId,
                personalisation: personalisation,
                clientReference: clientReference);
        });

        public ValueTask<string> SendSmsAsync(
            string templateId,
            string mobileNumber,
            Dictionary<string, dynamic> personalisation) =>
        TryCatch(async () =>
        {
            await ValidateOnSendSms(templateId, mobileNumber, personalisation);
            await ValidateDictionaryOnSendSms(personalisation);
            string clientReference = GetValueOrNull(personalisation, "clientReference");
            string smsSenderId = GetValueOrNull(personalisation, "smsSenderId");

            string interceptingMobileNumber = configurations.InterceptingMobileNumber;

            await ValidateInterceptingMobileNumberAsync(interceptingMobileNumber);

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
