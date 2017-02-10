using System.Diagnostics;
using Bot.Models.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("Pardoned {Target})")]
  public class SendablePardon : ISendable<Pardon> {
    public SendablePardon(IUser target) {
      Transmission = new Pardon(target);
    }

    public Pardon Transmission { get; }
    public IUser Target => Transmission.Target;
    public TResult Accept<TResult>(ISendableVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
