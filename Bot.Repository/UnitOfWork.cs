using Bot.Database.Interfaces;
using Bot.Repository.Interfaces;

namespace Bot.Repository {
  public class UnitOfWork : IUnitOfWork {
    private readonly IBotDbContext _context;

    public UnitOfWork(IBotDbContext context, IInMemoryRepository inMemoryRepository) {
      _context = context;
      StateIntegers = new StateIntegerRepository(_context.StateIntegers);
      AutoPunishments = new AutoPunishmentRepository(_context.AutoPunishments);
      PunishedUsers = new PunishedUserRepository(_context.PunishedUsers, _context.AutoPunishments);
      CustomCommand = new CustomCommandRepository(_context.CustomCommands);
      PeriodicMessages = new PeriodicMessageRepository(_context.PeriodicMessages );
      InMemory = inMemoryRepository;
    }

    public IStateIntegerRepository StateIntegers { get; }
    public IAutoPunishmentRepository AutoPunishments { get; }
    public IPunishedUserRepository PunishedUsers { get; }
    public IInMemoryRepository InMemory { get; }
    public ICustomCommandRepository CustomCommand { get; }
    public IPeriodicMessageRepository PeriodicMessages { get; }

    public void Dispose() => _context.Dispose();

    public int SaveChanges() => _context.SaveChanges();
  }
}
