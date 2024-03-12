using Clue_Less;
using Clue_Less.Enums;
using Clue_Less.Managers;
using Clue_Less.Models.GameplayObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Managers
{
    public class ClientBoardManager
    {
        private System.Numerics.Vector2 MenuPosition;

        private static readonly Lazy<ClientBoardManager> lazy = new Lazy<ClientBoardManager>(() => new ClientBoardManager());
        public static ClientBoardManager Instance { get { return lazy.Value; } }

        private const int BOARD_PADDING = 5;
        private List<ClientBoardTile> Tiles { get; } = new List<ClientBoardTile>();

        public ClientBoardManager()
        {
            //Instantiate all tiles and then draw them.
            InitalizeBoardTiles();
        }

        public void Draw(GameTime gameTime)
        {
            //Draw our tiles
            foreach (var tile in Tiles)
            {
                tile.Draw();
            }

            //Draw our menu in the correct place relative to the tiles.
            MenuManager.Instance.Draw(gameTime);
        }

        public void InitalizeBoardTiles()
        {
            var ballroomTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/ballroom");
            var billiardTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/billiard");
            var concertHallTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/concert_hall");
            var conservatoryTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/conservatory");
            var diningRoomTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/dining_room");
            var kitchenTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/kitchen");
            var libraryTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/library");
            var loungeTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/lounge");
            var studyTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/study");
            var hallwayTexVert = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway_vertical");
            var hallwayTexHoriz = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway_horizontal");


            var startingPos = new System.Numerics.Vector2(BOARD_PADDING, BOARD_PADDING);
            var currentPos = startingPos;
            //First row of rooms
            Tiles.Add(new ClientBoardTile(studyTex, currentPos, TileTypeEnum.Study));
            currentPos.X += studyTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(concertHallTex, currentPos, TileTypeEnum.ConcertHall));
            currentPos.X += concertHallTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(loungeTex, currentPos, TileTypeEnum.Lounge));

            //Second row
            currentPos.Y += hallwayTexVert.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexVert.Width;
            //Skip one spot
            currentPos.X += hallwayTexVert.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexVert.Width;
            //Skip one spot
            currentPos.X += hallwayTexVert.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, TileTypeEnum.Hallway));

            //Third Row
            currentPos.Y += hallwayTexHoriz.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(libraryTex, currentPos, TileTypeEnum.Library));
            currentPos.X += libraryTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(billiardTex, currentPos, TileTypeEnum.Billiard));
            currentPos.X += billiardTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(diningRoomTex, currentPos, TileTypeEnum.DiningRoom));
            currentPos.X += diningRoomTex.Width;

            MenuPosition.X = currentPos.X;

            //Fourth row
            currentPos.Y += hallwayTexVert.Height;
            MenuPosition.Y = currentPos.Y;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexVert.Width;
            //Skip one spot
            currentPos.X += hallwayTexVert.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexVert.Width;
            //Skip one spot
            currentPos.X += hallwayTexVert.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, TileTypeEnum.Hallway));


            //Fifth Row
            currentPos.Y += hallwayTexHoriz.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(conservatoryTex, currentPos, TileTypeEnum.Conservatory));
            currentPos.X += conservatoryTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(ballroomTex, currentPos, TileTypeEnum.Ballroom));
            currentPos.X += ballroomTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, TileTypeEnum.Hallway));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(kitchenTex, currentPos, TileTypeEnum.Kitchen));
            currentPos.X += kitchenTex.Width;

            MenuManager.Instance.SetAnchorPosition(MenuPosition);
        }
    }
}
