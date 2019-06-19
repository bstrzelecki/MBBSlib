using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MBBSlib.MonoGame
{
    public class GameMain : Game, IGetTexture
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private IStartingPoint start;

        protected static List<IDrawable> renderers = new List<IDrawable>();
        protected static List<IUpdateable> updates = new List<IUpdateable>();
        protected static List<IDrawable> queuedRenderers = new List<IDrawable>();
        protected static List<IUpdateable> queuedUpdates = new List<IUpdateable>();

        public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public Texture2D GetTexture(string key)
        {
            throw new NotImplementedException();
        }

        public bool ContainsTextureKey(string key)
        {
            throw new NotImplementedException();
        }

        public SpriteFont GetFont(string key)
        {
            throw new NotImplementedException();
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
        protected override void Initialize()
        {
            start.Start();
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

            foreach (IUpdateable update in queuedUpdates)
            {
                updates.Add(update);
            }

            queuedUpdates.Clear();

            foreach (IUpdateable update in updates)
            {
                update.Update();
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (IDrawable update in queuedRenderers)
            {
                renderers.Add(update);
            }

            queuedRenderers.Clear();

            spriteBatch.Begin();
            foreach (IDrawable draw in renderers)
            {
                draw.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
