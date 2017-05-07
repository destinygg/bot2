namespace Bot.Models.Websockets {
  public class SendablePublicMessage {
    public SendablePublicMessage(string input) {
      Data = input;
    }

    public string Data { get; set; }
  }
}
