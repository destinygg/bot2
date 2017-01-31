namespace Bot.Models.Interfaces {
  public static class IReceivedExtensionMethods {
    public static bool IsFromMod(this IReceived<IUser, ITransmittable> received) => received.Sender.IsMod;
  }
}
