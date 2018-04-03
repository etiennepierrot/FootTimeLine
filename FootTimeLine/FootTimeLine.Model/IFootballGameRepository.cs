namespace FootTimeLine.Model
{
    public interface IFootballGameRepository
    {
        void Save(FootballGame game);
        FootballGame Find(GameId gameId);
    }
}