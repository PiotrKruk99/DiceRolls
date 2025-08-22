namespace DiceRolls.Services.ErrorMessage;

public interface IErrorMessageService
{
    void SetGlobalErrorMessage(string errorMessage);
    string GetGlobalErrorMessage();
    void ClearErrorMessage();
    void AddGlobalEventToWatch(Action action);
}