using System;

namespace FootTimeLine.Model.Events
{
    public class NoEvent : MatchEvent
    {
        public NoEvent(TimeSpan @when)
        {
            When = when;
        }
        public override TimeSpan When { get; }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}