// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications
{
    internal interface INotificationService
    {
        ValueTask<string> SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation);

        ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null);

        ValueTask<string> SendSmsAsync(
            string templateId,
            string mobileNumber,
            Dictionary<string, dynamic> personalisation);

        ValueTask<string> SendLetterAsync(
            string templateId,
            string recipientName,
            string addressLine1,
            string addressLine2,
            string addressLine3,
            string addressLine4,
            string addressLine5,
            string postCode,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null);

        ValueTask<string> SendPrecompiledLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null);
    }
}
