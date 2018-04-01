using System;

namespace FootTimeLine.Model
{
    public class GenericEvent : MatchEvent
    {
        public GenericEvent(TimeSpan @when)
        {
            When = when;
        }

        public override TimeSpan When { get; }
    }
}