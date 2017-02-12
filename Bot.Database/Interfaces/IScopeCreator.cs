using System;

namespace Bot.Database.Interfaces {
  public interface IScopeCreator {
    IDisposable CreateScope();
  }
}
