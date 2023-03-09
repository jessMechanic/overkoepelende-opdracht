

using CardApi.Models.Matches;

namespace CardGame.Models.Matches
{
    public class Match
    {
        Guid Matchid;
        Guid Player1;
        Guid? Player2;

        List<Card> Player1Cards;
        List<Card> Player2Cards;
        public Match(Guid matchid, Guid player1)
        {
            Matchid = matchid;
            Player1 = player1;
            Player1Cards = new();
            Player2Cards = new();
        }

        public void DrawCard(Guid player)
        {
            bool isPlayer1 = Player1.Equals(player);
            bool isPlayer2 = Player2.Equals(player);
            if (isPlayer1 ^ isPlayer2 == false)
                return;

            if(isPlayer1)
                drawCardplr1();
            else
                drawCardplr2();
            

            return;
        }

        public void PlayCard(Guid player , int Id)
        {

        }

        private void drawCardplr1()
        {
            Card card = new();  //get random card with api and player 1 id
            Player1Cards.Add(card);

        }
        private void drawCardplr2()
        {
            Card card = new();  //get random card with api and player 1 id
            Player2Cards.Add(card);
        }

    }
}
