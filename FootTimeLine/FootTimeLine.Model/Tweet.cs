namespace FootTimeLine.Model
{
    public class Tweet
    {
        public Tweet(string user, string text, int count, string html)
        {
            User = user;
            Text = text;
            Count = count;
            Html = html;
        }

        public string User { get; }
        public string Text { get; }
        public int Count { get; }
        public string Html { get; }
        public static Tweet Null = new Tweet(null, null, 0, null);

        public override string ToString()
        {
            return Text;
        }
    }
}