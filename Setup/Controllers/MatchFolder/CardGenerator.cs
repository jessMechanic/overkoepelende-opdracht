using CardApi.Models.Matches;
using CardGame.Models.Matches;
using System;

namespace CardGame.Controllers.MatchFolder
{
    public static class CardGenerator
    {
        public static string[] CharacterNames = { "dragon", "naga", "commoner", "horse" };
        public static string[] CharacterPrefixes = { "normal", "special", "nauches", "depressed", "special", "enforced", "determent" };


        public static string[] BuildingNames = { "wall", "tower" };
        public static string[] BuildingPrefixes = { "tall", "small", "wide", "short" };



        static Random random = new();
        public static Card GenerateCard()
        {
            CardType type = (CardType)random.Next(Enum.GetNames(typeof(CardType)).Length);


            string name = "null"; 
            int effectLenght = 0;
            int damage = 0;
            switch (type)
            {
                case CardType.creature:
                    name = CharacterPrefixes[random.Next(CharacterPrefixes.Length)] + CharacterNames[random.Next(CharacterNames.Length)];
                    effectLenght = Enum.GetNames(typeof(CreatureEffect)).Length;
                    damage = random.Next(50);
                    break;
                case CardType.Defence:
                    name = BuildingPrefixes[random.Next(BuildingPrefixes.Length)] + BuildingNames[random.Next(BuildingNames.Length)];
                    effectLenght = Enum.GetNames(typeof(DefenceEffect)).Length;
                    break;
                default:
                  
                    break;
            }

            Effect effect = new( random.Next(effectLenght) );
            return new Card() { Name = name, Type = type,Health = random.Next(100),Damage = damage };
    }
}
}
