namespace Bot.Models.Contracts {
  public static class IReceivedExtensionMethods {
    public static bool IsFromMod(this IReceived<IUser> received) => received.Sender.IsMod;
  }
}
