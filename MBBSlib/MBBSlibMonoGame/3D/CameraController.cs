using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MBBSlib.MonoGame._3D
{
    public class CameraController
    {
        public static ICameraController None = new EmptyCameraController();
        public static ICameraController Fps = new FpsCameraController();
        public static ICameraController Free = new FreeCameraController();

        internal class EmptyCameraController : ICameraController
        {
            public Vector3 UpVector => Vector3.Up;
            public void Update(Camera3D camera, GameTime gameTime) { }
        }

        internal class FreeCameraController : ICameraController
        {
            private MouseState _mState;
            public Vector3 UpVector { get; private set; } = Vector3.Up;
            public void Update(Camera3D camera, GameTime gameTime)
            {
                UpVector = camera.CameraWorld.Up;
                MouseState state = Mouse.GetState(camera.GameWindow);
                KeyboardState kstate = Keyboard.GetState();
                if(kstate.IsKeyDown(Keys.E))
                {
                    camera.MoveForward(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.Q) == true)
                {
                    camera.MoveBackward(gameTime);
                }
                if(kstate.IsKeyDown(Keys.W))
                {
                    camera.RotateUp(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.S) == true)
                {
                    camera.RotateDown(gameTime);
                }
                if(kstate.IsKeyDown(Keys.A) == true)
                {
                    camera.RotateLeft(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.D) == true)
                {
                    camera.RotateRight(gameTime);
                }

                if(kstate.IsKeyDown(Keys.Left) == true)
                {
                    camera.MoveLeft(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.Right) == true)
                {
                    camera.MoveRight(gameTime);
                }
                // rotate 
                if(kstate.IsKeyDown(Keys.Up) == true)
                {
                    camera.MoveUp(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.Down) == true)
                {
                    camera.MoveDown(gameTime);
                }

                // roll counter clockwise
                if(kstate.IsKeyDown(Keys.Z) == true)
                {
                    camera.RotateRollCounterClockwise(gameTime);
                }
                // roll clockwise
                else if(kstate.IsKeyDown(Keys.C) == true)
                {
                    camera.RotateRollClockwise(gameTime);
                }

                if(state.RightButton == ButtonState.Pressed)
                {
                    Vector2 diff = state.Position.ToVector2() - _mState.Position.ToVector2();
                    if(diff.X != 0f)
                        camera.RotateLeftOrRight(gameTime, diff.X);
                    if(diff.Y != 0f)
                        camera.RotateUpOrDown(gameTime, diff.Y);
                }
                _mState = state;
            }
        }

        internal class FpsCameraController : ICameraController
        {
            private MouseState _mState;
            public Vector3 UpVector => Vector3.Up;
            public void Update(Camera3D camera, GameTime gameTime)
            {

                MouseState state = Mouse.GetState(camera.GameWindow);
                KeyboardState kstate = Keyboard.GetState();
                if(kstate.IsKeyDown(Keys.W))
                {
                    camera.MoveForward(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.S) == true)
                {
                    camera.MoveBackward(gameTime);
                }
                // strafe. 
                if(kstate.IsKeyDown(Keys.A) == true)
                {
                    camera.MoveLeft(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.D) == true)
                {
                    camera.MoveRight(gameTime);
                }

                // rotate 
                if(kstate.IsKeyDown(Keys.Left) == true)
                {
                    camera.RotateLeft(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.Right) == true)
                {
                    camera.RotateRight(gameTime);
                }
                // rotate 
                if(kstate.IsKeyDown(Keys.Up) == true)
                {
                    camera.RotateUp(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.Down) == true)
                {
                    camera.RotateDown(gameTime);
                }

                if(kstate.IsKeyDown(Keys.Q) == true)
                {
                    camera.MoveUpInNonLocalSystemCoordinates(gameTime);
                }
                else if(kstate.IsKeyDown(Keys.E) == true)
                {
                    camera.MoveDownInNonLocalSystemCoordinates(gameTime);
                }
                Vector2 diff = state.Position.ToVector2() - _mState.Position.ToVector2();
                if(diff.X != 0f)
                    camera.RotateLeftOrRight(gameTime, diff.X);
                if(diff.Y != 0f)
                    camera.RotateUpOrDown(gameTime, diff.Y);
                _mState = state;
            }
        }
    }
    public interface ICameraController
    {
        void Update(Camera3D camera, GameTime gameTime);
        Vector3 UpVector { get; }
    }
}
