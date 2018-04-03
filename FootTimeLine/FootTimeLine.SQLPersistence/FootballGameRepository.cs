using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FootTimeLine.Model;
using FootTimeLine.Model.Events;
using Newtonsoft.Json;

namespace FootTimeLine.SQLPersistence
{
    public class FootballGameRepository : IFootballGameRepository
    {
        public void Save(FootballGame game)
        {
            UnitOfWork(c =>
            {
                if (Find(game.GameId) != FootballGame.Null)
                    throw new ApplicationException("a game with this data already exist");

                var gameEntity = ConvertToEntity(game);
                gameEntity = c.FootballGames.Add(gameEntity);

                c.SaveChanges();

                var events = game.Events
                    .Select(@event => ConvertToEventData(gameEntity, @event))
                    .ToList();

                c.EventDatas.AddRange(events);
            });
        }

        private static FootballGameEntity ConvertToEntity(FootballGame game)
        {
            var gameEntity = new FootballGameEntity
            {
                AwayTeam = game.AwayTeam,
                HomeTeam = game.HomeTeam,
                League = game.League
            };
            return gameEntity;
        }

        private static EventData ConvertToEventData(FootballGameEntity gameEntity, MatchEvent @event)
        {
            return new EventData
            {
                FootballGameId = gameEntity.Id,
                SerializedEvent = JsonConvert.SerializeObject(@event),
                Type = @event.GetType().FullName
            };
        }

        public FootballGame Find(GameId gameId)
        {
            using (var context = new TimeLineContext())
            {
                var entity = context.FootballGames.SingleOrDefault(g =>
                    g.AwayTeam == gameId.AwayTeam
                    && g.HomeTeam == gameId.HomeTeam
                    && g.League == gameId.League
                );

                if (entity == null) return FootballGame.Null;

                var events = context.EventDatas
                    .Where(x => x.FootballGameId == entity.Id)
                    .ToList()
                    .Select(ConvertToModel);

                var game = new FootballGameProxy(entity);
                game.LoadEvents(events);
                return game;
            }
        }

        private MatchEvent ConvertToModel(EventData eventData)
        {
            Assembly asm = typeof(MatchEvent).Assembly;
            Type type = asm.GetType(eventData.Type);
            return JsonConvert.DeserializeObject(eventData.SerializedEvent, type) as MatchEvent;
        }

        private void UnitOfWork(Action<TimeLineContext> action)
        {
            using (var context = new TimeLineContext())
            {
                action(context);
                context.SaveChanges();
            }
        }
    }
}