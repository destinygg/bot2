﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Logic.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Main.Moderate {
  public class PeriodicTaskRunner {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly IProvider<IStreamStateService> _streamStateServiceProvider;
    private readonly IDownloadMapper _downloadMapper;
    private readonly ISettings _settings;
    private readonly IFactory<TimeSpan, Action, Task> _periodicTaskFactory;
    private readonly IPipelineManager _pipelineManager;
    private readonly ILogger _logger;

    public PeriodicTaskRunner(
      IQueryCommandService<IUnitOfWork> unitOfWork,
      IFactory<TimeSpan, Action, Task> periodicTaskFactory,
      IProvider<IStreamStateService> streamStateServiceProvider,
      IDownloadMapper downloadMapper,
      ISettings settings,
      IPipelineManager pipelineManager,
      ILogger logger) {
      _periodicTaskFactory = periodicTaskFactory;
      _pipelineManager = pipelineManager;
      _unitOfWork = unitOfWork;
      _streamStateServiceProvider = streamStateServiceProvider;
      _downloadMapper = downloadMapper;
      _settings = settings;
      _logger = logger;
    }

    public void Run() {
      RepeatingMessages(_periodicTaskFactory);
      RefreshStreamStatus(_periodicTaskFactory);
    }

    private void RepeatingMessages(IFactory<TimeSpan, Action, Task> periodicTaskFactory) {
      var rng = new Random();
      var messageCount = PeriodicMessages().Count();
      var i = rng.Next(messageCount);
      periodicTaskFactory.Create(_settings.PeriodicTaskInterval, () => {
        _pipelineManager.Enqueue(new SendablePublicMessage(PeriodicMessages().Skip(i).First()));
        i++;
        if (i >= messageCount) {
          i = 0;
        }
      });
    }

    private IEnumerable<string> PeriodicMessages() {
      foreach (var sendablePublicMessage in _unitOfWork.Query(u => u.PeriodicMessages.GetAll)) {
        yield return sendablePublicMessage;
      }
      yield return GetLatestYoutube();
    }

    private string GetLatestYoutube() {
      var feed = _downloadMapper.YoutubeFeed();
      var video = feed?.Entry.OrderByDescending(x => x.ParsedPublished).First();
      return video == null
        ? "An error occured while contacting YouTube."
        : $"\"{video.Title}\" posted {(DateTime.UtcNow - video.ParsedPublished).ToPretty(_logger)} ago youtu.be/{video.VideoId}";
    }

    private void RefreshStreamStatus(IFactory<TimeSpan, Action, Task> periodicTaskFactory) =>
      periodicTaskFactory.Create(_settings.PeriodicTaskInterval, () => _streamStateServiceProvider.Get().Get());

  }
}