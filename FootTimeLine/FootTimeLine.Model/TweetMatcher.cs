using System;
using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model.Events;

namespace FootTimeLine.Model
{
    public class TweetMatcher
    {
        private readonly List<Tweet> _tweets;
        private readonly FootballGame _footballGame;
        private readonly TimeSpan _delta = TimeSpan.FromSeconds(20);

        public TweetMatcher(IEnumerable<Tweet> tweets, FootballGame footballGame)
        {
            _tweets = tweets.ToList();
            _footballGame = footballGame;
        }

        public IEnumerable<Tweet> Find(MatchEvent @event)
        {
            var range = TimeSpan.FromMinutes(5);
            var startRange = _footballGame.MatchStart
                .Add(@event.When)
                .Add(_delta);

            var endRange = startRange.Add(range);
            return _tweets
                .Where(t => t.Date < endRange && t.Date > startRange);
        }
    }
}