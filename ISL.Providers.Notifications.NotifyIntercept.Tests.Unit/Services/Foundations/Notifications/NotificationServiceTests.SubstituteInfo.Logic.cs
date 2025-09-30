// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using ISL.Providers.Notifications.NotifyIntercept.Models;
using ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications;

namespace ISL.Providers.Notifications.NotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        [Fact]
        public async Task ShouldReplaceValuesWithOverrideValuesOnMatchingIdentifier()
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

            this.interceptBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldOnlyReplaceMethodValuesWithOverrideValuesOnMatchingIdentifier()
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

            this.interceptBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldReplaceValuesWithDefaultOverrideValuesOnNoMatchingIdentifier()
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

            this.interceptBroker.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldOnlyReplaceMethodValuesWithDefaultOverrideValuesOnNoMatchingIdentifier()
        {
            // given
            string randomIdentifier = GetRandomString();
            string randomAddressLine = GetRandomString();
            this.configurations.SubstituteDictionaryValues = false;

            this.configurations.NotificationOverrides = new List<NotificationOverride>
            {
                GetRandomNotificationOverride()
            };

            Dictionary<string, dynamic> inputPersonalisation =
                GetPersonalisationDictionaryForSubstitute(this.configurations, randomIdentifier, randomAddressLine);

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

            this.interceptBroker.VerifyNoOtherCalls();
        }
    }
}
