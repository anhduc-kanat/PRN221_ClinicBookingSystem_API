using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Billing;
using ClinicBookingSystem_Service.Models.Response.Billing;
using ClinicBookingSystem_Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;

[ApiController]
[Route("api/billing")]
public class BillingController : ControllerBase
{
    private readonly IBillingService _billingService;
    public BillingController(IBillingService billingService)
    {
        _billingService = billingService;
    }
    // GET: api/billing
    [HttpGet]
    [Route("get-all-billings")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetBillingResponse>>>> GetAllBilling()
    {
        var result = await _billingService.GetAllBilling();
        return Ok(result);
    }
    // GET: api/billing/{id}
    [HttpGet]
    [Route("get-billing-by-id/{id}")]
    public async Task<ActionResult<BaseResponse<GetBillingResponse>>> GetBillingById(int id)
    {
        var result = await _billingService.GetBillingById(id);
        return Ok(result);
    }
    // POST: api/billing
    [HttpPost]
    [Route("create-billing")]
    public async Task<ActionResult<BaseResponse<CreateBillingResponse>>> CreateBilling([FromBody] CreateBillingRequest request)
    {
        var result = await _billingService.CreateBilling(request);
        return Ok(result);
    }
    // PUT: api/billing/{id}
    [HttpPut]
    [Route("update-billing/{id}")]
    public async Task<ActionResult<BaseResponse<UpdateBillingResponse>>> UpdateBilling(int id, [FromBody] UpdateBillingRequest request)
    {
        var result = await _billingService.UpdateBilling(id, request);
        return Ok(result);
    }
    // DELETE: api/billing/{id}
    [HttpDelete]
    [Route("delete-billing/{id}")]
    public async Task<ActionResult<BaseResponse<DeleteBillingResponse>>> DeleteBilling(int id)
    {
        var result = await _billingService.DeleteBilling(id);
        return Ok(result);
    }


    [HttpGet("generate-pdf")]
    public async Task<IActionResult> GeneratePdf(int appointmentId)
    {
        return await _billingService.CreatePdfFromRazorPages(appointmentId);
    }

}