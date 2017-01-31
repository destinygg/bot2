using System;

namespace Bot.Logic.Interfaces {
  public interface IModCommandParser {

    /// <returns>User</returns>
    string Stalk(string input);
    /// <returns>On or Off; True or False</returns>
    bool SubOnly(string input);

    /// <returns>!trigger, response</returns>
    Tuple<string, string> AddCommand(string input);
    /// <returns>!triggerToDelete</returns>
    string DelCommand(string input);

    /// <returns>MutedWord, duration</returns>
    Tuple<string, TimeSpan> AddMute(string input);
    /// <returns>BannedWord, duration</returns>
    Tuple<string, TimeSpan> AddBan(string input);
    /// <returns>MutedRegex, duration</returns>
    Tuple<string, TimeSpan> AddMuteRegex(string input);
    /// <returns>BannedRegex, duration</returns>
    Tuple<string, TimeSpan> AddBanRegex(string input);

    /// <returns>muteToDelete</returns>
    string DelMute(string input);
    /// <returns>banToDelete</returns>
    string DelBan(string input);
    /// <returns>muteRegexToDelete</returns>
    string DelMuteRegex(string input);
    /// <returns>banRegexToDelete</returns>
    string DelBanRegex(string input);

    /// <returns>user, duration</returns>
    Tuple<string, TimeSpan> Mute(string input);
    /// <returns>user, duration</returns>
    Tuple<string, TimeSpan> Ban(string input);
    /// <returns>user, duration</returns>
    Tuple<string, TimeSpan> Ipban(string input);
    /// <returns>user</returns>
    string Pardon(string input);

    /// <returns>phrase, duration</returns>
    Tuple<string, TimeSpan> Nuke(string input);
    /// <returns>phrase, duration</returns>
    Tuple<string, TimeSpan> RegexNuke(string input);
  }
}
