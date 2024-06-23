using AutoMapper;
using EmployeeManagementSystem.Entity;
using EmployeeManagementSystem.ModelDto;

namespace EmployeeManagementSystem.Comman
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {

            CreateMap<EmployeeManagementSystem.Entity.WorkInfo_,EmployeeManagementSystem.ModelDto.Model_WorkInfo_>().ReverseMap();

            CreateMap<EmployeeManagementSystem.Entity.PersonalDetails_, EmployeeManagementSystem.ModelDto.Model_PersonalDetails_>().ReverseMap();

            CreateMap<EmployeeManagementSystem.Entity.IdentityInfo_, EmployeeManagementSystem.ModelDto.Model_IdentityInfo_>().ReverseMap();


            CreateMap<EmployeeAdditionalDetails,ModelEmployeeAdditionalDetails>().ReverseMap();

            CreateMap<EmployeeBasicDetails, ModelEmployeeBasicDetails>().ReverseMap();

        }

    }
}
