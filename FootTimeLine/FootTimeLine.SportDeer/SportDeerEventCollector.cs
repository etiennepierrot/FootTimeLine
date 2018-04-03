using System;
using System.Collections.Generic;
using System.Linq;
using FootTimeLine.Model;
using FootTimeLine.Model.Events;

namespace FootTimeLine.SportDeer
{
    public class SportDeerEventCollector : IEventCollector
    {
        private readonly Connector _connector;

        public SportDeerEventCollector(string refreshToken)
        {
            _connector = new Connector(refreshToken);
        }


        public List<MatchEvent> CollectEvent(GameId gameId)
        {
            var season = GetSeason(gameId);
            var (homeTeam, awayTeam) = GetTeams(gameId, season);
            SportDeerMatch sportDeerMatch = GetMatch(homeTeam, awayTeam);
            List<MatchEvent> events = new List<MatchEvent> {new MatchBegin(sportDeerMatch.game_started_at)};
            
            sportDeerMatch
                .events
                .ForEach(e => events.Add(CreateEvent(e)));

            events.Add(new MatchEnd(sportDeerMatch.game_started_at, sportDeerMatch.game_ended_at));

            return events;
        }

        private MatchEvent CreateEvent(Event @event)
        {
            switch (@event.type)
            {
                case "goal":
                    return CreateGoalEvent(@event);
                case "card":
                    return CreateCardEvent(@event);
                case "subst":
                    return CreateSubstitutionEvent(@event);
                default:
                    return new GenericEvent(TimeSpan.FromMinutes(@event.elapsed));
            }
        }

        private MatchEvent CreateSubstitutionEvent(Event @event)
        {
            var when = TimeSpan.FromMinutes(@event.elapsed);
            var playerOutObject = GetPlayer(@event.id_team_season_player_out);
            var playerOut = new Player(playerOutObject.player_name);
            var playerInObject = GetPlayer(@event.id_team_season_player_in);
            var playerIn = new Player(playerInObject.player_name);
            return new Substitution(when, playerOut, playerIn);
        }

        private MatchEvent CreateCardEvent(Event @event)
        {
            var when = TimeSpan.FromMinutes(@event.elapsed);
            var playerObject = GetPlayer(int.Parse(@event.id_team_season_player));
            var player = new Player(playerObject.player_name);
            switch (@event.card_type)
            {
                case "y":
                    return new YellowCard(when, player);
                case "r":
                    return new RedCard(when, player);
                default:
                    throw new ApplicationException("unknow card type");
            }
        }

        private MatchEvent CreateGoalEvent(Event @event)
        {
            var scorer = GetScorer(@event);
            var assister = GetAssister(@event);
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

        private Season GetSeason(GameId gameId)
        {
            var league = GetLeague(gameId.League);
            return league.Seasons.Last();
        }

        private PlayerTeamSeason GetPlayer(long idPlayer)
        {
            return _connector
                .Request<Connector.Response<PlayerTeamSeason>>($"v1/teamSeasonPlayers/{idPlayer}", 1)
                .Data.docs.Single();
        }

        private (Team homeTeam, Team awayTeam) GetTeams(GameId gameId, Season season)
        {
            var teams = GetTeams(season);
            var homeTeam = teams.Single(t => t.team_name == gameId.HomeTeam);
            var awayTeam = teams.Single(t => t.team_name == gameId.AwayTeam);
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
            public DateTime game_started_at { get; set; }
            public DateTime game_ended_at { get; set; }
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