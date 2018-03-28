namespace FootTimeLine.Model
{
    public interface ITweetConnector
    {
        Tweet ExtractPopularTweet(string query);
    }
}