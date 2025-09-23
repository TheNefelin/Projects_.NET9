using Infrastructure.Models;
using ProjectGamesGuide.Application.DTOs;
using ProjectGamesGuide.Application.Interfaces;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Application.Services;

public class GameGuideService : IGameGuideService
{
    private readonly IRepositoryBase<Game> _game;
    private readonly IRepositoryBase<Character> _character;
    private readonly IRepositoryBase<Source> _source;
    private readonly IRepositoryBase<BackgroundImg> _background;
    private readonly IRepositoryBase<Guide> _guide;
    private readonly IRepositoryBase<Adventure> _adventure;
    private readonly IRepositoryBase<AdventureImg> _adventureImg;
    private readonly IRepositoryByUser<UserGuide> _guideUser;
    private readonly IRepositoryByUser<UserAdventure> _adventureUser;

    public GameGuideService(
        IRepositoryBase<Game> game,
        IRepositoryBase<Character> character,
        IRepositoryBase<Source> source,
        IRepositoryBase<BackgroundImg> background,
        IRepositoryBase<Guide> guide,
        IRepositoryBase<Adventure> adventure,
        IRepositoryBase<AdventureImg> adventureImg,
        IRepositoryByUser<UserGuide> guideUser,
        IRepositoryByUser<UserAdventure> adventureUser
        )
    {
        _game = game;
        _character = character;
        _source = source;
        _background = background;
        _guide = guide;
        _adventure = adventure;
        _adventureImg = adventureImg;
        _guideUser = guideUser;
        _adventureUser = adventureUser;
    }

    public async Task<ApiResponse<IEnumerable<GameResponse>>> GetAllAsync(Guid User_Id, CancellationToken cancellationToken)
    {
        try
        {
            var taskGame = _game.GetAllAsync(cancellationToken);
            var taskCharacter = _character.GetAllAsync(cancellationToken);
            var taskSource = _source.GetAllAsync(cancellationToken);
            var taskBackground = _background.GetAllAsync(cancellationToken);
            var taskGuide = _guide.GetAllAsync(cancellationToken);
            var taskGuideUser = _guideUser.GetAllByUserIdAsync(User_Id, cancellationToken);
            var taskAdventure = _adventure.GetAllAsync(cancellationToken);
            var taskAdventureUser = _adventureUser.GetAllByUserIdAsync(User_Id, cancellationToken);
            var taskAdventureImg = _adventureImg.GetAllAsync(cancellationToken);

            Task.WaitAll(taskGame, taskCharacter, taskSource, taskBackground, taskGuide, taskAdventure, taskAdventureImg, taskGuideUser, taskAdventureUser);

            var game = await taskGame;
            var character = await taskCharacter;
            var source = await taskSource;
            var background = await taskBackground;
            var guide = await taskGuide;
            var adventure = await taskAdventure;
            var adventureImg = await taskAdventureImg;
            var guideUser = await taskGuideUser;
            var adventureUser = await taskAdventureUser;

            var result = game.Select(a => new GameResponse
            {
                Game_Id = a.Game_Id,
                Name = a.Name,
                Description = a.Description,
                ImgUrl = a.ImgUrl,
                IsEnabled = a.IsEnabled,
                Characters = character
                    .Where(b => b.Game_Id == a.Game_Id)
                    .Select(c => new CharacterResponse
                    {
                        Character_Id = c.Character_Id,
                        Name = c.Name,
                        Description = c.Description,
                        ImgUrl = c.ImgUrl
                    }).ToList(),
                Sources = source
                    .Where(d => d.Game_Id == a.Game_Id)
                    .Select(e => new SourceResponse
                    {
                        Source_Id = e.Source_Id,
                        Name = e.Name,
                        Url = e.Url
                    }).ToList(),
                BackgroundImgs = background
                    .Where(f => f.Game_Id == a.Game_Id)
                    .Select(g => new BackgroundImgResponse
                    {
                        BackgroundImg_Id = g.BackgroundImg_Id,
                        ImgUrl = g.ImgUrl
                    }).ToList(),
                guides = guide
                    .Where(h => h.Game_Id == a.Game_Id)
                    .Select(i => new GuideResponse
                    {
                        Guide_Id = i.Guide_Id,
                        Name = i.Name,
                        Sort = i.Sort,
                        GuideUser = guideUser
                            .Where(j => j.Guide_Id == i.Guide_Id && j.User_Id == User_Id)
                            .Select(j => new UserGuide
                            {
                                User_Id = j.User_Id,
                                Guide_Id = j.Guide_Id,
                                IsCheck = j.IsCheck
                            })
                            .FirstOrDefault() ?? (new UserGuide
                            {
                                Guide_Id = i.Guide_Id,
                                IsCheck = false
                            }),
                        Adventures = adventure
                            .Where(l => l.Guide_Id == i.Guide_Id)
                            .Select(m => new AdventureResponse
                            {
                                Adventure_Id = m.Adventure_Id,
                                Description = m.Description,
                                IsImportant = m.IsImportant,
                                Sort = m.Sort,
                                AdventureUser = adventureUser
                                    .Where(o => o.Adventure_Id == m.Adventure_Id && o.User_Id == User_Id)
                                    .Select(p => new UserAdventure
                                    {
                                        User_Id = p.User_Id,
                                        Adventure_Id = p.Adventure_Id,
                                        IsCheck = p.IsCheck
                                    })
                                    .FirstOrDefault() ?? (new UserAdventure
                                    {
                                        Adventure_Id = m.Adventure_Id,
                                        IsCheck = false
                                    }),
                                AdventureImg = adventureImg
                                    .Where(q => q.Adventure_Id == m.Adventure_Id)
                                    .Select(r => new AdventureImgResponse
                                    {
                                        AdventureImg_Id = r.AdventureImg_Id,
                                        ImgUrl = r.ImgUrl,
                                        Sort = r.Sort
                                    }).ToList(),
                            }).ToList(),
                    }).ToList()
            }).ToList();

            return new ApiResponse<IEnumerable<GameResponse>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<GameResponse>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
