using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface ICommandLogic {
    ISendable<PublicMessage> Time();
    ISendable<PublicMessage> Schedule();
    ISendable<PublicMessage> Blog();
    IEnumerable<ISendable<PublicMessage>> Streams();
  }
}
