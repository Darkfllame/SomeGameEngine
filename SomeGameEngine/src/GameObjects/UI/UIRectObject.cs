using SomeGameEngine.Graphics;
using SomeGameEngine.Utils;

namespace SomeGameEngine.GameObjects.UI;

public class UIRectObject : UIObject
{
    protected override void Render(RenderGraphics graphics)
    {
        var gDims = graphics.Dimensions;
        ToScreenDims(gDims, out var screenPos, out var screenSize);
        graphics.RenderDraw(
            new Rect(screenPos.X, screenPos.Y, screenSize.X, screenSize.Y),
            DrawMode.Fill,
            Color
        );
    }
}