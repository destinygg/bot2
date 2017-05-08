namespace Bot.Models {
  public class Civilian : User {

    public Civilian(string nick, bool isPunishable = true) : base(nick, false, isPunishable) {

    }

    public override string ToString() => Nick;
  }
}
