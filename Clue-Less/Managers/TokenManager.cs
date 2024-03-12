using Clue_Less.Enums;
using Clue_Less.Models.GameplayObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Models.GameplayObjects;
using System;
using System.Collections.Generic;

namespace Clue_Less.Managers
{
    public class TokenManager 
    {
        private static readonly Lazy<TokenManager> lazy = new Lazy<TokenManager>(() => new TokenManager());
        public static TokenManager Instance { get { return lazy.Value; } }
        public TokenManager() { }

        public List<ClientPlayer> InitializePlayers()
        {
            var playerList = new List<ClientPlayer>
            {
                new ClientPlayer { Name = "Mrs. Peacock", TokenId = 1, TokenValue = PlayerTokenEnum.MissPeacock, ContentUrl = "gametokens/miss_peacock_token" },
                new ClientPlayer { Name = "Professor Plum", TokenId = 2, TokenValue = PlayerTokenEnum.ProfPlum, ContentUrl = "gametokens/prof_plum_token" },
                new ClientPlayer { Name = "Miss Scarlet", TokenId = 3, TokenValue = PlayerTokenEnum.MissScarlet, ContentUrl = "gametokens/miss_scarlet_token" },
                new ClientPlayer { Name = "Col. Mustard", TokenId = 4, TokenValue = PlayerTokenEnum.ColMustard, ContentUrl = "gametokens/col_mustard_token" },
                new ClientPlayer { Name = "Mrs. White", TokenId = 5, TokenValue = PlayerTokenEnum.ColMustard, ContentUrl = "gametokens/miss_white_token" },
                new ClientPlayer { Name = "Mr. Green", TokenId = 6, TokenValue = PlayerTokenEnum.MrGreen, ContentUrl = "gametokens/mr_green_token" }
            };

            foreach (var player in playerList)
            {
                player.Texture = Globals.Instance.Content.Load<Texture2D>(player.ContentUrl);
                player.Position = new Vector2(100,100);
            }

            return playerList;
        }

        public List<ClientWeapon> InitializeWeapons()
        {
            var weaponList = new List<ClientWeapon>
            {
                new ClientWeapon { Id = 1, Name = "Candlestick", TokenId = 1, TokenValue = WeaponTokenEnum.Candlestick, ContentUrl = "gametokens/candlestick" },
                new ClientWeapon { Id = 2, Name = "Letter Opener", TokenId = 2 , TokenValue = WeaponTokenEnum.LetterOpener, ContentUrl = "gametokens/letter_opener" },
                new ClientWeapon { Id = 3, Name = "Revolver", TokenId = 3, TokenValue = WeaponTokenEnum.Revolver, ContentUrl = "gametokens/revolver" },
                new ClientWeapon { Id = 4, Name = "Lead Pipe", TokenId = 4, TokenValue = WeaponTokenEnum.Pipe, ContentUrl = "gametokens/pipe" },
                new ClientWeapon { Id = 5, Name = "Wrench", TokenId = 5, TokenValue = WeaponTokenEnum.Wrench, ContentUrl = "gametokens/wrench" },
                new ClientWeapon { Id = 6, Name = "Rope", TokenId = 6, TokenValue = WeaponTokenEnum.Rope, ContentUrl = "gametokens/rope" }
            };

            foreach ( var weapon in weaponList)
            {
                weapon.Texture = Globals.Instance.Content.Load<Texture2D>(weapon.ContentUrl);
                weapon.Position = new System.Numerics.Vector2(0, 0);
            }

            return weaponList;
        }

        public void DrawPlayerTokens()
        {
            Globals.Instance.SpriteBatch.Begin();
            var players = InitializePlayers();
            foreach (var player in players)
            {
                Globals.Instance.SpriteBatch.Draw(player.Texture, player.Position, Color.White);
            }
            Globals.Instance.SpriteBatch.End();
        }

        public void DrawWeaponTokens()
        {
            Globals.Instance.SpriteBatch.Begin();
            var weapons = InitializeWeapons();
            foreach (var weapon in weapons)
            {
                Globals.Instance.SpriteBatch.Draw(weapon.Texture, weapon.Position, Color.White);
            }
            Globals.Instance.SpriteBatch.End();
        }
    }
}
