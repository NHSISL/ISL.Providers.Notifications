﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.NotifyIntercept.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a validation error occurs while using the notification provider.
    /// For example, if required data is missing or invalid.
    /// </summary>
    public class NotifyInterceptProviderValidationException : Xeption, INotificationProviderValidationException
    {
        public NotifyInterceptProviderValidationException(string message, Xeption innerException, IDictionary data)
            : base(message: message, innerException, data)
        { }
    }
}
