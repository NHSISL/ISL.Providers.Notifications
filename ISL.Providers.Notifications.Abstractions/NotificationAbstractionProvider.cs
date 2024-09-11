// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.Abstractions
{
    public partial class NotificationAbstractionProvider : INotificationAbstractionProvider
    {
        private readonly INotificationProvider notificationProvider;

        public NotificationAbstractionProvider(INotificationProvider notificationProvider) =>
            this.notificationProvider = notificationProvider;

        public async ValueTask SendEmailAsync(
            string fromEmail,
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation) =>
            TryCatch(async () =>
            {
                await this.notificationProvider.SendEmailAsync(fromEmail, toEmail, subject, body, personalisation);
            });

        public async ValueTask SendLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null)
        {
            try
            {
                await this.notificationProvider.SendLetterAsync(templateId, pdfContents, postage);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async ValueTask SendSmsAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            try
            {
                await this.notificationProvider.SendSmsAsync(templateId, personalisation);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
