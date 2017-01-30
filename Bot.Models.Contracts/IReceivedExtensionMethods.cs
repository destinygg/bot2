namespace Bot.Models.Contracts {
  public static class IReceivedExtensionMethods {
    public static bool IsFromMod(this IReceived<IUser, ITransmittable> received) => received.Sender.IsMod;
  }
}
