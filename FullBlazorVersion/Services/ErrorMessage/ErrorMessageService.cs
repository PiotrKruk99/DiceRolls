namespace DiceRolls.Services.ErrorMessage;

public class ErrorMessageService : IErrorMessageService
{
    private string _globalErrorMessage = "";
    private event Action? OnEventTriggered;

    public void SetGlobalErrorMessage(string errorMessage)
    {
        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            _globalErrorMessage = errorMessage;
            OnEventTriggered?.Invoke();
        }
    }
    
    public string GetGlobalErrorMessage()
    {
        return _globalErrorMessage;
    }
    
    public void ClearErrorMessage()
    {
        _globalErrorMessage = string.Empty;
    }

    public void AddGlobalEventToWatch(Action action)
    {
        OnEventTriggered += action;
    }
}