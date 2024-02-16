using Clue_Less.Managers;
using Clue_Less.Managers.Interfaces;

using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.ImGuiNet;
using System;
using Microsoft.Xna.Framework.Audio;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Clue_Less_Server;

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
        public Texture2D ballTexture;
        public float ballSpeed;
        protected Texture2D _background;

        public ClueLess()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;            
        }

        protected override void Initialize()
        {
            _colorV4 = Color.CornflowerBlue.ToVector4().ToNumerics();
            _toolActive = true;

            InitializeMusicAndSound();
            GuiRenderer = new ImGuiRenderer(this);

            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 200f;

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

            //int location = client.GetPlayerLocation(new Empty()).Response;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            GuiRenderer.RebuildFontAtlas();

            // TODO: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("gameobjects/ball");
            _background = Content.Load<Texture2D>("gameobjects/gameboard");
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.W))
            {
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.S))
            {
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.A))
            {
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.D))
            {
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
            {
                ballPosition.X = _graphics.PreferredBackBufferWidth - ballTexture.Width / 2;
            }
            else if (ballPosition.X < ballTexture.Width / 2)
            {
                ballPosition.X = ballTexture.Width / 2;
            }
            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
            {
                ballPosition.Y = _graphics.PreferredBackBufferHeight - ballTexture.Height / 2;
            }
            else if (ballPosition.Y < ballTexture.Height / 2)
            {
                ballPosition.Y = ballTexture.Height / 2;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GUI library: https://github.com/Mezo-hx/MonoGame.ImGuiNet/wiki/SampleImplementation
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.Clear(new Color(_colorV4));

            // TODO: Add your drawing code here

            //ball tutorial
            _spriteBatch.Begin();
            _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(ballTexture, ballPosition, null, Color.White, 0f, new Vector2(ballTexture.Width / 2, ballTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            _spriteBatch.End();

            base.Draw(gameTime);

            #region ----------------GUI Tutorial-------------------
            GuiRenderer.BeginLayout(gameTime);
            if (_toolActive)
            {
                ImGui.Begin("My first tool", ref _toolActive, ImGuiWindowFlags.MenuBar);
                if (ImGui.BeginMenuBar())
                {
                    if (ImGui.BeginMenu("File"))
                    {
                        if (ImGui.MenuItem("Open..", "Ctrl+O")) { /* Do stuff */ }
                        if (ImGui.MenuItem("Save", "Ctrl+S")) { /* Do stuff */ }
                        if (ImGui.MenuItem("Close", "Ctrl+W")) { _toolActive = false; }
                        ImGui.EndMenu();
                    }
                }
                ImGui.EndMenuBar();
            }

            // Edit a color stored as 4 floats
            ImGui.ColorEdit4("Color", ref _colorV4);

            // Generate samples and plot them
            var samples = new float[100];
            for (var n = 0; n < samples.Length; n++)
                samples[n] = (float)Math.Sin(n * 0.2f + ImGui.GetTime() * 1.5f);
            ImGui.PlotLines("Samples", ref samples[0], 100);

            // Display contents in a scrolling region
            ImGui.TextColored(new Vector4(1, 1, 0, 1).ToNumerics(), "Important Stuff");
            ImGui.BeginChild("Scrolling", new System.Numerics.Vector2(0), ImGuiChildFlags.None);
            for (var n = 0; n < 50; n++)
                ImGui.Text($"{n:0000}: Some text");
            ImGui.EndChild();

            ImGui.End();
            GuiRenderer.EndLayout();

            #endregion
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