using ImGuiNET;
using Microsoft.Xna.Framework;
using Services;
using System;
using System.Diagnostics;


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
            RenderButtons();
            ImGui.End();

            //Notification Window
            ImGui.SetNextWindowPos(TopAnchorPosition, ImGuiCond.Always);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(400, 200));
            ImGui.Begin("Notification", window_flags);
            BroadcastGlobalNotification();
            ImGui.End();


        }

        public void RenderButtons()
        {
            if (ImGui.Button("Say Hello"))
            {
                // The port number must match the port of the gRPC server. 
                // TODO Revisit when hosted on Azure
                Debug.WriteLine("Beginning gRPC Client instantiation \n");
                var reply = ClientGRPCService.Instance.SayHello("GreeterClient");
                Debug.WriteLine("Greetings, Earthling!: " + reply);
                Debug.WriteLine("End gRPC Client instantiation \n");

                //Debug.WriteLine("gRPC Request to move Player Token begin");
            }
            //if (ImGui.Button("StartGame"))
            //{
            //    ClientGRPCService.Instance.StartGame();
            //}
            //if (ImGui.Button("AddFakePlayers"))
            //{
            //    TokenManager.Instance.AttemptLogin("Bob", Greet.PlayerCharacterOptions.MrGreen);
            //    TokenManager.Instance.AttemptLogin("George", Greet.PlayerCharacterOptions.MrsWhite);
            //}
            if (TokenManager.Instance.LoggedInPlayer == null)
            {
                if (!EnteredUserName && ImGui.InputText("Enter Desired UserName", PlayerUserNameBuf, 32, ImGuiInputTextFlags.EnterReturnsTrue, null))
                {
                    PlayerUserName = System.Text.Encoding.UTF8.GetString(PlayerUserNameBuf);
                    PlayerUserName = PlayerUserName.Trim('\0');
                    EnteredUserName = true;
                }
                else if (EnteredUserName && !SelectedCharacter)
                {
                    var ClientPlayers = TokenManager.Instance.clientPlayers;
                    foreach (var player in ClientPlayers)
                    {
                        if (player.PlayerId == 0)
                        {
                            if (ImGui.Button("Select " + player.TokenValue.ToString()))
                            {
                                if (TokenManager.Instance.AttemptLogin(PlayerUserName, player.TokenValue))
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
                    if (ImGui.Button("Initialize Players"))
                    {
                        Debug.WriteLine("Beginning Token Manager's Instantiation of Suspects/Players \n");
                        TokenManager.Instance.InitializePlayers();
                        Debug.WriteLine("Our suspects");
                        foreach (var player in TokenManager.Instance.clientPlayers)
                        {
                            Debug.WriteLine(player.Name);
                        }
                        Debug.WriteLine("Ending Token Manager's Instantiation of Suspects/Players \n");
                    }

                    if (ImGui.Button("Initialize Weapons"))
                    {
                        Debug.WriteLine("Beginning Token Manager's Instantiation of Weapons \n");
                        TokenManager.Instance.InitializeWeapons();
                        Debug.WriteLine("Possible murder weapons");
                        foreach (var weapon in TokenManager.Instance.clientWeapons)
                        {
                            Debug.WriteLine(weapon.Name);
                        }
                        Debug.WriteLine("End Token Manager's Instantiation of Weapons");
                    }

                    ImGui.Text("Move MrsPeacock! \n");
                    if (ImGui.Button("Move MrsPeacock"))
                    {
                        Debug.WriteLine("Begin Client side GRPC request to Validation Mgr on Server - Valid Player Action");
                        TokenManager.Instance.AssignPlayer(1, Greet.PlayerCharacterOptions.MrsPeacock);
                        TokenManager.Instance.MovePlayer(1, ClientGRPCService.Instance.MovePlayerLocation(1, Greet.Location.HallwayOne));
                        Debug.WriteLine("End Client side GRPC request to Validation Mgr on Server - Valid Player Action");
                    }

                    ImGui.Text("Move MissScarlet! \n");
                    if (ImGui.Button("Move MissScarlet to HallwayOne"))
                    {
                        Debug.WriteLine("Begin Client side GRPC request to Validation Mgr on Server - Valid Player Action");
                        TokenManager.Instance.AssignPlayer(2, Greet.PlayerCharacterOptions.MissScarlet);
                        TokenManager.Instance.MovePlayer(2, ClientGRPCService.Instance.MovePlayerLocation(2, Greet.Location.HallwayOne));
                        Debug.WriteLine("End Client side GRPC request to Validation Mgr on Server - Valid Player Action");
                    }

                    if (ImGui.Button("Move MissScarlet to Kitchen"))
                    {
                        TokenManager.Instance.MovePlayer(2, ClientGRPCService.Instance.MovePlayerLocation(2, Greet.Location.Kitchen));
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
                    
                    if (ImGui.Button("Check Player Cards"))
                    {
                        PlayerCardMessage = BoardManager.Instance.CheckPlayerCards(true);
                        ShowPlayerCardMessage = true;
                    }
                }
            }
            
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
