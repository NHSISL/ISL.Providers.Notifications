// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISL.Providers.Notifications.NotifyIntercept.Brokers;
using ISL.Providers.Notifications.NotifyIntercept.Models;
using ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications;

namespace ISL.Providers.Notifications.NotifyIntercept.Services.Foundations.Notifications
{
    internal partial class NotificationService : INotificationService
    {
        private readonly IInterceptBroker interceptBroker;
        private readonly NotifyConfigurations notifyConfigurations;

        public NotificationService(IInterceptBroker interceptBroker, NotifyConfigurations notifyConfigurations)
        {
            this.interceptBroker = interceptBroker;
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
            ValidateNotificationConfiguration(notifyConfigurations);
            SubstituteInfo substituteInfo = await SubstituteInfoAsync(personalisation);

            return await this.interceptBroker.SendEmailAsync(
                templateId: templateId,
                toEmail: substituteInfo.Email,
                personalisation: substituteInfo.Personalisation,
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
            ValidateNotificationConfiguration(notifyConfigurations);
            SubstituteInfo substituteInfo = await SubstituteInfoAsync(personalisation);

            return await this.interceptBroker.SendSmsAsync(
                mobileNumber: substituteInfo.MobileNumber,
                templateId: templateId,
                personalisation: substituteInfo.Personalisation,
                clientReference: clientReference,
                smsSenderId: smsSenderId);
        });

        public ValueTask<string> SendLetterAsync(
            string templateId,
            string addressLine1,
            string addressLine2,
            string addressLine3,
            string addressLine4,
            string addressLine5,
            string addressLine6,
            string addressLine7,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null) =>
            TryCatch(async () =>
            {
                ValidateOnSendLetter(templateId, personalisation);
                ValidateNotificationConfiguration(notifyConfigurations);
                SubstituteInfo substituteInfo = await SubstituteInfoAsync(personalisation);

                return await this.interceptBroker.SendLetterAsync(
                    templateId: templateId,
                    addressLine1: addressLine1,
                    addressLine2: addressLine2,
                    addressLine3: addressLine3,
                    addressLine4: addressLine4,
                    addressLine5: addressLine5,
                    addressLine6: addressLine6,
                    addressLine7: addressLine7,
                    personalisation: substituteInfo.Personalisation,
                    clientReference: clientReference);
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
            }

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
