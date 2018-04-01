using System;

namespace FootTimeLine.Model
{
    public class MatchBegin : MatchEvent
    {
        public MatchBegin(DateTime gameStartedAt)
        {
            Date = gameStartedAt;
            When = TimeSpan.Zero;
        }

        public DateTime Date { get; }
        public override TimeSpan When { get; }
    }
}