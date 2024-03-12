using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.ImGuiNet;
using Microsoft.Xna.Framework.Audio;
using Managers;
using System.Diagnostics;


namespace Clue_Less
{
    public class ClueLess : Game
    {
        private GraphicsDeviceManager _graphics;
        public Vector2 ballPosition;
        public int ballPosition_asPlayer = 0;
        public Texture2D ballTexture;
        public float ballSpeed;
        protected Texture2D _background;
        private int horizontalPixelOffsetForMovement = 50;
        private Vector2 startingPosition;
        public static ImGuiRenderer GuiRenderer;

        public ClueLess()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;            
        }

        protected override void Initialize()
        {
            //Globals inits
            Globals.Instance.Game = this;
            Globals.Instance.Bounds = new(1400, 1000);
            Globals.Instance.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Instance.Content = Content;

            _graphics.PreferredBackBufferWidth = Globals.Instance.Bounds.X;
            _graphics.PreferredBackBufferHeight = Globals.Instance.Bounds.Y;
            _graphics.ApplyChanges();
            Window.Title = "Clue-Less: Just like the real game...but less!";

            InitializeMusicAndSound();
            GuiRenderer = new ImGuiRenderer(Globals.Instance.Game);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GuiRenderer.RebuildFontAtlas();
        }
        protected override void Update(GameTime gameTime)
        {
            Globals.Instance.Update(gameTime);

            //TODO: Determine what is needed under here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Use monogame events for key presses or render
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.D))
            {
                //ballPosition_asPlayer = ClientGRPCService.Instance.MovePlayerLocation(0, ballPosition_asPlayer + 1);
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                //ballPosition_asPlayer = ClientGRPCService.Instance.MovePlayerLocation(0, ballPosition_asPlayer - 1);
            }

            //this will go in the ball position render logic
            ballPosition.X = startingPosition.X + (ballPosition_asPlayer * horizontalPixelOffsetForMovement);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GuiRenderer.BeginLayout(gameTime);
            //GUI library: https://github.com/Mezo-hx/MonoGame.ImGuiNet/wiki/SampleImplementation
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            ClientBoardManager.Instance.Draw(gameTime);
            TokenManager.Instance.Draw(gameTime);            
                        
            base.Draw(gameTime);
            GuiRenderer.EndLayout();
        }

        protected void InitializeMusicAndSound()
        {
            Debug.WriteLine("Begin Content Load \n");
            Debug.WriteLine("Initialize Music\n");
            //Music
            Song music = Content.Load<Song>("gamesounds/665215__danlucaz__mystery-loop-3"); //Sourced from: https://freesound.org/people/danlucaz/sounds/665215/
            Debug.WriteLine("Music title: " + music.Name + "\n");
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.15f;

            Debug.WriteLine("Initialize Thunderstorm \n");
            //Thunderstorm 
            SoundEffect ambientSounds = Content.Load<SoundEffect>("gamesounds/423610__nimlos__rain_thunder_loop");//Sourced from: https://freesound.org/people/Nimlos/sounds/423610/
            Debug.WriteLine("Sound effect name: " +  ambientSounds.Name + "\n");
            SoundEffectInstance ambientInstance = ambientSounds.CreateInstance();
            ambientInstance.IsLooped = true;
            ambientInstance.Volume = 0.5f;
            ambientInstance.Play();
            Debug.WriteLine("End Content Load");
        }
    }
}