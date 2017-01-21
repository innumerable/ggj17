using System.Linq;
using UnityEngine;

public static class ConnectionInterceptorModelCandidateSineLineGeneratorFactoryBean
{
    private static Equation[] components;

    public static float GetHeight(float x)
    {
        return components.Sum(eq => eq.GetValue(x));
    }

    public static void Initialise(Equation[] equations)
    {
        if (components == null)
            components = equations;
    }
}

/// <summary>
/// y = Asin(Bx + C) + D
/// </summary>
public struct Equation
{
    public float A;
    public float Ax;
    public float B;
    public float Bx;
    public float C;
    public float D;

    public Equation(float _A, float _Ax, float _B, float _Bx, float _C, float _D)
    {
        this.A = _A;
        this.Ax = _Ax;
        this.B = _B;
        this.Bx = _Bx;
        this.C = _C;
        this.D = _D;
    }

    public float GetValue(float x)
    {
        return (A + (Ax * x)) * Mathf.Sin((B + (Bx * x)) * x + C) + (D * x);
    }
}