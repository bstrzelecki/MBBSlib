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
    public class GameMain : Game, IGetTexture
    {
        public static GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private IStartingPoint start;
        public static GameMain lastCopy;
        protected struct Renderer
        {
            public int layer;
            public IDrawable drawable;
            public Renderer(int l, IDrawable draw)
            {
                layer = l;
                drawable = draw;
            }
            public override bool Equals(object obj)
            {
                if(obj is Renderer r)
                {
                    return ((r.layer == layer) && (r.drawable == drawable));
                }
                return false;
            }
        }
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
            if(renderers.Contains(r))
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
                    Debug.WriteLine("Trying to load "+f);
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
            }catch(Exception e)
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
            }catch (Exception e)
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
            foreach(IUpdateable update in rmQueuedUpdates)
            {
                if(updates.Contains(update))
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
        public static void Draw(this SpriteBatch sprite, Texture2D texture, Rectangle size)
        {
            sprite.Draw(texture, size, Color.White);
        }
        public static void Draw(this SpriteBatch sprite, Texture2D texture, Vector2 position)
        {
            sprite.Draw(texture, position, Color.White);
        }
        public Color BackgroundColor = Color.Black;
        public static GraphicsDevice graphicsDevice { get { return GameMain.lastCopy.GraphicsDevice; } }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            foreach(Renderer draw in rmQueuedRenderers)
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
}
