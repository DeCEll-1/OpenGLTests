using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RGL.API;

namespace RGL.Classes.impl.EFSs
{
    public class HandleMouse : EveryFrameScript
    {
        private bool _firstMove = true;
        private Vector2 _lastPos;
        private CursorState cursorState;
        public override void Init() { }

        public override void Advance()
        {
            var mouse = MouseState;
            if (KeyboardState.IsKeyPressed(Keys.R))
            {
                if (cursorState == CursorState.Grabbed)
                {
                    cursorState = CursorState.Normal;
                    Window.CursorState = cursorState;
                }
                else
                {
                    cursorState = CursorState.Grabbed;
                    _lastPos = new Vector2(mouse.X, mouse.Y);
                    Window.CursorState = cursorState;
                }
            }
            if (Window.CursorState == CursorState.Normal)
                return;



            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                Camera.Yaw += deltaX * APISettings.MouseSensitivity;
                Camera.Pitch -= deltaY * APISettings.MouseSensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }
    }
}
