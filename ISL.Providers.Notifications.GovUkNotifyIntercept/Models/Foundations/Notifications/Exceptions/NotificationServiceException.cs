// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions
{
    internal class NotificationServiceException : Xeption
    {
        public NotificationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}