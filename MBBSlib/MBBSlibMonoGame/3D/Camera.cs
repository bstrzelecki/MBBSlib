using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame._3D
{
    public class Camera
    {
        #region public properties
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                RecalculateViewMatrix();
            }
        }
        public Vector3 Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
                RecalculateViewMatrix();
            }
        }
        public float FOV
        {
            get
            {
                return MathHelper.ToDegrees(fov);
            }
            set
            {
                fov = MathHelper.ToRadians(value);
                RecalculateProjectionMatrix();
            }
        }
        public float AspectRatio
        {
            get
            {
                return aspectRatio;
            }
            set
            {
                aspectRatio = value;
                RecalculateProjectionMatrix();
            }
        }
        public float NearPlane
        {
            get
            {
                return nearPlane;
            }
            set
            {
                nearPlane = value;
                RecalculateProjectionMatrix();
            }
        }
        public float FarPlane
        {
            get
            {
                return farPlane;
            }
            set
            {
                farPlane = value;
                RecalculateProjectionMatrix();
            }
        }
        #endregion
        public BasicEffect GetBasicEffect(GraphicsDevice graphicsDevice)
        {
            BasicEffect basicEffect = new BasicEffect(graphicsDevice)
            {
                Projection = projectionMatrix,
                World = worldMatrix,
                View = viewMatrix
            };
            return basicEffect;
        }
        Vector3 target;
        Vector3 position;

        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        private float fov = MathHelper.ToRadians(45f);
        private float aspectRatio = 16 / 9;
        private float nearPlane = 1f;
        private float farPlane = 1000f;

        public Camera()
        {
            target = Vector3.Zero;
            position = new Vector3(0, 0, 100f);

            RecalculateProjectionMatrix();
            RecalculateViewMatrix();

            worldMatrix = Matrix.CreateWorld(target, Vector3.Forward, Vector3.Up);
        }
        private void RecalculateProjectionMatrix()
        {
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearPlane, farPlane);

        }
        private void RecalculateViewMatrix()
        {
            viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up);
        }
    }
}
