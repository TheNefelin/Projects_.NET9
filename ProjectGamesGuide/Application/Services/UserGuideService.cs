using Infrastructure.Models;
using ProjectGamesGuide.Application.DTOs;
using ProjectGamesGuide.Application.Interfaces;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Application.Services;

public class UserGuideService : IUserGuideService
{
    private readonly IRepositoryByUser<UserGuide> _repository;
    private readonly IUserGoogleRepository _userGoogleRepository;

    public UserGuideService(IRepositoryByUser<UserGuide> repository, IUserGoogleRepository userGoogleRepository)
    {
        _repository = repository;
        _userGoogleRepository = userGoogleRepository;
    }

    public async Task<ApiResponse<IEnumerable<UserGuide>>> GetAllByUserIdAsync(Guid Id_User, CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllByUserIdAsync(Id_User, cancellationToken);
            return new ApiResponse<IEnumerable<UserGuide>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<UserGuide>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<ApiResponse<object>> UpdateAsync(UserGuideRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var userLoggedToken = new UserLoggedToken
            {
                User_Id = request.UserToken.User_Id,
                SqlToken = request.UserToken.SqlToken,
            };

            var userGuide = new UserGuide()
            {
                User_Id = request.UserToken.User_Id,
                Guide_Id = request.Guide_Id,
                IsCheck = request.IsCheck,
            };

            var validateLogin = await _userGoogleRepository.ValidateLoginAsync(userLoggedToken, cancellationToken);

            if (validateLogin == null)
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Debes iniciar sesión",
                };

            var sqlResponse = await _repository.UpdateAsync(userGuide, cancellationToken);
            return new ApiResponse<object>
            {
                IsSuccess = sqlResponse.IsSuccess,
                StatusCode = sqlResponse.StatusCode,
                Message = sqlResponse.Message,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<object>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
