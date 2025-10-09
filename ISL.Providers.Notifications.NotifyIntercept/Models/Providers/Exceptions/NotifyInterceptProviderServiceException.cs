// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections;
using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.NotifyIntercept.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a service error occurs while using the notification provider. 
    /// For example, if there is a problem with the server or any other service failure.
    /// </summary>
    public class NotifyInterceptProviderServiceException : Xeption, INotificationProviderServiceException
    {
        public NotifyInterceptProviderServiceException(string message, Xeption innerException, IDictionary data)
            : base(message, innerException, data)
        { }
    }
}
