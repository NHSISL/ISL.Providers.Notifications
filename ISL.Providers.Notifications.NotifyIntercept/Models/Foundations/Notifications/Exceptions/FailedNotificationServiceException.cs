// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications.Exceptions
{
    internal class FailedNotificationServiceException : Xeption
    {
        public FailedNotificationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}