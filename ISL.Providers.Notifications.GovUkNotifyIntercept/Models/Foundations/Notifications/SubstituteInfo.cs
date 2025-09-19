using System.Collections.Generic;

namespace ISL.Providers.Notifications.GovUkNotifyIntercept.Models.Foundations.Notifications
{
    public class SubstituteInfo
    {
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public List<string> AddressLines { get; set; } = new List<string>();
        public Dictionary<string, dynamic> Overrides { get; set; } = new Dictionary<string, dynamic>();
    }
}
