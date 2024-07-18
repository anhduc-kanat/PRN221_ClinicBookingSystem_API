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
using RazorLight;

namespace ClinicBookingSystem_Service.Common.Utils;

public class RazorViewToStringRenderer
{
    private readonly RazorLightEngine _engine;
    private readonly IWebHostEnvironment _env;
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public RazorViewToStringRenderer(IWebHostEnvironment env, ITempDataProvider tempDataProvider, IHttpContextAccessor httpContextAccessor, IRazorViewEngine viewEngine)
    {
        _env = env;
        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(Directory.GetCurrentDirectory()) // or a specific path to your templates
            .UseMemoryCachingProvider()
            .Build();
        _httpContextAccessor = httpContextAccessor;
        _tempDataProvider = tempDataProvider;
        _viewEngine = viewEngine;
    }

    public async Task<string> RenderViewToStringAsync<TModel>(string viewPath, TModel model)
    {
        string templatePath = Path.Combine(_env.WebRootPath, viewPath);
        string templateContent = await File.ReadAllTextAsync(templatePath);
        string result = await _engine.CompileRenderStringAsync(viewPath, templateContent, model);
        return result;
    }





}