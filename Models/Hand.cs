using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Models
{
    public class Hand
    {
        public List<Card> Cards { get; set; } = new();
        public List<Joker> Jokers { get; set; } = new();


        /* TODO
        public List<string> Vouchers { get; set; } = new();
        */

        // result of the hand evaluation
        public int chips { get; set; } = 0;
        public int multiplier { get; set; } = 0;
        public string label { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Hand: [{string.Join(", ", Cards)}]";
        }
    }
}
