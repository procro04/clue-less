using Clue_Less;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Models.GameplayObjects;
using System;
using System.Collections.Generic;
using Greet;

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

        //Return true if found at correct location, false otherwise.
        public bool RemovePlayer(int playerId, Location removeFromLocation)
        {
            foreach (var tile in Tiles)
            {
                if (tile.TileType == removeFromLocation)
                {
                    for (int i = 0; i < tile.TokenSlots.Count; i++)
                    {
                        if (tile.TokenSlots[i].PlayerId == playerId)
                        {
                            tile.TokenSlots[i].PlayerId = 0;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public Vector2 MovePlayer(int playerId, Location moveToLocation)
        {
            foreach(var tile in Tiles)
            {
                if(tile.TileType == moveToLocation)
                {
                    for(int i = 0; i < tile.TokenSlots.Count; i++)
                    {
                        if (tile.TokenSlots[i].PlayerId == 0)
                        {
                            tile.TokenSlots[i].PlayerId = playerId;
                            return tile.TokenSlots[i].RenderPosition;
                        }
                    }
                }
            }
            return new Vector2(0.0f, 0.0f);
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
            Tiles.Add(new ClientBoardTile(studyTex, currentPos, Location.Study));
            currentPos.X += studyTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, Location.HallwayOne));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(concertHallTex, currentPos, Location.ConcertHall));
            currentPos.X += concertHallTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, Location.HallwayTwo));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(loungeTex, currentPos, Location.Lounge));

            //Second row
            currentPos.Y += hallwayTexVert.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, Location.HallwayThree));
            currentPos.X += hallwayTexVert.Width;
            //Skip one spot
            currentPos.X += hallwayTexVert.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, Location.HallwayFour));
            currentPos.X += hallwayTexVert.Width;
            //Skip one spot
            currentPos.X += hallwayTexVert.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, Location.HallwayFive));

            //Third Row
            currentPos.Y += hallwayTexHoriz.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(libraryTex, currentPos, Location.Library));
            currentPos.X += libraryTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, Location.HallwaySix));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(billiardTex, currentPos, Location.Billiard));
            currentPos.X += billiardTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, Location.HallwaySeven));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(diningRoomTex, currentPos, Location.DiningRoom));
            currentPos.X += diningRoomTex.Width;

            MenuPosition.X = currentPos.X;

            //Fourth row
            currentPos.Y += hallwayTexVert.Height;
            MenuPosition.Y = currentPos.Y;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, Location.HallwayEight));
            currentPos.X += hallwayTexVert.Width;
            //Skip one spot
            currentPos.X += hallwayTexVert.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, Location.HallwayNine));
            currentPos.X += hallwayTexVert.Width;
            //Skip one spot
            currentPos.X += hallwayTexVert.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexVert, currentPos, Location.HallwayTen));


            //Fifth Row
            currentPos.Y += hallwayTexHoriz.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(conservatoryTex, currentPos, Location.Conservatory));
            currentPos.X += conservatoryTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, Location.HallwayEleven));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(ballroomTex, currentPos, Location.Ballroom));
            currentPos.X += ballroomTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTexHoriz, currentPos, Location.HallwayTwelve));
            currentPos.X += hallwayTexHoriz.Width;
            Tiles.Add(new ClientBoardTile(kitchenTex, currentPos, Location.Kitchen));
            currentPos.X += kitchenTex.Width;

            MenuManager.Instance.SetAnchorPosition(MenuPosition);
        }
    }
}
