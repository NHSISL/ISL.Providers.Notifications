// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications
{
    internal partial class NotificationService
    {
        private static void ValidateOnSendEmail(
            string toEmail,
            string subject,
            string body,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: IsInvalid(toEmail), Parameter: nameof(toEmail)),
                (Rule: IsInvalid(subject), Parameter: nameof(subject)),
                (Rule: IsInvalid(body), Parameter: nameof(body)),
                (Rule: IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private static void ValidateDictionaryOnSendEmail(Dictionary<string, dynamic> personalisation)
        {
            string subject = GetValueOrNull(personalisation, "subject");
            string body = GetValueOrNull(personalisation, "body");
            string templateId = GetValueOrNull(personalisation, "templateId");

            Validate(
                (Rule: IsInvalid(subject, true), Parameter: nameof(subject)),
                (Rule: IsInvalid(body, true), Parameter: nameof(body)),
                (Rule: IsInvalid(templateId, true), Parameter: nameof(templateId)));
        }

        private static void ValidateOnSendEmailWithTemplateId(
            string toEmail,
            string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: IsInvalid(toEmail), Parameter: nameof(toEmail)),
                (Rule: IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private static void ValidateDictionaryOnSendEmailWithTemplateId(Dictionary<string, dynamic> personalisation)
        {
            string subject = GetValueOrNull(personalisation, "subject");
            string message = GetValueOrNull(personalisation, "message");

            Validate(
                (Rule: IsInvalid(subject, true), Parameter: nameof(subject)),
                (Rule: IsInvalid(message, true), Parameter: nameof(message)));
        }

        private static void ValidateOnSendSms(
            string templateId,
            string mobileNumber,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: IsInvalidMobileNumber(mobileNumber), Parameter: nameof(mobileNumber)),
                (Rule: IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private static void ValidateDictionaryOnSendSms(Dictionary<string, dynamic> personalisation)
        {
            string message = GetValueOrNull(personalisation, "message");

            Validate(
                (Rule: IsInvalid(message, true), Parameter: nameof(message)));
        }

        private static void ValidateOnSendLetter(
            string templateId)
        {
            Validate(
                (Rule: IsInvalid(templateId), Parameter: nameof(templateId)));
        }

        private static dynamic IsInvalid(string text, bool isDictionaryValue = false) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = isDictionaryValue == false ? "Text is required" : "Text is required for dictionary item"
        };

        private static dynamic IsInvalidMobileNumber(string mobileNumber)
        {
            bool isInvalidLocalNumber = !Regex.IsMatch(mobileNumber, @"^07\d{9}$");
            bool isInvalidInternationalNumber = !Regex.IsMatch(mobileNumber, @"^\+447\d{9}$");

            return new
            {
                Condition = isInvalidLocalNumber && isInvalidInternationalNumber,

                Message = "Mobile number must be in UK format: 07XXXXXXXXX (11 digits) " +
                    "or international format: +447XXXXXXXXX (12 digits)"
            };
        }

        private static dynamic IsInvalid(Dictionary<string, dynamic> dictionary) => new
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
