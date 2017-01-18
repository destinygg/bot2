namespace Bot.Models.Websockets {
  public class PardonSender {
    public PardonSender(string nick) {
      data = nick;
    }

    public string data { get; set; }
  }
}