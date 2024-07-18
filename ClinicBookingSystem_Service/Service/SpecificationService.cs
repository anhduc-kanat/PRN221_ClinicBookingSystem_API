using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Specification;
using ClinicBookingSystem_Service.Models.Response.Salary;
using ClinicBookingSystem_Service.Models.Response.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBookingSystem_Service.CustomException;
using ClinicBookingSystem_Service.Models.Response.Service;

namespace ClinicBookingSystem_Service.Service
{
    public class SpecificationService : ISpecificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SpecificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponse<CreateSpecificationResponse>> CreateSpecification(CreateSpecificationRequest request)
        {
            var specification = _mapper.Map<Specification>(request);
            if(specification == null) throw new CoreException("Specification is null", StatusCodeEnum.BadRequest_400);
            
            ICollection<BusinessService> businessServices = new List<BusinessService>();
            foreach (var bsId in request.BusinessServiceId)
            {
                var businessService = await _unitOfWork.ServiceRepository.GetServiceById(bsId);
                if(businessService == null) throw new CoreException("Some business service not found", StatusCodeEnum.BadRequest_400);
                businessServices.Add(businessService);
            }
            specification.BusinessServices = businessServices;
            
            await _unitOfWork.SpecificationRepository.AddAsync(specification);
            await _unitOfWork.SaveChangesAsync();
            var newSpecificationDto = _mapper.Map<CreateSpecificationResponse>(specification);
            return new BaseResponse<CreateSpecificationResponse>("Add specification successfully", StatusCodeEnum.OK_200, newSpecificationDto);
        }

        public async Task<BaseResponse<DeleteSpecificationResponse>> DeleteSpecification(int id)
        {
            var specification = await _unitOfWork.SpecificationRepository.GetSpecificationById(id);
            await _unitOfWork.SpecificationRepository.DeleteAsync(specification);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<DeleteSpecificationResponse>(specification);
            return new BaseResponse<DeleteSpecificationResponse>("Delete specification successfully", StatusCodeEnum.OK_200, result);
        }

        public async Task<BaseResponse<IEnumerable<GetSpecificationDetailResponse>>> GetAllSpecifications()
        {
            IEnumerable<Specification> specifications = await _unitOfWork.SpecificationRepository.GetAllSpecifications();
            var specificationsDto = _mapper.Map<IEnumerable<GetSpecificationDetailResponse>>(specifications);
            return new BaseResponse<IEnumerable<GetSpecificationDetailResponse>>("Get specifications successfully", StatusCodeEnum.OK_200,
                specificationsDto);
        }

        public async Task<BaseResponse<GetSpecificationDetailResponse>> GetSpecificationById(int id)
        {
            var businessServices = await _unitOfWork.ServiceRepository.GetServicesBySpecification(id);
            var specification = await _unitOfWork.SpecificationRepository.GetSpecificationById(id);
            var specificationDto = _mapper.Map<GetSpecificationDetailResponse>(specification);
            specificationDto.BusinessService = businessServices.Select(bs => _mapper.Map<GetServiceResponse>(bs)).ToList();
            return new BaseResponse<GetSpecificationDetailResponse>("Get specification by id successfully", StatusCodeEnum.OK_200, specificationDto);
        }

        public async Task<BaseResponse<UpdateSpecificationResponse>> UpdateSpecification(int id, UpdateSpecificationRequest request)
        {
            var existSpecification = await _unitOfWork.SpecificationRepository.GetSpecificationById(id);
            if(existSpecification == null) throw new CoreException("Specification not found", StatusCodeEnum.BadRequest_400);
            
            IList<BusinessService> bs = new List<BusinessService>();
            existSpecification.BusinessServices.Clear();
            foreach (var bsId in request.BusinessServiceId)
            {
                var businessService = await _unitOfWork.ServiceRepository.GetServiceById(bsId);
                if(businessService == null) throw new CoreException("Some business service not found", StatusCodeEnum.BadRequest_400);
                bs.Add(businessService);
            }
            var currentBusinessServices = await _unitOfWork.ServiceRepository.GetServicesBySpecification(id);
            foreach (var businessService in currentBusinessServices)
            {
                businessService.Specification = null;
            }
            existSpecification.BusinessServices = bs;
            _mapper.Map(request, existSpecification);
            await _unitOfWork.SpecificationRepository.UpdateAsync(existSpecification);
            await _unitOfWork.SaveChangesAsync();
            var result = _mapper.Map<UpdateSpecificationResponse>(existSpecification);
            return new BaseResponse<UpdateSpecificationResponse>("Update successfully", StatusCodeEnum.OK_200, result);
        }
    }
}
