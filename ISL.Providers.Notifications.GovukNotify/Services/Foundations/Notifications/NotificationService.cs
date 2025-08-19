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
        private readonly IGovukNotifyBroker govukNotifyBroker;
        private readonly NotifyConfigurations configurations;

        public NotificationService(IGovukNotifyBroker govukNotifyBroker, NotifyConfigurations configurations)
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
            await ValidateOnSendEmail(toEmail, subject, body, personalisation);
            AddOrUpdate(personalisation, "subject", subject);
            AddOrUpdate(personalisation, "body", body);
            string templateId = GetValueOrNull(personalisation, "templateId");
            string clientReference = GetValueOrNull(personalisation, "clientReference");
            string emailReplyToId = GetValueOrNull(personalisation, "emailReplyToId");
            string oneClickUnsubscribeURL = GetValueOrNull(personalisation, "oneClickUnsubscribeURL");
            await ValidateDictionaryOnSendEmail(personalisation);

            return await this.govukNotifyBroker.SendEmailAsync(
                toEmail,
                templateId,
                personalisation: personalisation,
                clientReference,
                emailReplyToId,
                oneClickUnsubscribeURL);
        });

        public async ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null)
        {
            string emailReplyToId = GetValueOrNull(personalisation, "emailReplyToId");
            string oneClickUnsubscribeURL = GetValueOrNull(personalisation, "oneClickUnsubscribeURL");

            return await this.govukNotifyBroker.SendEmailAsync(
                toEmail,
                templateId,
                personalisation: personalisation,
                clientReference: clientReference,
                emailReplyToId: emailReplyToId,
                oneClickUnsubscribeURL: oneClickUnsubscribeURL);
        }

        public async ValueTask<string> SendSmsAsync(string templateId, Dictionary<string, dynamic> personalisation) =>
            throw new NotImplementedException();

        public ValueTask<string> SendLetterAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null) =>
            TryCatch(async () =>
            {
                await ValidateOnSendLetter(templateId);

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
    }
}
