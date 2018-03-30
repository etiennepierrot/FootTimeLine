using System;
using System.Collections.Generic;
using System.Linq;

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

        public FootballGame Collect(string homeTeam, string awayTeam, string league)
        {
            FootballGame footballGame = new FootballGame(homeTeam, awayTeam, league);
            List<MatchEvent> matchEvents = _eventCollector.CollectEvent(footballGame);

            matchEvents.ForEach( e => footballGame.AddEvent(e));

            return footballGame;
        }

        public Dictionary<Goal, Tweet> FetchTweet(FootballGame footballGame, string hashtag)
        {
            return footballGame
                .GetGoals()
                .ToDictionary(g => g, g =>  _tweetConnector.ExtractPopularTweet(g.Scorer + " " + hashtag));
        }
    }
}