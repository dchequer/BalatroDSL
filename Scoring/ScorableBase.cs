using BalatroDSL.AST;
using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    public abstract class ScorableBase : IScorable
    {
        public abstract string Label { get; }
        public abstract bool Matches(List<Card> cards);
        public abstract List<Card> GetMatchedCards(List<Card> cards);
        public abstract int BaseChips { get; }
        public abstract int BaseMultiplier { get; }

        public ASTNode Score(List<Card> matchedCards, Hand hand)
        {
            int chips = BaseChips;
            int mult = BaseMultiplier;

            var root = new ASTNode($"Scoring Type: {Label}");
            root.AddChild(new ASTNode($"Base Score: {chips} Chips x {mult} Mult"));

            foreach (var card in matchedCards.OrderBy(c => c.OriginalIndex))
            {
                var cardNode = new ASTNode(card.ToString());

                int chipsValue = card.Chips;
                chips += chipsValue;
                cardNode.AddChild(new ASTNode($"+{chipsValue} Chips"));

                // apply modifiers
                switch (card.Modifier)
                {
                    case CardModifier.Bonus:
                        chips += 30;
                        cardNode.AddChild(new ASTNode($"+30 Chips from Bonus modifier"));
                        break;
                    case CardModifier.Mult:
                        mult += 4;
                        cardNode.AddChild(new ASTNode($"+4 Mult from Mult modifier"));
                        break;
                    case CardModifier.Glass:
                        // In theory glass card could break and not be used, but for simplicity we assume it is used
                        mult *= 2;
                        cardNode.AddChild(new ASTNode($"x2 Mult from Glass modifier"));
                        break;
                    default:
                        break;
                }

                // apply joker triggers
                foreach (var j in hand.Jokers)
                {
                    if (!string.IsNullOrEmpty(j.TriggerTarget) && j.TriggerTarget.Contains((char)card.Rank))
                    {
                        switch (j.Modifier)
                        {
                            case JokerModifier.Foil:
                                chips += 50;
                                cardNode.AddChild(new ASTNode($"+50 Chips from Foil Joker"));
                                break;
                            case JokerModifier.Holographic:
                                mult += 10;
                                cardNode.AddChild(new ASTNode($"+10 Mult from Joker"));
                                break;
                            case JokerModifier.Polychrome:
                                mult = (int)((double)mult * 1.5);
                                cardNode.AddChild(new ASTNode($"x1.5 Mult from Joker"));
                                break;
                            default:
                                break;
                        }
                    }
                }

                cardNode.AddChild(new ASTNode($"Current Score: {chips} x {mult}"));
                root.AddChild(cardNode);
            }

            // apply joker effects
            foreach (var j in hand.Jokers.Where(c => c.Type != JokerType.Retrigger).ToList())
            {
                var jokerNode = new ASTNode(j.ToASTString());
                switch (j.Type)
                {
                    case JokerType.ChipsAndAdditive:
                        if (j.EffectValue1 > 0)
                        {
                            chips += j.EffectValue1;
                            jokerNode.AddChild(new ASTNode($"+{j.EffectValue1} Chips from Joker"));
                        }
                        if (j.EffectValue2 > 0)
                        {
                            mult += j.EffectValue2.Value;
                            jokerNode.AddChild(new ASTNode($"+{j.EffectValue2.Value} Mult from Joker"));
                        }
                        break;
                    case JokerType.AdditiveMult:
                        if (j.EffectValue1 > 0)
                        {
                            mult += j.EffectValue1;
                            jokerNode.AddChild(new ASTNode($"+{j.EffectValue1} Mult from Joker"));
                        }
                        break;
                    case JokerType.Multiplicative:
                        if (j.EffectValue1 > 0)
                        {
                            mult *= j.EffectValue1;
                            jokerNode.AddChild(new ASTNode($"x{j.EffectValue1} Mult from Joker"));
                        }
                        break;
                    default:
                        break;
                }

                jokerNode.AddChild(new ASTNode($"Current Score: {chips} x {mult}"));
                root.AddChild(jokerNode);
            }
            root.AddChild(new ASTNode($"Final Score: {chips * mult}"));

            // update hand with final chips and mult
            hand.label = this.Label;
            hand.chips = chips;
            hand.multiplier = mult;


            return root;
        }
    }
}