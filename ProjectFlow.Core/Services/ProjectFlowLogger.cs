using Microsoft.Extensions.Logging;

namespace ProjectFlow.Core.Services;

public class ProjectFlowLogger : ILogger
{
    private static string _fileLog = "";
    private readonly object _locker = new();
    private readonly string _categoryName;

    public ProjectFlowLogger(string categoryName, string directoryFileLog)
    {
        _categoryName = categoryName;
        _fileLog = Path.Combine(directoryFileLog, $"log-{DateTime.Now.ToUniversalTime().AddHours(-3):dd-MM-yyyy}.txt");
    }
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);
        var formatedMessage = $"{DateTime.Now.ToUniversalTime().AddHours(-3):dd-MM-yyyy HH:mm:ss} [{logLevel}] - {_categoryName}: {message}";
        Console.WriteLine(formatedMessage);

        lock (_locker)
        {
            using var sw = new StreamWriter(_fileLog, true);
            sw.WriteLine(formatedMessage);
        }
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}