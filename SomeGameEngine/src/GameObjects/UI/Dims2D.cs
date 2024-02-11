namespace SomeGameEngine.GameObjects.UI;

public struct Dims2D
{
    public int X;
    public int Y;
    public float W;
    public float H;

    public Dims2D(int x = 0, int y = 0, float w = 0, float h = 0)
    {
        X = x;
        Y = y;
        W = w;
        H = h;
    }

    public override string ToString()
    {
        return $"({X}, {Y}, {W}, {H})";
    }
}