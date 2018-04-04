using System;
using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model.Events;

namespace FootTimeLine.Model
{
    public class Service
    {
        private readonly IEventCollector _eventCollector;
        private readonly ITweetConnector _tweetConnector;
        private readonly IFootballGameRepository _gameRepository;
        private readonly ITweetRepository _tweetRepository;

        public Service(IEventCollector eventCollector, ITweetConnector tweetConnector, IFootballGameRepository gameRepository, ITweetRepository tweetRepository)
        {
            _eventCollector = eventCollector;
            _tweetConnector = tweetConnector;
            _gameRepository = gameRepository;
            _tweetRepository = tweetRepository;
        }

        public FootballGame Create(GameId gameId)
        {
            var events = _eventCollector.CollectEvent(gameId);
            FootballGame game = new FootballGame(gameId);
            events.ForEach(game.AddEvent);
            _gameRepository.Save(game);
            return game;
        }

        public TimeLine BuildTimeLine(GameId gameId, string hashTag)
        {
            var footballGame = GetFootballGame(gameId);
            var tweets = GetTweets(gameId, hashTag, footballGame);
            
            return new TimeLine(footballGame, tweets);
        }

        private FootballGame GetFootballGame(GameId gameId)
        {
            FootballGame footballGame = _gameRepository.Find(gameId);
            if (footballGame == FootballGame.Null)
            {
                footballGame = Create(gameId);
            }
            return footballGame;
        }

        private List<Tweet> GetTweets(GameId gameId, string hashTag, FootballGame footballGame)
        {
            if (_tweetRepository.IsTweetOfGameCollected(gameId))
                return _tweetRepository.GetGameTweets(gameId);

            var tweets = _tweetConnector.CollectTweets(footballGame, hashTag).ToList();
            tweets.ForEach(t => _tweetRepository.AddEvent(gameId, t));
            return tweets;
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