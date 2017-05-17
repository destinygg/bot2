using System;
using Bot.Models.Json;

namespace Bot.Models {
  public class StreamState {

    public StreamState(StreamStatus streamStatus, DateTime latestStreamOnTime, DateTime latestStreamOffTime, TwitchStreamStatus.RootObject rawStatus) {
      StreamStatus = streamStatus;
      LatestStreamOnTime = latestStreamOnTime;
      LatestStreamOffTime = latestStreamOffTime;
      Viewers = rawStatus.stream?.viewers ?? 0;
      Title = rawStatus.stream?.channel.status;
      Game = rawStatus.stream?.game;
      Delay = rawStatus.stream?.delay ?? 0;
      AverageFps = rawStatus.stream?.average_fps ?? 0;
      CreatedAt = rawStatus.stream?.Parsed_created_at ?? DateTime.MaxValue;
    }

    public StreamStatus StreamStatus { get; }
    public DateTime LatestStreamOnTime { get; }
    public DateTime LatestStreamOffTime { get; }
    public string Title { get; }
    public string Game { get; }
    public int Viewers { get; }
    public int Delay { get; }
    public double AverageFps { get; }
    public DateTime CreatedAt { get; }
  }
}
