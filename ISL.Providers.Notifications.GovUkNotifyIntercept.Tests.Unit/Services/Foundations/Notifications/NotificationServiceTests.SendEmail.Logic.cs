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
            string interceptingEmail = this.configurations.InterceptingEmail;
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("subject", inputSubject);
            inputPersonalization.Add("message", inputMessage);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("oneClickUnsubscribeURL", inputOneClickUnsubscribeURL);

            this.govukNotifyBroker
                .Setup(broker =>
                    broker.SendEmailAsync(
                        inputTemplateId,
                        interceptingEmail,
                        It.Is(SameDictionaryAs(inputPersonalization)),
                        inputClientReference))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await this.notificationService.SendEmailAsync(
                templateId: inputTemplateId,
                toEmail: inputToEmail,
                personalisation: inputPersonalization,
                clientReference: inputClientReference);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);

            this.govukNotifyBroker
                .Verify(broker =>
                    broker.SendEmailAsync(
                        inputTemplateId,
                        interceptingEmail,
                        It.Is(SameDictionaryAs(inputPersonalization)),
                        inputClientReference),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
