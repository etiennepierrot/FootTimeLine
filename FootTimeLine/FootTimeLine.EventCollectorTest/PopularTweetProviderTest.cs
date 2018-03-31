using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using FootTimeLine.Model;
using FootTimeLine.SportDeer;
using FootTimeLine.TweetConnector;
using NUnit.Framework;
using Tweetinvi.Core.Extensions;

namespace FootTimeLine.EventCollectorTest
{
    public class PopularTweetProviderTest
    {
        private FootballGame _footballGame;
        private readonly NameValueCollection _conf = ConfigurationManager.AppSettings;

        [SetUp]
        public void Setup()
        {
            string refreshToken = _conf["Deersport.RefreshToken"];
            SportDeerEventCollector collector = new SportDeerEventCollector(refreshToken);
            Service service = new Service(collector, new TweetinviConnector());
            _footballGame = service.Collect("Marseille", "Lyon", "Ligue 1");
            var tweetDictionnary = service.FetchTweet(_footballGame, "#OMOL");

            tweetDictionnary.Values.ForEach(Console.WriteLine);
        }

        [Test]
        public void METHOD()
        {
            //var context = CreateTwitterContext();
            //var connector = new TweetConnector(context.Result);
            //connector.ExtractPopularTweet("#OMOL Mitroglou");
            //Assert.That(true, Is.True);
        }
    }
}