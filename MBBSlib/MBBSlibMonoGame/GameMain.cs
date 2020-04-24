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
        private SpriteBatch spriteBatch;
        /// <summary>
        /// Last copy of GameMain class
        /// </summary>
        public static GameMain Instance { get; private set; }
        public Camera3D camera3D = new Camera3D();
        public Camera camera2D = new Camera2D();
        private readonly IStartingPoint start;

        private static readonly Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static readonly Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
        private static readonly Dictionary<string, Model> models = new Dictionary<string, Model>();
        /// <summary>
        /// Returns a texture that corresponds to a given key
        /// </summary>
        /// <param name="key">key of a textur</param>
        /// <returns>2D texture</returns>
        public Texture2D GetTexture(string key)
        {
            if (ContainsTextureKey(key))
            {
                return textures[key];
            }
            return null;
        }
        public Model GetModel(string key)
        {
            if (ContainsModel(key))
            {
                return models[key];
            }
            return null;
        }
        public bool ContainsModel(string key)
        {
            return models.ContainsKey(key);
        }
        /// <summary>
        /// Checks if the registry contains specified key
        /// </summary>
        /// <param name="key">key of a texture</param>
        /// <returns></returns>
        public bool ContainsTextureKey(string key)
        {
            return textures.ContainsKey(key);

        }
        /// <summary>
        /// Returns specified font from the registry 
        /// </summary>
        /// <param name="key">key of a font</param>
        /// <returns>spritefont reference</returns>
        public SpriteFont GetFont(string key)
        {
            if (fonts.ContainsKey(key))
            {
                return fonts[key];
            }
            return null;
        }
        /// <summary>
        /// Base class of a game
        /// </summary>
        /// <param name="main"></param>
        public GameMain(IStartingPoint main)
        {
            graphics = new GraphicsDeviceManager(this);
            Instance = this;
            Content.RootDirectory = "Content";
            start = main;
        }

        public static void RegisterUpdate(IUpdateable update)
        {
            queuedUpdates.Add(update);
        }
        public static void RegisterRenderer(IDrawable renderer, int layer = 5)
        {
            queuedRenderers.Add(new Renderer(layer, renderer));
        }
        public static void UnregisterUpdate(IUpdateable update)
        {
            rmQueuedUpdates.Add(update);
        }
        public static void UnregisterRenderer(IDrawable renderer, int layer = 5)
        {
            Renderer r = new Renderer(layer, renderer);
            if (renderers.Contains(r))
                rmQueuedRenderers.Add(r);
        }
        protected override void Initialize()
        {
            Sprite.TextureStorage = this;
            start.Start(this);
            Time.Initialize();
            IsMouseVisible = true;
            InitializeComponents();
            new InputBindHandler();
            base.Initialize();
        }

        private void InitializeComponents()
        {
            Assembly ass = Assembly.GetEntryAssembly();
            foreach(var a in ass.GetTypes())
            {
                foreach(Attribute atr in Attribute.GetCustomAttributes(a))
                {
                    if(atr is GameComponent)
                    {
                        if (a.GetInterface("IDrawable") != null && a.GetInterface("IUpdateable") != null)
                        {
                            object o = Activator.CreateInstance(a);
                            RegisterRenderer((IDrawable)o);
                            RegisterUpdate((IUpdateable)o);
                        }
                        else if (a.GetInterface("IDrawable") != null )
                        {
                            object o = Activator.CreateInstance(a);
                            RegisterRenderer((IDrawable)o);
                        } 
                        else if (a.GetInterface("IUpdateable") != null)
                        {
                            object o = Activator.CreateInstance(a);
                            RegisterUpdate((IUpdateable)o);
                        }
                    }
                }
                
            }
        }

        private Resolution _res = Resolution.HDp;
        public Resolution Resolution
        {
            get
            {
                return _res;
            }
            set
            {
                SetResolution(value);
            }
        }
        public void SetResolution(Resolution resolution)
        {
            SetResolution(resolution.Width, resolution.Height);
        }
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            try
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\Content");

                foreach (string file in files)
                {
                    string f = file.Remove(0, file.LastIndexOf('\\') + 1);
                    f = f.Remove(f.Length - 4, 4);
                    Debug.WriteLine("Trying to load " + f);
                    if (!textures.ContainsKey(f) || fonts.ContainsKey(f))
                    {
                        Load(f);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Data);
            }
        }
        public void Load(string id)
        {
            try
            {
                textures.Add(id, Content.Load<Texture2D>(id));
                Debug.WriteLine("Loaded sprite: " + id);
            }
            catch
            {
                try
                {
                    LoadFont(id);
                    Debug.WriteLine("Loaded font: " + id);
                }
                catch
                {
                    try
                    {
                        models.Add(id, Content.Load<Model>(id));
                        Debug.WriteLine("Loaded model: " + id);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString());
                    }
                }
            }
        }
        public void LoadFont(string id)
        {
            try
            {
                fonts.Add(id, Content.Load<SpriteFont>(id));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
        public static bool DebugExit = false;
        protected override void Update(GameTime gameTime)
        {

            if (DebugExit && (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)))
            {
                Exit();
            }
            foreach (IUpdateable update in rmQueuedUpdates)
            {
                if (updates.Contains(update))
                    updates.Remove(update);
            }

            foreach (IUpdateable update in queuedUpdates)
            {
                updates.Add(update);
            }

            queuedUpdates.Clear();
            if (updates.Count <= 0) return;
            foreach (IUpdateable update in updates)
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
        public static GraphicsDevice graphicsDevice { get { return Instance.GraphicsDevice; } }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);



            foreach (Renderer draw in rmQueuedRenderers)
            {
                if (renderers.Contains(draw))
                    renderers.Remove(draw);
            }
            foreach (Renderer update in queuedRenderers)
            {
                renderers.Add(update);
                renderers = renderers.OrderBy(n => n.layer).ToList();
            }
            queuedRenderers.Clear();


            if (renderers.Count <= 0) return;
            spriteBatch.Begin();
            RenderBatch batch = new RenderBatch(spriteBatch, GraphicsDevice);
            foreach (Renderer draw in renderers)
            {
                draw.drawable.Draw(batch);
            }

            spriteBatch.End();
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
        public Vector2 Size { get { return new Vector2(Width, Height); } }

        /// <summary>
        /// 3840x2160
        /// </summary>
        public static Resolution UHD { get { return new Resolution(3840, 2160); } }
        /// <summary>
        /// 3200x1800
        /// </summary>
        public static Resolution QXGA { get { return new Resolution(3200, 1800); } }
        /// <summary>
        /// 2560x1440
        /// </summary>
        public static Resolution QHD { get { return new Resolution(2560, 1440); } }
        /// <summary>
        /// 2048x1152
        /// </summary>
        public static Resolution QWXGA { get { return new Resolution(2048, 1152); } }
        /// <summary>
        /// 1920x1080
        /// </summary>
        public static Resolution FHD { get { return new Resolution(1920, 1080); } }
        /// <summary>
        /// 1600x900
        /// </summary>
        public static Resolution HDp { get { return new Resolution(1600, 900); } }
        /// <summary>
        /// 1280x720
        /// </summary>
        public static Resolution XGA { get { return new Resolution(1280, 720); } }
        /// <summary>
        /// 960x540
        /// </summary>
        public static Resolution qHD { get { return new Resolution(960, 540); } }

        private Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
