using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Godot.Collections;

namespace SimpleGizmo
{
    [Tool]
    public class CannonGizmo : EditorSpatialGizmoPlugin
    {

        public CannonGizmo()
        {
            CreateMaterial("main", new Color(1f, 0f, 0f));
            CreateHandleMaterial("handles"); // using default texture
        }

        public override string GetName()
        {
            return "Cannon Gizmo";
        }

        public override bool HasGizmo(Spatial spatial)
        {
            return spatial is Cannon;
        }


        public override void Redraw(EditorSpatialGizmo gizmo)
        {
            gizmo.Clear();
            var cannon_spatial = gizmo.GetSpatialNode() as Cannon;
            var lines = new List<Vector3>();
            var handles = new List<Vector3>();

            lines.Add(cannon_spatial.GlobalTransform.origin);
            lines.Add(cannon_spatial.GlobalTransform.origin + cannon_spatial.target);

            handles.Add(cannon_spatial.GlobalTransform.origin + cannon_spatial.target);

            gizmo.AddLines(lines.ToArray(), GetMaterial("main", gizmo), false);
            gizmo.AddHandles(handles.ToArray(), GetMaterial("handles", gizmo));
        }

        public override void CommitHandle(EditorSpatialGizmo gizmo, int index, object restore, bool cancel)
        {

        }

        public override string GetHandleName(EditorSpatialGizmo gizmo, int index)
        {
            switch (index)
            {
                case 0:
                    return "Target";
                default:
                    return "Unnamed Handle";
            }
        }

        public override object GetHandleValue(EditorSpatialGizmo gizmo, int index)
        {
            var cannon_spatial = gizmo.GetSpatialNode() as Cannon;
            switch(index)
            {
                case 0:
                    return cannon_spatial.target;
                default:
                    return Vector3.Zero;
            }
        }

        public override void SetHandle(EditorSpatialGizmo gizmo, int index, Camera camera, Vector2 point)
        {
            var cannon_spatial = gizmo.GetSpatialNode() as Cannon;
            switch(index)
            {
                case 0:
                    MoveTarget(cannon_spatial, camera, point);
                    break;
                default:
                    return; // escape so no redraw occurs
            }
            Redraw(gizmo); // force a redraw on changes made.
        }

        private void MoveTarget(Cannon cannon, Camera camera, Vector2 mousePoint)
        {
            // this transformation is needed because the cannon.target variable is stored in local space.
            var glob_pos = cannon.GlobalTransform.origin + cannon.target;
            glob_pos = MoveHandleGlobal(camera, mousePoint, glob_pos);
            cannon.target = glob_pos - cannon.GlobalTransform.origin;
        }

        private Vector3 MoveHandleGlobal(Camera camera, Vector2 mousePoint, Vector3 handle)
        {
            // constructs a plan perpendicular to the camera view direction
            var cam_basis = camera.GlobalTransform.basis;
            var move_plane = new Plane(handle, handle + cam_basis.x, handle + cam_basis.y);

            // find the intersection of the projected mouse ray and the plane
            var temp = move_plane.IntersectRay(camera.ProjectRayOrigin(mousePoint), camera.ProjectRayNormal(mousePoint));

            // if the value is not null, return as a Vector3
            if (temp != null) return (Vector3)temp;
            // value was null, no intersection was found -> no changes to position
            return handle; // no change
        }
    }
}