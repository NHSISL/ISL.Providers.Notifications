// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.NotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Theory]
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnSendSms(Exception dependancyValidationException)
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputSmsSenderId = GetRandomString();
            string inputMessage = GetRandomString();
            string inputMobileNumber = GetRandomLocalMobileNumber();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("smsSenderId", inputSmsSenderId);
            inputPersonalization.Add("message", inputMessage);

            this.govukNotifyBroker.Setup(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
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
            ValueTask<string> sendEmailTask = this.notificationService.SendSmsAsync(
                templateId: inputTemplateId,
                mobileNumber: inputMobileNumber,
                personalisation: inputPersonalization);

            NotificationDependencyValidationException actualNotificationDependencyValidationException =
                await Assert.ThrowsAsync<NotificationDependencyValidationException>(sendEmailTask.AsTask);

            // then
            actualNotificationDependencyValidationException.Should()
                 .BeEquivalentTo(expectedNotificationDependencyValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnSendSms(Exception dependancyException)
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputSmsSenderId = GetRandomString();
            string inputMessage = GetRandomString();
            string inputMobileNumber = GetRandomLocalMobileNumber();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("smsSenderId", inputSmsSenderId);
            inputPersonalization.Add("message", inputMessage);

            this.govukNotifyBroker.Setup(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
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
            ValueTask<string> sendEmailTask = this.notificationService.SendSmsAsync(
                templateId: inputTemplateId,
                mobileNumber: inputMobileNumber,
                personalisation: inputPersonalization);

            NotificationDependencyException actualNotificationDependencyValidationException =
                await Assert.ThrowsAsync<NotificationDependencyException>(sendEmailTask.AsTask);

            // then
            actualNotificationDependencyValidationException.Should()
                 .BeEquivalentTo(expectedNotificationDependencyException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnSendSms()
        {
            // given
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputSmsSenderId = GetRandomString();
            string inputMessage = GetRandomString();
            string inputMobileNumber = GetRandomLocalMobileNumber();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("smsSenderId", inputSmsSenderId);
            inputPersonalization.Add("message", inputMessage);
            var serviceException = new Exception();

            this.govukNotifyBroker.Setup(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
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
            ValueTask<string> sendEmailTask = this.notificationService.SendSmsAsync(
                templateId: inputTemplateId,
                mobileNumber: inputMobileNumber,
                personalisation: inputPersonalization);

            NotificationServiceException actualNotificationServiceException =
                await Assert.ThrowsAsync<NotificationServiceException>(sendEmailTask.AsTask);

            // then
            actualNotificationServiceException.Should()
                 .BeEquivalentTo(expectedNotificationServiceException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendSmsAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
