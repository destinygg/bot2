namespace Bot.Pipeline.Interfaces {

  public interface ICommandHandler {
    void Handle();
  }

  public interface ICommandHandler<in TCommand> {
    void Handle(TCommand command);
  }

}
