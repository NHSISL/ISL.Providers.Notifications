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
    public class GovukNotifyProvider : IGovukNotifyProvider
    {
        private readonly NotifyConfigurations configurations;
        private INotificationService notificationService { get; set; }

        public GovukNotifyProvider(NotifyConfigurations configurations)
        {
            IServiceProvider serviceProvider = RegisterServices(configurations);
            InitializeClients(serviceProvider);
        }

        public async ValueTask SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation)
        {
            try
            {
                await this.notificationService.SendEmailAsync(toEmail, subject, body, personalisation);
            }
            catch (NotificationValidationException notificationValidationException)
            {
                throw CreateProviderValidationException(
                    notificationValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyValidationException notificationDependencyValidationException)
            {
                throw CreateProviderDependencyValidationException(
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

        public async ValueTask SendLetterAsync(string templateId, byte[] pdfContents, string postage = null)
        {
            try
            {
                await this.notificationService.SendLetterAsync(templateId, pdfContents, postage);
            }
            catch (NotificationValidationException notificationValidationException)
            {
                throw CreateProviderValidationException(
                    notificationValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyValidationException notificationDependencyValidationException)
            {
                throw CreateProviderDependencyValidationException(
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

        public async ValueTask SendSmsAsync(string templateId, Dictionary<string, dynamic> personalisation)
        {
            try
            {
                await this.notificationService.SendSmsAsync(templateId, personalisation);
            }
            catch (NotificationValidationException notificationValidationException)
            {
                throw CreateProviderValidationException(
                    notificationValidationException.InnerException as Xeption);
            }
            catch (NotificationDependencyValidationException notificationDependencyValidationException)
            {
                throw CreateProviderDependencyValidationException(
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
                message: "Gov.UK Notify provider validation error occurred, fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static GovUkNotifyProviderDependencyValidationException CreateProviderDependencyValidationException(
            Xeption innerException)
        {
            return new GovUkNotifyProviderDependencyValidationException(
                message: "Gov.UK Notify provider dependency validation error occurred, fix errors and try again.",
                innerException,
                data: innerException.Data);
        }

        private static GovUkNotifyProviderDependencyException CreateProviderDependencyException(
            Xeption innerException)
        {
            return new GovUkNotifyProviderDependencyException(
                message: "Gov.UK Notify provider dependency error occurred, contact support.",
                innerException);
        }

        private static GovUkNotifyProviderServiceException CreateProviderServiceException(Xeption innerException)
        {
            return new GovUkNotifyProviderServiceException(
                message: "Gov.UK Notify provider service error occurred, contact support.",
                innerException);
        }

        private void InitializeClients(IServiceProvider serviceProvider) =>
            notificationService = serviceProvider.GetRequiredService<INotificationService>();

        private static IServiceProvider RegisterServices(NotifyConfigurations configurations)
        {
            var serviceCollection = new ServiceCollection()
                .AddTransient<IGovukNotifyBroker, GovukNotifyBroker>()
                .AddTransient<INotificationService, NotificationService>()
                .AddSingleton(configurations);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
