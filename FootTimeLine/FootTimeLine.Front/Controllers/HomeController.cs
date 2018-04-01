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
                HashTag = "#DFCOOM",
                AwayTeam = "Marseille",
                HomeTeam = "Dijon"
            });
        }
        

        public ActionResult GetTimeline([FromJson]GameModelPost gameModelPost)
        {
            string homeTeam = gameModelPost.HomeTeam;
            string awayTeam = gameModelPost.AwayTeam;
            string league = gameModelPost.League;
            string hashTag = gameModelPost.HashTag;

            FootballGame footballGame = new FootballGame(homeTeam, awayTeam, league, hashTag);
            var timeLine = _service.BuildTimeLine(footballGame);

            return View("Index", ConvertToDto(timeLine));
        }

        private FeedDto ConvertToDto(TimeLine timeLine)
        {
            return new FeedDto
            {
                EventDtos = timeLine.GetElements().Select(e => new EventDto
                {
                    TweetHtml = e.Tweet.Display(),
                    EventDescription = e.Event.ToString()
                }).ToList()
            };
        }

        private static FeedDto ConvertToDto(Dictionary<Goal, Tweet> tweets)
        {
            return new FeedDto
            {
                EventDtos = tweets.Select(t => new EventDto
                {
                    TweetHtml = t.Value.Display(),
                    EventDescription = t.Key.ToString()
                }).ToList()
            };
        }

        private static Service CreateService()
        {
            return new Service(
                new SportDeerEventCollector(ConfigurationManager.AppSettings["Deersport.RefreshToken"]),
                new TweetinviConnector());
        }
    }
}
