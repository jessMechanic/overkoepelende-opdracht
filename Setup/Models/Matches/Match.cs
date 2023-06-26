


using CardGame.Controllers.MatchFolder;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using System.Reflection;

namespace CardGame.Models.Matches
{
    public class Match
    {
        public MatchPlayer player1 { get; set; }
        public MatchPlayer player2 { get; set; }
        List<Card> cards;

        public bool FirstPlayed;
        public bool AllowDraw;
        public Match(Guid player1Id, Guid player2Id)
        {
            cards = new();
            player1 = new(player1Id);
            player2 = new(player2Id);
            FirstPlayed = true;
            AllowDraw = false;

        }
        public Card? GetCard(int card)
        {
            return cards[card];
        }

        public int GetCardsInPlayCount(Guid id)
        {
        return    getPlayer(id).CardsInPlay.Count;
        }

        public int GetHandCount(Guid id)
        {
            return getPlayer(id).Hand.Count;
        }
        public void AddPlayer2(Guid id)
        {
            player2 = new(id);
        }

        public MatchPlayer? getPlayer(Guid id)
        {
            if (id == player1.Id)
                return player1;
            else if (id == player2.Id)
                return player2;
            return null;
        }
        public int DrawCard(Guid playerId)
        {
            MatchPlayer? player = getPlayer(playerId);
            if (player == null)
            {
                Console.WriteLine("player doesnt exist // drawCard");
                return -1;
            }
            Card card = CardGenerator.GenerateCard();
            int index = cards.Count;
            Console.WriteLine(index);
            card.Index = index;
            cards.Add(card);
            player.Hand.Add(index);
            return cards.Count - 1;

        }

        public bool PlayCard(Guid playerId, int Card)
        {
            MatchPlayer player = getPlayer(playerId);
            if (player == null) { Console.WriteLine("playeer doesnt excist"); return false; }
            if (player.Ready) { return false; }
            if (player.Hand.Contains(Card))
            {
                player.Hand.Remove(Card);
                player.CardsInPlay.Push(Card);
                return true;
            }
            Console.WriteLine("didnt contain in hand " + Card);
            foreach (var item in player.Hand)
            {
                Console.WriteLine(item);
            }
            return false;
        }
        public bool checkReady()
        {
            if (player1.Ready && player2.Ready)
            {
                PlayRound();
                return true;
            }
            Console.WriteLine($"player1 : {player1.Ready} && player2 : {player2.Ready}");
            return false;
        }
        public void PlayRound()
        {
            PlaySide(player1, player2);
            PlaySide(player2, player1);


            // cus else player2 will be sad after going last at everything
            RemoveDeadCards(player2);
            RemoveDeadCards(player1);
        }
        public void PlaySide(MatchPlayer sender, MatchPlayer recipient)
        {
            if (sender.CardsInPlay.IsNullOrEmpty()) { return; }
            Card card = cards.ElementAt(sender.CardsInPlay.Peek());
            DealDamage(recipient.Id, card.Damage);


        }

        public void RemoveDeadCards(MatchPlayer target)
        {
            if (target.CardsInPlay.IsNullOrEmpty())
            {
                Console.WriteLine($"player {target.Id} has lost");
                return;
            }
            if (cards.ElementAt(target.CardsInPlay.Peek()).Health < 0)
            {
                target.CardsInPlay.Pop();
            }
        }
        public void DealDamage(Guid target, int damage)
        {
            MatchPlayer? player = getPlayer(target);
            if (player == null) return;
            if (player.CardsInPlay.IsNullOrEmpty())
            {
                Console.WriteLine("player doesnt have cards");
                return;
            }
            if (player.CardsInPlay.Count > 0)
            {
                Card card = cards.ElementAt(player.CardsInPlay.Peek());
                if (card != null)
                {

                    card.Health -= damage;
                }
            }
        }


    }
}
