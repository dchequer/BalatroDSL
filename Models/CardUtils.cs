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
    }
}
