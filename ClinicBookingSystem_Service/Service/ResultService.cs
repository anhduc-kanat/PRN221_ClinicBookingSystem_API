using AutoMapper;
using ClinicBookingSystem_BusinessObject.Entities;
using ClinicBookingSystem_Repository.IRepositories;
using ClinicBookingSystem_Service.Common.Utils;
using ClinicBookingSystem_Service.CustomException;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Enums;
using ClinicBookingSystem_Service.Models.Request.Result;
using ClinicBookingSystem_Service.Models.Response.Appointment;
using ClinicBookingSystem_Service.Models.Response.Pdf;
using ClinicBookingSystem_Service.Models.Response.Result;
using iText.Html2pdf;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Globalization;

namespace ClinicBookingSystem_Service.Service;

public class ResultService : IResultService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly RazorViewToStringRenderer _razorRenderer;

    public ResultService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment hostingEnvironment
        , RazorViewToStringRenderer razorRenderer
        )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _hostingEnvironment = hostingEnvironment;
        _razorRenderer = razorRenderer;
    }
    //Get all results
    public async Task<BaseResponse<IEnumerable<GetResultResponse>>> GetAllResult()
    {
        IEnumerable<Result> results = await _unitOfWork.ResultRepository.GetAllAsync();
        return new BaseResponse<IEnumerable<GetResultResponse>>("Get all results successfully", StatusCodeEnum.OK_200,
            _mapper.Map<IEnumerable<GetResultResponse>>(results));
    }
    //Get result by id
    public async Task<BaseResponse<GetResultResponse>> GetResultById(int id)
    {
        Result result = await _unitOfWork.ResultRepository.GetByIdAsync(id);
        return new BaseResponse<GetResultResponse>("Get result by id successfully", StatusCodeEnum.OK_200,
            _mapper.Map<GetResultResponse>(result));
    }
    //Create new result
    public async Task<BaseResponse<CreateResultResponse>> CreateResult(CreateResultRequest request)
    {
        Result result = _mapper.Map<Result>(request);
        await _unitOfWork.ResultRepository.AddAsync(result);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<CreateResultResponse>("Create result successfully", StatusCodeEnum.Created_201,
            _mapper.Map<CreateResultResponse>(result));
    }
    //Update result
    public async Task<BaseResponse<UpdateResultResponse>> UpdateResult(int id, UpdateResultRequest request)
    {
        Result result = await _unitOfWork.ResultRepository.GetByIdAsync(id);
        _mapper.Map(request, result);
        _unitOfWork.ResultRepository.UpdateAsync(result);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<UpdateResultResponse>("Update result successfully", StatusCodeEnum.OK_200,
            _mapper.Map<UpdateResultResponse>(result));
    }
    //Delete result
    public async Task<BaseResponse<DeleteResultResponse>> DeleteResult(int id)
    {
        Result result = await _unitOfWork.ResultRepository.GetByIdAsync(id);
        await _unitOfWork.ResultRepository.DeleteAsync(result);
        await _unitOfWork.SaveChangesAsync();
        return new BaseResponse<DeleteResultResponse>("Delete result successfully", StatusCodeEnum.OK_200,
            _mapper.Map<DeleteResultResponse>(result));
    }

    

}