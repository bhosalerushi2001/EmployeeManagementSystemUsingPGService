using EmployeeManagementSystem.Comman;
using EmployeeManagementSystem.Entity;
using Microsoft.Azure.Cosmos;

namespace EmployeeManagementSystem.CosmosDb
{
    public class ServiceCosmosDb : InterfaceCosmosDb
    {
        public CosmosClient _cosmosClient;
        private readonly Container _container;

        public ServiceCosmosDb ()
        {
            _cosmosClient = new CosmosClient(Credentials.CosmosEndpoint, Credentials.PrimaryKey);
            _container=_cosmosClient.GetContainer(Credentials.DatabaseName,Credentials.ContainerName);
        }

        public async Task<EmployeeBasicDetails> AddEmployee(EmployeeBasicDetails emp)
        {
            var data= await _container.CreateItemAsync(emp);
            return data;
        }

        public async Task<EmployeeBasicDetails> GetEmployeeById(string iD)
        {
            var data =  _container.GetItemLinqQueryable<EmployeeBasicDetails>(true).Where(x=>x.Id==iD &&
              !x.Archived && x.Active && x.DocumentType==Credentials.EmployeeDocumentType).FirstOrDefault();
            return data;
        }

        public async Task ReplaceDelAsync(dynamic employee)
        {
            await _container.ReplaceItemAsync(employee,employee.Id);
        }

        public async Task<EmployeeAdditionalDetails> AddAdditinalDetails(EmployeeAdditionalDetails data)
        {
            var response= await _container.CreateItemAsync(data);
            return response;
        }

        public async Task<EmployeeAdditionalDetails> AdditinalDetailsGetById(string ID)
        {
            var response= _container.GetItemLinqQueryable<EmployeeAdditionalDetails>(true).Where(x=>x.Id== ID &&
            !x.Archived && x.Active && x.DocumentType==Credentials.EmployeeDocumentType).FirstOrDefault();
            return response;
        }

        public async Task<List<EmployeeAdditionalDetails>> GetAllEmployees()
        {
            var respnse = _container.GetItemLinqQueryable<EmployeeAdditionalDetails>(true).Where(x => x.Active == true
             && x.Archived == false && x.DocumentType == Credentials.EmployeeDocumentType).ToList();

            return respnse;
        }

        public async Task<List<EmployeeBasicDetails>> getAllBasicDetails()
        {
            var respnse = _container.GetItemLinqQueryable<EmployeeBasicDetails>(true).Where(x => x.Active == true
            && x.Archived == false && x.DocumentType == Credentials.EmployeeDocumentType).ToList();
            return respnse;
        }
    }
}
