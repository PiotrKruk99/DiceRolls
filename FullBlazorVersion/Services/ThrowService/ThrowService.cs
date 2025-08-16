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
}