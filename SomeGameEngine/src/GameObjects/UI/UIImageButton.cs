using SomeGameEngine.Graphics;
using SomeGameEngine.Utils;

namespace SomeGameEngine.GameObjects.UI;

public class UIImageButton : UIImageObject
{
    protected new Color Color;
    
    public delegate void ClickEvent(int mouseX, int mouseY);

    private bool _clicking;
    private bool _hovered;

    public Color BaseColor;
    public Color HoveredColor;
    public Color ClickColor;

    public event ClickEvent Clicks;

    public UIImageButton(string filename, Color baseColor, Color hoveredColor, Color clickColor)
        : base(filename)
    {
        BaseColor = baseColor;
        HoveredColor = hoveredColor;
        ClickColor = clickColor;
        Clicks = (_, _) => { };
    }

    protected override void Update(double dt)
    {
        var mPos = GetMousePosition();
        var mx = mPos.X;
        var my = mPos.Y;

        var gDims = Game.INSTANCE?.Graphics.Dimensions ?? throw new Exception("Unexpected null game instance");
        ToScreenDims(gDims, out var screenPos, out var screenSize);

        _hovered = mx > screenPos.X && mx < screenPos.X + screenSize.X &&
                   my > screenPos.Y && my < screenPos.Y + screenSize.Y;
    }

    protected override void Render(RenderGraphics graphics)
    {
        if (_clicking)
            Color = ClickColor;
        else if (_hovered)
            Color = HoveredColor;
        else
            Color = BaseColor;
        
        var gDims = graphics.Dimensions;
        ToScreenDims(gDims, out var screenPos, out var screenSize, out var clipPos, out var clipSize);
        graphics.RenderDraw(
            Texture,
            new Rect(clipPos.X, clipPos.Y, clipSize.X, clipSize.Y),
            new Rect(screenPos.X, screenPos.Y, screenSize.X, screenSize.Y),
            colorMod: Color
        );
    }

    protected override void OnButton(byte button, bool up, int mouseX, int mouseY)
    {
        if (button == 1)
        {
            if (_hovered)
            {
                if (!(up || _clicking))
                {
                    _clicking = true;
                }
            }
            if (up && _clicking)
            {
                var gDims = Game.INSTANCE?.Graphics.Dimensions ?? throw new Exception("Unexpected null game instance");
                ToScreenDims(gDims, out var screenPos, out _);
                
                _clicking = false;
                if (_hovered)
                    Clicks(mouseX - screenPos.X, mouseY - screenPos.Y);
            }
        }
    }
}