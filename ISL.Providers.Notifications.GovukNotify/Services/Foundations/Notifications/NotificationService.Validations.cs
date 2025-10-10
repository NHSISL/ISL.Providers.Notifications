// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions;

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
                (Rule: IsInvalidEmailAddress(toEmail), Parameter: nameof(toEmail)),
                (Rule: IsInvalid(subject), Parameter: nameof(subject)),
                (Rule: IsInvalid(body), Parameter: nameof(body)),
                (Rule: IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private static void ValidateOnSendEmailWithTemplateId(
            string toEmail,
            string templateId,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: IsInvalidEmailAddress(toEmail), Parameter: nameof(toEmail)),
                (Rule: IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: IsInvalid(personalisation), Parameter: nameof(personalisation)));
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

        private static void ValidateOnSendLetter(
            string templateId,
            string addressLine1,
            string addressLine2,
            string addressLine3,
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: IsInvalid(addressLine1), Parameter: nameof(addressLine1)),
                (Rule: IsInvalid(addressLine2), Parameter: nameof(addressLine2)),
                (Rule: IsInvalid(addressLine3), Parameter: nameof(addressLine3)));
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

        private static dynamic IsInvalidEmailAddress(string emailAddress)
        {
            bool isInvalidEmail;

            if (String.IsNullOrWhiteSpace(emailAddress))
            {
                isInvalidEmail = true;
            }
            else
            {
                isInvalidEmail = !Regex.IsMatch(emailAddress, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            }

            return new
            {
                Condition = isInvalidEmail,
                Message = "Email must be in format: XXX@XXX.XXX"
            };
        }

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
