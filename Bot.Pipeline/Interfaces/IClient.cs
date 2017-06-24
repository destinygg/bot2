using System;

namespace Bot.Pipeline.Interfaces {
  public interface IClient {
    void TryConnect();
    void Disconnect();
    void Send(string output);
    DateTime LatestReceivedAt { get; }
  }
}
