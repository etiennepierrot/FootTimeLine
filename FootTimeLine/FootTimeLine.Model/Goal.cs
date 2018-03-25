using System;

namespace FootTimeLine.Model
{
    public class Goal : MatchEvent
    {
        public Goal(TimeSpan goalTime, Player scorer, Player assister, TypeGoal type)
        {
            GoalTime = goalTime;
            Scorer = scorer;
            Assister = assister;
            Type = type;
        }

        public TimeSpan GoalTime { get; }
        public Player Scorer { get; }
        public Player Assister { get; }
        public TypeGoal Type { get; }
    }
}
