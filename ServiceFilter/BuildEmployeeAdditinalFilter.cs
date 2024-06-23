using EmployeeManagementSystem.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static EmployeeManagementSystem.Entity.EmployeeAdditionalDetails;

namespace EmployeeManagementSystem.ServiceFilter
{
    public class BuildEmployeeAdditinalFilter:IAsyncActionFilter
    {

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is EmployeeAdditionalFilterCriteria);
            if (param.Value == null)
            {
                context.Result = new BadRequestObjectResult("object is null");
                return;
            }

            EmployeeAdditionalFilterCriteria filterCriteria = (EmployeeAdditionalFilterCriteria)param.Value;

            var StatusFilter = filterCriteria.Filters.Find(a => a.FieldName == "status");
            if (StatusFilter == null)
            {
                StatusFilter = new FilterCriteria();
                StatusFilter.FieldName = "status";
                StatusFilter.FieldValue = "Active";
                filterCriteria.Filters.Add(StatusFilter);

            }

            filterCriteria.Filters.RemoveAll(a => string.IsNullOrEmpty(a.FieldName));
            var result = await next();
        }
    }
}
