using SDL2;

namespace SomeGameEngine.Inputs;

public class Keyboard
{
    private readonly bool[] _keys = new bool[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];
    private readonly IntPtr _keysPtr;
    private readonly bool[] _lastKeys = new bool[(int)SDL.SDL_Scancode.SDL_NUM_SCANCODES];

    public Keyboard(IntPtr keys)
    {
        _keysPtr = keys;
        Update();
    }

    internal void Update()
    {
        var len = _keys.Length;
        for (var i = 0; i < len; i++)
            _lastKeys[i] = _keys[i];
        unsafe
        {
            for (var i = 0; i < len; i++)
                _keys[i] = Convert.ToBoolean(((byte*)_keysPtr)[i]);
        }
    }

    public bool KeyDown(Key key)
    {
        switch (key)
        {
            case Key.Unknown:
                return false;
            case Key.Any:
                foreach (var t in _keys)
                    if (t)
                        return true;
                return false;
            default:
                return _keys[(ushort)key];
        }
    }

    public bool KeyPressed(Key key)
    {
        switch (key)
        {
            case Key.Unknown:
                return false;
            case Key.Any:
                foreach (var t in _keys)
                    if (t)
                        return true;
                return false;
            default:
                return _keys[(ushort)key] && !_lastKeys[(ushort)key];
        }
    }

    public bool KeyReleased(Key key)
    {
        switch (key)
        {
            case Key.Unknown:
                return false;
            case Key.Any:
                foreach (var t in _keys)
                    if (t)
                        return true;
                return false;
            default:
                return !_keys[(ushort)key] && _lastKeys[(ushort)key];
        }
    }
}