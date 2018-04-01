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
    public class TimelineBuilderTest
    {
        private readonly NameValueCollection _conf = ConfigurationManager.AppSettings;
        private Service _service;

        [SetUp]
        public void Setup()
        {
            string refreshToken = _conf["Deersport.RefreshToken"];
            SportDeerEventCollector collector = new SportDeerEventCollector(refreshToken);
            _service = new Service(collector, new TweetinviConnector());
        }

        [Test]
        public void CreateTimeline()
        {
            TimeLine timeLine = _service.BuildTimeLine(new FootballGame("Dijon", "Marseille", "Ligue 1", "#DFCOOM"));
            Assert.That(timeLine.GetElements(), Has.Count.AtLeast(5));
        } 
    }

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
            _footballGame = service.Collect(new FootballGame("Marseille", "Lyon", "Ligue 1", "#OMOL"));
            var tweetDictionnary = service.FetchTweet(_footballGame);

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