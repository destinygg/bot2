using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class LogFormatter : ILogFormatter {
    public string FormatWarning(string warning) => warning;
    public string FormatError(string error) => error;
    public string FormatInformation(string information) => information;
    public string FormatVerbose(string verbose) => verbose;
  }
}
