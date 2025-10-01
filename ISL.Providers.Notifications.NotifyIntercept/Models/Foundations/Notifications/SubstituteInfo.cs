using System.Collections.Generic;

namespace ISL.Providers.Notifications.NotifyIntercept.Models.Foundations.Notifications
{
    public class SubstituteInfo
    {
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public List<string> AddressLines { get; set; } = new List<string>();
        public Dictionary<string, dynamic> Personalisation { get; set; } = new Dictionary<string, dynamic>();
    }
}
