// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
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
        [MemberData(nameof(DependencyValidationExceptions))]
        public async Task ShouldThrowDependencyValidationExceptionOnSendEmail(Exception dependancyValidationException)
        {
            // given
            string inputToEmail = GetRandomEmailAddress();
            string inputSubject = GetRandomString();
            string inputBody = GetRandomString();
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputEmailReplyToId = GetRandomString();
            string inputOneClickUnsubscribeURL = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("templateId", inputTemplateId);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("oneClickUnsubscribeURL", inputOneClickUnsubscribeURL);

            this.govukNotifyBroker.Setup(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
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
            ValueTask sendEmailTask = this.notificationService.SendEmailAsync(
                toEmail: inputToEmail,
                subject: inputSubject,
                body: inputBody,
                personalisation: inputPersonalization);

            NotificationDependencyValidationException actualNotificationDependencyValidationException =
                await Assert.ThrowsAsync<NotificationDependencyValidationException>(sendEmailTask.AsTask);

            // then
            actualNotificationDependencyValidationException.Should()
                 .BeEquivalentTo(expectedNotificationDependencyValidationException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(DependencyExceptions))]
        public async Task ShouldThrowDependencyExceptionOnSendEmail(Exception dependancyException)
        {
            // given
            string inputToEmail = GetRandomEmailAddress();
            string inputSubject = GetRandomString();
            string inputBody = GetRandomString();
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputEmailReplyToId = GetRandomString();
            string inputOneClickUnsubscribeURL = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("templateId", inputTemplateId);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("oneClickUnsubscribeURL", inputOneClickUnsubscribeURL);

            this.govukNotifyBroker.Setup(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
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
            ValueTask sendEmailTask = this.notificationService.SendEmailAsync(
                toEmail: inputToEmail,
                subject: inputSubject,
                body: inputBody,
                personalisation: inputPersonalization);

            NotificationDependencyException actualNotificationDependencyValidationException =
                await Assert.ThrowsAsync<NotificationDependencyException>(sendEmailTask.AsTask);

            // then
            actualNotificationDependencyValidationException.Should()
                 .BeEquivalentTo(expectedNotificationDependencyException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnSendEmail()
        {
            // given
            string inputToEmail = GetRandomEmailAddress();
            string inputSubject = GetRandomString();
            string inputBody = GetRandomString();
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputEmailReplyToId = GetRandomString();
            string inputOneClickUnsubscribeURL = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("templateId", inputTemplateId);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("oneClickUnsubscribeURL", inputOneClickUnsubscribeURL);
            var serviceException = new Exception();

            this.govukNotifyBroker.Setup(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
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
            ValueTask sendEmailTask = this.notificationService.SendEmailAsync(
                toEmail: inputToEmail,
                subject: inputSubject,
                body: inputBody,
                personalisation: inputPersonalization);

            NotificationDependencyException actualNotificationDependencyValidationException =
                await Assert.ThrowsAsync<NotificationDependencyException>(sendEmailTask.AsTask);

            // then
            actualNotificationDependencyValidationException.Should()
                 .BeEquivalentTo(expectedNotificationServiceException);

            this.govukNotifyBroker.Verify(broker =>
                broker.SendEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, dynamic>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
