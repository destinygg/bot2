using System;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IReceivedFactory {
    PublicMessageFromMod ModPublicReceivedMessage(string text);
    PublicMessageFromMod ModPublicReceivedMessage(string text, DateTime timestamp);
    PublicMessageFromCivilian PublicReceivedMessage(string text);
    PublicMessageFromCivilian PublicReceivedMessage(string text, DateTime timestamp);
    ReceivedPardon ReceivedPardon(Moderator sender, Civilian target);
    ParsedNuke ParsedNuke(IReceivedMessage<Moderator> message);
    ParsedNuke ParsedNuke(string command);
  }
}
