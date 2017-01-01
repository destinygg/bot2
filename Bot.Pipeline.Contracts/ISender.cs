namespace Bot.Pipeline.Contracts {
  public interface ISender {
    void Send(ISendableProducer sendableProducer);
  }
}
