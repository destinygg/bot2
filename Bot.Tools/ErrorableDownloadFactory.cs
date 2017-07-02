using System;
using System.Collections.Generic;
using System.Linq;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Tools {
  public class ErrorableDownloadFactory : IErrorableFactory<string, string, string, string> {
    private readonly IFactory<string, string, string> _downloadFactory;
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;
    private readonly ILogger _logger;
    private readonly Dictionary<string, IList<DateTime>> _urlFailures;

    public ErrorableDownloadFactory(
      IFactory<string, string, string> downloadFactory,
      ITimeService timeService,
      ISettings settings,
      ILogger logger
      ) {
      _downloadFactory = downloadFactory;
      _timeService = timeService;
      _settings = settings;
      _logger = logger;
      _urlFailures = new Dictionary<string, IList<DateTime>>();
    }

    public string Create(string url, string header, string error) {
      OnErrorCreate = error;
      try {
        return _downloadFactory.Create(url, header);
      } catch (Exception) {
        _updateUrlFailures(url);
        var dailyFailCount = _urlFailures[url].Count;
        if (dailyFailCount > _settings.DownloadErrorLimit) {
          throw;
        }
        _logger.LogWarning($"Error in {nameof(ErrorableDownloadFactory)}.\r\n" +
                           $"{nameof(url)}={url}\r\n" +
                           $"{nameof(header)}={header}\r\n" +
                           $"{nameof(error)}={error}");
        return OnErrorCreate;
      }
    }

    private void _updateUrlFailures(string url) {
      if (!_urlFailures.ContainsKey(url)) {
        _urlFailures.Add(url, new List<DateTime>());
      }
      _urlFailures[url].Add(_timeService.UtcNow);
      _urlFailures[url] = _urlFailures[url].Where(x => x.IsWithin(_timeService.UtcNow, _settings.DownloadErrorWindow)).ToList();
    }

    public string OnErrorCreate { get; private set; }
  }
}
