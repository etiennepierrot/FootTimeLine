using System.Collections.Generic;
using FootTimeLine.Model.Events;

namespace FootTimeLine.Model
{
    public interface ITweetConnector
    {
        Tweet ExtractPopularTweet(string hashtag, Goal goal);
        List<Tweet> GetMostPopularTweets(FootballGame game);
    }
}