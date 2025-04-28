using BalatroDSL.AST;
using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    public interface IScorable
    {
        bool Matches(List<Card> cards);
        List<Card> GetMatchedCards(List<Card> cards);
        ASTNode Score(List<Card> matchedCards, Hand hand);
        string Label { get; }

        int BaseChips { get; }
        int BaseMultiplier { get; }
    }
}
