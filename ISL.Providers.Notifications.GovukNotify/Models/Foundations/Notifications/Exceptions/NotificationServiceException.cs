// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using Xeptions;

namespace ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions
{
    public class NotificationServiceException : Xeption
    {
        public NotificationServiceException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}