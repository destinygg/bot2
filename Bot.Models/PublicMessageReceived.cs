using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class PublicMessageReceived : PublicMessage, IPublicMessageReceived {
    public DateTime Timestamp { get; }
    public IUser Sender { get; }

    public PublicMessageReceived(string text) : base(text) {

    }
  }
}
