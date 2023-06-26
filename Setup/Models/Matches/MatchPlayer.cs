namespace CardGame.Models.Matches
{
    public class MatchPlayer
    {
        public Guid Id { get; set; }
        public string ConnectionString { get; set; }
        public bool Ready { get; set; }
        public Stack<int> CardsInPlay { get; set; }
        public List<int> Hand { get; set; }
        public int Health { get; set; }
        public MatchPlayer(Guid id)
        {
            Id = id;
            CardsInPlay= new Stack<int>();
            Health = 20;
            Hand = new List<int>();
            Ready = false;
        }
    }
}
