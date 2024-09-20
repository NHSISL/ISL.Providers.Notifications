// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.GovukNotify.Brokers;

namespace ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications
{
    internal class NotificationService : INotificationService
    {
        private readonly IGovukNotifyBroker govukNotifyBroker;

        public NotificationService(IGovukNotifyBroker govukNotifyBroker) =>
            this.govukNotifyBroker = govukNotifyBroker;

        public ValueTask SendEmailAsync(
            string fromEmail,
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation) =>
            throw new NotImplementedException();

        public ValueTask SendLetterAsync(string templateId, byte[] pdfContents, string postage = null) =>
            throw new NotImplementedException();

        public ValueTask SendSmsAsync(string templateId, Dictionary<string, dynamic> personalisation) =>
            throw new NotImplementedException();
    }
}
