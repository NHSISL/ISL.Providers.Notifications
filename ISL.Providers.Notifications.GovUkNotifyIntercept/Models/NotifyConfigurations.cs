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
        public List<string> InterceptingAddressLines { get; set; }
    }
}
