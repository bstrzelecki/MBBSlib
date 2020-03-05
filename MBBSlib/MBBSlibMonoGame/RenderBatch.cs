using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MBBSlib.MonoGame
{
    public class RenderBatch : IDisposable
    {
        SpriteBatch spriteBatch;
        GraphicsDevice graphicsDevice;
        public RenderBatch(SpriteBatch sb, GraphicsDevice gd)
        {
            spriteBatch = sb;
            graphicsDevice = gd;
        }
        public void Draw(Texture2D texture, Rectangle size)
        {
            spriteBatch.Draw(texture, size, Color.White);
        }
        public void Draw(Texture2D texture, Rectangle size, Color color)
        {
            spriteBatch.Draw(texture, size, color);
        }
        public void Draw(Texture2D texture, Vector2 position)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
        public void Draw(Texture2D texture, Vector2 position, Color color)
        {
            spriteBatch.Draw(texture, position, color);
        }
        public void Draw(string textureName, Vector2 position)
        {
            spriteBatch.Draw(new Sprite(textureName), position, Color.White);
        }
        public void Draw(string textureName, Vector2 position, Color color)
        {
            spriteBatch.Draw(new Sprite(textureName), position, color);
        }
        public void DrawString(SpriteFont font, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, text, position, color);
        }
        public void DrawString(string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(new Font("font"), text, position, color);
        }
        public void DrawMesh(Model model, Vector3 position)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = GameMain.instance.camera.viewMatrix;
                    effect.World = Matrix.CreateTranslation(position);
                    effect.Projection = GameMain.instance.camera.projectionMatrix;
                }
                mesh.Draw();
            }
        }
        public void DrawPrimitives(VertexPositionTexture[] mesh, Texture2D texture, bool disableCulling = false)
        {
            BasicEffect effect = new BasicEffect(graphicsDevice)
            {
                View = GameMain.instance.camera.viewMatrix,
                Projection = GameMain.instance.camera.projectionMatrix,
                TextureEnabled = true,
                Texture = texture
            };
            if (!disableCulling)
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                graphicsDevice.RasterizerState = rasterizerState;
            }
            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData(mesh);
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, mesh.Length);
            }
        }
        public void DrawPrimitives(VertexPositionTexture[] mesh, Vector3 position, Texture2D texture, bool disableCulling = false)
        {
            BasicEffect effect = new BasicEffect(graphicsDevice)
            {
                View = GameMain.instance.camera.viewMatrix,
                Projection = GameMain.instance.camera.projectionMatrix,
                TextureEnabled = true,
                Texture = texture
            };
            if (!disableCulling)
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                graphicsDevice.RasterizerState = rasterizerState;
            }
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].Position += position;
            }
            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionTexture), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData(mesh);
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, mesh.Length);
            }
        }
        public void DrawPrimitives(VertexPositionColor[] mesh, bool disableCulling = false)
        {
            BasicEffect effect = new BasicEffect(graphicsDevice)
            {
                View = GameMain.instance.camera.viewMatrix,
                Projection = GameMain.instance.camera.projectionMatrix,
                VertexColorEnabled = true
            };
            if (!disableCulling)
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                graphicsDevice.RasterizerState = rasterizerState;
            }
            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData(mesh);
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, mesh.Length);
            }
        }
        public void DrawPrimitives(VertexPositionColor[] mesh, Vector3 position, bool disableCulling = false)
        {
            BasicEffect effect = new BasicEffect(graphicsDevice)
            {
                View = GameMain.instance.camera.viewMatrix,
                Projection = GameMain.instance.camera.projectionMatrix,
                VertexColorEnabled = true
            };
            if (!disableCulling)
            {
                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                graphicsDevice.RasterizerState = rasterizerState;
            }
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].Position += position;
            }
            var vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData(mesh);
            graphicsDevice.SetVertexBuffer(vertexBuffer);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, mesh.Length);
            }
        }

        public void Dispose()
        {
            spriteBatch.Dispose();
            graphicsDevice.Dispose();
        }
        public static explicit operator SpriteBatch(RenderBatch rb)
        {
            return rb.spriteBatch;
        }
        public static explicit operator GraphicsDevice(RenderBatch rb)
        {
            return rb.graphicsDevice;
        }
    }
}
