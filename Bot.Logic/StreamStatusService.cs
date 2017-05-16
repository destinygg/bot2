using System;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {


  internal interface IStreamStatusContext {
    void TransitionToOn();
    void TransitionToOff();
    void TransitionToPossiblyOff();
  }


  public class StreamStatusService : IStreamStatusContext, IStreamStatusService {
    private readonly IDownloader _downloader;
    private readonly IStreamStatusStatus _onStatus;
    private readonly IStreamStatusStatus _offStatus;
    private readonly IStreamStatusStatus _possiblyOffStatus;

    private IStreamStatusStatus _currentStatus;

    public StreamStatusService(
      IQueryCommandService<IUnitOfWork> unitOfWork,
      ITimeService timeService,
      ISettings settings,
      IDownloader downloader
    ) {
      _downloader = downloader;

      _onStatus = new OnStatus(this, unitOfWork, timeService);
      _offStatus = new OffStatus(this, unitOfWork, timeService);
      _possiblyOffStatus = new PossiblyOffStatus(this, unitOfWork, timeService, settings);

      var initialStatus = unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      switch (initialStatus) {
        case StreamStatus.On:
          _currentStatus = _onStatus;
          break;
        case StreamStatus.Off:
          _currentStatus = _offStatus;
          break;
        case StreamStatus.PossiblyOff:
          _currentStatus = _possiblyOffStatus;
          break;
        default:
          throw new NotSupportedException($"The stream status {initialStatus} is not registered");
      }
    }

    void IStreamStatusContext.TransitionToOn() => _currentStatus = _onStatus;
    void IStreamStatusContext.TransitionToOff() => _currentStatus = _offStatus;
    void IStreamStatusContext.TransitionToPossiblyOff() => _currentStatus = _possiblyOffStatus;

    public StreamStatus Get() {
      var newStatus = _downloader.StreamStatus();
      _currentStatus.Refresh(newStatus.IsLive);
      return _currentStatus.StreamStatus;
    }


    #region Nested Members
    private interface IStreamStatusStatus {
      void Refresh(bool isLive);
      StreamStatus StreamStatus { get; }
    }


    private class OnStatus : IStreamStatusStatus {
      private readonly IStreamStatusContext _context;
      private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
      private readonly ITimeService _timeService;

      public OnStatus(IStreamStatusContext context, IQueryCommandService<IUnitOfWork> unitOfWork, ITimeService timeService) {
        _context = context;
        _unitOfWork = unitOfWork;
        _timeService = timeService;
      }

      public void Refresh(bool isLive) {
        if (!isLive) {
          _unitOfWork.Command(u => {
            u.StateIntegers.StreamStatus = StreamStatus.PossiblyOff;
            u.StateIntegers.LatestStreamOffTime = _timeService.UtcNow;
          });
          _context.TransitionToPossiblyOff();
        }
      }

      public StreamStatus StreamStatus => StreamStatus.On;
    }


    private class OffStatus : IStreamStatusStatus {
      private readonly IStreamStatusContext _context;
      private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
      private readonly ITimeService _timeService;

      public OffStatus(IStreamStatusContext context, IQueryCommandService<IUnitOfWork> unitOfWork, ITimeService timeService) {
        _context = context;
        _unitOfWork = unitOfWork;
        _timeService = timeService;
      }

      public void Refresh(bool isLive) {
        if (isLive) {
          _unitOfWork.Command(u => {
            u.StateIntegers.StreamStatus = StreamStatus.On;
            u.StateIntegers.LatestStreamOnTime = _timeService.UtcNow;
          });
          _context.TransitionToOn();
        }
      }

      public StreamStatus StreamStatus => StreamStatus.Off;
    }


    private class PossiblyOffStatus : IStreamStatusStatus {
      private readonly IStreamStatusContext _context;
      private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
      private readonly ITimeService _timeService;
      private readonly ISettings _settings;

      public PossiblyOffStatus(IStreamStatusContext context, IQueryCommandService<IUnitOfWork> unitOfWork, ITimeService timeService, ISettings settings) {
        _context = context;
        _unitOfWork = unitOfWork;
        _timeService = timeService;
        _settings = settings;
      }

      public void Refresh(bool isLive) {
        if (isLive) {
          _unitOfWork.Command(u => u.StateIntegers.StreamStatus = StreamStatus.On);
          _context.TransitionToOn();
        } else {
          var possibleStreamOffTime = _unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
          if (possibleStreamOffTime + _settings.OnOffTimeTolerance <= _timeService.UtcNow) {
            _unitOfWork.Command(u => u.StateIntegers.StreamStatus = StreamStatus.Off);
            _context.TransitionToOff();
          }
        }
      }

      public StreamStatus StreamStatus => StreamStatus.PossiblyOff;
    }
    #endregion


  }
}
