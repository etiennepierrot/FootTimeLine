using System;
using FootTimeLine.Model.Events;

namespace FootTimeLine.Model
{
    public class Service
    {
        private readonly IEventCollector _eventCollector;
        private readonly ITweetConnector _tweetConnector;
        private readonly IFootballGameRepository _repository;

        public Service(IEventCollector eventCollector, ITweetConnector tweetConnector, IFootballGameRepository repository)
        {
            _eventCollector = eventCollector;
            _tweetConnector = tweetConnector;
            _repository = repository;
        }

        public FootballGame Create(GameId gameId)
        {
            var events = _eventCollector.CollectEvent(gameId);
            FootballGame game = new FootballGame(gameId);
            events.ForEach(game.AddEvent);
            _repository.Save(game);
            return game;
        }

        public TimeLine BuildTimeLine(GameId gameId, string hashTag)
        {
            FootballGame footballGame = _repository.Find(gameId);
            if (footballGame == FootballGame.Null)
            {
                footballGame = Create(gameId);
            }
            
            var tweets = _tweetConnector.GetMostPopularTweets(footballGame, hashTag);
            return new TimeLine(footballGame, tweets);
        }
    }

    public class Element
    {
        public Element(MatchEvent @event, Tweet tweet)
        {
            Event = @event;
            Tweet = tweet;
        }

        public MatchEvent Event { get; }
        public TimeSpan Elapsed => Event.When;
        public Tweet Tweet { get; }
    }
}