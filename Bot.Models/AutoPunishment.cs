using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;

namespace Bot.Models {
  public class AutoPunishment {

    public AutoPunishment(AutoPunishmentEntity entity) {
      Id = entity.Id;
      Term = entity.Term;
      Type = entity.Type;
      Duration = entity.Duration;
      PunishedUsers = entity.PunishedUsers.Select(x => new PunishedUser(x, this)).ToList();
    }

    public int Id { get; }
    public string Term { get; }
    public AutoPunishmentType Type { get; }
    public long Duration { get; }
    public ICollection<PunishedUser> PunishedUsers { get; }

    public AutoPunishmentEntity ToEntity() => new AutoPunishmentEntity {
      Id = Id,
      Term = Term,
      Type = Type,
      Duration = Duration,
      PunishedUsers = PunishedUsers.Select(u => u.ToEntity()).ToList(),
    };

  }
}
