// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications.Exceptions;
using Moq;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnSendLetter(Exception dependancyValidationException)
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();

            this.govukNotifyBroker.Setup(broker =>
                broker.SendLetterAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()))
                .Throws(dependancyValidationException);

            var failedNotificationClientException = new FailedNotificationClientException(
                message: "Notification client error occurred, contact support.",
                innerException: dependancyValidationException,
                data: dependancyValidationException.Data);

            var expectedNotificationDependencyValidationException =
                new NotificationDependencyValidationException(
                    message: "Notification dependency validation error occurred, contact support.",
                    innerException: failedNotificationClientException);

            // when
            ValueTask<string> sendLetterTask = this.notificationService.SendLetterAsync(
                templateId: inputTemplateId,
                personalisation: inputPersonalization);

            NotificationDependencyValidationException actualNotificationDependencyValidationException =
                await Assert.ThrowsAsync<NotificationDependencyValidationException>(sendLetterTask.AsTask);

            // then
            actualNotificationDependencyValidationException.Should()
                 .BeEquivalentTo(expectedNotificationDependencyValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendLetterAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnSendLetter(Exception dependancyException)
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();

            this.govukNotifyBroker.Setup(broker =>
                broker.SendLetterAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()))
                .Throws(dependancyException);

            var failedNotificationServerException = new FailedNotificationServerException(
                message: "Notification server error occurred, contact support.",
                innerException: dependancyException,
                data: dependancyException.Data);

            var expectedNotificationDependencyException =
                new NotificationDependencyException(
                    message: "Notification dependency error occurred, contact support.",
                    innerException: failedNotificationServerException);

            // when
            ValueTask<string> sendLetterTask = this.notificationService.SendLetterAsync(
                templateId: inputTemplateId,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            NotificationDependencyException actualNotificationDependencyValidationException =
                await Assert.ThrowsAsync<NotificationDependencyException>(sendLetterTask.AsTask);

            // then
            actualNotificationDependencyValidationException.Should()
                 .BeEquivalentTo(expectedNotificationDependencyException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendLetterAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnSendLetter()
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            var serviceException = new Exception();

            this.govukNotifyBroker.Setup(broker =>
                broker.SendLetterAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()))
                .Throws(serviceException);

            var failedNotificationServiceException = new FailedNotificationServiceException(
                message: "Failed notification service error occurred, please contact support.",
                innerException: serviceException);

            var expectedNotificationServiceException =
                new NotificationServiceException(
                    message: "Notification service error occurred, please contact support.",
                    innerException: failedNotificationServiceException);

            // when
            ValueTask<string> sendLetterTask = this.notificationService.SendLetterAsync(
                templateId: inputTemplateId,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            NotificationServiceException actualNotificationServiceException =
                await Assert.ThrowsAsync<NotificationServiceException>(sendLetterTask.AsTask);

            // then
            actualNotificationServiceException.Should()
                 .BeEquivalentTo(expectedNotificationServiceException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendLetterAsync(
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
