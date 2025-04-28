using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalatroDSL.AST;
using BalatroDSL.Models;


namespace BalatroDSL.Scoring
{
    internal class ScorableFourOfAKind : ScorableBase
    {
        public override string Label => "Four Of A Kind";
        public override int BaseChips => 60;
        public override int BaseMultiplier => 7;

        public override bool Matches(List<Card> cards) => 
            cards
                .GroupBy(c => c.Rank)
                .Any(g => g.Count() == 4);

        public override List<Card> GetMatchedCards(List<Card> cards) =>
            cards.GroupBy(c => c.Rank)
                .Where(g => g.Count() == 4)
                .SelectMany(g => g)
                .ToList();
    }
}