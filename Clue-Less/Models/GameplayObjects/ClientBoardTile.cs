using Clue_Less.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Clue_Less.Models.GameplayObjects
{
    public class ClientBoardTile
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _position;
        private readonly TileTypeEnum _tileType;

        public ClientBoardTile(Texture2D texture, Vector2 position, TileTypeEnum tileType)
        {
            _texture = texture;
            _position = position;
            _tileType = tileType;
        }   

        public void Draw()
        {
            Globals.Instance.SpriteBatch.Begin();
            Globals.Instance.SpriteBatch.Draw(_texture, _position, Color.White);
            Globals.Instance.SpriteBatch.End();
        }
    }
}
