using System;

namespace Bot.Database.Contracts {
  public interface IStateVariablesApi {
    DateTime OnTime { get; set; }
  }
}
