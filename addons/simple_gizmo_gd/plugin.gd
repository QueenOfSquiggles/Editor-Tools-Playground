tool
extends EditorPlugin

var spatial_gizmo_script = preload("res://addons/simple_gizmo_gd/SpatialGizmo.gd")
var spatial_gizmo = null

func _enter_tree() -> void:
	spatial_gizmo = spatial_gizmo_script.new()
	add_spatial_gizmo_plugin(spatial_gizmo)


func _exit_tree() -> void:
	remove_spatial_gizmo_plugin(spatial_gizmo)
