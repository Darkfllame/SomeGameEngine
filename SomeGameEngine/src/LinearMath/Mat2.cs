namespace SomeGameEngine.LinearMath;

public struct Mat2
{
    public Vec2 I;
    public Vec2 J;

    public Mat2 Identity => new();

    public float Determinant => I.X * J.Y - I.Y * J.X;

    public Mat2 Inverse => new Mat2(J.Y, -I.Y, -J.X, I.X) / Determinant;

    public Mat2(Vec2 i, Vec2 j)
    {
        I = i;
        J = j;
    }

    public Mat2(float ix = 1, float iy = 0, float jx = 0, float jy = 0)
    {
        I.X = ix;
        I.Y = iy;
        J.X = jx;
        J.Y = jy;
    }

    public static Mat2 operator +(Mat2 m, Mat2 m2)
    {
        return new Mat2(m.I + m2.I, m.J + m2.J);
    }

    public static Mat2 operator -(Mat2 m, Mat2 m2)
    {
        return new Mat2(m.I - m2.I, m.J - m2.J);
    }

    public static Mat2 operator *(Mat2 m, Mat2 m2)
    {
        return new Mat2(m * m2.I, m * m2.J);
    }

    public static Vec2 operator *(Mat2 m, Vec2 v)
    {
        return v * m.I + v * m.J;
    }

    public static Mat2 operator *(Mat2 m, float scl)
    {
        return new Mat2(m.I * scl, m.J * scl);
    }

    public static Mat2 operator /(Mat2 m, float scl)
    {
        return new Mat2(m.I / scl, m.J / scl);
    }
}