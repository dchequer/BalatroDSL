using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalatroDSL.AST;
using BalatroDSL.Models;


namespace BalatroDSL.Scoring
{
    internal class ScorableFlushHouse : ScorableBase
    {
        public override string Label => "Flush House";

        public override int BaseChips => 140;
        public override int BaseMultiplier => 14;

        public override bool Matches(List<Card> cards) { 
            var suitedGroups = cards
                                .GroupBy(c => c.Suit)
                                .Where(g => g.Count() >= 5)
                                .ToList();

            foreach (var group in suitedGroups)
            {
                var rankGroups = group
                                    .GroupBy(c => c.Rank)
                                    .ToList();
                if (rankGroups.Any(g => g.Count() == 3) &&
                    rankGroups.Any(g => g.Count() == 2))
                    return true;
            }

            return false;
        }


        public override List<Card> GetMatchedCards(List<Card> cards)
        {
            var suitedGroups = cards
                                .GroupBy(c => c.Suit)
                                .Where(g => g.Count() >= 5)
                                .ToList();
            foreach (var group in suitedGroups)
            {
                var rankGroups = group
                                    .GroupBy(c => c.Rank)
                                    .ToList();
                var threeOfAKindGroup = rankGroups.First(g => g.Count() == 3);
                var pairGroup = rankGroups.First(g => g.Count() == 2);
                return [.. threeOfAKindGroup, .. pairGroup];
            }

            return new List<Card>();
        }
    }
}