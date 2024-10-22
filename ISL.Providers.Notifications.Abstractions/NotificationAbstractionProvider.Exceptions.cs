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

        private NotificationProviderValidationException CreateValidationException(
            Xeption exception)
        {
            var notificationValidationProviderException =
                new NotificationProviderValidationException(
                    message: "Notification validation errors occurred, please try again.",
                    innerException: exception,
                    data: exception.Data);

            return notificationValidationProviderException;
        }

        private NotificationProviderDependencyException CreateDependencyException(
            Xeption exception)
        {
            var notificationDependencyProviderException = new NotificationProviderDependencyException(
                message: "Notification dependency error occurred, contact support.",
                innerException: exception,
                data: exception.Data);

            return notificationDependencyProviderException;
        }

        private NotificationProviderServiceException CreateServiceException(
            Xeption exception)
        {
            var notificationServiceProviderException = new NotificationProviderServiceException(
                message: "Notification service error occurred, contact support.",
                innerException: exception,
                data: exception.Data);

            return notificationServiceProviderException;
        }

        private NotificationProviderServiceException CreateUncatagorizedServiceException(
            Exception exception)
        {
            var notificationServiceProviderException = new NotificationProviderServiceException(
                message: "Uncatagorized notification service error occurred, contact support.",
                innerException: exception as Xeption,
                data: exception.Data);

            return notificationServiceProviderException;
        }
    }
}
