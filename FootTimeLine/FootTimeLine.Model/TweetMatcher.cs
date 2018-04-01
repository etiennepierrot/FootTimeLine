using System;
using System.Collections.Generic;
using System.Linq;

namespace FootTimeLine.Model
{
    public class TweetMatcher
    {
        private readonly IEnumerable<Tweet> _tweets;
        private readonly FootballGame _footballGame;

        public TweetMatcher(IEnumerable<Tweet> tweets, FootballGame footballGame)
        {
            _tweets = tweets;
            _footballGame = footballGame;
        }

        public Tweet Find(MatchEvent @event)
        {
            var range = TimeSpan.FromMinutes(5);
            var startRange = _footballGame.MatchStart.Add(@event.When);
            var endRange = startRange.Add(range);
            var tweet = _tweets
                .Where(t => t.Date < endRange && t.Date > startRange)
                .OrderBy(x => x.Popularity)
                .LastOrDefault();

            return tweet ?? Tweet.Null;

        }
    }
}