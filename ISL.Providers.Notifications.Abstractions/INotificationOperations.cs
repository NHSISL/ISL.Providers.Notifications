// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.Abstractions
{
    public interface INotificationOperations
    {
        /// <summary>
        /// Sends an email to the specified email address with the specified
        /// subject, body and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent email.</returns>
        ValueTask<string> SendEmailAsync(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation);

        /// <summary>
        /// Sends an email to the specified email address using the specified
        /// template ID and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent email.</returns>
        ValueTask<string> SendEmailAsync(
            string templateId,
            string toEmail,
            Dictionary<string, dynamic> personalisation,
            string clientReference = null);

        /// <summary>
        /// Sends a SMS using the specified template ID and personalisation items.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent SMS.</returns>
        ValueTask<string> SendSmsAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation);

        /// <summary>
        /// Sends a letter using the specified template ID and personalisation contents.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent letter.</returns>
        ValueTask<string> SendLetterAsync(
            string templateId,
            Dictionary<string, dynamic> personalisation = null,
            string clientReference = null);

        /// <summary>
        /// Sends a letter using the specified template ID and PDF contents.
        /// </summary>
        /// <returns>A string representing the unique identifier of the sent letter.</returns>
        ValueTask<string> SendPrecompiledLetterAsync(
            string templateId,
            byte[] pdfContents,
            string postage = null);
    }
}
