using System.Collections.Generic;

namespace FootTimeLine.Model
{
    public interface ITweetConnector
    {
        List<Tweet> CollectTweets(FootballGame game, string hashtag);
    }
}