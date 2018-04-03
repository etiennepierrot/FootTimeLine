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
        private ITwitterCredentials _twitterCredentials;

        public TweetinviConnector()
        {
            ExceptionHandler.SwallowWebExceptions = false;
            var conf = ConfigurationManager.AppSettings;
            _twitterCredentials = Auth.SetApplicationOnlyCredentials(conf["Twitter.ConsumerKey"], conf["Twitter.ConsumerSecret"]);
            Auth.InitializeApplicationOnlyCredentials(_twitterCredentials);
        }

        public List<Tweet> GetMostPopularTweets(FootballGame game, string hashtag)
        {
            TimeSpan oneHour = TimeSpan.FromHours(1);

            return game.BuildQueriesGame(hashtag)
                .Select(SearchTweet)
                .SelectMany(x => x)
                .Distinct()
                .Where(x =>
                    x.Date > game.MatchStart.Subtract(oneHour)
                    && x.Date < game.MatchStop.Add(oneHour))
                .ToList();

        }

        private List<Tweet> SearchTweet(string query)
        {
            var searchParameter = new SearchTweetsParameters(query)
            {
                SearchType = SearchResultType.Mixed,
                MaximumNumberOfResults = 100,
                TweetSearchType = TweetSearchType.OriginalTweetsOnly
            };

            List<Tweet> tweets = Search.SearchTweets(searchParameter)
                .OrderByDescending(Popularity)
                .Select(CreateTweet)
                .ToList();
            return tweets;
        }


        private Tweet CreateTweet(ITweet tweet)
        {
            return tweet == null ? Tweet.Null : new TweeinviTweet(tweet);
        }

        private int Popularity(ITweet tweet)
        {
            return tweet.RetweetCount + tweet.FavoriteCount;
        }
    }

    class TweeinviTweet : Tweet
    {
        private readonly ITweet _tweet;

        public TweeinviTweet(ITweet tweet) : base(tweet.Id, tweet.Text, tweet.RetweetCount + tweet.FavoriteCount, tweet.CreatedAt)
        {
            _tweet = tweet;
        }

        public override string Display()
        {
            return _tweet.GenerateOEmbedTweet().HTML;
        }
    }
}