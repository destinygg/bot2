namespace Bot.Models {
  public class CustomCommand {
    public CustomCommand(string command, string response) {
      Command = command;
      Response = response;
    }

    public string Command { get; }
    public string Response { get; }

  }
}
