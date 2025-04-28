using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Models
{

    public class CardUtils
    {
        public static int Rank2Chips(Card card)
        {
            return card.Rank switch
            {
                Rank.Ace => 11,
                Rank.King => 10,
                Rank.Queen => 10,
                Rank.Jack => 10,
                _ => (int)card.Rank + 2,
            };
        }

        public static bool CardMatchesTrigger(Card card, string triggerTarget)
        {
            if (card == null || string.IsNullOrEmpty(triggerTarget))
                return false;

            switch (triggerTarget)
            {
                case "2":
                    return card.Rank == Rank.Two;
                case "3":
                    return card.Rank == Rank.Three;
                case "4":
                    return card.Rank == Rank.Four;
                case "5":
                    return card.Rank == Rank.Five;
                case "6":
                    return card.Rank == Rank.Six;
                case "7":
                    return card.Rank == Rank.Seven;
                case "8":
                    return card.Rank == Rank.Eight;
                case "9":
                    return card.Rank == Rank.Nine;
                case "10":
                    return card.Rank == Rank.Ten;
                case "J":
                    return card.Rank == Rank.Jack;
                case "Q":
                    return card.Rank == Rank.Queen;
                case "K":
                    return card.Rank == Rank.King;
                case "A":
                    return card.Rank == Rank.Ace;

                case "H":
                case "D":
                case "S":
                case "C":
                    return card.Suit.ToString().StartsWith(triggerTarget);

                default:
                    return false;
            }
        }
    }
}
