using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Models.Contracts;

namespace Bot.Models {
  public interface IReceivedMessage : IReceived<IUser>, IMessage {

  }
}
