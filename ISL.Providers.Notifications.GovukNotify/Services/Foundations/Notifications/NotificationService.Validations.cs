// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        private async ValueTask ValidateOnSendSms(
            string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: await IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: await IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private async ValueTask ValidateDictionaryOnSendSms(Dictionary<string, dynamic> personalisation)
        {
            string mobileNumber = GetValueOrNull(personalisation, "mobileNumber");

            Validate(
                (Rule: await IsInvalidMobileNumber(mobileNumber), Parameter: nameof(mobileNumber)));
        }

        private static async ValueTask<dynamic> IsInvalid(string text, bool isDictionaryValue = false) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = isDictionaryValue == false ? "Text is required" : "Text is required for dictionary item"
        };

        private static async ValueTask<dynamic> IsInvalidMobileNumber(string mobileNumber) => new
        {
            Condition = !Regex.IsMatch(mobileNumber, @"^0\d{10}$"),

            Message = "Mobile number in dictionary item must begin with 0, "
                + "only contain numbers and be exactly 11 digits long"
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
