using OpenTK.Mathematics;

namespace RGL.API.Rendering
{
    public class Camera
    {
        public Vector3 Position = Vector3.Zero;
        public int screenWidth { get => APISettings.SceneResolution.X; }
        public int screenHeight { get => APISettings.SceneResolution.Y; }
        public float depthNear { get => APISettings.CameraDepthNear; }
        public float depthFar { get => APISettings.CameraDepthFar; }
        private float _pitch;
        private float _yaw = -MathHelper.PiOver2;
        private Vector3 _front = -Vector3.UnitZ;
        private Vector3 _up = Vector3.UnitY;
        private Vector3 _right = Vector3.UnitX;

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                // We clamp the pitch value between -89 and 89 to prevent the camera from going upside down, and a bunch
                // of weird "bugs" when you are using euler angles for rotation.
                // If you want to read more about this you can try researching a topic called gimbal lock
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }
        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public Camera()
        {
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }

        public Matrix4 GetProjectionMatrix()
        { // TODO: refresh this once every frame instead of calculating it for each object
            return Matrix4.CreatePerspectiveFieldOfView(
                APISettings.FOVRadian,
                (float)screenWidth / (float)screenHeight,
                depthNear,
                depthFar
            );
        }

        // This function is going to update the direction vertices using some of the math learned in the web tutorials.
        private void UpdateVectors()
        {
            // First, the front matrix is calculated using some basic trigonometry.
            _front.X = MathF.Cos(_pitch) * MathF.Cos(_yaw);
            _front.Y = MathF.Sin(_pitch);
            _front.Z = MathF.Cos(_pitch) * MathF.Sin(_yaw);

            // We need to make sure the vectors are all normalized, as otherwise we would get some funky results.
            _front = Vector3.Normalize(_front);

            // Calculate both the right and the up vector using cross product.
            // Note that we are calculating the right from the global up; this behaviour might
            // not be what you need for all cameras so keep this in mind if you do not want a FPS camera.
            _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            _up = Vector3.Normalize(Vector3.Cross(_right, _front));
        }

        public override string ToString()
        {
            return
                $"Resolution: {APISettings.SceneResolution.ToString()}\n" +
                $"Aspect Ratio: {screenWidth / (float)screenHeight}\n" +
                $"Near: {depthNear}\n" +
                $"Far: {depthFar}\n" +
                $"FoV: {APISettings.Fov.ToString()}\n" +
                $"Pitch: {Pitch}\n" +
                $"Yaw: {Yaw}\n" +
                $"Position: {Position.ToString()}\n" +
                $"Projection Matrix:\n{GetProjectionMatrix().ToString()}\n" + 
                $"View Matrix:\n{GetViewMatrix().ToString()}"
                ;

        }
    }
}
