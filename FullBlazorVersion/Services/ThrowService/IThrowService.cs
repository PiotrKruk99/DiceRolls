using DiceRolls.Dtos;

namespace DiceRolls.Services.ThrowService;

public interface IThrowService
{
    Task ThrowDice(ThrowDiceInputDto input);
    Task<IEnumerable<ThrowDiceOutputDto>> GetThrows(string sessionId);
}