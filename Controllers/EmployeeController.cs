using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.Interface;
using EmployeeManagementSystem.ModelDto;
using EmployeeManagementSystem.ServiceFilter;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data.Common;
using static EmployeeManagementSystem.Entity.EmployeeAdditionalDetails;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeInterface _employeeService;

        public EmployeeController(EmployeeInterface employeeService)
        {

            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<ModelEmployeeBasicDetails> AddEmployee(ModelEmployeeBasicDetails details)
        {
            var response = await _employeeService.AddEmployee(details);
            return response;
        }

        [HttpGet]
        public async Task<ModelEmployeeBasicDetails> GetEmployeeById(string ID)
        {
            var response = await _employeeService.GetEmployeeById(ID);
            return response;
        }

        [HttpPost]
        public async Task<ModelEmployeeBasicDetails> UpdateEmployee(ModelEmployeeBasicDetails employee)
        {
            var response = await _employeeService.UpdateEmployee(employee);
            return response;

        }


        [HttpPost]
        public async Task<string> DeleteEmployee(string Id)
        {
            var res = await _employeeService.DeleteEmployee(Id);
            return res;
        }

        [HttpGet]
        public async Task<List<ModelEmployeeBasicDetails>> getAllBasicDetails()
        {
            var res = await _employeeService.getAllBasicDetails();
            return res;
        }



        //Additinal details beloq Api
        [HttpPost]
        public async Task<ModelEmployeeAdditionalDetails> AddAdditinalDetails(ModelEmployeeAdditionalDetails details)
        {
            var response = await _employeeService.AddAdditinalDetails(details);
            return response;

        }


        [HttpGet]
        public async Task<ModelEmployeeAdditionalDetails> AdditinalDetailsGetById(string ID)
        {
            var response = await _employeeService.AdditinalDetailsGetById(ID);
            return response;
        }

        [HttpPost]
        public async Task<ModelEmployeeAdditionalDetails> UpdateAdditinalDetails(ModelEmployeeAdditionalDetails details)
        {
            var resp = await _employeeService.UpdateAdditinalDetails(details);
            return resp;
        }


        [HttpPost]
        public async Task<string> DeleteAdditaionalDetails(string ID)
        {
            var resp = await _employeeService.DeleteAdditaionalDetails(ID);
            return resp;

        }

        [HttpGet]
        public async Task<List<ModelEmployeeAdditionalDetails>> GetAllEmployees()
        {
            var response = await _employeeService.GetAllEmployees();
            return response;
        }



        // importExcel for basicDetails  

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IFormFile file)
        {
            if (file == null && file.Length == 0)
                return BadRequest("file is empty or null");

            var employees = new List<ModelEmployeeBasicDetails>();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowcount = worksheet.Dimension.Rows;

                    for (int row = 1; row <= rowcount; row++)
                    {
                        var emp = new ModelEmployeeBasicDetails
                        {
                            FirstName = GetStringFroceCell(worksheet, row, 1),
                            LastName = GetStringFroceCell(worksheet, row, 2),
                            Email = GetStringFroceCell(worksheet, row, 3),
                            Mobile = GetStringFroceCell(worksheet, row, 4),
                            ReportingManagerName = GetStringFroceCell(worksheet, row, 5)

                        };

                        await AddEmployee(emp);

                        employees.Add(emp);
                    }

                }
            }

            return Ok(employees);

        }

        private string? GetStringFroceCell(ExcelWorksheet worksheet, int row, int column)
        {
            var cellValue = worksheet.Cells[row, column].Value;
            return cellValue?.ToString()?.Trim();
        }

        //Export for BasicDetails
        [HttpGet]
        public async Task<IActionResult> Export()
        {
            var employees = await _employeeService.getAllBasicDetails();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("employees");

                //header
                worksheet.Cells[1, 1].Value = "First Name";
                worksheet.Cells[1, 2].Value = "Last Name";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Phone Number";
                worksheet.Cells[1, 5].Value = "Reporting Manager Name";

                //header style
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;

                }

                //add Employee details
                for (int i = 0; i < employees.Count; i++)
                {
                    var emp = employees[i];
                    worksheet.Cells[i + 2, 1].Value = emp.FirstName;
                    worksheet.Cells[i + 2, 2].Value = emp.LastName;
                    worksheet.Cells[i + 2, 3].Value = emp.Email;
                    worksheet.Cells[i + 2, 4].Value = emp.Mobile;
                    worksheet.Cells[i + 2, 5].Value = emp.ReportingManagerName;

                }

                var stream = new System.IO.MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                var fileName = "EmployeeDetails.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);


            }


        }

        // search and pagination

        [HttpGet]
        public async Task<IActionResult> GetAllEmployeeBynickName(string nickName)
        {
            var response = await _employeeService.GetAllEmployeeBynickName(nickName);
            return Ok(response);
        }

        //basic details 

        [HttpPost]
        //servicefilter
        [ServiceFilter(typeof(BuildEmployeeFilter))]
        public async Task<EmployeeFilterCriteria> GetAllEmployeeByPagination(EmployeeFilterCriteria employeeFilterCriteria)
        {
            var response = await _employeeService.GetAllEmployeeByPagination(employeeFilterCriteria);
            return response;
        }

        //additional details
        [HttpPost]
        [ServiceFilter(typeof(BuildEmployeeAdditinalFilter))]
        public async Task<EmployeeAdditionalFilterCriteria> GetAllAdditinalDetailsbyPagination(EmployeeAdditionalFilterCriteria employeeAdditionalFilterCriteria)
        {
            var res = await _employeeService.GetAllAdditinalDetailsbyPagination(employeeAdditionalFilterCriteria);
            return res;
        }


        //api to api calling (micro) itself basicDetails api

        [HttpPost]
        public async Task<IActionResult> AddBasicDetailsByMakePostRequest(ModelEmployeeBasicDetails details)
        {
            var response = await _employeeService.AddBasicDetailsByMakePostRequest(details);
            return Ok(response);
        }

        [HttpGet]
        public async Task<List<ModelEmployeeBasicDetails>> GetBasicDetailsByMakeGetRequest()
        {
            return await _employeeService.GetBasicDetailsByMakeGetRequest();
        }

        //api to api call microservice itself AdditionalDetails api

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAdditionalByMakePostRequest(ModelEmployeeAdditionalDetails details)
        {
            var response = await _employeeService.AddEmployeeAdditionalByMakePostRequest(details);
            return Ok(response);
        }

        [HttpGet]
        public async Task<List<ModelEmployeeAdditionalDetails>> GetEmployeeAdditionalByMakeRequest()
        {
            return await _employeeService.GetEmployeeAdditionalByMakeRequest();
        }


        // a new API which will demonstrate the use of MakePostRequest for other project api
        [HttpPost]
        public async Task<IActionResult> AddManagerbymakePostRequest(ManagerModelDto manager )
        {
            var res= await _employeeService.AddManagerbymakePostRequest(manager);
            return Ok(res);

        }

        //a new API which will demonstrate the use of MakeGetRequest for other project api
        [HttpGet]
        public async Task<List<ManagerModelDto>> GetManagerAllMakeGetrequest()
        {
            return await _employeeService.GetManagerAllMakeGetrequest();
        }

        //Export an excel which will contains all basic details + additional details.
        [HttpGet]
        public async Task<IActionResult> ExportAllAdditionalBasicDetails()
        {
           ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            using(var package = new ExcelPackage()) 
            {
                var worksheet = package.Workbook.Worksheets.Add("employees");

                //Header 
                //BasicDetails
                worksheet.Cells[1, 1].Value = "Employee Id";
                worksheet.Cells[1, 2].Value = "UID";
                worksheet.Cells[1, 3].Value = "Salutary";
                worksheet.Cells[1, 4].Value = "First Name";
                worksheet.Cells[1, 5].Value = "Middle Name";
                worksheet.Cells[1, 6].Value = "Last Name";
                worksheet.Cells[1, 7].Value = "Nick Name";
                worksheet.Cells[1, 8].Value = "Email";
                worksheet.Cells[1, 9].Value = "Role";
                worksheet.Cells[1, 10].Value = "Reporting Manager UID";
                worksheet.Cells[1, 11].Value = "Reporting Manager Name";
                worksheet.Cells[1, 12].Value = "Address";
                worksheet.Cells[1, 13].Value = "Mobile";


                //Additinal Field in header

                worksheet.Cells[1, 14].Value = "Employee BasicDetails UId";
                worksheet.Cells[1, 15].Value = "Alternate Email";
                worksheet.Cells[1, 16].Value = "Alternate Mobile";
                worksheet.Cells[1, 17].Value = "DesignationName";
                worksheet.Cells[1, 18].Value = "Department Name";
                worksheet.Cells[1, 19].Value = "Location Name";
                worksheet.Cells[1, 20].Value = "Employee Status";
                worksheet.Cells[1, 21].Value = "Source Of Hire";
                worksheet.Cells[1, 22].Value = "Date Of Joining";
                worksheet.Cells[1, 23].Value = "Date Of Birth";
                worksheet.Cells[1, 24].Value = "Age";
                worksheet.Cells[1, 25].Value = "Gender";
                worksheet.Cells[1, 26].Value = "Religion";
                worksheet.Cells[1, 27].Value = "Caste";
                worksheet.Cells[1, 28].Value = "Marital Status";
                worksheet.Cells[1, 29].Value = "Blood Group";
                worksheet.Cells[1, 30].Value = "Height";
                worksheet.Cells[1, 31].Value = "Weight";
                worksheet.Cells[1, 32].Value = "PAN";
                worksheet.Cells[1, 33].Value = "Aadhar";
                worksheet.Cells[1, 34].Value = "Nationality";
                worksheet.Cells[1, 35].Value = "Passport Number";
                worksheet.Cells[1, 36].Value = "PF Number";

                //All header style 
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;

                }

                //get all both details
                var basicDetails = await _employeeService.getAllBasicDetails();

                var AdditinalDetails = await _employeeService.GetAllEmployees();


                int i = 0;

                while (i < basicDetails.Count)
                {
                    var EmpA = basicDetails[i];

                    var EmpB = AdditinalDetails.FirstOrDefault();

                    worksheet.Cells[i + 2, 1].Value = EmpA.EmployeeID;
                    worksheet.Cells[i + 2, 2].Value = EmpA.UId;
                    worksheet.Cells[i + 2, 3].Value = EmpA.Salutory;
                    worksheet.Cells[i + 2, 4].Value = EmpA.FirstName;
                    worksheet.Cells[i + 2, 5].Value = EmpA.MiddleName;
                    worksheet.Cells[i + 2, 6].Value = EmpA.LastName;
                    worksheet.Cells[i + 2, 7].Value = EmpA.NickName;
                    worksheet.Cells[i + 2, 8].Value = EmpA.Email;
                    worksheet.Cells[i + 2, 9].Value = EmpA.Role;
                    worksheet.Cells[i + 2, 10].Value = EmpA.ReportingManagerUId;
                    worksheet.Cells[i + 2, 11].Value = EmpA.ReportingManagerName;
                    worksheet.Cells[i + 2, 12].Value = EmpA.Address;
                    worksheet.Cells[i + 2, 13].Value = EmpA.Mobile;

                    worksheet.Cells[i + 2, 14].Value = EmpB.EmployeeBasicDetailsUId;
                    worksheet.Cells[i + 2, 15].Value = EmpB.AlternateEmail;
                    worksheet.Cells[i + 2, 16].Value = EmpB.AlternateMobile;
                    worksheet.Cells[i + 2, 17].Value = EmpB.WorkInformation.DesignationName;
                    worksheet.Cells[i + 2, 18].Value = EmpB.WorkInformation.DepartmentName;
                    worksheet.Cells[i + 2, 19].Value = EmpB.WorkInformation.LocationName;
                    worksheet.Cells[i + 2, 20].Value = EmpB.WorkInformation.EmployeeStatus;
                    worksheet.Cells[i + 2, 21].Value = EmpB.WorkInformation.SourceOfHire;
                    worksheet.Cells[i + 2, 22].Value = EmpB.WorkInformation.DateOfJoining;
                    worksheet.Cells[i + 2, 23].Value = EmpB.PersonalDetails.DateOfBirth;
                    worksheet.Cells[i + 2, 24].Value = EmpB.PersonalDetails.Age;
                    worksheet.Cells[i + 2, 25].Value = EmpB.PersonalDetails.Gender;
                    worksheet.Cells[i + 2, 26].Value = EmpB.PersonalDetails.Religion;
                    worksheet.Cells[i + 2, 27].Value = EmpB.PersonalDetails.Caste;
                    worksheet.Cells[i + 2, 28].Value = EmpB.PersonalDetails.MaritalStatus;
                    worksheet.Cells[i + 2, 29].Value = EmpB.PersonalDetails.BloodGroup;
                    worksheet.Cells[i + 2, 30].Value = EmpB.PersonalDetails.Height;
                    worksheet.Cells[i + 2, 31].Value = EmpB.PersonalDetails.Weight;
                    worksheet.Cells[i + 2, 32].Value = EmpB.IdentityInformation.PAN;
                    worksheet.Cells[i + 2, 33].Value = EmpB.IdentityInformation.Aadhar;
                    worksheet.Cells[i + 2, 34].Value = EmpB.IdentityInformation.Nationality;
                    worksheet.Cells[i + 2, 35].Value = EmpB.IdentityInformation.PassportNumber;
                    worksheet.Cells[i + 2, 36].Value = EmpB.IdentityInformation.PFNumber;

                    i++;

                }

                var stream = new System.IO.MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                var fileName = "EmployeeAdditinal&BasicDetails.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);


            }


        }



        //GetEmployeeAdditionalDetailsByBasicDetailsUId in which you will pass the basic details UId using filterAttribute in payload.
        [HttpPost]
        public  async Task<ModelEmployeeAdditionalDetails> GetEmployeeAdditionalDetailsByBasicDetailsUId(FilterCriteriaAdditional filterCriteriaAdditional)
        {
            var response=await _employeeService.GetEmployeeAdditionalDetailsByBasicDetailsUId(filterCriteriaAdditional);
            return response;
        }

    }
}
