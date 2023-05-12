using System;
using System.Collections.Specialized;
using OpenTK.Mathematics;

public class Light
{
    int R;
    int G;
    int B;

    Vector3 location;
    float[] intensity = new float[] { R, G, B };

    public Light(Vector3 _location, float[] intensity)
    {
    }
}