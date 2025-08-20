// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace ISL.Providers.Notifications.Abstractions.Tests.Acceptance
{
    public partial class NotificationAbstractionProviderTests
    {
        [Fact]
        public async Task ShouldSendEmailWithTemplateIdAsync()
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
            string identifier = await this.notificationAbstractionProvider.SendEmailAsync(
                templateId,
                toEmail,
                personalisation);

            // then
            identifier.Should().NotBeNullOrWhiteSpace();
        }
    }
}
