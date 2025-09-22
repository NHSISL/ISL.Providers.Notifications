// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

using System.Collections.Generic;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models
{
    public class NotifyConfigurations
    {
        public string ApiKey { get; set; }
        public string EmailTemplateId { get; set; }
        public string InterceptingEmail { get; set; }
        public string InterceptingMobileNumber { get; set; }
        public NotificationOverride DefaultOverride { get; set; }
        public List<NotificationOverride> NotificationOverrides { get; set; } = new List<NotificationOverride>();
        public bool SubstituteDictionaryValues { get; set; }
        public string IdentifierKey { get; set; }
        public string EmailKey { get; set; }
        public string PhoneKey { get; set; }
        public string AddressLine1Key { get; set; }
        public string AddressLine2Key { get; set; }
        public string AddressLine3Key { get; set; }
        public string AddressLine4Key { get; set; }
        public string AddressLine5Key { get; set; }
        public string AddressLine6Key { get; set; }
        public string AddressLine7Key { get; set; }
    }
}
