namespace BellatrixTest.Logger
{
    public interface ILogger
    {
        void LogMessage(string message, LogMessageType messageType);
    }
}
