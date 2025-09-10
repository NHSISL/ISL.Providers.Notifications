using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using System.Collections;
using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a validation error occurs while using the notification provider.
    /// For example, if required data is missing or invalid.
    /// </summary>
    public class GovUkNotifyInterceptProviderValidationException : Xeption, INotificationProviderValidationException
    {
        public GovUkNotifyInterceptProviderValidationException(string message, Xeption innerException, IDictionary data)
            : base(message: message, innerException, data)
        { }
    }
}
