using System.Collections.Generic;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models
{
    public class NotificationOverride
    {
        public string Identifier { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<string> AddressLines { get; set; }
    }
}
