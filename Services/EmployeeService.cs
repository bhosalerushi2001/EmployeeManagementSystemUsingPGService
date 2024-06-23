using AutoMapper;
using EmployeeManagementSystem.Comman;
using EmployeeManagementSystem.CosmosDb;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Interface;
using EmployeeManagementSystem.ModelDto;
using Newtonsoft.Json;
using System.Net;
using System.Xml.Linq;
using static EmployeeManagementSystem.Entity.EmployeeAdditionalDetails;

namespace EmployeeManagementSystem.Services
{
    public class EmployeeService : EmployeeInterface
    {
        public readonly InterfaceCosmosDb _CosmosDb;
        public readonly IMapper _mapper;

        public EmployeeService(InterfaceCosmosDb cosmosDb,IMapper mapper) 
        {
            _CosmosDb = cosmosDb;
            _mapper = mapper;

        }

        public async Task<ModelEmployeeAdditionalDetails> AddAdditinalDetails(ModelEmployeeAdditionalDetails details)
        {
            var data=_mapper.Map<EmployeeAdditionalDetails>(details);
            data.Intialize(true, Credentials.EmployeeDocumentType, "Rushi", "rushi");

            var save = await _CosmosDb.AddAdditinalDetails(data);
            var model=_mapper.Map<ModelEmployeeAdditionalDetails>(save);
            return model;
        }

        public async Task<ModelEmployeeAdditionalDetails> AdditinalDetailsGetById(string iD)
        {
            var details= await _CosmosDb.AdditinalDetailsGetById(iD);
            var data = _mapper.Map<ModelEmployeeAdditionalDetails>(details);
            return data;

        }


        public async Task<ModelEmployeeAdditionalDetails> UpdateAdditinalDetails(ModelEmployeeAdditionalDetails details)
        {
            var data = await _CosmosDb.AdditinalDetailsGetById(details.Id);

            data.Active = false;
            data.Archived = true;

            await _CosmosDb.ReplaceDelAsync(data);
            data.Intialize(false, Credentials.EmployeeDocumentType, "rushi", "rushi");
            data.Active = false;

             var dt=_mapper.Map<EmployeeAdditionalDetails>(data);
            var save = await _CosmosDb.AddAdditinalDetails(dt);
            var model = _mapper.Map<ModelEmployeeAdditionalDetails>(save);
            return model;
        }

        public async Task<string> DeleteAdditaionalDetails(string ID)
        {
            var data = await _CosmosDb.AdditinalDetailsGetById(ID);

            data.Active = false;
            data.Archived= true;

            await _CosmosDb.ReplaceDelAsync(data);
            data.Intialize(false, Credentials.EmployeeDocumentType, "Rushi", "rushi");
            data.Active = false;


            await _CosmosDb.AddAdditinalDetails(data);
            return "Delete Additinal details Succesfulllll......";

        }

        public async Task<List<ModelEmployeeAdditionalDetails>> GetAllEmployees()
        {
            var data = await _CosmosDb.GetAllEmployees();
            
            var empList=new List<ModelEmployeeAdditionalDetails>();

            foreach(var item in data) 
            {
                var emp = _mapper.Map<ModelEmployeeAdditionalDetails>(item);
                
                empList.Add(emp);
            
            }
            return empList;

        }




        //basic Details
        public async Task<ModelEmployeeBasicDetails> AddEmployee(ModelEmployeeBasicDetails details)
        {
            var emp = _mapper.Map<EmployeeBasicDetails>(details);
            emp.Intialize(true, Credentials.EmployeeDocumentType, "rushi", "Rushi");
            var respnse = await _CosmosDb.AddEmployee(emp);
            var model=_mapper.Map<ModelEmployeeBasicDetails>(respnse);
            return model;
        }

       

        public async Task<string> DeleteEmployee(string id)
        {
           var emp= await _CosmosDb.GetEmployeeById(id);

            emp.Active = false;
            emp.Archived = true;

            await _CosmosDb.ReplaceDelAsync(emp);
            emp.Intialize(false, Credentials.EmployeeDocumentType, "rushi", "rushi");
            emp.Active = false;

            await _CosmosDb.AddEmployee(emp);
            return "delete succesfullll.......";
        }

        public async Task<ModelEmployeeBasicDetails> GetEmployeeById(string iD)
        {
            var emp= await _CosmosDb.GetEmployeeById(iD);
            var model = _mapper.Map<ModelEmployeeBasicDetails>(emp);
            return model;
        }

        public async Task<ModelEmployeeBasicDetails> UpdateEmployee(ModelEmployeeBasicDetails employee)
        {
            var emp= await _CosmosDb.GetEmployeeById(employee.Id);

            emp.Active = false;
            emp.Archived = true;

            await _CosmosDb.ReplaceDelAsync(employee);
            emp.Intialize(false, Credentials.EmployeeDocumentType, "rushi", "rushi");
            emp.Active = false;

            var data=_mapper.Map<EmployeeBasicDetails>(emp);
            var response = await _CosmosDb.AddEmployee(data);
            var model =_mapper.Map<ModelEmployeeBasicDetails>(response);

            return model;
        }

        public async Task<List<ModelEmployeeBasicDetails>> getAllBasicDetails()
        {
            var data = await _CosmosDb.getAllBasicDetails();

            var listDetails= new List<ModelEmployeeBasicDetails>();

            foreach(var item in data)
            {
                var model = _mapper.Map<ModelEmployeeBasicDetails>(item);
                listDetails.Add(model);

            }
            return listDetails;
        }

