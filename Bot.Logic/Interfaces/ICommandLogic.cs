using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface ICommandLogic {
    ISendable<PublicMessage> Time();
    ISendable<PublicMessage> Schedule();
  }
}
