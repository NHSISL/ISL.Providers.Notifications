// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Services.Foundations.Notifications
{
    internal partial class NotificationService
    {
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

        private static void ValidateDictionaryOnSendEmailWithTemplateId(
            Dictionary<string, dynamic> personalisation)
        {
            string subject = GetValueOrNull(personalisation, "subject");
            string message = GetValueOrNull(personalisation, "message");

            Validate(
                (Rule: IsInvalid(subject, true), Parameter: nameof(subject)),
                (Rule: IsInvalid(message, true), Parameter: nameof(message)));
        }

        private static void ValidateInterceptingEmailAsync(string interceptingEmail)
        {
            Validate((Rule: IsInvalid(interceptingEmail), Parameter: nameof(interceptingEmail)));
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
            Dictionary<string, dynamic> personalisation)
        {
            Validate(
                (Rule: IsInvalid(templateId), Parameter: nameof(templateId)),
                (Rule: IsInvalid(personalisation), Parameter: nameof(personalisation)));
        }

        private static void ValidateInterceptingMobileNumber(string interceptingMobileNumber)
        {
            Validate(
                (Rule: IsInvalidMobileNumber(interceptingMobileNumber),
                Parameter: nameof(interceptingMobileNumber)));
        }

        private static void ValidateInterceptingEmail(string interceptingEmail)
        {
            Validate(
                (Rule: IsInvalidEmailAddress(interceptingEmail), Parameter: nameof(interceptingEmail)));
        }

        private static void ValidateInterceptingAddressLines(List<string> interceptingAddressLines)
        {
            Validate(
                (Rule: IsInvalid(interceptingAddressLines),
                Parameter: nameof(interceptingAddressLines)));
        }

        private static dynamic IsInvalid(string text, bool isDictionaryValue = false) => new
        {
            Condition = String.IsNullOrWhiteSpace(text),
            Message = isDictionaryValue == false ? "Text is required" : "Text is required for dictionary item"
        };

        private static dynamic IsInvalid(Dictionary<string, dynamic> dictionary) => new
        {
            Condition = dictionary == null,
            Message = "Dictionary is required"
        };

        private static dynamic IsInvalid(List<string> list) => new
        {
            Condition = list == null || list.Count == 0,
            Message = "List is required and cannot be empty"
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
