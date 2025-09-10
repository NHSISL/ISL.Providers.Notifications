using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a service error occurs while using the notification provider. 
    /// For example, if there is a problem with the server or any other service failure.
    /// </summary>
    public class GovUkNotifyInterceptProviderServiceException : Xeption, INotificationProviderServiceException
    {
        public GovUkNotifyInterceptProviderServiceException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
