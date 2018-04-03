using System;
using Newtonsoft.Json;

namespace FootTimeLine.Model.Events
{
    public class YellowCard : MatchEvent
    {
        public YellowCard(TimeSpan when, Player player)
        {
            When = when;
            Player = player;
        }

        [JsonProperty]
        public Player Player { get; private set; }

        public override string ToString()
        {
            return $"carton jaune pour {Player.Name} à la {When.TotalMinutes}ème minutes";
        }
    }
}