// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions
{
    public class FailedNotificationServerException : Xeption
    {
        internal FailedNotificationServerException(string message, Exception innerException)
            : base(message, innerException) { }

        internal FailedNotificationServerException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data) { }
    }
}
