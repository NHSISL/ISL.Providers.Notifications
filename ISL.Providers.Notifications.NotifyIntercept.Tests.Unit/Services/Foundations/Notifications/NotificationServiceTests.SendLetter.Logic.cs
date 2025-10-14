// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications;
using ISL.Providers.Notifications.NotifyIntercept.Services.Foundations.Notifications;
using Moq;

namespace ISL.Providers.Notifications.NotifyIntercept.Tests.Unit.Services.Foundations.Notifications
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
            string inputRecipientName = GetRandomString();
            string inputAddressLine1 = GetRandomString();
            string inputAddressLine2 = GetRandomString();
            string inputAddressLine3 = GetRandomString();
            string inputAddressLine4 = GetRandomString();
            string inputAddressLine5 = GetRandomString();
            string inputPostCode = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> updatedPersonalisation = inputPersonalization.DeepClone();
            updatedPersonalisation = GetSubstitutedLetterDictionary(updatedPersonalisation);
            SubstituteInfo randomSubstituteInfo = GetRandomSubstituteInfo(inputPersonalization);
            randomSubstituteInfo.Personalisation = updatedPersonalisation;
            SubstituteInfo outputSubstituteInfo = randomSubstituteInfo.DeepClone();

            var notificationServiceMock = new Mock<NotificationService>(
                this.interceptBroker.Object,
                this.configurations)
            { CallBase = true };

            notificationServiceMock.Setup(service =>
                service.SubstituteInfoAsync(inputPersonalization))
                    .ReturnsAsync(outputSubstituteInfo);

            this.interceptBroker
                .Setup(broker =>
                    broker.SendLetterAsync(
                        inputTemplateId,
                        outputSubstituteInfo.AddressLines.First(),
                        outputSubstituteInfo.AddressLines.ElementAt(1),
                        outputSubstituteInfo.AddressLines.ElementAt(2),
                        outputSubstituteInfo.AddressLines.ElementAt(3),
                        outputSubstituteInfo.AddressLines.ElementAt(4),
                        outputSubstituteInfo.AddressLines.ElementAt(5),
                        outputSubstituteInfo.AddressLines.Last(),
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await notificationServiceMock.Object.SendLetterAsync(
                templateId: inputTemplateId,
                recipientName: inputRecipientName,
                addressLine1: inputAddressLine1,
                addressLine2: inputAddressLine2,
                addressLine3: inputAddressLine3,
                addressLine4: inputAddressLine4,
                addressLine5: inputAddressLine5,
                postCode: inputPostCode,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);

            notificationServiceMock.Verify(service =>
                service.SubstituteInfoAsync(inputPersonalization),
                    Times.Once);

            this.interceptBroker
                .Verify(broker =>
                    broker.SendLetterAsync(
                        inputTemplateId,
                       outputSubstituteInfo.AddressLines.First(),
                        outputSubstituteInfo.AddressLines.ElementAt(1),
                        outputSubstituteInfo.AddressLines.ElementAt(2),
                        outputSubstituteInfo.AddressLines.ElementAt(3),
                        outputSubstituteInfo.AddressLines.ElementAt(4),
                        outputSubstituteInfo.AddressLines.ElementAt(5),
                        outputSubstituteInfo.AddressLines.Last(),
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference),
                Times.Once);

            this.interceptBroker.VerifyNoOtherCalls();
        }
    }
}
