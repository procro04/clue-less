﻿using Clue_Less_Server.Managers.Interfaces;
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
        
        public Greet.Location MovePlayer(int playerId, Greet.Location moveToPosition)
        {
            //You'll need to write validation code that makes sure this is a valid move, track the player, etc.
            //for now, we just say yep! Go there.
            return moveToPosition;
        }
    }
}
