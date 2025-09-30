// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace ISL.NotificationClient.Tests.Acceptance
{
    public partial class NotifyProviderTests
    {
        [Fact]
        public async Task ShouldSendLetterAsync()
        {
            // given
            string templateId = configuration.GetValue<string>("notifyConfigurations:letterTemplateId");
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>();

            // when
            string identifier = await this.notifyInterceptProvider.SendLetterAsync(
                templateId,
                personalisation);

            // then
            identifier.Should().NotBeNullOrWhiteSpace();
        }
    }
}
