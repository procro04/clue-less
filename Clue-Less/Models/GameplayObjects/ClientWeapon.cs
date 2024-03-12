using Clue_Less.Enums;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Models.GameplayObjects
{
    public class ClientWeapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TokenId { get; set; }
        public WeaponTokenEnum TokenValue { get; set; }
        public string ContentUrl { get; set; }
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }

    }
}
