using SomeGameEngine.GameObjects.UI;
using SomeGameEngine.Graphics;
using SomeGameEngine.Utils;

namespace SomeGameEngine;

[PreloadTexture("idontfuckingknow.png")]
public static class Program
{
    public static void Main(string[] args)
    {
        var game = new Game();
        var root = game.Root;

        game.Graphics.BlendMode = BlendMode.Blend;

        var imageButton = new UIImageButton(
            "idontfuckingknow.png",
            FColor.White,
            new FColor(0.7f, 0.7f, 1f),
            new FColor(1f, 0.5f, 0.5f)
        ) {
            Parent = root,
            Position = new Dims2D(0, 0, w: 0.25f, h: 0.25f),
            Size = new Dims2D(0, 0, .5f, .5f),
        };
        
        game.MainLoop();
    }
}