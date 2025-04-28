using BalatroDSL.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Models
{
    public static class Constants
    {
        // card modifiers
        public const int BONUS_CARD_CHIPS = 30;  // + 30
        public const int MULT_CARD_MULT = 4;     // + 4
        public const int GLASS_CARD_MULT = 2;    // * 2

        // joker modifiers
        public const int JOKER_FOIL_CHIPS = 50;    // + 50
        public const int JOKER_HOLO_MULT = 10;     // + 10
        public const double JOKER_POLY_MULT = 1.5; // * 1.5
    }



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

        public void Score(ref int chips, ref int mult, ref ASTNode cardNode)
        {
            chips += Chips;
            cardNode.AddChild(new ASTNode($"+{Chips} Chips"));

            // apply modifiers
            switch (Modifier)
            {
                case CardModifier.Bonus: 
                    chips += Constants.BONUS_CARD_CHIPS; 
                    cardNode.AddChild(new ASTNode($"+{Constants.BONUS_CARD_CHIPS} Chips from Bonus modifier"));
                    break;
                case CardModifier.Mult: 
                    mult += Constants.MULT_CARD_MULT;
                    cardNode.AddChild(new ASTNode($"+{Constants.MULT_CARD_MULT} Mult from Mult modifier"));
                    break;
                case CardModifier.Glass:
                    mult *= Constants.GLASS_CARD_MULT;
                    cardNode.AddChild(new ASTNode($"x{Constants.GLASS_CARD_MULT} Mult from Glass modifier"));
                    break;
            }
        }
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
        Trigger
    }
    public enum JokerTriggerMode
    {
        ReplayCard,
        AddEffectOnTrigger
    }
    public enum JokerTriggerEffect
    {
        None,
        AddMult,
        AddChips,
        MultiplyMult,
    }

    public class Joker
    {
        public JokerModifier Modifier { get; set; }
        public JokerType Type { get; set; }
        public int? EffectValue1 { get; set; } // First value (chips or mult)
        public int? EffectValue2 { get; set; } // Optional second value (for Chips&Mult)
        public string? TriggerTarget { get; set; } // Optional trigger target (like "3", "JQK")
        public JokerTriggerMode? TriggerMode { get; set; }
        public JokerTriggerEffect? TriggerEffect { get; set; } // Optional trigger effect (like "ReplayCard", "AddChipsOrMult")

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
        public void ApplyPassive(ref int chips, ref int mult, ref ASTNode jokerNode) // apply some effect 
        {
            switch (Modifier)
            {
                case JokerModifier.Foil:
                    chips += Constants.JOKER_FOIL_CHIPS;
                    jokerNode.AddChild(new ASTNode($"+{Constants.JOKER_FOIL_CHIPS} Chips from Foil modifier"));
                    break;
                case JokerModifier.Holographic:
                    mult += Constants.JOKER_HOLO_MULT;
                    jokerNode.AddChild(new ASTNode($"+{Constants.JOKER_HOLO_MULT} Mult from Holographic modifier"));
                    break;
                case JokerModifier.Polychrome:
                    mult = (int)((double)mult * Constants.JOKER_POLY_MULT);
                    jokerNode.AddChild(new ASTNode($"x{Constants.JOKER_POLY_MULT} Mult from Polychrome modifier"));
                    break;
            }

            switch (Type)
            {
                case JokerType.AdditiveMult:
                    mult += EffectValue1 ?? 0;
                    jokerNode.AddChild(new ASTNode($"+{EffectValue1} Mult"));
                    break;
                case JokerType.Multiplicative:
                    mult *= EffectValue1 ?? 1;
                    jokerNode.AddChild(new ASTNode($"x{EffectValue1} Mult"));
                    break;
                case JokerType.ChipsAndAdditive:
                    chips += EffectValue1 ?? 0;
                    mult += EffectValue2 ?? 0;
                    jokerNode.AddChild(new ASTNode($"+{EffectValue1} Chips & +{EffectValue2} Mult"));
                    break;

                case JokerType.Trigger:
                    switch (TriggerEffect)
                    {
                        case JokerTriggerEffect.AddMult:
                            mult += EffectValue1 ?? 0;
                            jokerNode.AddChild(new ASTNode($"+{EffectValue1} Mult"));
                            break;
                        case JokerTriggerEffect.MultiplyMult:
                            mult *= EffectValue1 ?? 1;
                            jokerNode.AddChild(new ASTNode($"x{EffectValue1} Mult"));
                            break;
                        case JokerTriggerEffect.AddChips:
                            chips += EffectValue1 ?? 0;
                            jokerNode.AddChild(new ASTNode($"+{EffectValue1} Chips"));
                            break;
                        default:
                            jokerNode.AddChild(new ASTNode("Hmm... Should never be here."));
                            break;
                    }
                    break;
            }
        }

    }
}
