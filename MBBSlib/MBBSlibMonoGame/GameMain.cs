using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MBBSlib.MonoGame
{
    public partial class GameMain : Game, IGetTexture
    {
        public static GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private readonly BasicEffect basicEffect;

        private IStartingPoint start;
        public static GameMain lastCopy;
        protected static List<Renderer> renderers = new List<Renderer>();
        protected static List<IUpdateable> updates = new List<IUpdateable>();
        protected static List<Renderer> queuedRenderers = new List<Renderer>();
        protected static List<IUpdateable> queuedUpdates = new List<IUpdateable>();
        protected static List<Renderer> rmQueuedRenderers = new List<Renderer>();
        protected static List<IUpdateable> rmQueuedUpdates = new List<IUpdateable>();

        protected static List<IDrawable> priorityRenderers = new List<IDrawable>();
        public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public Texture2D GetTexture(string key)
        {
            if (ContainsTextureKey(key))
            {
                return textures[key];
            }
            return null;
        }

        public bool ContainsTextureKey(string key)
        {
            return textures.ContainsKey(key);

        }

        public SpriteFont GetFont(string key)
        {
            if (fonts.ContainsKey(key))
            {
                return fonts[key];
            }
            return null;
        }

        public GameMain(IStartingPoint main)
        {
            graphics = new GraphicsDeviceManager(this);
            lastCopy = this;
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
            RegisterUpdate(new Time());
            IsMouseVisible = true;
            base.Initialize();
        }
        public void SetResolution(Resolution resolution)
        {
            graphics.PreferredBackBufferWidth = resolution.Width;
            graphics.PreferredBackBufferHeight = resolution.Height;
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
                Debug.WriteLine("Loaded " + id);
            } catch (Exception e)
            {
                Debug.WriteLine("Error while loading sprite retrying " + e.ToString());
                LoadFont(id);
            }
        }
        public void LoadFont(string id)
        {
            try
            {
                fonts.Add(id, Content.Load<SpriteFont>(id));
            } catch (Exception e)
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

        public Color BackgroundColor = Color.Black;
        public static GraphicsDevice graphicsDevice { get { return GameMain.lastCopy.GraphicsDevice; } }
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
            foreach (Renderer draw in renderers)
            {
                draw.drawable.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
    public struct Resolution
    {
        public int Width;
        public int Height;
        public Vector2 Size { get { return new Vector2(Width, Height); } }

        /// <summary>
        /// 3840x2160
        /// </summary>
        public static Resolution UHD { get{ return new Resolution(3840, 2160); } } 
        /// <summary>
        /// 3200x1800
        /// </summary>
        public static Resolution QXGA { get{ return new Resolution(3200, 1800); } } 
        /// <summary>
        /// 2560x1440
        /// </summary>
        public static Resolution QHD { get{ return new Resolution(2560, 1440); } } 
        /// <summary>
        /// 2048x1152
        /// </summary>
        public static Resolution QWXGA { get{ return new Resolution(2048, 1152); } } 
        /// <summary>
        /// 1920x1080
        /// </summary>
        public static Resolution FHD { get{ return new Resolution(1920, 1080); } } 
        /// <summary>
        /// 1600x900
        /// </summary>
        public static Resolution HD { get{ return new Resolution(1600, 900); } } 
        /// <summary>
        /// 1280x720
        /// </summary>
        public static Resolution XGA { get{ return new Resolution(1280, 720); } } 
        /// <summary>
        /// 960x540
        /// </summary>
        public static Resolution qHD { get{ return new Resolution(960, 540); } } 

        private Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
