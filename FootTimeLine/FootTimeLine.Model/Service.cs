using System;
using System.Collections.Generic;

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

        public void FetchTweet(FootballGame footballGame, string hashtag)
        {
            foreach (var goal in footballGame.GetGoals())
            {
                var tweet = _tweetConnector.ExtractPopularTweet(goal.Scorer + " " + hashtag);
                Console.WriteLine(goal);
                Console.WriteLine($"{tweet.Text}");
            }
        }
    }
}