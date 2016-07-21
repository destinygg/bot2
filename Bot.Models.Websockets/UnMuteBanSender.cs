namespace Bot.Models.Websockets {
  public class UnMuteBanSender {
    public UnMuteBanSender(string nick) {
      data = nick;
    }

    public string data { get; set; }
  }
}