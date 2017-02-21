using System;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Received;

namespace Bot.Logic.Interfaces {
  public interface IReceivedFactory {
    PublicMessageFromMod ModPublicReceivedMessage(string text);
    PublicMessageFromMod ModPublicReceivedMessage(string text, DateTime timestamp);
    PublicMessageFromCivilian PublicReceivedMessage(string text);
    PublicMessageFromCivilian PublicReceivedMessage(string text, DateTime timestamp);
    ReceivedPardon ReceivedPardon(Moderator sender, Civilian target);
    ParsedNuke ParsedNuke(IReceived<Moderator, IMessage> message);
    ParsedNuke ParsedNuke(string command);
  }
}
