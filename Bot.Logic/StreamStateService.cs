﻿using System;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Repository.Interfaces;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {


  internal interface IStreamStatusContext {
    void TransitionToOn(bool updateLatestStreamOnTime);
    void TransitionToOff();
    void TransitionToPossiblyOff();
  }


  public class StreamStateService : IStreamStatusContext, IStreamStateService {
    private readonly IQueryCommandService<IUnitOfWork> _unitOfWork;
    private readonly ITimeService _timeService;
    private readonly IDownloadMapper _downloadMapper;

    private readonly IStreamStatusStatus _onStatus;
    private readonly IStreamStatusStatus _offStatus;
    private readonly IStreamStatusStatus _possiblyOffStatus;

    private IStreamStatusStatus _currentStatus;
    private DateTime _latestStreamOnTime;
    private DateTime _latestStreamOffTime;

    public StreamStateService(
      IQueryCommandService<IUnitOfWork> unitOfWork,
      ITimeService timeService,
      IDownloadMapper downloadMapper,
      ISettings settings
    ) {
      _unitOfWork = unitOfWork;
      _timeService = timeService;
      _downloadMapper = downloadMapper;

      _onStatus = new OnStatus(this);
      _offStatus = new OffStatus(this);
      _possiblyOffStatus = new PossiblyOffStatus(this, _unitOfWork, _timeService, settings);

      var initialStatus = unitOfWork.Query(u => u.StateIntegers.StreamStatus);
      _latestStreamOnTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOnTime);
      _latestStreamOffTime = unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
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

    void IStreamStatusContext.TransitionToOn(bool updateLatestStreamOnTime) {
      _currentStatus = _onStatus;
      if (updateLatestStreamOnTime) {
        _latestStreamOnTime = _timeService.UtcNow;
        _unitOfWork.Command(u => {
          u.StateIntegers.StreamStatus = StreamStatus.On;
          u.StateIntegers.LatestStreamOnTime = _latestStreamOnTime;
        });
      } else {
        _unitOfWork.Command(u => u.StateIntegers.StreamStatus = StreamStatus.On);
      }
    }

    void IStreamStatusContext.TransitionToOff() {
      _currentStatus = _offStatus;
      _unitOfWork.Command(u => u.StateIntegers.StreamStatus = StreamStatus.Off);
    }

    void IStreamStatusContext.TransitionToPossiblyOff() {
      _currentStatus = _possiblyOffStatus;
      _latestStreamOffTime = _timeService.UtcNow;
      _unitOfWork.Command(u => {
        u.StateIntegers.StreamStatus = StreamStatus.PossiblyOff;
        u.StateIntegers.LatestStreamOffTime = _latestStreamOffTime;
      });
    }

    public StreamState Get() {
      var newStatus = _downloadMapper.StreamStatus();
      _currentStatus.Refresh(newStatus.IsLive);
      return new StreamState(_currentStatus.StreamStatus, _latestStreamOnTime, _latestStreamOffTime, newStatus);
    }


    #region Nested Members
    private interface IStreamStatusStatus {
      void Refresh(bool isLive);
      StreamStatus StreamStatus { get; }
    }


    private class OnStatus : IStreamStatusStatus {
      private readonly IStreamStatusContext _context;

      public OnStatus(IStreamStatusContext context) {
        _context = context;
      }

      public void Refresh(bool isLive) {
        if (!isLive) {
          _context.TransitionToPossiblyOff();
        }
      }

      public StreamStatus StreamStatus => StreamStatus.On;
    }


    private class OffStatus : IStreamStatusStatus {
      private readonly IStreamStatusContext _context;

      public OffStatus(IStreamStatusContext context) {
        _context = context;
      }

      public void Refresh(bool isLive) {
        if (isLive) {
          _context.TransitionToOn(true);
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
          _context.TransitionToOn(false);
        } else {
          var possibleStreamOffTime = _unitOfWork.Query(u => u.StateIntegers.LatestStreamOffTime);
          if (possibleStreamOffTime + _settings.OnOffTimeTolerance <= _timeService.UtcNow) {
            _context.TransitionToOff();
          }
        }
      }

      public StreamStatus StreamStatus => StreamStatus.PossiblyOff;
    }
    #endregion


  }
}
