using Clue_Less.Models.GameplayObjects;
using Clue_Less_Server.Managers;
using Greet;
using System.Numerics;

namespace Models.GameplayObjects
{
    public class Player
    {
        public Player(string name, int playerId, PlayerCharacterOptions character)
        {
            Name = name;
            PlayerId = playerId;            
            Character = character;
            NotificationManager.Instance.NewQueueForPlayer(PlayerId);
        }

        public string Name { get; set; }
        public int PlayerId { get; set; }
        public bool IsCurrentTurn { get; set; } = false;
        public bool IsMakingSuggestion { get; set; } = false;
        public bool IsMakingAccusation { get; set; } = false;
        public bool IsDisprovingSuggestions { get; set; } = false;
        public bool IsInHallway { get; set; } = false;
        public bool IsInRoom { get; set; } = false; 
        public bool IsBeingAccused { get; set; } = false;
        public Location PlayerLocation { get; set; }
        public PlayerCharacterOptions Character { get; set; }
        public bool HasMadeAccusation { get; set; } = false;
        public List<Card> CardsInHand { get; set; } = new List<Card>();
    }
}
