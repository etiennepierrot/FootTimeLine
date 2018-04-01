using System;

namespace FootTimeLine.Model.Events
{
    public abstract class MatchEvent
    {
        public  abstract  TimeSpan When { get; }
    }
}