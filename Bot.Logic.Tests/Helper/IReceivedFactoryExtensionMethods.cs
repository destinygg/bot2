using Bot.Logic.Interfaces;

namespace Bot.Logic.Tests.Helper {
  public static class IReceivedFactoryExtensionMethods {
    public static Nuke Nuke(this IReceivedFactory factory, string message) => factory.Nuke(factory.ModPublicReceivedMessage(message));
  }
}
