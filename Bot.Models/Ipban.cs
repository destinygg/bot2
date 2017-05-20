using System;

namespace Bot.Models {
  public class Ipban : Punishment {
    public Ipban(Civilian target, TimeSpan duration) : base(target, duration) { }
    public Ipban(Civilian target, TimeSpan duration, string reason) : base(target, duration, reason) { }
    public override string PastTense => "Ipbanned";
  }
}
