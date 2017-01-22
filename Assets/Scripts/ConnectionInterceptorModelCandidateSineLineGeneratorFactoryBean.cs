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
/// y = Asin(Bx + C) + Dx
/// </summary>
public struct Equation
{
    public float A;
    public float Ax;
    public float B;
    public float Bx;
    public float C;
    public float Dx;

    /// <summary>
    /// Bx does nothing LOL
    /// </summary>
    /// <param name="_A"></param>
    /// <param name="_Ax"></param>
    /// <param name="_B"></param>
    /// <param name="_Bx"></param>
    /// <param name="_C"></param>
    /// <param name="_Dx"></param>
    public Equation(float _A, float _Ax, float _B, float _Bx = 0, float _C = 0, float _Dx = 0)
    {
        this.A = _A;
        this.Ax = _Ax;
        this.B = _B;
        this.Bx = _Bx;
        this.C = _C;
        this.Dx = _Dx;
    }

    public float GetValue(float x)
    {
        return (A + (Ax * x)) * Mathf.Sin((B + Bx * x)* x + C) + (Dx * x);
    }
}