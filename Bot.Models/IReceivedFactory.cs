using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public interface IReceivedFactory {
    ModPublicReceivedMessage ModPublicReceivedMessage(string text);
    ModPublicReceivedMessage ModPublicReceivedMessage(string text, DateTime timestamp);
    PublicReceivedMessage PublicReceivedMessage(string text);
    PublicReceivedMessage PublicReceivedMessage(string text, DateTime timestamp);
    ReceivedUnMuteBan ReceivedUnMuteBan(IUser sender, IUser target);
    ReceivedRegexNuke ReceivedRegexNuke(ReceivedMessage message);
    ReceivedStringNuke ReceivedStringNuke(ReceivedMessage message);
    ReceivedRegexNuke ReceivedRegexNuke(string command);
    ReceivedStringNuke ReceivedStringNuke(string command);
  }
}