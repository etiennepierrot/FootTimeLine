using System;

namespace FootTimeLine.Model
{
    public class Tweet
    {
        protected Tweet()
        {
            
        }

        public long Id { get; protected set; }
        public string Text { get; protected set; }
        public int Popularity => RTCount + FavCount;
        public DateTime Date { get; protected set; }
        public bool Video { get; protected set; }
        public int RTCount { get; protected set; }
        public int FavCount { get; protected set; }
        public long CreatorId { get; protected set; }
        public string Language { get; protected set; }

        public static Tweet Null = new Tweet();

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