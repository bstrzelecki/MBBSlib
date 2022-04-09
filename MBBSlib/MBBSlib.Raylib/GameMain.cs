using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using MBBSlib.Utility;
using MBBSlib.Visuals;
using Raylib_cs;

namespace MBBSlib.Raylib {
    public class GameMain : IGetTexture {

        public static GameMain Instance { get; private set; }
        private readonly Dictionary<string, Texture2D> _textures = new();
        private string _title = "Default title.";

        public string WindowTitle {
            get => _title;
            set {
                Raylib_cs.Raylib.SetWindowTitle(value);
                _title = value;
            }
        }

        public GameMain() {
            Instance = this;
        }
        
        public void Initialize() {
            Raylib_cs.Raylib.InitWindow(Resolution.Width, Resolution.Height, WindowTitle);
            Sprite.TextureStorage = this;
            LoadAll($"{Environment.CurrentDirectory}/Content");
        }

        public Texture2D GetTexture(string key) {
            if (_textures.ContainsKey(key)) return _textures[key];
            throw new Exception($"Missing texture: {key}.");
        }

        public bool ContainsTextureKey(string key) {
            return _textures.ContainsKey(key);
        }

        private Resolution _res = Resolution.HDp;

        public Resolution Resolution {
            get => _res;
            set => SetResolution(value);
        }

        public void SetResolution(Resolution resolution) => SetResolution(resolution.Width, resolution.Height);

        public void SetResolution(int width, int height) {
            _res.Height = height;
            _res.Width = width;
            Raylib_cs.Raylib.SetWindowSize(width, height);
        }

        protected virtual void LoadAll(string directory) {
            string[] files = Directory.GetFiles(directory);

            foreach (var file in files) {
                Debug.WriteLine($"Trying to load {file}.");
                LoadResource(file);
            }
        }

        public void LoadResource(string path) {
            switch (Path.GetExtension(path)) {
                case "png":
                    if (!_textures.ContainsKey(Path.GetFileNameWithoutExtension(path)))
                        _textures.Add(Path.GetFileNameWithoutExtension(path), Raylib_cs.Raylib.LoadTexture(path));
                    else
                        Debug.WriteLine($"Resource {Path.GetFileNameWithoutExtension(path)} has already been loaded.");
                    break;
                default:
                    throw new Exception("Unrecognized file extension.");
            }
        }
        
        private static readonly List<IUpdateable> _rmQueuedUpdates = new List<IUpdateable>();
        private static readonly List<IUpdateable> _queuedUpdates = new List<IUpdateable>();
        private static readonly List<IUpdateable> _updates = new List<IUpdateable>();

        
        protected virtual void RunOneFrame(){
            foreach (IUpdateable update in _rmQueuedUpdates)
            {
                if (_updates.Contains(update))
                    _updates.Remove(update);
            }

            foreach (IUpdateable update in _queuedUpdates)
            {
                _updates.Add(update);
            }
            _queuedUpdates.Clear();
            _rmQueuedUpdates.Clear();
            
            foreach (IUpdateable update in _updates)
            {
                update.Update();
            }
        }

        private static readonly List<IDrawable> _rmQueuedRenderers = new();
        private static readonly SortedList<int ,IDrawable> _queuedRenderers = new();
        private static readonly SortedList<int ,IDrawable> _renderers = new();

        public void AddRenderer(IDrawable drawable, int layer = 0) {
            _queuedRenderers.Add(layer, drawable);
        }

        public void RemoveRenderer(IDrawable drawable) {
            _rmQueuedRenderers.Add(drawable);
        }
        

        protected void Draw() {
            
            foreach (var update in _queuedRenderers)
            {
                _renderers.Add(update.Key, update.Value);
            }
            
            foreach (var draw in _rmQueuedRenderers)
            {
                if (_renderers.ContainsValue(draw))
                    _renderers.RemoveAt(_renderers.IndexOfValue(draw));
            }

            Raylib_cs.Raylib.BeginDrawing();

            RenderBatch sprite = new();
            
            
            foreach (var draw in _renderers)
            {
                draw.Value.Draw(sprite);
            }
            
            Raylib_cs.Raylib.EndDrawing();
        }
    }

}