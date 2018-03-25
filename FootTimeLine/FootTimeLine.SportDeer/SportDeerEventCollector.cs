using System;
using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model;

namespace FootTimeLine.SportDeer
{
    public class SportDeerEventCollector : IEventCollector
    {
        private readonly Connector _connector;
        
        public SportDeerEventCollector(string refreshToken)
        {
            _connector = new Connector(refreshToken);
        }

        
        public List<MatchEvent> CollectEvent(FootballGame footballGame)
        {
            var season = GetSeason(footballGame);
            var (homeTeam, awayTeam) = GetTeams(footballGame, season);
            SportDeerMatch sportDeerMatch = GetMatch(homeTeam, awayTeam);

            return sportDeerMatch
                    .events
                    .Select(CreateEvent)
                    .ToList();
        }

        private MatchEvent CreateEvent(Event @event)
        {
            switch (@event.type)
            {
                case "goal":
                    return CreateGoalEvent(@event);
                default:
                    return new GenericEvent();
            }
        }

        private MatchEvent CreateGoalEvent(Event @event)
        {
            var scorer = GetScorer(@event);
            var assister =  GetAssister(@event);
            TypeGoal type = @event.goal_type_code == "og" ? TypeGoal.OwnGoal : TypeGoal.Normal;
            return new Goal(
                TimeSpan.FromMinutes(@event.elapsed),
                scorer, 
                assister, 
                type);
        }

        private Player GetScorer(Event @event)
        {
            var scorerEntity = GetPlayer(@event.id_team_season_scorer);
            return new Player(scorerEntity.player_name);
        }

        private Player GetAssister(Event @event)
        {
            if (@event.id_team_season_assister == 0)
            {
                return Player.Null;
            }

            var assister = GetPlayer(@event.id_team_season_assister);
            return new Player(assister.player_name);
        }

        private Season GetSeason(FootballGame footballGame)
        {
            var league = GetLeague(footballGame.League);
            return league.Seasons.Last();
        }

        private PlayerTeamSeason GetPlayer(long idPlayer)
        {
            return _connector
                .Request<Connector.Response<PlayerTeamSeason>>($"v1/teamSeasonPlayers/{idPlayer}", 1)
                .Data.docs.Single();
        }

        private (Team homeTeam, Team awayTeam) GetTeams(FootballGame footballGame, Season season)
        {
            var teams = GetTeams(season);
            var homeTeam = teams.Single(t => t.team_name == footballGame.HomeTeam);
            var awayTeam = teams.Single(t => t.team_name == footballGame.AwayTeam);
            return (homeTeam, awayTeam);
        }

        private SportDeerMatch GetMatch(Team homeTeam, Team awayTeam)
        {
            var restResponse = _connector.Request<Connector.Response<SportDeerMatch>>($"v1/teamSeason2teamSeason?teamSeason1={homeTeam._id}&teamSeason2={awayTeam._id}&populate=events", 1);
            return restResponse.Data.docs.Single(x =>
                x.id_team_season_home == homeTeam._id
                && x.id_team_season_away == awayTeam._id);
        }

        private List<Team> GetTeams(Season season)
        {
            string endpointTeams = $"v1/seasons/{season._id}/teamSeasons";
            var result = _connector.Request<Connector.Response<Team>>(endpointTeams, 1);
            return result.Data.docs;
        }

        private League GetLeague(string leagueName)
        {
            string endpointStages = "v1/leagues";
            League league = _connector.SearchInList<League>(leagueName, endpointStages);
            var result = _connector.Request<Connector.Response<League>>($"v1/leagues/{league._id}?populate=seasons", 1);
            return result.Data.docs.Single();
        }

        class League : Connector.Entity
        {
            public List<Season> Seasons { get; set; }
        }

        class Season : Connector.Entity
        {
            public string Years { get; set; }
        }

        class PlayerTeamSeason : Connector.Entity
        {
            public string player_name { get; set; }
        }

        class Team : Connector.Entity
        {
            public string team_name { get; set; }
        }

        class SportDeerMatch : Connector.Entity
        {
            public int id_team_season_away { get; set; }
            public int id_team_season_home { get; set; }
            public int number_goal_team_away { get; set; }
            public int number_goal_team_home { get; set; }
            public List<Event> events { get; set; }
        }

        class Event
        {
            public string _id { get; set; }
            public string type { get; set; }
            public int elapsed { get; set; }
            public string card_type { get; set; }
            public string id_team_season { get; set; }
            public string id_team_season_player { get; set; }
            public string goal_subtype { get; set; }
            public string goal_type_code { get; set; }
            public int id_team_season_scorer { get; set; }
            public int id_team_season_assister { get; set; }
            public int id_team_season_player_in { get; set; }
            public int id_team_season_player_out { get; set; }
        }
    }
}