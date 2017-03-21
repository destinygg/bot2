using System;
using Bot.Tools.Interfaces;

namespace Bot.Repository.Interfaces {
  public interface IUnitOfWork : IDisposable, ISavable {
    IStateIntegerRepository StateIntegers { get; }
    IAutoPunishmentRepository AutoPunishments { get; }
  }
}
