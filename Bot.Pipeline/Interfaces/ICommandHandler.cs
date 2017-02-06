namespace Bot.Pipeline.Interfaces {
  public interface ICommandHandler<in TCommand> {
    void Handle(TCommand command);
  }
}
