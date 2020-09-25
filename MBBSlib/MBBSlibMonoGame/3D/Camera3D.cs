using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MBBSlib.MonoGame._3D
{
    /// <summary>
    /// This is a camera i basically remade to make it work. 
    /// Using quite a bit of stuff from my camera class its nearly the same as mine but its a bit simpler. 
    /// I have bunches of cameras at this point and i need to combine them into a fully hard core non basic camera.
    /// That said simple makes for a better example and a better basis to combine them later.
    /// </summary>
    public class Camera3D
    {
        public readonly GameWindow GameWindow = null;

        public ICameraController Controller = CameraController.None;


        public float MovementUnitsPerSecond { get; set; } = 30f;
        public float RotationRadiansPerSecond { get; set; } = 60f;

        public float fieldOfViewDegrees = 80f;
        public float nearClipPlane = .05f;
        public float farClipPlane = 2000f;
        /// <summary>
        /// Constructs the camera.
        /// </summary>
        public Camera3D(GraphicsDevice gfxDevice, GameWindow window)
        {
            GameWindow = window;
            ReCreateWorldAndView();
            ReCreateThePerspectiveProjectionMatrix(gfxDevice, fieldOfViewDegrees);
        }

        /// <summary>
        /// This serves as the cameras up. For fixed cameras this might not change at all ever. For free cameras it changes constantly.
        /// A fixed camera keeps a fixed horizon but can gimble lock under normal rotation when looking straight up or down.
        /// A free camera has no fixed horizon but can't gimble lock under normal rotation as the up changes as the camera moves.
        /// Most hybrid cameras are a blend of the two but all are based on one or both of the above.
        /// </summary>
        private Vector3 _up = Vector3.Up;
        /// <summary>
        /// This serves as the cameras world orientation like almost all 3d game objects they have a world matrix. 
        /// It holds all orientational values and is used to move the camera properly thru the world space.
        /// </summary>
        public Matrix CameraWorld = Matrix.Identity;
        /// <summary>
        /// The view matrice is created from the cameras world matrice but it has special properties.
        /// Using create look at to create this matrix you move from the world space into the view space.
        /// If you are working on world objects you should not take individual elements from this to directly operate on world matrix components.
        /// As well the multiplication of a view matrix by a world matrix moves the resulting matrix into view space itself.
        /// </summary>
        public Matrix viewMatrix = Matrix.Identity;
        /// <summary>
        /// The projection matrix. This matrice creates a vanishing point and skews all objects drawn to create the illusion of depth and a perspective parallax view at distance.
        /// </summary>
        public Matrix projectionMatrix = Matrix.Identity;

        /// <summary>
        /// Gets or sets the the camera's position in the world.
        /// </summary>
        public Vector3 Position
        {
            set
            {
                CameraWorld.Translation = value;
                // since we know here that a change has occured to the cameras world orientations we can update the view matrix.
                ReCreateWorldAndView();
            }
            get => CameraWorld.Translation;
        }
        /// <summary>
        /// Gets or Sets the direction the camera is looking at in the world.
        /// The forward is the same as the look at direction it i a directional vector not a position.
        /// </summary>
        public Vector3 Forward
        {
            set
            {
                CameraWorld = Matrix.CreateWorld(CameraWorld.Translation, value, _up);
                // since we know here that a change has occured to the cameras world orientations we can update the view matrix.
                ReCreateWorldAndView();
            }
            get => CameraWorld.Forward;
        }
        /// <summary>
        /// Get the cameras up vector. You shouldn't need to set the up you shouldn't at all if you are using the free camera type.
        /// </summary>
        public Vector3 Up
        {
            set
            {
                _up = value;
                CameraWorld = Matrix.CreateWorld(CameraWorld.Translation, CameraWorld.Forward, value);
                // since we know here that a change has occured to the cameras world orientations we can update the view matrix.
                ReCreateWorldAndView();
            }
            get => _up;
        }

        /// <summary>
        /// Gets or Sets the direction the camera is looking at in the world as a directional vector.
        /// </summary>
        public Vector3 LookAtDirection
        {
            set
            {
                CameraWorld = Matrix.CreateWorld(CameraWorld.Translation, value, _up);
                // since we know here that a change has occured to the cameras world orientations we can update the view matrix.
                ReCreateWorldAndView();
            }
            get => CameraWorld.Forward;
        }
        /// <summary>
        /// Sets a positional target in the world to look at.
        /// </summary>
        public Vector3 TargetPositionToLookAt
        {
            set
            {
                CameraWorld = Matrix.CreateWorld(CameraWorld.Translation, Vector3.Normalize(value - CameraWorld.Translation), _up);
                // since we know here that a change has occured to the cameras world orientations we can update the view matrix.
                ReCreateWorldAndView();
            }
        }
        /// <summary>
        /// Turns the camera to face the target this method just takes in the targets matrix for convienience.
        /// </summary>
        public Matrix LookAtTheTargetMatrix
        {
            set
            {
                CameraWorld = Matrix.CreateWorld(CameraWorld.Translation, Vector3.Normalize(value.Translation - CameraWorld.Translation), _up);
                // since we know here that a change has occured to the cameras world orientations we can update the view matrix.
                ReCreateWorldAndView();
            }
        }

        /// <summary>
        /// Directly set or get the world matrix this also updates the view matrix
        /// </summary>
        public Matrix World
        {
            get => CameraWorld;
            set
            {
                CameraWorld = value;
                viewMatrix = Matrix.CreateLookAt(CameraWorld.Translation, CameraWorld.Forward + CameraWorld.Translation, CameraWorld.Up);
            }
        }

        /// <summary>
        /// Gets the view matrix we never really set the view matrix ourselves outside this method just get it.
        /// The view matrix is remade internally when we know the world matrix forward or position is altered.
        /// </summary>
        public Matrix View => viewMatrix;

        /// <summary>
        /// Gets the projection matrix.
        /// </summary>
        public Matrix Projection => projectionMatrix;

        /// <summary>
        /// When the cameras position or orientation changes, we call this to ensure that the cameras world matrix is orthanormal.
        /// We also set the up depending on our choices of is fix or free camera and we then update the view matrix.
        /// </summary>
        private void ReCreateWorldAndView()
        {
            _up = Controller.UpVector;
            CameraWorld = Matrix.CreateWorld(CameraWorld.Translation, CameraWorld.Forward, _up);
            viewMatrix = Matrix.CreateLookAt(CameraWorld.Translation, CameraWorld.Forward + CameraWorld.Translation, CameraWorld.Up);
        }

        /// <summary>
        /// Changes the perspective matrix to a new near far and field of view.
        /// </summary>
        public void ReCreateThePerspectiveProjectionMatrix(GraphicsDevice gd, float fovInDegrees) => projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fovInDegrees * ((3.14159265358f) / 180f), GameMain.Instance.Resolution.Width / (float)GameMain.Instance.Resolution.Height, .05f, 1000f);
        /// <summary>
        /// Changes the perspective matrix to a new near far and field of view.
        /// The projection matrix is typically only set up once at the start of the app.
        /// </summary>
        public void ReCreateThePerspectiveProjectionMatrix(float fieldOfViewInDegrees, float nearPlane, float farPlane)
        {
            // create the projection matrix.
            this.fieldOfViewDegrees = MathHelper.ToRadians(fieldOfViewInDegrees);
            nearClipPlane = nearPlane;
            farClipPlane = farPlane;
            float aspectRatio = GameMain.Instance.Resolution.Width / (float)GameMain.Instance.Resolution.Height;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(this.fieldOfViewDegrees, aspectRatio, nearClipPlane, farClipPlane);
        }

        /// <summary>
        /// update the camera.
        /// </summary>
        public void Update(GameTime gameTime) => Controller.Update(this, gameTime);


        /// <summary>
        /// This function can be used to check if gimble is about to occur in a fixed camera.
        /// If this value returns 1.0f you are in a state of gimble lock, However even as it gets near to 1.0f you are in danger of problems.
        /// In this case you should interpolate towards a free camera. Or begin to handle it.
        /// Earlier then .9 in some manner you deem to appear fitting otherwise you will get a hard spin effect. Though you may want that.
        /// </summary>
        public float GetGimbleLockDangerValue()
        {
            var c0 = Vector3.Dot(World.Forward, World.Up);
            if (c0 < 0f) c0 = -c0;
            return c0;
        }

        #region Local Translations and Rotations.

        public void MoveForward(GameTime gameTime) => Position += (CameraWorld.Forward * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveBackward(GameTime gameTime) => Position += (CameraWorld.Backward * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveLeft(GameTime gameTime) => Position += (CameraWorld.Left * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveRight(GameTime gameTime) => Position += (CameraWorld.Right * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveUp(GameTime gameTime) => Position += (CameraWorld.Up * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveDown(GameTime gameTime) => Position += (CameraWorld.Down * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;

        public void RotateUp(GameTime gameTime)
        {
            var radians = RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var matrix = Matrix.CreateFromAxisAngle(CameraWorld.Right, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }
        public void RotateDown(GameTime gameTime)
        {
            var radians = -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var matrix = Matrix.CreateFromAxisAngle(CameraWorld.Right, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }
        public void RotateLeft(GameTime gameTime)
        {
            var radians = RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var matrix = Matrix.CreateFromAxisAngle(CameraWorld.Up, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }
        public void RotateRight(GameTime gameTime)
        {
            var radians = -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var matrix = Matrix.CreateFromAxisAngle(CameraWorld.Up, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }
        public void RotateRollClockwise(GameTime gameTime)
        {
            var radians = RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var pos = CameraWorld.Translation;
            CameraWorld *= Matrix.CreateFromAxisAngle(CameraWorld.Forward, MathHelper.ToRadians(radians));
            CameraWorld.Translation = pos;
            ReCreateWorldAndView();
        }
        public void RotateRollCounterClockwise(GameTime gameTime)
        {
            var radians = -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var pos = CameraWorld.Translation;
            CameraWorld *= Matrix.CreateFromAxisAngle(CameraWorld.Forward, MathHelper.ToRadians(radians));
            CameraWorld.Translation = pos;
            ReCreateWorldAndView();
        }

        // just for example this is the same as the above rotate left or right.
        public void RotateLeftOrRight(GameTime gameTime, float amount)
        {
            var radians = amount * -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var matrix = Matrix.CreateFromAxisAngle(CameraWorld.Up, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }
        public void RotateUpOrDown(GameTime gameTime, float amount)
        {
            var radians = amount * -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var matrix = Matrix.CreateFromAxisAngle(CameraWorld.Right, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }

        #endregion

        #region Non Local System Translations and Rotations.

        public void MoveForwardInNonLocalSystemCoordinates(GameTime gameTime) => Position += (Vector3.Forward * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveBackwardsInNonLocalSystemCoordinates(GameTime gameTime) => Position += (Vector3.Backward * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveUpInNonLocalSystemCoordinates(GameTime gameTime) => Position += (Vector3.Up * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveDownInNonLocalSystemCoordinates(GameTime gameTime) => Position += (Vector3.Down * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveLeftInNonLocalSystemCoordinates(GameTime gameTime) => Position += (Vector3.Left * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        public void MoveRightInNonLocalSystemCoordinates(GameTime gameTime) => Position += (Vector3.Right * MovementUnitsPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;

        /// <summary>
        /// These aren't typically useful and you would just use create world for a camera snap to a new view. I leave them for completeness.
        /// </summary>
        public void NonLocalRotateLeftOrRight(GameTime gameTime, float amount)
        {
            var radians = amount * -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var matrix = Matrix.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }
        /// <summary>
        /// These aren't typically useful and you would just use create world for a camera snap to a new view.  I leave them for completeness.
        /// </summary>
        public void NonLocalRotateUpOrDown(GameTime gameTime, float amount)
        {
            var radians = amount * -RotationRadiansPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var matrix = Matrix.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(radians));
            LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
            ReCreateWorldAndView();
        }

        #endregion
    }
}
