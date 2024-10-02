// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.Abstractions.Tests.Unit.Models.Exceptions
{
    public class SomeNotificationServiceException : Xeption, INotificationProviderServiceException
    {
        public SomeNotificationServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
