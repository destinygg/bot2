using System;
using Bot.Models.Interfaces;

namespace Bot.Pipeline.Interfaces {
  public interface IPipeline {
    void Enqueue(IReceived<IUser, ITransmittable> received);
    void Enqueue(string received);
    void SetSender(Action<string> sender);
  }
}
