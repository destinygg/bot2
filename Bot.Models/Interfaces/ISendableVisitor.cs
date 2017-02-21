namespace Bot.Models.Interfaces {
  public interface ISendableVisitor<out TResult> {
    TResult Visit<TTransmission>(ISendable<TTransmission> sendable)
      where TTransmission : ITransmittable;
  }
}
