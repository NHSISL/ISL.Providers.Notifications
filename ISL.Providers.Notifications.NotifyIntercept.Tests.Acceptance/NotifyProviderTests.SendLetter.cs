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
        public async Task ShouldSendLetterAsync()
        {
            // given
            string templateId = configuration.GetValue<string>("notifyConfigurations:letterTemplateId");
            string outputIdentifier = GetRandomString();
            string expectedIdentifier = outputIdentifier;
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>();
            personalisation.Add("address_line_1", GetRandomString());
            personalisation.Add("address_line_2", GetRandomString());
            personalisation.Add("address_line_7", GetRandomString());

            notificationProvider.Setup(provider =>
                provider.SendLetterAsync(
                    templateId,
                    personalisation,
                    null))
                        .ReturnsAsync(outputIdentifier);

            // when
            string actualIdentifier = await this.notifyInterceptProvider.SendLetterAsync(
                templateId,
                personalisation);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);
        }
    }
}
