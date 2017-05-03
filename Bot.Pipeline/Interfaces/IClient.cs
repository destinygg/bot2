namespace Bot.Pipeline.Interfaces {
  public interface IClient {
    void Receive(string input);
    void Send(string output);
  }
}
