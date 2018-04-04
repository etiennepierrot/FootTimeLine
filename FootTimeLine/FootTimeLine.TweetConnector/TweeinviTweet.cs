using System.Linq;
using FootTimeLine.Model;
using Tweetinvi.Models;

namespace FootTimeLine.TweetConnector
{
    class TweeinviTweet : Tweet
    {
        public TweeinviTweet(ITweet tweet)
        {
            Id = tweet.Id;
            Text = tweet.Text;
            RTCount = tweet.RetweetCount;
            FavCount = tweet.FavoriteCount;
            Date = tweet.CreatedAt;
            CreatorId = tweet.CreatedBy.Id;
            Language = tweet.Language.ToString();
            Video = tweet.Media.Any(x => x.MediaType == "video");
        }
    }
}