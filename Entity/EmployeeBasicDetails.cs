using EmployeeManagementSystem.Comman;
using EmployeeManagementSystem.ModelDto;
using Newtonsoft.Json;
using System.Net;
using System.Security.Policy;

namespace EmployeeManagementSystem.Entity
{
    public class EmployeeBasicDetails : BaseEntity
    {
        [JsonProperty(propertyName:"salutary",NullValueHandling =NullValueHandling.Ignore)]
        public string Salutory { get; set; }

        [JsonProperty(propertyName: "firstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty(propertyName: "middleName", NullValueHandling = NullValueHandling.Ignore)]
        public string MiddleName { get; set; }

        [JsonProperty(propertyName: "lastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty(propertyName: "nickName", NullValueHandling = NullValueHandling.Ignore)]
        public string NickName { get; set; }

        [JsonProperty(propertyName: "email", NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(propertyName: "mobile", NullValueHandling = NullValueHandling.Ignore)]
        public string Mobile { get; set; }

        [JsonProperty(propertyName: "employeeId", NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeID { get; set; }

        [JsonProperty(propertyName: "role", NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        [JsonProperty(propertyName: "reportingManagerUId", NullValueHandling = NullValueHandling.Ignore)]
        public string ReportingManagerUId { get; set; }

        [JsonProperty(propertyName: "reportingManagerName", NullValueHandling = NullValueHandling.Ignore)]
        public string ReportingManagerName { get; set; }

        [JsonProperty(propertyName: "address", NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty(propertyName: "status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
    }

    public class EmployeeFilterCriteria
    {
        public EmployeeFilterCriteria() 
        {

            Filters = new List<FilterCriteria>();
            employees = new List<ModelEmployeeBasicDetails>();
        }
        public int Page {  get; set; } //page number

        public int PageSize { get; set; }//num of record in 1 page
        public int TotalCount {  get; set; }// total record present in the data
        public List<FilterCriteria> Filters { get; set; }//Pass Filter

        public List<ModelEmployeeBasicDetails> employees { get; set; }


    }

    public class FilterCriteria
    {
        public string FieldName { get; set; }

        public string FieldValue { get; set; }
    }
}
