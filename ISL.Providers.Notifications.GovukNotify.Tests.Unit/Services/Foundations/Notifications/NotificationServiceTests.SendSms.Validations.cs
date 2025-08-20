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
        public async Task ShouldValidateArgumentsOnSendSmsAsync(string invalidText)
        {
            // given
            string inputTemplateId = invalidText;
            string inputMobileNumber = GetRandomLocalMobileNumber();
            Dictionary<string, dynamic> inputPersonalization = null;

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

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
            ValueTask<string> sendSmsTask = this.notificationService.SendSmsAsync(
                templateId: inputTemplateId,
                mobileNumber: inputMobileNumber,
                personalisation: inputPersonalization);

            NotificationValidationException actualNotificationValidationException =
                await Assert.ThrowsAsync<NotificationValidationException>(async () =>
                    await sendSmsTask);

            // then
            actualNotificationValidationException.Should()
                .BeEquivalentTo(expectedNotificationValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("12345678901")]
        [InlineData("0723456789")]
        [InlineData("07234abc890")]
        [InlineData("+447234abc890")]
        public async Task ShouldValidateMobileNumberOnSendSmsAsync(string invalidMobileNumber)
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputMessage = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("message", inputMessage);

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "mobileNumber",
                values: "Mobile number must be in UK format: 07XXXXXXXXX (11 digits) " +
                    "or international format: +447XXXXXXXXX (12 digits)");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask<string> sendSmsTask = this.notificationService.SendSmsAsync(
                templateId: inputTemplateId,
                mobileNumber: invalidMobileNumber,
                personalisation: inputPersonalization);

            NotificationValidationException actualNotificationValidationException =
                await Assert.ThrowsAsync<NotificationValidationException>(async () =>
                    await sendSmsTask);

            // then
            actualNotificationValidationException.Should()
                .BeEquivalentTo(expectedNotificationValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldValidateDictionaryOnSendSmsAsync()
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputMobileNumber = GetRandomLocalMobileNumber();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("inputClientReference", GetRandomString());

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "message",
                values: "Text is required for dictionary item");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask<string> sendSmsTask = this.notificationService.SendSmsAsync(
                templateId: inputTemplateId,
                mobileNumber: inputMobileNumber,
                personalisation: inputPersonalization);

            NotificationValidationException actualNotificationValidationException =
                await Assert.ThrowsAsync<NotificationValidationException>(async () =>
                    await sendSmsTask);

            // then
            actualNotificationValidationException.Should()
                .BeEquivalentTo(expectedNotificationValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
