using Bot.Logic.Interfaces;

namespace Bot.Logic.Tests.Helper {
  public static class IReceivedFactoryExtensionMethods {
    public static ParsedNuke ParsedNuke(this IReceivedFactory factory, string message) => factory.ParsedNuke(factory.ModPublicReceivedMessage(message));
  }
}
