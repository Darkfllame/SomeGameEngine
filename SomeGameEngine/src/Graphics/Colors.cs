using SomeGameEngine.Utils;
// ReSharper disable MemberCanBePrivate.Global

namespace SomeGameEngine.Graphics;

public struct Color
{
    public byte R;
    public byte G;
    public byte B;
    public byte A;

    public static Color Transparent => new(a: 0);
    public static Color Black => new();
    public static Color White => new(255, 255, 255);
    public static Color Red => new(255);
    public static Color Green => new(g: 255);
    public static Color Blue => new(b: 255);
    public static Color Yellow => new(255, 255);
    public static Color Purple => new(255, b: 255);
    public static Color Cyan => new(g: 255, b: 255);

    public Color(byte r = 0, byte g = 0, byte b = 0, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public Color(uint hex)
    {
        R = (byte)((hex & 0xFF000000) >> 24);
        G = (byte)((hex & 0x00FF0000) >> 16);
        B = (byte)((hex & 0x0000FF00) >> 8);
        A = (byte)(hex & 0x000000FF);
    }

    public void Set(Color c)
    {
        R = c.R;
        G = c.G;
        B = c.B;
        A = c.A;
    }

    public static implicit operator FColor(Color c)
    {
        return new FColor(c.R / 255f, c.G / 255f, c.B / 255f, c.A / 255f);
    }

    public override string ToString()
    {
        return $"(0x{R:X2}{G:X2}{B:X2}{A:X2})";
    }
}

public struct FColor
{
    public float R;
    public float G;
    public float B;
    public float A;

    public static FColor Transparent => new(a: 0);
    public static FColor Black => new();
    public static FColor White => new(1, 1, 1);
    public static FColor Red => new(1);
    public static FColor Green => new(g: 1);
    public static FColor Blue => new(b: 1);
    public static FColor Yellow => new(1, 1);
    public static FColor Purple => new(1, b: 1);
    public static FColor Cyan => new(g: 1, b: 1);

    public FColor Clamped => new(R.Clamp01(), G.Clamp01(), B.Clamp01(), A.Clamp01());

    public FColor(float r = 0, float g = 0, float b = 0, float a = 1)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }

    public void Set(FColor c)
    {
        var clamped = c.Clamped;
        R = clamped.R;
        G = clamped.G;
        B = clamped.B;
        A = clamped.A;
    }

    public static FColor operator *(FColor c, float scl)
    {
        return new FColor(c.R * scl, c.G * scl, c.B * scl, c.A * scl);
    }

    public static FColor operator *(FColor c, FColor c2)
    {
        return new FColor(c.R * c2.R, c.G * c2.G, c.B * c2.B, c.A * c2.A);
    }

    public static FColor operator /(FColor c, float scl)
    {
        return new FColor(c.R / scl, c.G / scl, c.B / scl, c.A / scl);
    }

    public static FColor operator /(FColor c, FColor c2)
    {
        return new FColor(c.R / c2.R, c.G / c2.G, c.B / c2.B, c.A / c2.A);
    }


    public static implicit operator Color(FColor c)
    {
        var nc = c.Clamped * 255;
        return new Color(
            Convert.ToByte(nc.R),
            Convert.ToByte(nc.G),
            Convert.ToByte(nc.B),
            Convert.ToByte(nc.A)
        );
    }

    public override string ToString()
    {
        return $"({R * 100:N}, {G * 100:N}, {B * 100:N}, {A * 100:N})";
    }
}