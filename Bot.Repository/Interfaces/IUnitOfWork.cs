using System;
using Bot.Database.Interfaces;

namespace Bot.Repository.Interfaces {
  public interface IUnitOfWork : IDisposable, ISavable {
    IStateIntegerRepository StateIntegers { get; }
    IAutoPunishmentRepository AutoPunishments { get; }
    IUserRepository Users { get; }
    IPunishedUserRepository PunishedUsers { get; }
  }
}
