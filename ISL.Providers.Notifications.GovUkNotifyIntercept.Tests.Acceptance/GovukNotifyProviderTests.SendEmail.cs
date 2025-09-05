// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.NotificationClient.Tests.Acceptance
{
    public partial class GovukNotifyProviderTests
    {
        [Fact]
        public async Task ShouldSendEmailAsync()
        {
            // given
            string toEmail = TEST_EMAIL;
            string subject = GetRandomString();
            string message = GetRandomString();
            string templateId = configuration.GetValue<string>("notifyConfigurations:emailTemplateId");
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>();
            personalisation.Add("subject", subject);
            personalisation.Add("message", message);

            // when
            string identifier = await this.govUkNotifyInterceptProvider.SendEmailAsync(
                templateId,
                toEmail,
                personalisation);

            // then
            identifier.Should().NotBeNullOrWhiteSpace();
        }
    }
}
