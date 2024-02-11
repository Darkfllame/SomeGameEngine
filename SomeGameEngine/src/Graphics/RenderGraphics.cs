using SomeGameEngine.LinearMath;
using SomeGameEngine.Utils;
using static SDL2.SDL;

namespace SomeGameEngine.Graphics;

public class RenderGraphics
{
    private readonly IntPtr _renderer;
    private readonly IntPtr _window;

    internal RenderGraphics(IntPtr window, IntPtr renderer)
    {
        _window = window;
        _renderer = renderer;
    }

    public Color DrawColor
    {
        get
        {
            if (SDL_GetRenderDrawColor(_renderer, out var r, out var g, out var b, out var a) < 0)
                throw new Exception($"Cannot get drawing color: {SDL_GetError()}");

            return new Color(r, g, b, a);
        }
        set
        {
            if (SDL_SetRenderDrawColor(_renderer, value.R, value.G, value.B, value.A) < 0)
                throw new Exception($"Cannot set drawing color: {SDL_GetError()}");
        }
    }
    public BlendMode BlendMode
    {
        get
        {
            if (SDL_GetRenderDrawBlendMode(_renderer, out var blendMode) < 0)
                throw new Exception($"Error getting blending mode : {SDL_GetError()}");

            return blendMode.FromSdl();
        }
        set
        {
            if (SDL_SetRenderDrawBlendMode(_renderer, value.ToSdl()) < 0)
                throw new Exception($"Error changing blending mode : {SDL_GetError()}");
        }
    }

    public IntPtr RenderTarget
    {
        get
        {
            IntPtr target;
            if ((target = SDL_GetRenderTarget(_renderer)) == 0)
                throw new Exception($"Cannot get rendering target: {SDL_GetError()}");

            return target;
        }
        set
        {
            if (SDL_SetRenderTarget(_renderer, value) < 0)
                throw new Exception($"Cannot set rendering target: {SDL_GetError()}");
        }
    }

    public iVec2 Dimensions
    {
        get
        {
            SDL_GetWindowSize(_window, out var w, out var h);
            return new iVec2(w, h);
        }
        set => SDL_SetWindowSize(_window, value.X, value.Y);
    }

    public void RenderClear()
    {
        if (SDL_RenderClear(_renderer) < 0)
            throw new Exception($"Error clearing renderer : {SDL_GetError()}");
    }

    public void RenderClear(Color color)
    {
        DrawColor = color;
        if (SDL_RenderClear(_renderer) < 0)
            throw new Exception($"Error clearing renderer : {SDL_GetError()}");
    }

    internal void RenderCopy(IntPtr texture = 0, Rect? srcRect = null, Rect? dstRect = null, double angle = 0.0,
        Point? center = null, Flip flip = Flip.Vertical)
    {
        unsafe
        {
            var sdlSrc = srcRect?.ToSdl;
            var sdlDst = dstRect?.ToSdl;
            var sdlCen = center?.toSdl;
            
            var sdlSrcVal = sdlSrc ?? new SDL_Rect();
            var sdlDstVal = sdlDst ?? new SDL_Rect();
            var sdlCenVal = sdlCen ?? new SDL_Point();
            
            if (SDL_RenderCopyEx(
                    _renderer,
                    texture,
                    sdlSrc is null ? 0 : (IntPtr)(&sdlSrcVal),
                    sdlDst is null ? 0 : (IntPtr)(&sdlDstVal),
                    angle,
                    sdlCen is null ? 0 : (IntPtr)(&sdlCenVal),
                    flip.ToSdl()
                ) < 0)
                throw new Exception($"Error copying texture to renderer : {SDL_GetError()}");
        }
    }

    public void RenderPresent()
    {
        SDL_RenderPresent(_renderer);
    }

    public void RenderDraw(Texture texture, Rect? srcRect = null, Rect? dstRect = null, double angle = 0.0,
        iVec2? center = null, Flip flip = Flip.Vertical, Color? colorMod = null)
    {
        var texPtr = texture.ToSdl(_renderer);
        var color = colorMod ?? Color.White;

        if (SDL_SetTextureBlendMode(texPtr, SDL_BlendMode.SDL_BLENDMODE_BLEND) < 0)
            throw new Exception($"Cannot change texture blend mode : {SDL_GetError()}");
        if (SDL_SetTextureColorMod(texPtr, color.R, color.G, color.B) < 0)
            throw new Exception($"Cannot change texture color modifier : {SDL_GetError()}");
        if (SDL_SetTextureAlphaMod(texPtr, color.A) < 0)
            throw new Exception($"Cannot change texture alpha modifier : {SDL_GetError()}");
        
        RenderCopy(texPtr, srcRect, dstRect, angle, center != null ? new Point(center.Value.X, center.Value.Y) : null, flip);
        SDL_DestroyTexture(texPtr);
    }

    public void RenderDraw(Rect rect, DrawMode mode = DrawMode.Outline, Color? color = default)
    {
        DrawColor = color ?? DrawColor;

        var sdlRect = rect.ToSdl;

        switch (mode)
        {
            case DrawMode.Outline:
                if (SDL_RenderDrawRect(_renderer, ref sdlRect) < 0)
                    throw new Exception($"Error changing rendering color : {SDL_GetError()}");
                break;
            case DrawMode.Fill:
                if (SDL_RenderFillRect(_renderer, ref sdlRect) < 0)
                    throw new Exception($"Error changing rendering color : {SDL_GetError()}");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }

    public void RenderDraw(Rect[] rects, DrawMode mode = DrawMode.Outline, Color? color = default)
    {
        DrawColor = color ?? DrawColor;

        var len = rects.Length;

        var sdlRects = new SDL_Rect[len];
        for (var i = 0; i < rects.Length; i++)
            sdlRects[i] = rects[i].ToSdl;

        switch (mode)
        {
            case DrawMode.Outline:
                if (SDL_RenderDrawRects(_renderer, sdlRects, len) < 0)
                    throw new Exception($"Error changing rendering color : {SDL_GetError()}");
                break;
            case DrawMode.Fill:
                if (SDL_RenderFillRects(_renderer, sdlRects, len) < 0)
                    throw new Exception($"Error changing rendering color : {SDL_GetError()}");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
        }
    }
}