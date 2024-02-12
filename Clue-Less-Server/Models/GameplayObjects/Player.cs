using Clue_Less.Models.GameplayObjects;
using System.Numerics;

namespace Models.GameplayObjects
{
    public class Player
    {
        public string Name { get; set; }
        public bool IsCurrentTurn { get; set; } = false;
        public bool IsMakingSuggestion { get; set; } = false;
        public bool IsMakingAccusation { get; set; } = false;
        public bool IsDisprovingSuggestions { get; set; } = false;
        public bool IsInHallway { get; set; } = false;
        public bool IsInRoom { get; set; } = false; 
        public bool IsBeingAccused { get; set; } = false;
        public Vector2 PlayerLocation { get; set; }
        public int TokenId { get; set; }
        public bool HasMadeAccusation { get; set; } = false;
        public int RoomLocatedInId { get; set; }
        public List<Card> CardsInHand { get; set; } = new List<Card>();
    }
}
