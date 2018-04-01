using System;
using System.Collections.Generic;
using System.Linq;

namespace FootTimeLine.Model
{
    public class TimeLine
    {
        private readonly IEnumerable<Tweet> _tweets;
        private readonly List<Element> _elements;

        public TimeLine(FootballGame game, IEnumerable<Tweet> tweets)
        {
            _tweets = tweets;
            Game = game;
            _elements = new List<Element>();
        }

        public FootballGame Game { get; }

        public IEnumerable<Element> GetElements()
        {
            var matcher = new TweetMatcher(_tweets, Game);
            _elements.AddRange(GetElementEvent(matcher));
            _elements.AddRange(GetTweets(t => t.Date < Game.MatchStart));
            _elements.AddRange(GetTweets(t => t.Date > Game.MatchStart && t.Date < Game.MatchStop));
            _elements.AddRange(GetTweets(t => t.Date > Game.MatchStop));
            return _elements.OrderBy(x => x.Elapsed);
        }

        private IEnumerable<Element> GetTweets(Func<Tweet, bool> predicate)
        {
            var tweetWithoutEvent = _tweets.OrderByDescending(t => t.Popularity)
                .Except(_elements.Select(e => e.Tweet))
                .Where(predicate)
                .Take(2);

            return tweetWithoutEvent.Select(CreateElement);
        }

        private Element CreateElement(Tweet tweet)
        {
            TimeSpan timeSpan = tweet.Date.Subtract(Game.MatchStart);
            return new Element(new GenericEvent(timeSpan), tweet);
        }

        private IEnumerable<Element> GetElementEvent(TweetMatcher matcher)
        {
            foreach (var matchEvent in Game.Events)
            {
                var tweet = matcher.Find(matchEvent);
                yield return new Element(matchEvent, tweet);
            }
        }
    }
}