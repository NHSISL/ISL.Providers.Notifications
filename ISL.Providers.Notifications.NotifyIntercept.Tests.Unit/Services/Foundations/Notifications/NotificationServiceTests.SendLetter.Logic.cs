// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
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
            string inputAddressLine1 = GetRandomString();
            string inputAddressLine2 = GetRandomString();
            string inputAddressLine3 = GetRandomString();
            string inputAddressLine4 = GetRandomString();
            string inputAddressLine5 = GetRandomString();
            string inputAddressLine6 = GetRandomString();
            string inputAddressLine7 = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> updatedPersonalisation = inputPersonalization.DeepClone();

            updatedPersonalisation = UpdatePersonalisation(
                inputAddressLine1,
                inputAddressLine2,
                inputAddressLine3,
                inputAddressLine4,
                inputAddressLine5,
                inputAddressLine6,
                inputAddressLine7,
                updatedPersonalisation);

            SubstituteInfo randomSubstituteInfo = GetRandomSubstituteInfo(inputPersonalization);

            for (int i = 0; i < MaxAddressLines; i++)
            {
                string key = $"address_line_{i + 1}";

                string? value =
                    i < randomSubstituteInfo.AddressLines.Count ? randomSubstituteInfo.AddressLines[i] : null;

                updatedPersonalisation[key] = value;
            }

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
                        inputAddressLine1,
                        inputAddressLine2,
                        inputAddressLine3,
                        inputAddressLine4,
                        inputAddressLine5,
                        inputAddressLine6,
                        inputAddressLine7,
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await notificationServiceMock.Object.SendLetterAsync(
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

            notificationServiceMock.Verify(service =>
                service.SubstituteInfoAsync(inputPersonalization),
                    Times.Once);

            this.interceptBroker
                .Verify(broker =>
                    broker.SendLetterAsync(
                        inputTemplateId,
                        inputAddressLine1,
                        inputAddressLine2,
                        inputAddressLine3,
                        inputAddressLine4,
                        inputAddressLine5,
                        inputAddressLine6,
                        inputAddressLine7,
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference),
                Times.Once);

            this.interceptBroker.VerifyNoOtherCalls();
        }
    }
}
