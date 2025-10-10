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
            string addressLine1 = GetRandomString();
            string addressLine2 = GetRandomString();
            string addressLine3 = GetRandomString();
            string addressLine4 = GetRandomString();
            string addressLine5 = GetRandomString();
            string addressLine6 = GetRandomString();
            string addressLine7 = GetRandomString();
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>();

            notificationProvider.Setup(provider =>
                provider.SendLetterAsync(
                    templateId,
                    addressLine1,
                    addressLine2,
                    addressLine3,
                    addressLine4,
                    addressLine5,
                    addressLine6,
                    addressLine7,
                    personalisation,
                    null))
                        .ReturnsAsync(outputIdentifier);

            // when
            string actualIdentifier = await this.notifyInterceptProvider.SendLetterAsync(
                templateId,
                addressLine1,
                addressLine2,
                addressLine3,
                addressLine4,
                addressLine5,
                addressLine6,
                addressLine7,
                personalisation);

            // then
            actualIdentifier.Should().BeEquivalentTo(expectedIdentifier);
        }
    }
}
