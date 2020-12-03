using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Flamingo
{
[CustomEditor(typeof(ScenarioElement))]
public class ScenarioElementInspector : Editor
{
	private ScenarioElement scenarioElement; 	/// <summary>Inspector's Target.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		scenarioElement = target as ScenarioElement;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();
		GUILayout.Space(20.0f);
		if(GUILayout.Button("Calculate Nodes")) scenarioElement.CalculateCellNodes();		
	}
}
}