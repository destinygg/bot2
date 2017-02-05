namespace Bot.Tools.Interfaces {
  public interface ILogFormatter {
    string FormatWarning(string warning);
    string FormatError(string error);
    string FormatInformation(string information);
    string FormatVerbose(string verbose);
  }
}
