using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalatroDSL.AST;
using BalatroDSL.Models;

namespace BalatroDSL.Scoring
{
    public class ScorableRoyalFlush : ScorableBase
    {
        public override string Label => "Royal Flush";
        public override int BaseChips => 100;
        public override int BaseMultiplier => 8;
        public override bool Matches(List<Card> cards) => 
            cards
                .GroupBy(c => c.Suit)
                .Any(g => g.Count() >= 5 &&
                          g.Any(c => c.Rank == Models.Rank.Ten) &&
                          g.Any(c => c.Rank == Models.Rank.Jack) &&
                          g.Any(c => c.Rank == Models.Rank.Queen) &&
                          g.Any(c => c.Rank == Models.Rank.King) &&
                          g.Any(c => c.Rank == Models.Rank.Ace));

        public override List<Card> GetMatchedCards(List<Card> cards) =>
            cards
                .GroupBy(c => c.Suit)
                .Where(g => g.Count() >= 5)
                .SelectMany(g => g)
                .Where(c => c.Rank == Models.Rank.Ten || c.Rank == Models.Rank.Jack || c.Rank == Models.Rank.Queen || c.Rank == Models.Rank.King || c.Rank == Models.Rank.Ace)
                .ToList();
    }
}