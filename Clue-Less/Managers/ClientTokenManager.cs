﻿using Clue_Less;
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
    public class ClientTokenManager 
    {
        private static readonly Lazy<ClientTokenManager> lazy = new Lazy<ClientTokenManager>(() => new ClientTokenManager());
        public static ClientTokenManager Instance { get { return lazy.Value; } }
        public ClientTokenManager() {
            InitializeAvailableTokens();
            InitializeWeapons();
        }

        public Point PlayerTokenSize = new Point(50, 50);
        public Point HomeSquareSize = new Point(60, 60);
        public Point TileSize = new Point(200, 200);
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
            else
            {
                ClientMenuManager.Instance.ShowNotification($"{character} has already been selected. Please try again");
            }
            return false;            
        }

        public void StartGame(StartGameResponse startGameMessage)
        {
            int numberOfPlayers = startGameMessage.PlayerId.Count;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                var playerId = startGameMessage.PlayerId[i];
                var playerLocation = startGameMessage.PlayerLocation[i];
                var playerCharacter = startGameMessage.PlayerCharacter[i];
                if (playerId != LoggedInPlayer.PlayerId)
                {
                    AssignPlayer(playerId, playerCharacter);
                }

                MovePlayer(playerId, playerLocation);
            }

            ClientMenuManager.Instance.StartGame();
        }

        public bool AttemptLoginFakePlayers(string username, PlayerCharacterOptions character)
        {
            var result = ClientGRPCService.Instance.AttemptLogin(username, character);
            if (result.Success)
            {
                return true;
            }
            return false;
        }

        public void AssignPlayer(int playerId, PlayerCharacterOptions playerCharacter)
        {
            var existingPlayer = ClientPlayers.FirstOrDefault(x => x.PlayerId == playerId); 

            if (existingPlayer == null)
            {
                var player = new ClientPlayer();
                player.PlayerId = playerId;
                player.AssignedToken = AvailableTokens.FirstOrDefault(x => x.TokenValue == playerCharacter);
                if (player.AssignedToken != null)
                {
                    player.AssignedToken.AssignedToPlayerId = player.PlayerId;
                }

                ClientPlayers.Add(player);
            }
        }

        
        public void MovePlayer(int playerId, Location moveToLocation)
        {
            foreach (var player in ClientPlayers)
            {
                if (player.PlayerId == playerId)
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
                if (player.AssignedToken.CurrentLocation != Location.Invalid)
                {
                    Globals.Instance.SpriteBatch.Draw(player.AssignedToken.Texture, new Rectangle((int)player.AssignedToken.RenderPosition.X, (int)player.AssignedToken.RenderPosition.Y, PlayerTokenSize.X, PlayerTokenSize.Y), Color.White);
                }                
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

        public void InitializeAvailableTokens()
        {
            AvailableTokens = new List<ClientToken>
            {
                new ClientToken { Name = "Mrs. Peacock", TokenId = 1, TokenValue = PlayerCharacterOptions.MrsPeacock, CurrentLocation = Location.Invalid, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_peacock_token") },
                new ClientToken { Name = "Professor Plum", TokenId = 2, TokenValue = PlayerCharacterOptions.ProfessorPlum, CurrentLocation = Location.Invalid, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/prof_plum_token") },
                new ClientToken { Name = "Miss Scarlet", TokenId = 3, TokenValue = PlayerCharacterOptions.MissScarlet, CurrentLocation = Location.Invalid, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_scarlet_token") },
                new ClientToken { Name = "Col. Mustard", TokenId = 4, TokenValue = PlayerCharacterOptions.ColMustard, CurrentLocation = Location.Invalid, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/col_mustard_token") },
                new ClientToken { Name = "Mrs. White", TokenId = 5, TokenValue = PlayerCharacterOptions.MrsWhite, CurrentLocation = Location.Invalid, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/miss_white_token") },
                new ClientToken { Name = "Mr. Green", TokenId = 6, TokenValue = PlayerCharacterOptions.MrGreen, CurrentLocation = Location.Invalid, Texture = Globals.Instance.Content.Load<Texture2D>("gametokens/mr_green_token") }
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

    }
}