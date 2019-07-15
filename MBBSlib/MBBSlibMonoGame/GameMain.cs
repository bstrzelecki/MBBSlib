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

        protected static List<IDrawable> renderers = new List<IDrawable>();
        protected static List<IUpdateable> updates = new List<IUpdateable>();
        protected static List<IDrawable> queuedRenderers = new List<IDrawable>();
        protected static List<IUpdateable> queuedUpdates = new List<IUpdateable>();
        protected static List<IDrawable> rmQueuedRenderers = new List<IDrawable>();
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
            Content.RootDirectory = "Content";
            start = main;
        }

        public static void RegisterUpdate(IUpdateable update)
        {
            queuedUpdates.Add(update);
        }
        public static void RegisterRenderer(IDrawable renderer)
        {
            queuedRenderers.Add(renderer);
        }
        public static void UnregisterUpdate(IUpdateable update)
        {
            rmQueuedUpdates.Add(update);
        }
        public static void UnregisterRenderer(IDrawable renderer)
        {
            if(renderers.Contains(renderer))
                rmQueuedRenderers.Add(renderer);
            if (priorityRenderers.Contains(renderer))
                priorityRenderers.Remove(renderer);
        }
        public static void DealyUpdate(IUpdateable update)
        {
            rmQueuedUpdates.Add(update);
            queuedUpdates.Add(update);
        }
        public static void DealyRenderer(IDrawable renderer)
        {
            rmQueuedRenderers.Add(renderer);
            queuedRenderers.Add(renderer);
        }
        public static void RegisterPriorityRenderer(IDrawable renderer)
        {
            priorityRenderers.Add(renderer);
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
            textures.Add(id, Content.Load<Texture2D>(id));
        }
        public void LoadFont(string id)
        {
            fonts.Add(id, Content.Load<SpriteFont>(id));
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
        public Color BackgroundColor = Color.Black;
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);

            foreach(IDrawable draw in rmQueuedRenderers)
            {
                if (renderers.Contains(draw))
                    renderers.Remove(draw);
            }
            foreach (IDrawable update in queuedRenderers)
            {
                renderers.Add(update);
            }
            queuedRenderers.Clear();

            spriteBatch.Begin();
            foreach (IDrawable draw in priorityRenderers)
            {
                if (draw == null) continue;
                draw.Draw(spriteBatch);
            }

            if (renderers.Count <= 0) return;
            foreach (IDrawable draw in renderers)
            {
                if (draw == null) continue;
                    draw.Draw(spriteBatch);
            }

            spriteBatch.End();
        }
    }
}
