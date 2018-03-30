using System;
using System.Collections.Specialized;
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
        private readonly NameValueCollection _conf = ConfigurationManager.AppSettings;

        [SetUp]
        public void Setup()
        {
            string refreshToken = _conf["Deersport.RefreshToken"];
            SportDeerEventCollector collector = new SportDeerEventCollector(refreshToken);
            Service service = new Service(collector, new TweetConnector.TweetConnector(CreateTwitterContext().Result));
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

       

        private async Task<TwitterContext> CreateTwitterContext()
        {
            var auth = new ApplicationOnlyAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = _conf["Twitter.ConsumerKey"],
                    ConsumerSecret = _conf["Twitter.ConsumerSecret"]
                }
            };

            await auth.AuthorizeAsync();
            return new TwitterContext(auth);
        }
    }
}