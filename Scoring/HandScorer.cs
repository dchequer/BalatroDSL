using BalatroDSL.AST;
using BalatroDSL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Scoring
{
    public class HandScorer
    {
        public static ASTNode BuildScoringTree(Hand hand)
        {
            var root = new ASTNode($"Hand: [{string.Join(", ", hand.Cards)}]");

            var eval = HandEvaluator.Evaluate(hand);

            if (eval.Scorable != null)
            {
                var scoringNode = eval.Scorable.Score(eval.MatchedCards, hand);
                root.AddChild(scoringNode);
            }
            else
            {
                root.AddChild(new ASTNode("No scorable hand type matched."));
            }

            // Non-scoring cards
            var miscNode = new ASTNode("Non-scoring Cards:");
            foreach (var card in eval.Leftovers)
                miscNode.AddChild(new ASTNode(card.ToString()));
            root.AddChild(miscNode);

            // Jokers
            var jokerNode = new ASTNode("Jokers:");
            if (hand.Jokers.Any())
            {
                foreach (var j in hand.Jokers)
                    jokerNode.AddChild(new ASTNode(j.ToASTString()));
            }
            else
            {
                jokerNode.AddChild(new ASTNode("None"));
            }
            root.AddChild(jokerNode);

            return root;
        }
    }
}
