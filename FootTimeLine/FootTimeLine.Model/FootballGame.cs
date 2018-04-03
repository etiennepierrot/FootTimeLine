using System;
using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model.Events;

namespace FootTimeLine.Model
{
    public class FootballGame
    {
        public GameId GameId => new GameId(HomeTeam, AwayTeam, League); 
        public List<MatchEvent> Events { get; }
        public List<Tweet> Tweets { get; }
        public string HomeTeam { get; }
        public string AwayTeam { get; }
        public string League { get; }
        public DateTime MatchStart => ((MatchBegin) Events.Single(e => e is MatchBegin)).Date;
        public DateTime MatchStop => MatchStart.Add(Events.OrderBy(e => e.When.Minutes).Last().When);
        public static FootballGame Null = new FootballGame(new GameId(null, null, null));


        public FootballGame(GameId gameId)
        {
            HomeTeam = gameId.HomeTeam;
            AwayTeam = gameId.AwayTeam;
            League = gameId.League;
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

        public string[] BuildQueriesGame(string hashtag)
        {
            var eventQueries = CreateEventQueries().ToList();
            eventQueries.Add(hashtag);
            return eventQueries.Distinct()
                .ToArray();
        }

        private IEnumerable<string> CreateEventQueries()
        {
            foreach (var matchEvent in Events)
            {
                switch (matchEvent)
                {
                    case Goal g:
                        yield return $"but {g.Scorer}";
                        break;
                    case Substitution s:
                        yield return $"remplacement {s.PlayerIn} {s.PlayerOut}";
                        break;
                    case RedCard r:
                        yield return $"carton rouge {r.Player}";
                        break;
                    case YellowCard y:
                        yield return $"carton jaune {y.Player}";
                        break;
                }
            }

        }
    }
}