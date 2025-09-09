// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Services.Foundations.Notifications
{
    internal partial class NotificationService
    {
        private async ValueTask ValidateOnSendEmailWithTemplateIdAsync(
            string toEmail,
            string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: await IsInvalid(toEmail), Parameter: nameof(toEmail)),
                (Rule: await IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: await IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private async ValueTask ValidateDictionaryOnSendEmailWithTemplateIdAsync(
            Dictionary<string, dynamic> personalisation)
        {
            string subject = GetValueOrNull(personalisation, "subject");
            string message = GetValueOrNull(personalisation, "message");

            Validate(
                (Rule: await IsInvalid(subject, true), Parameter: nameof(subject)),
                (Rule: await IsInvalid(message, true), Parameter: nameof(message)));
        }

        private async ValueTask ValidateInterceptingEmailAsync(string interceptingEmail)
        {
            Validate((Rule: await IsInvalid(interceptingEmail), Parameter: nameof(interceptingEmail)));
        }

        private static async ValueTask<dynamic> IsInvalid(string text, bool isDictionaryValue = false) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = isDictionaryValue == false ? "Text is required" : "Text is required for dictionary item"
        };

        private static async ValueTask<dynamic> IsInvalid(Dictionary<string, dynamic> dictionary) => new
        {
            Condition = dictionary == null,
            Message = "Dictionary is required"
        };

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidArgumentNotificationException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }

            invalidArgumentNotificationException.ThrowIfContainsErrors();
        }
    }
}
