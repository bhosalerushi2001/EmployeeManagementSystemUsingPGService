using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Comman
{
    public class BaseEntity
    {
        [JsonProperty(PropertyName = "id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "uId", NullValueHandling = NullValueHandling.Ignore)]
        public string UId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "dType", NullValueHandling = NullValueHandling.Ignore)]
        public string DocumentType { get; set; }

        [JsonProperty(PropertyName = "version", NullValueHandling = NullValueHandling.Ignore)]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "updatedBy", NullValueHandling = NullValueHandling.Ignore)]
        public string UpdatedBy { get; set; }

        [JsonProperty(PropertyName = "updatedOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime UpdatedOn { get; set; }

        [JsonProperty(PropertyName = "createdBy", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }

        [JsonProperty(PropertyName = "createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreatedOn { get; set; }

        [JsonProperty(PropertyName = "active", NullValueHandling = NullValueHandling.Ignore)]
        public bool Active { get; set; }

        [JsonProperty(PropertyName = "archived", NullValueHandling = NullValueHandling.Ignore)]
        public bool Archived { get; set; }

        public void Intialize(bool isNew, string dType, string CreatedOrUpdateBy, string CreatedOrUpdateByName)
        {
            DocumentType = dType;
            Id = Guid.NewGuid().ToString();
            Active = true;
            Archived = false;
            if (isNew)
            {
                //adding new record
                UId = Guid.NewGuid().ToString();
                CreatedBy = CreatedOrUpdateBy;
                CreatedOn = DateTime.Now;
                Version = 1;
                UpdatedBy = CreatedOrUpdateBy;
                UpdatedOn = CreatedOn;

            }
            else
            {
                //update the record
                UpdatedBy = CreatedOrUpdateBy;
                UpdatedOn = DateTime.Now;
                Version++;
            }
        }
    }
}
