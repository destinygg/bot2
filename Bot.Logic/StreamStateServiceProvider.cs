using Bot.Logic.Interfaces;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {

  // StreamStateService requires a database call in its ctor, so this class provides a layer of indirection so that the DI Container may .Verify()
  public class StreamStateServiceProvider : IProvider<StreamStateService> {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly ITimeService _timeService;
    private readonly IDownloadMapper _downloadMapper;
    private readonly ISettings _settings;

    public StreamStateServiceProvider(
      IQueryCommandService<IUnitOfWork> unitOfWork,
      ITimeService timeService,
      IDownloadMapper downloadMapper,
      ISettings settings
    ) {
      _unitOfWork = unitOfWork;
      _timeService = timeService;
      _downloadMapper = downloadMapper;
      _settings = settings;
    }

    public StreamStateService Get() => new StreamStateService(_unitOfWork, _timeService, _downloadMapper, _settings);

  }
}
