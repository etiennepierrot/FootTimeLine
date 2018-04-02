using System;

namespace FootTimeLine.Model.Events
{
    public class MatchBegin : MatchEvent
    {
        public MatchBegin(DateTime gameStartedAt, string hashTag)
        {
            Date = gameStartedAt;
            HashTag = hashTag;
            When = TimeSpan.Zero;
        }

        public DateTime Date { get; }
        public string HashTag { get; }
        public override TimeSpan When { get; }
    }
}