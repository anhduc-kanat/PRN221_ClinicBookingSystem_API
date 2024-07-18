using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using RazorLight;

namespace ClinicBookingSystem_Service.Common.Utils;

public class RazorViewToStringRenderer
{
    private readonly RazorLightEngine _engine;
    private readonly IWebHostEnvironment _env;
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<RazorViewToStringRenderer> _logger;
    public RazorViewToStringRenderer(IWebHostEnvironment env, ITempDataProvider tempDataProvider,
        IHttpContextAccessor httpContextAccessor,
        IRazorViewEngine viewEngine,
        ILogger<RazorViewToStringRenderer> logger)
    {
        _env = env;
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Directory.GetCurrentDirectory()) // or a specific path to your templates
            .UseMemoryCachingProvider()
            .Build();
        _httpContextAccessor = httpContextAccessor;
        _tempDataProvider = tempDataProvider;
        _viewEngine = viewEngine;
        _logger = logger;
    }

    public async Task<string> RenderViewToStringAsync<TModel>(string viewPath, TModel model)
    {
        string templatePath = Path.Combine(_env.WebRootPath, viewPath);
        _logger.LogInformation(templatePath);
        string templateContent = await File.ReadAllTextAsync(templatePath);
        string result = await _engine.CompileRenderStringAsync(viewPath, templateContent, model);
        return result;
    }

    public async Task<string> RenderRazorViewToStringAsync<TModel>(string viewPath, TModel model)
    {
        _logger.LogInformation(viewPath);
        string templateContent = await File.ReadAllTextAsync(viewPath);
        string result = await _engine.CompileRenderStringAsync(viewPath, templateContent, model);
        return result;
    }





}