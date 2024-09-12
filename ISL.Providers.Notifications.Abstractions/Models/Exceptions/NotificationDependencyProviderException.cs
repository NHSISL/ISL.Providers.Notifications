// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using Xeptions;

namespace ISL.Providers.Notifications.Abstractions.Models.Exceptions
{
    public class NotificationDependencyProviderException : Xeption
    {
        public NotificationDependencyProviderException(string message, Xeption innerException)
        : base(message, innerException)
        { }

        public NotificationDependencyProviderException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
