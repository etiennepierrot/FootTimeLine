using System.Configuration;
using System.Linq;
using System.Reflection;
using FootTimeLine.Model;
using FootTimeLine.Model.Events;
using FootTimeLine.SportDeer;
using FootTimeLine.SQLPersistence;
using FootTimeLine.TweetConnector;
using NUnit.Framework;

namespace FootTimeLine.EventCollectorTest
{
    public class SqlPersistenceTest
    {
        [Test]
        public void PersistGame()
        {
            var gameId = new GameId("Lyon", "Toulouse", "Ligue 1");
            var footballGame = new FootballGameWithEvent(gameId);
            string refreshToken = ConfigurationManager.AppSettings["Deersport.RefreshToken"];

            var service = new Service(new SportDeerEventCollector(refreshToken), new TweetinviConnector(), new FootballGameRepository(), new TweetRepository());
            var sut = new FootballGameRepository();
            service.Create(new GameId("Lyon", "Toulouse", "Ligue 1"));

            sut.Save(footballGame);

            FootballGame game = sut.Find(new GameId("Lyon", "Toulouse", "Ligue 1"));

        }
    }
}