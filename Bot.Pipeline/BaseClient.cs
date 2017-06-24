using System;
using System.Threading;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public abstract class BaseClient : IClient {
    private readonly ILogger _logger;
    private readonly ITimeService _timeService;
    private readonly IPipelineManager _pipelineManager;

    protected const int MaximumBackoffTimeInSeconds = 60;
    private int _connectionFailureCount;
    private DateTime _lastConnectedAt;

    protected BaseClient(
      ILogger logger,
      ITimeService timeService,
      IPipelineManager pipelineManager
    ) {
      _logger = logger;
      _timeService = timeService;
      _pipelineManager = pipelineManager;
    }

    public void TryConnect() {
      if (_lastConnectedAt - _timeService.UtcNow > TimeSpan.FromSeconds(MaximumBackoffTimeInSeconds)) {
        _connectionFailureCount = 0;
      }
      while (!IsConnected()) {
        try {
          Connect();
        } catch (Exception e) {
          _logger.LogError($"{nameof(DestinyGgBaseClient)} had an error connecting.", e);
        } finally {
          if (!IsConnected()) {
            _connectionBackoff();
          }
          Thread.Sleep(TimeSpan.FromSeconds(1));
        }
      }
    }

    protected abstract Func<bool> IsConnected { get; }

    protected abstract Action Connect { get; }

    public abstract void Disconnect();

    public abstract void Send(string output);

    public DateTime LatestReceivedAt { get; private set; }

    private void _connectionBackoff() {
      _connectionFailureCount++;
      var backoffTimeInSeconds = Math.Min((int) Math.Pow(2, _connectionFailureCount) - 1, MaximumBackoffTimeInSeconds);
      _logger.LogWarning($"Unable to connect. {nameof(backoffTimeInSeconds)} is {backoffTimeInSeconds}. {nameof(_connectionFailureCount)} is {_connectionFailureCount}.");
      Thread.Sleep(TimeSpan.FromSeconds(backoffTimeInSeconds));
    }

    protected void Messaged(string message) {
      LatestReceivedAt = _timeService.UtcNow;
      _pipelineManager.Enqueue(message);
    }

    protected void Messaged(IReceived<IUser, ITransmittable> message) {
      LatestReceivedAt = _timeService.UtcNow;
      _pipelineManager.Enqueue(message);
    }

    protected void Connected() {
      _lastConnectedAt = _timeService.UtcNow;
      _logger.LogInformation("Connected!");
    }

    protected void Disconnected() {
      _logger.LogError("Connection closed! Delaying...");
      _connectionBackoff();
      Connect();
    }

    protected void Errored(Exception e) {
      _logger.LogError("Connection error! Disconnecting...", e);
      Disconnect();
    }

  }
}
