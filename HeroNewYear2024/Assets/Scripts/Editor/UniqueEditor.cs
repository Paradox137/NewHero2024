using System;
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

			if (string.IsNullOrEmpty(uniqueID.ID))
				Generate(uniqueID);
		}
		private void Generate(UniqueID uniqueID)
		{
			uniqueID.ID = Guid.NewGuid().ToString();
			
			if (!Application.isPlaying)
			{
				EditorUtility.SetDirty(uniqueID);

				EditorSceneManager.MarkSceneDirty(uniqueID.gameObject.scene);
			}
		}
	}
}
