using System;
using FootTimeLine.Model;

namespace FootTimeLine.SQLPersistence
{
    public class TweetEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public long TweetId { get; set; }
        public int RTCount { get; set; }
        public int FavCount { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string League { get; set; }
        public DateTime Date { get; set; }
        public long CreatorId { get; set; }
        public string Language { get; set; }
        public bool Video { get; set; }

    }
}