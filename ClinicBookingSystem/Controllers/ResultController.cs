using ClinicBookingSystem_Service.Common.Utils;
using ClinicBookingSystem_Service.IService;
using ClinicBookingSystem_Service.Models.BaseResponse;
using ClinicBookingSystem_Service.Models.Request.Result;
using ClinicBookingSystem_Service.Models.Response.Result;
using iText.Html2pdf;
using iText.Kernel.Exceptions;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem_API.Controllers;
[ApiController]
[Route("api/result")]
public class ResultController : ControllerBase
{
    private readonly IResultService _resultService;
    public ResultController(IResultService resultService)
    {
        _resultService = resultService;
    }
    // GET: api/result
    [HttpGet]
    [Route("get-all-result")]
    public async Task<ActionResult<BaseResponse<IEnumerable<GetResultResponse>>>> GetAllResult()
    {
        var result = await _resultService.GetAllResult();
        return Ok(result);
    }
    // GET: api/result/5
    [HttpGet]
    [Route("get-result-by-id/{id}")]
    public async Task<ActionResult<BaseResponse<GetResultResponse>>> GetResultById(int id)
    {
        var result = await _resultService.GetResultById(id);
        return Ok(result);
    }
    // POST: api/result
    [HttpPost]
    [Route("create-result")]
    public async Task<ActionResult<BaseResponse<CreateResultResponse>>> CreateResult([FromBody] CreateResultRequest request)
    {
        var result = await _resultService.CreateResult(request);
        return Ok(result);
    }
    // PUT: api/result/5
    [HttpPut]
    [Route("update-result/{id}")]
    public async Task<ActionResult<BaseResponse<UpdateResultResponse>>> UpdateResult(int id, [FromBody] UpdateResultRequest request)
    {
        var result = await _resultService.UpdateResult(id, request);
        return Ok(result);
    }
    // DELETE: api/result/5
    [HttpDelete]
    [Route("delete-result/{id}")]
    public async Task<ActionResult<BaseResponse<DeleteResultResponse>>> DeleteResult(int id)
    {
        var result = await _resultService.DeleteResult(id);
        return Ok(result);
    }
    // UPDATE: api/result/5



   
}