// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions
{
    public class FailedNotificationServerException : Xeption
    {
        public FailedNotificationServerException(string message, Exception innerException)
            : base(message, innerException) { }

        public FailedNotificationServerException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data) { }
    }
}
