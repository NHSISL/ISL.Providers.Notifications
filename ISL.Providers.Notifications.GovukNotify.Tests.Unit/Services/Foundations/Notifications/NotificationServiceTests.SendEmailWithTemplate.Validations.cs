// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Notifications.GovukNotify.Models.Foundations.Notifications.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovukNotify.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldValidateArgumentsOnSendEmailWithTemplateIdAsync(string invalidText)
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
                values: "Text is required");

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
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldValidateDictionaryOnSendEmailWithTemplateIdAsync()
        {
            // given
            string inputToEmail = GetRandomEmailAddress();
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "message",
                values: "Text is required for dictionary item");

            invalidArgumentNotificationException.AddData(
                key: "subject",
                values: "Text is required for dictionary item");

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
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
