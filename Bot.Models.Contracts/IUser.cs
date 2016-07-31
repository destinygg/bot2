using System.Collections.Generic;

namespace Bot.Models.Contracts {
  public interface IUser {
    string Nick { get; }
    IEnumerable<string> Flair { get; }
    bool IsMod { get; }
  }
}