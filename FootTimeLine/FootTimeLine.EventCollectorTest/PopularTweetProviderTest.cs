using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using FootTimeLine.Model;
using FootTimeLine.SportDeer;
using LinqToTwitter;
using NUnit.Framework;

namespace FootTimeLine.EventCollectorTest
{
    public class PopularTweetProviderTest
    {
        private FootballGame _footballGame;

        [SetUp]
        public void Setup()
        {
            string refreshToken = ConfigurationManager.AppSettings["Deersport.RefreshToken"];
            SportDeerEventCollector collector = new SportDeerEventCollector(refreshToken);
            Service service = new Service(collector, new TweetConnector(CreateTwitterContext().Result));
            _footballGame = service.Collect("Marseille", "Lyon", "Ligue 1");
            service.FetchTweet(_footballGame, "#OMOL");
            //_footballGame.GetGoals().ForEach(Console.WriteLine);
        }

        [Test]
        public void METHOD()
        {
            //var context = CreateTwitterContext();
            //var connector = new TweetConnector(context.Result);
            //connector.ExtractPopularTweet("#OMOL Mitroglou");
            //Assert.That(true, Is.True);
        }

       

        private static async Task<TwitterContext> CreateTwitterContext()
        {
            var auth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore()
                {
                    ConsumerKey = "MBLi0jie7W0iSA8ChDF0waCmx",
                    ConsumerSecret = "iA56ljZ8UFwqt5GXGWHx0qNRXM8AxS2fFxS034URsMuCWL4Hzo"
                }
            };

            await auth.AuthorizeAsync();
            return new TwitterContext(auth);
        }
    }
}