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
        ChipsAndAdditive,
        Retrigger
    }

    public class Joker
    {
        public JokerModifier Modifier { get; set; }
        public JokerType Type { get; set; }
        public int EffectValue1 { get; set; } // First value (chips or mult)
        public int? EffectValue2 { get; set; } // Optional second value (for Chips&Mult)
        public string? TriggerTarget { get; set; } // Optional trigger target (like "3", "JQK")
        public override string ToString()
        {
            var desc = $"Joker: {Type} [{Modifier}] with effect {EffectValue1}";
            if (EffectValue2.HasValue)
                desc += $" & {EffectValue2}";
            if (!string.IsNullOrEmpty(TriggerTarget))
                desc += $" Trigger: {TriggerTarget}";
            return desc;
        }

        public string ToASTString()
        {
            var desc = $"[{Modifier}] {Type}";

            if (EffectValue1 != 0)
                desc += $" {EffectValue1}";

            if (EffectValue2.HasValue)
                desc += $" / {EffectValue2.Value}";

            if (!string.IsNullOrEmpty(TriggerTarget))
                desc += $" (Trigger: {TriggerTarget})";

            return desc;
        }

    }
}
