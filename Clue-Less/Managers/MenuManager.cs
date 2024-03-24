using ImGuiNET;
using Microsoft.Xna.Framework;
using Services;
using System;
using System.Diagnostics;
using Greet;


namespace Managers
{
    public class MenuManager
    {
        public static ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoNav;

        private static readonly Lazy<MenuManager> lazy = new Lazy<MenuManager>(() => new MenuManager());
        public static MenuManager Instance { get { return lazy.Value; } }
        public MenuManager() { }

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
        private int TurnCounter = 0;
        public bool GameInstanceStarted = false;
        private bool DisplayStartGameButton = false;

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
            BroadcastGlobalNotification();
            ImGui.End();
        }

        public void HandleLogin()
        {
            if (TokenManager.Instance.LoggedInPlayer == null)
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
                    var possibleTokens = TokenManager.Instance.AvailableTokens;
                    foreach (var token in possibleTokens)
                    {
                        if (token.AssignedToPlayerId == 0)
                        {
                            if (ImGui.Button("Select " + token.TokenValue.ToString()))
                            {
                                if (TokenManager.Instance.AttemptLogin(PlayerUserName, token.TokenValue))
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
                            DisplayStartGameButton = false;
                            GameInstanceStarted = true;
                        }
                    }
                    if (ImGui.Button("Initialize Fake Players Bob and George"))
                    {
                        TokenManager.Instance.AttemptLoginFakePlayers("Bob", PlayerCharacterOptions.MrGreen);
                        TokenManager.Instance.AttemptLoginFakePlayers("George", PlayerCharacterOptions.MrsWhite);                        
                    }

                    if (GameInstanceStarted)
                    {
                        //Ingame UI goes here
                        if (ImGui.Button("Test, Move LocalPlayer to Hallway 3"))
                        {
                            TokenManager.Instance.MovePlayer(TokenManager.Instance.LoggedInPlayer.PlayerId, Location.HallwayThree);
                        }
                        if (ImGui.Button("Test, Move LocalPlayer to Hallway 2"))
                        {
                            TokenManager.Instance.MovePlayer(TokenManager.Instance.LoggedInPlayer.PlayerId, Location.HallwayTwo);
                        }
                        if (ImGui.Button("Test, Move LocalPlayer to Hallway 5"))
                        {
                            TokenManager.Instance.MovePlayer(TokenManager.Instance.LoggedInPlayer.PlayerId, Location.HallwayFive);
                        }
                    }
                }
            }            
        }

        public void DisplayGameMenus()
        {
            ImGui.Text("Move Current Player! \n");
            if (ImGui.Button("Move Us (Current Player) to HallwayOne"))
            {
                Debug.WriteLine("Begin Client side GRPC request to Validation Mgr on Server - Valid Player Action");
                TokenManager.Instance.MovePlayer(1, ClientGRPCService.Instance.MovePlayerLocation(1, Location.HallwayOne));
                Debug.WriteLine("End Client side GRPC request to Validation Mgr on Server - Valid Player Action");
            }

            ImGui.Text("Move Player 2(Bob)! \n");
            if (ImGui.Button("Move (Bob) MrGreen to HallwayOne"))
            {
                TokenManager.Instance.MovePlayer(2, ClientGRPCService.Instance.MovePlayerLocation(2, Location.HallwayOne));
            }
            ImGui.Text("Move Player 3(George)! \n");
            if (ImGui.Button("Move (George) MrsWhite to HallwayThree"))
            {
                Debug.WriteLine("Begin Client side GRPC request to Validation Mgr on Server - Valid Player Action");
                TokenManager.Instance.MovePlayer(3, ClientGRPCService.Instance.MovePlayerLocation(2, Location.HallwayThree));
                Debug.WriteLine("End Client side GRPC request to Validation Mgr on Server - Valid Player Action");
            }

            if (ImGui.Button("Move Player 2(Bob) to Kitchen"))
            {
                TokenManager.Instance.MovePlayer(2, ClientGRPCService.Instance.MovePlayerLocation(2, Location.Kitchen));
            }

            ImGui.Text("Make A Move! \n");
            if (ImGui.Button("Valid Move"))
            {
                Debug.WriteLine("Begin Client side GRPC request to Validation Mgr on Server - Valid Player Action");
                ClientGRPCService.Instance.ValidatePlayerAction(true);
                Debug.WriteLine("End Client side GRPC request to Validation Mgr on Server - Valid Player Action");
            }
            ImGui.SameLine();

            if (ImGui.Button("Invalid Move"))
            {
                Debug.WriteLine("Begin Client side GRPC request to Validation Mgr on Server - Invalid Player Action");
                ClientGRPCService.Instance.ValidatePlayerAction(false);
                Debug.WriteLine("End Client side GRPC request to Validation Mgr on Server - Invalid Player Action");
            }

            if (ImGui.Button("Broadcast Global Message"))
            {
                Debug.WriteLine("Client side begin global msg");
                ClientGRPCService.Instance.SendGlobalPlayerNotification("CLUE-LESS! JUST LIKE CLUE BUT LESS!");
                ShowText = true;
                Debug.WriteLine("Client side end global msg");
            }

            //if (ImGui.Button("Check Player Cards"))
            //{
            //    PlayerCardMessage = BoardManager.Instance.CheckPlayerCards(true);
            //    ShowPlayerCardMessage = true;
            //}

        }

        public void BroadcastGlobalNotification()
        {
            if (ShowText)
            {
                ImGui.Text("CLUE-LESS! JUST LIKE CLUE BUT LESS!");
            }

            if (ShowPlayerCardMessage)
            {
                ImGui.Text(PlayerCardMessage);
            }
        }
    }
}
