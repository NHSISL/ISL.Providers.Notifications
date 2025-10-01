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
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            Dictionary<string, dynamic> updatedPersonalisation = inputPersonalization.DeepClone();
            SubstituteInfo randomSubstituteInfo = GetRandomSubstituteInfo(inputPersonalization);

            for (int i = 0; i < MaxAddressLines; i++)
            {
                string key = $"addressLine{i + 1}";

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
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await notificationServiceMock.Object.SendLetterAsync(
                templateId: inputTemplateId,
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
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference),
                Times.Once);

            this.interceptBroker.VerifyNoOtherCalls();
        }
    }
}
