using DiceRolls.Enums;

namespace DiceRolls.Dtos;

public class ThrowDiceOutputDto
{
    public required string Login { get; set; }
    public required string Time { get; set; }
    public required string DiceType { get; set; }
    public required IEnumerable<int> DiceValues { get; set; }
}