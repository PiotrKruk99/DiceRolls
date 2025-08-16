using DiceRolls.Enums;

namespace DiceRolls.Dtos;

public class ThrowDiceInputDto
{
    public required string SessionId { get; set; }
    public string? Login { get; set; }
    public int Count { get; set; }
    public DiceTypeEnum DiceType { get; set; }
}