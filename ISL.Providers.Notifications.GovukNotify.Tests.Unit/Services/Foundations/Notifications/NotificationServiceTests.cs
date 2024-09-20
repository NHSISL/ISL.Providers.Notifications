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
    }
}
