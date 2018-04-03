using System;
using Newtonsoft.Json;

namespace FootTimeLine.Model.Events
{
    public class MatchBegin : MatchEvent
    {
        public MatchBegin(DateTime gameStartedAt)
        {
            Date = gameStartedAt;
            When = TimeSpan.Zero;
        }

        [JsonConstructor]
        protected MatchBegin()
        {
            
        }

        [JsonProperty]
        public DateTime Date { get; protected set; }
    }
}