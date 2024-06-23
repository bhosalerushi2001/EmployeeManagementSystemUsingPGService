using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.ModelDto;
using Microsoft.AspNetCore.Mvc;
using static EmployeeManagementSystem.Entity.EmployeeAdditionalDetails;

namespace EmployeeManagementSystem.Interface
{
    public interface EmployeeInterface
    {
        Task<ModelEmployeeAdditionalDetails> AddAdditinalDetails(ModelEmployeeAdditionalDetails details);
        Task<ModelEmployeeBasicDetails> AddEmployee(ModelEmployeeBasicDetails details);
        Task<ModelEmployeeAdditionalDetails> AdditinalDetailsGetById(string iD);
        Task<string> DeleteAdditaionalDetails(string ID);
        Task<string> DeleteEmployee(string id);
        Task<List<ModelEmployeeBasicDetails>> getAllBasicDetails();
        Task<List<ModelEmployeeAdditionalDetails>> GetAllEmployees();
        
        Task<ModelEmployeeBasicDetails> GetEmployeeById(string iD);
        Task<ModelEmployeeAdditionalDetails> UpdateAdditinalDetails(ModelEmployeeAdditionalDetails details);
        Task<ModelEmployeeBasicDetails> UpdateEmployee(ModelEmployeeBasicDetails employee);


        Task<EmployeeFilterCriteria> GetAllEmployeeByPagination(EmployeeFilterCriteria employeeFilterCriteria);
        Task<EmployeeAdditionalFilterCriteria> GetAllAdditinalDetailsbyPagination(EmployeeAdditionalFilterCriteria employeeAdditionalFilterCriteria);

        Task<List<ModelEmployeeBasicDetails>> GetAllEmployeeBynickName(string nickName);


        Task<ModelEmployeeAdditionalDetails> AddEmployeeAdditionalByMakePostRequest(ModelEmployeeAdditionalDetails details);
        Task<List<ModelEmployeeAdditionalDetails>> GetEmployeeAdditionalByMakeRequest();


        Task<ManagerModelDto> AddManagerbymakePostRequest(ManagerModelDto manager);
        Task<List<ManagerModelDto>> GetManagerAllMakeGetrequest();


        Task<ModelEmployeeBasicDetails> AddBasicDetailsByMakePostRequest(ModelEmployeeBasicDetails details);
        Task<List<ModelEmployeeBasicDetails>> GetBasicDetailsByMakeGetRequest();
    }
}
