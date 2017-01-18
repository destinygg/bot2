namespace Bot.Models.Contracts {
  public static class IReceivedExtensionMethods {
    public static bool FromMod(this IReceived received) => received.Sender.IsMod;
  }
}
