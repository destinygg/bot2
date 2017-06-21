using System;
using System.Threading;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Pipeline {
  public abstract class BaseClient : IClient {
    private readonly ILogger _logger;

    protected const int MaximumBackoffTimeInSeconds = 60;
    protected int ConnectionFailureCount;

    protected BaseClient(ILogger logger) {
      _logger = logger;
    }

    public abstract void Connect();

    public abstract void Send(string output);

    public DateTime LatestReceivedAt { get; protected set; }

    protected void _onConnectionFailure() {
      ConnectionFailureCount++;
      var backoffTimeInSeconds = Math.Min((int) Math.Pow(2, ConnectionFailureCount) - 1, MaximumBackoffTimeInSeconds);
      _logger.LogInformation($"Unable to connect. {nameof(backoffTimeInSeconds)} is {backoffTimeInSeconds}. {nameof(ConnectionFailureCount)} is {ConnectionFailureCount}.");
      Thread.Sleep(TimeSpan.FromSeconds(backoffTimeInSeconds));
    }

  }
}
