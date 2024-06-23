using Newtonsoft.Json;

namespace EmployeeManagementSystem.Entity
{
    public class PersonalDetails_
    {
        [JsonProperty(propertyName: "dateOfBirth", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty(propertyName: "age", NullValueHandling = NullValueHandling.Ignore)]
        public string Age { get; set; }

        [JsonProperty(propertyName: "gender", NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; set; }

        [JsonProperty(propertyName: "religion", NullValueHandling = NullValueHandling.Ignore)]
        public string Religion { get; set; }

        [JsonProperty(propertyName: "caste", NullValueHandling = NullValueHandling.Ignore)]
        public string Caste { get; set; }

        [JsonProperty(propertyName: "maritalStatus", NullValueHandling = NullValueHandling.Ignore)]
        public string MaritalStatus { get; set; }

        [JsonProperty(propertyName: "bloodGroup", NullValueHandling = NullValueHandling.Ignore)]
        public string BloodGroup { get; set; }

        [JsonProperty(propertyName: "height", NullValueHandling = NullValueHandling.Ignore)]
        public string Height { get; set; }

        [JsonProperty(propertyName: "weight", NullValueHandling = NullValueHandling.Ignore)]
        public string Weight { get; set; }
    }
}
