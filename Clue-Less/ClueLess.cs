using Clue_Less.Managers;
using Clue_Less.Managers.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Models.GameplayObjects;
using System;


namespace Clue_Less
{
    public class ClueLess : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public ClueLess()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;            
        }

        protected override void Initialize()
        {
            Song song = Content.Load<Song>("gamesounds/665215__danlucaz__mystery-loop-3"); //Sourced from: https://freesound.org/people/danlucaz/sounds/665215/
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.15f;

            //This is stupid and I hate it, but MonoGame gonna MonoGame
            //its DI is whack.
            TokenManager tokenManager = new TokenManager();
            BoardManager boardManager = new BoardManager();
            ValidationManager validationManager = new ValidationManager();

            Services.AddService(typeof(ITokenManager), tokenManager);
            Services.AddService(typeof(IBoardManager), boardManager);
            Services.AddService(typeof(IValidationManager), validationManager);

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


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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
            //GUI library: https://github.com/Mezo-hx/MonoGame.ImGuiNet/wiki/SampleImplementation
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}