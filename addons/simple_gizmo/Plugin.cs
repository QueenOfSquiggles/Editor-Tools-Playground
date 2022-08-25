#if TOOLS // limit to building with tools

using Godot;
using System;
using System.Diagnostics;

namespace SimpleGizmo
{
    [Tool]
    public class Plugin : EditorPlugin
    {
        private CannonGizmo cannonGizmo = null;


        public override void _EnterTree()
        {
            cannonGizmo = new CannonGizmo();            
            AddSpatialGizmoPlugin(cannonGizmo);
        }

        public override void _ExitTree()
        {
            if (cannonGizmo != null) RemoveSpatialGizmoPlugin(cannonGizmo);
        }
    }
}

#endif