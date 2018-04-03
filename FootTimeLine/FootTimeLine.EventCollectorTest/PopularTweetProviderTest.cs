using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using FootTimeLine.Model;
using FootTimeLine.SportDeer;
using FootTimeLine.SQLPersistence;
using FootTimeLine.TweetConnector;
using NUnit.Framework;

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
            _service = new Service(collector, new TweetinviConnector(), new FootballGameRepository());
        }

        [Test]
        public void CreateTimeline()
        {
            TimeLine timeLine = _service.BuildTimeLine(new GameId("Dijon", "Marseille", "Ligue 1"), "#DFCOOM");
            Assert.That(timeLine.GetElements().Count(), Is.GreaterThanOrEqualTo(5));
        } 
    }
}