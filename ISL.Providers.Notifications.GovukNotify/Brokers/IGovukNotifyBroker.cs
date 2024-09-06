// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovukNotify.Brokers
{
    internal interface IGovukNotifyBroker
    {
        ValueTask SendEmailAsync(
            string emailAddress,
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null,
            string emailReplyToId = null,
            string oneClickUnsubscribeURL = null);

        ValueTask SendSmsAsync(
            string mobileNumber,
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null,
            string smsSenderId = null);

        ValueTask SendLetterAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null);
    }
}
