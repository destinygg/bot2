using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class PublicMessageReceived : IPublicMessageReceived {
    public DateTime Received { get; }
    public IUser Sender { get; }
    public string Text { get; set; }
  }
}
