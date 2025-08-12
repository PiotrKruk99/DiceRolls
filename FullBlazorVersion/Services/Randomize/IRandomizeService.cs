using DiceRolls.Enums;

namespace DiceRolls.Services.Randomize
{
    public interface IRandomizeService
    {
        IEnumerable<int> GetRandomDice(int count, DiceTypeEnum diceType);
    }
}