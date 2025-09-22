// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovUkNotifyIntercept.Brokers;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Services.Foundations.Notifications
{
    internal partial class NotificationService : INotificationService
    {
        private readonly IGovUkNotifyBroker govukNotifyBroker;
        private readonly NotifyConfigurations notifyConfigurations;

        public NotificationService(IGovUkNotifyBroker govukNotifyBroker, NotifyConfigurations notifyConfigurations)
        {
            this.govukNotifyBroker = govukNotifyBroker;
            this.notifyConfigurations = notifyConfigurations;
        }

        public ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null) =>
        TryCatch(async () =>
        {
            ValidateOnSendEmailWithTemplateId(toEmail, templateId, personalisation);
            string interceptEmail = notifyConfigurations.InterceptingEmail;
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
            string interceptingMobileNumber = notifyConfigurations.InterceptingMobileNumber;
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

        virtual internal async ValueTask<SubstituteInfo> SubstituteInfoAsync(
            Dictionary<string, dynamic> personalisation)
        {
            var identifier = GetValueOrNull(personalisation, notifyConfigurations.IdentifierKey);

            var patientOverride = notifyConfigurations.NotificationOverrides
                .FirstOrDefault(p => p.Identifier == identifier);

            string mobileNumber = notifyConfigurations.DefaultOverride.Phone;
            string email = notifyConfigurations.DefaultOverride.Email;
            List<string> addressLines = notifyConfigurations.DefaultOverride.AddressLines;

            if (patientOverride is not null)
            {
                mobileNumber = patientOverride.Phone ?? mobileNumber;
                email = patientOverride.Email ?? email;
                addressLines = patientOverride.AddressLines ?? addressLines;
            }

            if (notifyConfigurations.SubstituteDictionaryValues)
            {
                personalisation[notifyConfigurations.PhoneKey] = mobileNumber;
                personalisation[notifyConfigurations.EmailKey] = email;

                personalisation[notifyConfigurations.AddressLine1Key] =
                    addressLines.ElementAtOrDefault(0) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine2Key] =
                    addressLines.ElementAtOrDefault(1) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine3Key] =
                    addressLines.ElementAtOrDefault(2) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine4Key] =
                    addressLines.ElementAtOrDefault(3) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine5Key] =
                    addressLines.ElementAtOrDefault(4) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine6Key] =
                    addressLines.ElementAtOrDefault(5) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine7Key] =
                    addressLines.ElementAtOrDefault(6) ?? string.Empty;
            }

            return new SubstituteInfo
            {
                MobileNumber = mobileNumber,
                Email = email,
                AddressLines = addressLines,
                Personalisation = personalisation
            };
        }
    }
}
