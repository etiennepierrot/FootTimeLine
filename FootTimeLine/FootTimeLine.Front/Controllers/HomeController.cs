using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using FootTimeLine.Front.ModelBinder;
using FootTimeLine.Front.Models;
using FootTimeLine.Model;
using FootTimeLine.SportDeer;
using FootTimeLine.TweetConnector;

namespace FootTimeLine.Front.Controllers
{
    public class HomeController : Controller
    {
        private readonly Service _service;

        public HomeController()
        {
            _service = CreateService();
        }

        public ActionResult Index()
        {
            return GetTimeline(new GameModelPost
            {
                League = "Ligue 1",
                HashTag = "#OMOL",
                AwayTeam = "Lyon",
                HomeTeam = "Marseille"
            });
        }
        

        public ActionResult GetTimeline([FromJson]GameModelPost gameModelPost)
        {
            FootballGame game = _service.Collect(
                gameModelPost.HomeTeam,
                gameModelPost.AwayTeam,
                gameModelPost.League
            );

            var tweets = _service.FetchTweet(game, gameModelPost.HashTag);

            return View("Index", ConvertToDto(tweets));
        }

        private static FeedDto ConvertToDto(Dictionary<Goal, Tweet> tweets)
        {
            var events = tweets.Select(t => new EventDto
            {
                TweetHtml = t.Value.Html,
                EventDescription = t.Key.ToString()
            });

            return new FeedDto
            {
                EventDtos = events.ToList()
            };
        }

        private Service CreateService()
        {
            var conf = ConfigurationManager.AppSettings;
            var sportDeerEventCollector = new SportDeerEventCollector(conf["Deersport.RefreshToken"]);
            var tweetConnector = new TweetinviConnector();
            Service service = new Service(
                sportDeerEventCollector,
                tweetConnector);
            return service;
        }
    }
}
