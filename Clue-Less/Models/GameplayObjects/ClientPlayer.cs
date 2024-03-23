using Greet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Models.GameplayObjects
{
    public class ClientPlayer
    {
        public string Name { get; set; }
        public int PlayerId { get; set; }
        public int TokenId { get; set; }
        public ClientAvailableToken AssignedToken { get; set; } = new ClientAvailableToken();
    }
}

