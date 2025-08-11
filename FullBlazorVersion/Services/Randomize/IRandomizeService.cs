using FullBlazorVersion.Enums;

namespace FullBlazorVersion.Services.Randomize
{
    public interface IRandomizeService
    {
        IEnumerable<int> GetRandomDice(int count, DiceTypeEnum diceType);
    }
}