﻿// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;

namespace ISL.Providers.Notifications.GovukNotify.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Fact]
        public async Task ShouldSendEmailAsync()
        {
            // given
            string randomIdentifier = GetRandomString();
            string expectedIdentifier = randomIdentifier;
            string inputToEmail = GetRandomEmailAddress();
            string inputSubject = GetRandomString();
            string inputBody = GetRandomString();
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputEmailReplyToId = GetRandomString();
            string inputOneClickUnsubscribeURL = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("templateId", inputTemplateId);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("oneClickUnsubscribeURL", inputOneClickUnsubscribeURL);

            Dictionary<string, dynamic> internalPersonalization = inputPersonalization.DeepClone();
            internalPersonalization.Add("subject", inputSubject);
            internalPersonalization.Add("body", inputBody);

            this.govukNotifyBroker
                .Setup(broker =>
                    broker.SendEmailAsync(
                        inputToEmail,
                        inputTemplateId,
                        It.Is(SameDictionaryAs(internalPersonalization)),
                        inputClientReference,
                        inputEmailReplyToId,
                        inputOneClickUnsubscribeURL))
                .ReturnsAsync(expectedIdentifier);

            // when
            string actualIdentifier = await this.notificationService.SendEmailAsync(
                toEmail: inputToEmail,
                subject: inputSubject,
                body: inputBody,
                personalisation: inputPersonalization);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);

            this.govukNotifyBroker
                .Verify(broker =>
                    broker.SendEmailAsync(
                        inputToEmail,
                        inputTemplateId,
                        It.Is(SameDictionaryAs(internalPersonalization)),
                        inputClientReference,
                        inputEmailReplyToId,
                        inputOneClickUnsubscribeURL),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
