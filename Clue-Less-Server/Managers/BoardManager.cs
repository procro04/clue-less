using Clue_Less_Server.Managers.Interfaces;
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
        public BoardManager() {
            boardPosition = new List<int> { 0, 1, 2 };
            playerPosition = 0;
        }

        public int GetPlayerLocation()
        {
            return playerPosition; 
        }
        
        public int MovePlayer(int playerId, int moveToPosition)
        {
            //positive case - can only go to the right by one square, but not off the board.
            if ((moveToPosition == (playerPosition + 1) && moveToPosition < 3) ||
                (moveToPosition == playerPosition - 1 && moveToPosition > -1))
            {
                //move position is valid
                playerPosition = moveToPosition;
            }
            return playerPosition;
        }
    }
}
