// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Threading.Tasks;
using ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions;
using Xeptions;

namespace ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications
{
    internal partial class NotificationService
    {
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (InvalidArgumentNotificationException invalidArgumentNotificationException)
            {
                throw CreateAndLogValidationException(invalidArgumentNotificationException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "Can't send to this recipient using a team-only API key")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "Can't send to this recipient when service is in trial mode - " +
                        "see https://www.notifications.service.gov.uk/trial-mode")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "File did not pass the virus scan")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "Error: Your system clock must be accurate to within 30 seconds")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "Invalid token: API key not found")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "Exceeded rate limit for key type TEAM/TEST/LIVE of " +
                    "3000 requests per 60 seconds")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "Exceeded send limits (LIMIT NUMBER) for today")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "precompiledPDF must be a valid PDF file")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "reference cannot be null or empty")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "precompiledPDF cannot be null or empty")
            {
                var failedNotificationClientException = new FailedNotificationClientException(
                    message: "Notification client error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyValidationException(failedNotificationClientException);
            }
            catch (Notify.Exceptions.NotifyClientException notifyClientException)
                when (notifyClientException.Message == "Internal server error")
            {
                var failedNotificationServerException = new FailedNotificationServerException(
                    message: "Notification server error occurred, contact support.",
                    innerException: notifyClientException,
                    data: notifyClientException.Data);

                throw CreateDependencyException(failedNotificationServerException);
            }
            catch (Exception exception)
            {
                var failedNotificationServiceException =
                    new FailedNotificationServiceException(
                        message: "Failed notification service error occurred, please contact support.",
                        innerException: exception);

                throw CreateAndLogServiceException(failedNotificationServiceException);
            }
        }

        private NotificationValidationException CreateAndLogValidationException(Xeption exception)
        {
            var notificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: exception);

            return notificationValidationException;
        }

        private NotificationDependencyValidationException CreateDependencyValidationException(Xeption exception)
        {
            var meshDependencyValidationException =
                new NotificationDependencyValidationException(
                    message: "Notification dependency validation error occurred, contact support.",
                    innerException: exception);

            return meshDependencyValidationException;
        }

        private NotificationDependencyException CreateDependencyException(Xeption exception)
        {
            var meshDependencyException =
                new NotificationDependencyException(
                    message: "Notification dependency error occurred, contact support.",
                    innerException: exception);

            throw meshDependencyException;
        }

        private NotificationServiceException CreateAndLogServiceException(Xeption exception)
        {
            var notificationServiceException =
                new NotificationServiceException(
                    message: "Notification service error occurred, please contact support.",
                    innerException: exception);

            return notificationServiceException;
        }
    }
}
