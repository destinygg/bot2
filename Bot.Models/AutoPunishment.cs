using System.Collections.Generic;
using Bot.Database.Entities;

namespace Bot.Models {
  public class AutoPunishment {
    public int Id { get; set; }
    public string Term { get; set; }
    public AutoPunishmentType Type { get; set; }
    public long Duration { get; set; }
    public virtual ICollection<PunishedUser> PunishedUsers { get; set; }
  }
}
