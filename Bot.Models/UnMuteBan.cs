using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class UnMuteBan: ITargetable {
    public IUser Target { get; protected set; }
  }
}
