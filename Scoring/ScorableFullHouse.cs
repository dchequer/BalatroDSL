using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalatroDSL.AST;
using BalatroDSL.Models;

namespace BalatroDSL.Scoring
{
    internal class ScorableFullHouse : ScorableBase
    {
        public override string Label => "Full House";

        public override int BaseChips => 40;
        public override int BaseMultiplier => 4;

        public override bool Matches(List<Card> cards) {
            var groups = cards
                            .GroupBy(c => c.Rank)
                            .ToList();
            return groups.Any(g => g.Count() == 3) && 
                   groups.Any(g => g.Count() == 2);
        }
        

        public override List<Card> GetMatchedCards(List<Card> cards)
        {
            var rankGroups = cards.GroupBy(c => c.Rank).ToList();
            var threeOfAKindGroup = rankGroups.First(g => g.Count() == 3);
            var pairGroup = rankGroups.First(g => g.Count() == 2);
            return [.. threeOfAKindGroup, .. pairGroup];
        }
    }
}