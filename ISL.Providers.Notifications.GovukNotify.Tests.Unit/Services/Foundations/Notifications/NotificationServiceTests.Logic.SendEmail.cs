// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;

namespace ISL.Providers.Notifications.GovukNotify.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Fact]
        public async Task ShouldSendEmailAsync()
        {
            // given
            string inputFromEmail = GetRandomEmailAddress();
            string inputToEmail = GetRandomEmailAddress();
            string inputSubject = GetRandomString();
            string inputBody = GetRandomString();
            string inputTemplateId = GetRandomString();
            string inputClientReference = GetRandomString();
            string inputEmailReplyToId = GetRandomString();
            string inputOneClickUnsubscribeURL = GetRandomString();
            Dictionary<string, dynamic> inputPersonalization = new Dictionary<string, dynamic>();
            inputPersonalization.Add("templateId", inputTemplateId);
            inputPersonalization.Add("clientReference", inputClientReference);
            inputPersonalization.Add("emailReplyToId", inputEmailReplyToId);
            inputPersonalization.Add("clickUnsubscribeURL", inputOneClickUnsubscribeURL);

            // when
            await this.notificationService.SendEmailAsync(
                fromEmail: inputFromEmail,
                toEmail: inputToEmail,
                subject: inputSubject,
                body: inputBody,
                personalisation: inputPersonalization);

            // then
            this.govukNotifyBroker.Verify(broker =>
                broker.SendEmailAsync(
                    inputToEmail,
                    inputTemplateId,
                    inputPersonalization,
                    inputClientReference,
                    inputEmailReplyToId,
                    inputOneClickUnsubscribeURL),
                Times.Once);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
