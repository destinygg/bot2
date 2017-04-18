namespace Bot.Database.Entities {
  public class PeriodicMessageEntity {
    public PeriodicMessageEntity(string message) {
      Message = message;
    }

    public PeriodicMessageEntity() {

    }

    public int Id { get; set; }
    public string Message { get; set; }

  }
}
