using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Salary;
using ClinicBookingSystem_Service.Models.Request.Specification;
using ClinicBookingSystem_Service.Models.Response.Salary;
using ClinicBookingSystem_Service.Models.Response.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.IService
{
    public interface ISpecificationService
    {
        public Task<BaseResponse<IEnumerable<GetSpecificationDetailResponse>>> GetAllSpecifications();
        public Task<BaseResponse<GetSpecificationDetailResponse>> GetSpecificationById(int id);
        public Task<BaseResponse<CreateSpecificationResponse>> CreateSpecification(CreateSpecificationRequest request);
        public Task<BaseResponse<DeleteSpecificationResponse>> DeleteSpecification(int id);
        public Task<BaseResponse<UpdateSpecificationResponse>> UpdateSpecification(int id, UpdateSpecificationRequest request);
    }
}
