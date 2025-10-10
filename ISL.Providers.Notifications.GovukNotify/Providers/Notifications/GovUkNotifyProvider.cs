// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.GovukNotify.Brokers;
using ISL.Providers.Notifications.GovukNotify.Models;
using ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions;
using ISL.Providers.Notifications.GovukNotify.Models.Providers.Exceptions;
using ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Xeptions;

namespace ISL.Providers.Notifications.GovukNotify.Providers.Notifications
{
    public class GovUkNotifyProvider : IGovUkNotifyProvider
    {
        private INotificationService notificationService { get; set; }

        public GovUkNotifyProvider(NotifyConfigurations configurations)
        {
            IServiceProvider serviceProvider = RegisterServices(configurations);
            InitializeClients(serviceProvider);
        }

        /// <summary>
        /// Sends an email to the specified email address with the specified
        /// subject, body and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent email.</returns>
        /// <exception cref="GovUkNotifyProviderValidationException" />
        /// <exception cref="GovUkNotifyProviderDependencyException" />
        /// <exception cref="GovUkNotifyProviderServiceException" />
        public async ValueTask<string> SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation)
        {
            try
            {
                return await this.notificationService.SendEmailAsync(toEmail, subject, body, personalisation);
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
                return await this.notificationService.SendSmsAsync(templateId, mobileNumber, personalisation);
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

        /// <summary>
        /// Sends a letter using the specified template ID and personalisation content.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent letter.</returns>
        /// <exception cref="GovUkNotifyProviderValidationException" />
        /// <exception cref="GovUkNotifyProviderDependencyException" />
        /// <exception cref="GovUkNotifyProviderServiceException" />
        public async ValueTask<string> SendLetterAsync(
            string templateId,
            string recipientName,
            string addressLine1,
            string addressLine2,
            string addressLine3,
            string addressLine4,
            string addressLine5,
            string postCode,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null)
        {
            try
            {
                return await this.notificationService.SendLetterAsync(
                    templateId,
                    recipientName,
                    addressLine1,
                    addressLine2,
                    addressLine3,
                    addressLine4,
                    addressLine5,
                    postCode,
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

        /// <summary>
        /// Sends a letter using the specified template ID and PDF contents.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent letter.</returns>
        /// <exception cref="GovUkNotifyProviderValidationException" />
        /// <exception cref="GovUkNotifyProviderDependencyException" />
        /// <exception cref="GovUkNotifyProviderServiceException" />
        public async ValueTask<string> SendPrecompiledLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null)
        {
            try
            {
                return await this.notificationService.SendPrecompiledLetterAsync(templateId, pdfContents, postage);
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

        private static GovUkNotifyProviderValidationException CreateProviderValidationException(
            Xeption innerException)
        {
            return new GovUkNotifyProviderValidationException(
                message: innerException.Message,
                innerException,
                data: innerException.Data);
        }

        private static GovUkNotifyProviderDependencyException CreateProviderDependencyException(
            Xeption innerException)
        {
            return new GovUkNotifyProviderDependencyException(
                message: innerException.Message,
                innerException,
                data: innerException.Data);
        }

        private static GovUkNotifyProviderServiceException CreateProviderServiceException(Xeption innerException)
        {
            return new GovUkNotifyProviderServiceException(
                message: innerException.Message,
                innerException,
                data: innerException.Data);
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
