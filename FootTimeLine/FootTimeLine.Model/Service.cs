﻿using System.Collections.Generic;

namespace FootTimeLine.Model
{
    public class Service
    {
        private readonly IEventCollector _eventCollector;

        public Service(IEventCollector eventCollector)
        {
            _eventCollector = eventCollector;
        }

        public FootballGame Collect(string homeTeam, string awayTeam, string league)
        {
            FootballGame footballGame = new FootballGame(homeTeam, awayTeam, league);
            List<MatchEvent> matchEvents = _eventCollector.CollectEvent(footballGame);

            foreach (var matchEvent in matchEvents)
            {
                footballGame.AddEvent(matchEvent);
            }

            return footballGame;
        }
    }
}