using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FootTimeLine.Model;
using FootTimeLine.Model.Events;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweet = FootTimeLine.Model.Tweet;

namespace FootTimeLine.TweetConnector
{
    public class TweetinviConnector : ITweetConnector
    {
        public TweetinviConnector()
        {
            ExceptionHandler.SwallowWebExceptions = false;
            var conf = ConfigurationManager.AppSettings;
            var credential = Auth.SetApplicationOnlyCredentials(conf["Twitter.ConsumerKey"], conf["Twitter.ConsumerSecret"]);
            Auth.InitializeApplicationOnlyCredentials(credential);
        }

        public void LoadTweet(FootballGame game)
        {
            var tweets = GetMostPopularTweets(game);
            tweets.ForEach( game.AddTweet);
        }

        public List<Tweet> GetMostPopularTweets(FootballGame game)
        {
            var searchParameter = new SearchTweetsParameters(game.HashTag)
            {
                SearchType = SearchResultType.Popular,
                MaximumNumberOfResults = 100,
                TweetSearchType = TweetSearchType.OriginalTweetsOnly,
                //Until = game.MatchStop.Add(TimeSpan.FromHours(3))
            };

            List<Tweet> mostPopularTweets = Search.SearchTweets(searchParameter)
                .OrderByDescending(Popularity)
                .Select(CreateTweet)
                .ToList();
            return mostPopularTweets;
        }

        public Tweet ExtractPopularTweet(string hashTag, Goal goal)
        {
            var searchParameter = new SearchTweetsParameters($"{hashTag} {goal.Scorer}")
            {
                SearchType = SearchResultType.Mixed,
                MaximumNumberOfResults = 100
            };

            ITweet mostPopularTweet = Search.SearchTweets(searchParameter)
                .OrderByDescending(Popularity)
                .FirstOrDefault();

            return CreateTweet(mostPopularTweet);
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

        public TweeinviTweet(ITweet tweet) : base(tweet.Id, tweet.CreatedBy.Name, tweet.Text, tweet.RetweetCount + tweet.FavoriteCount, tweet.CreatedAt)
        {
            _tweet = tweet;
        }

        public override string Display()
        {
            return _tweet.GenerateOEmbedTweet().HTML;
        }
    }
}