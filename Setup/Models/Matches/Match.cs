﻿


using CardGame.Controllers.MatchFolder;
using System.Numerics;
using System.Reflection;

namespace CardGame.Models.Matches
{
    public class Match
    {
        MatchPlayer player1 { get; set; }
        MatchPlayer player2 { get; set; }
        List<Card> cards;
        public Match(Guid player1Id, Guid player2Id)
        {
            cards = new();
            player1 = new(player1Id);
            player2 = new(player2Id);


        }
        public Card? GetCard(int card) {
             return cards[card];
        }
        public void AddPlayer2(Guid id)
        {
             player2= new(id);
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
                return -1;
            }
            cards.Add(CardGenerator.GenerateCard());
            player.Cards.Push(cards.Count);
            return cards.Count;

        }

        public void PlayCard(Guid playerId,  int Card)
        {
            MatchPlayer player = getPlayer(playerId);
            if (player == null) return;

            if (player.Hand.Contains(Card))
            {
                player.Hand.Remove(Card);
                player.Cards.Push(Card);
            }
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
            for (int i = 0; i < sender.Cards.Count; i++)
            {
                Card card = cards.ElementAt(sender.Cards.ElementAt(i));
                if (card.Type == CardType.creature)
                {
                    for (int j = 0; j < recipient.Cards.Count; j++)
                    {
                        Card target = cards.ElementAt(recipient.Cards.ElementAt(j));
                        if (target.Health > 0)
                        {
                            DealDamage(recipient.Id, card.Damage);
                            break;
                        }
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public void RemoveDeadCards(MatchPlayer target)
        {
            while (cards.ElementAt(target.Cards.Peek()).Health < 0)  { target.Cards.Pop(); };
        }
        public void DealDamage(Guid target, int damage)
        {
            MatchPlayer? player = getPlayer(target);
            if (player == null) return;

            if (player.Cards.Count > 0)
            {
                cards.ElementAt(player.Cards.Peek()).Health -= damage;
            }
        }


    }
}
