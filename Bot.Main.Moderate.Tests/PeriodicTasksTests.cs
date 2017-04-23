using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Pipeline.Interfaces;
using Bot.Pipeline.Tests;
using Bot.Tests;
using Bot.Tools.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimpleInjector;

namespace Bot.Main.Moderate.Tests {
  [TestClass]
  public class PeriodicTasksTests {

    [TestMethod]
    public void PeriodicTasks_Run_YieldsAlternatingMessages() {
      var downloadFactory = Substitute.For<IErrorableFactory<string, string, string, string>>();
      downloadFactory.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(youtubeData);
      var sender = new TestableSender();
      var container = new TestContainerManager(c => {
        var senderRegistration = Lifestyle.Singleton.CreateRegistration(() => sender, c);
        c.RegisterConditional(typeof(ICommandHandler<IEnumerable<ISendable<ITransmittable>>>), senderRegistration, _ => true);
        var downloaderRegistration = Lifestyle.Singleton.CreateRegistration(() => downloadFactory, c);
        c.RegisterConditional(typeof(IErrorableFactory<string, string, string, string>), downloaderRegistration, _ => true);
      }, settings => settings.PeriodicTaskInterval = TimeSpan.FromMilliseconds(100))
      .InitializeAndIsolateRepository();
      var tasks = container.GetInstance<PeriodicTasks>();

      tasks.Run();

      Task.Delay(900).Wait();
      Assert.AreEqual(3, sender.Outbox.Cast<SendablePublicMessage>().Count(x => x.Text.Contains("GreenManGaming")));
      Assert.AreEqual(3, sender.Outbox.Cast<SendablePublicMessage>().Count(x => x.Text.Contains("twitter.com")));
      Assert.AreEqual(3, sender.Outbox.Cast<SendablePublicMessage>().Count(x => x.Text.Contains("youtu.be")));
      Assert.AreEqual(9, sender.Outbox.Count);
    }

