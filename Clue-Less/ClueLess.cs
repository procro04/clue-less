using Clue_Less.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.ImGuiNet;
using Microsoft.Xna.Framework.Audio;
using Services;
using System;
using Managers;
using Clue_Less_Server;
using Grpc.Net.Client;
using System.Diagnostics;


namespace Clue_Less
{
    public class ClueLess : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public static ImGuiRenderer GuiRenderer;
        private bool _toolActive;
        private System.Numerics.Vector4 _colorV4;
        public Vector2 ballPosition;
        public int ballPosition_asPlayer = 0;
        public Texture2D ballTexture;
        public float ballSpeed;
        protected Texture2D _background;
        private int horizontalPixelOffsetForMovement = 50;
        private Vector2 startingPosition;

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
            Globals.Instance.Bounds = new(1920, 1080);
            Globals.Instance.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.Instance.Content = Content;

            _graphics.PreferredBackBufferWidth = Globals.Instance.Bounds.X;
            _graphics.PreferredBackBufferHeight = Globals.Instance.Bounds.Y;
            _graphics.ApplyChanges();
            Window.Title = "Clue-Less: Just like the real game...but less!";


            //Determine what to keep and what to get rid of below here
            _colorV4 = Color.CornflowerBlue.ToVector4().ToNumerics();
            _toolActive = true;

            InitializeMusicAndSound();
            GuiRenderer = new ImGuiRenderer(this);

            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            startingPosition = ballPosition;
            ballSpeed = 200f;

            Debug.WriteLine("Beginning Token Manager's Instantiation of Suspects/Players \n");
            var players = TokenManager.Instance.InitializePlayers();
            Debug.WriteLine("Our suspects");
            foreach (var player in players)
            {
                Debug.WriteLine(player.Name);
            }
            Debug.WriteLine("\n Ending Token Manager's Instantiation of Suspects/Players \n");

            Debug.WriteLine("Beginning Token Manager's Instantiation of Weapons \n");
            var weapons = TokenManager.Instance.InitializeWeapons();
            Debug.WriteLine("Possible murder weapons");
            foreach (var weapon in weapons)
            {
                Debug.WriteLine(weapon.Name);
            }

            Debug.WriteLine("\n End Token Manager's Instantiation of Weapons \n");
            // The port number must match the port of the gRPC server. 
            // TODO Revisit when hosted on Azure

            Debug.WriteLine("Beginning gRPC Client instantiation \n");
            var reply = GRPCService.Instance.SayHello("GreeterClient");
            Debug.WriteLine("Greetings, Earthling!: " + reply);
            Debug.WriteLine("End gRPC Client instantiation \n");

            //int location = client.GetPlayerLocation(new Empty()).Response;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            GuiRenderer.RebuildFontAtlas();

            // TODO: use this.Content to load your game content here
        }
        protected override void Update(GameTime gameTime)
        {
            Globals.Instance.Update(gameTime);

            //TODO: Determine what is needed under here
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Use monogame events for key presses or rende
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.D))
            {
                ballPosition_asPlayer = GRPCService.Instance.MovePlayerLocation(0, ballPosition_asPlayer + 1);
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                ballPosition_asPlayer = GRPCService.Instance.MovePlayerLocation(0, ballPosition_asPlayer - 1);
            }

            //this will go in the ball position render logic
            ballPosition.X = startingPosition.X + (ballPosition_asPlayer * horizontalPixelOffsetForMovement);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GUI library: https://github.com/Mezo-hx/MonoGame.ImGuiNet/wiki/SampleImplementation
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            ClientBoardManager.Instance.Draw();

            base.Draw(gameTime);
        }

        protected void InitializeMusicAndSound()
        {
            //Music
            Song music = Content.Load<Song>("gamesounds/665215__danlucaz__mystery-loop-3"); //Sourced from: https://freesound.org/people/danlucaz/sounds/665215/
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.15f;

            //Thunderstorm 
            SoundEffect ambientSounds = Content.Load<SoundEffect>("gamesounds/423610__nimlos__rain_thunder_loop");//Sourced from: https://freesound.org/people/Nimlos/sounds/423610/
            SoundEffectInstance ambientInstance = ambientSounds.CreateInstance();
            ambientInstance.IsLooped = true;
            ambientInstance.Volume = 0.5f;
            ambientInstance.Play();
        }
    }
}