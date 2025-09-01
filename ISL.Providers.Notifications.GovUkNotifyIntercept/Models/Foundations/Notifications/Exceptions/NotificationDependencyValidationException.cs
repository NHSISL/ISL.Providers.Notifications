// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions
{
    public class NotificationDependencyValidationException : Xeption
    {
        internal NotificationDependencyValidationException(string message, Xeption innerException)
         : base(message, innerException)
        { }
    }
}
