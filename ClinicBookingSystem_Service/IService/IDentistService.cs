using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Service.Models.Request.Dentist;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Dentist;
using ClinicBookingSystem_Service.Models.Request.User;
using ClinicBookingSystem_Service.Models.Response.Dentist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBookingSystem_Service.IServices
{
    public interface IDentistService
    {
        Task<BaseResponse<IEnumerable<GetAllDentistsResponse>>> GetAllDentists();
        Task<BaseResponse<IEnumerable<GetAllDentistsResponse>>> GetAllDentistsByService(string serviceName);
        Task<BaseResponse<IEnumerable<GetAllDentistsResponse>>> GetAllDentistsByServiceId(int serviceId);


        Task<BaseResponse<GetDentistByIdResponse>> GetDentistById(int id);
        Task<BaseResponse<CreateDentistResponse>> CreateDentist(CreateDentistRequest request);
        Task<BaseResponse<UpdateDentistResponse>> UpdateDentist(int id ,UpdateDentistRequest request);
        Task<BaseResponse<DeleteDentistResponse>> DeleteDentist(int id);
        Task<BaseResponse<IEnumerable<DateTime>>> GetAvailableDate(int id);

        Task<BaseResponse<AddDentistToBusinessServiceResponse>> AddDentistToService(int dentistId,
            int businessServiceId);

        Task<BaseResponse<AddDentistToSpecificationResponse>> UpdateDentistAndSpecification(int dentistId,
            UpdateDentistAndSpecificationRequest request);

        Task<BaseResponse<IEnumerable<GetAllDentistsResponse>>> GetDentistBySpecificationId(int specificationId);
    }
}