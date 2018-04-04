using System.Collections.Generic;

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

        public string HomeTeam { get; }
        public string AwayTeam { get; }
        public string League { get; }

        public override bool Equals(object obj)
        {
            var id = obj as GameId;
            return id != null &&
                   HomeTeam == id.HomeTeam &&
                   AwayTeam == id.AwayTeam &&
                   League == id.League;
        }

        public override int GetHashCode()
        {
            var hashCode = 251212329;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HomeTeam);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(AwayTeam);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(League);
            return hashCode;
        }
    }
}