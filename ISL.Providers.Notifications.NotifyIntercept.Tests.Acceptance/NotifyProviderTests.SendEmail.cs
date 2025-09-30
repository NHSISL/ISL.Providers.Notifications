// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ISL.NotificationClient.Tests.Acceptance
{
    public partial class NotifyProviderTests
    {
        [Fact]
        public async Task ShouldSendEmailAsync()
        {
            // given
            string templateId = configuration.GetValue<string>("notifyConfigurations:emailTemplateId");
            string toEmail = TEST_EMAIL;
            string subject = GetRandomString();
            string message = GetRandomString();
            string outputIdentifier = GetRandomString();
            string expectedIdentifier = outputIdentifier;
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>();
            personalisation.Add("subject", subject);
            personalisation.Add("message", message);

            notificationProvider.Setup(provider =>
                provider.SendEmailAsync(
                    templateId,
                    toEmail,
                    personalisation,
                    null))
                        .ReturnsAsync(outputIdentifier);

            // when
            string actualIdentifier = await this.notifyInterceptProvider.SendEmailAsync(
                templateId,
                toEmail,
                personalisation);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);
        }
    }
}
