using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model;

namespace FootTimeLine.SQLPersistence
{
    public class TweetRepository : Repository, ITweetRepository
    {
        public bool IsTweetOfGameCollected(GameId gameId)
        {
            using (var context = new TimeLineContext())
            {
                return context.Tweets.Count(t => t.HomeTeam == gameId.HomeTeam &&
                                               t.AwayTeam == gameId.AwayTeam && 
                                               t.League == gameId.League) > 0;
            }
        }

        public List<Tweet> GetGameTweets(GameId gameId)
        {
            using (var context = new TimeLineContext())
            {
                return context.Tweets
                    .Where(t => t.HomeTeam == gameId.HomeTeam &&
                                t.AwayTeam == gameId.AwayTeam && 
                                t.League == gameId.League)
                    .ToList()
                    .Select(t => new ProxyTweet(t))
                    .Cast<Tweet>()
                    .ToList();
            }
        }

        class ProxyTweet : Tweet
        {
            public ProxyTweet(TweetEntity entity)
            {
                CreatorId = entity.CreatorId;
                Date = entity.Date;
                FavCount = entity.FavCount;
                Language = entity.Language;
                RTCount = entity.RTCount;
                Text = entity.Text;
                Video = entity.Video;
                Id = entity.TweetId;
            }
        }

        public void AddEvent(GameId gameId, Tweet tweet)
        {
            UnitOfWork(c =>
            {
                TweetEntity tweetEntity = new TweetEntity
                {
                    Date = tweet.Date,
                    CreatorId = tweet.CreatorId,
                    FavCount = tweet.FavCount,
                    HomeTeam = gameId.HomeTeam,
                    AwayTeam = gameId.AwayTeam,
                    League = gameId.League,
                    Language = tweet.Language,
                    RTCount = tweet.RTCount,
                    Text = tweet.Text,
                    TweetId = tweet.Id,
                    Video = tweet.Video
                };

                c.Tweets.Add(tweetEntity);
                c.SaveChanges();
            });
        }
    }
}