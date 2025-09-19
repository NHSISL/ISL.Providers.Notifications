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
            ValidateDictionaryOnSendEmailWithTemplateId(personalisation);
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
            ValidateDictionaryOnSendSms(personalisation);
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

        virtual internal async ValueTask<SubstituteInfo> SubstituteInfoAsync(
            Dictionary<string, dynamic> personalisation)
        {
            var identifier = GetValueOrNull(personalisation, configurations.IdentifierKey);
            var patientOverride = configurations.NotificationOverrides.FirstOrDefault(p => p.Identifier == identifier);
            string mobileNumber = configurations.DefaultOverride.Phone;
            string email = configurations.DefaultOverride.Email;
            List<string> addressLines = configurations.DefaultOverride.AddressLines;

            if (patientOverride is not null)
            {
                mobileNumber = patientOverride.Phone ?? mobileNumber;
                email = patientOverride.Email ?? email;
                addressLines = patientOverride.AddressLines ?? addressLines;
            }

            if (configurations.SubstituteDictionaryValues)
            {
                personalisation[configurations.PhoneKey] = mobileNumber;
                personalisation[configurations.EmailKey] = email;
                personalisation[configurations.AddressLine1Key] = addressLines.ElementAtOrDefault(0) ?? string.Empty;
                personalisation[configurations.AddressLine2Key] = addressLines.ElementAtOrDefault(1) ?? string.Empty;
                personalisation[configurations.AddressLine3Key] = addressLines.ElementAtOrDefault(2) ?? string.Empty;
                personalisation[configurations.AddressLine4Key] = addressLines.ElementAtOrDefault(3) ?? string.Empty;
                personalisation[configurations.AddressLine5Key] = addressLines.ElementAtOrDefault(4) ?? string.Empty;
                personalisation[configurations.AddressLine6Key] = addressLines.ElementAtOrDefault(5) ?? string.Empty;
                personalisation[configurations.AddressLine7Key] = addressLines.ElementAtOrDefault(6) ?? string.Empty;
            }

            return new SubstituteInfo
            {
                MobileNumber = mobileNumber,
                Email = email,
                AddressLines = addressLines,
                Overrides = personalisation
            };
        }
    }
}
