using System;
using Newtonsoft.Json;

namespace FootTimeLine.Model.Events
{
    public abstract class MatchEvent
    {
        [JsonProperty]
        public TimeSpan When { get; protected set; }
    }
}