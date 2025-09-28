using Infrastructure.Models;
using ProjectPortfolio.Application.DTOs;
using ProjectPortfolio.Application.Interfaces;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Application.Services;

public class PublicProjectsService : IServicePortfolioBase<ProjectResponse>
{
    private readonly IRepositoryPortfolioBase<Project> _projectRepository;
    private readonly IRepositoryPortfolioBase<Language> _languageRepository;
    private readonly IRepositoryPortfolioBase<Technology> _technologyRepository;
    private readonly IRepositoryPortfolioBase<Pro_Lang> _prolangRepository;
    private readonly IRepositoryPortfolioBase<Pro_Tech> _protechRepository;

    public PublicProjectsService(
        IRepositoryPortfolioBase<Project> projectRepository,
        IRepositoryPortfolioBase<Language> languageRepository,
        IRepositoryPortfolioBase<Technology> technologyRepository,
        IRepositoryPortfolioBase<Pro_Lang> prolangRepository,
        IRepositoryPortfolioBase<Pro_Tech> protechRepository)
    {
        _projectRepository = projectRepository;
        _languageRepository = languageRepository;
        _technologyRepository = technologyRepository;
        _prolangRepository = prolangRepository;
        _protechRepository = protechRepository;
    }

    public async Task<ApiResponse<IEnumerable<ProjectResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var taskProjects = _projectRepository.GetAllAsync(cancellationToken);
            var taskLanguages = _languageRepository.GetAllAsync(cancellationToken);
            var taskTechnologies = _technologyRepository.GetAllAsync(cancellationToken);
            var taskProLangs = _prolangRepository.GetAllAsync(cancellationToken);
            var taskProTechs = _protechRepository.GetAllAsync(cancellationToken);

            await Task.WhenAll(taskProjects, taskLanguages, taskTechnologies, taskProLangs, taskProTechs);

            var projects = await taskProjects;
            var languages = await taskLanguages;
            var technologies = await taskTechnologies;
            var proLangs = await taskProLangs;
            var proTechs = await taskProTechs;

            var result = projects.Select(a => new ProjectResponse
            {
                Id = a.Id,
                Name = a.Name,
                ImgUrl = a.ImgUrl,
                RepoUrl = a.RepoUrl,
                AppUrl = a.AppUrl,
                Languages = proLangs
                .Where(b => b.Id_Project == a.Id)
                .Join(languages, b => b.Id_Language, c => c.Id, (b, c) => new Language
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImgUrl = c.ImgUrl
                }).ToList(),
                Technologies = proTechs
                .Where(b => b.Id_Project == a.Id)
                .Join(technologies, b => b.Id_Technology, c => c.Id, (b, c) => new Technology
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImgUrl = c.ImgUrl,
                }).ToList(),
            }).ToList();

            return new ApiResponse<IEnumerable<ProjectResponse>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<ProjectResponse>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public Task<ApiResponse<ProjectResponse?>> CreateAsync(ProjectResponse entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<ProjectResponse?>> UpdateAsync(ProjectResponse entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<object>> DeleteAsync(int Id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
