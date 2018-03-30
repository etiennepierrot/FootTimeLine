using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model;
using LinqToTwitter;

namespace FootTimeLine.TweetConnector
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
            var status = (from search in _context.Search
                    where search.Type == SearchType.Search &&
                          search.Query == query &&
                          search.ResultType == ResultType.Mixed
                    select search.Statuses)
                .Single()
                .FirstOrDefault();

            return status == null
                ? Tweet.Null 
                : new Tweet(status.User.ScreenNameResponse, status.Text, status.RetweetCount);
        }
    }
}