using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using FootTimeLine.Model;
using FootTimeLine.Model.Events;
using Tweetinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.WebLogic;
using HttpMethod = Tweetinvi.Models.HttpMethod;
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

        public void LoadTweet(FootballGame game)
        {
            var tweets = GetMostPopularTweets(game);
            tweets.ForEach( game.AddTweet);
        }

        public List<Tweet> GetMostPopularTweets(FootballGame game)
        {
            TimeSpan oneHour = TimeSpan.FromHours(1);

            return game.BuildQueriesGame()
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

        public List<Tweet> GetPopularTweetsPremium(FootballGame game)
        {
            var query = HttpUtility.UrlEncode($"{game.HashTag}");
            string uri = $"https://api.twitter.com/1.1/tweets/search/30day/dev.json?query={query}";
            var result = TwitterAccessor.GetQueryableJsonObjectFromGETQuery(uri);
            
            List<Tweet> tweets = new List<Tweet>();
            foreach (var t in result["results"].Children())
            {
                var date = Parse(t["created_at"].ToString());
                int rt = Int32.Parse(t["retweet_count"].ToString());
                int fav = Int32.Parse(t["favorite_count"].ToString());
                var tweet = new Tweet(long.Parse(t["id"].ToString()), t["text"].ToString(), rt + fav, date);
                tweets.Add(tweet);
            }

            return tweets;
        }

        private DateTime Parse(string date)
        {
            return DateTime.ParseExact(date, "ddd MMM dd HH:mm:ss +ffff yyyy", new CultureInfo("en-US"));
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

    public class ResultTweet
    {
        public string Text { get; set; }
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