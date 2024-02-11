using SDL2;

namespace SomeGameEngine.Utils;

public struct Rect
{
    public int x;
    public int y;
    public int w;
    public int h;

    internal SDL.SDL_Rect ToSdl
    {
        get
        {
            var sdlRect = new SDL.SDL_Rect { x = x, y = y, w = w, h = h };
            if (w < 0)
            {
                sdlRect.w = -w;
                sdlRect.x += w;
            }
            if (h < 0)
            {
                sdlRect.h = -h;
                sdlRect.y += h;
            }
            return sdlRect;
        }
    }

    public Rect(int x = 0, int y = 0, int w = 0, int h = 0)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
    }
}

public struct FRect
{
    public float x;
    public float y;
    public float w;
    public float h;

    internal SDL.SDL_FRect ToSdl => new() { x = x, y = y, w = w, h = h };

    public static implicit operator SDL.SDL_FRect(FRect rect)
    {
        return new SDL.SDL_FRect { x = rect.x, y = rect.y, w = rect.w, h = rect.h };
    }

    public FRect(float x = 0, float y = 0, float w = 0, float h = 0)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
    }
}