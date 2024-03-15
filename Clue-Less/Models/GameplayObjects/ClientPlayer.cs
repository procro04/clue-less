using Greet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Models.GameplayObjects
{
    public class ClientPlayer
    {
        public string Name { get; set; }
        public int TokenId { get; set; }
        public int PlayerId { get; set; }
        public Location CurrentLocation { get; set; }
        public PlayerCharacterOptions TokenValue { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 RenderPosition { get; set; }
    }
}

