using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using FootTimeLine.Front.ModelBinder;
using FootTimeLine.Front.Models;
using FootTimeLine.Model;
using FootTimeLine.SportDeer;
using FootTimeLine.SQLPersistence;
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
                HashTag = "#OLTFC",
                AwayTeam = "Toulouse",
                HomeTeam = "Lyon"
            });
        }

        public ActionResult GetTimeline([FromJson]GameModelPost gameModelPost)
        {
            string homeTeam = gameModelPost.HomeTeam;
            string awayTeam = gameModelPost.AwayTeam;
            string league = gameModelPost.League;
            string hashTag = gameModelPost.HashTag;

            var gameId = new GameId(homeTeam, awayTeam, league);
            var timeLine = _service.BuildTimeLine(gameId, hashTag);

            return View("Index", ConvertToDto(timeLine, gameModelPost));
        }

        private FeedDto ConvertToDto(TimeLine timeLine, GameModelPost gameDto)
        {
            return new FeedDto
            {
                GameDto = gameDto,
                EventDtos = timeLine.GetElements()
                    .Select(EventDto)
                    .ToList()
            };
        }

        private static EventDto EventDto(Element e)
        {
            return new EventDto
            {
                TweetId = e.Tweet.Id,
                EventDescription = e.Event.ToString()
            };
        }

        private static Service CreateService()
        {
            return new Service(
                new SportDeerEventCollector(ConfigurationManager.AppSettings["Deersport.RefreshToken"]),
                new TweetinviConnector(), 
                new FootballGameRepository(), 
                new TweetRepository());
        }
    }
}
