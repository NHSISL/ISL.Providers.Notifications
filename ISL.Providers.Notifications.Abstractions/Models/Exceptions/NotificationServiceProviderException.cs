// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using Xeptions;

namespace ISL.Providers.Notifications.Abstractions.Models.Exceptions
{
    public class NotificationServiceProviderException : Xeption
    {
        public NotificationServiceProviderException(string message, Xeption innerException)
            : base(message, innerException)
        { }

        public NotificationServiceProviderException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
