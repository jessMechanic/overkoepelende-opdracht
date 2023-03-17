

namespace CardGame.Models.Matches
{
    public enum CardType
    {
        creature,
        Defence,
    }
    public class Card
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public int Health { get; set; }
        public int Damage { get; set; }
        public int  Effects { get; set; }
        public CardType Type { get; set; }
    }
}
