using DiceRolls.Models;

namespace DiceRolls.Repositories.ThrowRepository;

public interface IThrowRepository
{
    Task AddThrow(Throw input);
    Task<IEnumerable<Throw>> GetThrows(string sessionId);
}