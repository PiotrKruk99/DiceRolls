namespace DiceRolls.Services.ErrorMessage;

public interface IErrorMessageService
{
    void SetGlobalErrorMessage(string errorMessage);
    void SetSessionErrorMessage(string errorMessage, string sessionId);
    string GetGlobalErrorMessage();
    string GetSessionErrorMessage(string sessionId);
    void ClearErrorMessage();
    void AddGlobalEventToWatch(Action action);
    void AddSessionEventToWatch(Action action, string sessionId);
}