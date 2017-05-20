using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Bot.Tools;

namespace Bot.Models {
  public class AutoPunishment {

    public AutoPunishment(string term, AutoPunishmentType type, TimeSpan duration) {
      Term = term;
      Type = type;
      Duration = duration;
      PunishedUsers = new List<PunishedUser>();
    }

    public AutoPunishment(AutoPunishmentEntity entity) {
      Id = entity.Id;
      Term = entity.Term;
      Type = entity.Type;
      Duration = TimeSpan.FromSeconds(entity.Duration);
      PunishedUsers = entity.PunishedUsers.Select(x => new PunishedUser(x, this)).ToList();
    }

    public int Id { get; }
    public string Term { get; }
    public AutoPunishmentType Type { get; }
    public TimeSpan Duration { get; set; }
    public ICollection<PunishedUser> PunishedUsers { get; }

    public void CopyTo(AutoPunishmentEntity entity) {
      entity.Id = Id;
      entity.Term = Term;
      entity.Type = Type;
      entity.Duration = Convert.ToInt32(Duration.TotalSeconds);
      entity.PunishedUsers.Merge(
        source: PunishedUsers.Select(x => x.ToEntity()),
        predicate: (a, b) => a.Id == b.Id,
        create: user => user,
        delete: person => entity.PunishedUsers.Remove(person),
        add: person => entity.PunishedUsers.Add(person),
        update: (d, s) => {
          d.Nick = s.Nick;
          d.Count = s.Count;
        }
      );
    }

  }
}
