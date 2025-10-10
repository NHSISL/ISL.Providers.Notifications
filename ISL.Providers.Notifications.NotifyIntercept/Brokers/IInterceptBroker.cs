// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.NotifyIntercept.Brokers
{
    internal interface IInterceptBroker
    {
        ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null);

        ValueTask<string> SendSmsAsync(
            string mobileNumber,
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null,
            string smsSenderId = null);

        ValueTask<string> SendLetterAsync(
            string templateId,
            string addressLine1,
            string addressLine2,
            string addressLine3,
            string addressLine4,
            string addressLine5,
            string addressLine6,
            string addressLine7,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null);
    }
}
