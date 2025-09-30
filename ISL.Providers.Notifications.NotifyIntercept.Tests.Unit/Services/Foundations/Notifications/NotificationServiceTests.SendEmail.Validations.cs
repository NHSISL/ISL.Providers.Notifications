// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Notifications.NotifyIntercept.Models;
using ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.NotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldValidateArgumentsOnSendEmailAsync(string invalidText)
        {
            // given
            string inputToEmail = invalidText;
            string inputTemplateId = invalidText;
            string inputClientReference = invalidText;
            Dictionary<string, dynamic> inputPersonalization = null;

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "toEmail",
                values: "Email must be in format: XXX@XXX.XXX");

            invalidArgumentNotificationException.AddData(
                key: "templateId",
                values: "Text is required");

            invalidArgumentNotificationException.AddData(
                key: "personalisation",
                values: "Dictionary is required");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask<string> sendEmailTask = this.notificationService.SendEmailAsync(
                templateId: inputTemplateId,
                toEmail: inputToEmail,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            NotificationValidationException actualNotificationValidationException =
                await Assert.ThrowsAsync<NotificationValidationException>(async () =>
                    await sendEmailTask);

            // then
            actualNotificationValidationException.Should()
                .BeEquivalentTo(expectedNotificationValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("test")]
        [InlineData("test@")]
        [InlineData("test@domain")]
        [InlineData("@domain.com")]
        [InlineData("test.com")]
        public async Task ShouldValidateConfigurationsOnSendEmailAsync(string invalidText)
        {
            // given
            string inputToEmail = GetRandomEmailAddress();
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputSubject = GetRandomString();
            string inputMessage = GetRandomString();
            string inputEmailReplyToId = GetRandomString();
            string inputOneClickUnsubscribeURL = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("subject", inputSubject);
            inputPersonalization.Add("message", inputMessage);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("oneClickUnsubscribeURL", inputOneClickUnsubscribeURL);
            this.configurations.DefaultOverride.Email = invalidText;

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "Email",
                values: "Email must be in format: XXX@XXX.XXX");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask<string> sendEmailTask = this.notificationService.SendEmailAsync(
                templateId: inputTemplateId,
                toEmail: inputToEmail,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            NotificationValidationException actualNotificationValidationException =
                await Assert.ThrowsAsync<NotificationValidationException>(async () =>
                    await sendEmailTask);

            // then
            actualNotificationValidationException.Should()
                .BeEquivalentTo(expectedNotificationValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldValidateConfigurationsOnSendEmailAsyncWithInvalidNotificationOverrides(
            string invalidText)
        {
            // given
            string inputToEmail = GetRandomEmailAddress();
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputSubject = GetRandomString();
            string inputMessage = GetRandomString();
            string inputEmailReplyToId = GetRandomString();
            string inputOneClickUnsubscribeURL = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("subject", inputSubject);
            inputPersonalization.Add("message", inputMessage);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("oneClickUnsubscribeURL", inputOneClickUnsubscribeURL);
            NotificationOverride randomInvalidNotificationOverride = GetRandomNotificationOverride();
            randomInvalidNotificationOverride.Identifier = invalidText;

            this.configurations.NotificationOverrides = new List<NotificationOverride>
            {
                randomInvalidNotificationOverride
            };

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "NotificationOverrides[0].Identifier",
                values: "Text is required");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask<string> sendEmailTask = this.notificationService.SendEmailAsync(
                templateId: inputTemplateId,
                toEmail: inputToEmail,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            NotificationValidationException actualNotificationValidationException =
                await Assert.ThrowsAsync<NotificationValidationException>(async () =>
                    await sendEmailTask);

            // then
            actualNotificationValidationException.Should()
                .BeEquivalentTo(expectedNotificationValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
