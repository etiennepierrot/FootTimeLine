using System;

namespace FootTimeLine.Model.Events
{
    public class GenericEvent : MatchEvent
    {
        public GenericEvent(TimeSpan @when)
        {
            When = when;
        }
        public override TimeSpan When { get; }

        public override string ToString()
        {
            return "This events has not been parsed";
        }
    }
}