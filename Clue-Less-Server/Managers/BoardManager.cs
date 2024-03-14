using Greet;
using Models.GameplayObjects;

namespace Managers
{
    public class BoardManager 
    {
        private static readonly Lazy<BoardManager> lazy = new Lazy<BoardManager>(() => new BoardManager());
        public static BoardManager Instance { get { return lazy.Value; } }

        private List<Player> playerList = new List<Player>();
        private int playerIdCounter = 0;
        private Solution solution = new Solution();
        Random rng = new Random();

        public BoardManager() {
        }

        public void StartGame()
        {
            GenerateSolution();            
            DealCards();
        }

        private void DealCards()
        {
            var shuffledCards = InitializeCardsToDeal();
            int i = 0;
            int playerCount = playerList.Count;
            foreach (var card in shuffledCards)
            {
                playerList[i].CardsInHand.Add(card);
                i++;
                if (i == playerCount)
                {
                    i = 0;
                }
            }
        }

        private List<Card> InitializeCardsToDeal()
        {
            var cards = new List<Card>();
            foreach(PlayerCharacterOptions character in Enum.GetValues(typeof(PlayerCharacterOptions)))
            {
                var newCard = new Card();
                newCard.Type = CardType.CharacterCard;
                newCard.Character = character;
                if (character != solution.Murderer)
                {
                    cards.Add(newCard);
                }                
            }
            foreach (WeaponTokenEnum weapon in Enum.GetValues(typeof(WeaponTokenEnum)))
            {
                var newCard = new Card();
                newCard.Type = CardType.WeaponCard;
                newCard.Weapon = weapon;
                if (weapon != solution.MurderWeapon)
                {
                    cards.Add(newCard);
                }
            }
            foreach (MurderRoomsEnum location in Enum.GetValues(typeof(MurderRoomsEnum)))
            {
                var newCard = new Card();
                newCard.Type = CardType.MurderRoomCard;
                newCard.MurderRoom = location;
                if (location != solution.MurderRoom)
                {
                    cards.Add(newCard);
                }
            }            
            var shuffledCards = cards.OrderBy(_ => rng.Next()).ToList();
            return shuffledCards;
        }

        //This is the real method but won't be hooked up for skeletal increment 
        //public bool CheckPlayerCards(int playerId, Card playerCardToCheck)
        //{
        //    var player = playerList.FirstOrDefault(x => x.PlayerId == playerId);
        //    if (player != null)
        //    {
        //        return player.CardsInHand.Contains(playerCardToCheck);
        //    }
        //    return false;
        //}

        public string CheckPlayerCards(bool hasInHand)
        {
            if (hasInHand)
            {
                return "Suggestion Disproven! This card is in Player_X's hand!";
            }
            return "This suggestion cannot be disproven!";
        }

        public Location MovePlayer(int playerId, Location moveToPosition)
        {
            //You'll need to write validation code that makes sure this is a valid move, track the player, etc.
            //for now, we just say yep! Go there.
            return moveToPosition;
        }

        public LoginReply AttemptLogin(string playerName, PlayerCharacterOptions character)
        {
            var LoginResult = new LoginReply
            {
                Success = true
            };
            foreach (var player in playerList)
            {
                if (player.Character == character)
                {
                    LoginResult.Success = false;
                }
            }
            if (LoginResult.Success)
            {
                var newPlayer = new Player(playerName, playerIdCounter++, character);
                LoginResult.PlayerId = newPlayer.PlayerId;
                playerList.Add(newPlayer);
            }
            return LoginResult;
        }

        private void GenerateSolution()
        {
            solution = new Solution();
            solution.Murderer = (PlayerCharacterOptions)rng.Next(0, Enum.GetNames(typeof(PlayerCharacterOptions)).Length);
            solution.MurderWeapon = (WeaponTokenEnum)rng.Next(0, Enum.GetNames(typeof(WeaponTokenEnum)).Length);
            solution.MurderRoom = (MurderRoomsEnum)rng.Next(0, Enum.GetNames(typeof(MurderRoomsEnum)).Length);

            Console.WriteLine($"Solution! It was { solution.Murderer } in the {solution.MurderRoom} with the {solution.MurderWeapon}!");
        }

        public SolutionResponse GetSolution(int requestingPlayerId, MurderRoomsEnum suspectedLocation, PlayerCharacterOptions suspectedCharacter, WeaponTokenEnum suspectedWeapon)
        {
            //validate whether it is the requesting player's turn
            //and that they have not made an accusation previously
            var Result = new SolutionResponse();
            Result.Correct = true;
            if(solution.MurderWeapon != suspectedWeapon)
            {
                Result.Correct = false;
            }
            if(solution.Murderer != suspectedCharacter)
            {
                Result.Correct = false;
            }
            if(solution.MurderRoom != suspectedLocation)
            {
                Result.Correct = false;
            }
            return Result;
        }
    }
}
