using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Bot.Logic.Contracts {
  public interface IModCommandRegex {
    Regex Sing { get; }
    Regex Dance { get; }

    Regex Stalk { get; }
    Regex SubOnly { get; }

    Regex AddCommand { get; }
    Regex DelCommand { get; }

    Regex AddMute { get; }
    Regex AddBan { get; }
    Regex AddMuteRegex { get; }
    Regex AddBanRegex { get; }

    Regex ListMute { get; }
    Regex ListBan { get; }
    Regex ListMuteRegex { get; }
    Regex ListBanRegex { get; }

    Regex DelMute { get; }
    Regex DelBan { get; }
    Regex DelMuteRegex { get; }
    Regex DelBanRegex { get; }

    Regex Mute { get; }
    Regex Ban { get; }
    Regex Ipban { get; }
    Regex UnMuteBan { get; }

    Regex Nuke { get; }
    Regex RegexNuke { get; }
    Regex Aegis { get; }

    IList<string> AllUnits { get; }
    IList<string> AllButPerm { get; }
    IList<string> Seconds { get; }
    IList<string> Minutes { get; }
    IList<string> Hours { get; }
    IList<string> Days { get; }
    IList<string> Perm { get; }
  }
}
