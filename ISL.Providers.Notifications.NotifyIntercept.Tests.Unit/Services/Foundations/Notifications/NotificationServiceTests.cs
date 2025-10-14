// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ISL.Providers.Notifications.NotifyIntercept.Brokers;
using ISL.Providers.Notifications.NotifyIntercept.Models;
using ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications;
using ISL.Providers.Notifications.NotifyIntercept.Services.Foundations.Notifications;
using KellermanSoftware.CompareNetObjects;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Notifications.NotifyIntercept.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        private readonly Mock<IInterceptBroker> interceptBroker;
        private readonly NotifyConfigurations configurations;
        private readonly NotificationService notificationService;
        private readonly ICompareLogic compareLogic;
        private readonly int MaxAddressLines = 7;
        public NotificationServiceTests()
        {
            this.interceptBroker = new Mock<IInterceptBroker>();
            this.configurations = GetRandomConfigurations();
            this.compareLogic = new CompareLogic();
            this.notificationService = new NotificationService(interceptBroker.Object, configurations);
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
                .OnProperty(overrideConfig => overrideConfig.Identifier).Use(identifier)
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
            string identifier,
            string addressLine = null)
        {
            if (addressLine is null)
            {
                addressLine = GetRandomString();
            }
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>
            {
                { notifyConfigurations.IdentifierKey, identifier },
                { notifyConfigurations.AddressLine1Key, addressLine },
                { notifyConfigurations.AddressLine2Key, addressLine },
                { notifyConfigurations.AddressLine3Key, addressLine },
                { notifyConfigurations.AddressLine4Key, addressLine },
                { notifyConfigurations.AddressLine5Key, addressLine },
                { notifyConfigurations.AddressLine6Key, addressLine },
                { notifyConfigurations.AddressLine7Key, addressLine }
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

            var addressParts = new List<string>
            {
                notifyConfigurations.DefaultOverride.RecipientName,
                notifyConfigurations.DefaultOverride.AddressLine1,
                notifyConfigurations.DefaultOverride.AddressLine2,
                notifyConfigurations.DefaultOverride.AddressLine3,
                notifyConfigurations.DefaultOverride.AddressLine4,
                notifyConfigurations.DefaultOverride.AddressLine5,
                notifyConfigurations.DefaultOverride.PostCode
            };

            if (notificationOverride is not null)
            {
                addressParts = new List<string>
                {
                    notificationOverride.RecipientName ?? notifyConfigurations.DefaultOverride.RecipientName,
                    notificationOverride.AddressLine1 ?? notifyConfigurations.DefaultOverride.AddressLine1,
                    notificationOverride.AddressLine2 ?? notifyConfigurations.DefaultOverride.AddressLine2,
                    notificationOverride.AddressLine3 ?? notifyConfigurations.DefaultOverride.AddressLine3,
                    notificationOverride.AddressLine4 ?? notifyConfigurations.DefaultOverride.AddressLine4,
                    notificationOverride.AddressLine5 ?? notifyConfigurations.DefaultOverride.AddressLine5,
                    notificationOverride.PostCode ?? notifyConfigurations.DefaultOverride.PostCode
                };
            }

            if (notifyConfigurations.SubstituteDictionaryValues)
            {
                personalisation[notifyConfigurations.PhoneKey] = mobileNumber;
                personalisation[notifyConfigurations.EmailKey] = email;
            }

            var populatedAddressParts = addressParts.Where(part => !string.IsNullOrWhiteSpace(part)).ToList();

            var addressKeys = new List<string>
            {
                notifyConfigurations.AddressLine1Key,
                notifyConfigurations.AddressLine2Key,
                notifyConfigurations.AddressLine3Key,
                notifyConfigurations.AddressLine4Key,
                notifyConfigurations.AddressLine5Key,
                notifyConfigurations.AddressLine6Key,
                notifyConfigurations.AddressLine7Key
            };

            int linesToProcess = Math.Min(populatedAddressParts.Count, addressKeys.Count);

            for (int i = 0; i < linesToProcess; i++)
            {
                if (!string.IsNullOrWhiteSpace(addressKeys[i]))
                {
                    personalisation[addressKeys[i]] = populatedAddressParts[i];
                }
            }

            SubstituteInfo substituteInfo = new SubstituteInfo
            {
                MobileNumber = mobileNumber,
                Email = email,
                AddressLines = populatedAddressParts,
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

        public static TheoryData<List<string>> InvalidLists()
        {
            return new TheoryData<List<string>>
            {
                null,
                new List<string>()
            };
        }

        private Dictionary<string, dynamic> GetSubstitutedLetterDictionary(Dictionary<string, dynamic> personalisation)
        {
            NotificationOverride defaultOverride = configurations.DefaultOverride;

            personalisation.Add(configurations.AddressLine1Key, defaultOverride.RecipientName);
            personalisation.Add(configurations.AddressLine2Key, defaultOverride.AddressLine1);
            personalisation.Add(configurations.AddressLine3Key, defaultOverride.AddressLine2);
            personalisation.Add(configurations.AddressLine4Key, defaultOverride.AddressLine3);
            personalisation.Add(configurations.AddressLine5Key, defaultOverride.AddressLine4);
            personalisation.Add(configurations.AddressLine6Key, defaultOverride.AddressLine5);
            personalisation.Add(configurations.AddressLine7Key, defaultOverride.PostCode);

            return personalisation;
        }
    }
}
