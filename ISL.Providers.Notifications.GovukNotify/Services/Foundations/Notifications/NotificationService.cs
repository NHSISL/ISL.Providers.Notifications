// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovukNotify.Brokers;
using ISL.Providers.Notifications.GovukNotify.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            ValidateOnSendEmail(toEmail, subject, body, personalisation);
            AddOrUpdate(personalisation, "subject", subject);
            AddOrUpdate(personalisation, "body", body);
            string templateId = GetValueOrNull(personalisation, "templateId");
            string clientReference = GetValueOrNull(personalisation, "clientReference");
            string emailReplyToId = GetValueOrNull(personalisation, "emailReplyToId");
            string oneClickUnsubscribeURL = GetValueOrNull(personalisation, "oneClickUnsubscribeURL");
            ValidateDictionaryOnSendEmail(personalisation);

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
            ValidateDictionaryOnSendEmailWithTemplateId(personalisation);

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
            ValidateDictionaryOnSendSms(personalisation);
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
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null) =>
            TryCatch(async () =>
            {
                ValidateOnSendLetter(templateId);

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
