using System;

namespace FootTimeLine.Model
{
    public abstract class MatchEvent
    {
        public  abstract  TimeSpan When { get; }
    }
}