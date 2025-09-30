// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.Abstractions;
using ISL.Providers.Notifications.NotifyIntercept.Models;
using ISL.Providers.Notifications.NotifyIntercept.Providers.Notifications;
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

            this.notificationProvider = new Mock<INotificationProvider>();

            this.notifyInterceptProvider = new NotifyInterceptProvider(
                notifyConfigurations,
                notificationProvider: notificationProvider.Object);
        }

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();
    }
}