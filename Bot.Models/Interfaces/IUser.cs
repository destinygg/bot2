namespace Bot.Models.Interfaces {
  public interface IUser {
    string Nick { get; }
    bool IsMod { get; }
    bool IsPunishable { get; }
  }
}
