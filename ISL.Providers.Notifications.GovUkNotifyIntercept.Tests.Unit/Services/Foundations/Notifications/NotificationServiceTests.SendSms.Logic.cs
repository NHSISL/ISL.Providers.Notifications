// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            string interceptingMobileNumber = this.configurations.InterceptingMobileNumber;
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("message", inputMessage);
            inputPersonalization.Add("smsSenderId", inputSmsSenderId);

            this.govukNotifyBroker
                .Setup(broker =>
                    broker.SendSmsAsync(
                        interceptingMobileNumber,
                        inputTemplateId,
                        It.Is(SameDictionaryAs(inputPersonalization)),
                        inputClientReference,
                        inputSmsSenderId))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await this.notificationService.SendSmsAsync(
                templateId: inputTemplateId,
                mobileNumber: inputMobileNumber,
                personalisation: inputPersonalization);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);

            this.govukNotifyBroker
                .Verify(broker =>
                    broker.SendSmsAsync(
                        interceptingMobileNumber,
                        inputTemplateId,
                        It.Is(SameDictionaryAs(inputPersonalization)),
                        inputClientReference,
                        inputSmsSenderId),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
