using Infrastructure.Models;
using ProjectGamesGuide.Application.DTOs;
using ProjectGamesGuide.Application.Interfaces;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Application.Services;

public class UserGoogleService : IUserGoogleService
{
    private readonly IUserGoogleRepository _authGoogleRepository;

    public UserGoogleService(IUserGoogleRepository authGoogleRepository)
    {
        _authGoogleRepository = authGoogleRepository;
    }

    public async Task<ApiResponse<UserLoggedToken>> LoginGoogleAsync(UserLoginGoogle login, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _authGoogleRepository.LoginGoogleAsync(login, cancellationToken);
            return new ApiResponse<UserLoggedToken>
            {
                IsSuccess = data.IsSuccess,
                StatusCode = data.StatusCode,
                Message = data.Message,
                Data = new UserLoggedToken
                {
                    User_Id = data.User_Id,
                    SqlToken = data.SqlToken
                }
            };

        }
        catch (Exception ex)
        {
            return new ApiResponse<UserLoggedToken>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
