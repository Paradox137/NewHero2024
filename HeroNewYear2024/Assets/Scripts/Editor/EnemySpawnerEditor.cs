using HeroScripts.Logic;
using UnityEditor;
using UnityEngine;

namespace HeroScripts.Editor
{
	[CustomEditor(typeof(SpawnMarker))]
	public class SpawnerMarkerEditor : UnityEditor.Editor
	{
		[DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
		public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmoType)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(spawner.transform.position, 0.5f);
		}
	}
}
