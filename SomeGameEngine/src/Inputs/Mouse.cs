using SomeGameEngine.LinearMath;
using static SDL2.SDL;

namespace SomeGameEngine.Inputs;

public class Mouse
{
    private uint _buttonMask;
    private uint _lastButtonMask;
    private int _mx;
    private int _my;
    private readonly Game _game;

    public Mouse(Game game)
    {
        _buttonMask = 0;
        _game = game;
        Update();
    }

    public void Update()
    {
        _lastButtonMask = _buttonMask;
        _buttonMask = SDL_GetMouseState(out _mx, out _my);
    }

    public iVec2 Position => new iVec2(_mx, _game.Height - _my);

    public bool ButtonDown(byte button)
    {
        return Convert.ToBoolean(_buttonMask & SDL_BUTTON(button));
    }

    public bool ButtonPressed(byte button)
    {
        return Convert.ToBoolean(_buttonMask & SDL_BUTTON(button)) &&
               !Convert.ToBoolean(_lastButtonMask & SDL_BUTTON(button));
    }

    public bool ButtonReleased(byte button)
    {
        return !Convert.ToBoolean(_buttonMask & SDL_BUTTON(button)) &&
               Convert.ToBoolean(_lastButtonMask & SDL_BUTTON(button));
    }
}