using System;
using FootTimeLine.Model;
using FootTimeLine.Model.Events;
using FootTimeLine.TweetConnector;
using NUnit.Framework;

namespace FootTimeLine.EventCollectorTest
{
    public class TweetConnectorTest
    {
        [Test]
        public void LoadTweetHashTag()
        {
            var sut = new TweetinviConnector();
            var gameId = new GameId("Lyon", "Toulouse", "Ligue 1");
            var footballGame = new FootballGameWithEvent(gameId);
            var tweets = sut.GetMostPopularTweets(footballGame, "OLTFC");

            Assert.That(tweets.Count, Is.GreaterThan(3));
        }
    }

    public class FootballGameWithEvent : FootballGame
    {
        public FootballGameWithEvent(GameId gameId) : base(gameId)
        {
            Events.Add(new MatchBegin(DateTime.Now));
        }

    }
}