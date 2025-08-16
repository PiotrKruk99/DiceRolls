using DiceRolls.Models;

namespace DiceRolls.Repositories.ThrowRepository;

public interface IThrowRepository
{
    Task AddThrow(Throw input);
}