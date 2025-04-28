using BalatroDSL.AST;
using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    internal class ScorableTwoPair : ScorableBase
    {
        public override string Label => "Two Pair";

        public override int BaseChips => 20;
        public override int BaseMultiplier => 2;

        public override bool Matches(List<Card> cards) =>
            cards.GroupBy(c => c.Rank).Count(g => g.Count() == 2) == 2;

        public override List<Card> GetMatchedCards(List<Card> cards)
        {
            return cards
                .GroupBy(c => c.Rank)
                .Where(g => g.Count() == 2)
                .SelectMany(g => g)
                .ToList();
        }
    }
}