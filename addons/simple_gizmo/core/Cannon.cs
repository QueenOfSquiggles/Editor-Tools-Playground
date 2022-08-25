using Godot;
using MonoCustomResourceRegistry;

namespace SimpleGizmo
{

    [Tool]
    [RegisteredType(nameof(Cannon), "", nameof(Spatial))]
    public class Cannon : Spatial
    {
        [Export] public Vector3 target = Vector3.Zero;
        [Export] public PackedScene projectileScene = null;
    }
}