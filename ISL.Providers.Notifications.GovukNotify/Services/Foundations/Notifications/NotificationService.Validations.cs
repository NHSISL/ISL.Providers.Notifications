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

        private async ValueTask ValidateOnSendEmailWithTemplateId(
            string toEmail,
            string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: await IsInvalid(toEmail), Parameter: nameof(toEmail)),
                (Rule: await IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: await IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private async ValueTask ValidateDictionaryOnSendEmailWithTemplateId(Dictionary<string, dynamic> personalisation)
        {
            string subject = GetValueOrNull(personalisation, "subject");
            string message = GetValueOrNull(personalisation, "message");

            Validate(
                (Rule: await IsInvalid(subject, true), Parameter: nameof(subject)),
                (Rule: await IsInvalid(message, true), Parameter: nameof(message)));
        }

        private async ValueTask ValidateOnSendSms(
            string templateId,
            string mobileNumber,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: await IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: await IsInvalidMobileNumber(mobileNumber), Parameter: nameof(mobileNumber)),
                (Rule: await IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private async ValueTask ValidateDictionaryOnSendSms(Dictionary<string, dynamic> personalisation)
        {
            string message = GetValueOrNull(personalisation, "message");

            Validate(
                (Rule: await IsInvalid(message, true), Parameter: nameof(message)));
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

        private static async ValueTask<dynamic> IsInvalidMobileNumber(string mobileNumber) {
            bool isInvalidLocalNumber = !Regex.IsMatch(mobileNumber, @"^07\d{9}$");
            bool isInvalidInternationalNumber = !Regex.IsMatch(mobileNumber, @"^\+447\d{9}$");

            return new
            {
                Condition = isInvalidLocalNumber && isInvalidInternationalNumber,

                Message = "Mobile number in dictionary item must begin with 07 or +447, "
                + "only contain numbers and be exactly 11 digits long"
            };
        }

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
