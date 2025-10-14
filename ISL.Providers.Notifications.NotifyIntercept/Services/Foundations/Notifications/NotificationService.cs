// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
            string recipientName,
            string addressLine1,
            string addressLine2,
            string addressLine3,
            string addressLine4,
            string addressLine5,
            string postCode,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null) =>
            TryCatch(async () =>
            {
                ValidateOnSendLetter(templateId, recipientName, addressLine1, postCode, personalisation);
                ValidateNotificationConfiguration(notifyConfigurations);
                SubstituteInfo substituteInfo = await SubstituteInfoAsync(personalisation);

                return await this.interceptBroker.SendLetterAsync(
                    templateId: templateId,
                    recipientName: substituteInfo.AddressLines.First(),
                    addressLine1: substituteInfo.AddressLines.ElementAtOrDefault(1),
                    addressLine2: substituteInfo.AddressLines.ElementAtOrDefault(2),
                    addressLine3: substituteInfo.AddressLines.ElementAtOrDefault(3),
                    addressLine4: substituteInfo.AddressLines.ElementAtOrDefault(4),
                    addressLine5: substituteInfo.AddressLines.ElementAtOrDefault(5),
                    postCode: substituteInfo.AddressLines.Last(),
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

            var addressParts = new List<string>
            {
                notifyConfigurations.DefaultOverride.RecipientName,
                notifyConfigurations.DefaultOverride.AddressLine1,
                notifyConfigurations.DefaultOverride.AddressLine2,
                notifyConfigurations.DefaultOverride.AddressLine3,
                notifyConfigurations.DefaultOverride.AddressLine4,
                notifyConfigurations.DefaultOverride.AddressLine5,
                notifyConfigurations.DefaultOverride.PostCode
            };

            if (patientOverride is not null)
            {
                mobileNumber = patientOverride.Phone ?? mobileNumber;
                email = patientOverride.Email ?? email;

                addressParts = new List<string>
                {
                    patientOverride.RecipientName ?? notifyConfigurations.DefaultOverride.RecipientName,
                    patientOverride.AddressLine1 ?? notifyConfigurations.DefaultOverride.AddressLine1,
                    patientOverride.AddressLine2 ?? notifyConfigurations.DefaultOverride.AddressLine2,
                    patientOverride.AddressLine3 ?? notifyConfigurations.DefaultOverride.AddressLine3,
                    patientOverride.AddressLine4 ?? notifyConfigurations.DefaultOverride.AddressLine4,
                    patientOverride.AddressLine5 ?? notifyConfigurations.DefaultOverride.AddressLine5,
                    patientOverride.PostCode ?? notifyConfigurations.DefaultOverride.PostCode
                };
            }

            if (notifyConfigurations.SubstituteDictionaryValues)
            {
                personalisation[notifyConfigurations.PhoneKey] = mobileNumber;
                personalisation[notifyConfigurations.EmailKey] = email;
            }

            var populatedAddressParts = addressParts.Where(part => !string.IsNullOrWhiteSpace(part)).ToList();

            var addressKeys = new List<string>
            {
                notifyConfigurations.AddressLine1Key,
                notifyConfigurations.AddressLine2Key,
                notifyConfigurations.AddressLine3Key,
                notifyConfigurations.AddressLine4Key,
                notifyConfigurations.AddressLine5Key,
                notifyConfigurations.AddressLine6Key,
                notifyConfigurations.AddressLine7Key
            };

            int linesToProcess = Math.Min(populatedAddressParts.Count, addressKeys.Count);

            for (int i = 0; i < linesToProcess; i++)
            {
                if (!string.IsNullOrWhiteSpace(addressKeys[i]))
                {
                    personalisation[addressKeys[i]] = populatedAddressParts[i];
                }
            }

            return new SubstituteInfo
            {
                MobileNumber = mobileNumber,
                Email = email,
                AddressLines = populatedAddressParts,
                Personalisation = personalisation
            };
        }
    }
}
