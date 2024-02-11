using SomeGameEngine.Graphics;
using SomeGameEngine.Inputs;
using SomeGameEngine.LinearMath;
using SomeGameEngine.Utils;

namespace SomeGameEngine.GameObjects;

public abstract class GameObject
{
    private readonly Utils.List<GameObject> _children;
    private GameObject? _parent;

    public Vec2 Position;
    public float Rotation;
    public Vec2 Size;

    public GameObject(Vec2 position = default, float rotation = 0f)
        : this(position, Vec2.One, rotation)
    {
    }

    public GameObject(Vec2 position, Vec2 size, float rotation = 0f)
    {
        Position = position;
        Size = size;
        Rotation = rotation;
        _children = new Utils.List<GameObject>(1);
    }

    public GameObject? Parent
    {
        get => _parent;
        set
        {
            _parent?._children.Remove(this);
            value?._children.Append(this);

            _parent = value;
        }
    }

    public ReadOnlySpan<GameObject> Children => _children.ToArray();

    public void Translate(Vec2 offset)
    {
        Position += offset;
    }

    public void MoveTo(Vec2 pos)
    {
        Position = pos;
    }
    
    // Some inputs shit

    protected iVec2 GetMousePosition()
    {
        return Game.INSTANCE?.Mouse.Position ?? iVec2.Zero;
    }

    protected bool KeyDown(Key key)
    {
        return Game.INSTANCE?.Keyboard.KeyDown(key) ?? false;
    }

    protected bool KeyPressed(Key key)
    {
        return Game.INSTANCE?.Keyboard.KeyPressed(key) ?? false;
    }

    protected bool KeyReleased(Key key)
    {
        return Game.INSTANCE?.Keyboard.KeyReleased(key) ?? false;
    }

    protected bool ButtonDown(byte button)
    {
        return Game.INSTANCE?.Mouse.ButtonDown(button) ?? false;
    }

    protected bool ButtonPressed(byte button)
    {
        return Game.INSTANCE?.Mouse.ButtonPressed(button) ?? false;
    }

    protected bool ButtonReleased(byte button)
    {
        return Game.INSTANCE?.Mouse.ButtonReleased(button) ?? false;
    }
    
    // Events

    protected virtual void Update(double dt)
    {
    }

    protected virtual void Render(RenderGraphics graphics)
    {
    }

    protected virtual void OnKey(Key key, bool up)
    {
    }

    protected virtual void OnButton(byte button, bool up, int mouseX, int mouseY)
    {
    }

    protected virtual void OnMouseMotion(int deltaX, int deltaY)
    {
    }
    
    // Events callers

    internal void UpdateSelf(double dt)
    {
        _children.ForEach((child, _) => child.UpdateSelf(dt));
        Update(dt);
    }

    internal void RenderSelf(RenderGraphics graphics)
    {
        Children.ForEach(child => child.RenderSelf(graphics));
        Render(graphics);
    }

    internal void OnKeySelf(Key key, bool up)
    {
        _children.ForEach((child, _) => child.OnKeySelf(key, up));
        OnKey(key, up);
    }

    internal void OnButtonSelf(byte button, bool up, int mouseX, int mouseY)
    {
        _children.ForEach((child, _) => child.OnButtonSelf(button, up, mouseX, mouseY));
        OnButton(button, up, mouseX, mouseY);
    }

    internal void OnMouseMotionSelf(int deltaX, int deltaY)
    {
        _children.ForEach((child, _) => child.OnMouseMotionSelf(deltaX, deltaY));
        OnMouseMotion(deltaX, deltaY);
    }
}