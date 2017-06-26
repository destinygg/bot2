using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface ICommandLogic {
    ISendable<PublicMessage> Time();
    ISendable<PublicMessage> Schedule();
    ISendable<PublicMessage> Blog();
    IEnumerable<ISendable<PublicMessage>> Streams();
    IEnumerable<ISendable<PublicMessage>> TwitterDestiny();
    IEnumerable<ISendable<PublicMessage>> TwitterAslan();
    IEnumerable<ISendable<PublicMessage>> Song();
    IEnumerable<ISendable<PublicMessage>> PreviousSong();
    IEnumerable<ISendable<PublicMessage>> Live();
    IEnumerable<ISendable<PublicMessage>> Youtube();
  }
}
