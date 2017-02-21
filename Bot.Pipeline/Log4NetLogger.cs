using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Tools.Logging;
using log4net;

namespace Bot.Pipeline {
  public class Log4NetLogger<T> : ILogger {
    private readonly ILog _logger;

    public Log4NetLogger() {
      _logger = LogManager.GetLogger(GetCSharpRepresentation(typeof(T), true));
    }

    public void LogDebug(string debug) => _logger.Debug(debug);
    public void LogInformation(string information) => _logger.Info(information);
    public void LogWarning(string warning) => _logger.Warn(warning);
    public void LogError(string error) => _logger.Error(error);
    public void LogError(string error, Exception exception) => _logger.Error(error, exception);
    public void LogFatal(string fatal) => _logger.Fatal(fatal);
    public void LogFatal(string fatal, Exception exception) => _logger.Fatal(fatal, exception);

    // http://stackoverflow.com/questions/2579734/get-the-type-name
    private string GetCSharpRepresentation(Type t, bool trimArgCount) {
      if (t.IsGenericType) {
        var genericArgs = t.GetGenericArguments().ToList();
        return GetCSharpRepresentation(t, trimArgCount, genericArgs);
      }
      return t.Name;
    }

    private string GetCSharpRepresentation(Type t, bool trimArgCount, List<Type> availableArguments) {
      if (t.IsGenericType) {
        var value = t.Name;
        if (trimArgCount && value.IndexOf("`") > -1) {
          value = value.Substring(0, value.IndexOf("`"));
        }
        if (t.DeclaringType != null) {
          // This is a nested type, build the nesting type first
          value = GetCSharpRepresentation(t.DeclaringType, trimArgCount, availableArguments) + "+" + value;                                                  
        }
        // Build the type arguments (if any)
        var argString = "";
        var thisTypeArgs = t.GetGenericArguments();
        for (var i = 0; i < thisTypeArgs.Length && availableArguments.Count > 0; i++) {
          if (i != 0) argString += ", ";
          argString += GetCSharpRepresentation(availableArguments[0], trimArgCount);
          availableArguments.RemoveAt(0);
        }
        // If there are type arguments, add them with < >
        if (argString.Length > 0) {
          value += "<" + argString + ">";
        }
        return value;
      }
      return t.Name;
    }

  }
}
