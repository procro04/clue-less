using Clue_Less;
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

        private const int ROOM_DIMENSION = 10;
        private const int TILE_SPACING = 10;
        private const int BOARD_PADDING = 50;
        public List<ClientBoardTile> Tiles { get; } = new List<ClientBoardTile>();

        public ClientBoardManager()
        {
            Texture2D[] rooms =
{
                Globals.Instance.Content.Load<Texture2D>("gamerooms/ballroom"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/ballroom"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/billiard"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/concert_hall"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/conservatory"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/dining_room"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/hallway"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/kitchen"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/library"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/lounge"),
                Globals.Instance.Content.Load<Texture2D>("gamerooms/study"),
            };

            var roomDistance = rooms[0].Width + TILE_SPACING;

            for (int i = 0; i < rooms.Length; i++)
            {
                var room = rooms[i];
                var x = (roomDistance * (i % ROOM_DIMENSION)) + BOARD_PADDING;
                var y = (roomDistance * (i % ROOM_DIMENSION)) + BOARD_PADDING;
                Tiles.Add(new(room, new(x, y)));
            }
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
