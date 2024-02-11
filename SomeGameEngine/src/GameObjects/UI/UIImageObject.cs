using SDL2;
using SomeGameEngine.Graphics;
using SomeGameEngine.LinearMath;
using SomeGameEngine.Utils;

namespace SomeGameEngine.GameObjects.UI;

public class UIImageObject : UIObject
{
    public Texture Texture;

    public Dims2D ClipRegion = new(0, 0, 1, 1);

    public UIImageObject(string filename)
    {
        Texture = Texture.FromFile(filename);
    }

    protected void ToScreenDims(iVec2 screen, out iVec2 screenPos, out iVec2 screenSize, out iVec2 clipPos, out iVec2 clipSize)
    {
        ToScreenDims(screen, out screenPos, out screenSize);
        clipPos = new iVec2(ClipRegion.X, ClipRegion.Y);
        clipSize = new iVec2((ClipRegion.W * Texture.Width).ToInt(), (ClipRegion.H * Texture.Height).ToInt());
    }

    protected override void Render(RenderGraphics graphics)
    {
        var gDims = graphics.Dimensions;
        ToScreenDims(gDims, out var screenPos, out var screenSize, out var clipPos, out var clipSize);
        graphics.RenderDraw(
            Texture,
            new Rect(clipPos.X, clipPos.Y, clipSize.X, clipSize.Y),
            new Rect(screenPos.X, screenPos.Y, screenSize.X, screenSize.Y),
            colorMod: Color
        );
    }
}