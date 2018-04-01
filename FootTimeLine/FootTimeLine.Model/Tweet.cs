using System;

namespace FootTimeLine.Model
{
    public class Tweet
    {
        protected Tweet(long id, string user, string text, int popularity, DateTime date)
        {
            Id = id;
            User = user;
            Text = text;
            Popularity = popularity;
            Date = date;
        }

        public long Id { get; }
        public string User { get; }
        public string Text { get; }
        public int Popularity { get; }
        public DateTime Date { get; }

        public virtual string Display()
        {
            return string.Empty;
        }

        public static Tweet Null = new Tweet(0, null, null, 0, DateTime.UtcNow);

        public override string ToString()
        {
            return Text;
        }

        public override bool Equals(object obj)
        {
            var tweet = obj as Tweet;
            return tweet != null &&
                   Id == tweet.Id;
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}