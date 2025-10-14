// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using ISL.Providers.Notifications.NotifyIntercept.Models;
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

            NotificationOverride defaultOverride = configuration
                .GetSection("notifyConfigurations:defaultOverride").Get<NotificationOverride>();

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
            Dictionary<string, dynamic> substitutedPersonalisation = GetSubstitutedLetterDictionary();

            notificationProvider.Setup(provider =>
                provider.SendLetterAsync(
                    templateId,
                    defaultOverride.RecipientName,
                    defaultOverride.AddressLine1,
                    defaultOverride.AddressLine2,
                    defaultOverride.AddressLine3,
                    defaultOverride.AddressLine4,
                    defaultOverride.AddressLine5,
                    defaultOverride.PostCode,
                    It.Is(SameDictionaryAs(substitutedPersonalisation)),
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
