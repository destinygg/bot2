namespace Bot.Pipeline.Interfaces {
  public interface IClient {
    void Connect();
    void Receive(string input);
    void Send(string output);
  }
}
