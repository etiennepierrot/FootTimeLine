using System.Data.Entity;

namespace FootTimeLine.SQLPersistence
{
    public class TimeLineContext : DbContext
    {
        public DbSet<FootballGameEntity> FootballGames { get; set; }
        public DbSet<EventData> EventDatas { get; set; }
        public DbSet<TweetEntity> Tweets { get; set; }
    }
}