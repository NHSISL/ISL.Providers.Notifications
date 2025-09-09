// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions
{
    internal class InvalidArgumentNotificationException : Xeption
    {
        public InvalidArgumentNotificationException(string message)
            : base(message)
        { }
    }
}