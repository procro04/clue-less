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
        private System.Numerics.Vector2 BottomMenuPosition;
        private System.Numerics.Vector2 TopMenuPosition;

        private static readonly Lazy<ClientBoardManager> lazy = new Lazy<ClientBoardManager>(() => new ClientBoardManager());
        public static ClientBoardManager Instance { get { return lazy.Value; } }

        private const int BOARD_PADDING = 2;
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
            foreach (var tile in Tiles)
            {
                if (tile.TileType == moveToLocation)
                {
                    for (int i = 0; i < tile.TokenSlots.Count; i++)
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
            ClientMenuManager.Instance.Draw(gameTime);
        }

        public string FormatLocationNames(Location location)
        {
            switch (location)
            {
                //Roooms
                case Location.Ballroom:
                    return "Ballroom";
                case Location.Billiard:
                    return "Billiard Room";
                case Location.ConcertHall:
                    return "Concert Hall";
                case Location.Conservatory:
                    return "Conservatory";
                case Location.DiningRoom:
                    return "Dining Room";
                case Location.Kitchen:
                    return "Kitchen";
                case Location.Library:
                    return "Library";
                case Location.Lounge:
                    return "Lounge";
                case Location.Study:
                    return "Study";

                //Hallways
                case Location.HallwayOne:
                    return "Hallway 1";
                case Location.HallwayTwo:
                    return "Hallway 2";
                case Location.HallwayThree:
                    return "Hallway 3";
                case Location.HallwayFour:
                    return "Hallway 4";
                case Location.HallwayFive:
                    return "Hallway 5";
                case Location.HallwaySix:
                    return "Hallway 6";
                case Location.HallwaySeven:
                    return "Hallway 7";
                case Location.HallwayEight:
                    return "Hallway 8";
                case Location.HallwayNine:
                    return "Hallway 9";
                case Location.HallwayTen:
                    return "Hallway 10";
                case Location.HallwayEleven:
                    return "Hallway 11";
                case Location.HallwayTwelve:
                    return "Hallway 12";
            }
            return "";
        }

        public void InitalizeBoardTiles()
        {
            //Individual Rooms
            var ballroomTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/ballroom");
            var billiardTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/billiard");
            var concertHallTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/concert_hall");
            var conservatoryTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/conservatory");
            var diningRoomTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/dining_room");
            var kitchenTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/kitchen");
            var libraryTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/library");
            var loungeTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/lounge");
            var studyTex = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/study");

            //Home Squares
            var missPeacockHomeSquareTex = Globals.Instance.Content.Load<Texture2D>("gameobjects/MissPeacockHomeSquare");
            var professorHomeSquareTex = Globals.Instance.Content.Load<Texture2D>("gameobjects/ProfPeacockHomeSquare"); //haha rip wrong name, right color this is prof plum.
            var missScarletHomeSquareTex = Globals.Instance.Content.Load<Texture2D>("gameobjects/MissScarletHomeSquare");
            var colMustardHomeSquareTex = Globals.Instance.Content.Load<Texture2D>("gameobjects/ColMustardHomeSquare");
            var missWhiteHomeSquareTex = Globals.Instance.Content.Load<Texture2D>("gameobjects/MissWhiteHomeSquare");
            var mrGreenHomeSquareTex = Globals.Instance.Content.Load<Texture2D>("gameobjects/MrGreenHomeSquare");

            //Hallways
            var hallwayOne = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway1");
            var hallwayTwo = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway2");
            var hallwayThree = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway3");
            var hallwayFour = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway4");
            var hallwayFive = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway5");
            var hallwaySix = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway6");
            var hallwaySeven = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway7");
            var hallwayEight= Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway8");
            var hallwayNine = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway9");
            var hallwayTen = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway10");
            var hallwayEleven = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway11");
            var hallwayTwelve = Globals.Instance.Content.Load<Texture2D>("gameroomslabeled/hallway12");

            var startingPos = new System.Numerics.Vector2(BOARD_PADDING, BOARD_PADDING);
            var currentPos = startingPos;
            //First row of rooms
            Tiles.Add(new ClientBoardTile(studyTex, currentPos, Location.Study));
            currentPos.X += studyTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayOne, currentPos, Location.HallwayOne, HorizontalHallway: true));
            currentPos.X += hallwayOne.Width;
            Tiles.Add(new ClientBoardTile(concertHallTex, currentPos, Location.ConcertHall));
            currentPos.X += concertHallTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTwo, currentPos, Location.HallwayTwo, HorizontalHallway: true));
            Tiles.Add(new ClientBoardTile(missScarletHomeSquareTex, currentPos, Location.MissScarletHomeSquare, HorizontalHallwayHomeSquare:true));
            currentPos.X += hallwayTwo.Width;
            Tiles.Add(new ClientBoardTile(loungeTex, currentPos, Location.Lounge));
            currentPos.X += hallwayTwo.Width;
            TopMenuPosition.X = currentPos.X;
            TopMenuPosition.Y = currentPos.Y;

            //Second row
            currentPos.Y += hallwayThree.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(hallwayThree, currentPos, Location.HallwayThree, VerticalHallway: true));
            Tiles.Add(new ClientBoardTile(professorHomeSquareTex, currentPos, Location.ProfessorPlumHomeSquare, VerticalHallwayHomeSquare: true));
            currentPos.X += hallwayThree.Width;
            //Skip one spot
            currentPos.X += hallwayFour.Width;
            Tiles.Add(new ClientBoardTile(hallwayFour, currentPos, Location.HallwayFour, VerticalHallway: true));
            currentPos.X += hallwayFour.Width;
            //Skip one spot
            currentPos.X += hallwayFive.Width;
            Tiles.Add(new ClientBoardTile(hallwayFive, currentPos, Location.HallwayFive, VerticalHallway: true));
            Tiles.Add(new ClientBoardTile(colMustardHomeSquareTex, currentPos, Location.ColMustardHomeSquare, outSide:true,  VerticalHallwayHomeSquare: true));

            //Third Row
            currentPos.Y += hallwaySix.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(libraryTex, currentPos, Location.Library));
            currentPos.X += libraryTex.Width;
            Tiles.Add(new ClientBoardTile(hallwaySix, currentPos, Location.HallwaySix));
            currentPos.X += hallwaySix.Width;
            Tiles.Add(new ClientBoardTile(billiardTex, currentPos, Location.Billiard));
            currentPos.X += billiardTex.Width;
            Tiles.Add(new ClientBoardTile(hallwaySeven, currentPos, Location.HallwaySeven));
            currentPos.X += hallwaySeven.Width;
            Tiles.Add(new ClientBoardTile(diningRoomTex, currentPos, Location.DiningRoom));
            currentPos.X += diningRoomTex.Width;

            BottomMenuPosition.X = currentPos.X;

            //Fourth row
            currentPos.Y += hallwayEight.Height;
            BottomMenuPosition.Y = currentPos.Y;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(hallwayEight, currentPos, Location.HallwayEight));
            Tiles.Add(new ClientBoardTile(missPeacockHomeSquareTex, currentPos, Location.MrsPeacockHomeSquare, VerticalHallwayHomeSquare: true));
            currentPos.X += hallwayEight.Width;
            //Skip one spot
            currentPos.X += hallwayNine.Width;
            Tiles.Add(new ClientBoardTile(hallwayNine, currentPos, Location.HallwayNine));
            currentPos.X += hallwayNine.Width;
            //Skip one spot
            currentPos.X += hallwayTen.Width;
            Tiles.Add(new ClientBoardTile(hallwayTen, currentPos, Location.HallwayTen));


            //Fifth Row
            currentPos.Y += hallwayEleven.Height;
            currentPos.X = startingPos.X;
            Tiles.Add(new ClientBoardTile(conservatoryTex, currentPos, Location.Conservatory));
            currentPos.X += conservatoryTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayEleven, currentPos, Location.HallwayEleven));
            Tiles.Add(new ClientBoardTile(mrGreenHomeSquareTex, currentPos, Location.MrGreenHomeSquare, outSide: true, HorizontalHallwayHomeSquare: true));
            currentPos.X += hallwayEleven.Width;
            Tiles.Add(new ClientBoardTile(ballroomTex, currentPos, Location.Ballroom));
            currentPos.X += ballroomTex.Width;
            Tiles.Add(new ClientBoardTile(hallwayTwelve, currentPos, Location.HallwayTwelve));
            Tiles.Add(new ClientBoardTile(missWhiteHomeSquareTex, currentPos, Location.MrsWhiteHomeSquare, outSide: true, HorizontalHallwayHomeSquare: true));
            currentPos.X += hallwayTwelve.Width;
            Tiles.Add(new ClientBoardTile(kitchenTex, currentPos, Location.Kitchen));
            currentPos.X += kitchenTex.Width;
            currentPos.Y += kitchenTex.Height;

            ClientMenuManager.Instance.SetBottomAnchorPosition(BottomMenuPosition);
            ClientMenuManager.Instance.SetTopAnchorPosition(TopMenuPosition);

        }
    }
}
