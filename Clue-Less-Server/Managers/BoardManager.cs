using Clue_Less_Server.Managers;
using Greet;
using Models.GameplayObjects;

namespace Managers
{
    public class BoardManager 
    {
        private static readonly Lazy<BoardManager> lazy = new Lazy<BoardManager>(() => new BoardManager());
        public static BoardManager Instance { get { return lazy.Value; } }

        private List<Player> PlayerList = new List<Player>();
        private int PlayerIdCounter = 1;
        private Solution Solution = new Solution();
        Random rng = new Random();
        private Dictionary<Location, List<Location>> BoardMap = new Dictionary<Location, List<Location>>();

        public BoardManager() {
        }

        public void StartGame()
        {
            GenerateSolution();            
            DealCards();
            MovePlayersToStartingPositions();
            InitializeBoardMap();
            
            //Tell all players all player locations as well as the fact that the game has started.
            HeartbeatResponse startGameResponse = new HeartbeatResponse();
            startGameResponse.Response = ServerHeartbeatResponse.StartGame;
            startGameResponse.StartGame = new StartGameResponse();
            Console.WriteLine($"StartGame!");
            foreach (var player in PlayerList)
            {
                startGameResponse.StartGame.PlayerId.Add(player.PlayerId);
                startGameResponse.StartGame.PlayerLocation.Add(player.PlayerLocation);
                startGameResponse.StartGame.PlayerCharacter.Add(player.Character);
                Console.WriteLine($"Player {player.PlayerId} is playing {player.Character}, starting at location {player.PlayerLocation}");
            }
            
            NotificationManager.Instance.SendGlobalMessage(startGameResponse);
            HeartbeatResponse  currentPlayerTurn = new HeartbeatResponse();
            currentPlayerTurn.Response = ServerHeartbeatResponse.CurrentTurn;
            currentPlayerTurn.CurrentTurn = new CurrentTurnResponse();
            currentPlayerTurn.CurrentTurn.PlayerId = PlayerList.First().PlayerId;
            NotificationManager.Instance.SendGlobalMessage(currentPlayerTurn);
        }

        private void MovePlayersToStartingPositions()
        {
            foreach (var player in PlayerList)
            {
                switch(player.Character)
                {
                    case PlayerCharacterOptions.MrsPeacock:
                        {
                            player.PlayerLocation = Location.MrsPeacockHomeSquare;
                        }
                        break;
                    case PlayerCharacterOptions.ProfessorPlum:
                        {
                            player.PlayerLocation = Location.ProfessorPlumHomeSquare;
                        }
                        break;
                    case PlayerCharacterOptions.MissScarlet:
                        {
                            player.PlayerLocation = Location.MissScarletHomeSquare;
                        }
                        break;
                    case PlayerCharacterOptions.ColMustard:
                        {
                            player.PlayerLocation = Location.ColMustardHomeSquare;
                        }
                        break;
                    case PlayerCharacterOptions.MrsWhite:
                        {
                            player.PlayerLocation = Location.MrsWhiteHomeSquare;
                        }
                        break;
                    case PlayerCharacterOptions.MrGreen:
                        {
                            player.PlayerLocation = Location.MrGreenHomeSquare;
                        }
                        break;
                }
            }
        }

        private void DealCards()
        {
            var shuffledCards = InitializeCardsToDeal();
            int i = 0;
            int playerCount = PlayerList.Count;
            foreach (var card in shuffledCards)
            {
                PlayerList[i].CardsInHand.Add(card);
                i++;
                if (i == playerCount)
                {
                    i = 0;
                }
            }
        }

