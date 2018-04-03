using System.Collections.Generic;

namespace FootTimeLine.Model
{
    public interface ITweetConnector
    {
        List<Tweet> GetMostPopularTweets(FootballGame game, string hashtag);
    }
}