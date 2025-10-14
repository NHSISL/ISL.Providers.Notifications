// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ISL.Providers.Notifications.Abstractions;
using ISL.Providers.Notifications.NotifyIntercept.Models;
using ISL.Providers.Notifications.NotifyIntercept.Providers.Notifications;
using KellermanSoftware.CompareNetObjects;
using Microsoft.Extensions.Configuration;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.NotificationClient.Tests.Acceptance
{
    public partial class NotifyProviderTests
    {
        private readonly Mock<INotificationProvider> notificationProvider;
        private readonly INotifyInterceptProvider notifyInterceptProvider;
        private readonly IConfiguration configuration;
        private readonly ICompareLogic compareLogic;
        private readonly string TEST_EMAIL = "simulate-delivered@notifications.service.gov.uk";
        private readonly string TEST_MOBILE_NUMBER = "07700900000";

        public NotifyProviderTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.ContinuousIntegration.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();

            NotifyConfigurations notifyConfigurations = configuration
                .GetSection("notifyConfigurations").Get<NotifyConfigurations>();

            this.compareLogic = new CompareLogic();
            this.notificationProvider = new Mock<INotificationProvider>();

            this.notifyInterceptProvider = new NotifyInterceptProvider(
                notifyConfigurations,
                notificationProvider: notificationProvider.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private Dictionary<string, dynamic> GetSubstitutedLetterDictionary()
        {
            Dictionary<string, dynamic> personalisation =
                new Dictionary<string, dynamic>();

            string addressLine1Key = configuration.GetSection("notifyConfigurations:addressLine1Key").Value;
            string addressLine2Key = configuration.GetSection("notifyConfigurations:addressLine2Key").Value;
            string addressLine3Key = configuration.GetSection("notifyConfigurations:addressLine3Key").Value;
            string addressLine4Key = configuration.GetSection("notifyConfigurations:addressLine4Key").Value;
            string addressLine5Key = configuration.GetSection("notifyConfigurations:addressLine5Key").Value;
            string addressLine6Key = configuration.GetSection("notifyConfigurations:addressLine6Key").Value;
            string addressLine7Key = configuration.GetSection("notifyConfigurations:addressLine7Key").Value;

            NotificationOverride defaultOverride = configuration
                .GetSection("notifyConfigurations:defaultOverride").Get<NotificationOverride>();

            personalisation.Add(addressLine1Key, defaultOverride.RecipientName);
            personalisation.Add(addressLine2Key, defaultOverride.AddressLine1);
            personalisation.Add(addressLine3Key, defaultOverride.AddressLine2);
            personalisation.Add(addressLine4Key, defaultOverride.AddressLine3);
            personalisation.Add(addressLine5Key, defaultOverride.AddressLine4);
            personalisation.Add(addressLine6Key, defaultOverride.AddressLine5);
            personalisation.Add(addressLine7Key, defaultOverride.PostCode);

            return personalisation;
        }

        private Expression<Func<Dictionary<string, dynamic>, bool>> SameDictionaryAs(
            Dictionary<string, dynamic> expectedDictionary) =>
            actualDictionary => this.compareLogic.Compare(expectedDictionary, actualDictionary).AreEqual;
    }
}