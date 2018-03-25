using System.Configuration;
using FootTimeLine.Model;
using FootTimeLine.SportDeer;
using NUnit.Framework;

namespace FootTimeLine.EventCollectorTest
{
    public class SportdeerEventCollectorTest
    {
        private FootballGame _footballGame;
        private readonly Player _mitroglou = new Player("Konstantinos Katsouranis");

        [SetUp]
        public void Setup()
        {
            string refreshToken = ConfigurationManager.AppSettings["Deersport.RefreshToken"];
            SportDeerEventCollector collector = new SportDeerEventCollector(refreshToken);
            Service service = new Service(collector);
            _footballGame = service.Collect("Marseille", "Lyon", "Ligue 1");
        }

        [Test]
        public void Olympico_Has_5_Scorer()
        {
            Assert.That(_footballGame.GetScorers(), Has.Count.EqualTo(5));
        }

        [Test]
        public void Mitroglou_Has_Score()
        {
            Assert.That(_footballGame.GetScorers(), Has.Member(_mitroglou));
        }
    }
}