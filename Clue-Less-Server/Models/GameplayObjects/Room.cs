using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Clue_Less.Models.GameplayObjects
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector2 RoomLocation { get; set; }
    }
}
