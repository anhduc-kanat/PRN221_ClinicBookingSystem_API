using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Billing;
using ClinicBookingSystem_Service.Models.Response.Billing;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_Service.IService;

public interface IBillingService
{
    // Get all billings
    Task<BaseResponse<IEnumerable<GetBillingResponse>>> GetAllBilling();
    // Get billing by id
    Task<BaseResponse<GetBillingResponse>> GetBillingById(int id);
    // Create billing
    Task<BaseResponse<CreateBillingResponse>> CreateBilling(CreateBillingRequest request);
    // Update billing
    Task<BaseResponse<UpdateBillingResponse>> UpdateBilling(int id, UpdateBillingRequest request);
    // Delete billing
    Task<BaseResponse<DeleteBillingResponse>> DeleteBilling(int id);
    Task<IActionResult> CreatePdfFromRazorPages(int appointmentId);

}