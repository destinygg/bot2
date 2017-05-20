using System;

namespace Bot.Models {
  public class Ipban : Punishment {
    public Ipban(Civilian target, TimeSpan duration, string reason = null) : base(target, duration, reason) { }
    public Ipban(string target, TimeSpan duration, string reason = null) : base(target, duration, reason) { }
    public override string PastTense => "Ipbanned";
  }
}
