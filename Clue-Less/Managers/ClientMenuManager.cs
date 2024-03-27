using ImGuiNET;
using Microsoft.Xna.Framework;
using Services;
using System;
using System.Diagnostics;
using Greet;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Linq;


namespace Managers
{
    public class ClientMenuManager
    {
        public static ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoNav;

        private static readonly Lazy<ClientMenuManager> lazy = new Lazy<ClientMenuManager>(() => new ClientMenuManager());
        public static ClientMenuManager Instance { get { return lazy.Value; } }
        public ClientMenuManager() { }

        private System.Numerics.Vector2 BottomAnchorPosition;
        private System.Numerics.Vector2 TopAnchorPosition;
        byte[] PlayerUserNameBuf = new byte[32];
        string PlayerUserName;
        bool EnteredUserName = false;
        bool SelectedCharacter = false;
        public bool ShowText = false;
        public string PlayerCardMessage = "";
        public bool ShowPlayerCardMessage = false;
        private bool ShowUserNameText = true;
        private int QueueMaxLength = 25;
        public bool GameInstanceStarted = false;
        private bool DisplayStartGameButton = false;
        public Queue<string> MessageQueue = new Queue<string>();
        private bool FirstTurnComplete  = false;

        public void SetBottomAnchorPosition(System.Numerics.Vector2 bottomAnchorPosition)
        {
            BottomAnchorPosition = bottomAnchorPosition;
        }

        public void SetTopAnchorPosition(System.Numerics.Vector2 topAnchorPosition)
        {
            TopAnchorPosition = topAnchorPosition;
        }

        public void Draw(GameTime gameTime)
        {
            //Bottom menu 
            ImGui.SetNextWindowPos(BottomAnchorPosition, ImGuiCond.Always);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(800,400));
            ImGui.Begin("PlayerOptions", window_flags);
            HandleLogin();
            ImGui.End();

            //Notification Window
            ImGui.SetNextWindowPos(TopAnchorPosition, ImGuiCond.Always);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(400, 200));
            ImGui.Begin("Notification", window_flags);
            foreach (var message in MessageQueue)
            {
                ImGui.Text(message);
            }

            if (MessageQueue.Count == QueueMaxLength)
            {
                MessageQueue.Clear();
                ImGui.Text("");
            }
            ImGui.End();
        }

        public void HandleLogin()
        {
            if (ClientTokenManager.Instance.LoggedInPlayer == null)
            {
                if  (ShowUserNameText)
                {
                    ImGui.Text("Input Desired UserName and press the Enter key");
                }

                if (!EnteredUserName && ImGui.InputText("Enter Desired UserName", PlayerUserNameBuf, 32, ImGuiInputTextFlags.EnterReturnsTrue, null))
                {
                    PlayerUserName = System.Text.Encoding.UTF8.GetString(PlayerUserNameBuf);
                    PlayerUserName = PlayerUserName.Trim('\0');
                    EnteredUserName = true;
                    ShowUserNameText = false;
                }
                else if (EnteredUserName && !SelectedCharacter)
                {
                    var possibleTokens = ClientTokenManager.Instance.AvailableTokens;
                    foreach (var token in possibleTokens)
                    {
                        if (token.AssignedToPlayerId == 0)
                        {
                            if (ImGui.Button("Select " + token.TokenValue.ToString()))
                            {
                                if (ClientTokenManager.Instance.AttemptLogin(PlayerUserName, token.TokenValue))
                                {
                                    SelectedCharacter = true;
                                }
                                else
                                {
                                    ImGui.Text("Character Taken, Pick another one! \n");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (EnteredUserName && SelectedCharacter)
                {
                    if (!GameInstanceStarted)
                    {
                        DisplayStartGameButton = true;
                    }

                    if (DisplayStartGameButton)
                    {
                        if (ImGui.Button("Start Game"))
                        {
                            ClientGRPCService.Instance.StartGame();
                        }
                    }
                    if (ImGui.Button("Initialize Fake Players Bob and George"))
                    {
                        ClientTokenManager.Instance.AttemptLoginFakePlayers("Bob", PlayerCharacterOptions.MrGreen);
                        ClientTokenManager.Instance.AttemptLoginFakePlayers("George", PlayerCharacterOptions.MrsWhite);                        
                    }

                    if (GameInstanceStarted)
                    {
                        //Ingame UI goes here
                        DisplayGameMenus();
                    }
                }
            }            
        }

        public void DisplayGameMenus()
        {
            var response = ClientGRPCService.Instance.Heartbeat();
            if (!FirstTurnComplete)
            {
                if (ImGui.Button("End Turn"))
                {
                    ClientGRPCService.Instance.AdvancePlayerTurn(response.CurrentTurn.PlayerId);
                }
            }
            //if (ImGui.Button("Test, Move LocalPlayer to Hallway 3"))
            //{
            //    ClientTokenManager.Instance.MovePlayer(ClientTokenManager.Instance.LoggedInPlayer.PlayerId, Location.HallwayThree);
            //}
            //if (ImGui.Button("Test, Move LocalPlayer to Hallway 2"))
            //{
            //    ClientTokenManager.Instance.MovePlayer(ClientTokenManager.Instance.LoggedInPlayer.PlayerId, Location.HallwayTwo);
            //}
            //if (ImGui.Button("Test, Move LocalPlayer to Hallway 5"))
            //{
            //    ClientTokenManager.Instance.MovePlayer(ClientTokenManager.Instance.LoggedInPlayer.PlayerId, Location.HallwayFive);
            //}
        }

        public void ShowNotification(string message)
        {
            MessageQueue.Enqueue(message);
        }

        public void StartGame()
        {
            DisplayStartGameButton = false;
            GameInstanceStarted = true;
        }
    }
}
