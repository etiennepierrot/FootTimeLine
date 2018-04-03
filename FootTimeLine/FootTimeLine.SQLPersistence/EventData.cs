namespace FootTimeLine.SQLPersistence
{
    public class EventData
    {
        public int Id { get; set; }
        public string SerializedEvent { get; set; }
        public string Type { get; set; }
        public int FootballGameId { get; set; }
    }
}