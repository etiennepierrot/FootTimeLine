using System;

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

        public Player PlayerOut { get; }
        public Player PlayerIn { get; }
        public override TimeSpan When { get; }

        public override string ToString()
        {
            return $"{PlayerOut.Name}  est remplacé par {PlayerIn.Name} à la {When.TotalMinutes}ème minutes";
        }
    }
}