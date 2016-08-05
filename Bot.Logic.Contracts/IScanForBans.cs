using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IScanForBans {
    IEnumerable<ISendable> Scan(IPublicMessageReceived message);
  }
}
