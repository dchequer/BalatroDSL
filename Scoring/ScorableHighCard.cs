using BalatroDSL.AST;
using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    public class ScorableHighCard : ScorableBase
    {
        public override string Label => "High Card";
        public override int BaseChips => 5;
        public override int BaseMultiplier => 1;
        public override bool Matches(List<Card> cards) =>
            cards.Any();
        public override List<Card> GetMatchedCards(List<Card> cards)
        {
            return cards.
                OrderByDescending(c => c.Rank)
                .Take(1)
                .ToList();
        }
    }
}
