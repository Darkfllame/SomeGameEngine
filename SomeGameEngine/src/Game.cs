using System.Collections;
using System.Diagnostics;
using SomeGameEngine.GameObjects;
using SomeGameEngine.Graphics;
using SomeGameEngine.Inputs;
using SomeGameEngine.Utils;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace SomeGameEngine;

public class Game
{
    // ReSharper disable once InconsistentNaming
    public static Game? INSTANCE { get; private set; }
    
    private readonly Stopwatch _timeWatch;
    private readonly bool _initialized;
    private readonly bool _initializedImg;
    private readonly IntPtr _window;
    private readonly IntPtr _renderer;
    private IntPtr _renderTexture;
    private bool _running;

    public int Width { get; private set; }
    public int Height { get; private set; }
    public readonly RootObject Root;

    public Keyboard Keyboard { get; }
    public Mouse Mouse { get; }
    public RenderGraphics Graphics { get; }

    public Game(int width = 800, int height = 600, bool resizable = false)
    {
        if (INSTANCE != null)
            throw new Exception("Cannot create another instance");

        INSTANCE = this;

        if (!(_initialized = SDL_Init(SDL_INIT_EVERYTHING) >= 0))
            throw new Exception($"Error when initializing SDL : {SDL_GetError()}");
        var imgFlags = IMG_InitFlags.IMG_INIT_PNG | IMG_InitFlags.IMG_INIT_JPG;
        if (!(_initializedImg = (IMG_Init(imgFlags) & (int)imgFlags) != 0))
            throw new Exception($"Error when initializing SDL_image : {SDL_GetError()}");


        if ((_window = SDL_CreateWindow(
                "My Window",
                SDL_WINDOWPOS_CENTERED,
                SDL_WINDOWPOS_CENTERED,
                width,
                height,
                resizable ? SDL_WindowFlags.SDL_WINDOW_RESIZABLE : 0
            )) == 0)
            throw new Exception($"Error when creating window : {SDL_GetError()}");
        Width = width;
        Height = height;

        if ((_renderer = SDL_CreateRenderer(
                _window,
                -1,
                SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC | SDL_RendererFlags.SDL_RENDERER_TARGETTEXTURE
            )) == 0)
            throw new Exception($"Error when creating renderer : {SDL_GetError()}");

        if ((_renderTexture = SDL_CreateTexture(
                _renderer,
                SDL_PIXELFORMAT_RGBA8888,
                (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET,
                width, height
            )) == 0)
            throw new Exception($"Error when creating rendering texture : {SDL_GetError()}");

        _timeWatch = new Stopwatch();
        
        Keyboard = new Keyboard(SDL_GetKeyboardState(out var _));
        Mouse = new Mouse(this);
        Graphics = new RenderGraphics(_window, _renderer);

        Root = new RootObject();
        
        Texture.PreloadTextures();
    }

    ~Game()
    {
        if (_renderTexture != 0)
            SDL_DestroyTexture(_renderTexture);
        if (_renderer != 0)
            SDL_DestroyRenderer(_renderer);
        if (_window != 0)
            SDL_DestroyWindow(_window);
        if (_initialized)
            SDL_Quit();
        if (_initializedImg)
            IMG_Quit();
    }

    public void MainLoop()
    {
        Stopwatch watch = new();
        watch.Start();
        _timeWatch.Start();

        _running = true;
        while (_running)
        {
            while (SDL_PollEvent(out var ev) != 0)
            {
                switch (ev.type)
                {
                    case SDL_EventType.SDL_QUIT:
                    {
                        _running = false;
                        return;
                    }
                    case SDL_EventType.SDL_KEYDOWN:
                    {
                        Root.OnKeySelf((Key)ev.key.keysym.scancode, false);
                        break;
                    }
                    case SDL_EventType.SDL_KEYUP:
                    {
                        Root.OnKeySelf((Key)ev.key.keysym.scancode, true);
                        break;
                    }
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                    {
                        Root.OnButtonSelf(ev.button.button, false, ev.button.x, Height - ev.button.y);
                        break;
                    }
                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                    {
                        Root.OnButtonSelf(ev.button.button, true, ev.button.x, Height - ev.button.y);
                        break;
                    }
                    case SDL_EventType.SDL_MOUSEMOTION:
                    {
                        Root.OnMouseMotionSelf(ev.motion.xrel, Height - ev.motion.yrel);
                        break;
                    }
                    case SDL_EventType.SDL_WINDOWEVENT:
                    {
                        switch (ev.window.windowEvent)
                        {
                            case SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
                                if (ev.window.windowID == SDL_GetWindowID(_window))
                                {
                                    _running = false;
                                    return;
                                }

                                break;
                            case SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED:
                                if (ev.window.windowID == SDL_GetWindowID(_window))
                                {
                                    Width = ev.window.data1;
                                    Height = ev.window.data2;
                                    
                                    if ((_renderTexture = SDL_CreateTexture(
                                            _renderer,
                                            SDL_PIXELFORMAT_RGBA8888,
                                            (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET,
                                            Width, Height
                                        )) == 0)
                                        throw new Exception($"Error when creating rendering texture : {SDL_GetError()}");
                                }

                                break;
                        }

                        break;
                    }
                }
            }

            Mouse.Update();
            Keyboard.Update();

            watch.Stop();
            var dt = watch.Elapsed.TotalSeconds;
            watch.Restart();

            Update(dt);

            Graphics.RenderTarget = _renderTexture;
            Graphics.RenderClear(Color.Black);

            Render();

            Graphics.RenderTarget = 0;
            Graphics.RenderClear(Color.Black);
            Graphics.RenderCopy(_renderTexture);
            Graphics.RenderPresent();
        }
    }

    private double _delta = 0;
    private void Update(double dt)
    {
        _delta += dt;

        if (_delta >= 1)
        {
            SDL_SetWindowTitle(_window, $"My Window | DT : {1 / dt:N0}");
            _delta = 0;
        }

        Root.UpdateSelf(dt);
    }

    private void Render()
    {
        Root.RenderSelf(Graphics);
    }

    public double GetTime()
    {
        return _timeWatch.Elapsed.TotalSeconds;
    }
}