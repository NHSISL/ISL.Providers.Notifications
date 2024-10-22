// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions
{
    public class FailedNotificationServiceException : Xeption
    {
        internal FailedNotificationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}