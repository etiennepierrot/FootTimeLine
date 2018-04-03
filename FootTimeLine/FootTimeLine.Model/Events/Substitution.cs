using System;
using Newtonsoft.Json;

namespace FootTimeLine.Model.Events
{
    public class Substitution : MatchEvent
    {
        public Substitution(TimeSpan when, Player playerOut, Player playerIn)
        {
            PlayerOut = playerOut;
            PlayerIn = playerIn;
            When = when;
        }

        [JsonProperty]
        public Player PlayerOut { get; private set; }
        [JsonProperty]
        public Player PlayerIn { get; private set; }

        public override string ToString()
        {
            return $"{PlayerOut.Name}  est remplacé par {PlayerIn.Name} à la {When.TotalMinutes}ème minutes";
        }
    }
}