using Clue_Less_Server.Managers.Interfaces;
using Greet;
using Models.GameplayObjects;
using System.Collections;
using System.ComponentModel;

namespace Clue_Less_Server.Managers
{
    public class BoardManager : IBoardManager
    {
        private static readonly Lazy<BoardManager> lazy = new Lazy<BoardManager>(() => new BoardManager());
        public static BoardManager Instance { get { return lazy.Value; } }

        private List<int> boardPosition;
        private int playerPosition;
        private List<Player> playerList = new List<Player>();
        private int playerIdCounter = 0;
        public BoardManager() {
            boardPosition = new List<int> { 0, 1, 2 };
            playerPosition = 0;
        }

        public int GetPlayerLocation()
        {
            return playerPosition; 
        }
        
        public Greet.Location MovePlayer(int playerId, Greet.Location moveToPosition)
        {
            //You'll need to write validation code that makes sure this is a valid move, track the player, etc.
            //for now, we just say yep! Go there.
            return moveToPosition;
        }

        public LoginReply AttemptLogin(string playerName, PlayerCharacterOptions character)
        {
            var LoginResult = new LoginReply();
            LoginResult.Success = true;
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
    }
}
