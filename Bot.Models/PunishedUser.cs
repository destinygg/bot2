namespace Bot.Models {
  public class PunishedUser {
    public int Id { get; set; }
    public int AutoPunishmentId { get; set; }
    public virtual AutoPunishment AutoPunishment { get; set; }
    public string Nick { get; set; }
    public int Count { get; set; }
  }
}
