using System;

namespace Bot.Tools.Logging {
  public interface ILogFormatter {
    string FormatWarning(string warning);
    string FormatError(string error);
    string FormatError(Exception e, string error);
    string FormatInformation(string information);
    string FormatVerbose(string verbose);
  }
}
