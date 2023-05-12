using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template
{
    internal class Camera
    {

        Vector3 position = new Vector3(0, 0, 0);
        Vector3 look_at = new Vector3(0, 0, 1);
        Vector3 up_direction = new Vector3(0, 1, 0);

        public Camera(Vector3 _position, Vector3 _look_at, Vector3 _up_direction) 
        {
            //position = _position;
            //look_at = _look_at;
            //up_direction = _up_direction;
        }
    }
}
