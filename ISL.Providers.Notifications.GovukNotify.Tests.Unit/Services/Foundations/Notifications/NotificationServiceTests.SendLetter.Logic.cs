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
        public async Task ShouldSendLetterAsync()
        {
            // given
            string randomIdentifier = GetRandomString();
            string expectedIdentifier = randomIdentifier;
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputAddressLine1 = GetRandomString();
            string inputAddressLine2 = GetRandomString();
            string inputAddressLine3 = GetRandomString();
            string inputAddressLine4 = GetRandomString();
            string inputAddressLine5 = GetRandomString();
            string inputAddressLine6 = GetRandomString();
            string inputAddressLine7 = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);

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
                addressLine1: inputAddressLine1,
                addressLine2: inputAddressLine2,
                addressLine3: inputAddressLine3,
                addressLine4: inputAddressLine4,
                addressLine5: inputAddressLine5,
                addressLine6: inputAddressLine6,
                addressLine7: inputAddressLine7,
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
