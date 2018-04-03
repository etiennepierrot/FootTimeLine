namespace FootTimeLine.SQLPersistence
{
    public class FootballGameEntity
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string League { get; set; }
        public string HashTag { get; set; }
    }
}