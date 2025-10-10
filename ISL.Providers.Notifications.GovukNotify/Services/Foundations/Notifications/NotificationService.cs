// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.GovukNotify.Brokers;
using ISL.Providers.Notifications.GovukNotify.Models;

namespace ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications
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
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation) =>
        TryCatch(async () =>
        {
            ValidateOnSendEmail(toEmail, subject, body, personalisation);
            AddOrUpdate(personalisation, "subject", subject);
            AddOrUpdate(personalisation, "body", body);
            string templateId = GetValueOrNull(personalisation, "templateId");
            string clientReference = GetValueOrNull(personalisation, "clientReference");
            string emailReplyToId = GetValueOrNull(personalisation, "emailReplyToId");
            string oneClickUnsubscribeURL = GetValueOrNull(personalisation, "oneClickUnsubscribeURL");

            return await this.govukNotifyBroker.SendEmailAsync(
                toEmail,
                templateId,
                personalisation: personalisation,
                clientReference,
                emailReplyToId,
                oneClickUnsubscribeURL);
        });

        public ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null) =>
        TryCatch(async () =>
        {
            ValidateOnSendEmailWithTemplateId(toEmail, templateId, personalisation);
            string emailReplyToId = GetValueOrNull(personalisation, "emailReplyToId");
            string oneClickUnsubscribeURL = GetValueOrNull(personalisation, "oneClickUnsubscribeURL");

            return await this.govukNotifyBroker.SendEmailAsync(
                toEmail,
                templateId,
                personalisation: personalisation,
                clientReference: clientReference,
                emailReplyToId: emailReplyToId,
                oneClickUnsubscribeURL: oneClickUnsubscribeURL);
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

            return await this.govukNotifyBroker.SendSmsAsync(
                mobileNumber: mobileNumber,
                templateId: templateId,
                personalisation: personalisation,
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
            Dictionary<string, dynamic> personalisation,
            string clientReference = null) =>
            TryCatch(async () =>
            {
                personalisation = UpdatePersonalisation(
                    addressLine1,
                    addressLine2,
                    addressLine3,
                    addressLine4,
                    addressLine5,
                    addressLine6,
                    addressLine7,
                    personalisation);

                ValidateOnSendLetter(templateId, addressLine1, addressLine2, addressLine3, personalisation);

                return await this.govukNotifyBroker.SendLetterAsync(
                    templateId,
                    personalisation,
                    clientReference);
            });

        public ValueTask<string> SendPrecompiledLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null) =>
            throw new NotImplementedException();

        private static void AddOrUpdate(Dictionary<string, dynamic> dictionary, string key, dynamic value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static dynamic GetValueOrNull(Dictionary<string, dynamic> dictionary, string key) =>
            dictionary.ContainsKey(key) ? dictionary[key] : null;

        private Dictionary<string, dynamic> UpdatePersonalisation(
            string addressLine1,
            string addressLine2,
            string addressLine3,
            string addressLine4,
            string addressLine5,
            string addressLine6,
            string addressLine7,
            Dictionary<string, dynamic> personalisation)
        {
            personalisation ??= new Dictionary<string, dynamic>();

            void UpsertAddress(string key, string value)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    personalisation[key] = value;
                }
                else
                {
                    if (personalisation.ContainsKey(key))
                    {
                        personalisation.Remove(key);
                    }
                }
            }

            UpsertAddress("addressLine1", addressLine1);
            UpsertAddress("addressLine2", addressLine2);
            UpsertAddress("addressLine3", addressLine3);
            UpsertAddress("addressLine4", addressLine4);
            UpsertAddress("addressLine5", addressLine5);
            UpsertAddress("addressLine6", addressLine6);
            UpsertAddress("addressLine7", addressLine7);

            return personalisation;
        }
    }
}
