using System.Collections.Generic;

namespace FootTimeLine.Model
{
    public interface IEventCollector
    {
        List<MatchEvent> CollectEvent(FootballGame footballGame);
    }
}