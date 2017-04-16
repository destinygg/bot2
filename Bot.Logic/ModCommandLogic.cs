using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Logic {
  public class ModCommandLogic : IModCommandLogic {
    private readonly ILogger _logger;
    private readonly IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> _nukeMuteFactory;
    private readonly IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> _aegisPardonFactory;
    private readonly IQueryCommandService<IUnitOfWork> _repository;

    public ModCommandLogic(
      IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> nukeMuteFactory,
      IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> aegisPardonFactory,
      IQueryCommandService<IUnitOfWork> repository,
      ILogger logger) {
      _logger = logger;
      _nukeMuteFactory = nukeMuteFactory;
      _aegisPardonFactory = aegisPardonFactory;
      _repository = repository;
    }

    public ISendable<PublicMessage> Long(IReadOnlyList<IReceived<IUser, ITransmittable>> context) {
      _logger.LogInformation($"Long running process beginning, context length: {context.Count}");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("#1");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("#2");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation("#3");
      for (var i = 0; i < 1000000000; i++) {
        var temp = i;
      }
      _logger.LogInformation($"Long running process ending, context length: {context.Count()}");
      return new SendablePublicMessage("This is a debug command; output appears in log.");
    }

    public ISendable<PublicMessage> Sing() => new SendablePublicMessage("/me sings a song");

    public IReadOnlyList<ISendable<ITransmittable>> Nuke(IReadOnlyList<IReceived<IUser, ITransmittable>> context, Nuke nuke) => _nukeMuteFactory.Create(nuke, context);

    public IReadOnlyList<ISendable<ITransmittable>> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context) => _aegisPardonFactory.Create(context);

    public IReadOnlyList<ISendable<ITransmittable>> AddCommand(string command, string response) {
      var existingCommand = _repository.Query(r => r.CustomCommand.Get(command));
      string confirmation;
      if (existingCommand == null) {
        _repository.Command(r => r.CustomCommand.Add(command, response));
        confirmation = $"!{command} added";
      } else {
        _repository.Command(r => r.CustomCommand.Update(command, response));
        confirmation = $"!{command} updated";
      }
      return new SendablePublicMessage(confirmation).Wrap().ToList();
    }

    public IReadOnlyList<ISendable<ITransmittable>> DelCommand(string command) {
      var existingCommand = _repository.Query(r => r.CustomCommand.Get(command));
      string confirmation;
      if (existingCommand == null) {
        confirmation = $"!{command} is not an existing custom command";
      } else {
        _repository.Command(r => r.CustomCommand.Delete(command));
        confirmation = $"!{command} removed";
      }
      return new SendablePublicMessage(confirmation).Wrap().ToList();
    }

  }
}
