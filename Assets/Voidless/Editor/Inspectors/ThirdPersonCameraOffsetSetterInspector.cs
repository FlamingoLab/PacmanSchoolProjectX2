using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(ThirdPersonCameraOffsetSetter))]
public class ThirdPersonCameraOffsetSetterInspector : Editor
{
	private static readonly string LABEL_TEST_CAMERA = "Test Camera."; 		/// <summary>Test Cameta button's label.</summary>

	private ThirdPersonCameraOffsetSetter thirdPersonCameraOffsetSetter; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	void OnEnable()
	{
		thirdPersonCameraOffsetSetter = target as ThirdPersonCameraOffsetSetter;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		if(GUILayout.Button(LABEL_TEST_CAMERA))
		{
			if(ThirdPersonCamera.Instance != null)
			thirdPersonCameraOffsetSetter.TestCamera(ThirdPersonCamera.Instance);
		}
	}
}
}