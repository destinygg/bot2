using System;
using Bot.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Bot.Database.Interfaces {
  public interface IBotDbContext : IDisposable, ISavable {
    DbSet<AutoPunishment> AutoPunishments { get; set; }
    DbSet<PunishedUser> PunishedUsers { get; set; }
    DbSet<StateInteger> StateIntegers { get; set; }
    DbSet<User> Users { get; set; }
    DatabaseFacade Database { get; }
  }
}
