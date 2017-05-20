using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IModCommandRepositoryLogic {
    IReadOnlyList<ISendable<ITransmittable>> AddCommand(string command, string response);
    IReadOnlyList<ISendable<ITransmittable>> DelCommand(string command);
    IReadOnlyList<ISendable<ITransmittable>> AddMute(string mutedPhrase, TimeSpan duration);
    IReadOnlyList<ISendable<ITransmittable>> AddMuteRegex(string mutedPhrase, TimeSpan duration);
    IReadOnlyList<ISendable<ITransmittable>> AddBan(string bannedPhrase, TimeSpan duration);
    IReadOnlyList<ISendable<ITransmittable>> AddBanRegex(string bannedPhrase, TimeSpan duration);
    IReadOnlyList<ISendable<ITransmittable>> DelMute(string mutedPhrase);
    IReadOnlyList<ISendable<ITransmittable>> DelMuteRegex(string mutedPhrase);
    IReadOnlyList<ISendable<ITransmittable>> DelBan(string bannedPhrase);
    IReadOnlyList<ISendable<ITransmittable>> DelBanRegex(string bannedPhrase);
  }
}
