// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications.Exceptions
{
    internal class NotificationDependencyValidationException : Xeption
    {
        public NotificationDependencyValidationException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
