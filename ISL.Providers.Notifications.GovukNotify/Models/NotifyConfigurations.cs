// ---------------------------------------------------------
// Copyright (c) North East London ICB. All rights reserved.
// ---------------------------------------------------------

namespace ISL.Providers.Notifications.GovukNotify.Models
{
    public class NotifyConfigurations
    {
        public string ApiKey { get; set; }
        public string EmailTemplateId { get; set; }
        public string SmsTemplateId { get; set; }
        public string LetterTemplateId { get; set; }
    }
}
