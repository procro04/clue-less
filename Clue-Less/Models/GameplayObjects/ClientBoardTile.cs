using Clue_Less;
using Clue_Less.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Greet;
using Managers;

namespace Models.GameplayObjects
{
    public class ClientBoardTile
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _position;
        public readonly Location TileType;

        public class tokenSlot
        {
            public Vector2 RenderPosition;
            public int PlayerId;            
        }

        public List<tokenSlot> TokenSlots = new List<tokenSlot>();

        public ClientBoardTile(Texture2D texture, Vector2 position, Location tileType)
        {
            _texture = texture;
            _position = position;
            TileType = tileType;

            //I'm making two slot here, you'll need to make 6 total
            var newTokenSlot = new tokenSlot();
            newTokenSlot.RenderPosition = _position;
            TokenSlots.Add(newTokenSlot);
            newTokenSlot = new tokenSlot();
            newTokenSlot.RenderPosition.X = _position.X + TokenManager.Instance.PlayerTokenSize.X;
            newTokenSlot.RenderPosition.Y = _position.Y;
            TokenSlots.Add(newTokenSlot);
        }

        public void Draw()
        {
            Globals.Instance.SpriteBatch.Begin();
            Globals.Instance.SpriteBatch.Draw(_texture, _position, Color.White);
            Globals.Instance.SpriteBatch.End();
        }
    }
}
