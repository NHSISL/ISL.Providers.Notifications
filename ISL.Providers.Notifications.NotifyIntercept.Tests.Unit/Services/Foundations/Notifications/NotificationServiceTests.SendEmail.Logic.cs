// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications;
using ISL.Providers.Notifications.NotifyIntercept.Services.Foundations.Notifications;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.NotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Fact]
        public async Task ShouldSendEmailAsync()
        {
            // given
            string randomIdentifier = GetRandomString();
            string expectedIdentifier = randomIdentifier;
            string inputTemplateId = GetRandomString();
            string inputToEmail = GetRandomEmailAddress();
            string inputClientReference = GetRandomString();
            string inputSubject = GetRandomString();
            string inputMessage = GetRandomString();
            string inputEmailReplyToId = GetRandomString();
            string inputOneClickUnsubscribeURL = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("subject", inputSubject);
            inputPersonalization.Add("message", inputMessage);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("oneClickUnsubscribeURL", inputOneClickUnsubscribeURL);
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
                    broker.SendEmailAsync(
                        inputTemplateId,
                        outputSubstituteInfo.Email,
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await notificationServiceMock.Object.SendEmailAsync(
                templateId: inputTemplateId,
                toEmail: inputToEmail,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);

            notificationServiceMock.Verify(service =>
                service.SubstituteInfoAsync(inputPersonalization),
                    Times.Once);

            this.govukNotifyBroker
                .Verify(broker =>
                    broker.SendEmailAsync(
                        inputTemplateId,
                        outputSubstituteInfo.Email,
                        It.Is(SameDictionaryAs(outputSubstituteInfo.Personalisation)),
                        inputClientReference),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
