using System.Linq;
using Bot.Database.Entities;
using Bot.Models;
using Bot.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bot.Repository {
  public class PunishedUserRepository : IPunishedUserRepository {
    private readonly DbSet<PunishedUserEntity> _punishedUserEntities;
    private readonly DbSet<AutoPunishmentEntity> _autoPunishmentEntities;

    public PunishedUserRepository(DbSet<PunishedUserEntity> punishedUserEntities, DbSet<AutoPunishmentEntity> autoPunishmentEntities) {
      _punishedUserEntities = punishedUserEntities;
      _autoPunishmentEntities = autoPunishmentEntities;
    }

    public PunishedUser GetUser(string nick) =>
        new PunishedUser(_punishedUserEntities.Include(x => x.AutoPunishmentEntity).Single(f => f.Nick == nick));

    public void Increment(string nick, string term) {
      var punishedUserEntity = _punishedUserEntities.Include(x => x.AutoPunishmentEntity).SingleOrDefault(pue => pue.Nick == nick);
      if (punishedUserEntity == null) {
        var autoPunishmentEntity = _autoPunishmentEntities.Single(ap => ap.Term == term);
        _punishedUserEntities.Add(new PunishedUserEntity {
          Nick = nick,
          Count = 1,
          AutoPunishmentId = autoPunishmentEntity.Id,
        });
      } else {
        punishedUserEntity.Count++;
        _punishedUserEntities.Update(punishedUserEntity);
      }

    }

  }
}
