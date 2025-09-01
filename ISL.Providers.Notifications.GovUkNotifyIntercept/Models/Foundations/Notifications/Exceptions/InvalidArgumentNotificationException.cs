// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions
{
    public class InvalidArgumentNotificationException : Xeption
    {
        internal InvalidArgumentNotificationException(string message)
            : base(message)
        { }
    }
}