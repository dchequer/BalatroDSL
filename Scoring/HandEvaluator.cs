using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    public class HandEvaluationResult
    {
        public IScorable Scorable { get; set; }
        public List<Card> MatchedCards { get; set; } = new();
        public List<Card> Leftovers { get; set; } = new();
    }
    internal class HandEvaluator
    {
        private static readonly List<ScorableBase> scorables = new()
        {
            // order scorables in the order of priority (royal flush -> high card)
            new ScorableFlushFive(),
            new ScorableFlushHouse(),
            new ScorableFiveOfAKind(),
            new ScorableRoyalFlush(),
            new ScorableStraightFlush(),
            new ScorableFourOfAKind(),
            new ScorableFullHouse(),
            new ScorableFlush(),
            new ScorableStraight(),
            new ScorableThreeOfAKind(),
            new ScorableTwoPair(),
            new ScorablePair(),
            new ScorableHighCard(),
            // Add more scorables here: Flush, FullHouse, etc.
        };

        public static HandEvaluationResult Evaluate(Hand hand)
        {
            foreach (var scorable in scorables)
            {
                if (scorable.Matches(hand.Cards))
                {
                    var matched = scorable.GetMatchedCards(hand.Cards);
                    var leftovers = hand.Cards.Except(matched).ToList();

                    return new HandEvaluationResult
                    {
                        Scorable = scorable,
                        MatchedCards = matched,
                        Leftovers = leftovers
                    };
                }
            }

            return new HandEvaluationResult
            {
                Scorable = null,
                MatchedCards = new(),
                Leftovers = hand.Cards
            };
        }
    }
}
