using static SDL2.SDL;
using static SDL2.SDL_image;

namespace SomeGameEngine.Graphics;

public class Texture
{
    private static readonly Utils.List<Texture> PreloadList = new();

    internal static void PreloadTextures()
    {
        var attributes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass)
            .SelectMany(x => x.GetCustomAttributes(typeof(PreloadTextureAttribute), false));

        var enumerable = attributes as object[] ?? attributes.ToArray();
        PreloadList.Capacity = enumerable.Count();
        foreach (var obj in enumerable)
        {
            if (obj is PreloadTextureAttribute attrib)
            {
                Console.WriteLine($"Preloading texture {attrib.Filename}");
                PreloadList.Append(new Texture(attrib.Filename));
            }
        }
    }
    
    private readonly unsafe SDL_Surface* _surface;
    
    public readonly string? Filename;
    public int Width
    {
        get
        {
            unsafe
            {
                return _surface->w;
            }
        }
    }
    public int Height
    {
        get
        {
            unsafe
            {
                return _surface->h;
            }
        }
    }
    public byte Depth
    {
        get
        {
            unsafe
            {
                return ((SDL_PixelFormat*)_surface->format)->BitsPerPixel;
            }
        }
    }

    public Texture(int width, int height, int depth)
    {
        var surf = SDL_CreateRGBSurface(0, width, height, depth, 0, 0, 0, 0);
        if (surf == 0)
            throw new Exception($"Cannot create surface : {SDL_GetError()}");

        unsafe
        {
            _surface = (SDL_Surface*)surf;
        }
    }

    public static Texture FromFile(string filename)
    {
        try
        {
            return PreloadList.First(x => filename.Equals(x.Filename));
        }
        catch (Exception e)
        {
            Console.WriteLine("Cannot find texture in preload list, creating a new one..");
        }

        return new Texture(filename);
    }
    private Texture(string filename)
    {
        var surf = IMG_Load(filename);
        if (surf == 0)
            throw new Exception($"Cannot load image from file : {SDL_GetError()}");
        Filename = filename;
        unsafe
        {
            _surface = (SDL_Surface*)surf;
        }
    }

    ~Texture()
    {
        unsafe
        {
            SDL_FreeSurface((IntPtr)_surface);
        }
    }

    internal IntPtr ToSdl(IntPtr renderer)
    {
        unsafe
        {
            var ptr = SDL_CreateTextureFromSurface(renderer, (IntPtr)_surface);
            if (ptr == 0)
                throw new Exception($"Cannot create texture : {SDL_GetError()}");
            return ptr;
        }
    }

    public override string ToString()
    {
        return Filename is null ?
            $"({Width}, {Height}, {Depth})" 
            : $"('/{Filename}', {Width}, {Height}, {Depth})";
    }
}