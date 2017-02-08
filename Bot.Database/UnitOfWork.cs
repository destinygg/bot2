using Bot.Database.Interfaces;

namespace Bot.Database {
  public class UnitOfWork : IUnitOfWork {
    private readonly BotDbContext _context;

    public UnitOfWork(BotDbContext context) {
      _context = context;
      StateIntegers = new StateIntegerRepository(_context.StateIntegers);
      AutoPunishments = new AutoPunishmentRepository(_context.AutoPunishments);
      Users = new UserRepository(_context.Users);
      PunishedUsers = new PunishedUserRepository(_context.PunishedUsers);
    }

    public IStateIntegerRepository StateIntegers { get; }
    public IAutoPunishmentRepository AutoPunishments { get; }
    public IUserRepository Users { get; }
    public IPunishedUserRepository PunishedUsers { get; }
    public int Complete() => _context.SaveChanges();

    public void Dispose() {
      _context.Dispose();
    }

  }
}
