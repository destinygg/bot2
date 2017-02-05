using System;
using System.Text;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class LogFormatter : ILogFormatter {
    public string FormatWarning(string warning) => warning;
    public string FormatError(string error) {
      var sb = new StringBuilder();
      sb.AppendLine($"{Environment.NewLine}===   Begin Error   ===");
      sb.AppendLine(error.Trim());
      sb.AppendLine("===    End Error    ===");
      return sb.ToString();
    }

    public string FormatError(Exception e, string error) {
      var sb = new StringBuilder();
      sb.AppendLine(error);
      sb.AppendLine(e.Message);
      sb.AppendLine(e.StackTrace);
      return FormatError(sb.ToString());
    }

    public string FormatInformation(string information) => information;
    public string FormatVerbose(string verbose) => verbose;
  }
}
