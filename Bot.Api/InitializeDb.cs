namespace Bot.Api {
  public class InitializeDb {
    public InitializeDb() {
      using (var context = new BotDbContext()) {
        context.Database.EnsureCreated();
        context.StateIntegers.Add(new StateInteger(nameof(StateIntegerApi.LatestLiveTime), 0));
        context.SaveChanges();
      }
    }

  }
}
