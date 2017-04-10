namespace Bot.Models {
  public class Moderator : User {

    public Moderator(string nick) : base(nick, true) {

    }

    public override string ToString() => $"{Nick}(Mod)";
  }
}
