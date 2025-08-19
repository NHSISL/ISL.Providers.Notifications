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
            string inputMobileNumber = GetRandomMobileNumber();
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
        [InlineData("0123456789")]
        [InlineData("01234abc890")]
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
                values: "Mobile number in dictionary item must begin with 0, " +
                    "only contain numbers and be exactly 11 digits long");

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
            string inputMobileNumber = GetRandomMobileNumber();
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
