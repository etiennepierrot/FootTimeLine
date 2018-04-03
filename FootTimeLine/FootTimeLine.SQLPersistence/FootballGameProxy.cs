using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model;
using FootTimeLine.Model.Events;

namespace FootTimeLine.SQLPersistence
{
    public class FootballGameProxy : FootballGame
    {
        public FootballGameProxy(FootballGameEntity entity) 
            : base(new GameId(entity.HomeTeam, entity.AwayTeam, entity.League))
        {
            
        }

        public void LoadEvents(IEnumerable<MatchEvent> events)
        {
            Events.AddRange(events);
        }
    }
}