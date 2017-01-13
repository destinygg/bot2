using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Models.Contracts {
  public static class IReceivedExtensionMethods {
    public static bool FromMod(this IReceived received) => received.Sender.IsMod;
  }
}
