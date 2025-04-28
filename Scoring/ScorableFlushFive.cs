using BalatroDSL.AST;
using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    internal class ScorableFlushFive : ScorableBase
    {
        public override string Label => "Flush Five";

        public override int BaseChips => 160;
        public override int BaseMultiplier => 16;

        public override bool Matches(List<Card> cards) => 
            cards
                .GroupBy(c => new { c.Suit, c.Rank })
                .Any(g => g.Count() >= 5);

        public override List<Card> GetMatchedCards(List<Card> cards) =>
            cards
                .GroupBy(c => new { c.Suit, c.Rank })
                .Where(g => g.Count() >= 5)
                .SelectMany(g => g)
                .ToList();
    }
}