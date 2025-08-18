using DiceRolls.Dtos;
using DiceRolls.Repositories.ThrowRepository;
using DiceRolls.Services.Randomize;
using DiceRolls.Models;

namespace DiceRolls.Services.ThrowService;

public class ThrowService : IThrowService
{
    private readonly IRandomizeService _randomizeService;
    private readonly IThrowRepository _throwRepository;

    public ThrowService(IRandomizeService randomizeService, IThrowRepository throwRepository)
    {
        _randomizeService = randomizeService;
        _throwRepository = throwRepository;
    }

    public async Task ThrowDice(ThrowDiceInputDto input)
    {
        var roll = _randomizeService.GetRandomDice(input.Count, input.DiceType);

        Throw throwDice = new Throw()
        {
            SessionId = input.SessionId,
            Login = input.Login,
            Date = DateTime.UtcNow,
            DiceType = input.DiceType,
            DiceValues = string.Join(',', roll)
        };

        await _throwRepository.AddThrow(throwDice);
    }
    
    public async Task<IEnumerable<ThrowDiceOutputDto>> GetThrows(string sessionId)
    {
        var throws = await _throwRepository.GetThrows(sessionId);
        var result = new List<ThrowDiceOutputDto>();

        foreach (var elem in throws.OrderByDescending(x => x.Date))
        {
            result.Add(new ThrowDiceOutputDto()
            {
                Login = elem.Login ?? "",
                Time = elem.Date.ToLocalTime().ToString("HH:mm:ss"),
                DiceType = elem.DiceType.ToString().ToLower(),
                DiceValues = elem.DiceValues.Split(',').Select(x => Convert.ToInt32(x))
            });
        }
        
        return result;
    }
}