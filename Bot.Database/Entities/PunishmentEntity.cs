using System.Collections.Generic;

namespace Bot.Database.Entities {

  public class AutoPunishmentEntity {
    public AutoPunishmentEntity() {
      PunishedUsers = new HashSet<PunishedUserEntity>();
    }
    public int Id { get; set; }
    public string Term { get; set; }
    public AutoPunishmentType Type { get; set; }
    public long Duration { get; set; }

    public virtual ICollection<PunishedUserEntity> PunishedUsers { get; set; }
  }

  public class PunishedUserEntity {
    public int Id { get; set; }
    public int AutoPunishmentId { get; set; }
    public virtual AutoPunishmentEntity AutoPunishmentEntity { get; set; }
    public string Nick { get; set; }
    public int Count { get; set; }
  }

  public enum AutoPunishmentType {
    BannedRegex,
    MutedRegex,
    BannedString,
    MutedString,
  }

}
