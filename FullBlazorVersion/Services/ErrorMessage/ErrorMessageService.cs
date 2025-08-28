namespace DiceRolls.Services.ErrorMessage;

public class ErrorMessageService : IErrorMessageService
{
    private string _globalErrorMessage = "";
    private event Action? OnGlobalEventTriggered;
    private readonly List<SessionEvent> _sessionEvents = new();
    private readonly List<SessionMessage> _sessionErrorMessages = new();

    public void SetGlobalErrorMessage(string errorMessage)
    {
        if (!string.IsNullOrWhiteSpace(errorMessage))
        {
            _globalErrorMessage = errorMessage;
            OnGlobalEventTriggered?.Invoke();
        }
    }

    public void SetSessionErrorMessage(string errorMessage, string sessionId)
    {
        if (!string.IsNullOrWhiteSpace(errorMessage)
            && _sessionEvents.Any(x => x.SessionId == sessionId))
        {
            if (_sessionErrorMessages.Any(x => x.SessionId == sessionId))
            {
                var sessionMessage = _sessionErrorMessages.First(x => x.SessionId == sessionId);
                sessionMessage.Message = errorMessage;
                sessionMessage.AddedTime = DateTime.UtcNow;
            }
            else
            {
                _sessionErrorMessages.Add(new SessionMessage()
                {
                    SessionId = sessionId,
                    Message = errorMessage,
                    AddedTime = DateTime.UtcNow
                });
            }

            var sessionEvent = _sessionEvents.First(x => x.SessionId == sessionId);
            sessionEvent.Invoke();
        }
    }

    public string GetGlobalErrorMessage()
    {
        return _globalErrorMessage;
    }

    public string GetSessionErrorMessage(string sessionId)
    {
        var errorMessage = _sessionErrorMessages.FirstOrDefault(x => x.SessionId == sessionId);

        if ((errorMessage != null && errorMessage.AddedTime.AddSeconds(10) < DateTime.UtcNow)
            || errorMessage == null)
        {
            if (errorMessage != null)
                _sessionErrorMessages.Remove(errorMessage);
            
            return string.Empty;
        }
        else
        {
            return errorMessage.Message;
        }
    }

    public void ClearErrorMessage()
    {
        _globalErrorMessage = string.Empty;
    }

    public void AddGlobalEventToWatch(Action action)
    {
        OnGlobalEventTriggered += action;
    }

    public void AddSessionEventToWatch(Action action, string sessionId)
    {
        if (_sessionEvents.Exists(x => x.SessionId == sessionId))
        {
            var sessionEvent = _sessionEvents.First(x => x.SessionId == sessionId);
            sessionEvent.OnSessionEventTriggered += action;
        }
        else
        {
            _sessionEvents.Add(new SessionEvent
            (
                action,
                sessionId
            ));
        }
    }

    private class SessionEvent
    {
        public SessionEvent(Action action, string sessionId)
        {
            OnSessionEventTriggered += action;
            SessionId = sessionId;
        }

        public event Action? OnSessionEventTriggered;
        public string SessionId { get; set; }

        public void Invoke()
        {
            OnSessionEventTriggered?.Invoke();
        }
    }

    private class SessionMessage
    {
        public required string SessionId { get; set; }
        public required string Message { get; set; }
        public required DateTime AddedTime { get; set; }
    }
}