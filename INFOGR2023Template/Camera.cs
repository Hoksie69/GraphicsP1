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
        public Vector3 right_direction;
        float angleXZ;
        float angleY;
        public Vector3 cameraPlaneBasisU;
        public Vector3 cameraPlaneBasisV;
        public float FOV = 90;

        public Camera(Vector3 _position, Vector3 _look_at, Vector3 _up_direction) 
        {
            position = _position;
            look_at = _look_at;
            look_at.Normalize();
            up_direction = _up_direction;
            up_direction.Normalize();
            GetPlane();
            cameraPlaneBasisU = screenPlane[1] - screenPlane[0];
            cameraPlaneBasisV = screenPlane[2] - screenPlane[0];
        }

        public void GetPlane()
        {
            look_at.Normalize();
            up_direction.Normalize();
            Vector3 planeCenter = position + (float)(FOV / 90) * look_at;
            float aspectRatio = 1.6f;
            right_direction = Vector3.Cross(look_at, up_direction);
            screenPlane[0] = planeCenter + up_direction - (aspectRatio * right_direction);
            screenPlane[1] = planeCenter + up_direction + (aspectRatio * right_direction);
            screenPlane[2] = planeCenter - up_direction - (aspectRatio * right_direction);
            screenPlane[3] = planeCenter - up_direction + (aspectRatio * right_direction);
            cameraPlaneBasisU = screenPlane[1] - screenPlane[0];
            cameraPlaneBasisV = screenPlane[2] - screenPlane[0];

        }

        public void GetNewAngle(float _angle)
        {
            angleXZ += _angle;
            Console.WriteLine(angleXZ);
            look_at.X = (float)Math.Cos(toRadians(angleXZ));
            look_at.Z = (float)Math.Sin(toRadians(angleXZ));
            look_at.Normalize();
            Console.WriteLine(look_at);
        }
       public void GetNewAngleY(float _angle)
        {
            angleY += _angle;
            Console.WriteLine(angleY);
            look_at.Y = (float)Math.Cos(toRadians(angleY));
            look_at.Normalize();
            Console.WriteLine(look_at);
        }

        float toRadians(float angle)
        {
            return (float)angle * (float)(Math.PI / 180);
        }

        public void AimAt(Vector3 target)
        {
            Vector3 targetVector = new Vector3();
            targetVector = target - position;
            look_at = targetVector.Normalized();
        }
    }
}