    private string youtubeData = @"
<feed xmlns:yt=""http://www.youtube.com/xml/schemas/2015"" xmlns:media=""http://search.yahoo.com/mrss/"" xmlns=""http://www.w3.org/2005/Atom"" class=""m1"">
<script/>
<link rel=""self"" href=""http://www.youtube.com/feeds/videos.xml?user=destiny""/>
<id>yt:channel:UC554eY5jNUfDq3yDOJYirOQ</id>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>Destiny</title>
<link rel=""alternate"" href=""https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ""/>
<author>...</author>
<published>2013-07-17T09:13:03+00:00</published>
<entry>
<id>yt:video:0c641pbeNK4</id>
<yt:videoId>0c641pbeNK4</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>Media Responsibility</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=0c641pbeNK4""/>
<author>...</author>
<published>2017-04-21T17:00:00+00:00</published>
<updated>2017-04-21T20:35:11+00:00</updated>
<media:group>...</media:group>
</entry>
<entry>
<id>yt:video:gH0-vfKJSxw</id>
<yt:videoId>gH0-vfKJSxw</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>We Are the One Who Blots</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=gH0-vfKJSxw""/>
<author>
<name>Destiny</name>
<uri>...</uri>
</author>
<published>2017-04-20T17:00:05+00:00</published>
<updated>2017-04-21T20:20:59+00:00</updated>
<media:group>...</media:group>
</entry>
<entry>
<id>yt:video:HVjlB6tOn9s</id>
<yt:videoId>HVjlB6tOn9s</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>Reckful Gets Deja Vu'D by Destiny ft. Ice Poseidon</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=HVjlB6tOn9s""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-19T17:00:01+00:00</published>
<updated>2017-04-21T19:53:12+00:00</updated>
<media:group>
<media:title>Reckful Gets Deja Vu'D by Destiny ft. Ice Poseidon</media:title>
<media:content url=""https://www.youtube.com/v/HVjlB6tOn9s?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i1.ytimg.com/vi/HVjlB6tOn9s/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
Destiny plays PlayerUnknown's Battlegrounds with Ice Poseidon, Reckful and RNA. Follow Destiny ►STREAM - http://www.destiny.gg/bigscreen ►TWITTER - https://www.twitter.com/OmniDestiny ►DISCORD - https://discordapp.com/invite/destiny Get the chair Destiny has, the SL5000, from http://www.destiny.gg/chair Put ""destiny"" as your checkout code to get a 10% discount! Use Destiny's affiliate link to buy stuff! http://www.amazon.com/?tag=des000-20 Edited By: ► CONTACT - garyukov@gmail.com ► TWITTER - https://twitter.com/garyukov Music: ►OUTRO: https://soundcloud.com/in-abstraction/compozish-competish
</media:description>
<media:community>
<media:starRating count=""406"" average=""4.74"" min=""1"" max=""5""/>
<media:statistics views=""15171""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:HLHAAblLGlY</id>
<yt:videoId>HLHAAblLGlY</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>""I Wanna Incest Your Wife""</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=HLHAAblLGlY""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-18T17:00:01+00:00</published>
<updated>2017-04-21T19:18:17+00:00</updated>
<media:group>
<media:title>""I Wanna Incest Your Wife""</media:title>
<media:content url=""https://www.youtube.com/v/HLHAAblLGlY?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i1.ytimg.com/vi/HLHAAblLGlY/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
Destiny talks with a person who decides to speculate on the number of incestuous relationships in the world. Link to full video: https://www.youtube.com/watch?v=HMgsQSwkqO4 Follow Destiny ►STREAM - http://www.destiny.gg/bigscreen ►TWITTER - https://www.twitter.com/OmniDestiny ►DISCORD - https://discordapp.com/invite/destiny Get the chair Destiny has, the SL5000, from http://www.destiny.gg/chair Put ""destiny"" as your checkout code to get a 10% discount! Use Destiny's affiliate link to buy stuff! http://www.amazon.com/?tag=des000-20 Edited By: ► CONTACT - garyukov@gmail.com ► TWITTER - https://twitter.com/garyukov Music: ►OUTRO: https://soundcloud.com/in-abstraction/compozish-competish
</media:description>
<media:community>
<media:starRating count=""739"" average=""4.86"" min=""1"" max=""5""/>
<media:statistics views=""18828""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:biGrDD0mkKw</id>
<yt:videoId>biGrDD0mkKw</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>
Destiny Secures a Warehouse Using Corny One Liners ft. Steel
</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=biGrDD0mkKw""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-17T17:00:03+00:00</published>
<updated>2017-04-21T18:14:16+00:00</updated>
<media:group>
<media:title>
Destiny Secures a Warehouse Using Corny One Liners ft. Steel
</media:title>
<media:content url=""https://www.youtube.com/v/biGrDD0mkKw?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i3.ytimg.com/vi/biGrDD0mkKw/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
Destiny plays PlayerUnknown's Battlegrounds with Steel. Outro music: https://soundcloud.com/in-abstraction/compozish-competish __________________________ Get the chair I have, the SL5000, from http://www.destiny.gg/chair ! Put ""destiny"" as your checkout code to get a 10% discount! Follow me on Twitter if you don't already - http://www.twitter.com/omnidestiny Watch the stream at http://www.destiny.gg/bigscreen !
</media:description>
<media:community>
<media:starRating count=""424"" average=""4.82"" min=""1"" max=""5""/>
<media:statistics views=""14664""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:H_5qChfKgwM</id>
<yt:videoId>H_5qChfKgwM</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>""Expert Analysis"" ft. Steel</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=H_5qChfKgwM""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-16T17:00:01+00:00</published>
<updated>2017-04-21T18:14:22+00:00</updated>
<media:group>
<media:title>""Expert Analysis"" ft. Steel</media:title>
<media:content url=""https://www.youtube.com/v/H_5qChfKgwM?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i1.ytimg.com/vi/H_5qChfKgwM/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
Destiny plays PlayerUnknown's Battlegrounds with Steel. Outro music: https://soundcloud.com/in-abstraction/compozish-competish __________________________ Get the chair I have, the SL5000, from http://www.destiny.gg/chair ! Put ""destiny"" as your checkout code to get a 10% discount! Follow me on Twitter if you don't already - http://www.twitter.com/omnidestiny Watch the stream at http://www.destiny.gg/bigscreen !
</media:description>
<media:community>
<media:starRating count=""519"" average=""4.73"" min=""1"" max=""5""/>
<media:statistics views=""15893""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:etN7Tzwa8xs</id>
<yt:videoId>etN7Tzwa8xs</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>
Military Grade Condoms ft. Ice Poseidon and Reckful
</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=etN7Tzwa8xs""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-15T17:04:50+00:00</published>
<updated>2017-04-21T18:14:28+00:00</updated>
<media:group>
<media:title>
Military Grade Condoms ft. Ice Poseidon and Reckful
</media:title>
<media:content url=""https://www.youtube.com/v/etN7Tzwa8xs?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i2.ytimg.com/vi/etN7Tzwa8xs/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
Destiny plays PlayerUnknown's Battlegrounds with Ice Poseidon, Reckful and RNA. Outro music: https://soundcloud.com/in-abstraction/compozish-competish __________________________ Get the chair I have, the SL5000, from http://www.destiny.gg/chair ! Put ""destiny"" as your checkout code to get a 10% discount! Follow me on Twitter if you don't already - http://www.twitter.com/omnidestiny Watch the stream at http://www.destiny.gg/bigscreen !
</media:description>
<media:community>
<media:starRating count=""566"" average=""4.84"" min=""1"" max=""5""/>
<media:statistics views=""16980""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:rH6Hw3BF-tY</id>
<yt:videoId>rH6Hw3BF-tY</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>Destiny's Munchpak Snack Review - Take 2</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=rH6Hw3BF-tY""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-09T22:49:38+00:00</published>
<updated>2017-04-21T18:14:34+00:00</updated>
<media:group>
<media:title>Destiny's Munchpak Snack Review - Take 2</media:title>
<media:content url=""https://www.youtube.com/v/rH6Hw3BF-tY?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i3.ytimg.com/vi/rH6Hw3BF-tY/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
For some reason they sent me a second box, so here I am reviewing it again. Sign up for and order munchpaks from here! - https://munchpak.com/
</media:description>
<media:community>
<media:starRating count=""737"" average=""4.48"" min=""1"" max=""5""/>
<media:statistics views=""20913""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:xIPd6Ov8In0</id>
<yt:videoId>xIPd6Ov8In0</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>Political Compass Test</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=xIPd6Ov8In0""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-08T23:00:25+00:00</published>
<updated>2017-04-21T14:54:52+00:00</updated>
<media:group>
<media:title>Political Compass Test</media:title>
<media:content url=""https://www.youtube.com/v/xIPd6Ov8In0?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i1.ytimg.com/vi/xIPd6Ov8In0/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
I feel like so many of these questions could go either way based on how you interpret them, so I end up doing two tests, one with slightly left-leaning arguments and one with slightly right-leaning arguments.
</media:description>
<media:community>
<media:starRating count=""402"" average=""4.07"" min=""1"" max=""5""/>
<media:statistics views=""22286""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:z_SqUxUeT84</id>
<yt:videoId>z_SqUxUeT84</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>Discussion with Jeff Holiday about Islam</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=z_SqUxUeT84""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-07T18:11:26+00:00</published>
<updated>2017-04-21T19:36:44+00:00</updated>
<media:group>
<media:title>Discussion with Jeff Holiday about Islam</media:title>
<media:content url=""https://www.youtube.com/v/z_SqUxUeT84?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i3.ytimg.com/vi/z_SqUxUeT84/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
Jeff Holiday's YouTube channel - https://www.youtube.com/user/RottingLepha Jeff and I got into a short bantz on Twitter about Islam, so we decided to have a chat about some of the problems and potential solutions facing the west today in regards to Islam. The conversation kind of sprawls to ""Trumpism"" in general.
</media:description>
<media:community>
<media:starRating count=""395"" average=""3.98"" min=""1"" max=""5""/>
<media:statistics views=""21457""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:0FT9ycFd9do</id>
<yt:videoId>0FT9ycFd9do</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>
A short talk with Steel_TV about ""autism"" as an insult
</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=0FT9ycFd9do""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-06T12:51:46+00:00</published>
<updated>2017-04-21T18:14:48+00:00</updated>
<media:group>
<media:title>
A short talk with Steel_TV about ""autism"" as an insult
</media:title>
<media:content url=""https://www.youtube.com/v/0FT9ycFd9do?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i1.ytimg.com/vi/0FT9ycFd9do/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
A random convo with CS:GO player Steel_TV about using certain words to insult people.
</media:description>
<media:community>
<media:starRating count=""368"" average=""4.42"" min=""1"" max=""5""/>
<media:statistics views=""19760""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:yWLrPvtPLbo</id>
<yt:videoId>yWLrPvtPLbo</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>
Talking with my buddy Irish Laddie about the rise of Trump and communism(?)
</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=yWLrPvtPLbo""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-04T22:04:30+00:00</published>
<updated>2017-04-21T07:01:17+00:00</updated>
<media:group>
<media:title>
Talking with my buddy Irish Laddie about the rise of Trump and communism(?)
</media:title>
<media:content url=""https://www.youtube.com/v/yWLrPvtPLbo?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i2.ytimg.com/vi/yWLrPvtPLbo/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
This is one of my really left leaning buddies, a full on anti-capitalist sociology major. I always appreciate my conversations with this guy as he can present his thoughts very concisely and we disagree on quite a bit.
</media:description>
<media:community>
<media:starRating count=""465"" average=""4.09"" min=""1"" max=""5""/>
<media:statistics views=""22834""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:mGQCfAkqJsU</id>
<yt:videoId>mGQCfAkqJsU</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>
A discussion about ""women's agency"" with anti-feminist(?) Kari
</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=mGQCfAkqJsU""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-04-03T17:12:24+00:00</published>
<updated>2017-04-20T13:31:57+00:00</updated>
<media:group>
<media:title>
A discussion about ""women's agency"" with anti-feminist(?) Kari
</media:title>
<media:content url=""https://www.youtube.com/v/mGQCfAkqJsU?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i2.ytimg.com/vi/mGQCfAkqJsU/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
No fancy intros/outros until I get my new YouTube dude. This is an interesting discussion because it demonstrates the effectiveness of anecdotes.
</media:description>
<media:community>
<media:starRating count=""433"" average=""4.03"" min=""1"" max=""5""/>
<media:statistics views=""22890""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:HMgsQSwkqO4</id>
<yt:videoId>HMgsQSwkqO4</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>Arguing in favor of incest and/or beastiality</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=HMgsQSwkqO4""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-03-30T17:45:19+00:00</published>
<updated>2017-04-21T02:16:09+00:00</updated>
<media:group>
<media:title>Arguing in favor of incest and/or beastiality</media:title>
<media:content url=""https://www.youtube.com/v/HMgsQSwkqO4?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i1.ytimg.com/vi/HMgsQSwkqO4/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
A caller claims I'm in favor of these acts. So for a laugh I see if I can debate in favor of them. Outro music: https://soundcloud.com/osvelit/destiny-comp-challenge-4-osvelit-ricercar-v2 ________________________________ Get the chair I have, the SL5000, from http://www.destiny.gg/chair ! Put ""destiny"" as your checkout code to get a 10% discount! Follow me on Twitter if you don't already - http://www.twitter.com/omnidestiny Watch the stream at http://www.destiny.gg/bigscreen !
</media:description>
<media:community>
<media:starRating count=""883"" average=""3.67"" min=""1"" max=""5""/>
<media:statistics views=""28425""/>
</media:community>
</media:group>
</entry>
<entry>
<id>yt:video:gX7Q1L7q8Sg</id>
<yt:videoId>gX7Q1L7q8Sg</yt:videoId>
<yt:channelId>UC554eY5jNUfDq3yDOJYirOQ</yt:channelId>
<title>Talking with Youtuber NoBullshit</title>
<link rel=""alternate"" href=""https://www.youtube.com/watch?v=gX7Q1L7q8Sg""/>
<author>
<name>Destiny</name>
<uri>
https://www.youtube.com/channel/UC554eY5jNUfDq3yDOJYirOQ
</uri>
</author>
<published>2017-03-29T17:00:04+00:00</published>
<updated>2017-04-21T18:47:18+00:00</updated>
<media:group>
<media:title>Talking with Youtuber NoBullshit</media:title>
<media:content url=""https://www.youtube.com/v/gX7Q1L7q8Sg?version=3"" type=""application/x-shockwave-flash"" width=""640"" height=""390""/>
<media:thumbnail url=""https://i4.ytimg.com/vi/gX7Q1L7q8Sg/hqdefault.jpg"" width=""480"" height=""360""/>
<media:description>
I talk to ""No Bullshit"" in response to his comments about me on regarding the JonTron debate. Find him here: https://www.youtube.com/channel/UCZNk7Jjb2t8EuBdgn4Zj1cw Outro music: https://soundcloud.com/calum-brockie/composition-challenge-1 ________________________________ Get the chair I have, the SL5000, from http://www.destiny.gg/chair ! Put ""destiny"" as your checkout code to get a 10% discount! Follow me on Twitter if you don't already - http://www.twitter.com/omnidestiny Watch the stream at http://www.destiny.gg/bigscreen !
</media:description>
<media:community>
<media:starRating count=""1057"" average=""4.02"" min=""1"" max=""5""/>
<media:statistics views=""33973""/>
</media:community>
</media:group>
</entry>
</feed>
";

  }
}
