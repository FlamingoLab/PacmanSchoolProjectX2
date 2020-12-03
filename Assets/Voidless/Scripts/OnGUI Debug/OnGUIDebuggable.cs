using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voidless
{
public class OnGUIDebuggable : MonoBehaviour
{
	[TextArea(5, 30)] 
	[SerializeField] private string _debugText; 	/// <summary>Debug's Text.</summary>

	/// <summary>Gets and Sets debugText property.</summary>
	public string debugText
	{
		get { return _debugText; }
		set { _debugText = value; }
	}

	/// <summary>Callback invoked when OnGUIDebuggable's instance is enabled.</summary>
	private void OnEnable()
	{
		OnGUIDebugger.AddObject(this);
	}

	/// <summary>Callback invoked when OnGUIDebuggable's instance is disabled.</summary>
	private void OnDisable()
	{
		OnGUIDebugger.RemoveObject(this);
	}

	/// <returns>String representing this debug text.</returns>
	public override string ToString()
	{
		return debugText;
	}
}
}