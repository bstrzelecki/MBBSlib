using MBBSlib.MonoGame._3D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MBBSlib.MonoGame
{
    public partial class GameMain : Game, IGetTexture
    {
        /// <summary>
        /// Defoult menager of graphics device
        /// </summary>
        public static GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        /// <summary>
        /// Last copy of GameMain class
        /// </summary>
        public static GameMain Instance { get; private set; }
        public Camera3D camera3D;
        public Camera camera2D = new Camera2D();
        private readonly IStartingPoint _start;

        private static readonly Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        private static readonly Dictionary<string, SpriteFont> _fonts = new Dictionary<string, SpriteFont>();
        private static readonly Dictionary<string, Model> _models = new Dictionary<string, Model>();

        private static readonly Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
        private static void AddSingleton(Type t, object obj)
        {
            if(_singletons.ContainsKey(t)) return;
            _singletons.Add(t, obj);
        }
        public static T GetGameComponent<T>() => (T)_singletons[typeof(T)];
        /// <summary>
        /// Returns a texture that corresponds to a given key
        /// </summary>
        /// <param name="key">key of a textur</param>
        /// <returns>2D texture</returns>
        public Texture2D GetTexture(string key) => ContainsTextureKey(key) ? _textures[key] : null;
        public Model GetModel(string key) => ContainsModel(key) ? _models[key] : null;
        public bool ContainsModel(string key) => _models.ContainsKey(key);
        /// <summary>
        /// Checks if the registry contains specified key
        /// </summary>
        /// <param name="key">key of a texture</param>
        /// <returns></returns>
        public bool ContainsTextureKey(string key) => _textures.ContainsKey(key);
        /// <summary>
        /// Returns specified font from the registry 
        /// </summary>
        /// <param name="key">key of a font</param>
        /// <returns>spritefont reference</returns>
        public SpriteFont GetFont(string key) => _fonts.ContainsKey(key) ? _fonts[key] : null;
        /// <summary>
        /// Base class of a game
        /// </summary>
        /// <param name="main"></param>
        public GameMain()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        [Obsolete]
        public GameMain(IStartingPoint main) : this() => _start = main;

        public static void RegisterUpdate(IUpdateable update) => _queuedUpdates.Add(update);
        public static void RegisterRenderer(IDrawable renderer, int layer = 5) => _queuedRenderers.Add(new Renderer(layer, renderer));
        public static void UnregisterUpdate(IUpdateable update) => _rmQueuedUpdates.Add(update);
        public static void UnregisterRenderer(IDrawable renderer, int layer = 5)
        {
            var r = new Renderer(layer, renderer);
            if(_renderers.Contains(r))
                _rmQueuedRenderers.Add(r);
        }
        protected override void Initialize()
        {
            Sprite.TextureStorage = this;
            IsMouseVisible = true;
            InitializeComponents();
            camera3D = new Camera3D(GraphicsDevice, Window);
            _start?.Start(this);
            Time.Initialize();
            new InputBindHandler();

            base.Initialize();
        }

        private void InitializeComponents()
        {
            var ass = Assembly.GetEntryAssembly();
            foreach(var a in ass.GetTypes())
            {
                foreach(Attribute atr in Attribute.GetCustomAttributes(a))
                {
                    if(atr is GameComponent)
                    {
                        object o = Activator.CreateInstance(a);
                        AddSingleton(a, o);
                        if(a.GetInterface("IDrawable") != null)
                        {
                            RegisterRenderer((IDrawable)o);
                        }
                        if(a.GetInterface("IUpdateable") != null)
                        {
                            RegisterUpdate((IUpdateable)o);
                        }
                    }
                }

            }
        }

        private Resolution _res = Resolution.HDp;
        public Resolution Resolution
        {
            get => _res;
            set => SetResolution(value);
        }
        public void SetResolution(Resolution resolution) => SetResolution(resolution.Width, resolution.Height);
        public void SetResolution(int width, int height)
        {
            _res.Height = height;
            _res.Width = width;
            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            try
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\Content");

                foreach(string file in files)
                {
                    string f = file.Remove(0, file.LastIndexOf('\\') + 1);
                    f = f.Remove(f.Length - 4, 4);
                    Debug.WriteLine("Trying to load " + f);
                    if(!_textures.ContainsKey(f) || _fonts.ContainsKey(f))
                    {
                        Load(f);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Data);
            }
        }
        public void Load(string id)
        {
            try
            {
                _textures.Add(id, Content.Load<Texture2D>(id));
                Debug.WriteLine("Loaded sprite: " + id);
            }
            catch
            {
                try
                {
                    _fonts.Add(id, Content.Load<SpriteFont>(id));
                    Debug.WriteLine("Loaded font: " + id);
                }
                catch
                {
                    try
                    {
                        _models.Add(id, Content.Load<Model>(id));
                        Debug.WriteLine("Loaded model: " + id);
                    }
                    catch(Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }
            }
        }
        public static bool DebugExit = false;
        public static bool IsMouseCentered = false;
        public GameTime gameTime;
        protected override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            if(DebugExit && (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                Exit();
            }
            if(IsMouseCentered)
            {
                Mouse.SetPosition(Resolution.Width / 2, Resolution.Height / 2);
            }
            foreach(IUpdateable update in _rmQueuedUpdates)
            {
                if(_updates.Contains(update))
                    _updates.Remove(update);
            }

            foreach(IUpdateable update in _queuedUpdates)
            {
                _updates.Add(update);
            }

            _queuedUpdates.Clear();
            if(_updates.Count <= 0) return;
            foreach(IUpdateable update in _updates)
            {
                update.Update();
            }
        }
        /// <summary>
        /// Color of an background
        /// </summary>
        public Color BackgroundColor = Color.Black;


        /// <summary>
        /// Defoult graphic device
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public static GraphicsDevice graphicsDevice => Instance.GraphicsDevice;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);



            foreach(Renderer draw in _rmQueuedRenderers)
            {
                if(_renderers.Contains(draw))
                    _renderers.Remove(draw);
            }
            foreach(Renderer update in _queuedRenderers)
            {
                _renderers.Add(update);
                _renderers = _renderers.OrderBy(n => n.layer).ToList();
            }
            _queuedRenderers.Clear();


            if(_renderers.Count <= 0) return;
            _spriteBatch.Begin();
            var batch = new RenderBatch(_spriteBatch, GraphicsDevice);
            foreach(Renderer draw in _renderers)
            {
                draw.drawable.Draw(batch);
            }

            _spriteBatch.End();
        }
    }
    /// <summary>
    /// Registry of supported resolutions
    /// </summary>
    public struct Resolution
    {

        /// <summary>
        /// Width in pixels
        /// </summary>
        public int Width;
        /// <summary>
        /// Height in pixels
        /// </summary>
        public int Height;
        /// <summary>
        /// SIze of windown in vector2
        /// </summary>
        public Vector2 Size => new Vector2(Width, Height);

        /// <summary>
        /// 3840x2160
        /// </summary>
        public static Resolution UHD => new Resolution(3840, 2160);
        /// <summary>
        /// 3200x1800
        /// </summary>
        public static Resolution QXGA => new Resolution(3200, 1800);
        /// <summary>
        /// 2560x1440
        /// </summary>
        public static Resolution QHD => new Resolution(2560, 1440);
        /// <summary>
        /// 2048x1152
        /// </summary>
        public static Resolution QWXGA => new Resolution(2048, 1152);
        /// <summary>
        /// 1920x1080
        /// </summary>
        public static Resolution FHD => new Resolution(1920, 1080);
        /// <summary>
        /// 1600x900
        /// </summary>
        public static Resolution HDp => new Resolution(1600, 900);
        /// <summary>
        /// 1280x720
        /// </summary>
        public static Resolution XGA => new Resolution(1280, 720);


        /// <summary>
        /// 960x540
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Current nameing style will create disambiguation between qHD and QHD")]
        public static Resolution qHD => new Resolution(960, 540);

        private Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
