using System;
using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model.Events;

namespace FootTimeLine.Model
{
    public class FootballGame
    {
        public List<MatchEvent> Events { get; }
        public List<Tweet> Tweets { get; }
        public string HomeTeam { get; }
        public string AwayTeam { get; }
        public string League { get; }
        public string HashTag { get; }
        public DateTime MatchStart => ((MatchBegin) Events.Single(e => e is MatchBegin)).Date;
        public DateTime MatchStop => MatchStart.Add(Events.OrderBy(e => e.When.Minutes).Last().When);


        public FootballGame(string homeTeam, string awayTeam, string league, string hashTag)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            League = league;
            HashTag = hashTag;
            Events = new List<MatchEvent>();
            Tweets = new List<Tweet>();
        }

        public void AddTweet(Tweet tweet)
        {
            Tweets.Add(tweet);
        }

        public void AddEvent(MatchEvent @event)
        {
            Events.Add(@event);
        }

        public List<Goal> GetGoals()
        {
            return Events
                .Where(e => e is Goal)
                .Cast<Goal>().ToList();
        }

        public List<Player> GetScorers()
        {
            return Events
                .Where(e => e is Goal)
                .Cast<Goal>().Select(x => x.Scorer).ToList();
        }
    }
}