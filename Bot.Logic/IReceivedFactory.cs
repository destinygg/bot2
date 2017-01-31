using System;
using Bot.Models;

namespace Bot.Logic {
  public interface IReceivedFactory {
    PublicMessageFromMod ModPublicReceivedMessage(string text);
    PublicMessageFromMod ModPublicReceivedMessage(string text, DateTime timestamp);
    PublicMessageFromCivilian PublicReceivedMessage(string text);
    PublicMessageFromCivilian PublicReceivedMessage(string text, DateTime timestamp);
    ReceivedPardon ReceivedPardon(Moderator sender, Civilian target);
    ReceivedRegexNuke ReceivedRegexNuke(IReceivedMessage<Moderator> message);
    ReceivedStringNuke ReceivedStringNuke(IReceivedMessage<Moderator> message);
    ReceivedRegexNuke ReceivedRegexNuke(string command);
    ReceivedStringNuke ReceivedStringNuke(string command);
  }
}