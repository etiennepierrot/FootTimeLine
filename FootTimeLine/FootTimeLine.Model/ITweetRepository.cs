using System.Collections.Generic;

namespace FootTimeLine.Model
{
    public interface ITweetRepository
    {
        bool IsTweetOfGameCollected(GameId gameId);
        List<Tweet> GetGameTweets(GameId gameId);
        void AddEvent(GameId gameId, Tweet tweet);
    }
}