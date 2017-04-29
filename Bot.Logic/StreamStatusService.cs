using Bot.Logic.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class StreamStatusService : IStreamStatusService {
    private readonly IDownloader _downloader;
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly ITimeService _timeService;
    private readonly ISettings _settings;

    public StreamStatusService(
      IDownloader downloader,
      IQueryCommandService<IUnitOfWork> unitOfWork,
      ITimeService timeService,
      ISettings settings) {
      _downloader = downloader;
      _unitOfWork = unitOfWork;
      _timeService = timeService;
      _settings = settings;
    }

    public StreamStatus Refresh() {
      var now = _timeService.UtcNow;
      var streamStatus = _downloader.StreamStatus();
      var isOn = streamStatus.stream != null;
      var previously = _unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      if (isOn) {
        if (previously == StreamStatus.Off) {
          _unitOfWork.Command(u => u.StateIntegers.LatestStreamOnTime = now);
        }
        if (previously != StreamStatus.On) {
          _unitOfWork.Command(u => u.StateIntegers.StreamStatus = StreamStatus.On);
        }
        return StreamStatus.On;
      }
      // not on
      if (previously == StreamStatus.On) {
        _unitOfWork.Command(u => {
          u.StateIntegers.StreamStatus = StreamStatus.PossiblyOff;
          u.StateIntegers.LatestStreamOffTime = now;
        });
        return StreamStatus.PossiblyOff;
      }
      var possibleStreamOffTime = _unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
      if (possibleStreamOffTime + _settings.OnOffTimeTolerance > now) {
        return StreamStatus.PossiblyOff;
      }
      // outside tolerance
      if (previously == StreamStatus.PossiblyOff) {
        _unitOfWork.Command(u => u.StateIntegers.StreamStatus = StreamStatus.Off);
      }
      return StreamStatus.Off;
    }

  }
}
