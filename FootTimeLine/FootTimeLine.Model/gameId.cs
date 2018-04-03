namespace FootTimeLine.Model
{
    public class GameId
    {
        public GameId(string homeTeam, string awayTeam, string league)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            League = league;
        }

        public string HomeTeam { get; private set; }
        public string AwayTeam { get; private set; }
        public string League { get; private set; }
    }
}