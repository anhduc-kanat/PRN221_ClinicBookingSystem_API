using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Service;
using ClinicBookingSystem_Service.Models.Request.Slot;
using ClinicBookingSystem_Service.Models.Response.Service;
using ClinicBookingSystem_Service.Models.Response.Slot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.IService
{
    public interface IServiceService
    {
        public Task<BaseResponse<IEnumerable<GetServiceResponse>>> GetAllServices();
        public Task<BaseResponse<IEnumerable<GetServiceResponse>>> GetAllExamServices();
        Task<BaseResponse<IEnumerable<GetServiceResponse>>> GetAllTreatmentServices();
        public Task<BaseResponse<GetServiceResponse>> GetServiceById(int id);
        public Task<BaseResponse<CreateServiceResponse>> CreateService(CreateServiceRequest request);
        public Task<BaseResponse<DeleteServiceResponse>> DeleteService(int id);
        public Task<BaseResponse<UpdateServiceResponse>> UpdateService(int id, UpdateServiceRequest request);

        Task<BaseResponse<IEnumerable<KeyValuePair<string, int>>>> CountUseService();


    }
}