using System;

namespace Bot.Tools.Interfaces {
  public interface IScopeCreator {
    IDisposable CreateScope();
  }
}
