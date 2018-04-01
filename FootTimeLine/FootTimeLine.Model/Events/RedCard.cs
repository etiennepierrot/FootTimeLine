using System;

namespace FootTimeLine.Model.Events
{
    public class RedCard : MatchEvent
    {
        public RedCard(TimeSpan when, Player player)
        {
            When = when;
            Player = player;
        }

        public override TimeSpan When { get; }
        public Player Player { get; }

        public override string ToString()
        {
            return $"carton jaune pour {Player.Name} à la {When.TotalMinutes}ème minutes";
        }
    }
}