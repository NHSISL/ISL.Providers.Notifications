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
        public async Task ShouldSendSmsAsync()
        {
            // given
            string toMobileNumber = TEST_MOBILE_NUMBER;
            string message = GetRandomString();
            string templateId = configuration.GetValue<string>("notifyConfigurations:smsTemplateId");
            string outputIdentifier = GetRandomString();
            string expectedIdentifier = outputIdentifier;
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>();
            personalisation.Add("message", message);

            notificationProvider.Setup(provider =>
                provider.SendSmsAsync(
                    templateId,
                    toMobileNumber,
                    personalisation))
                        .ReturnsAsync(outputIdentifier);

            // when
            string actualIdentifier = await this.notifyInterceptProvider.SendSmsAsync(
                templateId,
                toMobileNumber,
                personalisation);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);
        }
    }
}
