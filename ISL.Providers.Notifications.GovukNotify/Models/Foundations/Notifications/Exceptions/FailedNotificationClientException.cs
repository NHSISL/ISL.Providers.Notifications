﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections;
using Xeptions;

namespace ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions
{
    public class FailedNotificationClientException : Xeption
    {
        internal FailedNotificationClientException(string message, Exception innerException)
            : base(message, innerException)
        { }

        internal FailedNotificationClientException(string message, Exception innerException, IDictionary data)
            : base(message, innerException, data) { }
    }
}
