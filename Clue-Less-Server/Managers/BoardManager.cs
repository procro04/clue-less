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
        private Dictionary<Location, List<Location>> SuggestionBoardMap = new Dictionary<Location, List<Location>>();
        private int CurrentPlayerTurnId = 1;

        public BoardManager() {
        }

        public void StartGame()
        {
            GenerateSolution();            
            DealCards();
            MovePlayersToStartingPositions();
            InitializeBoardMap();
            InitializeSuggestionBoardMap();
            
            //Tell all players all player locations as well as the fact that the game has started.
            HeartbeatResponse startGameResponse = new HeartbeatResponse();
            startGameResponse.Response = ServerHeartbeatResponse.StartGame;
            startGameResponse.StartGame = new StartGameResponse();

            foreach (var player in PlayerList)
            {
                startGameResponse.StartGame.PlayerId.Add(player.PlayerId);
                startGameResponse.StartGame.PlayerLocation.Add(player.PlayerLocation);
                startGameResponse.StartGame.PlayerCharacter.Add(player.Character);
            }
            
            CurrentPlayerTurnId = PlayerList.First().PlayerId;
            SendCurrentTurnToPlayers();
            NotificationManager.Instance.SendGlobalMessage(startGameResponse);           
        }

        public bool ValidMoveRequest(Player player, Location moveToPosition)
        {
            var validMoveLocations = GetValidMoveLocations(player);

            if (!validMoveLocations.Contains(moveToPosition))
            {                
                return false;
            }
            return true;
        }

        public Player GetPlayerFromId(int playerId)
        {
            var player = PlayerList.First(x => x.PlayerId == playerId);
            return player;
        }

        public Location MovePlayer(int playerId, Location moveToPosition)
        {
            var player = GetPlayerFromId(playerId);

            if (!ValidMoveRequest(player, moveToPosition))
            {
                HeartbeatResponse invalidMoveResponse = new HeartbeatResponse();
                invalidMoveResponse.Response = ServerHeartbeatResponse.Error;
                invalidMoveResponse.ErrorMessage = new ErrorMessageResponse();
                invalidMoveResponse.ErrorMessage.Message = "Invalid move - please try again";
                NotificationManager.Instance.SendPlayerMessage(playerId, invalidMoveResponse);

                return player.PlayerLocation;
            }
            else
            {
                player.PlayerLocation = moveToPosition;
                return moveToPosition;
            }
        }

        public List<Location> GetValidMoveLocations(Player player, bool beingMovedForSuggestion = false)
        {
            var validPlayerLocations = BoardMap.FirstOrDefault(x => x.Key == player.PlayerLocation);

            if (!beingMovedForSuggestion)
            {
                var locationsToDelete = new List<Location>();
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
            }
            else
            {
                validPlayerLocations = SuggestionBoardMap.FirstOrDefault(x => x.Key == player.PlayerLocation);
            }

            if (validPlayerLocations.Value.Count == 0)
            {
                HeartbeatResponse invalidMoveResponse = new HeartbeatResponse();
                invalidMoveResponse.Response = ServerHeartbeatResponse.Error;
                invalidMoveResponse.ErrorMessage = new ErrorMessageResponse();
                invalidMoveResponse.ErrorMessage.Message = "There is nowhere for you to move at this time.";
                NotificationManager.Instance.SendPlayerMessage(player.PlayerId, invalidMoveResponse);
            }

            return validPlayerLocations.Value;
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

        public List<int> GetPlayerTurnOrder()
        {
            List<int> result = new List<int>();
            foreach (Player player in PlayerList)
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

        public void AdvancePlayerTurn()
        {
            CurrentPlayerTurnId++;

            if (CurrentPlayerTurnId > PlayerList.Count)
            {
                CurrentPlayerTurnId = 1;
            }

            SendCurrentTurnToPlayers();
        }

        public SolutionResponse GetSolution(int requestingPlayerId, MurderRoomsEnum suspectedLocation, PlayerCharacterOptions suspectedCharacter, WeaponTokenEnum suspectedWeapon)
        {
            var result = new SolutionResponse();
            result.Correct = true;
            HeartbeatResponse solutionResponse = new HeartbeatResponse();
            solutionResponse.Response = ServerHeartbeatResponse.Error;
            solutionResponse.ErrorMessage = new ErrorMessageResponse();

            if (solutionResponse.CurrentTurn.PlayerId != requestingPlayerId)
            {
                solutionResponse.ErrorMessage.Message = "It is not currently your turn, therefore you cannot make an accusation.";
                result.Correct = false;
                return result;
            }

            var player = PlayerList.First(x => x.PlayerId == requestingPlayerId);
            if (player.HasMadeAccusation)
            {
                solutionResponse.ErrorMessage.Message = "You have already made an accusation - you can only make one accusation per game.";
                result.Correct = false;
                return result;
            }

            if (Solution.MurderWeapon != suspectedWeapon)
            {
                result.Correct = false;
            }
            if (Solution.Murderer != suspectedCharacter)
            {
                result.Correct = false;
            }
            if (Solution.MurderRoom != suspectedLocation)
            {
                result.Correct = false;
            }

            if (!result.Correct)
            {
                solutionResponse.ErrorMessage.Message = "Sorry, you have made an incorrect accusation. \n " +
                    $"The murderer is {result.Murderer} in  the {result.MurderRoom} with the {result.MurderWeapon}. " +
                    "You will not be able to make any more accusations for the remainder of this game.";
            }
            else
            {
                solutionResponse.ErrorMessage.Message = "Congratulations! You solved the mystery!";
            }

            NotificationManager.Instance.SendPlayerMessage(requestingPlayerId, solutionResponse);
            return result;
        }


        #region --------------------- Private Methods ------------------------
        private void MovePlayersToStartingPositions()
        {
            foreach (var player in PlayerList)
            {
                switch (player.Character)
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

        private void SendCurrentTurnToPlayers()
        {
            HeartbeatResponse currentPlayerTurn = new HeartbeatResponse();
            currentPlayerTurn.Response = ServerHeartbeatResponse.CurrentTurn;
            currentPlayerTurn.CurrentTurn = new CurrentTurnResponse();
            currentPlayerTurn.CurrentTurn.PlayerId = CurrentPlayerTurnId;
            NotificationManager.Instance.SendGlobalMessage(currentPlayerTurn);
        }

        private void GenerateSolution()
        {
            Solution = new Solution();
            Solution.Murderer = (PlayerCharacterOptions)rng.Next(0, Enum.GetNames(typeof(PlayerCharacterOptions)).Length);
            Solution.MurderWeapon = (WeaponTokenEnum)rng.Next(0, Enum.GetNames(typeof(WeaponTokenEnum)).Length);
            Solution.MurderRoom = (MurderRoomsEnum)rng.Next(0, Enum.GetNames(typeof(MurderRoomsEnum)).Length);
        }

        private List<Card> InitializeCardsToDeal()
        {
            var cards = new List<Card>();
            foreach (PlayerCharacterOptions character in Enum.GetValues(typeof(PlayerCharacterOptions)))
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
            return shuffledCards;
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
            BoardMap[Location.ProfessorPlumHomeSquare] = new List<Location>
            {
                Location.HallwayThree
            };

            BoardMap[Location.MrsPeacockHomeSquare] = new List<Location>
            {
                Location.HallwayEight
            };

            BoardMap[Location.MrGreenHomeSquare] = new List<Location>
            {
                Location.HallwayEleven
            };

            BoardMap[Location.MrsWhiteHomeSquare] = new List<Location>
            {
                Location.HallwayTwelve
            };

            BoardMap[Location.ColMustardHomeSquare] = new List<Location>
            {
                Location.HallwayFive
            };

            BoardMap[Location.MissScarletHomeSquare] = new List<Location>
            {
                Location.HallwayTwo
            };

            //Actual valid board movements.
            BoardMap[Location.Study] = new List<Location>
            {
                Location.HallwayOne,
                Location.HallwayThree,
                Location.Kitchen
            };

            BoardMap[Location.HallwayOne] = new List<Location>
            {
                Location.Study,
                Location.ConcertHall
            };

            BoardMap[Location.ConcertHall] = new List<Location>
            {
                Location.HallwayOne,
                Location.HallwayTwo,
                Location.HallwayFour
            };

            BoardMap[Location.HallwayTwo] = new List<Location>
            {
                Location.ConcertHall,
                Location.Lounge
            };

            BoardMap[Location.Lounge] = new List<Location>
            {
                Location.HallwayFive,
                Location.HallwayTwo,
                Location.Conservatory
            };

            BoardMap[Location.HallwayTwo] = new List<Location>
            {
                Location.ConcertHall,
                Location.Lounge
            };

            BoardMap[Location.HallwayThree] = new List<Location>
            {
                Location.Study,
                Location.Library
            };

            BoardMap[Location.HallwayFour] = new List<Location>
            {
                Location.ConcertHall,
                Location.Billiard
            };

            BoardMap[Location.HallwayFive] = new List<Location>
            {
                Location.Lounge,
                Location.DiningRoom
            };

            BoardMap[Location.Library] = new List<Location>
            {
                Location.HallwayThree,
                Location.HallwayFive,
                Location.HallwayEight
            };

            BoardMap[Location.HallwaySix] = new List<Location>
            {
                Location.Library,
                Location.Billiard
            };

            BoardMap[Location.Billiard] = new List<Location>
            {
                Location.HallwayFour,
                Location.HallwaySix,
                Location.HallwaySeven,
                Location.HallwayNine
            };

            BoardMap[Location.HallwaySeven] = new List<Location>
            {
                Location.DiningRoom,
                Location.Billiard
            };

            BoardMap[Location.HallwayEight] = new List<Location>
            {
                Location.Library,
                Location.Conservatory
            };

            BoardMap[Location.HallwayNine] = new List<Location>
            {
                Location.Billiard,
                Location.Ballroom
            };

            BoardMap[Location.HallwayTen] = new List<Location>
            {
                Location.DiningRoom,
                Location.Kitchen
            };

            BoardMap[Location.Conservatory] = new List<Location>
            {
                Location.HallwayEight,
                Location.HallwayEleven,
                Location.Lounge
            };

            BoardMap[Location.HallwayEleven] = new List<Location>
            {
                Location.Conservatory,
                Location.Ballroom
            };

            BoardMap[Location.Ballroom] = new List<Location>
            {
                Location.HallwayNine,
                Location.HallwayEleven,
                Location.HallwayTwelve
            };
;
            BoardMap[Location.HallwayTwelve] = new List<Location>
            {
                Location.Kitchen,
                Location.Ballroom
            };

            BoardMap[Location.Kitchen] = new List<Location>
            {
                Location.HallwayTwelve,
                Location.HallwayTen,
                Location.Study
            };
        }

        private void InitializeSuggestionBoardMap()
        {
            //You can go to any room from any other room if you are being dragged there.
            var roomList = new List<Location>
            { 
                Location.Billiard,
                Location.Ballroom,
                Location.ConcertHall,
                Location.Conservatory,
                Location.DiningRoom,
                Location.Kitchen,
                Location.Library,
                Location.Lounge,
                Location.Study
            };

            SuggestionBoardMap[Location.Study] = roomList;
            SuggestionBoardMap[Location.ConcertHall] = roomList;
            SuggestionBoardMap[Location.Lounge] = roomList;
            SuggestionBoardMap[Location.Library] = roomList;
            SuggestionBoardMap[Location.Billiard] = roomList;
            SuggestionBoardMap[Location.Conservatory] = roomList;
            SuggestionBoardMap[Location.Ballroom] = roomList;
            SuggestionBoardMap[Location.Kitchen] = roomList;
        }

        private List<Location> GetHallwayLocations()
        {
            List<Location> locations = new List<Location>
            {
                Location.HallwayOne,
                Location.HallwayTwo,
                Location.HallwayThree,
                Location.HallwayFour,
                Location.HallwayFive,
                Location.HallwaySix,
                Location.HallwaySeven,
                Location.HallwayEight,
                Location.HallwayNine,
                Location.HallwayTen,
                Location.HallwayEleven,
                Location.HallwayTwelve
            };

            return locations;
        }
        #endregion
    }
}
