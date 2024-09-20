// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using ISL.Providers.Notifications.GovukNotify.Brokers;
using ISL.Providers.Notifications.GovukNotify.Services.Foundations.Notifications;
using Moq;
using Tynamix.ObjectFiller;

namespace ISL.Providers.Notifications.GovukNotify.Tests.Unit.Services.Foundations.Notifications
{
    public partial class NotificationServiceTests
    {
        private readonly Mock<IGovukNotifyBroker> govukNotifyBroker;
        private readonly NotificationService notificationService;

        public NotificationServiceTests()
        {
            this.govukNotifyBroker = new Mock<IGovukNotifyBroker>();
            this.notificationService = new NotificationService(this.govukNotifyBroker.Object);
        }

        private static int GetRandomNumber() =>
            new IntRange(min: 2, max: 10).GetValue();

        private static string GetRandomString() =>
            new MnemonicString(wordCount: GetRandomNumber()).GetValue();

        private static string GetRandomEmailAddress() =>
            new EmailAddresses().GetValue();
    }
}
