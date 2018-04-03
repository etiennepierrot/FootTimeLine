using System;
using Newtonsoft.Json;

namespace FootTimeLine.Model.Events
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
        [JsonConstructor]
        protected Goal() { }

        [JsonProperty]
        public Player Scorer { get; private set; }
        [JsonProperty]
        public Player Assister { get; private set; }
        [JsonProperty]
        public TypeGoal Type { get; private set; }

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

    }
}
