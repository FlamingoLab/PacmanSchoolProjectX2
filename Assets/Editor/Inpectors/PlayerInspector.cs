using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Flamingo
{
[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{
	private Player player; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		player = target as Player;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		GUILayout.Space(20.0f);
		if(player.mateo != null && GUILayout.Button("Parent Mateo")) player.ParentMateo();		
	}
}
}