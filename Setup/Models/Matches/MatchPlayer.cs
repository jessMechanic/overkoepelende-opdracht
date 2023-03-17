﻿namespace CardGame.Models.Matches
{
    public class MatchPlayer
    {
        public Guid Id { get; set; }
        public Stack<int> Cards { get; set; }
        public List<int> Hand { get; set; }
        public int Health { get; set; }
        public MatchPlayer(Guid id)
        {
            Id = id;
            Cards= new Stack<int>();
            Health = 20;
        }
    }
}