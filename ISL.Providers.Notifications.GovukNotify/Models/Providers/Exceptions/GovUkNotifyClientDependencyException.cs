﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.GovukNotify.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a dependency error occurs while using the notification provider.
    /// For example, if a required dependency is unavailable or incompatible.
    /// </summary>
    public class GovUkNotifyProviderDependencyException : Xeption, INotificationProviderDependencyException
    {
        public GovUkNotifyProviderDependencyException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
