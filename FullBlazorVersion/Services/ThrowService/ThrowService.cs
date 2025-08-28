using DiceRolls.Dtos;
using DiceRolls.Repositories.ThrowRepository;
using DiceRolls.Services.Randomize;
using DiceRolls.Models;
using DiceRolls.Services.ErrorMessage;

namespace DiceRolls.Services.ThrowService;

public class ThrowService : IThrowService
{
    private readonly IRandomizeService _randomizeService;
    private readonly IThrowRepository _throwRepository;
    private readonly IErrorMessageService _errorMessageService;

    public ThrowService(IRandomizeService randomizeService,
        IThrowRepository throwRepository,
        IErrorMessageService errorMessageService)
    {
        _randomizeService = randomizeService;
        _throwRepository = throwRepository;
        _errorMessageService = errorMessageService;
    }

    public async Task ThrowDice(ThrowDiceInputDto input)
    {
        try
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
        catch (Exception)
        {
            _errorMessageService.SetSessionErrorMessage("Nie udało się zapisać rzutu do bazy.", input.SessionId);
        }
    }

    public async Task<IEnumerable<ThrowDiceOutputDto>> GetThrows(string sessionId)
    {
        var result = new List<ThrowDiceOutputDto>();

        try
        {
            var throws = await _throwRepository.GetThrows(sessionId);

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
        }
        catch (Exception)
        {
            _errorMessageService.SetSessionErrorMessage("Nie udało się pobrać listy rzutów z bazy.", sessionId);
        }

        return result;
    }
}