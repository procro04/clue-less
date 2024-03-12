using Microsoft.Xna.Framework.Graphics;
using System.Numerics;

namespace Models.GameplayObjects
{
    public class ClientCard
    {
        private readonly Texture2D _texture;
        public Vector2 Position { get; set; }

        public ClientCard(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            Position = position;
        }
    }
}
