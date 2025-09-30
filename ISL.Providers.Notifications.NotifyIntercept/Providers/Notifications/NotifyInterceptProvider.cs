// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.NotifyIntercept.Brokers;
using ISL.Providers.Notifications.NotifyIntercept.Models;
using ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications.Exceptions;
using ISL.Providers.Notifications.NotifyIntercept.Models.Providers.Exceptions;
using ISL.Providers.Notifications.NotifyIntercept.Services.Foundations.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace ISL.Providers.Notifications.NotifyIntercept.Providers.Notifications
{
    public class NotifyInterceptProvider : INotifyInterceptProvider
    {
        private INotificationService notificationService;

        public NotifyInterceptProvider(NotifyConfigurations configurations)
        {
            IServiceProvider serviceProvider = RegisterServices(configurations);
            InitializeClients(serviceProvider);
        }

        /// <summary>
        /// Sends an email to the specified email address using the specified
        /// template ID and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent email.</returns>
        /// <exception cref="GovUkNotifyProviderValidationException" />
        /// <exception cref="GovUkNotifyProviderDependencyException" />
        /// <exception cref="GovUkNotifyProviderServiceException" />
        public async ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation,
            string clientReference = null)
        {
            try
            {
                return await this.notificationService.SendEmailAsync(
                    templateId,
                    toEmail,
                    personalisation,
                    clientReference);
            }
            catch (NotificationValidationException notificationValidationException)
            {
                throw CreateProviderValidationException(
                    notificationValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyValidationException notificationDependencyValidationException)
            {
                throw CreateProviderValidationException(
                    notificationDependencyValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyException notificationDependencyException)
            {
                throw CreateProviderDependencyException(
                    notificationDependencyException.InnerException as Xeption);
            }
            catch (NotificationServiceException notificationServiceException)
            {
                throw CreateProviderServiceException(
                    notificationServiceException.InnerException as Xeption);
            }
        }

        public ValueTask<string> SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sends a SMS using the specified template ID and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent SMS.</returns>
        /// <exception cref="GovUkNotifyProviderValidationException" />
        /// <exception cref="GovUkNotifyProviderDependencyException" />
        /// <exception cref="GovUkNotifyProviderServiceException" />
        public async ValueTask<string> SendSmsAsync(
            string templateId,
            string mobileNumber,
            Dictionary<string, dynamic> personalisation)
        {
            try
            {
                return await this.notificationService.SendSmsAsync(
                    templateId,
                    mobileNumber,
                    personalisation);
            }
            catch (NotificationValidationException notificationValidationException)
            {
                throw CreateProviderValidationException(
                    notificationValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyValidationException notificationDependencyValidationException)
            {
                throw CreateProviderValidationException(
                    notificationDependencyValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyException notificationDependencyException)
            {
                throw CreateProviderDependencyException(
                    notificationDependencyException.InnerException as Xeption);
            }
            catch (NotificationServiceException notificationServiceException)
            {
                throw CreateProviderServiceException(
                    notificationServiceException.InnerException as Xeption);
            }
        }

        public async ValueTask<string> SendLetterAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null)
        {
            try
            {
                return await this.notificationService.SendLetterAsync(
                    templateId,
                    personalisation,
                    clientReference);
            }
            catch (NotificationValidationException notificationValidationException)
            {
                throw CreateProviderValidationException(
                    notificationValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyValidationException notificationDependencyValidationException)
            {
                throw CreateProviderValidationException(
                    notificationDependencyValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyException notificationDependencyException)
            {
                throw CreateProviderDependencyException(
                    notificationDependencyException.InnerException as Xeption);
            }
            catch (NotificationServiceException notificationServiceException)
            {
                throw CreateProviderServiceException(
                    notificationServiceException.InnerException as Xeption);
            }
        }

        public ValueTask<string> SendPrecompiledLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null)
        {
            throw new NotImplementedException();
        }

        private static GovUkNotifyInterceptProviderValidationException CreateProviderValidationException(
            Xeption innerException)
        {
            return new GovUkNotifyInterceptProviderValidationException(
                message: "Gov.UK Notify Intercept provider validation error occurred, fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static GovUkNotifyInterceptProviderDependencyException CreateProviderDependencyException(
            Xeption innerException)
        {
            return new GovUkNotifyInterceptProviderDependencyException(
                message: "Gov.UK Notify Intercept provider dependency error occurred, contact support.",
                innerException);
        }

        private static GovUkNotifyInterceptProviderServiceException CreateProviderServiceException(
            Xeption innerException)
        {
            return new GovUkNotifyInterceptProviderServiceException(
                message: "Gov.UK Notify Intercept provider service error occurred, contact support.",
                innerException);
        }

        private void InitializeClients(IServiceProvider serviceProvider) =>
            notificationService = serviceProvider.GetRequiredService<INotificationService>();

        private static IServiceProvider RegisterServices(NotifyConfigurations configurations)
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IGovUkNotifyBroker, GovUkNotifyBroker>()
                .AddTransient<INotificationService, NotificationService>()
                .AddSingleton(configurations);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
