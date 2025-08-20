// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.Abstractions
{
    public partial class NotificationAbstractionProvider : INotificationAbstractionProvider
    {
        private readonly INotificationProvider notificationProvider;

        public NotificationAbstractionProvider(INotificationProvider notificationProvider) =>
            this.notificationProvider = notificationProvider;

        /// <summary>
        /// Sends an email to the specified email address with the specified
        /// subject, body and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent email.</returns>
        /// <exception cref="NotificationValidationProviderException" />
        /// <exception cref="NotificationDependencyProviderException" />
        /// <exception cref="NotificationServiceProviderException" />
        public ValueTask<string> SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation) =>
            TryCatch(async () =>
            {
                return await this.notificationProvider.SendEmailAsync(toEmail, subject, body, personalisation);
            });

        /// <summary>
        /// Sends an email to the specified email address using the specified
        /// template ID and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent email.</returns>
        /// <exception cref="NotificationValidationProviderException" />
        /// <exception cref="NotificationDependencyProviderException" />
        /// <exception cref="NotificationServiceProviderException" />
        public ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation,
            string clientReference = null) =>
            TryCatch(async () =>
            {
                return await this.notificationProvider.SendEmailAsync(
                    templateId, 
                    toEmail, 
                    personalisation, 
                    clientReference);
            });

        /// <summary>
        /// Sends a SMS using the specified template ID and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent SMS.</returns>
        /// <exception cref="NotificationValidationProviderException" />
        /// <exception cref="NotificationDependencyProviderException" />
        /// <exception cref="NotificationServiceProviderException" />
        public ValueTask<string> SendSmsAsync(
            string templateId,
            string mobileNumber,
            Dictionary<string, dynamic> personalisation) =>
            TryCatch(async () =>
            {
                return await this.notificationProvider.SendSmsAsync(templateId, mobileNumber, personalisation);
            });

        /// <summary>
        /// Sends a letter using the specified template ID and personalisation contents.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent letter.</returns>
        /// <exception cref="NotificationValidationProviderException" />
        /// <exception cref="NotificationDependencyProviderException" />
        /// <exception cref="NotificationServiceProviderException" />
        public ValueTask<string> SendLetterAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null) =>
        TryCatch(async () =>
        {
            return await this.notificationProvider.SendLetterAsync(templateId, personalisation, clientReference);
        });

        /// <summary>
        /// Sends a letter using the specified template ID and PDF contents.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent letter.</returns>
        /// <exception cref="NotificationValidationProviderException" />
        /// <exception cref="NotificationDependencyProviderException" />
        /// <exception cref="NotificationServiceProviderException" />
        public ValueTask<string> SendPrecompiledLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null) =>
        TryCatch(async () =>
        {
            return await this.notificationProvider.SendPrecompiledLetterAsync(templateId, pdfContents, postage);
        });
    }
}
