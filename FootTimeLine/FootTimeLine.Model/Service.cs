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

        public Service(IEventCollector eventCollector, ITweetConnector tweetConnector)
        {
            _eventCollector = eventCollector;
            _tweetConnector = tweetConnector;
        }

        public FootballGame Collect(FootballGame game)
        {
            _eventCollector.CollectEvent(game);

            return game;
        }

        public TimeLine BuildTimeLine(FootballGame game)
        {
            _eventCollector.CollectEvent(game);
            var tweets = _tweetConnector.GetMostPopularTweets(game);
            return new TimeLine(game, tweets);
        }

        public Dictionary<Goal, Tweet> FetchTweet(FootballGame footballGame)
        {
            return footballGame
                .GetGoals()
                .ToDictionary(g => g, g => _tweetConnector.ExtractPopularTweet(footballGame.HashTag, g));
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