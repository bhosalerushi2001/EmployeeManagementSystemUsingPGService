using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.ModelDto;

namespace EmployeeManagementSystem.CosmosDb
{
    public interface InterfaceCosmosDb
    {
        Task<EmployeeAdditionalDetails> AddAdditinalDetails(EmployeeAdditionalDetails data);
        Task<EmployeeBasicDetails> AddEmployee(EmployeeBasicDetails emp);
        Task<EmployeeAdditionalDetails> AdditinalDetailsGetById(string ID);
        
        Task<EmployeeBasicDetails> GetEmployeeById(string iD);
        Task ReplaceDelAsync(dynamic employee);
        Task<List<EmployeeAdditionalDetails>> GetAllEmployees();
        Task<List<EmployeeBasicDetails>> getAllBasicDetails();
    }
}
