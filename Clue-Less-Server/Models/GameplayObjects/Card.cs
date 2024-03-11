using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Clue_Less.Models.GameplayObjects
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool WasShown { get; set; }
        public int PlayerDealtToId { get; set; }
    }
}

