using Clue_Less;
using Greet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Models.GameplayObjects;
using Services;
using System;
using System.Collections.Generic;

namespace Managers
{
    public class TokenManager 
    {
        private static readonly Lazy<TokenManager> lazy = new Lazy<TokenManager>(() => new TokenManager());
        public static TokenManager Instance { get { return lazy.Value; } }
        public TokenManager() {
            InitializePlayers();
            InitializeWeapons();
        }

        public Point PlayerTokenSize = new Point(50, 50);
        private Point WeaponTokenSize = new Point(50, 50);

        public List<ClientWeapon> clientWeapons = new List<ClientWeapon>();
        public List<ClientPlayer> clientPlayers = new List<ClientPlayer>();
        public ClientPlayer LoggedInPlayer = null;

        public bool AttemptLogin(string username, PlayerCharacterOptions character)
        {
            var result = ClientGRPCService.Instance.AttemptLogin(username, character);
            if(result.Success)
            {
                AssignPlayer(result.PlayerId, character);
                foreach (var player in clientPlayers)
                {
                    if (player.PlayerId == result.PlayerId)
                    {
                        LoggedInPlayer = player;
                    }
                }
                return true;
            }
            return false;            
        }

        public void AssignPlayer(int playerId, PlayerCharacterOptions playerCharacter)
        {
            foreach (var player  in clientPlayers)
            {
                //Some sort of error handling if the token is already taken?
                if(player.TokenValue == playerCharacter)
                {
                    player.PlayerId = playerId;
                }
            }
        }

        public void InitializePlayers()
        {
            clientPlayers = new List<ClientPlayer>
            {
                new ClientPlayer { Name = "Mrs. Peacock",   TokenId = 1, TokenValue = PlayerCharacterOptions.MrsPeacock,    Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_peacock_token") },
                new ClientPlayer { Name = "Professor Plum", TokenId = 2, TokenValue = PlayerCharacterOptions.ProfessorPlum, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/prof_plum_token") },
                new ClientPlayer { Name = "Miss Scarlet",   TokenId = 3, TokenValue = PlayerCharacterOptions.MissScarlet,   Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_scarlet_token") },
                new ClientPlayer { Name = "Col. Mustard",   TokenId = 4, TokenValue = PlayerCharacterOptions.ColMustard,    Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/col_mustard_token") },
                new ClientPlayer { Name = "Mrs. White",     TokenId = 5, TokenValue = PlayerCharacterOptions.MrsWhite,    Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_white_token") },
                new ClientPlayer { Name = "Mr. Green",      TokenId = 6, TokenValue = PlayerCharacterOptions.MrGreen,       Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/mr_green_token") }
            };
        }

        public void InitializeWeapons()
        {
            clientWeapons = new List<ClientWeapon>
            {
                new ClientWeapon { Id = 1, Name = "Candlestick",    TokenId = 1, TokenValue = WeaponTokenEnum.Candlestick,  Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/candlestick") },
                new ClientWeapon { Id = 2, Name = "Letter Opener",  TokenId = 2, TokenValue = WeaponTokenEnum.LetterOpener, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/letter_opener") },
                new ClientWeapon { Id = 3, Name = "Revolver",       TokenId = 3, TokenValue = WeaponTokenEnum.Revolver,     Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/revolver") },
                new ClientWeapon { Id = 4, Name = "Lead Pipe",      TokenId = 4, TokenValue = WeaponTokenEnum.Pipe,         Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/pipe") },
                new ClientWeapon { Id = 5, Name = "Wrench",         TokenId = 5, TokenValue = WeaponTokenEnum.Wrench,       Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/wrench")},
                new ClientWeapon { Id = 6, Name = "Rope",           TokenId = 6, TokenValue = WeaponTokenEnum.Rope,         Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/rope") }
            };
        }

        public void MovePlayer(int playerId, Location moveToLocation)
        {
            foreach(var player in clientPlayers)
            {
                if(player.PlayerId == playerId)
                {                    
                    if(!RemovePlayer(playerId))
                    {
                        //Handle false case here.
                    }
                    //You'll need to add a function, or modify move player, to remove the player from their old position.
                    //i'd probably make it a seperate function.
                    player.RenderPosition = ClientBoardManager.Instance.MovePlayer(playerId, moveToLocation);
                    player.CurrentLocation = moveToLocation;
                }
            }
        }

        public bool RemovePlayer(int playerId)
        {
            foreach (var player in clientPlayers)
            {
                if (player.PlayerId == playerId)
                {
                    return ClientBoardManager.Instance.RemovePlayer(playerId, player.CurrentLocation);
                }
            }
            return false;
        }

        public void Draw(GameTime gameTime)
        {
            DrawPlayerTokens();
            //DrawWeaponTokens();
        }

        public void DrawPlayerTokens()
        {
            Globals.Instance.SpriteBatch.Begin();
            foreach (var player in clientPlayers)
            {
                Globals.Instance.SpriteBatch.Draw(player.Texture, new Rectangle((int)player.RenderPosition.X, (int)player.RenderPosition.Y, PlayerTokenSize.X, PlayerTokenSize.Y) , Color.White);
            }
            Globals.Instance.SpriteBatch.End();
        }

        public void DrawWeaponTokens()
        {
            Globals.Instance.SpriteBatch.Begin();
            foreach (var weapon in clientWeapons)
            {
                Globals.Instance.SpriteBatch.Draw(weapon.Texture, weapon.Position, Color.White);
            }
            Globals.Instance.SpriteBatch.End();
        }
    }
}
