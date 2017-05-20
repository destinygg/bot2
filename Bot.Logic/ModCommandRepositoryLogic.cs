using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Database.Entities;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class ModCommandRepositoryLogic : IModCommandRepositoryLogic {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly ILogger _logger;

    public ModCommandRepositoryLogic(
      IQueryCommandService<IUnitOfWork> unitOfWork,
      ILogger logger
    ) {
      _unitOfWork = unitOfWork;
      _logger = logger;
    }

    public IReadOnlyList<ISendable<ITransmittable>> AddCommand(string command, string response) {
      var existingCommand = _unitOfWork.Query(r => r.CustomCommand.Get(command));
      string confirmation;
      if (existingCommand == null) {
        _unitOfWork.Command(r => r.CustomCommand.Add(command, response));
        confirmation = $"!{command} added";
      } else {
        _unitOfWork.Command(r => r.CustomCommand.Update(command, response));
        confirmation = $"!{command} updated";
      }
      return new SendablePublicMessage(confirmation).Wrap().ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> DelCommand(string command) {
      var existingCommand = _unitOfWork.Query(r => r.CustomCommand.Get(command));
      string confirmation;
      if (existingCommand == null) {
        confirmation = $"!{command} is not an existing custom command";
      } else {
        _unitOfWork.Command(r => r.CustomCommand.Delete(command));
        confirmation = $"!{command} removed";
      }
      return new SendablePublicMessage(confirmation).Wrap().ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> AddMute(string mutedPhrase, TimeSpan duration) =>
      _addPunishment(mutedPhrase, duration, AutoPunishmentType.MutedString, "automute");

    public IReadOnlyList<ISendable<ITransmittable>> AddMuteRegex(string mutedPhrase, TimeSpan duration) =>
      _addPunishment(mutedPhrase, duration, AutoPunishmentType.MutedRegex, "automute regex");

    public IReadOnlyList<ISendable<ITransmittable>> AddBan(string bannedPhrase, TimeSpan duration) =>
      _addPunishment(bannedPhrase, duration, AutoPunishmentType.BannedString, "autoBAN");

    public IReadOnlyList<ISendable<ITransmittable>> AddBanRegex(string bannedPhrase, TimeSpan duration) =>
      _addPunishment(bannedPhrase, duration, AutoPunishmentType.BannedRegex, "autoBAN regex");

    public IReadOnlyList<ISendable<ITransmittable>> DelMute(string mutedPhrase) =>
      _deletePunishment(mutedPhrase, AutoPunishmentType.MutedString, "automute");

    public IReadOnlyList<ISendable<ITransmittable>> DelMuteRegex(string mutedPhrase) =>
      _deletePunishment(mutedPhrase, AutoPunishmentType.MutedRegex, "automute regex");

    public IReadOnlyList<ISendable<ITransmittable>> DelBan(string bannedPhrase) =>
      _deletePunishment(bannedPhrase, AutoPunishmentType.BannedString, "autoBAN");

    public IReadOnlyList<ISendable<ITransmittable>> DelBanRegex(string bannedPhrase) =>
      _deletePunishment(bannedPhrase, AutoPunishmentType.BannedRegex, "autoBAN regex");

    private IReadOnlyList<ISendable<ITransmittable>> _addPunishment(string punishedPhrase, TimeSpan duration, AutoPunishmentType type, string name) {
      var autoPunishment = _unitOfWork.Query(u => u.AutoPunishments.Get(punishedPhrase, type));
      if (autoPunishment == null) {
        _unitOfWork.Command(u => u.AutoPunishments.Add(new AutoPunishment(punishedPhrase, type, duration)));
        return new SendablePublicMessage($"{punishedPhrase} added to {name} list for {duration.ToPretty(_logger)}").Wrap().ToList();
      }
      autoPunishment.Duration = duration;
      _unitOfWork.Command(u => u.AutoPunishments.Update(autoPunishment));
      return new SendablePublicMessage($"{punishedPhrase} is already in the {name} list. Its duration has been updated to {duration.ToPretty(_logger)}").Wrap().ToList();
    }

    private IReadOnlyList<ISendable<ITransmittable>> _deletePunishment(string punishedPhrase, AutoPunishmentType type, string name) {
      var autoPunishment = _unitOfWork.Query(u => u.AutoPunishments.Get(punishedPhrase, type));
      if (autoPunishment == null) {
        return new SendablePublicMessage($"{punishedPhrase} is not in the {name} list").Wrap().ToList();
      }
      _unitOfWork.Command(u => u.AutoPunishments.Delete(autoPunishment));
      return new SendablePublicMessage($"{punishedPhrase} deleted from the {name} list").Wrap().ToList();
    }

  }
}
