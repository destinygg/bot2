using System.Collections.Generic;

namespace Bot.Database.Entities {

  public class AutoPunishmentEntity {
    public int Id { get; set; }
    public string Term { get; set; }
    public AutoPunishmentType Type { get; set; }
    public long Duration { get; set; }
    public virtual ICollection<PunishedUserEntity> PunishedUsers { get; set; }
  }

  public class UserEntity {
    public int Id { get; set; }
    public string Nick { get; set; }
    public virtual ICollection<PunishedUserEntity> PunishedUsers { get; set; }
  }

  public class PunishedUserEntity {
    public int AutoPunishmentId { get; set; }
    public virtual AutoPunishmentEntity AutoPunishmentEntity { get; set; }

    public int UserId { get; set; }
    public virtual UserEntity UserEntity { get; set; }

    public int Count { get; set; }
  }

  public enum AutoPunishmentType {
    BannedRegexm,
    MutedRegex,
    BannedString,
    MutedString,
  }

}
