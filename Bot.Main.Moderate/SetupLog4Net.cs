using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Bot.Main.Moderate {

  // http://stackoverflow.com/a/19538654
  public class SetupLog4Net {

    public void Setup(Level level) {
      var hierarchy = (Hierarchy) LogManager.GetRepository();

      var patternLayout = new PatternLayout {
        ConversionPattern = "%utcdate{ABSOLUTE} [%-2thread] %-5level - %message%newline%exception"
      };

      //RollingFileAppender roller = new RollingFileAppender();
      //roller.AppendToFile = false;
      //roller.File = @"Logs\EventLog.txt";
      //roller.Layout = patternLayout;
      //roller.MaxSizeRollBackups = 5;
      //roller.MaximumFileSize = "1GB";
      //roller.RollingStyle = RollingFileAppender.RollingMode.Size;
      //roller.StaticLogFileName = true;
      //roller.ActivateOptions();
      //hierarchy.Root.AddAppender(roller);

      //MemoryAppender memory = new MemoryAppender();
      //memory.ActivateOptions();
      //hierarchy.Root.AddAppender(memory);

      var consoleAppender = new ConsoleAppender {
        Layout = patternLayout,
      };
      consoleAppender.ActivateOptions();
      hierarchy.Root.AddAppender(consoleAppender);

      hierarchy.Root.Level = level;
      hierarchy.Configured = true;
    }

  }
}
