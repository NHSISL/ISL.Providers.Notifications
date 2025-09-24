// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Services.Foundations.Notifications;
using Moq;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Fact]
        public async Task ShouldSendSmsAsync()
        {
            // given
            string randomIdentifier = GetRandomString();
            string expectedIdentifier = randomIdentifier;
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputMessage = GetRandomString();
            string inputMobileNumber = GetRandomLocalMobileNumber();
            string inputSmsSenderId = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("message", inputMessage);
            inputPersonalization.Add("smsSenderId", inputSmsSenderId);
            SubstituteInfo randomSubstituteInfo = GetRandomSubstituteInfo(inputPersonalization);
            SubstituteInfo outputSubstituteInfo = randomSubstituteInfo.DeepClone();

            var notificationServiceMock = new Mock<NotificationService>(
                this.govukNotifyBroker.Object,
                this.configurations)
            { CallBase = true };

            notificationServiceMock.Setup(service =>
                service.SubstituteInfoAsync(inputPersonalization))
                    .ReturnsAsync(outputSubstituteInfo);

            this.govukNotifyBroker
                .Setup(broker =>
                    broker.SendSmsAsync(
                        outputSubstituteInfo.MobileNumber,
                        inputTemplateId,
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference,
                        inputSmsSenderId))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await notificationServiceMock.Object.SendSmsAsync(
                templateId: inputTemplateId,
                mobileNumber: inputMobileNumber,
                personalisation: inputPersonalization);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);

            notificationServiceMock.Verify(service =>
                service.SubstituteInfoAsync(inputPersonalization),
                    Times.Once);

            this.govukNotifyBroker
                .Verify(broker =>
                    broker.SendSmsAsync(
                        outputSubstituteInfo.MobileNumber,
                        inputTemplateId,
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference,
                        inputSmsSenderId),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
