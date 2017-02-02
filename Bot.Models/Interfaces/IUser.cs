using System.Collections.Generic;

namespace Bot.Models.Interfaces {
  public interface IUser {
    string Nick { get; }
    IEnumerable<string> Flair { get; }
    bool IsMod { get; }
    IReceivedVisitor Accept(IUserVisitor visitor);
  }
}
