using ditto;
using ditto.mono;
using GameForAndroid.Elements;
using GameForAndroid.Elements.Gameplay;
using GameForAndroid.Elements.Ships;
using GameForAndroid.Levels;
using GameForAndroid.Save;
using GameForAndroid.UI;
using GameForAndroid.UI.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameForAndroid
{
    enum State
    {
        Start,
        Menu,
        GameStory,
        GameEndless,
        Shop,
        Inventory,
        GameOver,
        FinalLevel,
        Pause
    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State _state = State.Start;
        private State _lastState;

        private ControlButton _pause;

        private CCLabel score;
        private CCLabel hightScore;

        private CCLabel LevelName;
        private CCLabel NeededScore;

        private StartScreen StartScreen;
        private GameOverScreen GameOverScreen;
        private VictoryScreen FinishScreen;
        private ShopScreen ShopScreen;
        private InventoryScreen InventoryScreen;
        private MenuScreen MenuScreen;
        private PauseScreen PauseScreen;

        private Player Player;

        private CCImage CCImage;

        public static int Score = 0;
        public static int HightScore = 0;


        private bool ShopClose;
        private bool InventoryClose;
        private bool PauseClose;

        private double TimeStart;

        private TouchPanelState TouchPanelState;
        private float _waitShoot;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this) { IsFullScreen = true, SupportedOrientations = DisplayOrientation.LandscapeLeft, PreferMultiSampling = true, SynchronizeWithVerticalRetrace = true, PreferredBackBufferFormat = SurfaceFormat.Color };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            TouchPanelState = TouchPanel.GetState(Window);
            TouchPanelState.EnabledGestures = GestureType.FreeDrag;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            FontManager.Register("RobotoBlack", Content.Load<SpriteFont>("RobotoBlack"));
            CCMonoHelper.InitializeContent(Content);

            //CCImage = new CCImage(Content.Load<Texture2D>("Background"));

            LoadState();

            score = new CCLabel($"Score: {Score}", "RobotoBlack", 30, CCTextAlignment.Center)
            {
                Color = CCColors.White
            };

            hightScore = new CCLabel($"HightScore: {HightScore}", "RobotoBlack", 30, CCTextAlignment.Center)
            {
                Color = CCColors.White
            };

            Texture2D pauseTex = Content.Load<Texture2D>("Pause");

            _pause = new ControlButton(pauseTex, Vector2.Zero, pauseTex.Width, pauseTex.Height);
            _pause.Position = new Vector2(GraphicsDevice.Viewport.Width - _pause.Texture.Width - 50, 20);

            Stars.CountStars = 100;
            Asteroids.CountAsteroids = 15;

            Stars.Width = GraphicsDevice.Viewport.Width;
            Stars.Height = GraphicsDevice.Viewport.Height;

            Asteroids.Width = GraphicsDevice.Viewport.Width;
            Asteroids.Height = GraphicsDevice.Viewport.Height;

            EnemyShips.Width = GraphicsDevice.Viewport.Width;
            EnemyShips.Height = GraphicsDevice.Viewport.Height;

            Sounds.DiedSound = Content.Load<SoundEffect>("DiedSound");
            Sounds.BackgroundSound = Content.Load<Song>("BackgroundSong");
            Sounds.FireHitSound = Content.Load<SoundEffect>("FireHitSound");
            Sounds.FireSound = Content.Load<SoundEffect>("FireSound");
            Sounds.ShipHitSound = Content.Load<SoundEffect>("ShipHit");
            Sounds.ShipDestroySound = Content.Load<SoundEffect>("ShipExplosion");

            LevelName = new CCLabel("text", "RobotoBlack", 60, CCTextAlignment.Center) { Color = CCColors.White };
            NeededScore = new CCLabel("text", "RobotoBlack", 60, CCTextAlignment.Center) { Color = CCColors.White };

            // TODO: use this.Content to load your game content here
        }

        private void LoadState()
        {
            try
            {
                using FileStream fileStream = new FileStream(Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath + "/Data.dat", FileMode.OpenOrCreate);

                BinaryFormatter binaryFormatter = new BinaryFormatter();

                if (binaryFormatter.Deserialize(fileStream) is SaveObject saveState)
                {
                    Player = new Player(saveState.PlayerShip, Vector2.Zero);

                    Player.Position = new Vector2(GraphicsDevice.Viewport.Bounds.X + 60, GraphicsDevice.Viewport.Bounds.Center.Y - (Player.Texture.Height / 2));

                    HightScore = saveState.Highscore;

                    Level.CurrentLevel = saveState.CurrentLevel;

                    Inventory.Coins = saveState.Coints;

#if DEBUG
                    Inventory.Coins = 500;
#endif

                    if (saveState.PurchaseShips.Count == 0)
                        Inventory.PurchasedShips.Add(ShipType.WhiteWarrior);
                    else
                        Inventory.PurchasedShips = saveState.PurchaseShips;
                }
                else
                {
                    Player = new Player(ShipType.WhiteWarrior, Vector2.Zero);

                    Player.Position = new Vector2(GraphicsDevice.Viewport.Bounds.X + 60, GraphicsDevice.Viewport.Bounds.Center.Y - (Player.Texture.Height / 2));

                    Inventory.PurchasedShips.Add(ShipType.WhiteWarrior);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while loading the game: " + ex.Message);

                Player = new Player(ShipType.WhiteWarrior, Vector2.Zero);

                Player.Position = new Vector2(GraphicsDevice.Viewport.Bounds.X + 60, GraphicsDevice.Viewport.Bounds.Center.Y - (Player.Texture.Height / 2));

                Inventory.PurchasedShips.Add(ShipType.WhiteWarrior);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            TouchCollection touches = TouchPanelState.GetState();

            switch (_state)
            {
                case State.Start:
                    UpdateStartGame(gameTime, touches);
                    break;
                case State.GameStory:
                    UpdateGame(gameTime, touches);
                    break;
                case State.FinalLevel:
                    UpdateFinalLevel(gameTime, touches);
                    break;
                case State.Pause:
                    UpdatePause(gameTime, touches);
                    break;
                case State.GameOver:
                    UpdateGameOver(gameTime, touches);
                    break;
                case State.Shop:
                    UpdateShop(gameTime, touches);
                    break;
                case State.Inventory:
                    UpdateInventory(gameTime, touches);
                    break;
                case State.Menu:
                    UpdateMenu(gameTime, touches);
                    break;
                case State.GameEndless:
                    UpdateGame(gameTime, touches);
                    break;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void UpdateMenu(GameTime gameTime, TouchCollection touches)
        {
            if(MenuScreen == null)
            {
                MenuScreen = new MenuScreen(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content.Load<Texture2D>("Background"))
                {
                    Position = CCPoint.Zero
                };
                MenuScreen.SetTitle(Content.Load<Texture2D>("Title"));
            }

            MenuScreen.Update(gameTime, touches);

            if (MenuScreen.Continue.IsPressed)
            {
                _state = State.GameStory;
                MenuScreen = null;
            }
            else if (MenuScreen.NewGame.IsPressed)
            {
                _state = State.GameStory;
                Level.CurrentLevel = LevelsName.Prologie;
                MenuScreen = null;
            }
            else if (MenuScreen.EndlessGame.IsPressed)
            {
                _state = State.GameEndless;
                Level.CurrentLevel = LevelsName.EndlessLevel;
                MenuScreen = null;
            }
            else if (MenuScreen.Shop.IsPressed)
                _state = State.Shop;
            else if (MenuScreen.Inventory.IsPressed)
                _state = State.Inventory;
            else if (MenuScreen.Exit.IsPressed)
            {
                UnloadContent();
                Exit();
            }

            _lastState = State.Menu;
        }
        private void UpdatePause(GameTime gameTime, TouchCollection touches)
        {
            if(PauseScreen == null)
            {
                PauseScreen = new PauseScreen(0, 0, CCColors.Transparent, Content.Load<Texture2D>("Pause"));
                PauseScreen.Position = new CCPoint(GraphicsDevice.Viewport.Bounds.Center.X - (PauseScreen.Width / 2), GraphicsDevice.Viewport.Height + 5);
            }

            if (!PauseClose && PauseScreen.Position.Y != GraphicsDevice.Viewport.Bounds.Center.Y - (PauseScreen.Height / 2))
                PauseScreen.Position = new CCPoint(PauseScreen.Position.X, PauseScreen.Position.Y - 20);

            if (!PauseClose && PauseScreen.Position.Y < GraphicsDevice.Viewport.Bounds.Center.Y - (PauseScreen.Height / 2))
                PauseScreen.Position = new CCPoint(PauseScreen.Position.X, GraphicsDevice.Viewport.Bounds.Center.Y - (PauseScreen.Height / 2));

            PauseScreen.Update(gameTime, touches);

            if (!PauseClose && (PauseScreen.Continue.IsPressed || PauseScreen.ToMenu.IsPressed))
                PauseClose = true;

            if (PauseClose)
            {
                if (PauseScreen.Position.Y < GraphicsDevice.Viewport.Height + 2)
                    PauseScreen.Position = new CCPoint(PauseScreen.Position.X, PauseScreen.Position.Y + 20);

                if (PauseScreen.Position.Y >= GraphicsDevice.Viewport.Height + 2)
                {
                    if(PauseScreen.ContinuePressed)
                    {
                        _state = Level.CurrentLevel == LevelsName.EndlessLevel ? State.GameEndless : State.GameStory;
                        PauseScreen.ContinuePressed = false;
                        PauseScreen = null;
                    }
                    else if(PauseScreen.MenuPressed)
                    {
                        _state = State.Menu;
                        PauseScreen.MenuPressed = false;
                        PauseScreen = null;
                    }

                    PauseClose = false;
                    _pause.IsPressed = false;
                }
            }

            _lastState = State.Pause;
        }
        private void UpdateStartGame(GameTime gameTime, TouchCollection touches)
        {
            if(StartScreen == null)
            {
                StartScreen = new StartScreen(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Content.Load<Texture2D>("Background"))
                {
                    Position = CCPoint.Zero,
                    Title = Content.Load<Texture2D>("Title")
                };
            }

            StartScreen.Update(gameTime, touches);

            if (StartScreen.StartButton.IsPressed)
            {
                _state = State.Menu;
                StartScreen = null;
            }

            _lastState = State.Start;
        }
        private void UpdateGame(GameTime gameTime, TouchCollection touches)
        {
            if (!Stars.isInit)
                Stars.Init(Content.Load<Texture2D>("Star"));

            Level.StartPlay = Level.CurrentLevel != LevelsName.EndlessLevel;

            _pause.Update(touches, gameTime);

            if (_pause.IsPressed)
            {
                _lastState = State.GameStory;
                _state = State.Pause;
                return;
            }

            Stars.Update();


            if (TimeStart <= 5.0)
            {
                Player.Update();

                LevelName.Text = $"{Level.CurrentLevel}";
                LevelName.Position = new CCPoint((GraphicsDevice.Viewport.Bounds.Center.X) - (LevelName.Width / 2), (GraphicsDevice.Viewport.Bounds.Center.Y) - (LevelName.Height / 2));

                NeededScore.Text = $"Reached {Level.NeededScore} score!";
                NeededScore.Position = new CCPoint(LevelName.RenderRectangle.Center.X - (NeededScore.Width / 2), LevelName.RenderRectangle.Bottom + 20);
            }
            else if (Score >= Level.NeededScore && Level.CurrentLevel != LevelsName.EndlessLevel)
            {
                Player.Update();

                if (Player.Health <= 0)
                {
                    MediaPlayer.Stop();

                    _lastState = State.GameStory;
                    _state = State.GameOver;

                    Sounds.DiedSound.Play(1, 0, 0);
                }

                PlayerMoveAndShot(gameTime);

                if (!EnemyShips.BossInitialized)
                    EnemyShips.InitializeBoss(Content.Load<Texture2D>(Level.CurrentBossName));


                if (EnemyShips.Boss.Direction == Vector2.Zero && Player.Rectangle.Y < EnemyShips.Boss.Rectangle.Y)
                    EnemyShips.Boss.Direction = new Vector2(0, -4);
                else if (EnemyShips.Boss.Direction == Vector2.Zero && Player.Rectangle.Y > EnemyShips.Boss.Rectangle.Y)
                    EnemyShips.Boss.Direction = new Vector2(0, 4);

                Asteroids.Update();

                EnemyShips.Update();

                if (EnemyShips.Boss.Destroy)
                {
                    _lastState = State.GameStory;
                    _state = State.FinalLevel;
                }
            }
            else
            {
                if (_lastState != State.GameStory && MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Volume = 50;
                    MediaPlayer.Play(Sounds.BackgroundSound);
                }

                if (!Asteroids.isInit)
                    Asteroids.Init(Content.Load<Texture2D>("asteroid"));

                if (!EnemyShips.isInit)
                    EnemyShips.Init(Content.Load<Texture2D>("Enemy"), Content.Load<Texture2D>("EnemyLaser"));

                Asteroids.Update();

                EnemyShips.Update();

                Player.Update();

                if (Player.Health <= 0)
                {
                    MediaPlayer.Stop();

                    _lastState = State.GameStory;
                    _state = State.GameOver;

                    Sounds.DiedSound.Play(1, 0, 0);
                }

                PlayerMoveAndShot(gameTime);

                score.Position = new CCPoint(10, 5);
                score.Text = $"Score: {Score}";
                score.Update(gameTime);

                if (HightScore > 0)
                {
                    hightScore.Position = new CCPoint(10, score.RenderRectangle.Bottom + 2);
                    hightScore.Text = $"Hight Score: {HightScore}";
                    hightScore.Update(gameTime);
                }
            }

            TimeStart += gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void PlayerMoveAndShot(GameTime gameTime)
        {
            if (TouchPanelState.IsGestureAvailable)
            {
                while (TouchPanelState.IsGestureAvailable)
                {
                    GestureSample gs = TouchPanelState.ReadGesture();
                    if (gs.GestureType == TouchPanelState.EnabledGestures)
                    {
                        if (gs.Delta.X > 0 || Player.Rectangle.Center.X < gs.Position.X)
                            Player.Right();
                        if (gs.Delta.Y > 0 || Player.Rectangle.Center.Y < gs.Position.Y)
                            Player.Down();
                        if (gs.Delta.X < 0 || Player.Rectangle.Center.X > gs.Position.X)
                            Player.Left();
                        if (gs.Delta.Y < 0 || Player.Rectangle.Center.Y > gs.Position.Y)
                            Player.Up();
                    }
                }
            }
            else
            {
                if (_waitShoot >= 500f)
                {
                    Player.Shoot();
                    _waitShoot = 0;
                }

                _waitShoot += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        private void UpdateFinalLevel(GameTime gameTime, TouchCollection touches)
        {
            if (FinishScreen == null)
            {
                Texture2D victory = Content.Load<Texture2D>("VictoryScreen");
                FinishScreen = new VictoryScreen(victory.Width, victory.Height, victory);
                FinishScreen.Position = new CCPoint(GraphicsDevice.Viewport.Bounds.Center.X - (FinishScreen.RenderRectangle.Width / 2), GraphicsDevice.Viewport.Bounds.Center.Y - (FinishScreen.RenderRectangle.Height / 2));
            }

            Level.StartPlay = false;

            if (_lastState == State.GameStory)
                Level.CurrentLevel++;

            Stars.Update();
            FinishScreen.Coints.Text = $"Coints: {Inventory.Coins}";
            FinishScreen.Position = new CCPoint(GraphicsDevice.Viewport.Bounds.Center.X - (FinishScreen.RenderRectangle.Width / 2), GraphicsDevice.Viewport.Bounds.Center.Y - (FinishScreen.RenderRectangle.Height / 2));
            FinishScreen.Update(gameTime, touches);

            if (FinishScreen.RestartButton.IsPressed)
            {
                Level.CurrentLevel--;

                Restart();
                _state = State.GameStory;
                FinishScreen = null;
            }
            else if (FinishScreen.NextButton.IsPressed)
            {
                Restart();
                _state = State.GameStory;
                FinishScreen = null;
            }
            else if (FinishScreen.ToMenu.IsPressed)
            {
                _state = State.Menu;
                FinishScreen = null;
            }

            TimeStart = 0;

            _lastState = State.FinalLevel;
        }
        private void UpdateGameOver(GameTime gameTime, TouchCollection touches)
        {
            if(GameOverScreen == null)
            {
                GameOverScreen = new GameOverScreen(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 3, Content.Load<Texture2D>("GameOver"))
                {
                    Position = new CCPoint(0, GraphicsDevice.Viewport.Height / 3)
                };
            }

            Level.StartPlay = false;

            Stars.Update();

            GameOverScreen.Score.Text = $"Score: {Score}";

            GameOverScreen.Update(gameTime, touches);

            if (Score > HightScore)
            {
                HightScore = Score;
                SaveState();
            }

            if (GameOverScreen.RestartButton.IsPressed)
            {
                Restart();
                _state = State.GameStory;
                GameOverScreen = null;
            }
            else if (GameOverScreen.ToMenu.IsPressed)
            {
                _state = State.Menu;
                GameOverScreen = null;
            }

            TimeStart = 0;

            _lastState = State.GameOver;
        }
        private void UpdateShop(GameTime gameTime, TouchCollection touches)
        {
            if(ShopScreen == null)
            {
                ShopScreen = new ShopScreen(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, new CCColor(0, 0, 10), Player.PlayerShip.ShipType)
                {
                    Position = new CCPoint(0, GraphicsDevice.Viewport.Height + 5)
                };
            }

            if (!ShopClose && ShopScreen.Position.Y != 0)
                ShopScreen.Position = new CCPoint(ShopScreen.Position.X, ShopScreen.Position.Y - 20);

            if (!ShopClose && ShopScreen.Position.Y < 0)
                ShopScreen.Position = new CCPoint(ShopScreen.Position.X, 0);

            ShopScreen.Update(gameTime, touches, Player);

            if (ShopScreen.CloseShop.IsPressed)
                ShopClose = true;

            if (ShopClose)
            {
                if (ShopScreen.Position.Y < GraphicsDevice.Viewport.Height + 2)
                    ShopScreen.Position = new CCPoint(ShopScreen.Position.X, ShopScreen.Position.Y + 20);
                if (ShopScreen.Position.Y >= GraphicsDevice.Viewport.Height + 2)
                {
                    _state = _lastState;
                    _lastState = State.Shop;
                    ShopClose = false;
                    ShopScreen = null;
                }
            }
        }
        private void UpdateInventory(GameTime gameTime, TouchCollection touches)
        {

            if(InventoryScreen == null)
            {
                InventoryScreen = new InventoryScreen(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, new CCColor(0, 0, 10), Player.PlayerShip.ShipType)
                {
                    Position = new CCPoint(0, GraphicsDevice.Viewport.Height + 5)
                };
            }

            if (!InventoryClose && InventoryScreen.Position.Y != 0)
                InventoryScreen.Position = new CCPoint(InventoryScreen.Position.X, InventoryScreen.Position.Y - 20);

            if (!InventoryClose && InventoryScreen.Position.Y < 0)
                InventoryScreen.Position = new CCPoint(InventoryScreen.Position.X, 0);

            InventoryScreen.Update(gameTime, touches, Player);

            if (InventoryScreen.CloseInventory.IsPressed)
                InventoryClose = true;

            if (InventoryClose)
            {
                if (InventoryScreen.Position.Y < GraphicsDevice.Viewport.Height + 2)
                    InventoryScreen.Position = new CCPoint(InventoryScreen.Position.X, InventoryScreen.Position.Y + 20);
                if (InventoryScreen.Position.Y >= GraphicsDevice.Viewport.Height + 2)
                {
                    _state = _lastState;
                    _lastState = State.Inventory;
                    InventoryClose = false;
                    InventoryScreen = null;
                }
            }
        }

        public void SaveState()
        {
            SaveObject saveState = new SaveObject
            {
                Highscore = HightScore,
                PlayerShip = Player.PlayerShip.ShipType,
                CurrentLevel = Level.CurrentLevel,
                Coints = Inventory.Coins,
                PurchaseShips = Inventory.PurchasedShips
            };

            try
            {
                using FileStream fileStream = new FileStream(Android.App.Application.Context.GetExternalFilesDir("").AbsolutePath + "/Data.dat", FileMode.Create);

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, saveState);
                Debug.WriteLine("Save is done");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred while saving the game: " + ex.Message);
            }
        }

        private void Restart()
        {
            Player.ReloadShip();
            Player.ClearShoots();
            Player.Position = new Vector2(GraphicsDevice.Viewport.Bounds.X + 60, GraphicsDevice.Viewport.Bounds.Center.Y - (Player.Texture.Height / 2));

            Score = 0;

            Asteroids.AsteroidsList.Clear();

            EnemyShips.ShipsList.Clear();
            EnemyShips.BossInitialized = false;
            EnemyShips.Boss = null;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.FromNonPremultiplied(0, 0, 10, 255));

            _spriteBatch.Begin(SpriteSortMode.Immediate);

            switch (_state)
            {
                case State.Start:
                    if (StartScreen != null)
                        StartScreen.Draw(GraphicsDevice);
                    break;
                case State.GameStory:
                    DrawGame();
                    break;
                case State.FinalLevel:
                    Stars.Draw(_spriteBatch);
                    if (FinishScreen != null)
                        FinishScreen.Draw(GraphicsDevice);
                    break;
                case State.Pause:
                    DrawGame();
                    if (PauseScreen != null)
                        PauseScreen.Draw(GraphicsDevice);
                    break;
                case State.GameOver:
                    Stars.Draw(_spriteBatch);
                    if(GameOverScreen != null)
                        GameOverScreen.Draw(GraphicsDevice);
                    break;
                case State.Shop:
                    switch (_lastState)
                    {
                        case State.Menu:
                            MenuScreen.Draw(GraphicsDevice);
                            break;
                    }
                    if (ShopScreen != null)
                        ShopScreen.Draw(GraphicsDevice);
                    break;
                case State.Inventory:
                    switch (_lastState)
                    {
                        case State.Menu:
                            MenuScreen.Draw(GraphicsDevice);
                            break;
                    }
                    if(InventoryScreen != null)
                        InventoryScreen.Draw(GraphicsDevice);
                    break;
                case State.Menu:
                    if(MenuScreen != null)
                        MenuScreen.Draw(GraphicsDevice);
                    break;
                case State.GameEndless:
                    DrawGame();
                    break;
            }

            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void DrawGame()
        {
            Stars.Draw(_spriteBatch);

            if (TimeStart <= 5.2)
            {
                Player.Draw(_spriteBatch);
                LevelName.Draw(GraphicsDevice);
                NeededScore.Draw(GraphicsDevice);
            }
            else if (Score >= Level.NeededScore && Level.CurrentLevel != LevelsName.EndlessLevel)
            {
                Player.Draw(_spriteBatch);

                Asteroids.Draw(_spriteBatch);

                EnemyShips.Draw(_spriteBatch);
            }
            else
            {
                Asteroids.Draw(_spriteBatch);
                EnemyShips.Draw(_spriteBatch);

                Player.Draw(_spriteBatch);

                score.Draw(GraphicsDevice);

                if (HightScore > 0)
                    hightScore.Draw(GraphicsDevice);
            }

            _pause.Draw(_spriteBatch);
        }
    }
}