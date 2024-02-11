namespace SomeGameEngine.Graphics;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true, Inherited = false)]
public class PreloadTextureAttribute : Attribute
{
    internal readonly string Filename;

    public PreloadTextureAttribute(string filename)
    {
        Filename = filename;
    }
}