using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Models
{
    public enum Rank
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
    }
    public enum Suit
    {
        Hearts = 'H',
        Diamonds = 'D',
        Spades = 'S',
        Clubs = 'C'
    }
    public enum CardModifier
    {
        None,
        Bonus,
        Mult,
        Glass,
        /*
         * steel
         * stone
         * gold
         * lucky
         * wild
         */
    }

    public class Card
    {
        public Rank Rank { get; set; }
        public Suit Suit { get; set; }
        public int Chips => CardUtils.Rank2Chips(this);
        public CardModifier Modifier { get; set; } = CardModifier.None;
        public int OriginalIndex { get; set; } = -1; // Used for sorting

        public override string ToString() =>
            $"{Rank} of {Suit}" + (Modifier != CardModifier.None ? $" [{Modifier}]" : "");
    }

    public enum JokerModifier
    {
        None,
        Foil,
        Holographic,
        Polychrome,
        /*
         * negative
         */
    }
    public enum JokerType
    {
        AdditiveMult,
        Multiplicative,
        ChipAndAdditive,
        Retrigger
    }

    public class Joker
    {
        public JokerModifier Modifier { get; set; }
        public JokerType Type { get; set; }
        public int EffectValue { get; set; } // e.g., x2 → 2
        public override string ToString() =>
            $"Joker: {Type} [{Modifier}] with effect value {EffectValue}" +
            (Modifier != JokerModifier.None ? $" [{Modifier}]" : "");
    }
}
