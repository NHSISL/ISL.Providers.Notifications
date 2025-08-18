// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions;

namespace ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications
{
    internal partial class NotificationService
    {
        private async ValueTask ValidateOnSendEmail(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: await IsInvalid(toEmail), Parameter: nameof(toEmail)),
                (Rule: await IsInvalid(subject), Parameter: nameof(subject)),
                (Rule: await IsInvalid(body), Parameter: nameof(body)),
                (Rule: await IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private async ValueTask ValidateDictionaryOnSendEmail(Dictionary<string, dynamic> personalisation)
        {
            string subject = GetValueOrNull(personalisation, "subject");
            string body = GetValueOrNull(personalisation, "body");
            string templateId = GetValueOrNull(personalisation, "templateId");

            Validate(
                (Rule: await IsInvalid(subject, true), Parameter: nameof(subject)),
                (Rule: await IsInvalid(body, true), Parameter: nameof(body)),
                (Rule: await IsInvalid(templateId, true), Parameter: nameof(templateId)));
        }

        private async ValueTask ValidateOnSendLetter(
            string templateId)
        {
            Validate(
                (Rule: await IsInvalid(templateId), Parameter: nameof(templateId)));
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
