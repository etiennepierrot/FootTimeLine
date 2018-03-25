namespace FootTimeLine.Model
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public static Player Null = new Player(null);

        public override bool Equals(object obj)
        {
            return obj is Player player &&
                   Name == player.Name;
        }

        protected bool Equals(Player other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}