using SomeGameEngine.Graphics;
using SomeGameEngine.LinearMath;
using SomeGameEngine.Utils;

namespace SomeGameEngine.GameObjects.UI;

public abstract class UIObject : GameObject
{
    public FColor Color;
    public new Dims2D Position;
    public new Dims2D Size;

    protected void ToScreenDims(iVec2 screen, out iVec2 screenPos, out iVec2 screenSize)
    {
        var width = screen.X;
        var height = screen.Y;
        screenPos =  new iVec2((Position.X + Position.W * width).ToInt(), (Position.Y + Position.H * height).ToInt());
        screenSize = new iVec2((Size.X + Size.W * width).ToInt(), (Size.Y + Size.H * height).ToInt());
    }
}