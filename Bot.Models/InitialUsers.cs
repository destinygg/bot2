using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;

namespace Bot.Models {
  public class InitialUsers : ITransmittable {
    public InitialUsers(IEnumerable<IUser> users) {
      Users = users.ToList(); // creates a new instance
    }

    public List<IUser> Users { get; }
  }
}
