namespace Bot.Models {
  public class Civilian : User {

    public Civilian(string nick) : base(nick, false) {

    }

    public override string ToString() => Nick;
  }
}
