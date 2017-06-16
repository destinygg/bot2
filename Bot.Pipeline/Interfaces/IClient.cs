using System;

namespace Bot.Pipeline.Interfaces {
  public interface IClient {
    void Connect();
    void Send(string output);
    DateTime LatestReceivedAt { get; }
  }
}
