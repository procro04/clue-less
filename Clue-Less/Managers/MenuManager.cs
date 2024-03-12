using Clue_Less.Enums;
using Clue_Less.Models.GameplayObjects;
using Clue_Less_Server.Managers;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Models.GameplayObjects;
using Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Clue_Less.Managers
{
    public class MenuManager
    {
        public static ImGuiWindowFlags window_flags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoNav;

        private static readonly Lazy<MenuManager> lazy = new Lazy<MenuManager>(() => new MenuManager());
        public static MenuManager Instance { get { return lazy.Value; } }
        public bool ShowText = false;
        public MenuManager() { }

        private System.Numerics.Vector2 AnchorPosition;

        public void SetAnchorPosition(System.Numerics.Vector2 anchorPosition)
        {
            AnchorPosition = anchorPosition;
        }

        public void Draw(GameTime gameTime)
        {
            ImGui.SetNextWindowPos(AnchorPosition, ImGuiCond.Always);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(800,400));
            ImGui.Begin("PlayerOptions", window_flags);
            RenderButtons();
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

            if (ImGui.Button("Initialize Players"))
            {
                Debug.WriteLine("Beginning Token Manager's Instantiation of Suspects/Players \n");
                var players = TokenManager.Instance.InitializePlayers();
                Debug.WriteLine("Our suspects");
                foreach (var player in players)
                {
                    Debug.WriteLine(player.Name);
                }
                Debug.WriteLine("Ending Token Manager's Instantiation of Suspects/Players \n");
            }

            if (ImGui.Button("Initialize Weapons"))
            {
                Debug.WriteLine("Beginning Token Manager's Instantiation of Weapons \n");
                var weapons = TokenManager.Instance.InitializeWeapons();
                Debug.WriteLine("Possible murder weapons");
                foreach (var weapon in weapons)
                {
                    Debug.WriteLine(weapon.Name);
                }
                Debug.WriteLine("End Token Manager's Instantiation of Weapons");
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

            if (ShowText)
            {
                ImGui.Text("CLUE-LESS! JUST LIKE CLUE BUT LESS!");
            }
        }
    }
}
