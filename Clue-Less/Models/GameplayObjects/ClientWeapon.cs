using Microsoft.Xna.Framework.Graphics;
using System.Numerics;
using Greet;

namespace Models.GameplayObjects
{
    public class ClientWeapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TokenId { get; set; }
        public WeaponTokenEnum TokenValue { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

    }
}
