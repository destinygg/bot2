using System.Collections.Generic;

namespace Bot.Database.Entities {

  public class AutoPunishment {
    public int Id { get; set; }
    public string Term { get; set; }
    public int Type { get; set; }
    public long Duration { get; set; }
    public virtual ICollection<PunishedUser> PunishedUsers { get; set; }
  }

  public class User {
    public int Id { get; set; }
    public string Nick { get; set; }
    public virtual ICollection<PunishedUser> PunishedUsers { get; set; }
  }

  public class PunishedUser {
    public int AutoPunishmentId { get; set; }
    public virtual AutoPunishment AutoPunishment { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }

    public int Count { get; set; }
  }

  public class AutoPunishmentType {
    public int Id { get; set; }
    public string Name { get; set; }
  }

}
