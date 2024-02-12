﻿using Clue_Less.Managers.Interfaces;
using Models.GameplayObjects;
using System.Collections.Generic;

namespace Clue_Less.Managers
{
    public class TokenManager : ITokenManager
    {
        public TokenManager() { }

        public List<Player> InitializePlayers()
        {
            var playerList = new List<Player>
            {
                new Player { Name = "Mrs. Peacock", TokenId = 1 },
                new Player { Name = "Professor Plum", TokenId = 2 },
                new Player { Name = "Miss Scarlet", TokenId = 3 },
                new Player { Name = "Col. Mustard", TokenId = 4 },
                new Player { Name = "Mrs. White", TokenId = 5 },
                new Player { Name = "Mr. Green", TokenId = 6 }
            };
            
            return playerList;
        }

        public List<Weapon> InitializeWeapons()
        {
            var playerList = new List<Weapon>
            {
                new Weapon { Id = 1, Name = "Candlestick", TokenId = 1 },
                new Weapon { Id = 2, Name = "Letter Opener", TokenId = 2 },
                new Weapon { Id = 3, Name = "Revolver", TokenId = 3 },
                new Weapon { Id = 4, Name = "Lead Pipe", TokenId = 4 },
                new Weapon { Id = 5, Name = "Wrench", TokenId = 5 },
                new Weapon { Id = 6, Name = "Rope", TokenId = 6 }
            };

            return playerList;
        }
    }
}
