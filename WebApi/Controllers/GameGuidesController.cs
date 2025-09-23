using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectGamesGuide.Application.DTOs;
using ProjectGamesGuide.Application.Interfaces;
using ProjectGamesGuide.Domain.Entities;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/game-guide")]
[ApiController]
[ServiceFilter(typeof(ApiKeyFilter))]
public class GameGuidesController : ControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IGameGuideService _gameGuide;
    private readonly IUserGoogleService _authGoogleService;
    private readonly IUserGuideService _guideUser;
    private readonly IUserAdventureService _adventureUser;
    private readonly IServiceBase<Game> _game;
    private readonly IServiceBase<Adventure> _adventure;
    private readonly IServiceBase<AdventureImg> _adventureimg;
    private readonly IServiceBase<BackgroundImg> _backgroundimg;
    private readonly IServiceBase<Character> _character;
    private readonly IServiceBase<Guide> _guide;
    private readonly IServiceBase<Source> _source;

    public GameGuidesController(
        IWebHostEnvironment webHostEnvironment,
        IGameGuideService gameGuide,
        IUserGoogleService authGoogleService,
        IUserGuideService guideUser,
        IUserAdventureService adventureUser,
        IServiceBase<Game> game,
        IServiceBase<Adventure> adventure,
        IServiceBase<AdventureImg> adventureimg,
        IServiceBase<BackgroundImg> backgroundimg,
        IServiceBase<Character> character,
        IServiceBase<Guide> guide,
        IServiceBase<Source> source
        )
    {
        _webHostEnvironment = webHostEnvironment;
        _gameGuide = gameGuide;
        _authGoogleService = authGoogleService;
        _guideUser = guideUser;
        _adventureUser = adventureUser;
        _adventure = adventure;
        _adventureimg = adventureimg;
        _backgroundimg = backgroundimg;
        _character = character;
        _game = game;
        _guide = guide;
        _source = source;
    }

    [HttpGet("img")]
    public IActionResult GetImg(string fileName)
    {
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "GamesGuide", fileName);

        if (System.IO.File.Exists(filePath))
        {
            byte[] b = System.IO.File.ReadAllBytes(filePath);

            return File(b, "image/webp");
        }

        return BadRequest(new { Msge = "El Archivo No Existe" });
    }

    [HttpGet("{user_id}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<GameResponse>>>> GetAllGameGuides(Guid user_id, CancellationToken cancelationToken)
    {
        //Id_User = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        var apiResult = await _gameGuide.GetAllAsync(user_id, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPost("auth-google")]
    public async Task<ActionResult<ApiResponse<UserLoggedToken>>> GoogleLoginAsync(UserLoginGoogle login, CancellationToken cancellationToken)
    {
        var result = await _authGoogleService.LoginGoogleAsync(login, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("user-guide")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateGuideUserByUserId(UserGuideRequest userGuideRequest, CancellationToken cancelationToken)
    {
        var apiResult = await _guideUser.UpdateAsync(userGuideRequest, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPost("user-adventure")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateAdventureUserlByUserId(UserAdventureRequest userAdventureRequest, CancellationToken cancelationToken)
    {
        var apiResult = await _adventureUser.UpdateAsync(userAdventureRequest, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpGet("game")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Game>>>> GetAllGame(CancellationToken cancelationToken)
    {
        var apiResult = await _game.GetAllAsync(cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpGet("character")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Character>>>> GetAllCharacter(CancellationToken cancelationToken)
    {
        var apiResult = await _character.GetAllAsync(cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpGet("source")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Source>>>> GetAllSource(CancellationToken cancelationToken)
    {
        var apiResult = await _source.GetAllAsync(cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpGet("background-img")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BackgroundImg>>>> GetAllBackgroundImg(CancellationToken cancelationToken)
    {
        var apiResult = await _backgroundimg.GetAllAsync(cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpGet("guide")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Guide>>>> GetAllGuide(CancellationToken cancelationToken)
    {
        var apiResult = await _guide.GetAllAsync(cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }


    [HttpGet("adventure")]
    public async Task<ActionResult<ApiResponse<IEnumerable<Adventure>>>> GetAllAdventure(CancellationToken cancelationToken)
    {
        var apiResult = await _adventure.GetAllAsync(cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpGet("adventure-img")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AdventureImg>>>> GetAllAdventureImg(CancellationToken cancelationToken)
    {
        var apiResult = await _adventureimg.GetAllAsync(cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }
}
