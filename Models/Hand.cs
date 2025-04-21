using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BalatroDSL.Models
{
    internal class Hand
    {
        public List<Card> Cards { get; set; } = new();


        /* TODO
        public List<string> Jokers { get; set; } = new();
        public List<string> Enhancements { get; set; } = new();
        public List<string> Vouchers { get; set; } = new();
        */

        public override string ToString()
        {
            return $"Hand: [{string.Join(", ", Cards)}]";
        }
    }
}
