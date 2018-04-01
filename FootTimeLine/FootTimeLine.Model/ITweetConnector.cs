using System.Collections.Generic;

namespace FootTimeLine.Model
{
    public interface ITweetConnector
    {
        Tweet ExtractPopularTweet(string hashtag, Goal goal);
        List<Tweet> GetMostPopularTweets(FootballGame game);
    }
}