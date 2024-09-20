// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions;
using Moq;

namespace ISL.Providers.Notifications.GovukNotify.Tests.Unit.Services.Foundations.Notifications
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
            string inputFromEmail = invalidText;
            string inputToEmail = invalidText;
            string inputSubject = invalidText;
            string inputBody = invalidText;
            string inputTemplateId = invalidText;
            string inputClientReference = invalidText;
            string inputEmailReplyToId = invalidText;
            string inputOneClickUnsubscribeURL = invalidText;
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "toEmail",
                values: "Text is required");

            invalidArgumentNotificationException.AddData(
                key: "subject",
                values: "Text is required");

            invalidArgumentNotificationException.AddData(
                key: "body",
                values: "Text is required");

            invalidArgumentNotificationException.AddData(
                key: "personalisation",
                values: "Dictionary is required");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask sendEmailTask = this.notificationService.SendEmailAsync(
                toEmail: inputToEmail,
                subject: inputSubject,
                body: inputBody,
                personalisation: inputPersonalization);

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
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldValidateDictionaryOnSendEmailAsync(string invalidText)
        {
            // given
            string inputFromEmail = GetRandomEmailAddress();
            string inputToEmail = GetRandomEmailAddress();
            string inputSubject = GetRandomString();
            string inputBody = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("inputClientReference", GetRandomString());

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "templateId",
                values: "Text is required for dictionary item");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask sendEmailTask = this.notificationService.SendEmailAsync(
                toEmail: inputToEmail,
                subject: inputSubject,
                body: inputBody,
                personalisation: inputPersonalization);

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
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
