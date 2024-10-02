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
            catch (Xeption ex) when (ex is INotificationProviderValidationException)
            {
                throw CreateValidationException(ex);
            }
            catch (Xeption ex) when (ex is INotificationProviderDependencyValidationException)
            {
                throw CreateValidationException(ex);
            }
            catch (Xeption ex) when (ex is INotificationProviderDependencyException)
            {
                throw CreateDependencyException(ex);
            }
            catch (Xeption ex) when (ex is INotificationProviderServiceException)
            {
                throw CreateServiceException(ex);
            }
            catch (Exception ex)
            {
                var uncatagorizedNotificationProviderException =
                    new UncatagorizedNotificationProviderException(
                        message: "Notification provider not properly implemented. Uncatagorized errors found, " +
                            "contact the notification provider owner for support.",
                        innerException: ex,
                        data: ex.Data);

                throw CreateUncatagorizedServiceException(uncatagorizedNotificationProviderException);
            }
        }

        private NotificationValidationProviderException CreateValidationException(
            Xeption exception)
        {
            var notificationValidationProviderException =
                new NotificationValidationProviderException(
                    message: "Notification validation errors occurred, please try again.",
                    innerException: exception,
                    data: exception.Data);

            return notificationValidationProviderException;
        }

        private NotificationDependencyProviderException CreateDependencyException(
            Xeption exception)
        {
            var notificationDependencyProviderException = new NotificationDependencyProviderException(
                message: "Notification dependency error occurred, contact support.",
                innerException: exception,
                data: exception.Data);

            return notificationDependencyProviderException;
        }

        private NotificationServiceProviderException CreateServiceException(
            Xeption exception)
        {
            var notificationServiceProviderException = new NotificationServiceProviderException(
                message: "Notification service error occurred, contact support.",
                innerException: exception,
                data: exception.Data);

            return notificationServiceProviderException;
        }

        private NotificationServiceProviderException CreateUncatagorizedServiceException(
            Exception exception)
        {
            var notificationServiceProviderException = new NotificationServiceProviderException(
                message: "Uncatagorized notification service error occurred, contact support.",
                innerException: exception as Xeption,
                data: exception.Data);

            return notificationServiceProviderException;
        }
    }
}
