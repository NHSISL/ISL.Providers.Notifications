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
        public async Task ShouldValidateArgumentsOnSendLetterAsync(string invalidText)
        {
            // given
            string inputTemplateId = invalidText;
            string inputAddressLine1 = GetRandomString();
            string inputAddressLine2 = GetRandomString();
            string inputAddressLine3 = GetRandomString();
            string inputAddressLine4 = GetRandomString();
            string inputAddressLine5 = GetRandomString();
            string inputAddressLine6 = GetRandomString();
            string inputAddressLine7 = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = null;

            var invalidArgumentNotificationException =
                new InvalidArgumentNotificationException(
                    message: "Invalid notification argument exception. Please correct the errors and try again.");

            invalidArgumentNotificationException.AddData(
                key: "templateId",
                values: "Text is required");

            var expectedNotificationValidationException =
                new NotificationValidationException(
                    message: "Notification validation error occurred, please correct the errors and try again.",
                    innerException: invalidArgumentNotificationException);

            // when
            ValueTask<string> sendLetterTask = this.notificationService.SendLetterAsync(
                templateId: inputTemplateId,
                addressLine1: inputAddressLine1,
                addressLine2: inputAddressLine2,
                addressLine3: inputAddressLine3,
                addressLine4: inputAddressLine4,
                addressLine5: inputAddressLine5,
                addressLine6: inputAddressLine6,
                addressLine7: inputAddressLine7,
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
