namespace FootTimeLine.Model
{
    public class Tweet
    {
        public Tweet(string user, string text, int count)
        {
            User = user;
            Text = text;
            Count = count;
        }

        public string User { get; }
        public string Text { get; }
        public int Count { get; }
    }
}