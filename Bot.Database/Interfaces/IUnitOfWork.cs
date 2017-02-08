using System;

namespace Bot.Database.Interfaces {
  public interface IUnitOfWork : IDisposable {
    IStateIntegerRepository StateIntegers { get; }
    IAutoPunishmentRepository AutoPunishments { get; }
    IUserRepository Users { get; }
    IPunishedUserRepository PunishedUsers { get; }
    int Complete();
  }
}
