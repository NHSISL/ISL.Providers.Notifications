// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace ISL.NotificationClient.Tests.Acceptance
{
    public partial class GovukNotifyProviderTests
    {
        [Fact]
        public async Task ShouldSendSmsAsync()
        {
            // given
            string toMobileNumber = TEST_MOBILE_NUMBER;
            string message = GetRandomString();
            string templateId = configuration.GetValue<string>("notifyConfigurations:smsTemplateId");
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>();
            personalisation.Add("message", message);

            // when
            string identifier = await this.govUkNotifyInterceptProvider.SendSmsAsync(
                templateId,
                toMobileNumber,
                personalisation);

            // then
            identifier.Should().NotBeNullOrWhiteSpace();
        }
    }
}
