namespace SomeGameEngine.LinearMath;

public struct Vec2
{
    public static Vec2 Zero => new();
    public static Vec2 One => new(1, 1);
    public static Vec2 Left => new(-1);
    public static Vec2 Right => new(1);
    public static Vec2 Down => new(y: -1);
    public static Vec2 Up => new(y: 1);

    public float X;
    public float Y;

    public float Magnitude => float.Sqrt(X * X + Y * Y);
    public Vec2 Unit => Magnitude <= 0 ? Zero : this / Magnitude;

    public Vec2(float x = 0, float y = 0)
    {
        X = x;
        Y = y;
    }

    public static float Distance(Vec2 a, Vec2 b)
    {
        return (b - a).Magnitude;
    }

    public float DistanceTo(Vec2 dst)
    {
        return Distance(this, dst);
    }

    public float Dot(Vec2 b)
    {
        return X * b.X + Y * b.Y;
    }

    public static Vec2 operator +(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X + b.X, a.Y + b.Y);
    }

    public static Vec2 operator -(Vec2 a)
    {
        return new Vec2(-a.X, -a.Y);
    }

    public static Vec2 operator -(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X - b.X, a.Y - b.Y);
    }

    public static Vec2 operator *(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X * b.X, a.Y * b.Y);
    }

    public static Vec2 operator *(Vec2 a, float scl)
    {
        return new Vec2(a.X * scl, a.Y * scl);
    }

    public static Vec2 operator /(Vec2 a, Vec2 b)
    {
        return new Vec2(a.X / b.X, a.Y / b.Y);
    }

    public static Vec2 operator /(Vec2 a, float scl)
    {
        return new Vec2(a.X / scl, a.Y / scl);
    }

    public static implicit operator Vec2(iVec2 v)
    {
        return new Vec2(v.X, v.Y);
    }
}

public struct iVec2
{
    public static iVec2 Zero => new();
    public static iVec2 One => new(1, 1);
    public static iVec2 Left => new(-1);
    public static iVec2 Right => new(1);
    public static iVec2 Down => new(y: -1);
    public static iVec2 Up => new(y: 1);

    public int X;
    public int Y;

    public float Magnitude => float.Sqrt(X * X + Y * Y);
    public Vec2 Unit => Magnitude <= 0 ? Zero : this / Magnitude;

    public iVec2(int x = 0, int y = 0)
    {
        X = x;
        Y = y;
    }

    public float DistanceTo(iVec2 dst)
    {
        return (this - dst).Magnitude;
    }

    public static iVec2 operator +(iVec2 a, iVec2 b)
    {
        return new iVec2(a.X + b.X, a.Y + b.Y);
    }

    public static iVec2 operator -(iVec2 a)
    {
        return new iVec2(-a.X, -a.Y);
    }

    public static iVec2 operator -(iVec2 a, iVec2 b)
    {
        return new iVec2(a.X - b.X, a.Y - b.Y);
    }

    public static iVec2 operator *(iVec2 a, iVec2 b)
    {
        return new iVec2(a.X * b.X, a.Y * b.Y);
    }

    public static iVec2 operator *(iVec2 a, int scl)
    {
        return new iVec2(a.X * scl, a.Y * scl);
    }

    public static Vec2 operator *(iVec2 a, float scl)
    {
        return new Vec2(a.X * scl, a.Y * scl);
    }

    public static iVec2 operator /(iVec2 a, iVec2 b)
    {
        return new iVec2(a.X / b.X, a.Y / b.Y);
    }

    public static iVec2 operator /(iVec2 a, int scl)
    {
        return new iVec2(a.X / scl, a.Y / scl);
    }

    public static Vec2 operator /(iVec2 a, float scl)
    {
        return new Vec2(a.X / scl, a.Y / scl);
    }

    public static implicit operator iVec2(Vec2 v)
    {
        return new iVec2((int)v.X, (int)v.Y);
    }
}