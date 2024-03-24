using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.ImGuiNet;
using Microsoft.Xna.Framework.Audio;
using Managers;
using System.Diagnostics;
using Services;
using System.Timers;
using System.Linq;


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

        System.Timers.Timer heartbeatTimer = new System.Timers.Timer();

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
            Globals.Instance.Bounds = new(1400, 1080);
            Globals.Instance.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Instance.Content = Content;

            _graphics.PreferredBackBufferWidth = Globals.Instance.Bounds.X;
            _graphics.PreferredBackBufferHeight = Globals.Instance.Bounds.Y;
            _graphics.ApplyChanges();
            Window.Title = "Clue-Less: Just like the real game...but less!";

            //InitializeMusicAndSound();
            GuiRenderer = new ImGuiRenderer(Globals.Instance.Game);

            heartbeatTimer.Elapsed += new ElapsedEventHandler(PerformHeartbeat);
            heartbeatTimer.Interval = 500; // half second
            heartbeatTimer.Enabled = true;

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
            {
                Exit();
            }
            

            base.Update(gameTime);
        }        

        private static void PerformHeartbeat(object source,  ElapsedEventArgs e)
        {
            var response = ClientGRPCService.Instance.Heartbeat();
            if (response.Response == Greet.ServerHeartbeatResponse.StartGame)
            {
                ClientTokenManager.Instance.StartGame(response.StartGame);
            }

            if (response.Response == Greet.ServerHeartbeatResponse.CurrentTurn)
            {
                var player = ClientTokenManager.Instance.ClientPlayers.FirstOrDefault(x => x.PlayerId == response.CurrentTurn.PlayerId);
                ClientMenuManager.Instance.ShowNotification($"Currently {player.AssignedToken.Name}'s turn!");
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GuiRenderer.BeginLayout(gameTime);
            //GUI library: https://github.com/Mezo-hx/MonoGame.ImGuiNet/wiki/SampleImplementation
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            ClientBoardManager.Instance.Draw(gameTime);
            if (ClientMenuManager.Instance.GameInstanceStarted)
            {
                ClientTokenManager.Instance.Draw(gameTime);
            }            
                        
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