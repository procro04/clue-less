using Clue_Less;
using Clue_Less.Enums;
using Clue_Less.Models.GameplayObjects;
using Clue_Less_Server.Managers;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Mime;
using System.Numerics;
using TiledSharp;

namespace Managers
{
    public class ClientBoardManager
    {

        private static readonly Lazy<ClientBoardManager> lazy = new Lazy<ClientBoardManager>(() => new ClientBoardManager());
        public static ClientBoardManager Instance { get { return lazy.Value; } }

        private const int BOARD_PADDING = 5;
        private List<ClientBoardTile> Tiles { get; } = new List<ClientBoardTile>();

        public ClientBoardManager()
        {
            var ballroomTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/ballroom");
            var billiardTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/billiard");
            var concertHallTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/concert_hall");
            var conservatoryTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/conservatory");
            var diningRoomTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/dining_room");
            var kitchenTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/kitchen");
            var libraryTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/library");
            var loungeTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/lounge");
            var studyTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/study");
            var hallwayTex = Globals.Instance.Content.Load<Texture2D>("gamerooms/hallway");

            var startingPos = new Vector2(BOARD_PADDING, BOARD_PADDING);
            var currentPos = startingPos;
            //First row of rooms
            Tiles.Add(new ClientBoardTile(studyTex, currentPos, TileTypeEnum.Study));
            currentPos.X += studyTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(concertHallTex, currentPos, TileTypeEnum.ConcertHall));
            currentPos.X += concertHallTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(loungeTex, currentPos, TileTypeEnum.Lounge));

            //Second row
            currentPos.Y += hallwayTex.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            //Skip one spot
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            //Skip one spot
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));

            //Third Row
            currentPos.Y += hallwayTex.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(libraryTex, currentPos, TileTypeEnum.Library));
            currentPos.X += libraryTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(billiardTex, currentPos, TileTypeEnum.Billiard));
            currentPos.X += billiardTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(diningRoomTex, currentPos, TileTypeEnum.DiningRoom));
            currentPos.X += diningRoomTex.Width;

            //Fourth row
            currentPos.Y += hallwayTex.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            //Skip one spot
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            //Skip one spot
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));


            //Fifth Row
            currentPos.Y += hallwayTex.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(conservatoryTex, currentPos, TileTypeEnum.Conservatory));
            currentPos.X += conservatoryTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(ballroomTex, currentPos, TileTypeEnum.Ballroom));
            currentPos.X += ballroomTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTex, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTex.Width;
            Tiles.Add(new ClientBoardTile(kitchenTex, currentPos, TileTypeEnum.Kitchen));
            currentPos.X += kitchenTex.Width;

        }

        public void Draw()
        {
            foreach (var tile in Tiles)
            {
                tile.Draw();
            }
        }
    }
}
