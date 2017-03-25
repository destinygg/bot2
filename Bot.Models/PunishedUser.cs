using Bot.Database.Entities;

namespace Bot.Models {
  public class PunishedUser {

    public PunishedUser(PunishedUserEntity entity, AutoPunishment autoPunishment) {
      Id = entity.Id;
      Nick = entity.Nick;
      Count = entity.Count;
      AutoPunishment = autoPunishment;
      AutoPunishment = autoPunishment;
    }

    public int Id { get; }
    public string Nick { get; }
    public int Count { get; }
    public AutoPunishment AutoPunishment { get; }

    public PunishedUserEntity ToEntity() => new PunishedUserEntity {
      Id = Id,
      Nick = Nick,
      Count = Count,
    };

  }
}
