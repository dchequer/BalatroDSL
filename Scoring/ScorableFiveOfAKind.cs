using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalatroDSL.AST;
using BalatroDSL.Models;

namespace BalatroDSL.Scoring
{
    internal class ScorableFiveOfAKind : ScorableBase
    {
        public override string Label => "Five Of A Kind";

        public override int BaseChips => 120;
        public override int BaseMultiplier => 12;

        public override bool Matches(List<Card> cards) =>
            cards.GroupBy(c => c.Rank).Any(g => g.Count() == 5);

        public override List<Card> GetMatchedCards(List<Card> cards) =>
            cards
                .GroupBy(c => c.Rank)
                .Where(g => g.Count() == 5)
                .SelectMany(g => g)
                .ToList();
    }
}