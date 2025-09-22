// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Brokers;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications;
using ISL.Providers.Notifications.GovUkNotifyIntercept.Services.Foundations.Notifications;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        private readonly Mock<IGovUkNotifyBroker> govukNotifyBroker;
        private readonly NotifyConfigurations configurations;
        private readonly NotificationService notificationService;
        private readonly ICompareLogic compareLogic;

        public NotificationServiceTests()
        {
            this.govukNotifyBroker = new Mock<IGovUkNotifyBroker>();
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

        private static NotificationOverride GetRandomNotificationOverride(string identifier = null) =>
            CreateNotificationOverrideFiller(identifier).Create();

        private static Filler<NotificationOverride> CreateNotificationOverrideFiller(string identifier = null)
        {
            if (identifier is null)
            {
                identifier = GetRandomString();
            }

            var filler = new Filler<NotificationOverride>();

            filler.Setup()
                .OnProperty(overrideConfig => overrideConfig.AddressLines).Use(GetRandomAddressLines())
                .OnProperty(overrideConfig => overrideConfig.Email).Use(GetRandomEmailAddress())
                .OnProperty(overrideConfig => overrideConfig.Phone).Use(GetRandomLocalMobileNumber());

            return filler;
        }

        private static List<string> GetRandomAddressLines()
        {
            var addressLines = new List<string>();

            for (int i = 0; i < 7; i++)
            {
                addressLines.Add(GetRandomString());
            }

            return addressLines;
        }

        private static NotifyConfigurations GetRandomConfigurations() =>
            CreateNotifyConfigurationsFiller().Create();

        private static Filler<NotifyConfigurations> CreateNotifyConfigurationsFiller()
        {
            var filler = new Filler<NotifyConfigurations>();

            filler.Setup()
                .OnProperty(config => config.InterceptingEmail).Use(GetRandomEmailAddress())
                .OnProperty(config => config.DefaultOverride).Use(GetRandomNotificationOverride());

            return filler;
        }

        private static SubstituteInfo GetRandomSubstituteInfo(Dictionary<string, dynamic> dictionary) =>
            CreateSubstituteInfoFiller(dictionary).Create();

        private static Filler<SubstituteInfo> CreateSubstituteInfoFiller(Dictionary<string, dynamic> dictionary)
        {
            var filler = new Filler<SubstituteInfo>();
            filler.Setup()
                .OnProperty(substituteInfo => substituteInfo.Personalisation).Use(dictionary);

            return filler;
        }

        private static Dictionary<string, dynamic> GetPersonalisationDictionaryForSubstitute(
            NotifyConfigurations notifyConfigurations,
            string identifier)
        {
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>
            {
                { notifyConfigurations.IdentifierKey, identifier }
            };

            return personalisation;
        }

        private static SubstituteInfo GetExpectedSubstituteInfo(
            NotifyConfigurations notifyConfigurations,
            Dictionary<string, dynamic> personalisation,
            NotificationOverride notificationOverride = null)
        {
            string mobileNumber = notificationOverride is not null ?
                notificationOverride.Phone : notifyConfigurations.DefaultOverride.Phone;

            string email = notificationOverride is not null ?
                notificationOverride.Email : notifyConfigurations.DefaultOverride.Email;

            List<string> addressLines = notificationOverride is not null ?
                notificationOverride.AddressLines : notifyConfigurations.DefaultOverride.AddressLines;

            if (notifyConfigurations.SubstituteDictionaryValues)
            {
                personalisation[notifyConfigurations.PhoneKey] = mobileNumber;
                personalisation[notifyConfigurations.EmailKey] = email;

                personalisation[notifyConfigurations.AddressLine1Key] =
                    addressLines.ElementAtOrDefault(0) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine2Key] =
                    addressLines.ElementAtOrDefault(1) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine3Key] =
                    addressLines.ElementAtOrDefault(2) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine4Key] =
                    addressLines.ElementAtOrDefault(3) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine5Key] =
                    addressLines.ElementAtOrDefault(4) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine6Key] =
                    addressLines.ElementAtOrDefault(5) ?? string.Empty;

                personalisation[notifyConfigurations.AddressLine7Key] =
                    addressLines.ElementAtOrDefault(6) ?? string.Empty;
            }

            SubstituteInfo substituteInfo = new SubstituteInfo
            {
                MobileNumber = mobileNumber,
                Email = email,
                AddressLines = addressLines,
                Personalisation = personalisation
            };

            return substituteInfo;
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
