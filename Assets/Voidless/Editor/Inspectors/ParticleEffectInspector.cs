using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Voidless
{
[CustomEditor(typeof(ParticleEffect))]
public class ParticleEffectInspector : Editor
{
	protected ParticleEffect particleEffect; 	/// <summary>Inspector's Target.</summary>
	private float time; 						/// <summary>simulation's time.</summary>

	/// <summary>Sets target property.</summary>
	private void OnEnable()
	{
		particleEffect = target as ParticleEffect;
		time = 1.0f;
	}

	/// <summary>OnInspectorGUI override.</summary>
	public override void OnInspectorGUI()
	{	
		DrawDefaultInspector();

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();

		time = EditorGUILayout.FloatField("Simulation Time: ", time);

		if(!particleEffect.isPlaying && GUILayout.Button("Simulate")) particleEffect.Simulate(true, time);
		if(!particleEffect.isPlaying && GUILayout.Button("Play")) particleEffect.Play();
		if(particleEffect.isPlaying && GUILayout.Button("Pause")) particleEffect.Pause();
		if(!particleEffect.isStopped && GUILayout.Button("Stop")) particleEffect.Stop();
		if(particleEffect.isPlaying && GUILayout.Button("Clear")) particleEffect.Clear();

		EditorUtility.SetDirty(particleEffect);
   		serializedObject.ApplyModifiedProperties();
	}
}
}