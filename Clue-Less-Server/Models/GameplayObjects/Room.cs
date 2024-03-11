using System.Numerics;

namespace Clue_Less.Models.GameplayObjects
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector2 RoomLocation { get; set; }
    }
}
