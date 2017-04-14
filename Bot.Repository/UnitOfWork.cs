using Bot.Database.Interfaces;
using Bot.Repository.Interfaces;

namespace Bot.Repository {
  public class UnitOfWork : IUnitOfWork {
    private readonly IBotDbContext _context;

    public UnitOfWork(IBotDbContext context, INukeRepository nukeRepository) {
      _context = context;
      StateIntegers = new StateIntegerRepository(_context.StateIntegers);
      AutoPunishments = new AutoPunishmentRepository(_context.AutoPunishments);
      PunishedUsers = new PunishedUserRepository(_context.PunishedUsers, _context.AutoPunishments);
      Nukes = nukeRepository;
    }

    public IStateIntegerRepository StateIntegers { get; }
    public IAutoPunishmentRepository AutoPunishments { get; }
    public IPunishedUserRepository PunishedUsers { get; }
    public INukeRepository Nukes { get; }

    public void Dispose() => _context.Dispose();

    public int SaveChanges() => _context.SaveChanges();
  }
}
