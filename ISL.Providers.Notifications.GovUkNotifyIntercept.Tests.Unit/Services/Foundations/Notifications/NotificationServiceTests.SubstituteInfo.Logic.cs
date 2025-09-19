// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Fact]
        public async Task ShouldSubstituteInfoAsyncWithSubstituteDictionaryTrueAndMatchingIdentifier()
        {
            // given
            string randomIdentifier = GetRandomString();
            this.configurations.SubstituteDictionaryValues = true;

            this.configurations.NotificationOverrides = new List<NotificationOverride>
            {
                GetRandomNotificationOverride(randomIdentifier)
            };

            Dictionary<string, dynamic> inputPersonalisation =
                GetPersonalisationDictionaryForSubstitute(this.configurations, randomIdentifier);

            Dictionary<string, dynamic> updatePersonalisation = inputPersonalisation.DeepClone();

            NotificationOverride inputNotificationOverride = this.configurations.NotificationOverrides
                .Find(overrideConfig => overrideConfig.Identifier == randomIdentifier);

            SubstituteInfo expectedSubstituteInfo =
                GetExpectedSubstituteInfo(configurations, updatePersonalisation, inputNotificationOverride);

            // when
            SubstituteInfo actualSubstituteInfo =
                await this.notificationService.SubstituteInfoAsync(inputPersonalisation);

            // then
            actualSubstituteInfo.Should().BeEquivalentTo(expectedSubstituteInfo);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSubstituteInfoAsyncWithSubstituteDictionaryFalseAndMatchingIdentifier()
        {
            // given
            string randomIdentifier = GetRandomString();
            this.configurations.SubstituteDictionaryValues = false;

            this.configurations.NotificationOverrides = new List<NotificationOverride>
            {
                GetRandomNotificationOverride(randomIdentifier)
            };

            Dictionary<string, dynamic> inputPersonalisation =
                GetPersonalisationDictionaryForSubstitute(this.configurations, randomIdentifier);

            Dictionary<string, dynamic> updatePersonalisation = inputPersonalisation.DeepClone();

            NotificationOverride inputNotificationOverride = this.configurations.NotificationOverrides
                .Find(overrideConfig => overrideConfig.Identifier == randomIdentifier);

            SubstituteInfo expectedSubstituteInfo =
                GetExpectedSubstituteInfo(configurations, updatePersonalisation, inputNotificationOverride);

            // when
            SubstituteInfo actualSubstituteInfo =
                await this.notificationService.SubstituteInfoAsync(inputPersonalisation);

            // then
            actualSubstituteInfo.Should().BeEquivalentTo(expectedSubstituteInfo);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSubstituteInfoAsyncWithSubstituteDictionaryTrueAndNoMatchingIdentifier()
        {
            // given
            string randomIdentifier = GetRandomString();
            this.configurations.SubstituteDictionaryValues = true;

            this.configurations.NotificationOverrides = new List<NotificationOverride>
            {
                GetRandomNotificationOverride()
            };

            Dictionary<string, dynamic> inputPersonalisation =
                GetPersonalisationDictionaryForSubstitute(this.configurations, randomIdentifier);

            Dictionary<string, dynamic> updatePersonalisation = inputPersonalisation.DeepClone();

            NotificationOverride inputNotificationOverride = this.configurations.NotificationOverrides
                .Find(overrideConfig => overrideConfig.Identifier == randomIdentifier);

            SubstituteInfo expectedSubstituteInfo =
                GetExpectedSubstituteInfo(configurations, updatePersonalisation, inputNotificationOverride);

            // when
            SubstituteInfo actualSubstituteInfo =
                await this.notificationService.SubstituteInfoAsync(inputPersonalisation);

            // then
            actualSubstituteInfo.Should().BeEquivalentTo(expectedSubstituteInfo);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldSubstituteInfoAsyncWithSubstituteDictionaryFalseAndNoMatchingIdentifier()
        {
            // given
            string randomIdentifier = GetRandomString();
            this.configurations.SubstituteDictionaryValues = false;

            this.configurations.NotificationOverrides = new List<NotificationOverride>
            {
                GetRandomNotificationOverride()
            };

            Dictionary<string, dynamic> inputPersonalisation =
                GetPersonalisationDictionaryForSubstitute(this.configurations, randomIdentifier);

            Dictionary<string, dynamic> updatePersonalisation = inputPersonalisation.DeepClone();

            NotificationOverride inputNotificationOverride = this.configurations.NotificationOverrides
                .Find(overrideConfig => overrideConfig.Identifier == randomIdentifier);

            SubstituteInfo expectedSubstituteInfo =
                GetExpectedSubstituteInfo(configurations, updatePersonalisation, inputNotificationOverride);

            // when
            SubstituteInfo actualSubstituteInfo =
                await this.notificationService.SubstituteInfoAsync(inputPersonalisation);

            // then
            actualSubstituteInfo.Should().BeEquivalentTo(expectedSubstituteInfo);

            this.govukNotifyBroker.VerifyNoOtherCalls();
        }
    }
}
