using Bot.Models.Contracts;

namespace Bot.Models {
  public class SendableUnMuteBan : UnMuteBan, ISendable {
    public SendableUnMuteBan(IUser target) : base(target) { }
  }
}
