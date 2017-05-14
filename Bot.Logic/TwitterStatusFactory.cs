using Bot.Tools.Interfaces;
using CoreTweet;
using CoreTweet.Streaming;

namespace Bot.Logic {
  public class TwitterStatusFactory : IFactory<StreamingMessage, Status> {

    public Status Create(StreamingMessage streamingMessage) {
      var statusMessage = (StatusMessage) streamingMessage;
      return statusMessage.Status;
    }

  }
}
