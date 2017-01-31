namespace Bot.Pipeline.Interfaces {
  public interface IPipeline {
    void Run(ISampleReceived sampleReceived);
  }
}