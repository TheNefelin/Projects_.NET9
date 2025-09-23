using Infrastructure.Models;
using ProjectPortfolio.Application.DTOs;
using ProjectPortfolio.Application.Interfaces;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Application.Services;

public class PublicUrlsService : IServicePortfolioBase<UrlResponse>
{
    private readonly IRepositoryPortfolioBase<UrlGrp> _urlGrpRepository;
    private readonly IRepositoryPortfolioBase<Url> _urlRepository;

    public PublicUrlsService(
        IRepositoryPortfolioBase<UrlGrp> urlGrpRepository,
        IRepositoryPortfolioBase<Url> urlRepository)
    {
        _urlGrpRepository = urlGrpRepository;
        _urlRepository = urlRepository;
    }

    public async Task<ApiResponse<IEnumerable<UrlResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var taskUrlsGrp = _urlGrpRepository.GetAllAsync(cancellationToken);
            var taskUrls = _urlRepository.GetAllAsync(cancellationToken);

            await Task.WhenAll(taskUrlsGrp, taskUrls);

            var urlsGrp = await taskUrlsGrp;
            var urls = await taskUrls;

            var result = urlsGrp.Select(a => new UrlResponse
            {
                Id = a.Id,
                Name = a.Name,
                IsEnable = a.IsEnable,
                Urls = urls.Where(b => b.Id_UrlGrp == a.Id).ToList()
            }).ToList();

            return new ApiResponse<IEnumerable<UrlResponse>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<UrlResponse>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
