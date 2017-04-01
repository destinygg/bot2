﻿using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Bot.Tools;

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

    public void CopyTo(AutoPunishmentEntity entity) {
      entity.Id = Id;
      entity.Term = Term;
      entity.Type = Type;
      entity.Duration = Duration;
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
