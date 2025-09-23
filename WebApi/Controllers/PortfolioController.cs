using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectPortfolio.Application.DTOs;
using ProjectPortfolio.Application.Interfaces;
using ProjectPortfolio.Domain.Entities;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/portfolio")]
[ApiController]
[ServiceFilter(typeof(ApiKeyFilter))]
public class PortfolioController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IServicePortfolioBase<ProjectResponse> _publicProjectsService;
    private readonly IServicePortfolioBase<UrlResponse> _publicUrlsService;
    private readonly IServicePortfolioBase<Project> _serviceProject;
    private readonly IServicePortfolioBase<Language> _serviceLanguage;
    private readonly IServicePortfolioBase<Technology> _serviceTechnology;
    private readonly IServicePortfolioBase<UrlGrp> _serviceUrlGrp;
    private readonly IServicePortfolioBase<Url> _serviceUrl;

    public PortfolioController(
        IWebHostEnvironment webHostEnvironment,
        IServicePortfolioBase<ProjectResponse> publicProjectsService,
        IServicePortfolioBase<UrlResponse> publicUrlsService,
        IServicePortfolioBase<Project> serviceProject,
        IServicePortfolioBase<Language> serviceLanguage,
        IServicePortfolioBase<Technology> serviceTechnology,
        IServicePortfolioBase<UrlGrp> serviceUrlGrp,
        IServicePortfolioBase<Url> serviceUrl)
    {
        _webHostEnvironment = webHostEnvironment;
        _publicProjectsService = publicProjectsService;
        _publicUrlsService = publicUrlsService;
        _serviceProject = serviceProject;
        _serviceLanguage = serviceLanguage;
        _serviceTechnology = serviceTechnology;
        _serviceUrlGrp = serviceUrlGrp;
        _serviceUrl = serviceUrl;
    }

    [HttpGet("img")]
    public IActionResult GetImg(string fileName)
    {
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Portfolio", fileName);

        if (System.IO.File.Exists(filePath))
        {
            byte[] b = System.IO.File.ReadAllBytes(filePath);

            return File(b, "image/webp");
        }

        return BadRequest(new { Msge = "El Archivo No Existe" });
    }

    [HttpGet("public-projects")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectResponse>>>> GetAllPublicProjects(CancellationToken cancellationToken)
    {
        var result = await _publicProjectsService.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("public-urls")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UrlResponse>>>> GetAllPublicUrls(CancellationToken cancellationToken)
    {
        var result = await _publicUrlsService.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("projects")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Project>>>> GetAllProjects(CancellationToken cancellationToken)
    {
        var result = await _serviceProject.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("languages")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Language>>>> GetAllLanguages(CancellationToken cancellationToken)
    {
        var result = await _serviceLanguage.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("technologies")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Technology>>>> GetAllTechnologies(CancellationToken cancellationToken)
    {
        var result = await _serviceTechnology.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("url-grps")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UrlGrp>>>> GetAllUrlGrps(CancellationToken cancellationToken)
    {
        var result = await _serviceUrlGrp.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("urls")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Url>>>> GetAllUrls(CancellationToken cancellationToken)
    {
        var result = await _serviceUrl.GetAllAsync(cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
