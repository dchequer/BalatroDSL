using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalatroDSL.AST;
using BalatroDSL.Models;

namespace BalatroDSL.Scoring
{
    public class ScorablePair : ScorableBase
    {
        public override string Label => "Pair";

        public override int BaseChips => 10;
        public override int BaseMultiplier => 2;

        public override bool Matches(List<Card> cards) =>
            cards.GroupBy(c => c.Rank).Any(g => g.Count() == 2);

        public override List<Card> GetMatchedCards(List<Card> cards) =>
            cards
                .GroupBy(c => c.Rank)
                .Where(g => g.Count() == 2)
                .SelectMany(g => g)
                .ToList();
    }
}
