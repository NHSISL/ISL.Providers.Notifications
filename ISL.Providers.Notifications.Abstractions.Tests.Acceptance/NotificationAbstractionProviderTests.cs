// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovukNotify.Models;
using ISL.Providers.Notifications.GovukNotify.Providers.Notifications;
using Microsoft.Extensions.Configuration;
using System;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Notifications.Abstractions.Tests.Acceptance
{
    public partial class NotificationAbstractionProviderTests
    {
        private readonly INotificationAbstractionProvider notificationAbstractionProvider;
        private readonly IConfiguration configuration;
        private readonly string TEST_MOBILE_NUMBER = "07700900000";

        public NotificationAbstractionProviderTests()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            configuration = configurationBuilder.Build();

            NotifyConfigurations notifyConfigurations = configuration
                .GetSection("notifyConfigurations").Get<NotifyConfigurations>();

            INotificationProvider govukNotifyProvider = new GovukNotifyProvider(notifyConfigurations);
            this.notificationAbstractionProvider = new NotificationAbstractionProvider(govukNotifyProvider);
        }

        private static string GetRandomEmail() =>
            new EmailAddresses().GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}