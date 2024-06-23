namespace EmployeeManagementSystem.Comman
{
    public class Credentials
    {

        public static readonly string DatabaseName = Environment.GetEnvironmentVariable("databaseName");

        public static readonly string ContainerName = Environment.GetEnvironmentVariable("containerName");

        public static readonly string CosmosEndpoint = Environment.GetEnvironmentVariable("cosmosUrl");

        public static readonly string PrimaryKey = Environment.GetEnvironmentVariable("primaryKey");


        //self api call in additionaldetails

        public static readonly string EmployeeUrl=Environment.GetEnvironmentVariable("employeeUrl");

        public static readonly string AddEmployeeAdditinalEndPoint = "/api/Employee/AddAdditinalDetails";

        public static readonly string GetEmployeeEndPoint = "/api/Employee/GetAllEmployees";


        //Document Type
        public static readonly string EmployeeDocumentType ="Employee";



        //other project api call (microservice)
        internal static readonly string ManagerUrl = Environment.GetEnvironmentVariable("managerUrl");

        internal static readonly string AddManagerEndPoint = "/api/Home/CreateManager";

        internal static readonly string GetAllManagerEndPoint = "/api/Home/managerGetAll";


        //self api call in additionaldetails
        internal static readonly string AddBasicDetailsEndPoint = "/api/Employee/AddEmployee";

        internal static readonly string GetAllBasicDetailsEndPoint = "/api/Employee/getAllBasicDetails";
    }
}
