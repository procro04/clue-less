using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Clue_Less
{
    public class Globals
    {
        private static readonly Lazy<Globals> lazy = new(() => new Globals());
        public static Globals Instance { get { return lazy.Value; } }

        public float Time { get; private set; }
        public ContentManager Content { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public Point Bounds { get; set; }
        public Game Game { get; set; }

        public void Update(GameTime gt)
        {
            Time = (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}