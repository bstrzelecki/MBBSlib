using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MBBSlib.MonoGame
{
    public class RenderBatch : IDisposable
    {
        readonly SpriteBatch _spriteBatch;
        readonly GraphicsDevice _graphicsDevice;
        public RenderBatch(SpriteBatch sb, GraphicsDevice gd)
        {
            _spriteBatch = sb;
            _graphicsDevice = gd;
        }
        public void DrawRelative(Texture2D texture, Math.Vector2 position)
        {
            var p = new Vector2(position.x, position.y);
            _spriteBatch.Draw(texture, p - GameMain.Instance.camera2D.Position, Color.White);
        }
        public void DrawRelative(Texture2D texture, Math.Vector2 position, Color color)
        {
            var p = new Vector2(position.x, position.y);
            _spriteBatch.Draw(texture, p - GameMain.Instance.camera2D.Position, color);
        }
        public void Draw(Texture2D texture, Rectangle size)
        {
            _spriteBatch.Draw(texture, size, Color.White);
        }
        public void Draw(Texture2D texture, Rectangle size, Color color)
        {
            _spriteBatch.Draw(texture, size, color);
        }
        public void Draw(Texture2D texture, Vector2 position)
        {
            _spriteBatch.Draw(texture, position, Color.White);
        }
        public void Draw(Texture2D texture, Math.Vector2 position, Color color)
        {
            _spriteBatch.Draw(texture, new Vector2(position.x,position.y), color);
        }
        public void Draw(Texture2D texture, Math.Vector2 position)
        {
            var p = new Vector2(position.x, position.y);
            _spriteBatch.Draw(texture, p, Color.White);
        }
        public void Draw(string texture, Math.Vector2 position)
        {
            var p = new Vector2(position.x, position.y);
            _spriteBatch.Draw(new Sprite(texture), p, Color.White);
        }
        public void Draw(Texture2D texture, Math.Vector2 position, float rotation, Math.Vector2 origin)
        {
            _spriteBatch.Draw(new Sprite(texture), new Rectangle((int)position.x, (int)position.y, texture.Width, texture.Height), null, Color.White, rotation, new Vector2(origin.x, origin.y), SpriteEffects.None, 0);
        }
        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            _spriteBatch.Draw(texture, position, color);
        }
        public void Draw(string textureName, Vector2 position)
        {
            _spriteBatch.Draw(new Sprite(textureName), position, Color.White);
        }
        public void Draw(string textureName, Vector2 position, Color color)
        {
            _spriteBatch.Draw(new Sprite(textureName), position, color);
        }
        public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            _spriteBatch.DrawString(font, text, position, color);
        }
        public void DrawString(string text, Vector2 position, Color color)
        {
            _spriteBatch.DrawString(new Font("font"), text, position, color);
        }
        public void DrawMesh(Model model, Vector3 position)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0, 0, 0);
                    effect.View = GameMain.Instance.camera3D.viewMatrix;
                    effect.World = Matrix.CreateTranslation(position);
                    effect.Projection = GameMain.Instance.camera3D.projectionMatrix;
                }
                mesh.Draw();
            }
        }
        public void DrawMesh(Model model, Vector3 position, Sprite texture)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.View = GameMain.Instance.camera3D.viewMatrix;
                    effect.Projection = GameMain.Instance.camera3D.projectionMatrix;

                    effect.AmbientLightColor = new Vector3(0, 0, 0);
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateTranslation(position);
                    effect.TextureEnabled = true;
                    effect.Texture = texture;
                }
                mesh.Draw();
            }
        }
        public void DrawPrimitives(VertexPositionTexture[] mesh, Texture2D texture, bool disableCulling = false)
        {
            var effect = GetBasicEffect(_graphicsDevice);
            effect.TextureEnabled = true;
            effect.Texture = texture;
            
            if (!disableCulling)
            {
                _graphicsDevice.RasterizerState = GetPrimitiveRasterizerState(disableCulling);
            }
            var vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionTexture), mesh.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(mesh);
            _graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, mesh.Length);
            }
        }
        public void DrawPrimitives(VertexPositionTexture[] mesh, Vector3 position, Texture2D texture, bool disableCulling = false)
        {
            for(int i = 0; i < mesh.Length; i++)
            {
                mesh[i].Position += position;
            }
            DrawPrimitives(mesh, texture, disableCulling);
        }
        public void DrawPrimitives(VertexPositionColor[] mesh, bool disableCulling = false)
        {
            var effect = GetBasicEffect(_graphicsDevice);
            effect.VertexColorEnabled = true;
            
            var dss = new DepthStencilState();
            dss.DepthBufferEnable = true;
            _graphicsDevice.DepthStencilState = dss;
            
            if (!disableCulling)
            {
                _graphicsDevice.RasterizerState = GetPrimitiveRasterizerState(disableCulling);
            }
            var vertexBuffer = new VertexBuffer(_graphicsDevice, typeof(VertexPositionColor), mesh.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(mesh);
            _graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, mesh.Length);
            }
        }
        public void DrawPrimitives(VertexPositionColor[] mesh, Vector3 position, bool disableCulling = false)
        {
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].Position += position;
            }
            DrawPrimitives(mesh, disableCulling);
        }
        private BasicEffect GetBasicEffect(GraphicsDevice gd) => new BasicEffect(gd)
        {
            View = GameMain.Instance.camera3D.viewMatrix,
            Projection = GameMain.Instance.camera3D.projectionMatrix
        };
        private RasterizerState GetPrimitiveRasterizerState(bool disableCulling) => new RasterizerState
        {
            CullMode = disableCulling?CullMode.None:CullMode.CullClockwiseFace,
            FillMode = FillMode.Solid,
            DepthClipEnable = true
        };
        public void Dispose()
        {
            _spriteBatch.Dispose();
            _graphicsDevice.Dispose();
        }

        public static explicit operator SpriteBatch(RenderBatch rb)
        {
            return rb._spriteBatch;
        }
        public static explicit operator GraphicsDevice(RenderBatch rb)
        {
            return rb._graphicsDevice;
        }
    }
}
