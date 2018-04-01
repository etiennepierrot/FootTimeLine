using System;
using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model.Events;

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
            _elements.AddRange(GetTweets(t => t.Date < Game.MatchStart, 2));
            _elements.AddRange(GetTweets(t => t.Date > Game.MatchStart && t.Date < Game.MatchStop, 4));
            _elements.AddRange(GetTweets(t => t.Date > Game.MatchStop, 2));
            return _elements.OrderBy(x => x.Elapsed);
        }

        private IEnumerable<Element> GetTweets(Func<Tweet, bool> predicate, int qty)
        {
            return _tweets.OrderByDescending(t => t.Popularity)
                .Except(_elements.Select(e => e.Tweet))
                .Where(predicate)
                .Take(qty)
                .Select(CreateElement);
        }

        private Element CreateElement(Tweet tweet)
        {
            TimeSpan timeSpan = tweet.Date.Subtract(Game.MatchStart);
            return new Element(new NoEvent(timeSpan), tweet);
        }

        private IEnumerable<Element> GetElementEvent(TweetMatcher matcher)
        {
            foreach (var matchEvent in Game.Events)
            {
                yield return new Element(matchEvent, FindMatchingTweet(matcher, matchEvent));
            }
        }

        private Tweet FindMatchingTweet(TweetMatcher matcher, MatchEvent matchEvent)
        {
            var tweets = matcher.Find(matchEvent);
            var selectedTweet = tweets
                .Except(_elements.Select(e => e.Tweet))
                .OrderByDescending(t => t.Popularity)
                .LastOrDefault();

            return selectedTweet ?? Tweet.Null;
        }
    }
}