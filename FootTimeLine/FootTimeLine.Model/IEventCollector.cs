using System.Collections.Generic;
using FootTimeLine.Model.Events;

namespace FootTimeLine.Model
{
    public interface IEventCollector
    {
        List<MatchEvent> CollectEvent(GameId gameId);
    }
}