using BalatroDSL.AST;
using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    internal class ScorableThreeOfAKind : ScorableBase
    {
        public override string Label => "Three Of A Kind";

        public override int BaseChips => 30;
        public override int BaseMultiplier => 3;

        public override bool Matches(List<Card> cards) =>
            cards.GroupBy(c => c.Rank).Any(g => g.Count() == 3);
        public override List<Card> GetMatchedCards(List<Card> cards)
        {
            return cards
                .GroupBy(c => c.Rank)
                .Where(g => g.Count() == 3)
                .SelectMany(g => g)
                .ToList();
        }
    }
}