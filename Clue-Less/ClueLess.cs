using Clue_Less.Managers;
using Clue_Less.Managers.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Grpc.Net.Client;
using Clue_Less_Server;


namespace Clue_Less
{
    public class ClueLess : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        protected Texture2D _background;

        public ClueLess()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;            
        }

        protected override void Initialize()
        {
            TokenManager tokenManager = new TokenManager();
            Services.AddService(typeof(ITokenManager), tokenManager);

            var players = tokenManager.InitializePlayers();
            System.Diagnostics.Debug.WriteLine("Our suspects");
            foreach (var player in players)
            {
                System.Diagnostics.Debug.WriteLine(player.Name);
            }

            var weapons = tokenManager.InitializeWeapons();
            System.Diagnostics.Debug.WriteLine("Possible murder weapons");
            foreach (var weapon in weapons)
            {
                System.Diagnostics.Debug.WriteLine(weapon.Name);
            }

            // The port number must match the port of the gRPC server. 
            // TODO Revisit when hosted on Azure
            using var channel = GrpcChannel.ForAddress("https://localhost:7052");
            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(new HelloRequest { Name = "GreeterClient" });
            System.Diagnostics.Debug.WriteLine("Greetings, Earthling!: " + reply.Message);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Render game content
            _background = Content.Load<Texture2D>("gameobjects/gameboard");
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Put any update logic on sprites here
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}