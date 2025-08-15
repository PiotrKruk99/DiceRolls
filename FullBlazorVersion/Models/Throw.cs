using DiceRolls.Enums;

namespace DiceRolls.Models;

public class Throw
{
    public int Id { get; set; }
    public required string SessionId { get; set; }
    public string? Login { get; set; }
    public DateTime Date { get; set; }
    public DiceTypeEnum DiceType { get; set; }
    public required string DiceValues { get; set; }
}