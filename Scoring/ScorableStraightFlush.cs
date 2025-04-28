using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BalatroDSL.AST;
using BalatroDSL.Models;

namespace BalatroDSL.Scoring
{
    internal class ScorableStraightFlush : ScorableBase
    {
        public override string Label => "Straight Flush";

        public override int BaseChips => 100;
        public override int BaseMultiplier => 8;

        public override bool Matches(List<Card> cards)
        {
            var suitedCards = cards
                .GroupBy(c => c.Suit)
                .Where(g => g.Count() >= 5)
                .SelectMany(g => g)
                .ToList();

            var values = suitedCards
                .Select(c => c.Rank)
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            if (values.Count < 5)
                return false;

            for (int i = 0; i < values.Count - 4; i++)
            {
                if (values[i + 4] - values[i] == 4 && // check for 5 consecutive values
                    values.Skip(i).Take(5).Distinct().Count() == 5)
                    return true;
            }

            return false;
        }

        public override List<Card> GetMatchedCards(List<Card> cards) =>
            // technically we could just return all 5 cards (but balatro does support hands of > 5 cards)
            cards
                .GroupBy(c => c.Suit)
                .Where(g => g.Count() >= 5)
                .SelectMany(g => g)
                .Select(c => c.Rank)
                .Distinct()
                .OrderBy(v => v)
                .ToList()
                .SelectMany(value => cards.Where(c => c.Rank == value))
                .Take(5)
                .ToList();

    }
}