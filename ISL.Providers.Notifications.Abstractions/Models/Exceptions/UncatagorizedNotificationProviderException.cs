// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.Providers.Notifications.Abstractions.Models.Exceptions
{
    public class UncatagorizedNotificationProviderException : Xeption
    {
        public UncatagorizedNotificationProviderException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public UncatagorizedNotificationProviderException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
