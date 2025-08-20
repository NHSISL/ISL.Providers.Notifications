// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ISL.Providers.Notifications.GovukNotify.Brokers;
using ISL.Providers.Notifications.GovukNotify.Models;
using ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Notifications.GovukNotify.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        private readonly Mock<IGovukNotifyBroker> govukNotifyBroker;
        private readonly NotifyConfigurations configurations;
        private readonly NotificationService notificationService;
        private readonly ICompareLogic compareLogic;

        public NotificationServiceTests()
        {
            this.govukNotifyBroker = new Mock<IGovukNotifyBroker>();
            this.configurations = GetRandomConfigurations();
            this.compareLogic = new CompareLogic();
            this.notificationService = new NotificationService(govukNotifyBroker.Object, configurations);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomEmailAddress() =>
            new EmailAddresses().GetValue();

        private static string GetRandomLocalMobileNumber()
        {
            Random random = new Random();
            var randomNumberEnd = random.Next(100000000, 200000000).ToString();
            string randomNumber = $"07{randomNumberEnd}";

            return randomNumber;
        }

        private static string GetRandomInternationalMobileNumber()
        {
            Random random = new Random();
            var randomNumberEnd = random.Next(100000000, 200000000).ToString();
            string randomNumber = $"+447{randomNumberEnd}";

            return randomNumber;
        }

        private static NotifyConfigurations GetRandomConfigurations() =>
            CreateNotifyConfigurationsFiller().Create();

        private static Filler<NotifyConfigurations> CreateNotifyConfigurationsFiller()
        {
            var filler = new Filler<NotifyConfigurations>();
            filler.Setup();

            return filler;
        }

        private Expression<Func<Dictionary<string, dynamic>, bool>> SameDictionaryAs(
            Dictionary<string, dynamic> expectedDictionary) =>
            actualDictionary => this.compareLogic.Compare(expectedDictionary, actualDictionary).AreEqual;

        public static TheoryData<Exception> DependencyValidationExceptions()
        {
            return new TheoryData<Exception>
            {
                new Notify.Exceptions.NotifyClientException(
                    message: "Can't send to this recipient using a team-only API key"),

                new Notify.Exceptions.NotifyClientException(
                    message: "Can't send to this recipient when service is in trial mode - " +
                        "see https://www.notifications.service.gov.uk/trial-mode"),

                new Notify.Exceptions.NotifyClientException(
                    message: "File did not pass the virus scan"),

                new Notify.Exceptions.NotifyClientException(
                    message: "Error: Your system clock must be accurate to within 30 seconds"),

                new Notify.Exceptions.NotifyClientException(
                    message: "Invalid token: API key not found"),

                new Notify.Exceptions.NotifyClientException(
                    message: "Exceeded rate limit for key type TEAM/TEST/LIVE of 3000 requests per 60 seconds"),

                new Notify.Exceptions.NotifyClientException(
                    message: "Exceeded send limits (LIMIT NUMBER) for today"),

                new Notify.Exceptions.NotifyClientException(
                    message: "precompiledPDF must be a valid PDF file"),

                new Notify.Exceptions.NotifyClientException(
                    message: "reference cannot be null or empty"),

                new Notify.Exceptions.NotifyClientException(
                    message: "precompiledPDF cannot be null or empty"),
            };
        }

        public static TheoryData<Exception> DependencyExceptions()
        {
            return new TheoryData<Exception>
            {
                new Notify.Exceptions.NotifyClientException(
                    message: "Internal server error"),
            };
        }
    }
}
