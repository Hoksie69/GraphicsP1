using System;
using System.Collections.Specialized;
using OpenTK.Mathematics;

public class Light
{
    public Vector3 location;
    public Vector3 intensity;

    public Light(Vector3 _location, Vector3 _intensity)
    {
        location = _location;
        intensity = _intensity;
    }
}