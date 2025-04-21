using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Models
{
    public enum Suit
    {
        Hearts = 'H',
        Diamonds = 'D',
        Spades = 'S',
        Clubs = 'C'
    }

    internal class Card
    {
        public string Rank { get; set; }
        public Suit Suit { get; set; }

        public override string ToString() => $"{Rank} of {Suit}";
    }
}
