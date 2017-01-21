namespace Bot.Pipeline.Contracts {
  public interface IPipeline {
    void Run(ISampleReceived sampleReceived);
  }
}