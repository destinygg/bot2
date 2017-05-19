using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
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
    private readonly IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> _nukeMuteFactory;
    private readonly IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> _aegisPardonFactory;
    private readonly IQueryCommandService<IUnitOfWork> _repository;
    private readonly IDownloadMapper _downloadMapper;
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;
    private readonly ILogger _logger;

    public ModCommandLogic(
      IErrorableFactory<Nuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> nukeMuteFactory,
      IErrorableFactory<IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> aegisPardonFactory,
      IQueryCommandService<IUnitOfWork> repository,
      IDownloadMapper downloadMapper,
      ITimeService timeService,
      ISettings settings,
      ILogger logger) {
      _nukeMuteFactory = nukeMuteFactory;
      _aegisPardonFactory = aegisPardonFactory;
      _repository = repository;
      _downloadMapper = downloadMapper;
      _timeService = timeService;
      _settings = settings;
      _logger = logger;
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

    public IReadOnlyList<ISendable<ITransmittable>> Stalk(string user) {
      var output = new List<ISendable<ITransmittable>>();
      if (_settings.ClientType.Contains("DestinyGg")) {
        output.Add(new SendablePublicMessage($"dgg.overrustlelogs.net/{user}"));
        string rawDownload;
        try {
          rawDownload = _downloadMapper.OverRustleLogs(user);
        } catch (WebException wex) {
          if (((HttpWebResponse) wex.Response).StatusCode == HttpStatusCode.NotFound) {
            return new SendablePublicMessage($"{user} not found").Wrap().ToList();
          }
          throw;
        }
        var lines = Regex.Split(rawDownload, "\n");
        var lastlines = lines.TakeLast(4).Where(l => !string.IsNullOrWhiteSpace(l)); // last line is empty
        foreach (var line in lastlines) {
          var match = new Regex(@"\[([\d- :]+)UTC] (?:.*: )(.*)").Match(line);
          var stringTime = match.Groups[1].Value;
          var message = match.Groups[2].Value;
          var timeStamp = DateTime.ParseExact(stringTime, "yyyy-MM-dd HH:mm:ss ", CultureInfo.InvariantCulture);
          var delta = (_timeService.UtcNow - timeStamp).ToPretty(_logger);
          output.Add(new SendablePublicMessage($"{delta} ago: {message}"));
        }
      }
      return output;
    }

  }
}
