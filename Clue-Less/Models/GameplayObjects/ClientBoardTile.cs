using Clue_Less;
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
        private readonly Vector2 _size;

        public class tokenSlot
        {
            public Vector2 RenderPosition;
            public int PlayerId;            
        }

        public List<tokenSlot> TokenSlots = new List<tokenSlot>();

        public ClientBoardTile(Texture2D texture, Vector2 position, Location tileType, bool outSide = false, bool HorizontalHallwayHomeSquare = false, bool VerticalHallwayHomeSquare = false, bool HorizontalHallway = false, bool VerticalHallway = false)
        {
            _texture = texture;            
            TileType = tileType;

            //I'm making two slot here, you'll need to make 6 total
            if (HorizontalHallwayHomeSquare)
            {
                if (outSide)
                {
                    _position.X = position.X + (TokenManager.Instance.TileSize.X / 2) - (TokenManager.Instance.HomeSquareSize.X / 2);
                    _position.Y = position.Y + (TokenManager.Instance.TileSize.Y - TokenManager.Instance.HomeSquareSize.Y );
                }
                else
                {
                    _position.X = position.X + (TokenManager.Instance.TileSize.X / 2) - (TokenManager.Instance.HomeSquareSize.X / 2);
                    _position.Y = position.Y;
                }
                
                var newTokenSlot = new tokenSlot();
                newTokenSlot.RenderPosition.X = _position.X + ((TokenManager.Instance.HomeSquareSize.X - TokenManager.Instance.PlayerTokenSize.X)/2);
                newTokenSlot.RenderPosition.Y = _position.Y + ((TokenManager.Instance.HomeSquareSize.Y - TokenManager.Instance.PlayerTokenSize.Y)/2);
                TokenSlots.Add(newTokenSlot);
                _size = new Vector2(TokenManager.Instance.HomeSquareSize.X, TokenManager.Instance.HomeSquareSize.Y);
            }
            else if (VerticalHallwayHomeSquare)
            {
                
                var newTokenSlot = new tokenSlot();
                if (outSide)
                {
                    _position.X = position.X + (TokenManager.Instance.TileSize.X - TokenManager.Instance.HomeSquareSize.X);
                    _position.Y = position.Y + (TokenManager.Instance.TileSize.Y / 2) - (TokenManager.Instance.HomeSquareSize.Y / 2);                    
                }
                else
                {
                    _position.X = position.X;
                    _position.Y = position.Y + (TokenManager.Instance.TileSize.Y / 2) - (TokenManager.Instance.HomeSquareSize.Y / 2);                    
                }
                newTokenSlot.RenderPosition.X = _position.X + ((TokenManager.Instance.HomeSquareSize.X - TokenManager.Instance.PlayerTokenSize.X) / 2);
                newTokenSlot.RenderPosition.Y = _position.Y + ((TokenManager.Instance.HomeSquareSize.Y - TokenManager.Instance.PlayerTokenSize.Y) / 2);
                TokenSlots.Add(newTokenSlot);
                _size = new Vector2(TokenManager.Instance.HomeSquareSize.X, TokenManager.Instance.HomeSquareSize.Y);
            }
            else if(HorizontalHallway)
            {                
                var newTokenSlot = new tokenSlot();               
                _position = position;
                newTokenSlot.RenderPosition = _position;
                newTokenSlot.RenderPosition.X = _position.X + ((TokenManager.Instance.TileSize.X - TokenManager.Instance.PlayerTokenSize.X) / 2);
                newTokenSlot.RenderPosition.Y = _position.Y + ((TokenManager.Instance.TileSize.Y - TokenManager.Instance.PlayerTokenSize.Y) / 2);
                
                TokenSlots.Add(newTokenSlot);
                _size = new Vector2(TokenManager.Instance.TileSize.X, TokenManager.Instance.TileSize.Y);
            }
            else if(VerticalHallway)
            {
                _position = position;
                var newTokenSlot = new tokenSlot();
                newTokenSlot.RenderPosition = _position;
                newTokenSlot.RenderPosition.X = _position.X + ((TokenManager.Instance.TileSize.X - TokenManager.Instance.PlayerTokenSize.X) / 2);
                newTokenSlot.RenderPosition.Y = _position.Y + ((TokenManager.Instance.TileSize.Y - TokenManager.Instance.PlayerTokenSize.Y) / 2);
                TokenSlots.Add(newTokenSlot);
                _size = new Vector2(TokenManager.Instance.TileSize.X, TokenManager.Instance.TileSize.Y);
            }
            else
            {
                _position = position;
                //Rooms need six slots total, I leave it  to you to implement.
                var newTokenSlot = new tokenSlot();
                newTokenSlot.RenderPosition = _position;
                TokenSlots.Add(newTokenSlot);
                newTokenSlot = new tokenSlot();
                newTokenSlot.RenderPosition.X = _position.X + TokenManager.Instance.PlayerTokenSize.X;
                newTokenSlot.RenderPosition.Y = _position.Y;
                TokenSlots.Add(newTokenSlot);
                _size = new Vector2(TokenManager.Instance.TileSize.X, TokenManager.Instance.TileSize.Y);
            }
        }

        public void Draw()
        {
            Globals.Instance.SpriteBatch.Begin();
            Globals.Instance.SpriteBatch.Draw(_texture, new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y), Color.White);
            Globals.Instance.SpriteBatch.End();
        }
    }
}
