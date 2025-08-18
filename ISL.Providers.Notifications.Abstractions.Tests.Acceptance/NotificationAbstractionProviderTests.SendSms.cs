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
        public async Task ShouldSendSmsAsync()
        {
            // given
            string mobileNumber = GetRandomMobileNumber();
            string templateId = configuration.GetValue<string>("notifyConfigurations:templateId");
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>();
            personalisation.Add("mobileNumber", mobileNumber);

            // when
            string identifier = await this.notificationAbstractionProvider.SendSmsAsync(
                templateId,
                personalisation);

            // then
            identifier.Should().NotBeNullOrWhiteSpace();
        }
    }
}
