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
    public float B;
    public float C;
    public float D;

    public Equation(float _A, float _B, float _C, float _D)
    {
        this.A = _A;
        this.B = _B;
        this.C = _C;
        this.D = _D;
    }

    public float GetValue(float x)
    {
        return A * Mathf.Sin(B*x + C) + D;
    }
}