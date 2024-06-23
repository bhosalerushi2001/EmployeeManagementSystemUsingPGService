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



        //for Excel

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


        //api calling to other project api (microservice)
        [HttpPost]
        public async Task<IActionResult> AddManagerbymakePostRequest(ManagerModelDto manager )
        {
            var res= await _employeeService.AddManagerbymakePostRequest(manager);
            return Ok(res);

        }

        [HttpGet]
        public async Task<List<ManagerModelDto>> GetManagerAllMakeGetrequest()
        {
            return await _employeeService.GetManagerAllMakeGetrequest();
        }


    }
}
