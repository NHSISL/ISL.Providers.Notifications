// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions
{
    public class NotificationValidationException : Xeption
    {
        internal NotificationValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}