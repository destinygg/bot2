namespace Bot.Models.Contracts {
  public static class IReceivedExtensionMethods {
    public static bool IsFromMod(this IReceived received) => received.Sender.IsMod;
  }
}
