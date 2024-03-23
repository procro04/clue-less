using Clue_Less;
using Greet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Models.GameplayObjects;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Managers
{
    public class TokenManager 
    {
        private static readonly Lazy<TokenManager> lazy = new Lazy<TokenManager>(() => new TokenManager());
        public static TokenManager Instance { get { return lazy.Value; } }
        public TokenManager() {
            InitializeAvailableTokens();
            InitializeWeapons();
        }

        public Point PlayerTokenSize = new Point(50, 50);
        private Point WeaponTokenSize = new Point(50, 50);

        public List<ClientWeapon> ClientWeapons = new List<ClientWeapon>();
        public List<ClientPlayer> ClientPlayers = new List<ClientPlayer>();
        public List<ClientToken> AvailableTokens = new List<ClientToken>();
        public ClientPlayer LoggedInPlayer = null;

        public bool AttemptLogin(string username, PlayerCharacterOptions character)
        {
            var result = ClientGRPCService.Instance.AttemptLogin(username, character);
            if (result.Success)
            {
                AssignPlayer(result.PlayerId, character);
                foreach (var player in ClientPlayers)
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

        public List<int> GetPlayerTurnOrder()
        {
            return ClientGRPCService.Instance.GetPlayerTurnOrder();
        }

        public void AssignPlayer(int playerId, PlayerCharacterOptions playerCharacter)
        {
            var existingPlayer = ClientPlayers.FirstOrDefault(x => x.PlayerId == playerId); 

            if (existingPlayer == null)
            {
                var player = new ClientPlayer();
                player.PlayerId = playerId;
                player.AssignedToken = AvailableTokens.FirstOrDefault(x => x.TokenValue == playerCharacter);
                
                if  (player.AssignedToken != null)
                {
                    player.AssignedToken.AssignedToPlayerId = player.PlayerId;
                }

                ClientPlayers.Add(player);
            }
            else
            {
                ClientGRPCService.Instance.SendGlobalPlayerNotification("That token has already been selected. Please select another.");
            }
        }

        public void InitializeAvailableTokens()
        {
            AvailableTokens = new List<ClientToken>
            {
                new ClientToken { Name = "Mrs. Peacock", TokenId = 1, TokenValue = PlayerCharacterOptions.MrsPeacock, CurrentLocation = Location.HallwayEight, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_peacock_token") },
                new ClientToken { Name = "Professor Plum", TokenId = 2, TokenValue = PlayerCharacterOptions.ProfessorPlum, CurrentLocation = Location.HallwayThree, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/prof_plum_token") },
                new ClientToken { Name = "Miss Scarlet", TokenId = 3, TokenValue = PlayerCharacterOptions.MissScarlet, CurrentLocation = Location.HallwayTwo, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_scarlet_token") },
                new ClientToken { Name = "Col. Mustard", TokenId = 4, TokenValue = PlayerCharacterOptions.ColMustard, CurrentLocation = Location.HallwayFive, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/col_mustard_token") },
                new ClientToken { Name = "Mrs. White", TokenId = 5, TokenValue = PlayerCharacterOptions.MrsWhite, CurrentLocation = Location.HallwayTwelve, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_white_token") },
                new ClientToken { Name = "Mr. Green", TokenId = 6, TokenValue = PlayerCharacterOptions.MrGreen, CurrentLocation = Location.HallwayEleven, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/mr_green_token") }
            };
        }

        public void InitializeWeapons()
        {
            ClientWeapons = new List<ClientWeapon>
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
            foreach(var player in ClientPlayers)
            {
                if(player.PlayerId == playerId)
                {                    
                    if(!RemovePlayer(playerId))
                    {
                        //Handle false case here.
                    }
                    //You'll need to add a function, or modify move player, to remove the player from their old position.
                    //i'd probably make it a seperate function.
                    player.AssignedToken.RenderPosition = ClientBoardManager.Instance.MovePlayer(playerId, moveToLocation);
                    player.AssignedToken.CurrentLocation = moveToLocation;
                }
            }
        }

        public bool RemovePlayer(int playerId)
        {
            foreach (var player in ClientPlayers)
            {
                if (player.PlayerId == playerId)
                {
                    return ClientBoardManager.Instance.RemovePlayer(playerId, player.AssignedToken.CurrentLocation);
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
            foreach (var player in ClientPlayers)
            {
                Globals.Instance.SpriteBatch.Draw(player.AssignedToken.Texture, new Rectangle((int)player.AssignedToken.RenderPosition.X, (int)player.AssignedToken.RenderPosition.Y, PlayerTokenSize.X, PlayerTokenSize.Y) , Color.White);
            }
            Globals.Instance.SpriteBatch.End();
        }

        public void DrawWeaponTokens()
        {
            Globals.Instance.SpriteBatch.Begin();
            foreach (var weapon in ClientWeapons)
            {
                Globals.Instance.SpriteBatch.Draw(weapon.Texture, weapon.Position, Color.White);
            }
            Globals.Instance.SpriteBatch.End();
        }
    }
}
