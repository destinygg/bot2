using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  public class ReceivedPardon : Received<Moderator, Pardon> {
    public ReceivedPardon(Moderator sender, Civilian target, ITimeService timeService) : base(timeService.UtcNow, sender) {
      Transmission = new Pardon(target);
    }

    public override Pardon Transmission { get; }
    public IUser Target => Transmission.Target;
    public override TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
