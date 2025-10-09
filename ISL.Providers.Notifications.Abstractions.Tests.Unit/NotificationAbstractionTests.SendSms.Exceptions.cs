// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.Providers.Notifications.Abstractions.Models.Exceptions;
using ISL.Providers.Notifications.Abstractions.Tests.Unit.Models.Exceptions;
using Moq;
using Xeptions;

namespace ISL.Providers.Notifications.Abstractions.Tests.Unit
{
    public partial class NotificationAbstractionTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionWhenTypeINotificationValidationException()
        {
            // given
            var someException = new Xeption();

            someException.AddData(
                key: "someKey",
                values: "someValues");

            var someNotificationValidationException =
                new SomeNotificationValidationException(
                    message: "Some notification validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            NotificationProviderValidationException expectedNotificationValidationProviderException =
                new NotificationProviderValidationException(
                    message: someNotificationValidationException.Message,
                    innerException: someNotificationValidationException,
                    data: someNotificationValidationException.Data);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someNotificationValidationException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationProviderValidationException actualNotificationValidationProviderException =
                await Assert.ThrowsAsync<NotificationProviderValidationException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationValidationProviderException.Should().BeEquivalentTo(
                expectedNotificationValidationProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
                    Times.Once);

            this.notificationProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionWhenTypeINotificationDependencyException()
        {
            // given
            var someException = new Xeption();

            var someNotificationValidationException =
                new SomeNotificationDependencyException(
                    message: "Some notification dependency exception occurred",
                    innerException: someException);

            someNotificationValidationException.AddData(
                key: "someKey",
                values: "someValues");

            NotificationProviderDependencyException expectedNotificationDependencyProviderException =
                new NotificationProviderDependencyException(
                    message: someNotificationValidationException.Message,
                    innerException: someNotificationValidationException,
                    data: someNotificationValidationException.Data);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someNotificationValidationException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationProviderDependencyException actualNotificationDependencyProviderException =
                await Assert.ThrowsAsync<NotificationProviderDependencyException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationDependencyProviderException.Should().BeEquivalentTo(
                expectedNotificationDependencyProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
                    Times.Once);

            this.notificationProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionWhenTypeINotificationServiceException()
        {
            // given
            var someException = new Xeption();

            var someNotificationValidationException =
                new SomeNotificationServiceException(
                    message: "Some notification service exception occurred",
                    innerException: someException);

            someNotificationValidationException.AddData(
                key: "someKey",
                values: "someValues");

            NotificationProviderServiceException expectedNotificationServiceProviderException =
                new NotificationProviderServiceException(
                    message: someNotificationValidationException.Message,
                    innerException: someNotificationValidationException,
                    data: someNotificationValidationException.Data);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someNotificationValidationException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationProviderServiceException actualNotificationServiceProviderException =
                await Assert.ThrowsAsync<NotificationProviderServiceException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationServiceProviderException.Should().BeEquivalentTo(
                expectedNotificationServiceProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
                    Times.Once);

            this.notificationProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowUncatagorizedServiceExceptionWhenTypeIsNotExpected()
        {
            // given
            var someException = new Xeption();

            someException.AddData(
                key: "someKey",
                values: "someValues");

            NotificationProviderServiceException expectedNotificationServiceProviderException =
                new NotificationProviderServiceException(
                    message: "Notification provider not properly implemented. Uncatagorized errors found, " +
                        "contact the notification provider owner for support.",

                    innerException: someException,
                    data: someException.Data);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationProviderServiceException actualNotificationServiceProviderException =
                await Assert.ThrowsAsync<NotificationProviderServiceException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationServiceProviderException.Should().BeEquivalentTo(
                expectedNotificationServiceProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
                    Times.Once);

            this.notificationProviderMock.VerifyNoOtherCalls();
        }
    }
}
