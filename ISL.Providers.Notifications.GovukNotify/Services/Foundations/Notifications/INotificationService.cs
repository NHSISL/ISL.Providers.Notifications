// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications
{
    internal interface INotificationService
    {
        ValueTask SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation);

        ValueTask SendSmsAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation);

        ValueTask SendLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null);
    }
}
