using System;

namespace FootTimeLine.Model
{
    public class Goal : MatchEvent
    {
        public Goal(TimeSpan when, Player scorer, Player assister, TypeGoal type)
        {
            When = when;
            Scorer = scorer;
            Assister = assister;
            Type = type;
        }

        public Player Scorer { get; }
        public Player Assister { get; }
        public TypeGoal Type { get; }

        public override string ToString()
        {
            switch (Type)
            {
                case TypeGoal.OwnGoal:
                    return $"La loose, CSC de {Scorer} à la {When.Minutes}ème minute";
                default:
                    return IsAssisted()
                        ? $"But de {Scorer} à la {When.TotalMinutes}ème minute sur une passe de {Assister}"
                        : $"But de {Scorer} à la {When.TotalMinutes}ème minute";
            }
        }

        private bool IsAssisted()
        {
            return !Equals(Assister, Player.Null);
        }

        public override TimeSpan When { get; }
    }
}