        public List<Location> GetValidMoveLocations(Player player)
        {
            var locationsToDelete = new List<Location>();
            var validPlayerLocations = BoardMap.FirstOrDefault(x => x.Key == player.PlayerLocation);

            foreach (var possibleLocation in validPlayerLocations.Value)
            {
                if (IsHallway(possibleLocation))
                {
                    if (IsOccupied(possibleLocation))
                    {
                        locationsToDelete.Add(possibleLocation);
                    }
                }
            }

            if (locationsToDelete.Count > 0)
            {
                foreach (var possibleLocation in locationsToDelete)
                {
                    validPlayerLocations.Value.Remove(possibleLocation);
                }
            }

            return validPlayerLocations.Value;
        }        

        private List<Card> InitializeCardsToDeal()
        {
            var cards = new List<Card>();
            foreach(PlayerCharacterOptions character in Enum.GetValues(typeof(PlayerCharacterOptions)))
            {
                var newCard = new Card();
                newCard.Type = CardType.CharacterCard;
                newCard.Character = character;
                if (character != Solution.Murderer)
                {
                    cards.Add(newCard);
                }                
            }
            foreach (WeaponTokenEnum weapon in Enum.GetValues(typeof(WeaponTokenEnum)))
            {
                var newCard = new Card();
                newCard.Type = CardType.WeaponCard;
                newCard.Weapon = weapon;
                if (weapon != Solution.MurderWeapon)
                {
                    cards.Add(newCard);
                }
            }
            foreach (MurderRoomsEnum location in Enum.GetValues(typeof(MurderRoomsEnum)))
            {
                var newCard = new Card();
                newCard.Type = CardType.MurderRoomCard;
                newCard.MurderRoom = location;
                if (location != Solution.MurderRoom)
                {
                    cards.Add(newCard);
                }
            }            
            var shuffledCards = cards.OrderBy(_ => rng.Next()).ToList();
            shuffledCards.ForEach(c => Console.WriteLine("Shuffled cards " + c.Type));
            return shuffledCards;
        }

        public bool CheckPlayerCards(int playerId, Card playerCardToCheck)
        {
            var player = PlayerList.FirstOrDefault(x => x.PlayerId == playerId);
            if (player != null)
            {
                return player.CardsInHand.Contains(playerCardToCheck);
            }
            return false;
        }

        public Location MovePlayer(int playerId, Location moveToPosition)
        {
            //You'll need to write validation code that makes sure this is a valid move, track the player, etc.
            //for now, we just say yep! Go there.
            return moveToPosition;
        }

        public List<int> GetPlayerTurnOrder()
        {
            List<int> result = new List<int>();
            foreach(Player player in PlayerList)
            {
                result.Add(player.PlayerId);
            }
            return result;
        }

        public LoginReply AttemptLogin(string playerName, PlayerCharacterOptions character)
        {
            var LoginResult = new LoginReply
            {
                Success = true
            };

            foreach (var player in PlayerList)
            {
                if (player.Character == character)
                {
                    LoginResult.Success = false;
                }
            }
            if (LoginResult.Success)
            {
                var newPlayer = new Player(playerName, PlayerIdCounter++, character);
                LoginResult.PlayerId = newPlayer.PlayerId;
                PlayerList.Add(newPlayer);                
            }
            return LoginResult;
        }

        private void GenerateSolution()
        {
            Solution = new Solution();
            Solution.Murderer = (PlayerCharacterOptions)rng.Next(0, Enum.GetNames(typeof(PlayerCharacterOptions)).Length);
            Solution.MurderWeapon = (WeaponTokenEnum)rng.Next(0, Enum.GetNames(typeof(WeaponTokenEnum)).Length);
            Solution.MurderRoom = (MurderRoomsEnum)rng.Next(0, Enum.GetNames(typeof(MurderRoomsEnum)).Length);

            Console.WriteLine($"Solution! It was {Solution.Murderer } in the {Solution.MurderRoom} with the {Solution.MurderWeapon}!");
        }

