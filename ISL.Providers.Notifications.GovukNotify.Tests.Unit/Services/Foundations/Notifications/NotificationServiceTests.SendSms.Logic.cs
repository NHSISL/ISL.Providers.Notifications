// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;

namespace ISL.Providers.Notifications.GovukNotify.Tests.Unit.Services.Foundations.Notifications
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
            string inputMobileNumber = GetRandomMobileNumber();
            string inputSmsSenderId = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("templateId", inputTemplateId);
            inputPersonalization.Add("mobileNumber", inputMobileNumber);
            inputPersonalization.Add("smsSenderId", inputSmsSenderId);

            this.govukNotifyBroker
                .Setup(broker =>
                    broker.SendSmsAsync(
                        inputMobileNumber,
                        inputTemplateId,
                        It.Is(SameDictionaryAs(inputPersonalization)),
                        inputClientReference,
                        inputSmsSenderId))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await this.notificationService.SendSmsAsync(
                templateId: inputTemplateId,
                personalisation: inputPersonalization);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);

            this.govukNotifyBroker
                .Verify(broker =>
                    broker.SendSmsAsync(
                        inputMobileNumber,
                        inputTemplateId,
                        It.Is(SameDictionaryAs(inputPersonalization)),
                        inputClientReference,
                        inputSmsSenderId),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
