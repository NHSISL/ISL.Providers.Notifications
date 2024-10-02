// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.GovukNotify.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a dependency validation error occurs while using the notification provider.
    /// For example, if an external dependency used by the provider requires data that is missing or invalid.
    /// </summary>
    public class GovUkNotifyProviderDependencyValidationException
        : Xeption, INotificationProviderDependencyValidationException
    {
        public GovUkNotifyProviderDependencyValidationException(
            string message,
            Xeption innerException,
            IDictionary data)
            : base(message: message, innerException, data)
        { }
    }
}