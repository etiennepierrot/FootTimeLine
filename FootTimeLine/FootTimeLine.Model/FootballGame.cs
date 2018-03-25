using System.Collections.Generic;
using System.Linq;

namespace FootTimeLine.Model
{
    public class FootballGame
    {
        private readonly List<MatchEvent> _events;

        public string HomeTeam { get; }
        public string AwayTeam { get; }
        public string League { get; }

        public FootballGame(string homeTeam, string awayTeam, string league)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            League = league;
            _events = new List<MatchEvent>();
        }

        public void AddEvent(MatchEvent @event)
        {
            _events.Add(@event);
        }

        public List<Goal> GetGoals()
        {
            return _events
                .Where(e => e is Goal)
                .Cast<Goal>().ToList();
        }

        public List<Player> GetScorers()
        {
            return _events
                .Where(e => e is Goal)
                .Cast<Goal>().Select(x => x.Scorer).ToList();
        }

    }
}