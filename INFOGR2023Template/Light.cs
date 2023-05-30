using System;
using System.Collections.Specialized;
using OpenTK.Mathematics;

public class Light
{
    Vector3 location;
    Vector3 intensity;

    public Light(Vector3 _location, Vector3 _intensity)
    {
        location = _location;
        intensity = _intensity;
    }
}