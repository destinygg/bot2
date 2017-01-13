using Bot.Models.Contracts;

namespace Bot.Models {
  public class SendableUnMuteBan : UnMuteBan, ISendable {
    public SendableUnMuteBan(IUser user) {
      Target = user;
    }
  }
}
