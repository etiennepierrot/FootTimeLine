using System;
using System.Linq;
using System.Threading.Tasks;
using FootTimeLine.Model;
using LinqToTwitter;

namespace FootTimeLine.EventCollectorTest
{
    public class TweetConnector : ITweetConnector
    {
        private readonly TwitterContext _context;

        public TweetConnector(TwitterContext context)
        {
            _context = context;
        }

        public Tweet ExtractPopularTweet(string query)
        {
            Status tweet =
                (from search in _context.Search
                    where search.Type == SearchType.Search &&
                          search.Query == query &&
                          search.ResultType == ResultType.Mixed
                    select search)
                .SingleOrDefault()
                ?.Statuses.First();          ;

            return new Tweet(tweet.User.ScreenNameResponse, tweet.Text, tweet.RetweetCount);
        }
    }
}