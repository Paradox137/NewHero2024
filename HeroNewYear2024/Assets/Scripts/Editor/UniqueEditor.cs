using System;
using System.Linq;
using HeroScripts.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HeroScripts.Editor
{
	[CustomEditor(typeof(UniqueID))]
	public class UniqueEditor : UnityEditor.Editor
	{
		private void OnEnable()
		{
			var uniqueID = (UniqueID) target;

			if(IsPrefab(uniqueID))
				return;
			
			if (string.IsNullOrEmpty(uniqueID.ID))
				Generate(uniqueID);
			else
			{
				UniqueID[] uniqueIds = FindObjectsOfType<UniqueID>();
				
				if(uniqueIds.Any(other => other != uniqueID && other.ID == uniqueID.ID))
					Generate(uniqueID);
			}
		}
		private void Generate(UniqueID uniqueID)
		{
			uniqueID.ID = $"{uniqueID.gameObject.scene.name}_{Guid.NewGuid().ToString()}";
			
			if (!Application.isPlaying)
			{
				EditorUtility.SetDirty(uniqueID);

				EditorSceneManager.MarkSceneDirty(uniqueID.gameObject.scene);
			}
		}
		private bool IsPrefab(UniqueID uniqueID) => uniqueID.gameObject.scene.rootCount == 0;
	}
}
