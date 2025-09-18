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
        public async Task ShouldSendLetterAsync()
        {
            // given
            string randomIdentifier = GetRandomString();
            string expectedIdentifier = randomIdentifier;
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            List<string> interceptingAddressLines = this.configurations.InterceptingAddressLines;
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("addressLine1", interceptingAddressLines[0]);
            inputPersonalization.Add("addressLine2", interceptingAddressLines[1]);
            inputPersonalization.Add("addressLine3", interceptingAddressLines[2]);
            inputPersonalization.Add("addressLine4", interceptingAddressLines[3]);
            inputPersonalization.Add("addressLine5", interceptingAddressLines[4]);
            inputPersonalization.Add("addressLine6", interceptingAddressLines[5]);
            inputPersonalization.Add("addressLine7", interceptingAddressLines[6]);

            this.govukNotifyBroker
                .Setup(broker =>
                    broker.SendLetterAsync(
                        inputTemplateId,
                        It.Is(SameDictionaryAs(inputPersonalization)),
                        inputClientReference))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await this.notificationService.SendLetterAsync(
                templateId: inputTemplateId,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);

            this.govukNotifyBroker
                .Verify(broker =>
                    broker.SendLetterAsync(
                        inputTemplateId,
                        It.Is(SameDictionaryAs(inputPersonalization)),
                        inputClientReference),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
