using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalatroDSL.AST;
using BalatroDSL.Models;

namespace BalatroDSL.Scoring
{
    public class ScorableFlush : ScorableBase
    {
        public override string Label => "Flush";

        public override int BaseChips => 35;
        public override int BaseMultiplier => 4;

        public override bool Matches(List<Card> cards) =>
            cards
                .GroupBy(c => c.Suit)
                .Any(g => g.Count() >= 5);

        public override List<Card> GetMatchedCards(List<Card> cards)
        {
            // technically we could just return all 5 cards (but balatro does support hands of > 5 cards)
            return cards
                .GroupBy(c => c.Suit)
                .Where(g => g.Count() >= 5)
                .SelectMany(g => g)
                //.Take(5)
                .ToList();
        }
    }
}