        public SolutionResponse GetSolution(int requestingPlayerId, MurderRoomsEnum suspectedLocation, PlayerCharacterOptions suspectedCharacter, WeaponTokenEnum suspectedWeapon)
        {
            //validate whether it is the requesting player's turn
            //and that they have not made an accusation previously
            var Result = new SolutionResponse();
            Result.Correct = true;
            if (Solution.MurderWeapon != suspectedWeapon)
            {
                Result.Correct = false;
            }
            if (Solution.Murderer != suspectedCharacter)
            {
                Result.Correct = false;
            }
            if (Solution.MurderRoom != suspectedLocation)
            {
                Result.Correct = false;
            }
            return Result;
        }

        private bool IsHallway(Location location)
        {
            var hallwayLocations = GetHallwayLocations();
            return hallwayLocations.Contains(location);
        }

        private bool IsOccupied(Location hallway)
        {
            return PlayerList.Any(x => x.PlayerLocation == hallway);
        }

        private void InitializeBoardMap()
        {
            //Home squares only have one valid location to move to.
            BoardMap[Location.ProfessorPlumHomeSquare] = new List<Location>();
            BoardMap[Location.ProfessorPlumHomeSquare].Add(Location.HallwayThree);

            BoardMap[Location.MrsPeacockHomeSquare] = new List<Location>();
            BoardMap[Location.MrsPeacockHomeSquare].Add(Location.HallwayEight);

            BoardMap[Location.MrGreenHomeSquare] = new List<Location>();
            BoardMap[Location.MrGreenHomeSquare].Add(Location.HallwayEleven);

            BoardMap[Location.MrsWhiteHomeSquare] = new List<Location>();
            BoardMap[Location.MrsWhiteHomeSquare].Add(Location.HallwayTwelve);

            BoardMap[Location.ColMustardHomeSquare] = new List<Location>();
            BoardMap[Location.ColMustardHomeSquare].Add(Location.HallwayFive);

            BoardMap[Location.MissScarletHomeSquare] = new List<Location>();
            BoardMap[Location.MissScarletHomeSquare].Add(Location.HallwayTwo);

            //Actual valid board movements.
            BoardMap[Location.Study] = new List<Location>();
            BoardMap[Location.Study].Add(Location.HallwayOne);
            BoardMap[Location.Study].Add(Location.HallwayThree);
            BoardMap[Location.Study].Add(Location.Kitchen);

            BoardMap[Location.HallwayOne] = new List<Location>();
            BoardMap[Location.HallwayOne].Add(Location.Study);
            BoardMap[Location.HallwayOne].Add(Location.ConcertHall);

            BoardMap[Location.ConcertHall] = new List<Location>();
            BoardMap[Location.ConcertHall].Add(Location.HallwayOne);
            BoardMap[Location.ConcertHall].Add(Location.HallwayTwo);
            BoardMap[Location.ConcertHall].Add(Location.HallwayFour);

            BoardMap[Location.HallwayTwo] = new List<Location>();
            BoardMap[Location.HallwayTwo].Add(Location.ConcertHall);
            BoardMap[Location.HallwayTwo].Add(Location.Lounge);

            BoardMap[Location.Lounge] = new List<Location>();
            BoardMap[Location.Lounge].Add(Location.HallwayFive);
            BoardMap[Location.Lounge].Add(Location.HallwayTwo);
            BoardMap[Location.Lounge].Add(Location.Conservatory);

            BoardMap[Location.HallwayTwo] = new List<Location>();
            BoardMap[Location.HallwayTwo].Add(Location.ConcertHall);
            BoardMap[Location.HallwayTwo].Add(Location.Lounge);

            BoardMap[Location.HallwayThree] = new List<Location>();
            BoardMap[Location.HallwayThree].Add(Location.Study);
            BoardMap[Location.HallwayThree].Add(Location.Library);

            BoardMap[Location.HallwayFour] = new List<Location>();
            BoardMap[Location.HallwayFour].Add(Location.ConcertHall);
            BoardMap[Location.HallwayFour].Add(Location.Billiard);

            BoardMap[Location.HallwayFive] = new List<Location>();
            BoardMap[Location.HallwayFive].Add(Location.Lounge);
            BoardMap[Location.HallwayFive].Add(Location.DiningRoom);

            BoardMap[Location.Library] = new List<Location>();
            BoardMap[Location.Library].Add(Location.HallwayThree);
            BoardMap[Location.Library].Add(Location.HallwayFive);
            BoardMap[Location.Library].Add(Location.HallwayEight);

            BoardMap[Location.HallwaySix] = new List<Location>();
            BoardMap[Location.HallwaySix].Add(Location.Library);
            BoardMap[Location.HallwaySix].Add(Location.Billiard);

            BoardMap[Location.Billiard] = new List<Location>();
            BoardMap[Location.Billiard].Add(Location.HallwayFour);
            BoardMap[Location.Billiard].Add(Location.HallwaySix);
            BoardMap[Location.Billiard].Add(Location.HallwaySeven);
            BoardMap[Location.Billiard].Add(Location.HallwayNine);

            BoardMap[Location.HallwaySeven] = new List<Location>();
            BoardMap[Location.HallwaySeven].Add(Location.DiningRoom);
            BoardMap[Location.HallwaySeven].Add(Location.Billiard);

            BoardMap[Location.HallwayEight] = new List<Location>();
            BoardMap[Location.HallwayEight].Add(Location.Library);
            BoardMap[Location.HallwayEight].Add(Location.Conservatory);

            BoardMap[Location.HallwayNine] = new List<Location>();
            BoardMap[Location.HallwayNine].Add(Location.Billiard); ;
            BoardMap[Location.HallwayNine].Add(Location.Ballroom);

            BoardMap[Location.HallwayTen] = new List<Location>();
            BoardMap[Location.HallwayTen].Add(Location.DiningRoom); 
            BoardMap[Location.HallwayTen].Add(Location.Kitchen);

            BoardMap[Location.Conservatory] = new List<Location>();
            BoardMap[Location.Conservatory].Add(Location.HallwayEight); ;
            BoardMap[Location.Conservatory].Add(Location.HallwayEleven);
            BoardMap[Location.Conservatory].Add(Location.Lounge);

            BoardMap[Location.HallwayEleven] = new List<Location>();
            BoardMap[Location.HallwayEleven].Add(Location.Conservatory);
            BoardMap[Location.HallwayEleven].Add(Location.Ballroom);

            BoardMap[Location.Ballroom] = new List<Location>();
            BoardMap[Location.Ballroom].Add(Location.HallwayNine); ;
            BoardMap[Location.Ballroom].Add(Location.HallwayEleven);
            BoardMap[Location.Ballroom].Add(Location.HallwayTwelve);

            BoardMap[Location.HallwayTwelve] = new List<Location>();
            BoardMap[Location.HallwayTwelve].Add(Location.Kitchen);
            BoardMap[Location.HallwayTwelve].Add(Location.Ballroom);

            BoardMap[Location.Kitchen] = new List<Location>();
            BoardMap[Location.Kitchen].Add(Location.HallwayTwelve);
            BoardMap[Location.Kitchen].Add(Location.HallwayTen);
            BoardMap[Location.Kitchen].Add(Location.Study);
        }

        private List<Location> GetHallwayLocations()
        {
            List<Location> locations = new List<Location>();

            locations.Add(Location.HallwayOne);
            locations.Add(Location.HallwayTwo);
            locations.Add(Location.HallwayThree);
            locations.Add(Location.HallwayFour);
            locations.Add(Location.HallwayFive);
            locations.Add(Location.HallwaySix);
            locations.Add(Location.HallwaySeven);
            locations.Add(Location.HallwayEight);
            locations.Add(Location.HallwayNine);
            locations.Add(Location.HallwayTen);
            locations.Add(Location.HallwayEleven);
            locations.Add(Location.HallwayTwelve);

            return locations;
        }
    }
}
