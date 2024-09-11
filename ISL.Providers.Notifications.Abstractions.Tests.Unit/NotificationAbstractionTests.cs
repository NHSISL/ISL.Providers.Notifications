// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using Moq;

namespace ISL.Providers.Notifications.Abstractions.Tests.Unit
{
    public partial class NotificationAbstractionTests
    {
        private readonly Mock<INotificationProvider> notificationProviderMock;
        private readonly NotificationAbstractionProvider notificationAbstractionProvider;

        public NotificationAbstractionTests()
        {
            this.notificationProviderMock = new Mock<INotificationProvider>();

            this.notificationAbstractionProvider =
                new NotificationAbstractionProvider(this.notificationProviderMock.Object);
        }
    }
}
