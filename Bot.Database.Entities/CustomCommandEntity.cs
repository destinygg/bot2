namespace Bot.Database.Entities {
  public class CustomCommandEntity {
    public CustomCommandEntity(string command, string response) {
      Command = command;
      Response = response;
    }

    public CustomCommandEntity() {

    }

    public string Command { get; set; }
    public string Response { get; set; }

  }
}
