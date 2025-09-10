using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Providers.Exceptions
{
    /// <summary>
    /// This exception is thrown when a dependency error occurs while using the notification provider.
    /// For example, if a required dependency is unavailable or incompatible.
    /// </summary>
    public class GovUkNotifyInterceptProviderDependencyException : Xeption, INotificationProviderDependencyException
    {
        public GovUkNotifyInterceptProviderDependencyException(string message, Xeption innerException)
            : base(message, innerException)
        { }
    }
}
