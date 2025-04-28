using BalatroDSL.AST;
using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    public class ScorableStraight : ScorableBase
    {
        public override string Label => "Straight";

        public override int BaseChips => 30;
        public override int BaseMultiplier => 4;

        public override bool Matches(List<Card> cards)
        {
            // get distinct ranks and sort them
            var distinctRanks = cards
                .Select(c => c.Rank)
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            // early exit if we dont even have enough ranks
            if (distinctRanks.Count < 5)
                return false;

            return distinctRanks
            .Select((rank, index) => distinctRanks.Skip(index).Take(5)) // Create sliding windows of 5
            .Where(window => window.Count() == 5)                      // Ensure the window has 5 elements
            .Any(window => window.Zip(window.Skip(1), (a, b) => b - a).All(diff => diff == 1)); // Check consecutive
        }

        public override List<Card> GetMatchedCards(List<Card> cards)
        {
            var distinctSorted = cards
                .GroupBy(c => c.Rank)
                .Select(g => g.First())
                .OrderBy(c => c.Rank)
                .ToList();

            return Enumerable.Range(0, distinctSorted.Count - 4)
                .Select(i => distinctSorted.Skip(i).Take(5).ToList())
                .Where(window => window.Count == 5)
                .FirstOrDefault(window =>
                    window.Zip(window.Skip(1), (a, b) => (int)b.Rank - (int)a.Rank)
                          .All(diff => diff == 1))
                ?? new List<Card>();
        }


    }
}