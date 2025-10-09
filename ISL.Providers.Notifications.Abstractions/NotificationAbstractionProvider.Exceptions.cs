// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.Abstractions
{
    public partial class NotificationAbstractionProvider
    {
        private delegate ValueTask<string> ReturningStringFunction();

        private async ValueTask<string> TryCatch(ReturningStringFunction returningStringFunction)
        {
            try
            {
                return await returningStringFunction();
            }
            catch (Xeption exception) when (exception is INotificationProviderValidationException)
            {
                throw CreateValidationException(exception);
            }
            catch (Xeption exception) when (exception is INotificationProviderDependencyException)
            {
                throw CreateDependencyException(exception);
            }
            catch (Xeption exception) when (exception is INotificationProviderServiceException)
            {
                throw CreateServiceException(exception);
            }
            catch (Exception exception)
            {
                throw CreateUncatagorizedServiceException(exception);
            }
        }

        private NotificationProviderValidationException CreateValidationException(
            Xeption exception)
        {
            var notificationValidationProviderException =
                new NotificationProviderValidationException(
                    message: exception.Message,
                    innerException: exception,
                    data: exception.Data);

            return notificationValidationProviderException;
        }

        private NotificationProviderDependencyException CreateDependencyException(
            Xeption exception)
        {
            var notificationDependencyProviderException = new NotificationProviderDependencyException(
                message: exception.Message,
                innerException: exception,
                data: exception.Data);

            return notificationDependencyProviderException;
        }

        private NotificationProviderServiceException CreateServiceException(
            Xeption exception)
        {
            var notificationServiceProviderException = new NotificationProviderServiceException(
                message: exception.Message,
                innerException: exception,
                data: exception.Data);

            return notificationServiceProviderException;
        }

        private NotificationProviderServiceException CreateUncatagorizedServiceException(
            Exception exception)
        {
            var notificationServiceProviderException = new NotificationProviderServiceException(
                message: "Notification provider not properly implemented. Uncatagorized errors found, " +
                    "contact the notification provider owner for support.",
                innerException: exception as Xeption,
                data: exception.Data);

            return notificationServiceProviderException;
        }
    }
}
