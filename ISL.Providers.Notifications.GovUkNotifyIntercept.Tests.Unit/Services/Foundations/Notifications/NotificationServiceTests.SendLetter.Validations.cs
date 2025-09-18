// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task ShouldValidateArgumentsOnSendLetterAsync(string invalidText)
        {
            // given
            string inputTemplateId = invalidText;
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
            ValueTask<string> sendLetterTask = this.notificationService.SendLetterAsync(
                templateId: inputTemplateId,
                personalisation: inputPersonalization);

            NotificationValidationException actualNotificationValidationException =
                await Assert.ThrowsAsync<NotificationValidationException>(async () =>
                    await sendLetterTask);

            // then
            actualNotificationValidationException.Should()
                .BeEquivalentTo(expectedNotificationValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendLetterAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidLists))]
        public async Task ShouldValidateInterceptingAddressOnSendLetterAsync(List<string> invalidInterceptingAddress)
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputMessage = GetRandomString();
            string inputMobileNumber = GetRandomLocalMobileNumber();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            this.configurations.InterceptingAddressLines = invalidInterceptingAddress;

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "interceptingAddressLines",
                values: "List is required and cannot be empty");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask<string> sendLetterTask = this.notificationService.SendLetterAsync(
                templateId: inputTemplateId,
                personalisation: inputPersonalization);

            NotificationValidationException actualNotificationValidationException =
                await Assert.ThrowsAsync<NotificationValidationException>(async () =>
                    await sendLetterTask);

            // then
            actualNotificationValidationException.Should()
                .BeEquivalentTo(expectedNotificationValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendLetterAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()),
                Times.Never);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
