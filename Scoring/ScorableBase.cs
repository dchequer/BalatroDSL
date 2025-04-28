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

                // apply base score and modifiers
                card.Score(ref chips, ref mult, ref cardNode);

                // apply joker, if any, active joker effects
                foreach (var joker in hand.Jokers.Where(j => j.Type == JokerType.Trigger && CardUtils.CardMatchesTrigger(card, j.TriggerTarget)))
                {   
                    if (joker.TriggerMode == JokerTriggerMode.ReplayCard)
                    {
                        // replay the card
                        var replayNode = new ASTNode($"Replaying {card.ToString()}");
                        card.Score(ref chips, ref mult, ref replayNode);
                        replayNode.AddChild(new ASTNode($"Current Score: {chips} x {mult}"));
                        cardNode.AddChild(replayNode);
                    }
                    else if (joker.TriggerMode == JokerTriggerMode.AddEffectOnTrigger)
                    {
                        // apply joker effect
                        var jokerNode = new ASTNode(joker.ToASTString());
                        joker.ApplyPassive(ref chips, ref mult, ref jokerNode);
                        jokerNode.AddChild(new ASTNode($"Current Score: {chips} x {mult}"));
                        cardNode.AddChild(jokerNode);
                    }

                }

                    // add current score and append node
                    cardNode.AddChild(new ASTNode($"Current Score: {chips} x {mult}"));
                root.AddChild(cardNode);
            }

            // apply "passive" joker effects
            foreach (var joker in hand.Jokers.Where(j => j.Type != JokerType.Trigger).ToList())
            {
                var jokerNode = new ASTNode(joker.ToASTString());

                // apply passive joker effects
                joker.ApplyPassive(ref chips, ref mult, ref jokerNode);

                // add rolling score and append node
                jokerNode.AddChild(new ASTNode($"Current Score: {chips} x {mult}"));
                root.AddChild(jokerNode);
            }

            // update final score
            root.AddChild(new ASTNode($"Final Score: {chips * mult}"));

            // update hand with final chips and mult
            hand.label = this.Label;
            hand.chips = chips;
            hand.multiplier = mult;


            return root;
        }
    }
}