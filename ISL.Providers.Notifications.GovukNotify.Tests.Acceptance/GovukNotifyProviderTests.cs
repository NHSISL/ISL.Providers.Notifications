// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovukNotify.Models;
using ISL.Providers.Notifications.GovukNotify.Providers.Notifications;
using Microsoft.Extensions.Configuration;
using System;
using Tynamix.ObjectFiller;

namespace ISL.NotificationClient.Tests.Acceptance
{
    public partial class GovukNotifyProviderTests
    {
        private readonly IGovukNotifyProvider govukNotifyProvider;
        private readonly IConfiguration configuration;

        public GovukNotifyProviderTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();

            NotifyConfigurations notifyConfigurations = configuration
                .GetSection("notifyConfigurations").Get<NotifyConfigurations>();

            this.govukNotifyProvider = new GovukNotifyProvider(notifyConfigurations);
        }

        private static string GetRandomEmail() =>
            new EmailAddresses().GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomMobileNumber()
        {
            Random random = new Random();
            var randomNumberEnd = random.Next(100000000, 200000000).ToString();

            string randomNumber = $"07{randomNumberEnd}";

            return randomNumber;
        }
    }
}