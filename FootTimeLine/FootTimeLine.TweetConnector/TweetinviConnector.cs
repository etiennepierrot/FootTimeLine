using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FootTimeLine.Model;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweet = FootTimeLine.Model.Tweet;

namespace FootTimeLine.TweetConnector
{
    public class TweetinviConnector : ITweetConnector
    {
        public Tweet ExtractPopularTweet(string query)
        {
            ExceptionHandler.SwallowWebExceptions = false;
            var conf = ConfigurationManager.AppSettings;
            var credential = Auth.SetApplicationOnlyCredentials(conf["Twitter.ConsumerKey"], conf["Twitter.ConsumerSecret"]);
            Auth.InitializeApplicationOnlyCredentials(credential);

            var searchParameter = new SearchTweetsParameters(query)
            {
                SearchType = SearchResultType.Mixed,
                MaximumNumberOfResults = 100,
            };

            IEnumerable<ITweet> tweets = Search.SearchTweets(searchParameter);

            ITweet mostPopularTweet = tweets
                .OrderByDescending(x => x.RetweetCount  + x.FavoriteCount)
                .FirstOrDefault();
            return mostPopularTweet == null 
                ? Tweet.Null 
                : new Tweet(mostPopularTweet.CreatedBy.Name, mostPopularTweet.Text,  mostPopularTweet.FavoriteCount + mostPopularTweet.RetweetCount);
        }
    }
}