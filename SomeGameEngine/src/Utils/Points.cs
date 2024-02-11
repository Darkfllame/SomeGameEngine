using SDL2;

namespace SomeGameEngine.Utils;

public struct Point
{
    public int x;
    public int y;

    internal SDL.SDL_Point toSdl => new() { x = x, y = y };

    public Point(int x = 0, int y = 0)
    {
        this.x = x;
        this.y = y;
    }
}

public struct FPoint
{
    public float x;
    public float y;

    internal SDL.SDL_FPoint toSDL => new() { x = x, y = y };

    public FPoint(float x = 0, float y = 0)
    {
        this.x = x;
        this.y = y;
    }
}