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

            var someNotificationValidationException =
                new SomeNotificationValidationException(
                    message: "Some notification validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            NotificationValidationProviderException expectedNotificationValidationProviderException =
                new NotificationValidationProviderException(
                    message: "Notification validation errors occurred, please try again.",
                    innerException: someNotificationValidationException);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someNotificationValidationException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationValidationProviderException actualNotificationValidationProviderException =
                await Assert.ThrowsAsync<NotificationValidationProviderException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationValidationProviderException.Should().BeEquivalentTo(
                expectedNotificationValidationProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
                    Times.Once);

            this.notificationProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionWhenTypeINotificationDependencyValidationException()
        {
            // given
            var someException = new Xeption();

            var someNotificationValidationException =
                new SomeNotificationDependencyValidationException(
                    message: "Some notification dependency validation exception occurred",
                    innerException: someException,
                    data: someException.Data);

            NotificationValidationProviderException expectedNotificationValidationProviderException =
                new NotificationValidationProviderException(
                    message: "Notification validation errors occurred, please try again.",
                    innerException: someNotificationValidationException);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someNotificationValidationException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationValidationProviderException actualNotificationValidationProviderException =
                await Assert.ThrowsAsync<NotificationValidationProviderException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationValidationProviderException.Should().BeEquivalentTo(
                expectedNotificationValidationProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
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

            NotificationDependencyProviderException expectedNotificationDependencyProviderException =
                new NotificationDependencyProviderException(
                    message: "Notification dependency error occurred, contact support.",
                    innerException: someNotificationValidationException);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someNotificationValidationException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationDependencyProviderException actualNotificationDependencyProviderException =
                await Assert.ThrowsAsync<NotificationDependencyProviderException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationDependencyProviderException.Should().BeEquivalentTo(
                expectedNotificationDependencyProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
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

            NotificationServiceProviderException expectedNotificationServiceProviderException =
                new NotificationServiceProviderException(
                    message: "Notification service error occurred, contact support.",
                    innerException: someNotificationValidationException);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someNotificationValidationException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationServiceProviderException actualNotificationServiceProviderException =
                await Assert.ThrowsAsync<NotificationServiceProviderException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationServiceProviderException.Should().BeEquivalentTo(
                expectedNotificationServiceProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
                    Times.Once);

            this.notificationProviderMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowUncatagorizedServiceExceptionWhenTypeIsNotExpected()
        {
            // given
            var someException = new Xeption();

            var uncatagorizedNotificationProviderException =
                new UncatagorizedNotificationProviderException(
                    message: "Notification provider not properly implemented. Uncatagorized errors found, " +
                            "contact the notification provider owner for support.",
                    innerException: someException,
                    data: someException.Data);

            NotificationServiceProviderException expectedNotificationServiceProviderException =
                new NotificationServiceProviderException(
                    message: "Uncatagorized notification service error occurred, contact support.",
                    innerException: uncatagorizedNotificationProviderException);

            this.notificationProviderMock.Setup(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()))
                    .ThrowsAsync(someException);

            // when
            ValueTask<string> sendSmsTask =
                this.notificationAbstractionProvider
                    .SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>());

            NotificationServiceProviderException actualNotificationServiceProviderException =
                await Assert.ThrowsAsync<NotificationServiceProviderException>(testCode: sendSmsTask.AsTask);

            // then
            actualNotificationServiceProviderException.Should().BeEquivalentTo(
                expectedNotificationServiceProviderException);

            this.notificationProviderMock.Verify(provider =>
                provider.SendSmsAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, dynamic>>()),
                    Times.Once);

            this.notificationProviderMock.VerifyNoOtherCalls();
        }
    }
}
