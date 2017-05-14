using System;
using System.Collections.Generic;
using Bot.Models.Sendable;
using CoreTweet.Streaming;

namespace Bot.Logic.Interfaces {
  public interface ITwitterStreamingMessageObserver : IObserver<StreamingMessage> {
    void SetReconnect(Action reconnect);
    void SetSend(Action<IReadOnlyList<SendablePublicMessage>> send);
  }
}
