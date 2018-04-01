﻿using System;

namespace FootTimeLine.Model
{
    public class MatchEnd : MatchEvent
    {
        public MatchEnd(DateTime gameStart, DateTime gameEndedAt)
        {
            When = gameEndedAt.Subtract(gameStart);
        }

        public override TimeSpan When { get; }
    }
}