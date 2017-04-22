namespace Bot.Tools.Interfaces {
  public interface IPrivateConstants {
    string TestAccountWebsocketAuth { get; }
    string BotWebsocketAuth { get; }
    string ImgurAuthHeader { get; }
    string TwitterAccessToken { get; }
    string TwitterAccessTokenSecret { get; }
    string TwitterConsumerKey { get; }
    string TwitterConsumerSecret { get; }
    string LastFmApiKey { get; }
    string Youtube { get; }
    string TwitchNick { get; }
    string TwitchOauth { get; }
    string IrcFloodBypassPassword { get; }
    string BattleNet { get; }
    string GoogleClientId { get; }
    string GoogleClientSecret { get; }
    string GoogleKey { get; }
    string TwitchClientId { get; }
  }
}
