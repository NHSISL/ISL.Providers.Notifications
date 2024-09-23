// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.Abstractions.Tests.Unit.Models.Exceptions
{
    public class SomeNotificationDependencyException : Xeption, INotificationProviderDependencyException
    {
        public SomeNotificationDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
