using OpenTK.Mathematics;
using System.Windows;

namespace Template
{
    internal class Camera
    {

        public Vector3 position;
        public Vector3 look_at;
        public Vector3 up_direction;
        public Vector3[] screenPlane = new Vector3[4];

        public Camera(Vector3 _position, Vector3 _look_at, Vector3 _up_direction) 
        {
            position = _position;
            look_at = _look_at;
            look_at.Normalize();
            up_direction = _up_direction;
            up_direction.Normalize();
            GetPlane();
        }

        public void GetPlane()
        {
            Vector3 planeCenter = position + look_at;
            float aspectRatio = 1.6f;
            Vector3 rightDirection = Vector3.Cross(look_at, up_direction);
            screenPlane[0] = planeCenter + up_direction - (aspectRatio * rightDirection);
            screenPlane[1] = planeCenter + up_direction + (aspectRatio * rightDirection);
            screenPlane[2] = planeCenter - up_direction - (aspectRatio * rightDirection);
            screenPlane[3] = planeCenter - up_direction + (aspectRatio * rightDirection);
            
        }
    }
}
