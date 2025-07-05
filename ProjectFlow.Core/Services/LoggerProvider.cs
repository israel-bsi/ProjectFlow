using Microsoft.Extensions.Logging;

namespace ProjectFlow.Core.Services;

public class LoggerProvider : ILoggerProvider
{
    private readonly string _directoryLog;

    public LoggerProvider(string directoryLog)
    {
        _directoryLog = directoryLog;
    }
    
    public ILogger CreateLogger(string categoryName)
    {
        return new ProjectFlowLogger(categoryName, _directoryLog);
    }

    public void Dispose()
    {

    }
}