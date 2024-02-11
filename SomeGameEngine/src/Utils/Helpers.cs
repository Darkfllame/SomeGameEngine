using SDL2;

namespace SomeGameEngine.Utils;

public static class HelperExtensions
{
    internal static SDL.SDL_RendererFlip ToSdl(this Flip flip)
    {
        return flip switch
        {
            Flip.Vertical => SDL.SDL_RendererFlip.SDL_FLIP_VERTICAL,
            Flip.Horizontal => SDL.SDL_RendererFlip.SDL_FLIP_HORIZONTAL,
            _ => SDL.SDL_RendererFlip.SDL_FLIP_NONE
        };
    }
    internal static SDL.SDL_BlendMode ToSdl(this BlendMode mode)
    {
        return mode switch
        {
            BlendMode.Blend => SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND,
            BlendMode.Add => SDL.SDL_BlendMode.SDL_BLENDMODE_ADD,
            BlendMode.Mod => SDL.SDL_BlendMode.SDL_BLENDMODE_MOD,
            BlendMode.Mul => SDL.SDL_BlendMode.SDL_BLENDMODE_MUL,
            _ => SDL.SDL_BlendMode.SDL_BLENDMODE_NONE
        };
    }
    internal static BlendMode FromSdl(this SDL.SDL_BlendMode mode)
    {
        return mode switch
        {
            SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND => BlendMode.Blend,
            SDL.SDL_BlendMode.SDL_BLENDMODE_ADD => BlendMode.Add,
            SDL.SDL_BlendMode.SDL_BLENDMODE_MOD => BlendMode.Mod,
            SDL.SDL_BlendMode.SDL_BLENDMODE_MUL => BlendMode.Mul,
            _ => BlendMode.None
        };
    }

    public static void ForEach<T>(this T[] arr, Action<T> action)
    {
        Array.ForEach(arr, action);
    }

    public static void ForEach<T>(this ReadOnlySpan<T> arr, Action<T> action)
    {
        foreach (var elem in arr)
            action.Invoke(elem);
    }

    public static float Clamp(this float v, float min, float max) =>
        float.Clamp(v, min, max);

    public static float Clamp01(this float v) => v.Clamp(0, 1);
    
    public static int ToInt(this float v) => Convert.ToInt32(v);
}

public enum Flip
{
    None,
    Vertical,
    Horizontal
}

public enum DrawMode
{
    Outline,
    Fill
}

public enum BlendMode
{
    None,
    Blend,
    Add,
    Mod,
    Mul,
}