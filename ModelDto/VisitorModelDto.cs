using Newtonsoft.Json;

namespace EmployeeManagementSystem.ModelDto
{
    public class VisitorModelDto
    {
        [JsonProperty(PropertyName = "uId", NullValueHandling = NullValueHandling.Ignore)]
        public string UID { get; set; }

        [JsonProperty(PropertyName = "name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "phonenumber", NullValueHandling = NullValueHandling.Ignore)]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "visittocomapanyname", NullValueHandling = NullValueHandling.Ignore)]
        public string VisitToComapanyName { get; set; }

        [JsonProperty(PropertyName = "purpose", NullValueHandling = NullValueHandling.Ignore)]
        public string purpose { get; set; }

        [JsonProperty(PropertyName = "entrytime", NullValueHandling = NullValueHandling.Ignore)]
        public string EntryTime { get; set; }

        [JsonProperty(PropertyName = "exittime", NullValueHandling = NullValueHandling.Ignore)]
        public string ExitTime { get; set; }
    }
}
