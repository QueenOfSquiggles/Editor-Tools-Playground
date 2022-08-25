tool
extends EditorSpatialGizmoPlugin

var handle_pos = Vector3()

func _init():
	create_material("main", Color(1,0,0))
	create_handle_material("handles")

func has_gizmo(spatial):
	return spatial is Spatial


func redraw(gizmo):
	gizmo.clear()

	var spatial = gizmo.get_spatial_node()

	var lines = PoolVector3Array()

	lines.push_back(handle_pos)
	lines.push_back(spatial.global_transform.origin)

	var handles = PoolVector3Array()

	handles.push_back(handle_pos)

	gizmo.add_lines(lines, get_material("main", gizmo), false)
	gizmo.add_handles(handles, get_material("handles", gizmo))

func get_handle_name(gizmo: EditorSpatialGizmo, index: int) -> String:
	match index:
		0: return "Handle"
	return "Unnamed Handle"

func get_handle_value(gizmo: EditorSpatialGizmo, index: int):
	match index:
		0: return handle_pos
	return 0

func set_handle(gizmo: EditorSpatialGizmo, index: int, camera: Camera, point: Vector2) -> void:
	# redirect logic to 
	match index:
		0 : 
			handle_pos = _move_handle_global(gizmo, camera, point, handle_pos)
		_ : return
	redraw(gizmo)
	
func _move_handle_global(gizmo: EditorSpatialGizmo, camera: Camera, point: Vector2, handle: Vector3) -> Vector3:
	# a standard system for moving a handle in 3D space
	# assumes that the handle vector is in global space
	# - - - - - - 

	# construct a plane that is perpendicular to the camera view direction and intersecting with the current handle position
	var cam_basis = camera.global_transform.basis
	var plane = Plane(handle, handle + cam_basis.x, handle + cam_basis.y)
	
	# constructs components to create a ray of infinite length emitting from the mouse position
	var ray_from = camera.project_ray_origin(point)
	var ray_dir = camera.project_ray_normal(point)

	# uses the plane's built-in function for finding a point that intersects with the ray (built from the components found earlier)
	# It is technically possible for this to return null, but because the plane is perpendicular to the camera view angle, only an FOV of 180+ degrees would allow for a null value to be returned. 
	var pos = plane.intersects_ray(ray_from, ray_dir)
	# return the value to be used
	return handle
