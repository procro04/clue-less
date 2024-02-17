using Clue_Less.Managers.Interfaces;
using Models.GameplayObjects;
using System.Collections.Generic;

namespace Clue_Less.Managers
{
    public class TokenManager : ITokenManager
    {
        public TokenManager() { }

        public List<ClientPlayer> InitializePlayers()
        {
            var playerList = new List<ClientPlayer>
            {
                new ClientPlayer { Name = "Mrs. Peacock", TokenId = 1 },
                new ClientPlayer { Name = "Professor Plum", TokenId = 2 },
                new ClientPlayer { Name = "Miss Scarlet", TokenId = 3 },
                new ClientPlayer { Name = "Col. Mustard", TokenId = 4 },
                new ClientPlayer { Name = "Mrs. White", TokenId = 5 },
                new ClientPlayer { Name = "Mr. Green", TokenId = 6 }
            };
            
            return playerList;
        }

        public List<ClientWeapon> InitializeWeapons()
        {
            var weaponList = new List<ClientWeapon>
            {
                new ClientWeapon { Id = 1, Name = "Candlestick", TokenId = 1 },
                new ClientWeapon { Id = 2, Name = "Letter Opener", TokenId = 2 },
                new ClientWeapon { Id = 3, Name = "Revolver", TokenId = 3 },
                new ClientWeapon { Id = 4, Name = "Lead Pipe", TokenId = 4 },
                new ClientWeapon { Id = 5, Name = "Wrench", TokenId = 5 },
                new ClientWeapon { Id = 6, Name = "Rope", TokenId = 6 }
            };

            return weaponList;
        }
    }
}