        //search
        public async Task<List<ModelEmployeeBasicDetails>> GetAllEmployeeBynickName(string nickName)
        {
            var allEmployees= await getAllBasicDetails();

            return allEmployees.FindAll(a => a.NickName == nickName);
        }

        //basicDetails Pagination
        public async Task<EmployeeFilterCriteria> GetAllEmployeeByPagination(EmployeeFilterCriteria employeeFilterCriteria)
        {
            EmployeeFilterCriteria responseObject = new EmployeeFilterCriteria();

            //filter ==>status
            var checkFilter = employeeFilterCriteria.Filters.Any(a => a.FieldName == "status");

            //var
            var status = "";
            if(checkFilter)
            {
                 status = employeeFilterCriteria.Filters.Find(a => a.FieldName == "status").FieldValue;
            }

            var employess=await getAllBasicDetails();

            var filterRecords =employess.FindAll(a => a.Status==status);

            responseObject.TotalCount = employess.Count;
            responseObject.Page=employeeFilterCriteria.Page;
            responseObject.PageSize=employeeFilterCriteria.PageSize;

            var skip=employeeFilterCriteria.PageSize * (employeeFilterCriteria.Page- 1);

            filterRecords= filterRecords.Skip(skip).Take(employeeFilterCriteria.PageSize).ToList();
           foreach(var item in employess)
            {
                responseObject.employees.Add(item);
            }
            return responseObject;
        }



        

        //calling add manager api by itself in additionalDetails

        public async Task<ModelEmployeeAdditionalDetails> AddEmployeeAdditionalByMakePostRequest(ModelEmployeeAdditionalDetails details)
        {         
            var serializable = JsonConvert.SerializeObject(details);
            var requestObj = await httpClientHelper.MakePostRequest(Credentials.EmployeeUrl, Credentials.AddEmployeeAdditinalEndPoint, serializable);
            var model = JsonConvert.DeserializeObject<ModelEmployeeAdditionalDetails>(requestObj);
            return model;
        }

        public async Task<List<ModelEmployeeAdditionalDetails>> GetEmployeeAdditionalByMakeRequest()
        {
            var request = await httpClientHelper.MakeGetRequest(Credentials.EmployeeUrl, Credentials.GetEmployeeEndPoint);
            return JsonConvert.DeserializeObject<List<ModelEmployeeAdditionalDetails>>(request);
        }


        //calling add manager api by itself in additionalDetails

        public async Task<ModelEmployeeBasicDetails> AddBasicDetailsByMakePostRequest(ModelEmployeeBasicDetails details)
        {
            var serializable=JsonConvert.SerializeObject(details);
            var request = await httpClientHelper.MakePostRequest(Credentials.EmployeeUrl, Credentials.AddBasicDetailsEndPoint, serializable);
            var model = JsonConvert.DeserializeObject<ModelEmployeeBasicDetails>(request);
            return model;
        }



        //api calling to other project api (microservice)

        public async Task<ManagerModelDto> AddManagerbymakePostRequest(ManagerModelDto visitor)
        {
          var serializable=JsonConvert.SerializeObject(visitor);
            var obj=await httpClientHelper.MakePostRequest(Credentials.ManagerUrl, Credentials.AddManagerEndPoint, serializable);
            var model=JsonConvert.DeserializeObject<ManagerModelDto>(obj);
            return model;

        }

        public async Task<List<ManagerModelDto>> GetManagerAllMakeGetrequest()
        {
            var request = await httpClientHelper.MakeGetRequest(Credentials.ManagerUrl, Credentials.GetAllManagerEndPoint);
            return JsonConvert.DeserializeObject<List<ManagerModelDto>>(request);
        }

        public async Task<List<ModelEmployeeBasicDetails>> GetBasicDetailsByMakeGetRequest()
        {
            var request = await httpClientHelper.MakeGetRequest(Credentials.EmployeeUrl, Credentials.GetAllBasicDetailsEndPoint);
            return JsonConvert.DeserializeObject<List<ModelEmployeeBasicDetails>>(request);
        }


        //Additinal Details 
        public async Task<EmployeeAdditionalFilterCriteria> GetAllAdditinalDetailsbyPagination(EmployeeAdditionalFilterCriteria employeeAdditionalFilterCriteria)
        {  

            EmployeeAdditionalFilterCriteria responseObject = new EmployeeAdditionalFilterCriteria();

            //filter ==>status
            var checkFilter = employeeAdditionalFilterCriteria.Filters.Any(a => a.FieldName == "status");

            //var
            var status = "";
            if (checkFilter)
            {
                status = employeeAdditionalFilterCriteria.Filters.Find(a => a.FieldName == "status").FieldValue;
            }

            var emp = await GetAllEmployees();

            var filterRecords = emp.FindAll(a => a.Status == status);

            responseObject.TotalCount = emp.Count;
            responseObject.Page = employeeAdditionalFilterCriteria.Page;
            responseObject.PageSize = employeeAdditionalFilterCriteria.PageSize;

            var skip = employeeAdditionalFilterCriteria.PageSize * (employeeAdditionalFilterCriteria.Page - 1);

            filterRecords = filterRecords.Skip(skip).Take(employeeAdditionalFilterCriteria.PageSize).ToList();
            foreach (var item in emp)
            {
                responseObject.Employees.Add(item);
            }
            return responseObject;
        }        
        
    }
    
}
