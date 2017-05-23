namespace Bot.Models.Websockets {
  public class ReceivedPrivateMessage {
    public int messageid { get; set; }
    public long timestamp { get; set; }
    public string nick { get; set; }
    public string data { get; set; }
  }
}
