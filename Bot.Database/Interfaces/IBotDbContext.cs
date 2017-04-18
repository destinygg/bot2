using System;
using Bot.Database.Entities;
using Bot.Tools.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Bot.Database.Interfaces {
  public interface IBotDbContext : IDisposable, ISavable {
    DbSet<AutoPunishmentEntity> AutoPunishments { get; set; }
    DbSet<PunishedUserEntity> PunishedUsers { get; set; }
    DbSet<StateIntegerEntity> StateIntegers { get; set; }
    DbSet<CustomCommandEntity> CustomCommands { get; set; }
    DbSet<PeriodicMessageEntity> PeriodicMessages { get; set; }
    DatabaseFacade Database { get; }
  }
}
